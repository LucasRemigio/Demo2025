using engimatrix.Utils;
using Microsoft.AspNetCore.SignalR;

namespace engimatrix.Hubs;
public class AuditMessage
{
    public string email_token { get; set; }
    public int status_id { get; set; }
    public string status_description { get; set; }

    public override string ToString()
    {
        return $"Email Token: {email_token}, Status ID: {status_id}, Status Description: {status_description}";
    }
}