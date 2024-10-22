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
        public MenuStudent()
        {
            InitializeComponent();
            openChildForm(new Home());
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
            pnBody.Controls.Clear(); // Clear existing controls
            pnBody.Controls.Add(childForm);
            pnBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            openChildForm(new Home());
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            openChildForm(new Profile());
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {

           
            this.Close();
           
        }
    }
}
