// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using Engimatrix.ModelObjs;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

namespace Engimatrix.Models;

public class FilteringModel
{
    public static async Task<List<FilteredEmail>> GetFiltered(string start_date, string end_date, string categoryId, bool showOnlyNonResolved, string execute_user, List<string> filteredTokens)
    {
        /*
         *  SECTION 1 - Filtering email retrieval
         */

        Dictionary<string, string> dic = new Dictionary<string, string>();

        string query =
            // Filtered email info [0-7]
            "SELECT category.title, status.description, fe.date, fe.token, fe.confidence, fe.validated, fe.resolved_by, fe.resolved_at, " +
            // Original email info [8-12]
            "e.id, e.from, e.to, e.subject, e.date " +
            "FROM filtered_email fe JOIN category ON category.id = fe.category " +
            "JOIN status ON status.id = fe.status " +
            "JOIN email e ON e.id = fe.email " +
            // Adding a subquery to get the date of the last reply
            "LEFT JOIN (SELECT email_token, MAX(date) AS last_reply_date FROM reply WHERE is_read = 0 GROUP BY email_token) r ON fe.token = r.email_token " +
            "WHERE 1=1 ";

        if (!string.IsNullOrEmpty(start_date))
        {
            dic.Add("@start_date", start_date);
            query += "AND DATE(COALESCE(r.last_reply_date, e.date)) >= @start_date ";
        }
        if (!string.IsNullOrEmpty(end_date))
        {
            dic.Add("@end_date", end_date);
            query += "AND DATE(COALESCE(r.last_reply_date, e.date)) <= @end_date ";
        }

        if (!string.IsNullOrEmpty(categoryId))
        {
            dic.Add("@categoryId", categoryId);
            query += "AND fe.category = @categoryId ";
        }
        else
        {
            // category is empty -> filtering audit where it does not send any category
            // filter the duplicates out. Only show the duplicates when it is explicit by the categoryId that the user wants them

            dic.Add("@duplicatesCategoryId", CategoryConstants.CategoryCode.DUPLICADOS.ToString());
            dic.Add("@spamCategoryId", CategoryConstants.CategoryCode.SPAM.ToString());

            query += "AND fe.category NOT IN (@duplicatesCategoryId, @spamCategoryId) ";
        }

        if (showOnlyNonResolved)
        {
            dic.Add("@statusId", StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE.ToString());
            query += "AND fe.status != @statusId ";
        }

        if (filteredTokens != null && filteredTokens.Count > 0)
        {
            string tokens = string.Join(",", filteredTokens.Select(t => $"'{t}'"));
            query += $"AND fe.token IN ({tokens}) ";
        }

        // Ordering by the effective date (last reply date or email date)
        query += "ORDER BY COALESCE(r.last_reply_date, e.date) DESC";

        // TODO: This query takes 500ms minimum even when nothing is returned. Check how can we speed this up
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFilteredEmails");
        // No filtered email found
        if (response.out_data.Count <= 0)
        {
            return new List<FilteredEmail>();
        }

        /*
        *  SECTION 2 - Filtering Email informations retrieval
        */

        // Group previously gotten tokens to only fetch the replies associated with the filtered emails retrieved
        List<string> tokensRetrieved = response.out_data.Select(item => item["3"]).ToList();

        (Dictionary<string, engimatrix.ModelObjs.ReplyInfo> replyLookup,
         Dictionary<string, engimatrix.ModelObjs.ForwardInfo> forwardLookup) = await GetEmailActivityInfoAsync(tokensRetrieved, execute_user);

        /*
        *  SECTION 3 - Fill the filtered email list for the responde
        */

        List<FilteredEmail> filteredEmails = new List<FilteredEmail>();
        foreach (Dictionary<string, string> item in response.out_data)
        {
            // Email Info
            if (ConfigManager.isProduction)
            {
                try
                {
                    item["10"] = Cryptography.Decrypt(item["10"], item["9"]);
                    item["11"] = Cryptography.Decrypt(item["11"], item["9"]);
                }
                catch (Exception)
                {
                    // do nothing to not spam the logs
                }
            }
            EmailItem email = new(item["8"], item["9"], item["10"], item["11"], String.Empty, DateTime.Parse(item["12"]));

            string token = item["3"];

            // Retrieve reply information from the lookup dictionary
            engimatrix.ModelObjs.ReplyInfo replyData = replyLookup.TryGetValue(token, out engimatrix.ModelObjs.ReplyInfo reply) ? reply : null;
            int replyCount = replyData?.ReplyCount ?? 0;
            int unreadRepliesCount = replyData?.UnreadReplyCount ?? 0;
            DateTime repliedAt = replyData?.LastRepliedAt ?? DateTime.MinValue;
            string repliedBy = replyData?.LastRepliedBy ?? string.Empty;

            // Retrieve forward information from the lookup dictionary
            engimatrix.ModelObjs.ForwardInfo forwardData = forwardLookup.TryGetValue(token, out engimatrix.ModelObjs.ForwardInfo forward) ? forward : null;
            DateTime forwardedAt = forwardData?.LastForwardedAt ?? DateTime.MinValue;
            string forwardedBy = forwardData?.LastForwardedBy ?? string.Empty;

            DateTime resolvedAt = String.IsNullOrEmpty(item["7"]) ? DateTime.MinValue : DateTime.Parse(item["7"]);

            FilteredEmail filteredEmail = new(email, item["0"], item["1"], DateTime.Parse(item["2"]), token, item["4"], item["5"], replyCount, unreadRepliesCount, repliedBy, repliedAt, item["6"], resolvedAt, forwardedBy, forwardedAt);
            filteredEmails.Add(filteredEmail.toItem());
        }

        return filteredEmails;
    }

    public static async Task<List<FilteredEmailDTONew>> GetFilteredDtoNew(List<string> filteredTokens, string execute_user)
    {
        /*
         *  SECTION 1 - Filtering email retrieval
         */

        if (filteredTokens == null || filteredTokens.Count == 0)
        {
            return [];
        }

        Dictionary<string, string> dic = [];

        string query =
            // Filtered email info [0-7]
            "SELECT c.title AS c_title, c.id AS c_id, c.slug AS c_slug, s.description AS s_description, s.id AS s_id, " +
            "fe.date AS fe_date, fe.token AS fe_token, fe.confidence AS fe_confidence, fe.validated AS fe_validated, " +
            "fe.resolved_by AS fe_resolved_by, fe.resolved_at AS fe_resolved_at, " +
            // Original email info [8-12]
            "e.id AS e_id, e.from AS e_from, e.to AS e_to, e.subject AS e_subject, e.date AS e_date " +
            "FROM filtered_email fe " +
            "JOIN category c ON c.id = fe.category " +
            "JOIN status s ON s.id = fe.status " +
            "JOIN email e ON e.id = fe.email " +
            // Adding a subquery to get the date of the last reply
            "LEFT JOIN (SELECT email_token, MAX(date) AS last_reply_date FROM reply WHERE is_read = 0 GROUP BY email_token) r ON fe.token = r.email_token " +
            "WHERE 1=1 ";

        if (filteredTokens != null && filteredTokens.Count > 0)
        {
            string tokens = string.Join(",", filteredTokens.Select(t => $"'{t}'"));
            query += $"AND fe.token IN ({tokens}) ";
        }

        // Ordering by the effective date (last reply date or email date)
        query += "ORDER BY COALESCE(r.last_reply_date, e.date) DESC";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetFilteredEmails");
        // No filtered email found
        if (response.out_data.Count <= 0)
        {
            return [];
        }

        /*
        *  SECTION 2 - Filtering Email informations retrieval
        */

        // Group previously gotten tokens to only fetch the replies associated with the filtered emails retrieved
        List<string> tokensRetrieved = [.. response.out_data.Select(item => item["fe_token"])];

        (Dictionary<string, engimatrix.ModelObjs.ReplyInfo> replyLookup,
         Dictionary<string, engimatrix.ModelObjs.ForwardInfo> forwardLookup) = await GetEmailActivityInfoAsync(tokensRetrieved, execute_user);

        /*
        *  SECTION 3 - Fill the filtered email list for the responde
        */

        List<FilteredEmailDTONew> filteredEmails = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            // Email Info
            EmailItem email = new(item["e_id"], item["e_from"], item["e_to"], item["e_subject"], String.Empty, DateTime.Parse(item["e_date"]));
            email = EmailModel.DecryptEmailItem(email);

            string token = item["fe_token"];

            // Retrieve reply information from the lookup dictionary
            engimatrix.ModelObjs.ReplyInfo? replyData = replyLookup.TryGetValue(token, out engimatrix.ModelObjs.ReplyInfo? reply) ? reply : null;
            int replyCount = replyData?.ReplyCount ?? 0;
            int unreadRepliesCount = replyData?.UnreadReplyCount ?? 0;
            DateTime repliedAt = replyData?.LastRepliedAt ?? DateTime.MinValue;
            string repliedBy = replyData?.LastRepliedBy ?? string.Empty;

            // Retrieve forward information from the lookup dictionary
            engimatrix.ModelObjs.ForwardInfo? forwardData = forwardLookup.TryGetValue(token, out engimatrix.ModelObjs.ForwardInfo? forward) ? forward : null;
            DateTime forwardedAt = forwardData?.LastForwardedAt ?? DateTime.MinValue;
            string forwardedBy = forwardData?.LastForwardedBy ?? string.Empty;

            DateTime resolvedAt = String.IsNullOrEmpty(item["fe_resolved_at"]) ? DateTime.MinValue : DateTime.Parse(item["fe_resolved_at"]);

            CategoryItem category = new(Convert.ToInt32(item["c_id"]), item["c_title"], item["c_slug"]);
            StatusItem status = new(Convert.ToInt32(item["s_id"]), item["s_description"]);

            FilteredEmailDTONew filteredEmail = new FilteredEmailDTONewBuilder()
                .SetEmail(email)
                .SetCategory(category)
                .SetStatus(status)
                .SetDate(DateTime.Parse(item["fe_date"]))
                .SetToken(token)
                .SetConfidence(Convert.ToInt32(item["fe_confidence"]))
                .SetValidated(Convert.ToBoolean(item["fe_validated"]))
                .SetReplyCount(replyCount)
                .SetUnreadRepliesCount(unreadRepliesCount)
                .SetRepliedBy(repliedBy)
                .SetRepliedAt(repliedAt)
                .SetResolvedBy(item["fe_resolved_by"])
                .SetResolvedAt(resolvedAt)
                .SetForwardedBy(forwardedBy)
                .SetForwardedAt(forwardedAt)
                .Build();

            filteredEmails.Add(filteredEmail);
        }

        return filteredEmails;
    }

    public static async Task<(Dictionary<string, engimatrix.ModelObjs.ReplyInfo> ReplyLookup,
                           Dictionary<string, engimatrix.ModelObjs.ForwardInfo> ForwardLookup)>
    GetEmailActivityInfoAsync(List<string> filteredTokens, string executeUser)
    {
        string tokenList = string.Join(",", filteredTokens.Select(t => $"'{t}'"));

        // Build reply query string
        string replyQuery = "SELECT fe.token, " +
                "COUNT(r.id) AS reply_count, " +
                "SUM(CASE WHEN r.is_read = 0 THEN 1 ELSE 0 END) AS unread_reply_count, " +
                "MAX(r.date) AS last_replied_at, " +
                "MAX(r.replied_by) AS last_replied_by " +
                "FROM filtered_email fe " +
                "LEFT JOIN reply r ON r.email_token = fe.token " +
                $"WHERE fe.token IN ({tokenList}) " +
                "GROUP BY fe.token";

        // Build forward query string
        string forwardQuery = "SELECT fe.token, COUNT(f.id) AS forward_count, MAX(f.forwarded_at) AS last_forwarded_at, " +
                "MAX(f.forwarded_by) AS last_forwarded_by " +
                "FROM filtered_email fe " +
                "LEFT JOIN email_forward f ON f.email_token = fe.token " +
                $"WHERE fe.token IN ({tokenList}) " +
                "GROUP BY fe.token";

        // Execute the queries concurrently using Task.Run.
        Task<SqlExecuterItem> replyTask = Task.Run(() =>
            SqlExecuter.ExecuteFunction(replyQuery, [], executeUser, false, "GetRepliesAssociatedWithRetrievedFilteredEmails"));
        Task<SqlExecuterItem> forwardTask = Task.Run(() =>
            SqlExecuter.ExecuteFunction(forwardQuery, [], executeUser, false, "GetForwardsAssociatedWithRetrievedFilteredEmails"));

        // Await both tasks simultaneously.
        await Task.WhenAll(replyTask, forwardTask);

        SqlExecuterItem replyCountResponse = await replyTask;
        SqlExecuterItem forwardCountResponse = await forwardTask;

        // Build explicit dictionary for reply info.
        Dictionary<string, engimatrix.ModelObjs.ReplyInfo> replyLookup = replyCountResponse.out_data.ToDictionary(
            r => r["token"],
            r => new engimatrix.ModelObjs.ReplyInfo(
                int.Parse(r["reply_count"]),
                int.Parse(r["unread_reply_count"]),
                string.IsNullOrEmpty(r["last_replied_at"]) ? DateTime.MinValue : DateTime.Parse(r["last_replied_at"]),
                r["last_replied_by"]
            )
        );

        // Build explicit dictionary for forward info.
        Dictionary<string, engimatrix.ModelObjs.ForwardInfo> forwardLookup = forwardCountResponse.out_data.ToDictionary(
            f => f["token"],
            f => new engimatrix.ModelObjs.ForwardInfo(
                int.Parse(f["forward_count"]),
                string.IsNullOrEmpty(f["last_forwarded_at"]) ? DateTime.MinValue : DateTime.Parse(f["last_forwarded_at"]),
                f["last_forwarded_by"]
            )
        );

        return (replyLookup, forwardLookup);
    }

    public static FilteredEmail SaveFiltered(FilteredEmail filteredEmail) => SaveFiltered(filteredEmail, string.Empty);

    public static FilteredEmail SaveFiltered(FilteredEmail filteredEmail, string execute_user)
    {
        // Fetch if there is already a filtered email with the given token
        /*
            A UUID/GUID is a 128-bit value
            Total possible unique values: 2^128 (approximately 3.4 x 10^38)
            That's 340 undecillion unique values
            So, the chance of a collision is extremely low, so we can assume that this is a duplicate email being saved
        */
        if (EmailTokenExists(filteredEmail.token, execute_user))
        {
            throw new ItemAlreadyExistsException($"Filtered email already exists with the given token {filteredEmail.token}");
        }

        Dictionary<string, string> dic = [];

        int emailId = EmailModel.SaveEmail(filteredEmail.email, "System");

        filteredEmail.email.id = emailId.ToString();
        string token = filteredEmail.token;
        string category = filteredEmail.category;
        string dateFiltered = DateTime.Parse(filteredEmail.date.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
        dic.Add("token", token);
        dic.Add("email", filteredEmail.email.id);
        dic.Add("category", category);
        dic.Add("status", filteredEmail.status);
        dic.Add("date", dateFiltered);
        dic.Add("confidence", filteredEmail.confidence);
        dic.Add("validated", filteredEmail.validated);

        string query = "INSERT INTO filtered_email (email, status, category, date, token, confidence, validated) " +
            " VALUES (@email, @status, @category, @date, @token, @confidence, @validated)";

        SqlExecuter.ExecFunction(query, dic, execute_user, true, "InsertFilteredEmailRecord");
        return filteredEmail;
    }

    public static List<FilteredEmail> getToValidate(string execute_user, int statusId, int categoryId, DateOnly? startDate, DateOnly? endDate)
    {
        Dictionary<string, string> dic = [];

        string query = // Filtered email info [0-5]
            "SELECT category.title, status.description, fe.date, fe.token, fe.confidence, fe.validated, " +
            // Original email info [6-10]
            "e.id, e.from, e.to, e.subject, e.date " +
            "FROM filtered_email fe JOIN category ON category.id = fe.category " +
            "JOIN status ON status.id = fe.status " +
            "JOIN email e ON e.id = fe.email " +
            "WHERE validated = 0 ";

        if (statusId != 0)
        {
            // For the Error status, we also want to show the Other category
            dic.Add("statusId", statusId.ToString());
            query += "AND status = @statusId ";
        }
        if (categoryId != 0)
        {
            dic.Add("categoryId", categoryId.ToString());
            query += "AND category = @categoryId ";
        }

        if (startDate.HasValue)

        {
            dic.Add("startDate", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND fe.date >= @startDate ";
        }
        if (endDate.HasValue)
        {
            dic.Add("endDate", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND fe.date <= @endDate ";
        }

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFilteredEmails");

        List<FilteredEmail> filteredEmails = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            EmailItem email = new(item["6"], item["7"], item["8"], item["9"], String.Empty, DateTime.Parse(item["10"]));

            FilteredEmail filteredEmail = new(email, item["0"], item["1"], DateTime.Parse(item["2"]), item["3"], item["4"], item["5"]);

            if (ConfigManager.isProduction)
            {
                filteredEmail.email.to = Cryptography.Decrypt(filteredEmail.email.to, filteredEmail.email.from);
                filteredEmail.email.subject = Cryptography.Decrypt(filteredEmail.email.subject, filteredEmail.email.from);
            }

            filteredEmails.Add(filteredEmail);
        }

        return filteredEmails;
    }

    public static List<FilteredEmail> GetToValidateOrders(string execute_user, int categoryId, DateOnly? startDate, DateOnly? endDate)
    {
        Dictionary<string, string> dic = [];

        string query = // Filtered email info [0-5]
            "SELECT category.title, status.description, fe.date, fe.token, fe.confidence, fe.validated, " +
            // Original email info [6-10]
            "e.id, e.from, e.to, e.subject, e.date " +
            "FROM filtered_email fe JOIN category ON category.id = fe.category " +
            "JOIN status ON status.id = fe.status " +
            "JOIN email e ON e.id = fe.email " +
            "WHERE validated = 0 ";

        if (categoryId != 0)
        {
            dic.Add("categoryId", categoryId.ToString());
            query += "AND category = @categoryId ";
        }

        if (startDate.HasValue)
        {
            dic.Add("startDate", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND fe.date >= @startDate ";
        }

        if (endDate.HasValue)

        {
            dic.Add("endDate", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND fe.date <= @endDate ";
        }

        // the status can be the following:
        // Aguarda_Validacao, Pendente_Administracao, Erro, Aprovado_Direcao_Comercial
        dic.Add("@StatusAguardaValidacao", StatusConstants.StatusCode.AGUARDA_VALIDACAO.ToString());
        query += "AND (status = @StatusAguardaValidacao ";

        dic.Add("@StatusPendenteAdministracao", StatusConstants.StatusCode.PENDENTE_APROVACAO_ADMINISTRACAO.ToString());
        query += "OR status = @StatusPendenteAdministracao ";

        dic.Add("@StatusErro", StatusConstants.StatusCode.ERRO.ToString());
        query += "OR status = @StatusErro ";


        dic.Add("@StatusAprovadoDirecaoComercial", StatusConstants.StatusCode.APROVADO_DIRECAO_COMERCIAL.ToString());
        query += "OR status = @StatusAprovadoDirecaoComercial) ";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFilteredEmails");

        List<FilteredEmail> filteredEmails = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            EmailItem email = new(item["6"], item["7"], item["8"], item["9"], String.Empty, DateTime.Parse(item["10"]));

            FilteredEmail filteredEmail = new(email, item["0"], item["1"], DateTime.Parse(item["2"]), item["3"], item["4"], item["5"]);

            if (ConfigManager.isProduction)
            {
                filteredEmail.email.to = Cryptography.Decrypt(filteredEmail.email.to, filteredEmail.email.from);
                filteredEmail.email.subject = Cryptography.Decrypt(filteredEmail.email.subject, filteredEmail.email.from);
            }

            filteredEmails.Add(filteredEmail);
        }

        return filteredEmails;
    }

    public static List<FilteredEmail> GetToValidatePendingClient(string execute_user, DateOnly? startDate, DateOnly? endDate)
    {
        Dictionary<string, string> dic = [];
        // The filtering emails pending from the client must only be quotations. The orders are instantly created, so no need for the client to verify.
        // In the quotation, they must be with is_adjudicated = 0 (still hasnt accepted) and filtered.status = confirmed_by_operator

        string query = // Filtered email info [0-5]
            "SELECT category.title, status.description, fe.date, fe.token, fe.confidence, fe.validated, " +
            // Original email info [6-10]
            "e.id, e.from, e.to, e.subject, e.date " +
            "FROM filtered_email fe JOIN category ON category.id = fe.category " +
            "JOIN status ON status.id = fe.status " +
            "JOIN email e ON e.id = fe.email " +
            "JOIN `order` o ON o.email_token = fe.token " +
            "WHERE 1=1 ";

        if (startDate.HasValue)
        {
            dic.Add("startDate", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND fe.date >= @startDate ";
        }

        if (endDate.HasValue)
        {
            dic.Add("endDate", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND fe.date <= @endDate ";
        }

        // the status can be the following:
        // Aguarda_Validacao, Pendente_Administracao, Erro, Aprovado_Direcao_Comercial
        dic.Add("@Status", StatusConstants.StatusCode.PENDENTE_CONFIRMACAO_CLIENTE.ToString());
        query += "AND status = @Status AND o.is_adjudicated = 0 ";
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFilteredEmails");


        List<FilteredEmail> filteredEmails = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            EmailItem email = new(item["6"], item["7"], item["8"], item["9"], String.Empty, DateTime.Parse(item["10"]));

            FilteredEmail filteredEmail = new(email, item["0"], item["1"], DateTime.Parse(item["2"]), item["3"], item["4"], item["5"]);

            if (ConfigManager.isProduction)
            {
                filteredEmail.email.to = Cryptography.Decrypt(filteredEmail.email.to, filteredEmail.email.from);
                filteredEmail.email.subject = Cryptography.Decrypt(filteredEmail.email.subject, filteredEmail.email.from);
            }

            filteredEmails.Add(filteredEmail);
        }

        return filteredEmails;
    }

    public static List<FilteredEmail> GetRequestsToValidateAllDetails(string execute_user, int statusId)
    {
        Dictionary<string, string> dic = [];
        // string with cotaçoes and encomendas categories ids
        string categories = $"{CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS}, {CategoryConstants.CategoryCode.ENCOMENDAS}";

        string query =
            "SELECT fe.category, fe.status, fe.date, fe.token, fe.confidence, fe.validated, " +
            "e.id, e.from, e.to, e.subject, e.date AS email_date, e.body " +
            "FROM filtered_email fe " +
            "JOIN email e ON e.id = fe.email " +
            "WHERE validated = 0 " +
            $"AND category IN ({categories})";

        if (statusId != 0)
        {
            dic.Add("statusId", statusId.ToString());
            query += "AND status = @statusId ";
        }

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetFilteredEmails");

        List<FilteredEmail> filteredEmails = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            EmailItem email = new(item["id"], item["from"], item["to"], item["subject"], item["body"], DateTime.Parse(item["email_date"]));
            email = EmailModel.DecryptEmailItem(email);

            FilteredEmail filteredEmail = new(email, item["category"], item["status"], DateTime.Parse(item["date"]), item["token"], item["confidence"], item["validated"]);

            filteredEmails.Add(filteredEmail);
        }

        return filteredEmails;
    }

    // get requests confirmed by operator or client
    public static List<FilteredEmail> GetFilteredEmailsConfirmed(string execute_user)
    {
        Dictionary<string, string> dic = [];
        string statusList = $"{StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE}, {StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR}";

        string query =
            "SELECT fe.category, fe.status, fe.date, fe.token, fe.confidence, fe.validated, " +
            "e.id, e.from, e.to, e.subject, e.date AS email_date, e.body " +
            "FROM filtered_email fe " +
            "JOIN email e ON e.id = fe.email " +
            "WHERE validated = 1 " +
            $"AND status IN ({statusList})";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetFilteredEmails");

        if (!response.operationResult)
        {
            throw new Exception("Error fetching filtered emails");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<FilteredEmail> filteredEmails = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            EmailItem email = new(item["id"], item["from"], item["to"], item["subject"], item["body"], DateTime.Parse(item["email_date"]));

            FilteredEmail filteredEmail = new(email, item["category"], item["status"], DateTime.Parse(item["date"]), item["token"], item["confidence"], item["validated"]);

            if (ConfigManager.isProduction)
            {
                filteredEmail.email.to = Cryptography.Decrypt(filteredEmail.email.to, filteredEmail.email.from);
                filteredEmail.email.subject = Cryptography.Decrypt(filteredEmail.email.subject, filteredEmail.email.from);
            }

            filteredEmails.Add(filteredEmail);
        }

        return filteredEmails;
    }

    public static FilteredEmail getFilteredEmail(string execute_user, string emailIdOrToken, bool isToken)
    {
        Dictionary<string, string> dic = [];

        string query = "SELECT f.email, status.description as status, category.title,  " +
                                "f.date, f.token, f.confidence, f.validated, f.resolved_by, f.resolved_at, " +
                                "e.id, e.from, e.to, e.cc, e.bcc, e.subject, e.body, e.date " +
                        "FROM filtered_email f " +
                                "JOIN category ON category.id = category " +
                                "JOIN status ON status = status.id " +
                                "JOIN email e ON e.id = f.email ";

        if (isToken)
        {
            dic.Add("emailToken", emailIdOrToken);
            query += "WHERE token=@emailToken";
        }
        else
        {
            dic.Add("emailId", emailIdOrToken);
            query += "WHERE email=@emailId";
        }

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFilteredEmailToValidate");

        // Check if it returned anything, act acordingly
        if (!response.operationResult || response.out_data.Count == 0)
        {
            Log.Debug("Error fetching email to validate with token/id " + emailIdOrToken);
            return new FilteredEmail();
        }

        Dictionary<string, string> item = response.out_data[0];

        EmailItem email = new EmailItem(item["9"], item["10"], item["11"], item["12"], item["13"], item["14"], item["15"], DateTime.Parse(item["16"]));
        email = EmailModel.DecryptEmailItem(email);

        DateTime resolvedAt = String.IsNullOrEmpty(item["8"]) ? DateTime.MinValue : DateTime.Parse(item["8"]);

        FilteredEmail filteredEmail = new FilteredEmail(email, item["2"], item["1"], DateTime.Parse(item["3"]), item["4"], item["5"], item["6"], item["7"], resolvedAt);

        return filteredEmail;
    }

    public static FilteredEmailDTO getFilteredEmailDTO(string execute_user, string emailId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        string query = "SELECT f.email, status.description as status, category.title,  " +
                                "f.date, f.token, f.confidence, f.validated, f.resolved_by, f.resolved_at, " +
                                "e.id, e.from, e.to, e.cc, e.bcc, e.subject, e.body, e.date " +
                        "FROM filtered_email f " +
                                "JOIN category ON category.id = category " +
                                "JOIN status ON status = status.id " +
                                "JOIN email e ON e.id = f.email ";

        dic.Add("emailId", emailId);
        query += "WHERE email=@emailId";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFilteredEmailToValidate");

        // Check if it returned anything, act acordingly
        if (!response.operationResult || response.out_data.Count == 0)
        {
            Log.Debug("Error fetching email to validate with token/id " + emailId);
            return new();
        }

        Dictionary<string, string> item = response.out_data[0];

        EmailItem email = new EmailItem(item["9"], item["10"], item["11"], item["12"], item["13"], item["14"], item["15"], DateTime.Parse(item["16"]));
        email = EmailModel.DecryptEmailItem(email);

        DateTime resolvedAt = String.IsNullOrEmpty(item["8"]) ? DateTime.MinValue : DateTime.Parse(item["8"]);

        FilteredEmailDTO filteredEmail = new FilteredEmailDTOBuilder()
            .SetEmail(email)
            .SetCategory(item["2"])
            .SetStatus(item["1"])
            .SetDate(DateTime.Parse(item["3"]))
            .SetToken(item["4"])
            .SetConfidence(item["5"])
            .SetValidated(item["6"])
            .SetResolvedBy(item["7"])
            .SetResolvedAt(resolvedAt)
            .Build();

        return filteredEmail;
    }

    public static string getFilteredCategoryFromEmailToken(string emailToken)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("token", emailToken);
        SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT category FROM filtered_email WHERE token = @token", dic, "", false, "GetEmailIdFromFilteredToken");

        if (response.out_data.Count == 0)
        {
            return String.Empty;
        }

        return response.out_data[0]["0"];
    }

    public static int? GetFilteredStatusFromEmailToken(string emailToken, string executeUser)
    {
        Dictionary<string, string> dic = new()
            {
                { "token", emailToken }
            };

        string query = "SELECT status FROM filtered_email WHERE token = @token";
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, executeUser, false, "GetEmailIdFromFilteredToken");

        if (response.out_data.Count == 0)
        {
            return null;
        }

        return int.Parse(response.out_data[0]["0"]);
    }

    public static List<CategoriesItem> getCategories(string execute_user)
    {
        SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT * FROM category;", [], execute_user, false, "GetCategories");

        List<CategoriesItem> categories = new List<CategoriesItem>();

        foreach (Dictionary<string, string> item in response.out_data)
        {
            CategoriesItem category = new CategoriesItem(int.Parse(item["0"]), item["1"], item["2"]);

            categories.Add(category.ToItem());
        }
        return categories;
    }

    public static async Task updateFilteredEmail(string execute_user, string emailId, int categoryId)
    {
        int status = StatusConstants.StatusCode.TRIAGEM_REALIZADA;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("emailId", emailId);
        dic.Add("categoryId", categoryId.ToString());
        dic.Add("status", status.ToString());

        string token = SqlExecuter.ExecFunction("SELECT token FROM filtered_email WHERE email=@emailId", dic, execute_user, false, "GetFiltratedEmailToken").out_data[0]["0"];
        SqlExecuter.ExecFunction("UPDATE filtered_email SET category = @categoryId, status = @status, validated = true WHERE email = @emailId", dic, execute_user, true, "UpdateFilteredEmail");

        string toMoveFolder = EmailCategorizer.GetFolderByCategoryId(categoryId);

        await MasterFerro.MoveEmailByTokenAsync(ConfigManager.ValidateFolder, toMoveFolder, token, ConfigManager.MailboxAccount[0]);
    }

    public static async Task ChangeFilteredEmailCategory(string execute_user, string emailIdOrToken, bool isToken, int categoryId)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@emailIdOrToken", emailIdOrToken);

        // Get the email token and the email inbox to move the email to
        string query = "SELECT f.token, e.to, f.category, e.from " +
            "FROM filtered_email f " +
            "JOIN email e ON f.email = e.id ";

        if (isToken)
        {
            query += "WHERE f.token=@emailIdOrToken";
        }
        else
        {
            query += "WHERE f.email=@emailIdOrToken";
        }
        SqlExecuterItem sqlData = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetFiltratedEmailToken");

        if (sqlData.out_data.Count <= 0 || !sqlData.operationResult)
        {
            Log.Debug($"ChangeFilteredEmailCategory error fetching email with id {emailIdOrToken}");
            return;
        }

        string token = sqlData.out_data[0]["0"];
        string mailbox = sqlData.out_data[0]["1"];
        int previousCategoryId = int.Parse(sqlData.out_data[0]["2"]);
        dic.Add("categoryId", categoryId.ToString());
        dic.Add("statusId", StatusConstants.StatusCode.TRIAGEM_REALIZADA.ToString());

        query = "UPDATE filtered_email SET category = @categoryId, status = @statusId, validated = true ";

        if (isToken)
        {
            query += "WHERE token=@emailIdOrToken";
        }
        else
        {
            query += "WHERE email = @emailIdOrToken";
        }

        SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateFilteredEmail");

        // Get the folders to move from and to
        string previousCategoryName = CategoryConstants.GetCategoryNameByCode(previousCategoryId);
        (string previousFolder, _) = EmailCategorizer.GetFolderNameAndCategoryConstantFromCategory(previousCategoryName, "100");
        string toMoveFolder = EmailCategorizer.GetFolderByCategoryId(categoryId);

        if (ConfigManager.isProduction)
        {
            string keyTo = sqlData.out_data[0]["3"];
            mailbox = Cryptography.Decrypt(mailbox, keyTo);
        }

        await MasterFerro.MoveEmailByTokenAsync(previousFolder, toMoveFolder, token, mailbox);
    }

    public static async Task MarkEmailAsSpam(string emailToken, string execute_user)
    {
        await ChangeFilteredEmailCategory(execute_user, emailToken, true, CategoryConstants.CategoryCode.SPAM);
    }

    public static void ChangeEmailStatus(string emailToken, string status, string executeUser) => ChangeEmailStatus(emailToken, status, executeUser, true);

    public static void ChangeEmailStatus(string emailToken, string status, string execute_user, bool markValidated)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("emailToken", emailToken);
        dic.Add("statusId", status);

        string query = "UPDATE filtered_email SET status = @statusId";

        if (markValidated)
        {
            dic.Add("validated", "1");
            query += ", validated = @validated";
        }

        // If its to resolve, add the audit (who and when) to the database
        if (status == StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE.ToString())
        {
            dic.Add("resolvedBy", execute_user);
            dic.Add("resolvedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // format to match SQL datetime format

            query += ", resolved_by = @resolvedBy, resolved_at = @resolvedAt";
        }

        query += " WHERE token = @emailToken";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateFilteredEmailStatus");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred on the SQL operation");
        }

        Log.Debug($"ChangeEmailStatus: Changed email status to {status} for token {emailToken}");
    }

    public static void ChangeEmailStatusUnvalidated(string emailToken, string status, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("emailToken", emailToken);
        dic.Add("statusId", status);

        string query = "UPDATE filtered_email SET status = @statusId, validated = 0";

        // If its to resolve, add the audit (who and when) to the database
        if (status == StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE.ToString())
        {
            dic.Add("resolvedBy", execute_user);
            dic.Add("resolvedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // format to match SQL datetime format

            query += ", resolved_by = @resolvedBy, resolved_at = @resolvedAt";
        }

        query += " WHERE token = @emailToken";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateFilteredEmailStatus");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred on the SQL operation");
        }
    }

    // check if order was already checked by either operator or client
    public static bool CheckOrderIsChecked(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = new()
            {
                { "@OrderToken", orderToken }
            };

        string query = $"SELECT COUNT(*) AS count " +
            $"FROM `filtered_email` f " +
            "JOIN `order` o ON o.email_token = f.token " +
            $"WHERE f.token = @OrderToken " +
            $"AND f.status != {StatusConstants.StatusCode.AGUARDA_VALIDACAO}";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "CheckOrderIsChecked");
        if (!response.operationResult)
        {
            throw new Exception("An error occurred on the SQL operation of checking if order is checked");
        }
        int count = int.Parse(response.out_data[0]["count"]);

        query = "SELECT is_adjudicated FROM `order` WHERE token = @OrderToken";

        response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "CheckOrderIsChecked");
        if (!response.operationResult)
        {
            throw new Exception("An error occurred on the SQL operation of checking if order is adjudicated");
        }
        bool isAdjudicated = Convert.ToBoolean(response.out_data[0]["is_adjudicated"]);

        Log.Debug($"CheckOrderIsChecked: Order with token {orderToken} has been checked has a count of {count} and is adjudicated {isAdjudicated}");

        return count > 0 || isAdjudicated;
    }

    public static async Task<FilteringStatisticsItem> getStatistics(string execute_user, string start_date, string end_date, int? categoryId)
    {
        Dictionary<string, string> dic = new()
            {
                { "@start_date", start_date },
                { "@end_date", end_date },
                { "@categoryId", categoryId.ToString() }
            };

        string filteredQuery = BuildFilteringStatisticsQuery(start_date, end_date, categoryId);
        string replyQuery = BuildReplyStatisticsQuery(start_date, end_date, categoryId);
        string filteredTotalQuery = BuilderFilteringTotalQuery(start_date, end_date);

        Task<SqlExecuterItem> filteredTask = Task.Run(() => SqlExecuter.ExecuteFunction(filteredQuery, dic, execute_user, false, "GetStatistics"));
        Task<SqlExecuterItem> replyTask = Task.Run(() => SqlExecuter.ExecuteFunction(replyQuery, dic, execute_user, false, "GetStatistics"));
        Task<SqlExecuterItem> totalTask = Task.Run(() => SqlExecuter.ExecuteFunction(filteredTotalQuery, dic, execute_user, false, "GetStatistics"));

        await Task.WhenAll(filteredTask, replyTask, totalTask);

        if (!filteredTask.Result.operationResult || filteredTask.Result.out_data.Count == 0 || !replyTask.Result.operationResult || replyTask.Result.out_data.Count == 0)
        {
            return new FilteringStatisticsItem();
        }

        SqlExecuterItem filteredResponse = await filteredTask;
        SqlExecuterItem replyResponse = await replyTask;
        SqlExecuterItem filteredTotalResponse = await totalTask;

        Dictionary<string, string> filteredItem = filteredResponse.out_data[0];
        Dictionary<string, string> replyItem = replyResponse.out_data[0];
        Dictionary<string, string> totalItem = filteredTotalResponse.out_data[0];

        double avgConfidence = string.IsNullOrEmpty(filteredItem["avgConfidence"]) ? 0 : Math.Round(double.Parse(filteredItem["avgConfidence"]), 2);

        FilteringStatisticsItem statistics = new FilteringStatisticsItemBuilder()
            .SetToValidate(int.Parse(filteredItem["toValidate"]))
            .SetAvgConfidence(avgConfidence)
            .SetLowConfidence(int.Parse(filteredItem["lowConfidence"]))
            .SetError(int.Parse(filteredItem["error"]))
            .SetOrders(int.Parse(filteredItem["orders"]))
            .SetQuotations(int.Parse(filteredItem["quotations"]))
            .SetReceipts(int.Parse(filteredItem["receipts"]))
            .SetOthers(int.Parse(filteredItem["others"]))
            .SetErrors(int.Parse(filteredItem["errors"]))
            .SetDuplicates(int.Parse(filteredItem["duplicates"]))
            .SetCertificates(int.Parse(filteredItem["certificates"]))
            .SetTotal(int.Parse(filteredItem["total"]))
            .SetAutomatic(int.Parse(filteredItem["automatic"]))
            .SetManual(int.Parse(filteredItem["manual"]))
            .SetResolved(int.Parse(filteredItem["numero_de_resolvidos"]))
            .SetUnresolved(int.Parse(filteredItem["numero_de_resolvidos_nao"]))
            .SetUnresolvedQuotations(int.Parse(filteredItem["numero_de_resolvidos_nao_quotations"]))
            .SetTotalRepliesMasterferro(int.Parse(replyItem["employee_replies"]))
            .SetTotalRepliesClient(int.Parse(replyItem["client_replies"]))
            .SetTotalOnlyDates(int.Parse(totalItem["total_only_dates"]))
            .Build();

        return statistics;
    }

    private static string BuildFilteringStatisticsQuery(string startDate, string endDate, int? categoryId)
    {
        string filteredQuery = $"SELECT COUNT(CASE WHEN status = {StatusConstants.StatusCode.AGUARDA_VALIDACAO} THEN 1 END) AS toValidate, " +
                "(SELECT AVG(confidence) " +
                    "FROM filtered_email " +
                    "WHERE confidence > 0 {category} {start_date} {end_date}) AS avgConfidence, " +
                "COUNT(CASE WHEN confidence < 50 {category} THEN 1 END) AS lowConfidence, " +
                "(SELECT COUNT(*) " +
                     "FROM filtered_email " +
                     "WHERE confidence = 0 {category} {start_date} {end_date}) AS error, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.ENCOMENDAS} THEN 1 END) AS orders, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS} THEN 1 END) AS quotations, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.COMPROVATIVOS_PAGAMENTO} THEN 1 END) AS receipts, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.OUTROS} THEN 1 END) AS others, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.ERRO} THEN 1 END) AS errors, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.DUPLICADOS} THEN 1 END) AS duplicates, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.CERTIFICADOS_QUALIDADE} THEN 1 END) AS certificates, " +
                $"COUNT(CASE WHEN category = {CategoryConstants.CategoryCode.SPAM} THEN 1 END) AS spams, " +
                "COUNT(*) AS total, " +
                "COUNT(CASE WHEN resolved_by IS NOT NULL {category} THEN 1 END) AS numero_de_resolvidos, " +
                "COUNT(CASE WHEN resolved_by IS NULL {category} THEN 1 END) AS numero_de_resolvidos_nao, " +
                $"COUNT(CASE WHEN resolved_by IS NULL AND category = {CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS} THEN 1 END) AS numero_de_resolvidos_nao_quotations, " +
                $"COUNT(CASE WHEN category != {CategoryConstants.CategoryCode.ERRO} OR confidence > 0 THEN 1 END) AS automatic, " +
                $"COUNT(CASE WHEN status = {StatusConstants.StatusCode.AGUARDA_VALIDACAO} OR category = {CategoryConstants.CategoryCode.ERRO} OR confidence = 0 THEN 1 END) AS manual " +
            "FROM filtered_email " +
            "WHERE 1=1 {category} {start_date} {end_date}";

        return ReplaceQueryPlaceholders(filteredQuery, startDate, endDate, categoryId, "filtered_email");
    }

    private static string BuilderFilteringTotalQuery(string startDate, string endDate)
    {
        string filteredQuery = "SELECT COUNT(*) AS total_only_dates " +
            "FROM filtered_email " +
            "WHERE 1=1 {start_date} {end_date}";

        return ReplaceQueryPlaceholders(filteredQuery, startDate, endDate, 0, "filtered_email");
    }

    private static string BuildReplyStatisticsQuery(string startDate, string endDate, int? categoryId)
    {
        string replyQuery = "SELECT " +
            // The reply table has all client and employee replies, the difference is the the replies from the employees have the
            // replied_by field filled in
            "COUNT(CASE WHEN r.replied_by IS NOT NULL OR r.replied_by != '' THEN 1 END) AS employee_replies, " +
            "COUNT(CASE WHEN r.replied_by IS NULL OR r.replied_by = '' THEN 1 END) AS client_replies " +
            "FROM reply r " +
                "JOIN filtered_email f ON f.token = r.email_token " +
            "WHERE 1=1 {category} {start_date} {end_date}";

        return ReplaceQueryPlaceholders(replyQuery, startDate, endDate, categoryId, "f");
    }

    private static string ReplaceQueryPlaceholders(string query, string startDate, string endDate, int? categoryId, string alias)
    {
        string categoryCondition = categoryId.HasValue && categoryId != 0
            ? $"AND {alias}.category = @categoryId"
            : $"AND {alias}.category != {CategoryConstants.CategoryCode.DUPLICADOS}";

        return query
            .Replace("{start_date}", string.IsNullOrEmpty(startDate) ? string.Empty : $"AND DATE({alias}.date) >= @start_date")
            .Replace("{end_date}", string.IsNullOrEmpty(endDate) ? string.Empty : $"AND DATE({alias}.date) <= @end_date")
            .Replace("{category}", categoryCondition);
    }

    public static string GetFilteredTokenByEmailId(string emailId, string execute_user)
    {
        Dictionary<string, string> dic = new()
            {
                { "emailId", emailId }
            };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction("SELECT token FROM filtered_email WHERE email = @emailId", dic, execute_user, false, "GetFilteredTokenByEmailId");

        if (response.out_data.Count == 0)
        {
            return String.Empty;
        }

        return response.out_data[0]["token"];
    }

    public static bool EmailTokenExists(string emailToken, string executeUser)
    {
        string query = "SELECT COUNT(token) AS count FROM filtered_email WHERE token = @emailToken";

        Dictionary<string, string> dic = new()
            {
                { "emailToken", emailToken }
            };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "EmailTokenExists");

        if (!response.operationResult)
        {
            throw new DatabaseException("An error occurred on the SQL operation");
        }

        if (response.out_data.Count == 0)
        {
            return false;
        }

        return int.Parse(response.out_data[0]["count"]) > 0;
    }

    public static void SetEmailAsAdministrationPending(string filteredToken, string executer_user)
    {
        string status = StatusConstants.StatusCode.PENDENTE_APROVACAO_ADMINISTRACAO.ToString();
        ChangeEmailStatus(filteredToken, status, executer_user, false);
    }

    public static bool IsFilteredEmailAlreadyConfirmed(string filteredToken, string executer_user)
    {
        int? filteredStatus = GetFilteredStatusFromEmailToken(filteredToken, executer_user);
        if (!filteredStatus.HasValue)
        {
            throw new ResourceEmptyException($"The filtered email with token {filteredToken} does not exist");
        }

        List<int> possibleStatuses = [
            StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE,
                StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR
        ];

        if (possibleStatuses.Contains(filteredStatus.Value))
        {
            return true;
        }

        return false;
    }

    public async static Task GenerateAuditEmail(string emailBody, string executeUser)
    {
        // this will call the master ferro process to filter the email
        string account = ConfigManager.MailboxAccount[0];
        using ImapClient client = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);
        IMailFolder folder = client.GetFolder(ConfigManager.InboxFolder);

        string signature = SignatureModel.GetDefaultFormattedSignature(executeUser);
        emailBody = emailBody + "<br><br><br><br>" + signature;

        // create the MimeMessage 
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress(ConfigManager.MailboxAccount[0], ConfigManager.MailboxAccount[0]));
        message.To.Add(new MailboxAddress(ConfigManager.MailboxAccount[0], ConfigManager.MailboxAccount[0]));
        message.Subject = "Audit Email";
        message.Body = new TextPart("html")
        {
            Text = emailBody
        };

        string token = Guid.NewGuid().ToString();

        try
        {
            OpenAiEmailCategorization? categorization = await OpenAI.ProcessMoveEmailCategoryOpenAI(message);
            if (categorization is null)
            {
                Log.Debug("Open AI response came null, creating default categorization...");
                categorization = new OpenAiEmailCategorizationBuilder().SetConfianca("0").SetCategoria("").Build();
            }
            Log.Debug("Categoria: " + categorization.categoria + " / Confiança: " + categorization.confianca);

            // In case the OpenAI gives an error, we need to send to the error folder. By giving the confidence of 0, the next function will get the Error folder
            if (string.IsNullOrEmpty(categorization.confianca)) { categorization.confianca = "0"; }

            (string folderToSave, int categoryToSave) = EmailCategorizer.GetFolderNameAndCategoryConstantFromCategory(categorization.categoria, categorization.confianca);
            int status = MasterFerro.GetStatusFromCategoryAndFolder(categoryToSave, folderToSave);
            // Check if the email is a forward and update the sender if necessary

            FilteredEmail filteredEmail = MasterFerro.CreateFilteredEmail(message, account, categoryToSave.ToString(), status.ToString(), token, categorization.confianca, "0");
            MasterFerro.SaveFilteredEmailAndAttachments(filteredEmail, message);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error categorizing email with   subject: {message.Subject}, from: {message.From} with message: {ex}");
        }
    }
}
