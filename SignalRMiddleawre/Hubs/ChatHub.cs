using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRMiddleawre.Hubs
{
    public partial class MainHub : Hub
    {
        public async Task Chat_SendMessageToAll(string user, string message)
            => await Clients.All.SendAsync("Chat_ReceiveMessage", user, message);

        public async Task Chat_SendMessageToUser(string connectionId)
            => await Clients.All.SendAsync("Chat_Closed", connectionId);


    }
}
