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
    public partial class Home : Form
    {
        private HomeSV homeSV;
        private string studentId;

        public Home(string studentId)
        {
            InitializeComponent();
            homeSV = new HomeSV();
            this.studentId = studentId;
            LoadStudentInfo(studentId);
            LoadSemesters();

            // Ẩn guna2VProgressBar1 và label6 cũ
            guna2VProgressBar1.Visible = false;
            label6.Visible = false;

            // Thêm sự kiện SelectedIndexChanged cho ComboBox
            guna2ComboBox1.SelectedIndexChanged += Guna2ComboBox1_SelectedIndexChanged;
        }

        // Load thông tin sinh viên
        private void LoadStudentInfo(string studentId)
        {
            try
            {
                DataTable dt = homeSV.GetStudentInfo(studentId);
                if (dt.Rows.Count > 0)
                {
                    DataRow reader = dt.Rows[0];
                    lblMSSV.Text = "MaSV: " + reader["MaSV"].ToString();
                    lblHoTen.Text = "Họ Tên: " + reader["HoVaTen"].ToString();
                    lblLop.Text = "Lớp: " + reader["Lop"].ToString();
                    lblTenHe.Text = "Tên Hệ: " + reader["TenHe"].ToString();
                    lblNganh.Text = "Ngành: " + reader["Nganh"].ToString();

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
                    MessageBox.Show("Không tìm thấy sinh viên với mã: " + studentId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Load điểm trung bình
        private void LoadSemesters()
        {
            try
            {
                DataTable dt = homeSV.GetSemesters();
                foreach (DataRow row in dt.Rows)
                {
                    string value = row["TenHK"].ToString();
                    Console.WriteLine("Đã lấy TenHK: " + value);
                    guna2ComboBox1.Items.Add(value);
                }

                if (guna2ComboBox1.Items.Count > 0)
                {
                    guna2ComboBox1.SelectedIndex = 0;
                    Console.WriteLine("Đã chọn mục đầu tiên trong ComboBox.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Load môn học
        private void LoadSubjectsAndProgressBars(string value)
        {
            try
            {
                DataTable dt = homeSV.GetSubjectsAndScores(studentId, value);

                // Xóa các điều khiển động cũ
                var controlsToRemove = new List<Control>();
                controlsToRemove.AddRange(guna2CustomGradientPanel3.Controls.OfType<Label>().Where(l => l.Tag != null));
                controlsToRemove.AddRange(guna2CustomGradientPanel3.Controls.OfType<Guna.UI2.WinForms.Guna2VProgressBar>().Where(pb => pb.Tag != null));
                foreach (var control in controlsToRemove)
                {
                    guna2CustomGradientPanel3.Controls.Remove(control);
                }

                int xOffset = 20; // Khoảng cách x ban đầu
                int yOffset = 20; // Khoảng cách y ban đầu
                int controlSpacingX = 60; // Khoảng cách giữa các điều khiển theo chiều ngang
                int controlSpacingY = 60; // Khoảng cách giữa các hàng
                int maxColumns = 4; // Số cột tối đa

                int currentColumn = 0; // Cột hiện tại

                foreach (DataRow reader in dt.Rows)
                {
                    // Tạo và thêm Label
                    Label label = new Label();
                    label.Text = reader["TenMon"].ToString();
                    label.Location = new Point(label6.Location.X + xOffset, label6.Location.Y + yOffset);
                    label.AutoSize = label6.AutoSize;
                    label.Font = label6.Font;
                    label.ForeColor = label6.ForeColor;
                    label.BackColor = label6.BackColor;
                    label.Visible = false; // Ẩn Label ban đầu
                    label.Tag = "dynamic"; // Đánh dấu là điều khiển động
                    guna2CustomGradientPanel3.Controls.Add(label);

                    // Tạo và thêm ProgressBar
                    Guna.UI2.WinForms.Guna2VProgressBar progressBar = new Guna.UI2.WinForms.Guna2VProgressBar();
                    progressBar.Location = new Point(guna2VProgressBar1.Location.X + xOffset, guna2VProgressBar1.Location.Y + yOffset);
                    progressBar.Size = guna2VProgressBar1.Size;
                    progressBar.FillColor = guna2VProgressBar1.FillColor;
                    progressBar.ProgressColor = guna2VProgressBar1.ProgressColor;
                    progressBar.ProgressColor2 = guna2VProgressBar1.ProgressColor2;
                    progressBar.ShadowDecoration.Parent = guna2VProgressBar1.ShadowDecoration.Parent;
                    progressBar.Visible = true;
                    progressBar.Tag = "dynamic"; // Đánh dấu là điều khiển động

                    if (reader["DiemTB"] != DBNull.Value)
                    {
                        double averageScore;
                        bool isParsed = double.TryParse(reader["DiemTB"].ToString(), out averageScore);
                        if (isParsed)
                        {
                            int percentage = (int)(averageScore * 10); // Chuyển đổi điểm trung bình hệ 10 thành phần trăm
                            progressBar.Value = percentage;
                            Console.WriteLine($"DiemTB: {averageScore}, Giá trị ProgressBar: {percentage}");
                        }
                        else
                        {
                            progressBar.Value = 0;
                            Console.WriteLine("Không thể chuyển đổi DiemTB.");
                        }
                    }
                    else
                    {
                        progressBar.Value = 0;
                        Console.WriteLine("DiemTB là NULL.");
                    }

                    // Thêm sự kiện MouseEnter và MouseLeave cho ProgressBar
                    progressBar.MouseEnter += (s, e) => { label.Visible = true; };
                    progressBar.MouseLeave += (s, e) => { label.Visible = false; };

                    guna2CustomGradientPanel3.Controls.Add(progressBar);

                    // Tăng xOffset để đặt điều khiển tiếp theo
                    xOffset += controlSpacingX;
                    currentColumn++;

                    // Nếu đạt đến số cột tối đa, chuyển sang hàng tiếp theo
                    if (currentColumn >= maxColumns)
                    {
                        currentColumn = 0;
                        xOffset = 20; // Đặt lại xOffset cho hàng mới
                        yOffset += controlSpacingY; // Tăng yOffset để đặt điều khiển ở hàng tiếp theo
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem != null)
            {
                string selectedValue = guna2ComboBox1.SelectedItem.ToString();
                Console.WriteLine("Giá trị đã chọn: " + selectedValue);
                LoadSubjectsAndProgressBars(selectedValue);
            }
        }
    }
}
