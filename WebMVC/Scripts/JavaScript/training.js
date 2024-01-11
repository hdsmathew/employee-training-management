$(function () {
    checkIfTableEmpty("No available trainings.");

    function checkIfTableEmpty(message) {
        const tableRows = $(".table tr");

        let isTableEmpty = tableRows.length < 2;
        if (isTableEmpty) {
            $(".table").html(`<tr><td colspan="4" style="color: red; text-align: center;"><h4>${message}</h4></td></tr>`);
        }
        return isTableEmpty;
    }

    let trainings = JSON.parse(sessionStorage.getItem("Trainings"));

    const createForm = (action, formId, formEnctype) => $("<form>").attr({
        action: action,
        id: formId,
        enctype: formEnctype
    });
    const createHiddenInput = (name, value) => $("<input>").attr({
        type: "hidden",
        name,
        value,
        id: name
    });
    const createLabel = (cssClass, htmlFor, labelText) => $("<label>")
        .addClass(cssClass)
        .attr({
            for: htmlFor
        })
        .text(labelText);
    const createFileInput = (cssClass, inputName, inputId, dataPrerequisiteId, isRequired) => $("<input>")
        .addClass(cssClass)
        .attr({
            type: "file",
            name: inputName,
            id: inputId,
            required: isRequired,
            "data-prerequisiteid": dataPrerequisiteId
        });

    function createPrerequisitesForm(trainingId, requiredPrerequisites) {
        const enrollForm = createForm("", "enrollForm", "multipart/form-data")
        enrollForm.append(createHiddenInput("TrainingId", trainingId));

        requiredPrerequisites.forEach((prerequisite, i) => {
            const label = createLabel("col-md-4", `fileUpload_${i}`, `${prerequisite.DocumentName}:`);
            const fileInput = createFileInput("form-control", "EmployeeUploads", `fileUpload_${i}`, prerequisite.PrerequisiteId, true);
            const formGroupDiv = $("<div>").addClass("d-flex");
            const fileInputDiv = $("<div>").addClass("col-md-8").append(fileInput);

            formGroupDiv.append(label, fileInputDiv);
            enrollForm.append(formGroupDiv);
        });

        return enrollForm;
    }

    function loadEnrollModalContent(trainingId) {
        return new Promise((resolve, reject) => {
            $(".modal-body").empty();
            const trainingSelected = _.find(trainings, (o) => o.TrainingId == trainingId);
            let requiredPrequisites = [];

            if (trainingSelected.Prerequisites.length < 1) {
                const prerequisiteStatus = $("<p>")
                    .attr("id", "prerequisiteStatus")
                    .text("No prerequisites needed.");
                $(".modal-body").append(prerequisiteStatus);
                $(".modal-body").append(createPrerequisitesForm(trainingId, requiredPrequisites));
                resolve(true);
                return;
            }

            const employeeId = JSON.parse(sessionStorage.getItem("EmployeeId"));
            $.ajax({
                url: "/Employee/GetEmployeeUploads",
                type: "GET",
                data: { employeeId: employeeId },
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        requiredPrequisites = _.differenceBy(trainingSelected.Prerequisites, response.Result.EmployeeUploads, "PrerequisiteId");
                        $(".modal-body").append(createPrerequisitesForm(trainingSelected.TrainingId, requiredPrequisites));

                        if (requiredPrequisites.length < 1) {
                            const prerequisiteStatus = $("<p>")
                                .attr("id", "prerequisiteStatus")
                                .text("All prerequisites for this training are satisfied.");
                            $(".modal-body").append(prerequisiteStatus);
                        }
                    }
                    resolve(response.Success);
                },
                error: function (error) {
                    console.error("Error:", error);
                    reject(false);
                }
            });
        });
    }

    $("[data-target='#templateModal']").on("click", function () {
        loadEnrollModalContent($(this).attr("data-trainingId"))
            .then((enrollModalContentLoaded) => {
                if (!enrollModalContentLoaded) {
                    showToastrNotification("Cannot enroll at this moment. Please try again later.", "error");
                    return;
                }
                $("#templateModal").modal("show");
            })
            .catch((error) => {
                showToastrNotification("Cannot enroll at this moment. Please try again later.", "error");
                console.error("An error occurred:", error);
            });
    });


    $("[data-dismiss='modal']").on("click", function () {
        $("#templateModal").modal("hide");
    });

    $(".submitEnrollment").on("click", function (event) {
        event.preventDefault();

        const trainingId = $("#TrainingId").val();
        let formData = new FormData();
        formData.append("TrainingId", trainingId);
        $("input[type='file']").each(function (index, fileInput) {
            let file = fileInput.files[0];
            let prerequisiteId = $(fileInput).attr("data-prerequisiteid");

            formData.append("EmployeeUploads", file);
            formData.append("PrerequisiteIds", prerequisiteId);
        });

        $.ajax({
            url: "/Enrollment/SubmitEnrollment",
            type: "POST",
            data: formData,
            processData: false, // Don't convert to query string
            contentType: false, // Let browser set to delimit form fields in request body
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    $(`#tr_${trainingId}`).remove();
                    checkIfTableEmpty("No available trainings.");
                }
                showToastrNotification(response.Message, response.Success ? "success" : "error");
            },
            error: function (error) {
                showToastrNotification("Cannot enroll at this moment. Please try again later.", "error");
                console.error("Error:", error);
            }
        });
    });

    $(".deleteTraining").on("click", function () {
        const trainingId = $(this).attr("data-trainingId");

        $.ajax({
            url: "/Training/Delete",
            type: "POST",
            data: { trainingId: trainingId },
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    $(`#tr_${trainingId}`).remove();
                    checkIfTableEmpty("No available trainings.");
                }
                showToastrNotification(response.Message, response.Success ? "success" : "error");
            },
            error: function (error) {
                showToastrNotification("Cannot delete training. Please try again later.", "error");
                console.error("Error:", error);
            }
        });
    });

    $(".generateEnrollmentsReport").on("click", function () {
        const trainingId = $(this).attr("data-trainingId");

        $.ajax({
            url: "/Enrollment/GenerateEnrollmentsReportByTraining",
            type: "POST",
            data: { trainingId: trainingId },
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    $(".processEnrollments").prop("disabled", true);
                }
                showToastrNotification(response.Message, response.Success ? "success" : "error");
            },
            error: function (error) {
                showToastrNotification("Cannot process training enrollments. Please try again later.", "error");
                console.error("Error:", error);
            }
        });
    });

    $(".processEnrollments").on("click", function () {
        const trainingId = $(this).attr("data-trainingId");

        $.ajax({
            url: "/Enrollment/ValidateApprovedEnrollmentsByTraining",
            type: "POST",
            data: { trainingId: trainingId },
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    $(".processEnrollments").prop("disabled", true);
                }
                showToastrNotification(response.Message, response.Success ? "success" : "error");
            },
            error: function (error) {
                showToastrNotification("Cannot process training enrollments. Please try again later.", "error");
                console.error("Error:", error);
            }
        });
    });
});