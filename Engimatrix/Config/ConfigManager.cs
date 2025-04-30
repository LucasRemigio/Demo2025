// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;
using MySqlConnector;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.Models;
using engimatrix.Utils;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Microsoft.AspNetCore.Http;
using Engimatrix.ModelObjs;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using Engimatrix.Models;

namespace engimatrix.Config
{
    public static class ConfigManager
    {
        public static List<string> MailboxAccount = new List<string>();
        public static List<string> MailboxPassword = new List<string>();
        public static Dictionary<string, string> MailboxCredentials = new Dictionary<string, string>();

        public static string InboxFolder = string.Empty;
        public static string OrdersFolder = string.Empty;
        public static string QuotationsFolder = string.Empty;
        public static string ValidateFolder = string.Empty;
        public static string ReceiptsFolder = string.Empty;
        public static string OthersFolder = string.Empty;
        public static string SentFolder = string.Empty;
        public static string DuplicatesFolder = string.Empty;
        public static string TestFolder = string.Empty;
        public static string TrashFolder = string.Empty;
        public static string CertificatesFolder = string.Empty;
        public static string SpamFolder = string.Empty;

        public static string SystemEmail = string.Empty;
        public static string SystemPassword = string.Empty;
        public static string EmailServer = string.Empty;
        public static string EmailPassword = string.Empty;
        public static string IMAPPort = string.Empty;
        public static string SMTPPort = string.Empty;

        public static string engimatrixInternalApiKey = string.Empty;
        public static string postalCodesPath = string.Empty;

        public static string GoogleMapsApiKey = string.Empty;
        public static string HereApiKey = string.Empty;
        public static string HereAppKeyId = string.Empty;
        public static string HereAppKeySecret = string.Empty;
        public static HereAuthItem? HereAuth = null;
        public static decimal MFLat = 0;
        public static decimal MFLng = 0;

        public static string MasterferroAddress = "Av. Barros e Soares, 531";
        public static string MasterferroCity = "Nogueira";
        public static string MasterferroPostalCode = "4715-213";
        public static string MasterferroPostalCodeLocality = "Braga";
        public static string MasterferroCountry = "PT";

        public static string PrimaveraApiUrl = String.Empty;
        public static string PrimaveraReadUsername = String.Empty;
        public static string PrimaveraWriteUsername = String.Empty;
        public static string PrimaveraPassword = String.Empty;
        public static string PrimaveraCompany = String.Empty;
        public static string PrimaveraInstance = String.Empty;
        public static string PrimaveraGrantType = String.Empty;
        public static string PrimaveraLine = String.Empty;
        public static PrimaveraUrlsConfig PrimaveraUrls = new();
        public static int delayMinutesBetweenRepeatedEmailChecks = 10;
        public static int timespanForEmailRetrieval = 20 * 60;

        public static bool sendAutomaticallyDrafts = false;
        public static string ccConfirmations = string.Empty;
        public static int theresholdConfiancaAI = 90;
        public static string apiKeyOpenAI = string.Empty;
        public static string endpointOpenAI = string.Empty;
        public static int delayBetweenRequestsOpenAI = 5000;
        public static string AzureOCRKey = string.Empty;
        public static string AzureOCREndpoint = string.Empty;
        public static string ERPEndpoint = string.Empty;
        public static string ERPUser = string.Empty;
        public static string ERPpassword = string.Empty;


        public static string ChristmasGif = string.Empty;
        public static string Logo = string.Empty;
        public static string companyName = string.Empty;
        public static string productShortName = string.Empty;
        public static string productFullName = string.Empty;
        public static string nodeName = string.Empty;
        public static bool isProduction = false;
        public static string defaultLanguage = "pt";
        public static string[] corsAllowedAddress = null;

        public static string generalKey = string.Empty;
        public static string sqlServer = string.Empty;

        public static int sqlPort = 0;
        public static string sqlDatabase = string.Empty;
        public static string sqlUser = string.Empty;
        public static string sqlPassword = string.Empty;

        private static Timer timerReloadDinamicConfigs;
        public static Dictionary<string, string> dinamicConfigs = new Dictionary<string, string>();
        public static Dictionary<string, string> platformSettings = [];
        private static int defaultPasswordExpirationPolicyDays = 60;
        private static int defaultTimeout_retry = 60;
        private static int defaultRestTokenExpirationTimeHours = 1;

        public static string imageMaxHeight = string.Empty;
        public static string imageMaxWidth = string.Empty;

        public static void LoadConfigs()
        {
            Log.Init();
            Log.Info("**********************************");
            Log.Info("Starting Configuration Manager...");
            Log.Info("**********************************");

            MailboxAccount = ConfigurationManager.AppSettings["MailboxAccount"].Split(";").ToList();
            MailboxPassword = ConfigurationManager.AppSettings["MailboxPassword"].Split(";").ToList();

            for (int i = 0; i < MailboxAccount.Count; i++)
            {
                MailboxCredentials.Add(MailboxAccount[i], MailboxPassword[i]);
            }

            InboxFolder = ConfigurationManager.AppSettings["InboxFolder"];
            QuotationsFolder = ConfigurationManager.AppSettings["QuotationsFolder"];
            ValidateFolder = ConfigurationManager.AppSettings["ValidateFolder"];
            ReceiptsFolder = ConfigurationManager.AppSettings["ReceiptsFolder"];
            OthersFolder = ConfigurationManager.AppSettings["OthersFolder"];
            OrdersFolder = ConfigurationManager.AppSettings["OrdersFolder"];
            SentFolder = ConfigurationManager.AppSettings["SentFolder"];
            DuplicatesFolder = ConfigurationManager.AppSettings["DuplicatesFolder"];
            TestFolder = ConfigurationManager.AppSettings["TestFolder"];
            TrashFolder = ConfigurationManager.AppSettings["TrashFolder"];
            CertificatesFolder = ConfigurationManager.AppSettings["CertificatesFolder"];
            SpamFolder = ConfigurationManager.AppSettings["SpamFolder"];

            SystemEmail = ConfigurationManager.AppSettings["SystemEmail"];
            SystemPassword = ConfigurationManager.AppSettings["SystemPassword"];

            engimatrixInternalApiKey = ConfigurationManager.AppSettings["engimatrix_internal_api_key"];
            postalCodesPath = ConfigurationManager.AppSettings["postalCodesPath"];

            GoogleMapsApiKey = ConfigurationManager.AppSettings["GoogleMapsApiKey"];
            HereApiKey = ConfigurationManager.AppSettings["HereApiKey"];
            HereAppKeyId = ConfigurationManager.AppSettings["HereAppKeyId"];
            HereAppKeySecret = ConfigurationManager.AppSettings["HereAppKeySecret"];
            MFLat = decimal.Parse(ConfigurationManager.AppSettings["MFLat"]);
            MFLng = decimal.Parse(ConfigurationManager.AppSettings["MFLng"]);

            delayMinutesBetweenRepeatedEmailChecks = Int32.Parse(ConfigurationManager.AppSettings["delayMinutesBetweenRepeatedEmailChecks"]);
            timespanForEmailRetrieval = Int32.Parse(ConfigurationManager.AppSettings["timespanForEmailRetrieval"]) * 60;

            PrimaveraApiUrl = ConfigurationManager.AppSettings["PrimaveraApiUrl"];
            PrimaveraReadUsername = ConfigurationManager.AppSettings["PrimaveraReadUsername"];
            PrimaveraWriteUsername = ConfigurationManager.AppSettings["PrimaveraWriteUsername"];
            PrimaveraPassword = ConfigurationManager.AppSettings["PrimaveraPassword"];
            PrimaveraCompany = ConfigurationManager.AppSettings["PrimaveraCompany"];
            PrimaveraInstance = ConfigurationManager.AppSettings["PrimaveraInstance"];
            PrimaveraGrantType = ConfigurationManager.AppSettings["PrimaveraGrantType"];
            PrimaveraLine = ConfigurationManager.AppSettings["PrimaveraLine"];

            PrimaveraUrls.Moedas = ConfigurationManager.AppSettings["PrimaveraLista_Moedas"];
            PrimaveraUrls.Paises = ConfigurationManager.AppSettings["PrimaveraLista_Paises"];
            PrimaveraUrls.Distritos = ConfigurationManager.AppSettings["PrimaveraLista_Distritos"];
            PrimaveraUrls.TaxasIVA = ConfigurationManager.AppSettings["PrimaveraLista_TaxasIVA"];
            PrimaveraUrls.ModoPagamento = ConfigurationManager.AppSettings["PrimaveraLista_ModoPagamento"];
            PrimaveraUrls.CondPagamento = ConfigurationManager.AppSettings["PrimaveraLista_CondPagamento"];
            PrimaveraUrls.Unidades = ConfigurationManager.AppSettings["PrimaveraLista_Unidades"];
            PrimaveraUrls.UnidadesConversao = ConfigurationManager.AppSettings["PrimaveraLista_UnidadesConversao"];
            PrimaveraUrls.Marcas = ConfigurationManager.AppSettings["PrimaveraLista_Marcas"];
            PrimaveraUrls.Modelos = ConfigurationManager.AppSettings["PrimaveraLista_Modelos"];
            PrimaveraUrls.Familias = ConfigurationManager.AppSettings["PrimaveraLista_Familias"];
            PrimaveraUrls.SubFamilias = ConfigurationManager.AppSettings["PrimaveraLista_SubFamilias"];
            PrimaveraUrls.Dimensoes = ConfigurationManager.AppSettings["PrimaveraLista_Dimensoes"];
            PrimaveraUrls.LinhaDimensao = ConfigurationManager.AppSettings["PrimaveraLista_LinhaDimensao"];
            PrimaveraUrls.Artigos = ConfigurationManager.AppSettings["PrimaveraLista_Artigos"];
            PrimaveraUrls.ArtigosUnidades = ConfigurationManager.AppSettings["PrimaveraLista_ArtigosUnidades"];
            PrimaveraUrls.Clientes = ConfigurationManager.AppSettings["PrimaveraLista_Clientes"];
            PrimaveraUrls.Fornecedores = ConfigurationManager.AppSettings["PrimaveraLista_Fornecedores"];
            PrimaveraUrls.MoradasAlternativas = ConfigurationManager.AppSettings["PrimaveraLista_MoradasAlternativas"];
            PrimaveraUrls.StockDisponivelArtigo = ConfigurationManager.AppSettings["PrimaveraLista_StockDisponivelArtigo"];
            PrimaveraUrls.StockDisponivelArtigoArmazem = ConfigurationManager.AppSettings["PrimaveraLista_StockDisponivelArtigoArmazem"];
            PrimaveraUrls.PrecoVenda = ConfigurationManager.AppSettings["PrimaveraLista_PrecoVenda"];
            PrimaveraUrls.PrecoCompra = ConfigurationManager.AppSettings["PrimaveraLista_PrecoCompra"];
            PrimaveraUrls.Pendentes = ConfigurationManager.AppSettings["PrimaveraLista_Pendentes"];
            PrimaveraUrls.EncomendasCabecalhos = ConfigurationManager.AppSettings["PrimaveraLista_EncomendasCabecalhos"];
            PrimaveraUrls.EncomendasLinhas = ConfigurationManager.AppSettings["PrimaveraLista_EncomendasLinhas"];
            PrimaveraUrls.MFClientes = ConfigurationManager.AppSettings["PrimaveraLista_MFClientes"];
            PrimaveraUrls.MFArtigos = ConfigurationManager.AppSettings["PrimaveraLista_MFArtigos"];
            PrimaveraUrls.MFFaturas = ConfigurationManager.AppSettings["PrimaveraLista_MFFaturas"];

            sendAutomaticallyDrafts = bool.Parse(ConfigurationManager.AppSettings["sendAutomaticallyDrafts"]);
            ccConfirmations = ConfigurationManager.AppSettings["ccConfirmations"];

            apiKeyOpenAI = ConfigurationManager.AppSettings["ApiKeyOpenAI"];
            endpointOpenAI = ConfigurationManager.AppSettings["endpointOpenAI"];
            delayBetweenRequestsOpenAI = int.Parse(ConfigurationManager.AppSettings["DelayBetweenRequestsOpenAI"]);
            theresholdConfiancaAI = int.Parse(ConfigurationManager.AppSettings["theresholdConfiancaAI"]);
            AzureOCRKey = ConfigurationManager.AppSettings["AzureOCRKey"];
            AzureOCREndpoint = ConfigurationManager.AppSettings["AzureOCREndpoint"];
            ERPEndpoint = ConfigurationManager.AppSettings["ERPEndpoint"];
            ERPUser = ConfigurationManager.AppSettings["ERPUser"];
            ERPpassword = ConfigurationManager.AppSettings["ERPpassword"];

            ChristmasGif = Util.ImageToBase64("Emails/logo/MasterferroHappyChristmas.gif");
            Logo = Util.ImageToBase64("Emails/logo/masterferro_logo.png");
            companyName = ConfigurationManager.AppSettings["company_name"];
            productShortName = ConfigurationManager.AppSettings["product_short_name"];
            productFullName = ConfigurationManager.AppSettings["product_full_name"];
            nodeName = ConfigurationManager.AppSettings["node_name"];
            isProduction = bool.Parse(ConfigurationManager.AppSettings["is_production"]);
            defaultLanguage = ConfigurationManager.AppSettings["default_language"];
            generalKey = ConfigurationManager.AppSettings["general_key"];
            corsAllowedAddress = ConfigurationManager.AppSettings["cors_allowed_address"].Split(";");
            sqlServer = ConfigurationManager.AppSettings["sql_server"];
            sqlPort = int.Parse(ConfigurationManager.AppSettings["sql_port"]);
            sqlDatabase = ConfigurationManager.AppSettings["sql_database"];
            sqlUser = ConfigurationManager.AppSettings["sql_user"];
            sqlPassword = ConfigurationManager.AppSettings["sql_password"];

            imageMaxHeight = ConfigurationManager.AppSettings["imageMaxHeight"];
            imageMaxWidth = ConfigurationManager.AppSettings["imageMaxWidth"];

            EmailServer = ConfigurationManager.AppSettings["EmailServer"];
            EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            IMAPPort = ConfigurationManager.AppSettings["IMAPPort"];
            SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];

            ValidateConfigs();

            Log.Info("*** Listing all static configurations ***");
            Log.Info("[is_production] - " + isProduction);
            Log.Info("[MailboxAccount] - " + MailboxAccount);
            Log.Info("[InboxFolder] - " + InboxFolder);
            //Log.Info("[MsInputFolder] - " + MsInputFolderPO);
            Log.Info("[sendAutomaticallyDrafts] - " + sendAutomaticallyDrafts);
            //Log.Info("[ccConfirmations] - " + ccConfirmations);

            Log.Info("[delayBetweenRequestsOpenAI] - " + delayBetweenRequestsOpenAI);
            Log.Info("[theresholdConfiancaAI] - " + theresholdConfiancaAI);
            Log.Info("[sql_server] - " + sqlServer);
            Log.Info("[sql_port] - " + sqlPort);
            Log.Info("[sql_database] - " + sqlDatabase);
            Log.Info("[sql_user] - " + sqlUser);
        }

        private static void ValidateConfigs()
        {
            if (
                MailboxAccount.Count == 0 ||
                MailboxPassword.Count == 0 ||
                MailboxAccount.Count != MailboxPassword.Count ||
                string.IsNullOrEmpty(apiKeyOpenAI) ||
                string.IsNullOrEmpty(endpointOpenAI) ||
                delayBetweenRequestsOpenAI == 0 ||
                theresholdConfiancaAI == 0 ||
                timespanForEmailRetrieval == 0 ||
                string.IsNullOrEmpty(AzureOCRKey) ||
                string.IsNullOrEmpty(AzureOCREndpoint) ||
                string.IsNullOrEmpty(ERPEndpoint) ||
                string.IsNullOrEmpty(ERPUser) ||
                string.IsNullOrEmpty(ERPpassword) ||
                string.IsNullOrEmpty(InboxFolder) ||
                string.IsNullOrEmpty(OrdersFolder) ||
                string.IsNullOrEmpty(OthersFolder) ||
                string.IsNullOrEmpty(ValidateFolder) ||
                string.IsNullOrEmpty(QuotationsFolder) ||
                string.IsNullOrEmpty(CertificatesFolder) ||
                string.IsNullOrEmpty(DuplicatesFolder) ||
                string.IsNullOrEmpty(SentFolder) ||
                string.IsNullOrEmpty(ReceiptsFolder) ||
                string.IsNullOrEmpty(SpamFolder) ||
                string.IsNullOrEmpty(EmailServer) ||
                string.IsNullOrEmpty(IMAPPort) ||
                string.IsNullOrEmpty(EmailPassword) ||
                string.IsNullOrEmpty(SMTPPort) ||
                string.IsNullOrEmpty(PrimaveraApiUrl) ||
                string.IsNullOrEmpty(PrimaveraReadUsername) ||
                string.IsNullOrEmpty(PrimaveraWriteUsername) ||
                string.IsNullOrEmpty(PrimaveraPassword) ||
                string.IsNullOrEmpty(PrimaveraCompany) ||
                string.IsNullOrEmpty(PrimaveraInstance) ||
                string.IsNullOrEmpty(PrimaveraGrantType) ||
                string.IsNullOrEmpty(PrimaveraLine) ||
                string.IsNullOrEmpty(ChristmasGif) ||
                string.IsNullOrEmpty(Logo) ||
                string.IsNullOrEmpty(HereApiKey) ||
                string.IsNullOrEmpty(HereAppKeyId) ||
                string.IsNullOrEmpty(HereAppKeySecret) ||
                MFLat == 0 ||
                MFLng == 0 ||
                string.IsNullOrEmpty(engimatrixInternalApiKey) ||
                string.IsNullOrEmpty(postalCodesPath)
            )
            {
                Log.Error("App Configs is not valid!");
                throw new InvalidOperationException("One or more configuration settings are missing.");
            }

            // Validate PrimaveraUrls
            if (PrimaveraUrls == null ||
                string.IsNullOrEmpty(PrimaveraUrls.Artigos) ||
                string.IsNullOrEmpty(PrimaveraUrls.ArtigosUnidades) ||
                string.IsNullOrEmpty(PrimaveraUrls.Clientes) ||
                string.IsNullOrEmpty(PrimaveraUrls.CondPagamento) ||
                string.IsNullOrEmpty(PrimaveraUrls.Dimensoes) ||
                string.IsNullOrEmpty(PrimaveraUrls.Distritos) ||
                string.IsNullOrEmpty(PrimaveraUrls.Familias) ||
                string.IsNullOrEmpty(PrimaveraUrls.Fornecedores) ||
                string.IsNullOrEmpty(PrimaveraUrls.LinhaDimensao) ||
                string.IsNullOrEmpty(PrimaveraUrls.Marcas) ||
                string.IsNullOrEmpty(PrimaveraUrls.Moedas) ||
                string.IsNullOrEmpty(PrimaveraUrls.MoradasAlternativas) ||
                string.IsNullOrEmpty(PrimaveraUrls.Paises) ||
                string.IsNullOrEmpty(PrimaveraUrls.Pendentes) ||
                string.IsNullOrEmpty(PrimaveraUrls.PrecoCompra) ||
                string.IsNullOrEmpty(PrimaveraUrls.PrecoVenda) ||
                string.IsNullOrEmpty(PrimaveraUrls.StockDisponivelArtigo) ||
                string.IsNullOrEmpty(PrimaveraUrls.StockDisponivelArtigoArmazem) ||
                string.IsNullOrEmpty(PrimaveraUrls.SubFamilias) ||
                string.IsNullOrEmpty(PrimaveraUrls.TaxasIVA) ||
                string.IsNullOrEmpty(PrimaveraUrls.Unidades) ||
                string.IsNullOrEmpty(PrimaveraUrls.UnidadesConversao) ||
                string.IsNullOrEmpty(PrimaveraUrls.Modelos) ||
                string.IsNullOrEmpty(PrimaveraUrls.EncomendasCabecalhos) ||
                string.IsNullOrEmpty(PrimaveraUrls.EncomendasLinhas) ||
                string.IsNullOrEmpty(PrimaveraUrls.MFClientes) ||
                string.IsNullOrEmpty(PrimaveraUrls.MFArtigos) ||
                string.IsNullOrEmpty(PrimaveraUrls.MFFaturas)
            )
            {
                Log.Error("PrimaveraUrls is not valid!");
                throw new InvalidOperationException("PrimaveraUrls is not valid.");
            }
        }

        public static async Task LoadDinamicConfigsAsync()
        {
            await ExecuteLoadDinamicConfAsync(true);

            // set timer to periodic refresh from
            timerReloadDinamicConfigs = new Timer(async x =>
            {
                await ExecuteLoadDinamicConfAsync(false);
            }, null, new TimeSpan(00, 00, 00), new TimeSpan(00, 10, 00));
        }

        private static async Task ExecuteLoadDinamicConfAsync(bool isFirstTime)
        {
            using (var connSQL = new MySqlConnection(SqlConn.GetConnectionBuilder()))
            {
                connSQL.Open();
                var command = connSQL.CreateCommand();
                command.CommandText = "SELECT * FROM config;";

                using (var reader = command.ExecuteReader())
                {
                    if (isFirstTime)
                    {
                        Log.Info("*** Loading dinamic configurations ***");
                    }
                    while (reader.Read())
                    {
                        string key = reader.GetString(0);
                        string value = reader.GetString(1);

                        if (isFirstTime)
                        {
                            dinamicConfigs.Add(key, value);
                            Log.Info("[" + key + "] - " + value);
                        }
                        else
                        {
                            if (!dinamicConfigs[key].Equals(value))
                            {
                                dinamicConfigs[key] = value;
                                Log.Info("Loading a new value for dinamic config - " + key + " - new value - " + value);
                            }
                        }
                    }
                }
            }

            await ValidateDinamicConfigsAsync();
        }
        public static async Task LoadPlatformSettingsAsync()
        {
            // This function will always only be executed once. From there, any update done in the platform will update automatically the config
            List<PlatformSettingItem> settings = PlatformSettingModel.GetAll("system");

            Log.Info("*** Loading platform settings ***");

            foreach (PlatformSettingItem setting in settings)
            {
                string key = setting.setting_key;
                string value = setting.setting_value;

                platformSettings.Add(key, value);
                Log.Info("[" + key + "] - " + value);
            }

            ValidatePlatformSettings();
        }

        private static void UpdateDinamicConfig(string key, string value)
        {
            Dictionary<string, string> param = new()
            {
                { "@key", key },
                { "@value", value }
            };

            SqlExecuter.ExecFunction("UPDATE config set value = @value Where config = @key", param, "system", false, "Update Dynamic Config");
        }

        private static async Task ValidateDinamicConfigsAsync()
        {
            if (!dinamicConfigs.ContainsKey("dnos_number_requests") || string.IsNullOrEmpty(dinamicConfigs["dnos_number_requests"]) ||
               !dinamicConfigs.ContainsKey("dnos_number_requests_diff_seconds") || string.IsNullOrEmpty(dinamicConfigs["dnos_number_requests_diff_seconds"]) ||
               !dinamicConfigs.ContainsKey("log_level") || string.IsNullOrEmpty(dinamicConfigs["log_level"]) ||
               !dinamicConfigs.ContainsKey("notification_email_hostname") || string.IsNullOrEmpty(dinamicConfigs["notification_email_hostname"]) ||
               !dinamicConfigs.ContainsKey("notification_email_password") || string.IsNullOrEmpty(dinamicConfigs["notification_email_password"]) ||
               !dinamicConfigs.ContainsKey("notification_email_port") || string.IsNullOrEmpty(dinamicConfigs["notification_email_port"]) ||
               !dinamicConfigs.ContainsKey("notification_email_username") || string.IsNullOrEmpty(dinamicConfigs["notification_email_username"]) ||
               !dinamicConfigs.ContainsKey("client_endpoint") || string.IsNullOrEmpty(dinamicConfigs["client_endpoint"]) ||
               !dinamicConfigs.ContainsKey("auth2f_code_expiration_time") || string.IsNullOrEmpty(dinamicConfigs["auth2f_code_expiration_time"]) ||
               !dinamicConfigs.ContainsKey("use_2factor_auth") || string.IsNullOrEmpty(dinamicConfigs["use_2factor_auth"]) ||
               !dinamicConfigs.ContainsKey("delete_auth2fcodes_older_than_this_minutes") || string.IsNullOrEmpty(dinamicConfigs["delete_auth2fcodes_older_than_this_minutes"]) ||
               !dinamicConfigs.ContainsKey("last_msToken_validation_time") || string.IsNullOrEmpty(dinamicConfigs["last_msToken_validation_time"]) ||
               !dinamicConfigs.ContainsKey("password_expiration_policy_days") || string.IsNullOrEmpty(dinamicConfigs["password_expiration_policy_days"]) ||
               !dinamicConfigs.ContainsKey("email_feedback_exp_time") || string.IsNullOrEmpty(dinamicConfigs["email_feedback_exp_time"]) ||
               !dinamicConfigs.ContainsKey("email_sender_feedback") || string.IsNullOrEmpty(dinamicConfigs["email_sender_feedback"]) ||
               !dinamicConfigs.ContainsKey("reset_token_expiration_time") || string.IsNullOrEmpty(dinamicConfigs["reset_token_expiration_time"])
                )
            {
                throw new MissingDinamicConfigException("It's missing a dinamic config from DB");
            }
        }

        private static void ValidatePlatformSettings()
        {
            if (platformSettings == null || platformSettings.Count == 0)
            {
                throw new MissingDinamicConfigException("It's missing a plataform setting from DB");
            }

            if (!platformSettings.ContainsKey(PlatformSettingId.QuotationExpirationTime.GetDescription()) || string.IsNullOrEmpty(platformSettings[PlatformSettingId.QuotationExpirationTime.GetDescription()]))
            {
                throw new MissingDinamicConfigException("It's missing a plataform setting from DB - quotation_expiration_time");
            }
        }

        public static Log.LogLevel LogLevel()
        {
            if (dinamicConfigs.ContainsKey("log_level"))
            {
                return (Log.LogLevel)Enum.Parse(typeof(Log.LogLevel), dinamicConfigs["log_level"]);
            }

            return Log.LogLevel.INFO;
        }

        public static int DnosNumberRequests()
        {
            if (dinamicConfigs.ContainsKey("dnos_number_requests"))
            {
                return int.Parse(dinamicConfigs["dnos_number_requests"]);
            }

            throw new MissingDinamicConfigException("Missing config dnos_number_requests");
        }

        public static int DnosNumberRequestsDiffSeconds()
        {
            if (dinamicConfigs.ContainsKey("dnos_number_requests_diff_seconds"))
            {
                return int.Parse(dinamicConfigs["dnos_number_requests_diff_seconds"]);
            }

            throw new MissingDinamicConfigException("Missing config dnos_number_requests_diff_seconds");
        }

        public static string NotificationEmailHostname()
        {
            if (dinamicConfigs.ContainsKey("notification_email_hostname"))
            {
                return dinamicConfigs["notification_email_hostname"];
            }

            throw new MissingDinamicConfigException("Missing config notification_email_hostname");
        }

        public static string NotificationEmailPassword()
        {
            if (dinamicConfigs.ContainsKey("notification_email_password"))
            {
                return Cryptography.Decrypt(dinamicConfigs["notification_email_password"], "notification_email_password");
            }

            throw new MissingDinamicConfigException("Missing config notification_email_password");
        }

        public static string NotificationEmailPort()
        {
            if (dinamicConfigs.ContainsKey("notification_email_port"))
            {
                return dinamicConfigs["notification_email_port"];
            }

            throw new MissingDinamicConfigException("Missing config notification_email_port");
        }

        public static string NotificationEmailUsername()
        {
            if (dinamicConfigs.ContainsKey("notification_email_username"))
            {
                return dinamicConfigs["notification_email_username"];
            }

            throw new MissingDinamicConfigException("Missing config notification_email_username");
        }

        public static string ClientEndpoint()
        {
            if (dinamicConfigs.ContainsKey("client_endpoint"))
            {
                return dinamicConfigs["client_endpoint"];
            }

            throw new MissingDinamicConfigException("Missing config client_endpoint");
        }

        public static int PasswordExpirationPolicyDays()
        {
            if (dinamicConfigs.ContainsKey("password_expiration_policy_days"))
            {
                return int.Parse(dinamicConfigs["password_expiration_policy_days"]);
            }

            return defaultPasswordExpirationPolicyDays;
        }

        public static int TimeoutRetry()
        {
            if (dinamicConfigs.ContainsKey("timeout_retry"))
            {
                return Int32.Parse(dinamicConfigs["timeout_retry"]);
            }

            return defaultTimeout_retry;
        }

        public static int ResetTokenExpirationTime()
        {
            if (dinamicConfigs.ContainsKey("reset_token_expiration_time"))
            {
                return int.Parse(dinamicConfigs["reset_token_expiration_time"]);
            }

            return defaultRestTokenExpirationTimeHours;
        }

        public static int Auth2FCodeExpTime()
        {
            if (dinamicConfigs.ContainsKey("auth2f_code_expiration_time"))
            {
                return int.Parse(dinamicConfigs["auth2f_code_expiration_time"]);
            }

            throw new MissingDinamicConfigException("Missing config auth2f_code_expiration_time");
        }

        public static bool Use2FactorAuth()
        {
            if (dinamicConfigs.ContainsKey("use_2factor_auth"))
            {
                return bool.Parse(dinamicConfigs["use_2factor_auth"]);
            }

            throw new MissingDinamicConfigException("Missing config use_2factor_auth");
        }

        public static int DeleteOldAuthf2Codes()
        {
            if (dinamicConfigs.ContainsKey("delete_auth2fcodes_older_than_this_minutes"))
            {
                return int.Parse(dinamicConfigs["delete_auth2fcodes_older_than_this_minutes"]);
            }

            throw new MissingDinamicConfigException("Missing config delete_auth2fcodes_older_than_this_minutes");
        }
    }
}
