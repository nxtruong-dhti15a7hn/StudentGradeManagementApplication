using StudentGradeManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentGradeManagementApplication.Forms
{
    public partial class StudentManagement : Form
    {
        private QLSinhVien qlSinhVien;

        public StudentManagement()
        {
            InitializeComponent();
            qlSinhVien = new QLSinhVien();
            LoadStudentData();
            LoadClassData();
            comboBoxClasses.SelectedIndexChanged += ComboBoxClasses_SelectedIndexChanged;
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;
            guna2GradientButton1.Click += Guna2GradientButton1_Click;
        }

        private void LoadStudentData()
        {
            DataTable dt = qlSinhVien.GetAllStudents();
            guna2DataGridView1.DataSource = dt;
        }

        private void LoadClassData()
        {
            DataTable dt = qlSinhVien.GetAllClasses();
            comboBoxClasses.DataSource = dt;
            comboBoxClasses.DisplayMember = "TenLop";
            comboBoxClasses.ValueMember = "MaLop";
        }

        private void ComboBoxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMaLop = comboBoxClasses.SelectedValue.ToString();
            DataTable dt = qlSinhVien.GetStudentsByClass(selectedMaLop);
            guna2DataGridView1.DataSource = dt;
        }

        private void Guna2GradientButton1_Click(object sender, EventArgs e)
        {
            try
            {
                qlSinhVien.SaveStudents(guna2DataGridView1);
                MessageBox.Show("Dữ liệu đã được thêm thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBoxSearch.Text.Trim();
            DataTable dt = qlSinhVien.SearchStudents(searchText);
            guna2DataGridView1.DataSource = dt;
        }
    }
}
