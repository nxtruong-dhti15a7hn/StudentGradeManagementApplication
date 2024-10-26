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
    public partial class Transcript : Form
    {
        public Transcript()
        {
            InitializeComponent();
        }

        private void Transcript_Load(object sender, EventArgs e)
        {
            // Tạo một DataTable để lưu trữ dữ liệu
            DataTable dt = new DataTable();
            guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


            // Thêm các cột với tên giống như hình
            dt.Columns.Add("STT", typeof(int)).Caption = "STT";
            dt.Columns.Add("MaLop", typeof(string)).Caption = "Mã lớp học phần";
            dt.Columns.Add("TenMon", typeof(string)).Caption = "Tên môn học/học phần";
            dt.Columns.Add("SoTinChi", typeof(int)).Caption = "Số tín chỉ";
            dt.Columns.Add("ChuyenCan", typeof(float)).Caption = "Chuyên cần";
            dt.Columns.Add("LTHS1", typeof(float)).Caption = "LT Hệ số 1";
            dt.Columns.Add("LTHS2", typeof(float)).Caption = "LT Hệ số 2";
            dt.Columns.Add("THHS1", typeof(float)).Caption = "TH Hệ số 1";
            dt.Columns.Add("THHS2", typeof(float)).Caption = "TH Hệ số 2";
            dt.Columns.Add("TBThuongKy", typeof(float)).Caption = "TB thường kỳ";
            dt.Columns.Add("DuocDuThi", typeof(string)).Caption = "Được dự thi";
            dt.Columns.Add("CuoiKy", typeof(float)).Caption = "Cuối kỳ";
            dt.Columns.Add("DiemTongKet", typeof(float)).Caption = "Điểm tổng kết";

            // Thêm dữ liệu ví dụ
            dt.Rows.Add(1, "010100019711", "Tin cơ sở", 4, 8.0, 5.0, 6.0, 5.0, 5.5, 6.1, "✓", 4.0, 4.8);
            dt.Rows.Add(2, "010100058410", "Logic học", 2, 10.0, 8.0, 7.5, 0.0, 0.0, 8.5, "✓", 6.0, 7.0);
            dt.Rows.Add(3, "010100096911", "Tin học văn phòng", 2, 10.0, 7.0, 9.0, 8.9, 0.0, 9.0, "✓", 9.0, 9.0);
            // Thêm các dòng khác tương tự

            // Gán nguồn dữ liệu cho DataGridView
            guna2DataGridView1.DataSource = dt;

            // Đặt tên cho các cột trong DataGridView
            guna2DataGridView1.Columns["STT"].HeaderText = "STT";
            guna2DataGridView1.Columns["MaLop"].HeaderText = "Mã lớp học phần";
            guna2DataGridView1.Columns["TenMon"].HeaderText = "Tên môn học/học phần";
            guna2DataGridView1.Columns["SoTinChi"].HeaderText = "Số tín chỉ";
            guna2DataGridView1.Columns["ChuyenCan"].HeaderText = "Chuyên cần";
            guna2DataGridView1.Columns["LTHS1"].HeaderText = "LT Hệ số 1";
            guna2DataGridView1.Columns["LTHS2"].HeaderText = "LT Hệ số 2";
            guna2DataGridView1.Columns["THHS1"].HeaderText = "TH Hệ số 1";
            guna2DataGridView1.Columns["THHS2"].HeaderText = "TH Hệ số 2";
            guna2DataGridView1.Columns["TBThuongKy"].HeaderText = "TB thường kỳ";
            guna2DataGridView1.Columns["DuocDuThi"].HeaderText = "Được dự thi";
            guna2DataGridView1.Columns["CuoiKy"].HeaderText = "Cuối kỳ";
            guna2DataGridView1.Columns["DiemTongKet"].HeaderText = "Điểm tổng kết";

            // Đặt DataGridView không cho phép sửa đổi dữ liệu
            guna2DataGridView1.ReadOnly = true;

        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím Enter được nhấn
            if (e.KeyCode == Keys.Enter)
            {
                // Lấy giá trị từ TextBox
                string searchValue = textBoxSearch.Text.ToLower();

                // Thực hiện tìm kiếm trong DataGridView
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    // Ẩn tất cả các hàng trước khi thực hiện tìm kiếm
                    row.Visible = false;

                    // Kiểm tra nếu bất kỳ ô nào trong hàng có giá trị chứa chuỗi tìm kiếm
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchValue))
                        {
                            // Nếu tìm thấy giá trị, hiển thị hàng
                            row.Visible = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
