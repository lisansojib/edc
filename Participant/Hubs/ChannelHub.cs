using Microsoft.AspNetCore.SignalR;

namespace Presentation.Participant.Hubs
{
    public class ChannelHub : Hub
    {
        public void Send(string channelName)
        {
            Clients.All.SendAsync("channelAdded", channelName);
        }
    }
}
