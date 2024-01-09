﻿$(function () {
    checkIfTableEmpty("No pending enrollments.");

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

    $("[data-target='#templateModal']").on("click", function () {
        loadDeclineModalContent($(this).attr("data-enrollmentId"));
        $("#templateModal").modal("show");
    });

    $("[data-dismiss='modal']").on("click", function () {
        $("#templateModal").modal("hide");
    });

    function removeTableRow(row) {
        row.remove();
        checkIfTableEmpty("No pending enrollments");
    }

    $(".approveEnrollment").on("click", function () {
        const enrollmentId = $(this).attr("data-enrollmentId");

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
    });

    $(".declineEnrollment").on("click", function () {
        const enrollmentId = $("[data-target='#templateModal']").attr("data-enrollmentId");

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
                }
                showToastrNotification(response.Message, response.Success ? "success" : "error");
            },
            error: function (error) {
                showToastrNotification("Cannot decline enrollment. Please try again later.", "error");
                console.error("Error:", error);
            }
        });
    });
});