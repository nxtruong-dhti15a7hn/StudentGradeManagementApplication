using StudentGradeManagementApplication.Forms;
using StudentGradeManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentGradeManagementApplication
{
    public partial class login : Form
    {
        private DangNhap dangNhap;

        public login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Load += new EventHandler(Form_Load);
            dangNhap = new DangNhap();
            txtPassword.PasswordChar = '*'; // Hiển thị ký tự * khi nhập mật khẩu
            LoadLoginInfo();                // Load thông tin đăng nhập nếu đã lưu
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Không cần thiết phải làm gì trong Form_Load cho việc bo góc
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int radius = 40; // Bán kính của góc bo tròn
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            try
            {
                User user = dangNhap.AuthenticateUser(username, password);
                if (user != null)
                {
                    MessageBox.Show("Đăng nhập thành công! Loại người dùng: " + user.UserType,
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    // Kiểm tra loại người dùng và điều hướng đến form tương ứng
                    if (user.UserType == "Student")
                    {
                        // Lấy MaSV (studentId) từ cơ sở dữ liệu
                        string studentId = dangNhap.GetStudentId(username);
                        if (studentId != null)
                        {
                            // Mở form MenuStudent
                            MenuStudent menuStudentForm = new MenuStudent(studentId);
                            menuStudentForm.Show();
                        }
                    }
                    else if (user.UserType == "Teacher")
                    {
                        // Lấy MaGV (teacherId) từ cơ sở dữ liệu
                        string teacherId = dangNhap.GetTeacherId(username);
                        if (teacherId != null)
                        {
                            // Mở form MenuTeacher
                            MenuTeacher menuTeacherForm = new MenuTeacher(teacherId);
                            menuTeacherForm.Show();
                        }
                    }
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không hợp lệ.",
                                    "Lỗi đăng nhập",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void SaveLoginInfo(string username, string password)
        {
            Properties.Settings.Default.Username = username;
            Properties.Settings.Default.Password = password;
            Properties.Settings.Default.RememberMe = true;
            Properties.Settings.Default.Save();
        }

        private void ClearLoginInfo()
        {
            Properties.Settings.Default.Username = string.Empty;
            Properties.Settings.Default.Password = string.Empty;
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
        }

        private void LoadLoginInfo()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Properties.Settings.Default.Password;
                chkRememberMe.Checked = true;
            }
        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Đảm bảo rằng ứng dụng sẽ thoát khi form đăng nhập bị đóng
            Application.Exit();
        }
    }
}
