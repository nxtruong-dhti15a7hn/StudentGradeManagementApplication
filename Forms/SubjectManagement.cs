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
    
public partial class SubjectManagement : Form
    {
        private QLMon qlMon;

        public SubjectManagement()
        {
            InitializeComponent();
            qlMon = new QLMon();
            LoadSubjectData();
            LoadHocKiData();
        }

        private void LoadSubjectData()
        {
            DataTable dt = qlMon.GetAllSubjects();
            guna2DataGridView1.DataSource = dt;
        }

        private void LoadHocKiData()
        {
            DataTable dt = qlMon.GetAllHocKi();
            guna2ComboBox1.DataSource = dt;
            guna2ComboBox1.DisplayMember = "TenHK";
            guna2ComboBox1.ValueMember = "MaHK";
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedValue != null && guna2ComboBox1.SelectedValue is DataRowView)
            {
                string selectedMaHK = ((DataRowView)guna2ComboBox1.SelectedValue)["MaHK"].ToString();
                DataTable dt = qlMon.GetSubjectsByHocKi(selectedMaHK);
                guna2DataGridView1.DataSource = dt;
            }
            else if (guna2ComboBox1.SelectedValue != null)
            {
                string selectedMaHK = guna2ComboBox1.SelectedValue.ToString();
                DataTable dt = qlMon.GetSubjectsByHocKi(selectedMaHK);
                guna2DataGridView1.DataSource = dt;
            }
            else
            {
                guna2DataGridView1.DataSource = null;
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBoxSearch.Text.Trim();
            DataTable dt = qlMon.SearchSubjects(searchText);
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)guna2DataGridView1.DataSource;
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    qlMon.UpdateSubject(row);
                }
                else if (row.RowState == DataRowState.Added)
                {
                    qlMon.InsertSubject(row);
                }
                else if (row.RowState == DataRowState.Deleted)
                {
                    qlMon.DeleteSubject(row["MaMon", DataRowVersion.Original].ToString());
                }
            }
            LoadSubjectData(); // Reload data after save
        }
    }
}
