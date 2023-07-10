"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/mainHub").withAutomaticReconnect().build();


//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("Chat_ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});



//connection.start().then(function () {
//    debugger
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//connection.onclose(function () {
//    return console.log("Connection Disconnected")
//});

//connection.onreconnected(function() {
//    return console.log("Connection Reconnected")
//});

//connection.onreconnecting(function (err) {
//    return console.log(err.message)
//});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("Chat_SendMessageToAll", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});