using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SignalRMiddleawre.Hubs
{
    /// <summary>
    /// MainHub 包含通用的连接信息
    /// 包含建立通用连接，记录用户连接信息
    ///         发送通用消息等
    /// </summary>
    [Authorize]
    public partial class MainHub : Hub
    {
        #region Connection
        /// <summary>
        /// 管理连接的用户
        /// </summary>
#pragma warning disable CS8714 // 类型不能用作泛型类型或方法中的类型参数。类型参数的为 Null 性与 "notnull" 约束不匹配。
        private static ConcurrentDictionary<string?, List<string>>? ConnectedUsers = new ConcurrentDictionary<string?, List<string>>();
#pragma warning restore CS8714 // 类型不能用作泛型类型或方法中的类型参数。类型参数的为 Null 性与 "notnull" 约束不匹配。
        /// <summary>
        /// 连接后
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        /// 
        public override async Task OnConnectedAsync()
        {
            // Get HttpContext In asp.net core signalr
            //IHttpContextFeature hcf = (IHttpContextFeature)this.Context.Features[typeof(IHttpContextFeature)];
            //HttpContext hc = hcf.HttpContext;
            //string uid = hc.Request.Path.Value.Split(new string[] { "=", "" }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();

            string? userid = Context.User?.Identity?.Name;
            if (userid == null || userid.Equals(string.Empty))
            {
                Trace.TraceInformation("user not loged in, can't connect signalr service");
                return;
            }
            Trace.TraceInformation(userid + "connected");
            // save connection
            List<string>? existUserConnectionIds;
            ConnectedUsers.TryGetValue(userid, out existUserConnectionIds);
            if (existUserConnectionIds == null)
            {
                existUserConnectionIds = new List<string>();
            }
            existUserConnectionIds.Add(Context.ConnectionId);
            ConnectedUsers.TryAdd(userid, existUserConnectionIds);

            await Clients.All.SendAsync("ServerInfo", userid, userid + " connected, connectionId = " + Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 断开当前连接后
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string? userid = Context.User?.Identity?.Name;
            // save connection
            List<string>? existUserConnectionIds;
#pragma warning disable CS8602 // 解引用可能出现空引用。
            ConnectedUsers.TryGetValue(userid, out existUserConnectionIds);
#pragma warning restore CS8602 // 解引用可能出现空引用。

#pragma warning disable CS8602 // 解引用可能出现空引用。
            existUserConnectionIds.Remove(Context.ConnectionId);
#pragma warning restore CS8602 // 解引用可能出现空引用。

            if (existUserConnectionIds.Count == 0)
            {
                List<string> garbage;
                ConnectedUsers.TryRemove(userid, out garbage);
            }

            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region Message
        /// <summary>
        /// 发送消息通知所有用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string msgType, string message)
        {
            // 增加逻辑， 处理指令
            await Clients.All.SendAsync("ReceiveMessage", msgType, message);
        }

        /// <summary>
        /// 根据用户id， 找到所有connectionId， 发送消息
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="message">message format : type-message </param>
        /// <returns></returns>
        public async Task SendToSingleUser(string userid, string message)
        {
            List<string>? existUserConnectionIds;
            // 根据用户id， 找到所有connectionId， 发送消息
#pragma warning disable CS8602 // 解引用可能出现空引用。
            ConnectedUsers.TryGetValue(userid, out existUserConnectionIds);
#pragma warning restore CS8602 // 解引用可能出现空引用。
            if (existUserConnectionIds == null)
            {
                existUserConnectionIds = new List<string>();
            }
            existUserConnectionIds.Add(Context.ConnectionId);
            ConnectedUsers.TryAdd(userid, existUserConnectionIds);
            await Clients.Clients(existUserConnectionIds).SendAsync("ReceiveMessage", message);
        }
        #endregion

    }
}