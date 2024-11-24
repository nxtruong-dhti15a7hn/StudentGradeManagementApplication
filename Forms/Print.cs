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
    public partial class Print : Form
    {
        public Print()
        {
            InitializeComponent();
        }

        public void SetData(string tenHK, string maLop, string tenLop, string tenMon, DataGridView dataGridView)
        {
            lbHocKy.Text = tenHK;
            lbMaLop.Text = "Mã lớp: " + maLop;
            lbTenLop.Text = "Tên lớp: " + tenLop;
            lbTenMon.Text = "Tên môn: " + tenMon;

            guna2DataGridView1.Columns.Clear();
            guna2DataGridView1.Rows.Clear();

            // Add columns to DataGridView
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                guna2DataGridView1.Columns.Add((DataGridViewColumn)column.Clone());
            }

            // Add rows to DataGridView
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    int rowIndex = guna2DataGridView1.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        guna2DataGridView1.Rows[rowIndex].Cells[i].Value = row.Cells[i].Value;
                    }
                }
            }
        }
    }
}
