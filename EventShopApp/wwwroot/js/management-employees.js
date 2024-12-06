function openEditModal(employeeId) {
    fetch(`/Management/Employees/Edit/${employeeId}`, { method: "GET" })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to fetch employee data.");
            }
            return response.json();
        })
        .then(employee => {
            document.getElementById("editEmployeeId").value = employee.id;
            document.getElementById("editRole").value = employee.role;
            document.getElementById("editIsFired").checked = employee.isFired;

            const editModal = new bootstrap.Modal(document.getElementById("editEmployeeModal"));
            editModal.show();
        })
        .catch(error => console.error("Error fetching employee data:", error));
}


document.getElementById("saveEditEmployeeButton").addEventListener("click", function () {
    const employeeId = document.getElementById("editEmployeeId").value;
    const updatedEmployee = {
        Id: employeeId,
        Role: document.getElementById("editRole").value,
        IsFired: document.getElementById("editIsFired").checked
    };

    fetch(`/Management/Employees/Edit`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(updatedEmployee)
    })
        .then(response => {
            if (response.ok) {
                location.reload(); // Refresh the page to show updated data
            } else {
                return response.json().then(data => {
                    alert("Error: " + (data.errors || "Failed to save changes"));
                });
            }
        })
        .catch(error => console.error("Error saving employee changes:", error));
});

document.getElementById("addReinforcementLink").addEventListener("click", function (event) {
    event.preventDefault();
    window.location.href = '/Management/Employees/Add';
});