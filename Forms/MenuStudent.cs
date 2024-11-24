using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentGradeManagementApplication.Forms
{
    public partial class MenuStudent : Form
    {
        private string studentId;
        private Form currentChildForm;

        public MenuStudent(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            // Mở form Home với studentId
            openChildForm(new Home(studentId));
        }

        public void openChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnBody.Controls.Clear(); // Clear existing controls
            pnBody.Controls.Add(childForm);
            pnBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            openChildForm(new Home(studentId));
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            openChildForm(new Profile(studentId));
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            // Hiển thị thông báo xác nhận đăng xuất
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Đóng form hiện tại
                this.Close();

                // Mở lại form đăng nhập
                login loginForm = new login();
                loginForm.Show();
            }
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            openChildForm(new Transcript(studentId));
        }

        private void LoadStudentInfo(string studentId)
        {
            // Hàm để tải thông tin sinh viên dựa trên studentId
        }
    }
}
