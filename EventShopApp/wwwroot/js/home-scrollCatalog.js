document.addEventListener("DOMContentLoaded", function () {
    const scrollCatalog = (catalog, speed) => {
        let scrollAmount = 0;
        let direction = 1; // Start with scrolling right

        const interval = setInterval(() => {
            // Scroll the catalog in the current direction
            catalog.scrollLeft += direction * speed;

            scrollAmount += Math.abs(direction * speed);

            // If it reaches the end of the scroll, reverse direction
            if (scrollAmount >= catalog.scrollWidth || scrollAmount <= 0) {
                direction *= -1;
                scrollAmount = 0;
            }
        }, 30);
    };

    const flowersCatalog = document.querySelector('.flowers-catalog');
    const arrangementsCatalog = document.querySelector('.arrangements-catalog');

    // Start scrolling in both directions
    scrollCatalog(flowersCatalog, -1);
    scrollCatalog(arrangementsCatalog, 1);
});