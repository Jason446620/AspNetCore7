"use strict";

// Send to All Client
function SendToAll(userid, type, message) {
    // 发送初始化消息
    connection.invoke("SendMessage", userid, type, message).catch(function (err) {
        return console.log(err.message);
    });
}
// Send to Specific UserId
function SendToUserByUid(connectionId, message) {
    // 发送初始化消息
    connection.invoke("SendToSingleUser", connectionId, message).catch(function (err) {
        return console.log(err.message);
    });
}
// Send to Specific ConnectionID
function SendToUserByConnid(connectionId, message) {
    // 发送初始化消息
    connection.invoke("SendToSingleUser", connectionId, message).catch(function (err) {
        return console.log(err.message);
    });
}