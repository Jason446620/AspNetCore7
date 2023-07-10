"use strict";
// Define the message type
var HubMsg = {
    Message: Msg,
    Notification: Notify,
    Console: 300
}

var Msg = {
    DefaultMsg:100,
    SystemMsg: 101,
    NormalMsg: 102
}
var Notify = {
    DefaultNotify: 200,
    SystemNotify: 201,
    NormalNotify: 202
}