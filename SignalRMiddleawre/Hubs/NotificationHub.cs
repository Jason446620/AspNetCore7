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
        public async Task Botify_SendMessage(string userid, string message)
            => await Clients.All.SendAsync("Notification Message", userid, message);
    }
}
