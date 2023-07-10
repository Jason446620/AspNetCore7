"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/mainHub")
    .configureLogging(signalR.LogLevel.Trace).build();
//connection.serverTimeoutInMilliseconds = 120000;

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    // 后期会结合notification 或者其他业务 ，实现页面动态加载
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    // 成功建立连接后， 会生成connectionId， 此时将connectionId 和 UserId 一并传给后端存储
    // 以确保每个打开的页面都可以收到 当前用户的信息
    console.log(connection.connectionId);
    // 加载状态显示
    loadStatus("available");
    //Send("jason446620","test");
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//connection.onclose(function () {
   
//});

var tryingToReconnect = false;

// Seems not work
connection.onreconnected(function () {
    tryingToReconnect = false;
    loadStatus("available");
    return console.log("Connection Reconnected")
});
// Seems not work
connection.onreconnecting(function (err) {
    tryingToReconnect = true;
    return console.log(err.message)
});

async function start() {
    try {
        await connection.start();
        loadStatus("available");
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        loadStatus("connecting");
        setTimeout(start, 5000);
    }
};

connection.onclose(async (e) => {
    loadStatus("disconnected");
    console.log("Connection Disconnected :" +e.error)
    console.log("Try to reconnecting...")
    await start();
});