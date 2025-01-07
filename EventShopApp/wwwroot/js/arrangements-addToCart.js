function adjustQuantity(change, arrangementId, maxQuantity) {
    const quantityInput = document.getElementById("quantity-" + arrangementId);
    const cartQuantityInput = document.getElementById("cart-quantity-" + arrangementId);

    // Adjust quantity
    let newQuantity = parseInt(quantityInput.value) + change;
    if (newQuantity < 0) newQuantity = 0;

    quantityInput.value = newQuantity;
    cartQuantityInput.value = newQuantity;

    validateQuantity(arrangementId, maxQuantity);
}

function validateQuantity(arrangementId, maxQuantity) {
    const quantityInput = document.getElementById("quantity-" + arrangementId);
    const form = document.getElementById("cart-form-" + arrangementId);
    const addToCartButton = document.getElementById("add-to-cart-button-" + arrangementId);
    const warning = document.getElementById("stock-warning-" + arrangementId);

    const quantity = parseInt(quantityInput.value);

    // Show form if quantity > 0, but check stock availability
    form.style.display = quantity > 0 ? "block" : "none";
    if (quantity > maxQuantity) {
        addToCartButton.disabled = true;
        warning.style.display = "block";
    } else {
        addToCartButton.disabled = false;
        warning.style.display = "none";
    }
}

function addToCart(arrangementId) {
    const form = document.getElementById("cart-form-" + arrangementId);
    const formData = new FormData(form);

    fetch(form.action, {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert("Item added to cart successfully!");
            } else {
                alert("Failed to add item to cart.");
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert("An error occurred while adding the item to the cart.");
        });
}