
document.querySelector("form.d-flex").addEventListener("submit", function (e) {
    e.preventDefault();

    const keyword = document.getElementById("searchInput").value.toLowerCase().trim();

    const sections = document.querySelectorAll(".featured-products");

    sections.forEach(section => {
        const cards = section.querySelectorAll(".product-card");
        let matchFound = false;

        cards.forEach(card => {
            const name = card.getAttribute("data-name").toLowerCase();
            if (name.includes(keyword)) {
                card.style.display = "block";
                matchFound = true;
            } else {
                card.style.display = "none";
            }``
        });

        // Ẩn section nếu không có sản phẩm nào khớp
        section.style.display = matchFound ? "block" : "none";
    });
});




document.querySelectorAll(".product-card").forEach(card => {
    const productName = card.querySelector(".product-name")?.innerText || "";
    card.setAttribute("data-name", productName.trim());
});


