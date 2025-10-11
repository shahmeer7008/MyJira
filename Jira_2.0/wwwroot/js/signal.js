let userRoleGlobal;

document.addEventListener('DOMContentLoaded', function () {
    const roleFromView = window.userRoleFromView || "Unknown";

    if (typeof signalR === 'undefined') {
        var script = document.createElement('script');
        script.src = "/libs/microsoft/signalr/signalr.min.js";
        script.onload = () => initializeSignalR(roleFromView);
        document.head.appendChild(script);
    } else {
        initializeSignalR(roleFromView);
    }
});

function initializeSignalR(userRole) {
    userRoleGlobal = userRole;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub", {
            withCredentials: true
        })
        .build();

    connection.on("ReceiveNotification", function (notification) {
       
        const isReply = notification.isReply ? "<strong>[REPLY]</strong> " : "";
        const msg = `
            <h1>Notification received</h1>
            <div class="card mb-2">
                <div class="card-body">
                    ${isReply}
                    <p><strong>${notification.sender}:</strong> ${notification.message}</p>
                    <small>${new Date(notification.timestamp).toLocaleString()}</small>
                    ${!notification.isReply ? `
                        <button class="btn btn-sm btn-outline-primary mt-2"
                            onclick="showReplyBox('${notification.originalSenderId}', '${notification.messageId}')">
                            Reply
                        </button>` : ""}
                </div>
            </div>`;
        document.getElementById("notificationsArea").innerHTML += msg;
    });

    connection.start().then(() => console.log("SignalR Connected")).catch(err => {
        console.error("SignalR Connection Error: " + err.toString());
        document.getElementById("notificationsArea").innerHTML =
            `<div class="alert alert-danger">Failed to connect to notification service.</div>`;
    });

    function sendMessage(targetId, message, isReply = false, originalMessageId = '') {
        if (!message) return;

        let method = "";

        if (userRole === "Admin") {
            if (isReply) {
                if (originalMessageId.startsWith('PM-')) {
                    method = 'ReplyToProjectManager';
                } else if (originalMessageId.startsWith('TM-')) {
                    method = 'ReplyToTeamMember';
                } else {
                    method = 'ReplyToProjectManager';
                }
            } else {
                if (targetId.startsWith('PM-') || targetId.includes('project')) {
                    method = 'SendToProjectManager';
                } else if (targetId.startsWith('TM-') || targetId.includes('team')) {
                    method = 'SendToTeamMember';
                } else {
                    method = 'SendToProjectManager';
                }
            }
        } else if (userRole === "ProjectManager") {
            if (isReply) {
                if (originalMessageId.startsWith('AD-')) {
                    method = 'ReplyToAdminFromProjectManager';
                } else if (originalMessageId.startsWith('TM-')) {
                    method = 'ReplyToTeamMember';
                } else {
                    method = 'ReplyToAdminFromProjectManager';
                }
            } else {
                if (targetId.startsWith('AD-') || targetId.includes('admin')) {
                    method = 'SendToAdminFromProjectManager';
                } else if (targetId.startsWith('TM-') || targetId.includes('team')) {
                    method = 'SendToTeamMember';
                } else {
                    method = 'SendToAdminFromProjectManager';
                }
            }
        } else if (userRole === "TeamMember") {
            if (isReply) {
                if (originalMessageId.startsWith('AD-')) {
                    method = 'ReplyToAdminFromTeamMember';
                } else if (originalMessageId.startsWith('PM-')) {
                    method = 'ReplyToProjectManager';
                } else {
                    method = 'ReplyToProjectManager';
                }
            } else {
                if (targetId.startsWith('AD-') || targetId.includes('admin')) {
                    method = 'SendToAdminFromTeamMember';
                } else if (targetId.startsWith('PM-') || targetId.includes('project')) {
                    method = 'SendToProjectManager';
                } else {
                    method = 'SendToProjectManager';
                }
            }
        }

        console.log(`Calling method: ${method} for role: ${userRole}`);

        connection.invoke(method, targetId, message, originalMessageId)
            .catch(err => {
                console.error(`Error calling ${method}:`, err.toString());
                alert(`Failed to send message. Error: ${err.toString()}`);
            });
    }

    document.getElementById("sendMessageButton").addEventListener("click", function () {
        const targetId = document.getElementById("targetId").value.trim();
        const messageText = document.getElementById("messageText").value.trim();
        sendMessage(targetId, messageText);
        document.getElementById("messageText").value = '';
    });

    window.showReplyBox = function (originalSenderId, originalMessageId) {
        const replyBoxId = `replyBox-${originalMessageId}`;
        if (document.getElementById(replyBoxId)) return;

        const replyBox = document.createElement('div');
        replyBox.innerHTML = `
            <div class="input-group mt-2" id="${replyBoxId}">
                <input type="text" class="form-control" placeholder="Type reply..." />
                <button class="btn btn-outline-success" onclick="sendReply('${originalSenderId}', '${originalMessageId}', '${replyBoxId}')">Send</button>
            </div>`;
        document.getElementById("notificationsArea").appendChild(replyBox);
    };

    window.sendReply = function (originalSenderId, originalMessageId, replyBoxId) {
        const input = document.querySelector(`#${replyBoxId} input`);
        const replyText = input.value.trim();
        if (replyText) {
            sendMessage(originalSenderId, replyText, true, originalMessageId);
            input.value = '';
            document.getElementById(replyBoxId).remove();
        }
    };
}
