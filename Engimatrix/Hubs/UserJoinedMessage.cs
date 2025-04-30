using engimatrix.Utils;
using Microsoft.AspNetCore.SignalR;

namespace engimatrix.Hubs;
public class UserJoinedMessage
{
    public string email_token { get; set; }
    public string user_email { get; set; }
    public DateTime date { get; set; }

    public override string ToString()
    {
        return $"Email Token: {email_token}, User Email: {user_email}, Date: {date}";
    }
}