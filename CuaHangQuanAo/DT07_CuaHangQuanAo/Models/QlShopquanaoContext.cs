using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DT07_CuaHangQuanAo.Models;

public partial class QlShopquanaoContext : DbContext
{
    public QlShopquanaoContext()
    {
    }

    public QlShopquanaoContext(DbContextOptions<QlShopquanaoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DiaChi> DiaChis { get; set; }

    public virtual DbSet<DiaChiNguoiDung> DiaChiNguoiDungs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GiamGium> GiamGia { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<HangHoaBienThe> HangHoaBienThes { get; set; }

    public virtual DbSet<HinhAnhBienThe> HinhAnhBienThes { get; set; }

    public virtual DbSet<KichThuoc> KichThuocs { get; set; }

    public virtual DbSet<LoaiHang> LoaiHangs { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }

    public virtual DbSet<ThuongHieu> ThuongHieus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.IddonHang, e.IdbienThe }).HasName("PK__ChiTiet___55E4086429884F13");

            entity.ToTable("ChiTiet_DonHang");

            entity.Property(e => e.IddonHang).HasColumnName("IDDonHang");
            entity.Property(e => e.IdbienThe).HasColumnName("IDBienThe");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdbienTheNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.IdbienThe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTiet_D__IDBie__02084FDA");

            entity.HasOne(d => d.IddonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.IddonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTiet_D__IDDon__01142BA1");
        });

        modelBuilder.Entity<DiaChi>(entity =>
        {
            entity.HasKey(e => e.IddiaChi).HasName("PK__DiaChi__7B67D63A17C94B78");

            entity.ToTable("DiaChi");

            entity.Property(e => e.IddiaChi).HasColumnName("IDDiaChi");
            entity.Property(e => e.ChiTiet).HasMaxLength(255);
            entity.Property(e => e.PhuongXa).HasMaxLength(100);
            entity.Property(e => e.QuanHuyen).HasMaxLength(100);
            entity.Property(e => e.TinhThanh).HasMaxLength(100);
        });

        modelBuilder.Entity<DiaChiNguoiDung>(entity =>
        {
            entity.HasKey(e => new { e.IdnguoiDung, e.IddiaChi }).HasName("PK__DiaChi_N__4B61A66A50E7D0D1");

            entity.ToTable("DiaChi_NguoiDung");

            entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");
            entity.Property(e => e.IddiaChi).HasColumnName("IDDiaChi");
            entity.Property(e => e.LaMacDinh).HasDefaultValue(false);

            entity.HasOne(d => d.IddiaChiNavigation).WithMany(p => p.DiaChiNguoiDungs)
                .HasForeignKey(d => d.IddiaChi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiaChi_Ng__IDDia__6754599E");

            entity.HasOne(d => d.IdnguoiDungNavigation).WithMany(p => p.DiaChiNguoiDungs)
                .HasForeignKey(d => d.IdnguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiaChi_Ng__IDNgu__66603565");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.IddonHang).HasName("PK__DonHang__9CA232F79C19626C");

            entity.ToTable("DonHang");

            entity.Property(e => e.IddonHang).HasColumnName("IDDonHang");
            entity.Property(e => e.DiaChiGiao).HasMaxLength(500);
            entity.Property(e => e.GiamGia)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdgiamGia).HasColumnName("IDGiamGia");
            entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");
            entity.Property(e => e.IdphuongThucThanhToan).HasColumnName("IDPhuongThucThanhToan");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhiVanChuyen)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNguoiNhan).HasMaxLength(100);
            entity.Property(e => e.TongThanhToan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TongTienHang).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdgiamGiaNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.IdgiamGia)
                .HasConstraintName("FK__DonHang__IDGiamG__7D439ABD");

            entity.HasOne(d => d.IdnguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.IdnguoiDung)
                .HasConstraintName("FK__DonHang__IDNguoi__7C4F7684");

            entity.HasOne(d => d.IdphuongThucThanhToanNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.IdphuongThucThanhToan)
                .HasConstraintName("FK__DonHang__IDPhuon__7E37BEF6");
        });

        modelBuilder.Entity<GiamGium>(entity =>
        {
            entity.HasKey(e => e.IdgiamGia).HasName("PK__GiamGia__F091CBDC73B9A887");

            entity.HasIndex(e => e.MaGiamGia, "UQ__GiamGia__EF9458E52B0493A3").IsUnique();

            entity.Property(e => e.IdgiamGia).HasColumnName("IDGiamGia");
            entity.Property(e => e.DonHangToiThieu)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaTri).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiamToiDa).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaGiamGia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TenChuongTrinh).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => new { e.IdnguoiDung, e.IdbienThe }).HasName("PK__GioHang__3591E19AC13A869F");

            entity.ToTable("GioHang");

            entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");
            entity.Property(e => e.IdbienThe).HasColumnName("IDBienThe");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.IdbienTheNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.IdbienThe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GioHang__IDBienT__76969D2E");

            entity.HasOne(d => d.IdnguoiDungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.IdnguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GioHang__IDNguoi__75A278F5");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.Idhang).HasName("PK__HangHoa__9CEFB0483B68DB01");

            entity.ToTable("HangHoa");

            entity.Property(e => e.Idhang).HasColumnName("IDHang");
            entity.Property(e => e.IdloaiHang).HasColumnName("IDLoaiHang");
            entity.Property(e => e.IdthuongHieu).HasColumnName("IDThuongHieu");
            entity.Property(e => e.MoTa).HasColumnType("ntext");
            entity.Property(e => e.TenHang).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.IdloaiHangNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.IdloaiHang)
                .HasConstraintName("FK__HangHoa__IDLoaiH__619B8048");

            entity.HasOne(d => d.IdthuongHieuNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.IdthuongHieu)
                .HasConstraintName("FK__HangHoa__IDThuon__628FA481");
        });

        modelBuilder.Entity<HangHoaBienThe>(entity =>
        {
            entity.HasKey(e => e.IdbienThe).HasName("PK__HangHoa___9463A93DF3620830");

            entity.ToTable("HangHoa_BienThe");

            entity.HasIndex(e => e.Sku, "UQ__HangHoa___CA1ECF0D38669B73").IsUnique();

            entity.Property(e => e.IdbienThe).HasColumnName("IDBienThe");
            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaKhuyenMai).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Idhang).HasColumnName("IDHang");
            entity.Property(e => e.IdkichThuoc).HasColumnName("IDKichThuoc");
            entity.Property(e => e.IdmauSac).HasColumnName("IDMauSac");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SKU");

            entity.HasOne(d => d.IdhangNavigation).WithMany(p => p.HangHoaBienThes)
                .HasForeignKey(d => d.Idhang)
                .HasConstraintName("FK__HangHoa_B__IDHan__6C190EBB");

            entity.HasOne(d => d.IdkichThuocNavigation).WithMany(p => p.HangHoaBienThes)
                .HasForeignKey(d => d.IdkichThuoc)
                .HasConstraintName("FK__HangHoa_B__IDKic__6E01572D");

            entity.HasOne(d => d.IdmauSacNavigation).WithMany(p => p.HangHoaBienThes)
                .HasForeignKey(d => d.IdmauSac)
                .HasConstraintName("FK__HangHoa_B__IDMau__6D0D32F4");
        });

        modelBuilder.Entity<HinhAnhBienThe>(entity =>
        {
            entity.HasKey(e => e.IdhinhAnh).HasName("PK__HinhAnh___2B573EE8DD67BEA9");

            entity.ToTable("HinhAnh_BienThe");

            entity.Property(e => e.IdhinhAnh).HasColumnName("IDHinhAnh");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IdbienThe).HasColumnName("IDBienThe");
            entity.Property(e => e.LaAnhChinh).HasDefaultValue(false);

            entity.HasOne(d => d.IdbienTheNavigation).WithMany(p => p.HinhAnhBienThes)
                .HasForeignKey(d => d.IdbienThe)
                .HasConstraintName("FK__HinhAnh_B__IDBie__71D1E811");
        });

        modelBuilder.Entity<KichThuoc>(entity =>
        {
            entity.HasKey(e => e.IdkichThuoc).HasName("PK__KichThuo__CEC1D5045C49AF2F");

            entity.ToTable("KichThuoc");

            entity.Property(e => e.IdkichThuoc).HasColumnName("IDKichThuoc");
            entity.Property(e => e.TenKichThuoc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoaiHang>(entity =>
        {
            entity.HasKey(e => e.IdloaiHang).HasName("PK__LoaiHang__93B2345A129E2966");

            entity.ToTable("LoaiHang");

            entity.Property(e => e.IdloaiHang).HasColumnName("IDLoaiHang");
            entity.Property(e => e.TenLoaiHang).HasMaxLength(100);
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.IdmauSac).HasName("PK__MauSac__43136EAE652474C7");

            entity.ToTable("MauSac");

            entity.Property(e => e.IdmauSac).HasColumnName("IDMauSac");
            entity.Property(e => e.MaMauHex)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.TenMau).HasMaxLength(50);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.IdnguoiDung).HasName("PK__NguoiDun__FCD7DB09708702F6");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.SoDienThoai, "UQ__NguoiDun__0389B7BD7D256518").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D1053474D7BDF8").IsUnique();

            entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.LoaiTk)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Customer")
                .HasColumnName("LoaiTK");
            entity.Property(e => e.MatKhauHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhuongThucThanhToan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhuongTh__3214EC27EFABE1CA");

            entity.ToTable("PhuongThucThanhToan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenPhuongThuc).HasMaxLength(100);
        });

        modelBuilder.Entity<ThuongHieu>(entity =>
        {
            entity.HasKey(e => e.IdthuongHieu).HasName("PK__ThuongHi__D4ADEAC83EC66B09");

            entity.ToTable("ThuongHieu");

            entity.Property(e => e.IdthuongHieu).HasColumnName("IDThuongHieu");
            entity.Property(e => e.TenThuongHieu).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
