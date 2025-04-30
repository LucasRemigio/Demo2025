using engimatrix.Models;
using engimatrix.Utils;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace engimatrix.Hubs;
public class AuditHub : Hub
{
    public async Task SendMessage(string message)
    {
        AuditMessage auditMessage = JsonConvert.DeserializeObject<AuditMessage>(message);

        Log.Debug($"WS (AuditHub): {auditMessage}");

        await Clients.All.SendAsync("ReceiveMessage", message);
    }


    // Concurrency methods
    public async Task JoinEmailGroup(string message)
    {
        UserJoinedMessage userJoined = JsonConvert.DeserializeObject<UserJoinedMessage>(message);

        Log.Debug($"WS (AuditHub): {userJoined}");

        await Clients.Group(userJoined.email_token).SendAsync("userJoined", userJoined);

        // only add after to not receive updated unnecessary messages
        await Groups.AddToGroupAsync(Context.ConnectionId, userJoined.email_token);
    }

    public async Task LeaveEmailGroup(string message)
    {
        UserJoinedMessage userExited = JsonConvert.DeserializeObject<UserJoinedMessage>(message);

        Log.Debug($"WS (AuditHub): {userExited}");

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userExited.email_token);

        await Clients.Group(userExited.email_token).SendAsync("userExited", userExited);
    }
}