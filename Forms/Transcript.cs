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
    public partial class Transcript : Form
    {
        private DiemSV diemSV;
        private string studentId;

        public Transcript(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            diemSV = new DiemSV();
        }

        private void Transcript_Load(object sender, EventArgs e)
        {
            LoadHocKi();
            LoadTranscriptData(null, studentId); // Load all records for the student
        }

        private void LoadHocKi()
        {
            try
            {
                DataTable dt = diemSV.GetHocKi();

                // Add an empty row at the beginning
                DataRow emptyRow = dt.NewRow();
                emptyRow["MaHK"] = DBNull.Value; // or set it to a specific value like string.Empty
                emptyRow["TenHK"] = "All"; // Display text for the empty item
                dt.Rows.InsertAt(emptyRow, 0);

                guna2ComboBox1.DataSource = dt;
                guna2ComboBox1.DisplayMember = "TenHK";
                guna2ComboBox1.ValueMember = "MaHK";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadTranscriptData(string maHK, string maSV)
        {
            try
            {
                DataTable dt = diemSV.GetTranscriptData(maHK, maSV);

                // Clear old data
                guna2DataGridView1.DataSource = null;
                guna2DataGridView1.Rows.Clear();
                guna2DataGridView1.Columns.Clear();

                guna2DataGridView1.DataSource = dt;

                guna2DataGridView1.Columns["MaSV"].HeaderText = "Mã sinh viên";
                guna2DataGridView1.Columns["MaMon"].HeaderText = "Mã môn học";
                guna2DataGridView1.Columns["TenMon"].HeaderText = "Tên môn học";
                guna2DataGridView1.Columns["MaHK"].HeaderText = "Mã học kỳ";
                guna2DataGridView1.Columns["D1_1"].HeaderText = "Điểm HS1";
                guna2DataGridView1.Columns["D1_2"].HeaderText = "Điểm HS1";
                guna2DataGridView1.Columns["D2_1"].HeaderText = "Điểm HS2";
                guna2DataGridView1.Columns["D2_2"].HeaderText = "Điểm HS3";
                guna2DataGridView1.Columns["DiemTB"].HeaderText = "Điểm tổng kết";

                guna2DataGridView1.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedValue != null && guna2ComboBox1.SelectedValue is DataRowView)
            {
                string selectedMaHK = ((DataRowView)guna2ComboBox1.SelectedValue)["MaHK"].ToString();
                LoadTranscriptData(selectedMaHK, studentId);
            }
            else if (guna2ComboBox1.SelectedValue != null)
            {
                string selectedMaHK = guna2ComboBox1.SelectedValue.ToString();
                LoadTranscriptData(selectedMaHK, studentId);
            }
            else
            {
                LoadTranscriptData(null, studentId);
            }
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchValue = textBoxSearch.Text.ToLower();
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[guna2DataGridView1.DataSource];

                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Check if the row is not a new row
                    {
                        if (row.Cells["TenMon"].Value != null && row.Cells["TenMon"].Value.ToString().ToLower().Contains(searchValue))
                        {
                            row.Visible = true;
                        }
                        else
                        {
                            // Temporarily move the currency manager to a different row
                            if (currencyManager.Position == row.Index)
                            {
                                currencyManager.Position = (row.Index == 0) ? 1 : row.Index - 1;
                            }
                            row.Visible = false;
                        }
                    }
                }
            }
        }
    }
}
