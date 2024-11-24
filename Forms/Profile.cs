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
    public partial class Profile : Form
    {
        private ProfileSV profileSV;
        private string studentId;

        public Profile(string studentId)
        {
            InitializeComponent();
            profileSV = new ProfileSV();
            this.studentId = studentId;
            LoadStudentInfo(studentId);
        }

        private void LoadStudentInfo(string studentId)
        {
            try
            {
                DataTable dt = profileSV.GetStudentInfo(studentId);
                if (dt.Rows.Count > 0)
                {
                    DataRow reader = dt.Rows[0];
                    lbMSSV.Text = "MSSV: " + reader["MaSV"].ToString();
                    lbHT.Text = "Họ Tên: " + reader["HoVaTen"].ToString();
                    lbNvao.Text = "Ngày Sinh: " + Convert.ToDateTime(reader["NgaySinh"]).ToString("dd/MM/yyyy");
                    lbCS.Text = "Quê: " + reader["Que"].ToString();
                    lbGT.Text = "Giới Tính: " + reader["GioiTinh"].ToString();
                    lbKH.Text = "Khóa Học: " + reader["KhoaHoc"].ToString();
                    lbCN.Text = "Ngành: " + reader["Nganh"].ToString();
                    lbEmail.Text = "Email: " + reader["Email"].ToString();
                    lbCCCD.Text = "CCCD: " + reader["CCCD"].ToString();
                    lbLop.Text = "Lớp: " + reader["TenLop"].ToString();
                    lbLH.Text = "Hệ Đào Tạo: " + reader["TenHe"].ToString();

                    string imagePath = reader["HinhAnh"].ToString();
                    string fullImagePath = Path.Combine(Application.StartupPath, imagePath);

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
                    MessageBox.Show("Không tìm thấy sinh viên với mã: " + studentId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
