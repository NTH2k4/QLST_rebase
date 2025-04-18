﻿using Microsoft.Data.SqlClient;
using QLST_rebase.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLST_rebase
{
    public partial class FAddHH : Form
    {
        public FAddHH()
        {
            InitializeComponent();
        }

        private bool check = false;

        public bool TestAddHH(String tenHang, String ngayNhap, String giaTien, String soLuong, String donViTinh, String nhaCC, String loaiHang)
        {
            txtTenHang.Text = tenHang;
            DateTime parseNgayNhap = DateTime.TryParse(ngayNhap, out parseNgayNhap) ? parseNgayNhap : DateTime.Now;
            dtNgayNhap.Value = (parseNgayNhap >= dtNgayNhap.MinDate && parseNgayNhap <= dtNgayNhap.MaxDate ? parseNgayNhap : dtNgayNhap.Value);
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
            if (string.IsNullOrWhiteSpace(txtTenHang.Text))
                return "Tên hàng không được để trống.";
            if (string.IsNullOrWhiteSpace(txtGiaTien.Text))
                return "Giá tiền không được để trống.";
            if (string.IsNullOrWhiteSpace(NmrSoLuong.Text))
                return "Số lượng không được để trống.";
            if (string.IsNullOrWhiteSpace(txtDonViTinh.Text))
                return "Đơn vị tính không được để trống.";
            if (string.IsNullOrWhiteSpace(txtNhaCC.Text))
                return "Nhà cung cấp không được để trống.";
            if (string.IsNullOrWhiteSpace(cbLoaiHang.Text))
                return "Loại hàng không được để trống.";
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
            try
            {
                using (DataDBContext context = new())
                {
                    var goods = new goods
                    {
                        goodsName = txtTenHang.Text,
                        entryDate = DateOnly.Parse(dtNgayNhap.Value.ToString("dd/MM/yyyy")),
                        price = Double.Parse(txtGiaTien.Text),
                        quantity = int.Parse(NmrSoLuong.Value.ToString()),
                        unit = txtDonViTinh.Text,
                        suppiler = txtNhaCC.Text,
                        type = cbLoaiHang.Text
                    };
                    context.goodss.Add(goods);
                    check = context.SaveChanges() > 0;
                    MessageBox.Show("Thêm thành công!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin\n");
            }
        }

        private void FAddHH_Load(object sender, EventArgs e)
        {
            cbLoaiHang.SelectedItem = "Thực phẩm";
        }
    }
}

