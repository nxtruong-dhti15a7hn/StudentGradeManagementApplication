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

        public Grade()
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
            guna2DataGridView1.Columns["D1_1"].DisplayIndex = 4;
            guna2DataGridView1.Columns["D1_1"].HeaderText = "Điểm hệ số 1";
            guna2DataGridView1.Columns["D1_2"].DisplayIndex = 5;
            guna2DataGridView1.Columns["D1_2"].HeaderText = "Điểm hệ số 1";
            guna2DataGridView1.Columns["D2_1"].DisplayIndex = 6;
            guna2DataGridView1.Columns["D2_1"].HeaderText = "Điểm hệ số 2";
            guna2DataGridView1.Columns["D2_2"].DisplayIndex = 7;
            guna2DataGridView1.Columns["D2_2"].HeaderText = "Điểm hệ số 3";
            guna2DataGridView1.Columns["DiemTB"].DisplayIndex = 8;
            guna2DataGridView1.Columns["DiemTB"].HeaderText = "Điểm trung bình";
        }

        private void LoadComboBoxes()
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                connection.Open();

                // Load cbHK
                cbHK.Items.Add(new ComboBoxItem("", "")); // Add blank item
                string queryHK = "SELECT MaHK, TenHK FROM HocKi";
                SqlCommand commandHK = new SqlCommand(queryHK, connection);
                SqlDataReader readerHK = commandHK.ExecuteReader();
                while (readerHK.Read())
                {
                    cbHK.Items.Add(new ComboBoxItem(readerHK["TenHK"].ToString(), readerHK["MaHK"].ToString()));
                }
                readerHK.Close();
                cbHK.SelectedIndex = 0; // Set blank item as default

                // Load cbMon
                cbMon.Items.Add(new ComboBoxItem("", "")); // Add blank item
                string queryMon = "SELECT MaMon, TenMon FROM MonHoc";
                SqlCommand commandMon = new SqlCommand(queryMon, connection);
                SqlDataReader readerMon = commandMon.ExecuteReader();
                while (readerMon.Read())
                {
                    cbMon.Items.Add(new ComboBoxItem(readerMon["TenMon"].ToString(), readerMon["MaMon"].ToString()));
                }
                readerMon.Close();
                cbMon.SelectedIndex = 0; // Set blank item as default

                // Load cbClass
                cbLop.Items.Add(new ComboBoxItem("", "")); // Add blank item
                string queryClass = "SELECT MaLop, TenLop FROM Lop";
                SqlCommand commandClass = new SqlCommand(queryClass, connection);
                SqlDataReader readerClass = commandClass.ExecuteReader();
                while (readerClass.Read())
                {
                    cbLop.Items.Add(new ComboBoxItem(readerClass["TenLop"].ToString(), readerClass["MaLop"].ToString()));
                }
                readerClass.Close();
                cbLop.SelectedIndex = 0; // Set blank item as default
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
            ReloadData();
        }

        private void CbMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
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
