document.getElementById("addFlowerButton").addEventListener("click", function () {
    openFlowerModal("Add Flower", {});
});

function openEditModal(flowerId) {
    fetch(`/Management/Flowers/Edit/${flowerId}`, { method: "GET" })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to fetch flower data.");
            }
            return response.json();
        })
        .then(flower => {
            document.getElementById("editFlowerId").value = flower.id;
            document.getElementById("editFlowerQuantity").value = flower.flowerQuantity;
            document.getElementById("editFlowerDescription").value = flower.description;
            document.getElementById("editFlowerImageUrl").value = flower.flowerImageUrl;

            const editModal = new bootstrap.Modal(document.getElementById("editFlowerModal"));
            editModal.show();
        })
        .catch(error => console.error("Error fetching flower data:", error));
}

document.getElementById("saveEditFlowerButton").addEventListener("click", function () {
    const flowerId = document.getElementById("editFlowerId").value;
    const updatedFlower = {
        Id: flowerId,
        FlowerType: document.querySelector(`tr[data-flower-id="${flowerId}"] td:first-child`).innerText,
        FlowerQuantity: parseInt(document.getElementById("editFlowerQuantity").value, 10),
        Description: document.getElementById("editFlowerDescription").value,
        FlowerImageUrl: document.getElementById("editFlowerImageUrl").value,
        Price: parseFloat(document.querySelector(`tr[data-flower-id="${flowerId}"] td:nth-child(2)`).innerText.replace(/[^\d.]/g, "")),
        IsAvailable: true
    };

    fetch(`/Management/Flowers/Edit`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(updatedFlower)
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                return response.json().then(data => {
                    alert("Failed to update the flower: " + (data.errors || "Unknown error"));
                });
            }
        })
        .catch(error => console.error("Error saving flower:", error));
});

function deleteFlower(id) {
    if (confirm("Are you sure you want to delete this flower?")) {
        fetch(`/Management/Flowers/Delete/${id}`, {
            method: "POST",
            headers: { "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value }
        })
            .then(response => {
                if (response.ok) {
                    document.querySelector(`tr[data-flower-id="${id}"]`).remove();
                } else {
                    alert("Failed to delete the flower.");
                }
            })
            .catch(error => console.error("Error deleting flower:", error));
    }
}

function bringBackFlower(id) {
    if (confirm("Are you sure you want to bring back this flower?")) {
        fetch(`/Management/Flowers/BringBack/${id}`, {
            method: "POST",
            headers: { "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value }
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    alert("Failed to bring back the flower.");
                }
            })
            .catch(error => console.error("Error bringing back flower:", error));
    }
}

document.getElementById("saveFlowerButton").addEventListener("click", function () {
    const id = document.getElementById("flowerId").value;
    const flower = {
        Id: id || 0,
        FlowerType: document.getElementById("flowerType").value,
        Price: parseFloat(document.getElementById("flowerPrice").value),
        FlowerQuantity: parseInt(document.getElementById("flowerQuantity").value, 10),
        Description: document.getElementById("flowerDescription").value,
        FlowerImageUrl: document.getElementById("flowerImageUrl").value,
        IsAvailable: true
    };

    const url = id ? `/Management/Flowers/Edit` : `/Management/Flowers/Add`;
    const method = "POST";

    fetch(url, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(flower)
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                return response.json().then(data => {
                    alert("Error: " + (data.errors || "Failed to save flower"));
                });
            }
        })
        .catch(error => console.error("Error saving flower:", error));
});

function openFlowerModal(title, flower) {
    document.getElementById("flowerModalLabel").textContent = title;
    document.getElementById("flowerId").value = flower.Id || "";
    document.getElementById("flowerType").value = flower.FlowerType || "";
    document.getElementById("flowerPrice").value = flower.Price || "";
    document.getElementById("flowerQuantity").value = flower.FlowerQuantity || "";
    document.getElementById("flowerDescription").value = flower.Description || "";
    document.getElementById("flowerImageUrl").value = flower.FlowerImageUrl || "";

    const modal = new bootstrap.Modal(document.getElementById("flowerModal"));
    modal.show();
}
function applyFilter() {
    const filter = document.getElementById('filter').value;
    const sortOrder = document.getElementById('sortOrder').value;
    const url = `?filter=${filter}&sortOrder=${sortOrder}`;
    window.location.href = url;
}