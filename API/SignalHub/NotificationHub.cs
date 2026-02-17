using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace API.SignalHub
{
    public class NotificationHub : Hub
    {
        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        // When a user disconnects, remove them from the group
        public async Task LeaveGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
        //public void SentNotification(string userId, List<UserAlert> oAlerts)
        //{
        //    Clients.Group(userId).SendAsync("Alert", oAlerts.Where(x => x.Type == "Alert" && !x.MarkAsRead).ToList());
        //    //Clients.All.SendAsync("Alert", oAlerts.Where(x => x.Type == "Alert" && !x.MarkAsRead).ToList());
        //}
    }
}
