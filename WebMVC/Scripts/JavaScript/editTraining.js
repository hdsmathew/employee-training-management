$(function () {
    $("#editTraining").on("click", function (event) {
        event.preventDefault();

        let form = $("#editTrainingForm");
        if (form.valid()) {
            $.ajax({
                url: "/Training/Edit",
                type: "POST",
                data: new FormData($("#editTrainingForm")[0]),
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (response) {
                    $("span[data-valmsg]").empty();
                    if (!response.Success) {
                        $.each(response.Errors, function (key, value) {
                            $(`[data-valmsg-for="${key}"]`).text(value.join(' '));
                        });
                    }
                    showToastrNotification(response.Message, response.Success ? "success" : "error");
                },
                error: function (error) {
                    showToastrNotification("Cannot edit training. Please try again later.", "error");
                    console.error("Error:", error);
                }
            });
        }
    });
});