// Thêm vào cuối <script> trong tintuc.js hoặc trong file hiện tại

const newsData = [
  {
    img: 'image_Kumpoo/Kumpoo (1).webp',
    date: '07/06/2025',
    views: '1079',
    title: 'Top 5 Vợt Cầu Lông Kumpoo Giá Rẻ Mà Bạn Không Thể Bỏ Qua',
    desc: 'Bạn đang tìm kiếm một cây vợt chất lượng với mức giá phải chăng?...'
  },
  {
    img: 'image_Lining/Lining (1).webp',
    date: '07/06/2025',
    views: '898',
    title: 'Top 5 vợt cầu lông Lining cao cấp trên 3 triệu',
    desc: 'FBShop sẽ giới thiệu đến bạn top 5 vợt cầu lông Lining cao cấp...'
  },
  {
    img: 'image_Yonex/Lining (6).webp',
    date: '10/06/2025',
    views: 'N/A',
    title: 'Các Thương Hiệu Giày Cầu Lông Hàng Đầu Và Sản Phẩm Nổi Bật',
    desc: 'Trong thế giới cầu lông, việc chọn một đôi giày phù hợp rất quan trọng...'
  }
];

let currentSlide = 0;
let slideTimer;

function renderSlide(index) {
  const news = newsData[index];
  document.getElementById('newsSlide').innerHTML = `
    <img class="news-image" src="${news.img}" alt="${news.title}" />
    <div class="news-content">
      <div class="news-meta">📅 ${news.date} &nbsp; 👁 ${news.views}</div>
      <div class="news-title">${news.title}</div>
      <p class="news-text mt-2">${news.desc}</p>
    </div>
  `;
}

function changeSlide(dir) {
  currentSlide = (currentSlide + dir + newsData.length) % newsData.length;
  renderSlide(currentSlide);
}

function autoSlide() {
  slideTimer = setInterval(() => {
    changeSlide(1);
  }, 5000);
}

document.addEventListener("DOMContentLoaded", () => {
  renderSlide(currentSlide);
  autoSlide();
});
document.addEventListener("DOMContentLoaded", () => {
  renderSlide(currentSlide);
  autoSlide();

  // Gắn sự kiện click cho slide chuyển sang trang tintucgia.html
  const slide = document.getElementById('newsSlide');
  slide.style.cursor = 'pointer';
  slide.addEventListener('click', () => {
    window.location.href = 'tintucgia.html';
  });
});
