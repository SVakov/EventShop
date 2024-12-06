function adjustQuantity(change, flowerId, maxQuantity) {
    const quantityInput = document.getElementById("quantity-" + flowerId);
    const cartQuantityInput = document.getElementById("cart-quantity-" + flowerId);

    // Adjust quantity
    let newQuantity = parseInt(quantityInput.value) + change;
    if (newQuantity < 0) newQuantity = 0;

    quantityInput.value = newQuantity;
    cartQuantityInput.value = newQuantity;

    validateQuantity(flowerId, maxQuantity);
}

function validateQuantity(flowerId, maxQuantity) {
    const quantityInput = document.getElementById("quantity-" + flowerId);
    const form = document.getElementById("cart-form-" + flowerId);
    const addToCartButton = document.getElementById("add-to-cart-button-" + flowerId);
    const warning = document.getElementById("stock-warning-" + flowerId);

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

function addToCart(flowerId) {
    const form = document.getElementById("cart-form-" + flowerId);
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
        .catch(error => console.error('Error:', error));
}