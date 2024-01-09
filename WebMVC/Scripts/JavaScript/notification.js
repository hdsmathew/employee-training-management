$(function () {
    function checkIfNotificationListEmpty(message) {
        const notificationList = $("#notificationList");
        if (notificationList.children().length === 0) {
            notificationList.html(`${message}`);
        }
    }

    function createNotification(title, content, sentAt) {
        const listItem = $("<li>").addClass("list-group-item");
        const titleElement = $("<h5>").addClass("mb-1").text(title);
        const contentElement = $("<p>").addClass("mb-1").text(content);
        const sentAtDatetime = new Date(+sentAt.replace(/\D/g, ''));
        const sentAtElement = $("<small>").text(sentAtDatetime.toDateString() + " " + sentAtDatetime.toLocaleTimeString());

        listItem.append(titleElement, contentElement, sentAtElement);
        return listItem;
    }

    function addNotification(title, content, timestamp) {
        const notificationItem = createNotification(title, content, timestamp);
        $("#notificationList").append(notificationItem);
    }

    $.ajax({
        url: "/Notification/GetNotifications",
        method: "GET",
        dataType: "json",
        success: function (response) {
            $("#notificationList").empty();
            if (response.Success) {
                $("#notificationList").empty();
                response.Result.Notifications.forEach(function (notification) {
                    addNotification("MOCK TITLE", notification.NotificationMessage, notification.SentAt);
                });
            }
            checkIfNotificationListEmpty("You're up-to-date.");
            showToastrNotification(response.Message, response.Success ? "success" : "error");
        },
        error: function (error) {
            showToastrNotification("Error fetching notifications", "error")
            console.error(error);
        }
    });
});