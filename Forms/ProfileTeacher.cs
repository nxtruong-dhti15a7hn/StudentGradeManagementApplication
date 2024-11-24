using StudentGradeManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentGradeManagementApplication.Forms
{
    public partial class ProfileTeacher : Form
    {
        private string maGV; // This should be set to the logged-in user's MaGV
        private ProfileGV profileGV;

        public ProfileTeacher(string maGV)
        {
            InitializeComponent();
            this.maGV = maGV;
            this.profileGV = new ProfileGV();
            LoadTeacherProfile();
        }

        private void LoadTeacherProfile()
        {
            try
            {
                DataTable dt = profileGV.GetTeacherProfile(maGV);
                if (dt.Rows.Count > 0)
                {
                    DataRow reader = dt.Rows[0];
                    lbMaGV.Text = "Mã giáo viên: " + reader["MaGV"].ToString();
                    lbHoTen.Text = "Họ và tên: " + reader["HoVaTen"].ToString();
                    lbKhoa.Text = "Khoa: " + reader["Khoa"].ToString();
                    lbViTriCongTac.Text = "Vị trí công tác: " + reader["ViTriCongTac"].ToString();

                    string imagePath = reader["HinhAnh"].ToString();
                    string fullImagePath = Path.Combine(Application.StartupPath, imagePath);

                    // In ra đường dẫn đầy đủ để kiểm tra
                    Console.WriteLine("Full Image Path: " + fullImagePath);

                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(fullImagePath))
                    {
                        pictureBoxHinhAnh.Image = Image.FromFile(fullImagePath);
                    }
                    else
                    {
                        pictureBoxHinhAnh.Image = null; // Hoặc đặt ảnh mặc định
                        MessageBox.Show("File ảnh không tồn tại: " + fullImagePath);
                    }

                    lblChucDanh.Text = "Chức danh: " + reader["ChucDanh"].ToString();
                    lblBangCap.Text = "Bằng cấp: " + reader["BangCap"].ToString();
                    lblLinhVuc.Text = "Lĩnh vực: " + reader["LinhVuc"].ToString();
                    lblCongTrinh.Text = "Công trình: " + reader["CongTrinh"].ToString();
                    lblQueQuan.Text = "Quê quán: " + reader["QueQuan"].ToString();
                    lblLichSuCT.Text = "Lịch sử công tác: " + reader["LichSuCT"].ToString();
                    lblDuAn.Text = "Dự án: " + reader["DuAn"].ToString();
                    lblHoatDong.Text = "Hoạt động: " + reader["HoatDong"].ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy giáo viên với mã: " + maGV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải thông tin giáo viên: " + ex.Message);
            }
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom paint logic if needed
        }
    }
}
