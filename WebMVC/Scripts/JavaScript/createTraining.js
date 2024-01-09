$(function () {
    let prerequisites = JSON.parse(sessionStorage.getItem("Prerequisites"));
    let dropdownCounter = 1;

    $("#addPrerequisite").on("click", function () {
        let dropdown = $("<select>")
            .addClass("form-select")
            .attr({
                id: "prerequisiteDropdown" + dropdownCounter,
                name: "SelectedPrerequisiteIds"
            });

        $.each(prerequisites, function (index, prerequisite) {
            dropdown.append($("<option>", {
                value: prerequisite.PrerequisiteId,
                text: prerequisite.DocumentName
            }));
        });

        let removeButton = $("<button>")
            .addClass("btn btn-danger btn-sm")
            .text("Remove")
            .on("click", function () {
                dropdownDiv.remove();
            });

        let dropdownDiv = $("<div>")
            .addClass("d-flex justify-content-between mb-2")
            .append(dropdown)
            .append(removeButton);

        $("#prerequisitesContainer").append(dropdownDiv);
        dropdownCounter++;
    });

    $("#createTraining").on("click", function (event) {
        event.preventDefault();

        let form = $("#createTrainingForm");
        if (form.valid()) {
            $.ajax({
                url: "/Training/Create",
                type: "POST",
                data: new FormData($("#createTrainingForm")[0]),
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (response) {
                    $("span[data-valmsg]").empty();
                    if (response.Success) {
                        $.each(response.Errors, function (key, value) {
                            $(`[data-valmsg-for="${key}"]`).text(value.join(' '));
                        });
                    }
                    showToastrNotification(response.Message, response.Success ? "success" : "error");
                },
                error: function (error) {
                    showToastrNotification("Cannot create training. Please try again later.", "error");
                    console.error("Error:", error);
                }
            });
        }
    });
});