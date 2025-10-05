document.addEventListener('DOMContentLoaded', () => {
    // --- ĐOẠN SCRIPT QUAN TRỌNG ĐỂ THÊM VÀO GIỎ HÀNG ---
    const addToCartButtons = document.querySelectorAll('.add-to-cart');

    addToCartButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            event.preventDefault();

            // Lấy thông tin sản phẩm trực tiếp từ data-attribute của nút
            const productId = button.dataset.productId;
            const productName = button.dataset.productName;
            const productImage = button.dataset.productImage;
            const productPrice = parseFloat(button.dataset.productPrice);

            // Lấy giỏ hàng hiện tại từ localStorage
            let cart = JSON.parse(localStorage.getItem('cart')) || [];

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            const existingProductIndex = cart.findIndex(item => item.id === productId);

            if (existingProductIndex > -1) {
                cart[existingProductIndex].quantity += 1;
            } else {
                cart.push({
                    id: productId,
                    name: productName,
                    image: productImage,
                    price: productPrice,
                    quantity: 1
                });
            }

            // Lưu giỏ hàng
            localStorage.setItem('cart', JSON.stringify(cart));

            // Hiệu ứng nút
            button.style.transform = 'scale(1)';
            button.textContent = 'Đã thêm!';
            button.style.background = 'linear-gradient(45deg, #27ae60, #2ed573)';

            setTimeout(() => {
                button.style.transform = 'scale(1)';
                button.textContent = 'Bỏ vào giỏ';
                button.style.background = '';
               
              
            }, 300);
        });
    });
});
