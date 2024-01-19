$(function () {
    const prerequisites = JSON.parse(sessionStorage.getItem("Prerequisites"));
    const prerequisitesLength = prerequisites.length;
    let dropdownCounter = 0;

    $("#addPrerequisite").on("click", function () {
        if (dropdownCounter == prerequisitesLength) {
            showToastrNotification(`Only ${prerequisitesLength} prerequisites can be added`, "error");
            return;
        }

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
                dropdownCounter--;
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
        const formData = new FormData($("#createTrainingForm")[0]);
        const selectedPrerequisiteIds = formData.getAll("SelectedPrerequisiteIds");

        if (_.uniq(selectedPrerequisiteIds).length != selectedPrerequisiteIds.length) {
            showToastrNotification(`Cannot select duplicate prerequisites`, "error");
            return;
        }

        let form = $("#createTrainingForm");
        if (form.valid()) {
            showOverlay(0);
            $.ajax({
                url: "/Training/Create",
                type: "POST",
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (response) {
                    $("span[data-valmsg-for]").empty();

                    if (!response.Success) {
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
            hideOverlay(500);
        }
    });
});