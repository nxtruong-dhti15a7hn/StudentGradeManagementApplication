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
    public partial class Grade : Form
    {
        private Connect dbConnect;
        private DiemGV diemGV;

        public Grade(string maGV)
        {
            InitializeComponent();
            this.dbConnect = new Connect();
            this.diemGV = new DiemGV();
            LoadData();
            LoadComboBoxes();
            cbHK.SelectedIndexChanged += CbHK_SelectedIndexChanged;
            cbMon.SelectedIndexChanged += CbMon_SelectedIndexChanged;
            cbLop.SelectedIndexChanged += CbClass_SelectedIndexChanged;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            btnSave.Click += BtnSave_Click;
        }

        private void LoadData(string selectedHK = null, string selectedMon = null, string selectedClass = null, string searchText = null)
        {
            DataTable dataTable = diemGV.GetGrades(selectedHK, selectedMon, selectedClass, searchText);

            // Debugging: Log the data being fetched
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"MaSV: {row["MaSV"]}, MaMon: {row["MaMon"]}, MaHK: {row["MaHK"]}, DiemTB: {row["DiemTB"]}");
            }

            guna2DataGridView1.DataSource = dataTable;

            // Set the display order and header text of the columns
            guna2DataGridView1.Columns["STT"].DisplayIndex = 0;
            guna2DataGridView1.Columns["STT"].HeaderText = "STT";
            guna2DataGridView1.Columns["MaSV"].DisplayIndex = 1;
            guna2DataGridView1.Columns["MaSV"].HeaderText = "Mã SV";
            guna2DataGridView1.Columns["HoVaTen"].DisplayIndex = 2;
            guna2DataGridView1.Columns["HoVaTen"].HeaderText = "Họ Tên";
            guna2DataGridView1.Columns["MaMon"].DisplayIndex = 3;
            guna2DataGridView1.Columns["MaMon"].HeaderText = "Mã Môn";
            guna2DataGridView1.Columns["TenMon"].DisplayIndex = 4;
            guna2DataGridView1.Columns["TenMon"].HeaderText = "Tên Môn";

            guna2DataGridView1.Columns["MaHK"].DisplayIndex = 5;
            guna2DataGridView1.Columns["MaHK"].HeaderText = "Học kì";
            guna2DataGridView1.Columns["D1_1"].DisplayIndex = 6;
            guna2DataGridView1.Columns["D1_1"].HeaderText = "Điểm hệ số 1";
            guna2DataGridView1.Columns["D1_2"].DisplayIndex = 7;
            guna2DataGridView1.Columns["D1_2"].HeaderText = "Điểm hệ số 1";
            guna2DataGridView1.Columns["D2_1"].DisplayIndex = 8;
            guna2DataGridView1.Columns["D2_1"].HeaderText = "Điểm hệ số 2";
            guna2DataGridView1.Columns["D2_2"].DisplayIndex = 9;
            guna2DataGridView1.Columns["D2_2"].HeaderText = "Điểm hệ số 3";
            guna2DataGridView1.Columns["DiemTB"].DisplayIndex = 10;
            guna2DataGridView1.Columns["DiemTB"].HeaderText = "Điểm TB";
        }

        private void LoadComboBoxes()
        {
            LoadMonHoc();
            LoadHocKi();
            LoadClasses();
            cbLop.Enabled = cbLop.Items.Count > 0;
            cbLop.Visible = cbLop.Items.Count > 0;
        }

        private void LoadClasses()
        {
            if (cbMon.SelectedItem is ComboBoxItem selectedMon && cbHK.SelectedItem is ComboBoxItem selectedHK)
            {
                var lopTable = diemGV.GetLop(selectedMon.Value);
                cbLop.Items.Clear();

                foreach (DataRow row in lopTable.Rows)
                {
                    cbLop.Items.Add(new ComboBoxItem(row["TenLop"].ToString(), row["MaLop"].ToString()));
                }

                // Ensure the ComboBox is visible and enabled
                cbLop.Enabled = cbLop.Items.Count > 0;
                cbLop.Visible = cbLop.Items.Count > 0;
            }
        }

        private void LoadMonHoc()
        {
            var monHocTable = diemGV.GetMonHoc();
            var monHocSet = new HashSet<string>();

            cbMon.Items.Clear();

            foreach (DataRow row in monHocTable.Rows)
            {
                var tenMon = row["TenMon"].ToString();
                if (!monHocSet.Contains(tenMon))
                {
                    monHocSet.Add(tenMon);
                    cbMon.Items.Add(new ComboBoxItem(tenMon, row["MaMon"].ToString()));
                }
            }
        }

        private void LoadHocKi()
        {
            if (cbMon.SelectedItem is ComboBoxItem selectedMon)
            {
                var hocKiTable = diemGV.GetHocKi();
                cbHK.Items.Clear();

                foreach (DataRow row in hocKiTable.Rows)
                {
                    cbHK.Items.Add(new ComboBoxItem(row["TenHK"].ToString(), row["MaHK"].ToString()));
                }
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public ComboBoxItem(string text, string value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void CbHK_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClasses();
        }

        private void CbMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHocKi();
            LoadClasses();
        }

        private void CbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            diemGV.SaveGrades(guna2DataGridView1);
            ReloadData();
        }

        private void ReloadData()
        {
            var selectedHK = cbHK.SelectedItem as ComboBoxItem;
            var selectedMon = cbMon.SelectedItem as ComboBoxItem;
            var selectedClass = cbLop.SelectedItem as ComboBoxItem;
            var searchText = textBoxSearch.Text;
            LoadData(selectedHK?.Value, selectedMon?.Value, selectedClass?.Value, searchText);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var selectedHK = cbHK.SelectedItem as ComboBoxItem;
            var selectedClass = cbLop.SelectedItem as ComboBoxItem;
            var selectedMon = cbMon.SelectedItem as ComboBoxItem;

            Print printForm = new Print();
            printForm.SetData(selectedHK?.Text, selectedClass?.Value, selectedClass?.Text, selectedMon?.Text, guna2DataGridView1);
            printForm.ShowDialog();
        }
    }
}
