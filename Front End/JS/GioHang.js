document.addEventListener('DOMContentLoaded', () => {
    const cartItemsContainer = document.getElementById('cartItems');
    const cartTotalElement = document.getElementById('cartTotal');

    const checkoutBtn = document.getElementById('checkoutBtn');
    const clearCartBtn = document.getElementById('clearCartBtn');
    const checkoutForm = document.getElementById('checkoutForm'); // Form đặt hàng cuối cùng
    const backToCartBtn = document.getElementById('backToCartBtn');

    const cartSection = document.getElementById('cart-section');
    const checkoutSection = document.getElementById('checkout-section');

    const cartItemCountElement = document.getElementById('cartCount');

    // --- Hàm lấy giỏ hàng từ localStorage ---
    const getCart = () => {
        const cart = localStorage.getItem('cart');
        return cart ? JSON.parse(cart) : [];
    };

    // --- Hàm lưu giỏ hàng vào localStorage ---
    const saveCart = (cart) => {
        localStorage.setItem('cart', JSON.stringify(cart));
        updateCartItemCount(); 
    };

    // --- Hàm cập nhật số lượng sản phẩm trên icon giỏ hàng (header) ---
    const updateCartItemCount = () => {
        const cart = getCart();
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
        if (cartItemCountElement) { 
            cartItemCountElement.textContent = totalItems;
        }
    };

    // --- Helper Functions for Orders in Local Storage ---
    const ORDERS_STORAGE_KEY = 'badmintonShopOrders';

    const getOrdersFromLocalStorage = () => {
        const storedOrders = localStorage.getItem(ORDERS_STORAGE_KEY);
        return storedOrders ? JSON.parse(storedOrders) : [];
    };

    const saveOrdersToLocalStorage = (orders) => {
        localStorage.setItem(ORDERS_STORAGE_KEY, JSON.stringify(orders));
    };

    // --- Hàm render (hiển thị) giỏ hàng ---
    const renderCart = () => {
        let cart = getCart();
        cartItemsContainer.innerHTML = ''; 
        let totalAmount = 0;

        if (cart.length === 0) {
            cartItemsContainer.innerHTML = '<p style="text-align: center; color: #777;">Giỏ hàng của bạn đang trống.</p>';
            cartTotalElement.textContent = '0 VNĐ';
            checkoutBtn.disabled = true; 
            clearCartBtn.disabled = true; 
            return;
        }

        checkoutBtn.disabled = false; 
        clearCartBtn.disabled = false; 

        cart.forEach((item, index) => {
            const itemTotal = item.price * item.quantity;
            totalAmount += itemTotal;

            const cartItemDiv = document.createElement('div');
            cartItemDiv.classList.add('cart-item');
            cartItemDiv.innerHTML = `
                <img src="${item.image}" alt="${item.name}">
                <div class="cart-item-details">
                    <h4>${item.name}</h4>
                    <p>Giá: ${item.price.toLocaleString('vi-VN')} VNĐ</p>
                </div>
                <div class="cart-item-actions">
                    <input type="number" value="${item.quantity}" min="1" data-index="${index}">
                    <button class="remove-item-btn" data-index="${index}">Xóa</button>
                </div>
            `;
            cartItemsContainer.appendChild(cartItemDiv);
        });

        cartTotalElement.textContent = `${totalAmount.toLocaleString('vi-VN')} VNĐ`;

        document.querySelectorAll('.cart-item input[type="number"]').forEach(input => {
            input.addEventListener('change', (e) => {
                const index = parseInt(e.target.dataset.index);
                const newQuantity = parseInt(e.target.value);
                if (newQuantity > 0) {
                    cart[index].quantity = newQuantity;
                    saveCart(cart);
                    renderCart(); 
                } else {
                    if (confirm('Bạn có muốn xóa sản phẩm này khỏi giỏ hàng?')) {
                        cart.splice(index, 1);
                        saveCart(cart);
                        renderCart();
                    } else {
                        e.target.value = cart[index].quantity; 
                    }
                }
            });
        });

        document.querySelectorAll('.remove-item-btn').forEach(button => {
            button.addEventListener('click', (e) => {
                const index = parseInt(e.target.dataset.index);
                cart.splice(index, 1); 
                saveCart(cart); 
                renderCart(); 
            });
        });
        
    };
 
    // --- Xử lý nút "Tiến hành đặt hàng" (Chuyển sang form thanh toán) ---
    checkoutBtn.addEventListener('click', () => {
        const cart = getCart();
        if (cart.length === 0) {
            alert('Giỏ hàng của bạn đang trống! Vui lòng thêm sản phẩm trước khi thanh toán.');
            return;
        }
        cartSection.classList.add('hidden');
        checkoutSection.classList.remove('hidden');
        // Sau khi hiển thị form checkout, reset form và ẩn tất cả các thông báo lỗi
        checkoutForm.reset(); // Reset form để xóa dữ liệu cũ
        hideError(errorName, customerNameInput); // Truyền cả input
        hideError(errorPhone, customerPhoneInput); // Truyền cả input
        hideError(errorAddress, customerAddressInput); // Truyền cả input
    });
    
    // --- Xử lý nút "Quay lại giỏ hàng" ---
    backToCartBtn.addEventListener('click', () => {
        checkoutSection.classList.add('hidden');
        cartSection.classList.remove('hidden');
    });

    // --- Xử lý nút "Xóa hết giỏ hàng" ---
    clearCartBtn.addEventListener('click', () => {
        if (confirm('Bạn có chắc chắn muốn xóa tất cả sản phẩm khỏi giỏ hàng?')) {
            localStorage.removeItem('cart'); 
            renderCart(); 
            alert('Giỏ hàng đã được xóa!');
        }
    });

    // -------------------------------------------------------------
    // START: Code Validation
    // -------------------------------------------------------------
    // SỬA LỖI: Lấy đúng các element input thay vì element lỗi
    const customerNameInput = document.getElementById('customerName'); 
    const customerPhoneInput = document.getElementById('customerPhone');
    const customerAddressInput = document.getElementById('customerAddress');
    
    // Các element hiển thị thông báo lỗi (đúng rồi)
    const errorName = document.getElementById('errorName');
    const errorPhone = document.getElementById('errorPhone');
    const errorAddress = document.getElementById('errorAddress');

    // Hàm hiển thị lỗi (đã sửa để nhận thêm inputElement)
    function showError(element, message, inputElement) {
        element.textContent = message;
        element.style.display = 'block'; 
        if (inputElement) {
            inputElement.classList.add('error'); // Thêm class 'error' vào input
        }
    }

    // Hàm ẩn lỗi (đã sửa để nhận thêm inputElement)
    function hideError(element, inputElement) {
        element.style.display = 'none'; 
        if (inputElement) {
            inputElement.classList.remove('error'); // Xóa class 'error' khỏi input
        }
    }

    // Hàm kiểm tra tên (đã sửa Regex và cách gọi showError/hideError)
    function validateName() {
        const nameValue = customerNameInput.value.trim();
        // Regex cho phép chữ cái (bao gồm tiếng Việt), khoảng trắng, tối thiểu 10 ký tự
        const namePattern = /^[\p{L}\s]+$/u;

        if (nameValue === '') {
            showError(errorName, 'Vui lòng nhập họ và tên của bạn!', customerNameInput); 
            return false;
        } else if (!namePattern.test(nameValue)) {
            showError(errorName, 'Tên phải là chữ cái (có dấu), khoảng trắng và tối thiểu 10 ký tự!', customerNameInput); 
            return false;
        }
        hideError(errorName, customerNameInput); 
        return true;
    }

    // Hàm kiểm tra số điện thoại (đã sửa cách gọi showError/hideError)
    function validatePhone() {
        const phoneValue = customerPhoneInput.value.trim();
        const phonePattern = /^[0-9]{10}$/; 

        if (phoneValue === '') {
            showError(errorPhone, 'Vui lòng nhập số điện thoại!', customerPhoneInput); 
            return false;
        } else if (!phonePattern.test(phoneValue)) {
            showError(errorPhone, 'Số điện thoại phải gồm 10 chữ số!', customerPhoneInput); 
            return false;
        }
        hideError(errorPhone, customerPhoneInput); 
        return true;
    }

    // Hàm kiểm tra địa chỉ (đã sửa cách gọi showError/hideError)
    function validateAddress() {
        const addressValue = customerAddressInput.value.trim();
        if (addressValue === '') {
            showError(errorAddress, 'Vui lòng nhập địa chỉ giao hàng!', customerAddressInput); 
            return false;
        }
        hideError(errorAddress, customerAddressInput); 
        return true;
    }

    // Gắn sự kiện 'input' để kiểm tra lỗi ngay khi người dùng gõ
    customerNameInput.addEventListener('input', validateName);
    customerPhoneInput.addEventListener('input', validatePhone);
    customerAddressInput.addEventListener('input', validateAddress);

    // Ẩn các thông báo lỗi khi tải trang lần đầu (đã sửa để truyền inputElement)
    hideError(errorName, customerNameInput);
    hideError(errorPhone, customerPhoneInput);
    hideError(errorAddress, customerAddressInput);

    // -------------------------------------------------------------
    // END: Code Validation
    // -------------------------------------------------------------

    // --- Xử lý form đặt hàng (khi nhấn nút xác nhận đặt hàng cuối cùng) ---
    checkoutForm.addEventListener('submit', (e) => {
        e.preventDefault(); // Ngăn chặn form submit mặc định

        // CHẠY CÁC HÀM VALIDATE TRƯỚC KHI THỰC HIỆN LOGIC ĐẶT HÀNG
        const isNameValid = validateName();
        const isPhoneValid = validatePhone();
        const isAddressValid = validateAddress(); 

        if (!isNameValid || !isPhoneValid || !isAddressValid) {
            // alert('Vui lòng kiểm tra lại thông tin đặt hàng còn lỗi!'); // Bỏ dòng alert này
            // Cuộn đến trường bị lỗi đầu tiên
            if (!isNameValid) customerNameInput.focus();
            else if (!isPhoneValid) customerPhoneInput.focus();
            else if (!isAddressValid) customerAddressInput.focus();
            return; // Dừng lại nếu có lỗi
        }

        const name = customerNameInput.value; // Lấy giá trị từ input
        const phone = customerPhoneInput.value; // Lấy giá trị từ input
        const address = customerAddressInput.value; // Lấy giá trị từ input
        const paymentMethod = document.getElementById('paymentMethod').value;
        const currentCart = getCart(); 

        if (currentCart.length === 0) {
            alert('Giỏ hàng trống! Không thể đặt hàng.');
            return; 
        }

        const totalAmount = currentCart.reduce((sum, item) => sum + item.price * item.quantity, 0);

        const newOrder = {
            id: 'ORDER-' + Date.now(), 
            customer: { 
                name: name,
                phone: phone,
                address: address,
                paymentMethod: paymentMethod
            },
            items: currentCart.map(item => ({ 
                id: item.id,
                name: item.name,
                quantity: item.quantity,
                price: item.price
            })),
            totalAmount: totalAmount, 
            orderDate: new Date().toLocaleDateString('vi-VN') + ' ' + new Date().toLocaleTimeString('vi-VN'), 
            status: 'Đang chờ xử lý' 
        };

        console.log('Thông tin đơn hàng mới:', newOrder); 

        let allExistingOrders = getOrdersFromLocalStorage();
        allExistingOrders.push(newOrder);
        saveOrdersToLocalStorage(allExistingOrders);

        alert(`Đơn hàng của bạn đã được đặt thành công!\nTổng tiền: ${newOrder.totalAmount.toLocaleString('vi-VN')} VNĐ\nChúng tôi sẽ liên hệ bạn sớm nhất.`);

        localStorage.removeItem('cart');
        updateCartItemCount(); 
        renderCart(); 

        window.location.href = 'SanPham.html';
    });

    // --- Initial Load ---
    renderCart(); 
    updateCartItemCount(); 
});








