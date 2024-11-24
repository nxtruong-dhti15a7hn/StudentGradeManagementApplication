using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace StudentGradeManagementApplication.Forms
{
    public partial class MenuTeacher : Form
    {
        private string teacherId;

        public MenuTeacher(string teacherId)
        {
            InitializeComponent();
            this.teacherId = teacherId;
            openChildForm(new HomeTeacher(teacherId));
        }

        private Form currentChildForm;

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
            pnBody2.Controls.Clear(); // Clear existing controls
            pnBody2.Controls.Add(childForm);
            pnBody2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
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

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            openChildForm(new HomeTeacher(teacherId));
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            openChildForm(new ProfileTeacher(teacherId));
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            openChildForm(new Grade());
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            openChildForm(new StudentManagement());
        }

        private void guna2ImageButton6_Click(object sender, EventArgs e)
        {
            openChildForm(new SubjectManagement());
        }
    }
}
