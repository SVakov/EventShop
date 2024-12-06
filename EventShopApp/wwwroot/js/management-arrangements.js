document.getElementById("addArrangementButton").addEventListener("click", function () {
    openArrangementModal("Add Arrangement", {});
});

function openArrangementModal(title, arrangement) {
    document.getElementById("addArrangementModalLabel").textContent = title;
    document.getElementById("arrangementId").value = arrangement.Id || "";
    document.getElementById("arrangementType").value = arrangement.ArrangementItemType || "";
    document.getElementById("arrangementPrice").value = arrangement.Price || "";
    document.getElementById("arrangementQuantity").value = arrangement.ArrangementItemsQuantity || "";
    document.getElementById("arrangementDescription").value = arrangement.Description || "";
    document.getElementById("arrangementImageUrl").value = arrangement.ArrangementItemImageUrl || "";

    const modal = new bootstrap.Modal(document.getElementById("addArrangementModal"));
    modal.show();
}

function openEditModal(arrangementId) {
    fetch(`/Management/Arrangements/Edit/${arrangementId}`, { method: "GET" })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to fetch arrangement data.");
            }
            return response.json();
        })
        .then(arrangement => {
            document.getElementById("editArrangementId").value = arrangement.id;
            document.getElementById("editArrangementPrice").value = arrangement.price.toFixed(2);
            document.getElementById("editArrangementQuantity").value = arrangement.arrangementItemsQuantity;
            document.getElementById("editArrangementDescription").value = arrangement.description;
            document.getElementById("editArrangementImageUrl").value = arrangement.arrangementItemImageUrl;

            const editModal = new bootstrap.Modal(document.getElementById("editArrangementModal"));
            editModal.show();
        })
        .catch(error => console.error("Error fetching arrangement data:", error));
}


document.getElementById("saveArrangementButton").addEventListener("click", function () {
    const id = document.getElementById("arrangementId").value;
    const arrangement = {
        Id: id || 0, // 0 for new arrangements
        ArrangementItemType: document.getElementById("arrangementType").value,
        Price: parseFloat(document.getElementById("arrangementPrice").value),
        ArrangementItemsQuantity: parseInt(document.getElementById("arrangementQuantity").value, 10),
        Description: document.getElementById("arrangementDescription").value,
        ArrangementItemImageUrl: document.getElementById("arrangementImageUrl").value,
        IsAvailable: true
    };

    const url = id ? `/Management/Arrangements/Edit` : `/Management/Arrangements/Add`;
    const method = "POST";

    fetch(url, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(arrangement)
    })
        .then(response => {
            if (response.ok) {
                location.reload(); // Reload to reflect changes
            } else {
                return response.json().then(data => {
                    alert("Error: " + (data.errors || "Failed to save arrangement"));
                });
            }
        })
        .catch(error => console.error("Error saving arrangement:", error));
});

document.getElementById("saveEditArrangementButton").addEventListener("click", function () {
    const arrangementId = document.getElementById("editArrangementId").value;
    const updatedArrangement = {
        Id: arrangementId,
        ArrangementItemType: document.querySelector(`tr[data-arrangement-id="${arrangementId}"] td:first-child`).innerText,
        Price: parseFloat(document.getElementById("editArrangementPrice").value),
        ArrangementItemsQuantity: parseInt(document.getElementById("editArrangementQuantity").value, 10),
        Description: document.getElementById("editArrangementDescription").value,
        ArrangementItemImageUrl: document.getElementById("editArrangementImageUrl").value,
        IsAvailable: true
    };

    fetch(`/Management/Arrangements/Edit`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(updatedArrangement)
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                return response.json().then(data => {
                    alert("Failed to update the arrangement: " + (data.errors || "Unknown error"));
                });
            }
        })
        .catch(error => console.error("Error saving arrangement:", error));
});


function deleteArrangement(id) {
    if (confirm("Are you sure you want to delete this arrangement?")) {
        fetch(`/Management/Arrangements/Delete/${id}`, {
            method: "POST",
            headers: { "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value }
        })
            .then(response => {
                if (response.ok) {
                    document.querySelector(`tr[data-arrangement-id="${id}"]`).remove();
                } else {
                    response.json().then(data => {
                        alert("Failed to delete the arrangement: " + (data.message || "Unknown error"));
                    });
                }
            })
            .catch(error => console.error("Error deleting arrangement:", error));
    }
}

function bringBackArrangement(id) {
    if (confirm("Are you sure you want to bring back this arrangement?")) {
        fetch(`/Management/Arrangements/BringBack/${id}`, {
            method: "POST",
            headers: { "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value }
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    alert("Failed to bring back the arrangement.");
                }
            })
            .catch(error => console.error("Error bringing back arrangement:", error));
    }
}
// Filtering logic for arrangements
function applyFilter() {
    const filter = document.getElementById('filter').value;
    const sortOrder = document.getElementById('sortOrder').value;
    const url = `?filter=${filter}&sortOrder=${sortOrder}`;
    window.location.href = url;
}