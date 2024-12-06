function redirectToCart() {
    window.location.href = '/RegisteredUser/RegisteredUsersCart';
}

function submitOrder() {
    const form = document.querySelector('#orderForm');
    const formData = new FormData(form);

    fetch('/RegisteredUser/RegisteredUsersCart/SubmitOrder', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {

                window.location.href = '/RegisteredUser/RegisteredUsersCart/OrderConfirmation';
            } else {
                alert('Failed to submit order. Please try again.');
            }
        })
        .catch(error => console.error('Error:', error));
}

document.getElementById('orderButton').addEventListener('click', function () {
    const form = document.querySelector('#orderForm');
    const formData = new FormData(form);

    fetch('/RegisteredUser/RegisteredUsersCart/SubmitOrder', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log("Order submitted successfully!");
                window.location.href = data.redirectUrl;
            } else {
                alert(data.message || "Failed to submit order. Please try again.");
            }
        })
        .catch(error => console.error('Error:', error));
});