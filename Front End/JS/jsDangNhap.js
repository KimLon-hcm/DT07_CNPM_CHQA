
  document.getElementById("registerForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const username = document.getElementById("username").value.trim();
    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value.trim();
    const confirmPassword = document.getElementById("confirmPassword").value.trim();

    if (!username || !email || !password || !confirmPassword) {
      alert("Vui lòng điền đầy đủ tất cả các trường.");
      return;
    }

    if (password !== confirmPassword) {
      alert("Mật khẩu xác nhận không khớp!");
      return;
    }

    // Lưu thông tin đăng ký vào localStorage
    const user = {
      username: username,
      email: email,
      password: password
    };

    localStorage.setItem("user_" + username, JSON.stringify(user));

    alert("Đăng ký thành công! Dữ liệu đã lưu trên trình duyệt.");
    this.reset();
  });

