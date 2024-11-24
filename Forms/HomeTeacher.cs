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
    public partial class HomeTeacher : Form
    {
        private HomeGV homeGV;
        private string teacherId;

        public HomeTeacher(string teacherId)
        {
            InitializeComponent();
            homeGV = new HomeGV();
            this.teacherId = teacherId;
            LoadTeacherInfo(teacherId);
        }

        // Load thông tin giáo viên
        private void LoadTeacherInfo(string teacherId)
        {
            try
            {
                DataTable dt = homeGV.GetTeacherInfo(teacherId);
                if (dt.Rows.Count > 0)
                {
                    DataRow reader = dt.Rows[0];
                    lblMaGV.Text = "MaGV: " + reader["MaGV"].ToString();
                    lblHoTen.Text = "Họ Tên: " + reader["HoVaTen"].ToString();
                    lblKhoa.Text = "Khoa: " + reader["Khoa"].ToString();
                    lblViTriCongTac.Text = "Vị Trí Công Tác: " + reader["ViTriCongTac"].ToString();

                    string imagePath = reader["HinhAnh"].ToString();
                    string fullImagePath = Path.Combine(Application.StartupPath, imagePath);

                    // In ra đường dẫn đầy đủ để kiểm tra
                    Console.WriteLine("Full Image Path: " + fullImagePath);

                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(fullImagePath))
                    {
                        guna2PictureBox1.Image = Image.FromFile(fullImagePath);
                    }
                    else
                    {
                        guna2PictureBox1.Image = null; // Hoặc đặt ảnh mặc định
                        MessageBox.Show("File ảnh không tồn tại: " + fullImagePath);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy giáo viên với mã: " + teacherId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải thông tin giáo viên: " + ex.Message);
            }
        }
    }
}
