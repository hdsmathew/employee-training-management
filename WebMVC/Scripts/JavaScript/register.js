$(function () {
    const managersDropdownlist = $("#ManagerId");
    let managers = JSON.parse(sessionStorage.getItem("Managers"));
    let populateManagersDropDownList = (managers) => {
        $.each(managers, function (index, manager) {
            managersDropdownlist.append(`<option value="${manager.EmployeeId}">${manager.FirstName + manager.LastName}</option>`);
        });

        managersDropdownlist.find("option:first").prop("selected", true);
        $("#DepartmentId").val(managers[0].DepartmentId);
    };

    managers && populateManagersDropDownList(managers);

    if (!managers) {
        showOverlay(0);
        $.ajax({
            url: "/Employee/GetManagers",
            type: "GET",
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    populateManagersDropDownList(response.Result.Managers);
                    sessionStorage.setItem("Managers", JSON.stringify(response.Result.Managers));
                } else {
                    console.error(response.Message);
                }
            },
            error: function (error) {
                console.error(error);
            }
        });
        hideOverlay(500);
    }

    $("#ManagerId").change(function () {
        let selectedManagerId = $(this).val();
        let managers = JSON.parse(sessionStorage.getItem("Managers"));
        let selectedManager = _.find(managers, function (m) {
            return m.EmployeeId == selectedManagerId;
        });

        $("#DepartmentId").val(selectedManager.DepartmentId);
    });
});