
    document.addEventListener("DOMContentLoaded", function () {
        const filterLinks = document.querySelectorAll(".filter-link");
        const filterPrices = document.querySelectorAll(".filter-price");
        const productCards = document.querySelectorAll(".product-card");

        let selectedCategory = "all";
        let selectedMin = null;
        let selectedMax = null;

        function filterProducts() {
            productCards.forEach(card => {
                const category = card.dataset.category.trim().toLowerCase();
                const price = parseInt(card.dataset.productPrice);

                const matchCategory = (selectedCategory === "all" || category === selectedCategory);
                const matchPrice = (
                    selectedMin === null ||
                    (price >= selectedMin && price <= selectedMax)
                );

                if (matchCategory && matchPrice) {
                    card.style.display = "";
                } else {
                    card.style.display = "none";
                }
            });
        }

        filterLinks.forEach(link => {
            link.addEventListener("click", function (e) {
                e.preventDefault();
                selectedCategory = this.dataset.filter.trim().toLowerCase();
                filterProducts();
            });
        });

        filterPrices.forEach(priceLink => {
            priceLink.addEventListener("click", function (e) {
                e.preventDefault();
                selectedMin = parseInt(this.dataset.min);
                selectedMax = parseInt(this.dataset.max);
                filterProducts();
            });
        });
    });

