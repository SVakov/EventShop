document.getElementById("DeadLineDate").addEventListener("change", function () {
    const selectedDate = new Date(this.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
        document.getElementById("deadlineError").style.display = "block";
        this.value = "";
        return;
    }

    const currentDate = new Date();
    if (
        selectedDate.toDateString() === currentDate.toDateString() &&
        (selectedDate.getHours() > 18 || (selectedDate.getHours() === 18 && selectedDate.getMinutes() > 0))
    ) {
        errorElement.textContent = "Deadline on the same day cannot be after 18:00.";
        errorElement.style.display = "block";
        this.value = ""; 
        return;
    }

    errorElement.style.display = "none";
});