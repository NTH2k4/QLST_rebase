﻿using QLST_rebase.DAO;
using System.Globalization;

namespace QLST_rebase
{
    public partial class FEditHH : Form
    {
        public FEditHH()
        {
            InitializeComponent();
        }
        public void getId(string a)
        {
            txtMaHang.Text = a;
        }
        private void FEditHH_Load(object sender, EventArgs e)
        {
            using (DataDBContext context = new())
            {

                var item = context.goodss.FirstOrDefault(p => p.goodsId == int.Parse(txtMaHang.Text));
                if (item != null)
                {
                    txtTenHang.Text = item.goodsName.ToString();
                    dtNgayNhap.Value = DateTime.Parse(item.entryDate.ToString());
                    txtGiaTien.Text = item.price.ToString();
                    NmrSoLuong.Value = int.Parse(item.quantity.ToString());
                    txtDonViTinh.Text = item.unit.ToString();
                    txtNhaCC.Text = item.suppiler.ToString();
                    cbLoaiHang.SelectedItem = item.type.ToString();
                }
            }
        }

        private bool check = false;

        public bool TestEditHH(int searchId, String tenHang, String ngayNhap, String giaTien, String soLuong, String donViTinh, String nhaCC, String loaiHang)
        {
            getId(searchId.ToString());
            txtTenHang.Text = tenHang;
            DateTime parseNgayNhap = DateTime.TryParse(ngayNhap, out parseNgayNhap) ? parseNgayNhap : dtNgayNhap.Value;
            if (parseNgayNhap >= dtNgayNhap.MinDate && parseNgayNhap <= dtNgayNhap.MaxDate)
            {
                dtNgayNhap.Value = parseNgayNhap;
            }
            else
            {
                dtNgayNhap.Tag = parseNgayNhap;
            }
            txtGiaTien.Text = giaTien;
            decimal parsedSoLuong = decimal.TryParse(soLuong, out parsedSoLuong) ? parsedSoLuong : NmrSoLuong.Value;
            NmrSoLuong.Value = (parsedSoLuong >= NmrSoLuong.Minimum && parsedSoLuong <= NmrSoLuong.Maximum ? parsedSoLuong : NmrSoLuong.Value);
            txtDonViTinh.Text = donViTinh;
            txtNhaCC.Text = nhaCC;
            if (cbLoaiHang.Items.Contains(loaiHang))
                cbLoaiHang.SelectedItem = loaiHang;
            else
            {
                cbLoaiHang.Items.Add(loaiHang);
                cbLoaiHang.SelectedItem = loaiHang;
            }
            //MessageBox.Show(tenHang + "\n" + ngayNhap + "\n" + giaTien + "\n" + soLuong + "\n" + donViTinh + "\n" + nhaCC + "\n" + loaiHang);
            //MessageBox.Show(txtTenHang.Text + "\n" + dtNgayNhap.Value + "\n" + txtGiaTien.Text + "\n" + NmrSoLuong.Value + "\n" + txtDonViTinh.Text + "\n" + txtNhaCC.Text + "\n" + cbLoaiHang.Text);
            btnConfirm_Click(null, null);
            return check;
        }

        private string ValidateInputs()
        {
            if (txtTenHang.Text.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
            {
                return "Tên hàng không được chứa ký tự đặc biệt.";
            }

            if (string.IsNullOrWhiteSpace(txtTenHang.Text))
            {
                return "Tên hàng không được để trống.";
            }

            if (txtTenHang.Text.Length < 2 || txtTenHang.Text.Length > 30)
            {
                return "Tên hàng phải từ 2 đến 30 ký tự.";
            }

            if (dtNgayNhap.Tag != null || dtNgayNhap.Value < dtNgayNhap.MinDate || dtNgayNhap.Value > dtNgayNhap.MaxDate || dtNgayNhap.Value > DateTime.Now)
            {
                return "Ngày nhập không hợp lệ.";
            }

            if (string.IsNullOrWhiteSpace(txtGiaTien.Text) || !double.TryParse(txtGiaTien.Text, out double parsedPrice))
            {
                return "Giá tiền không hợp lệ.";
            }

            if (!double.TryParse(txtGiaTien.Text, out double price) || price <= 0)
            {
                return "Giá tiền phải lớn hơn 0.";
            }

            if (price < 1000 || price > 100000000)
            {
                return "Giá tiền phải nằm trong khoảng từ 1.000 đến 100.000.000.";
            }

            if (!int.TryParse(NmrSoLuong.Value.ToString(), out int quantity))
            {
                return "Số lượng không hợp lệ.";
            }

            if (NmrSoLuong.Value <= 0)
            {
                return "Số lượng phải lớn hơn 0.";
            }

            if (txtDonViTinh.Text.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
            {
                return "Đơn vị tính không được chứa ký tự đặc biệt.";
            }

            if (string.IsNullOrWhiteSpace(txtDonViTinh.Text))
            {
                return "Đơn vị tính không được để trống.";
            }

            if (txtDonViTinh.Text.Length < 2 || txtDonViTinh.Text.Length > 30)
            {
                return "Đơn vị tính phải từ 2 đến 30 ký tự.";
            }

            if (txtNhaCC.Text.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
            {
                return "Nhà cung cấp không được chứa ký tự đặc biệt.";
            }

            if (string.IsNullOrWhiteSpace(txtNhaCC.Text))
            {
                return "Nhà cung cấp không được để trống.";
            }

            if (txtNhaCC.Text.Length < 2 || txtNhaCC.Text.Length > 30)
            {
                return "Nhà cung cấp phải từ 2 đến 30 ký tự.";
            }

            if (string.IsNullOrWhiteSpace(cbLoaiHang.Text))
            {
                return "Loại hàng không được để trống.";
            }

            return string.Empty;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string error = ValidateInputs();
            if (!string.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error, "Thông báo");
                return;
            }
            using (DataDBContext context = new())
            {
                goods goods = context.goodss.FirstOrDefault(p => p.goodsId == int.Parse(txtMaHang.Text));
                if (goods != null)
                {
                    try
                    {
                        goods.goodsName = txtTenHang.Text;
                        goods.entryDate = DateOnly.Parse(dtNgayNhap.Value.ToString("dd/MM/yyyy"));
                        goods.price = double.Parse(txtGiaTien.Text);
                        goods.quantity = int.Parse(NmrSoLuong.Value.ToString());
                        goods.unit = txtDonViTinh.Text;
                        goods.suppiler = txtNhaCC.Text;
                        goods.type = cbLoaiHang.Text;
                        check = context.SaveChanges() > 0;
                        MessageBox.Show("Sửa thành công!", "Thông báo");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Sửa không thành công!\nLỗi: " + ex.Message, "Thông báo");
                        return;
                    }
                }
            }
        }
    }
}
