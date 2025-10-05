document.querySelectorAll(".add-to-cart").forEach(btn => {
  btn.addEventListener("click", () => {
    const product = {
      id: btn.dataset.productId,
      name: btn.dataset.productName,
      price: parseInt(btn.dataset.productPrice),
      image: btn.dataset.productImage,
      quantity: 1
    };

    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    // kiểm tra sản phẩm đã có trong giỏ chưa
    const existing = cart.find(item => item.id === product.id);
    if (existing) {
      existing.quantity += 1;
    } else {
      cart.push(product);
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    alert("Đã thêm vào giỏ hàng!");
  });
});
function renderCart() {
  let cart = JSON.parse(localStorage.getItem("cart")) || [];
  let tbody = document.querySelector("#orderTable tbody");
  tbody.innerHTML = "";

  let total = 0;
  cart.forEach(item => {
    total += item.price * item.quantity;
    tbody.innerHTML += `
      <tr>
        <td>${item.id}</td>
        <td>${item.name}</td>
        <td>${item.quantity}</td>
        <td>${item.price.toLocaleString()}₫</td>
        <td>${(item.price * item.quantity).toLocaleString()}₫</td>
      </tr>
    `;
  });

  document.getElementById("noOrdersMessage").classList.toggle("hidden", cart.length > 0);
}
renderCart();
