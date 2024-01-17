$(function () {
    checkIfTableEmpty("No enrollments.");

    function checkIfTableEmpty(message) {
        const tableRows = $(".table tr");

        if (tableRows.length < 2) {
            $(".table").html(`<tr><td colspan="4" style="color: red; text-align: center;"><h4>${message}</h4></td></tr>`);
        }
    }

    const createForm = (action, formId, formEnctype) => $("<form>")
        .addClass("w-50")
        .attr({
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
    const createTextarea = (cssClass, name, id, rows, isRequired) => $("<textarea>")
        .addClass(cssClass)
        .attr({
            name: name,
            id: id,
            rows: rows,
            required: isRequired,
        });

    function createDeclineForm(enrollmentId) {
        const declineForm = createForm("", "declineForm", "application/x-www-form-urlencoded")
        declineForm.append(createHiddenInput("EnrollmentId", enrollmentId));

        const label = createLabel("col-md-4", "ReasonMessage", "Reason");
        const textArea = createTextarea("form-control", "ReasonMessage", "ReasonMessage", 5, true);
        const formGroupDiv = $("<div>").addClass("row");
        const textAreaDiv = $("<div>").addClass("col-md-8").append(textArea);

        formGroupDiv.append(label, textAreaDiv);
        declineForm.append(formGroupDiv);

        return declineForm;
    }

    function loadDeclineModalContent(enrollmentId) {
        $(".modal-body").empty();
        $(".modal-body").append(createDeclineForm(enrollmentId));
    }

    $(".declineEnrollmentModal").on("click", function () {
        loadDeclineModalContent($(this).attr("data-enrollmentId"));
        $("#templateModalLongTitle").text("Decline Reason");

        const declineEnrollmentBtn = $("<button>", {
            "form": "declineForm",
            "type": "button",
            "class": "btn btn-primary declineEnrollment",
            "text": "Decline"
        });

        declineEnrollmentBtn.on("click", function () {
            const enrollmentId = $(".declineEnrollmentModal").attr("data-enrollmentId");

            showOverlay(0);
            $.ajax({
                url: "/Enrollment/Decline",
                type: "POST",
                data: new FormData($("#declineForm")[0]),
                processData: false,
                contentType: false,
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        removeTableRow($(`#tr_${enrollmentId}`));
                        checkIfTableEmpty("No pending enrollments");
                        $("#templateModal").modal("hide");
                    }
                    showToastrNotification(response.Message, response.Success ? "success" : "error");
                },
                error: function (error) {
                    showToastrNotification("Cannot decline enrollment. Please try again later.", "error");
                    console.error("Error:", error);
                }
            });
            hideOverlay(500);
        });

        $(".modal-footer").append(declineEnrollmentBtn);

        $("#templateModal").modal("show");
    });

    $("[data-dismiss='modal']").on("click", function () {
        $("#templateModal").modal("hide");
    });

    $("#templateModal").on("hidden.bs.modal", function () {
        $(".declineEnrollment").remove();
    });

    function removeTableRow(row) {
        row.remove();
        checkIfTableEmpty("No pending enrollments");
    }

    $(".approveEnrollment").on("click", function () {
        const enrollmentId = $(this).attr("data-enrollmentId");

        showOverlay(0);
        $.ajax({
            url: "/Enrollment/Approve",
            type: "POST",
            data: { enrollmentId: enrollmentId },
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    removeTableRow($(`#tr_${enrollmentId}`));
                    checkIfTableEmpty("No pending enrollments");
                }
                showToastrNotification(response.Message, response.Success ? "success" : "error");
            },
            error: function (error) {
                showToastrNotification("Cannot approve enrollment. Please try again later.", "error");
                console.error("Error:", error);
            }
        });
        hideOverlay(500);
    });

    function createDocumentUploadedList(documentUploads) {
        let documentUploadsList = $("<ul>");
        $.each(documentUploads, function (index, documentUpload) {
            const listItem = $("<li>").text(` Prerequisite ${documentUpload.PrerequisiteId}: ${documentUpload.UploadedFileName} `);
            const downloadLink = $("<a>")
                .attr({
                    href: `/Enrollment/GetDocumentUpload?uploadedFileName=${encodeURIComponent(documentUpload.UploadedFileName)}`,
                    download: documentUpload.UploadedFileName
                })
                .text("Download");

            listItem.append(downloadLink);
            documentUploadsList.append(listItem);
        });
        return documentUploadsList;
    }

    function loadDocumentsModalContent(enrollmentId) {
        return new Promise((resolve, reject) => {
            $(".modal-body").empty();

            $.ajax({
                url: "/Employee/GetEmployeeUploadsByEnrollmentId",
                type: "GET",
                data: { enrollmentId: enrollmentId },
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        $(".modal-body").append(createDocumentUploadedList(response.Result.EmployeeUploads));

                        if (response.Result.EmployeeUploads.length < 1) {
                            const uploadedDocumentStatus = $("<p>")
                                .attr("id", "uploadedDocumentStatus")
                                .text("No prerequisites needed.");
                            $(".modal-body").append(uploadedDocumentStatus);
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

    $(".viewDocumentsModal").on("click", function () {
        showOverlay(0);
        loadDocumentsModalContent($(this).attr("data-enrollmentId"))
            .then((documentsModalContentLoaded) => {
                if (!documentsModalContentLoaded) {
                    showToastrNotification("Cannot load documents at this moment. Please try again later.", "error");
                    return;
                }
                $("#templateModalLongTitle").text("Documents Uploaded");
                $("#templateModal").modal("show");
            })
            .catch((error) => {
                showToastrNotification("Cannot load documents at this moment. Please try again later.", "error");
                console.error("An error occurred:", error);
            });
        hideOverlay(500);
    });
});