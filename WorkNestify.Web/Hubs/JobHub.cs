using Microsoft.AspNetCore.SignalR;

namespace WorkNestify.Web.Hubs;

public class JobHub : Hub
{
    public async Task SubscribeToJobUpdates()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "JobSeekers");
    }
}