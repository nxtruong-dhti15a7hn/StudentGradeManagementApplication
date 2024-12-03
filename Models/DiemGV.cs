using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentGradeManagementApplication.Models
{
    internal class DiemGV
    {
        private Connect dbConnect;

        public DiemGV()
        {
            this.dbConnect = new Connect();
        }

        public DataTable GetGrades(string selectedHK = null, string selectedMon = null, string selectedClass = null, string searchText = null)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                // Tạo truy vấn SQL động với các điều kiện lọc
                StringBuilder queryBuilder = new StringBuilder(@"
                    SELECT DISTINCT DiemSV.MaSV, SinhVien.HoVaTen, DiemSV.MaMon, MonHoc.TenMon, DiemSV.MaHK, 
                    DiemSV.D1_1, DiemSV.D1_2, DiemSV.D2_1, DiemSV.D2_2 
                    FROM DiemSV 
                    JOIN SinhVien ON DiemSV.MaSV = SinhVien.MaSV 
                    JOIN MonHoc ON DiemSV.MaMon = MonHoc.MaMon 
                    ");

                // Kiểm tra và thêm các điều kiện lọc vào truy vấn
                if (!string.IsNullOrEmpty(selectedHK) || !string.IsNullOrEmpty(selectedMon) || !string.IsNullOrEmpty(selectedClass) || !string.IsNullOrEmpty(searchText))
                {
                    queryBuilder.Append(" WHERE");
                    bool firstCondition = true;

                    if (!string.IsNullOrEmpty(selectedHK))
                    {
                        queryBuilder.Append(" DiemSV.MaHK = @selectedHK");
                        firstCondition = false;
                    }
                    if (!string.IsNullOrEmpty(selectedMon))
                    {
                        if (!firstCondition) queryBuilder.Append(" AND");
                        queryBuilder.Append(" DiemSV.MaMon = @selectedMon");
                        firstCondition = false;
                    }
                    if (!string.IsNullOrEmpty(selectedClass))
                    {
                        if (!firstCondition) queryBuilder.Append(" AND");
                        queryBuilder.Append(" SinhVien.MaLop = @selectedClass");
                        firstCondition = false;
                    }
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        if (!firstCondition) queryBuilder.Append(" AND");
                        queryBuilder.Append(" (SinhVien.HoVaTen LIKE @searchText OR DiemSV.MaSV LIKE @searchText)");
                    }
                }

                // Tạo SqlCommand và thêm các tham số
                SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection);
                if (!string.IsNullOrEmpty(selectedHK)) command.Parameters.AddWithValue("@selectedHK", selectedHK);
                if (!string.IsNullOrEmpty(selectedMon)) command.Parameters.AddWithValue("@selectedMon", selectedMon);
                if (!string.IsNullOrEmpty(selectedClass)) command.Parameters.AddWithValue("@selectedClass", selectedClass);
                if (!string.IsNullOrEmpty(searchText)) command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                // Sử dụng SqlDataAdapter để lấy dữ liệu vào DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Thêm cột STT (Số thứ tự)
                if (!dataTable.Columns.Contains("STT"))
                {
                    dataTable.Columns.Add("STT", typeof(int));
                }

                // Thêm cột DiemTB (Điểm trung bình)
                if (!dataTable.Columns.Contains("DiemTB"))
                {
                    dataTable.Columns.Add("DiemTB", typeof(double));
                }

                // Tính toán STT và DiemTB cho từng dòng
                int stt = 1;
                foreach (DataRow row in dataTable.Rows)
                {
                    row["STT"] = stt++;
                    double D1_1 = Convert.IsDBNull(row["D1_1"]) ? 0 : Convert.ToDouble(row["D1_1"]);
                    double D1_2 = Convert.IsDBNull(row["D1_2"]) ? 0 : Convert.ToDouble(row["D1_2"]);
                    double D2_1 = Convert.IsDBNull(row["D2_1"]) ? 0 : Convert.ToDouble(row["D2_1"]);
                    double D2_2 = Convert.IsDBNull(row["D2_2"]) ? 0 : Convert.ToDouble(row["D2_2"]);
                    double DiemTB = (D1_1 + D1_2 + (D2_1 * 2) + (D2_2 * 3)) / 7;
                    row["DiemTB"] = Math.Round(DiemTB, 2); // Làm tròn đến 2 chữ số thập phân
                }

                return dataTable;
            }
        }


        public DataTable GetHocKi()
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                /*string query = @"
                            SELECT DISTINCT HocKi.MaHK, HocKi.TenHK 
                            FROM HocKi
                            JOIN DiemSV ON HocKi.MaHK = DiemSV.MaHK
                            JOIN MonHoc ON DiemSV.MaMon = MonHoc.MaMon
                            WHERE MonHoc.MaGV = @maGV";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maGV", maGV);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;*/
                string query = @"
                    SELECT DISTINCT HocKi.MaHK, HocKi.TenHK 
                    FROM HocKi";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetMonHoc()
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = "SELECT MaMon, TenMon FROM MonHoc";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetLop( string maMon)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = @"
                    SELECT DISTINCT MonHoc.MaLop, Lop.TenLop 
                    FROM MonHoc
                    JOIN Lop ON MonHoc.MaLop = Lop.MaLop
                    WHERE MonHoc.MaMon = @maMon";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maMon", maMon);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void SaveGrades(DataGridView dataGridView)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                connection.Open();
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    string maSV = row.Cells["MaSV"].Value?.ToString();
                    string maMon = row.Cells["MaMon"].Value?.ToString();
                    // Bỏ qua các dòng thiếu mã sinh viên hoặc mã môn
                    if (string.IsNullOrWhiteSpace(maSV) || string.IsNullOrWhiteSpace(maMon))
                    {
                        MessageBox.Show("MaSV hoặc MaMon bị thiếu. Không thể lưu dòng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                    double d1_1 = Convert.IsDBNull(row.Cells["D1_1"].Value) ? 0 : Convert.ToDouble(row.Cells["D1_1"].Value);
                    double d1_2 = Convert.IsDBNull(row.Cells["D1_2"].Value) ? 0 : Convert.ToDouble(row.Cells["D1_2"].Value);
                    double d2_1 = Convert.IsDBNull(row.Cells["D2_1"].Value) ? 0 : Convert.ToDouble(row.Cells["D2_1"].Value);
                    double d2_2 = Convert.IsDBNull(row.Cells["D2_2"].Value) ? 0 : Convert.ToDouble(row.Cells["D2_2"].Value);

                    // Calculate the average score and round to 2 decimal places
                    double diemTB = Math.Round((d1_1 + d1_2 + (d2_1 * 2) + (d2_2 * 3)) / 7, 2);

                    // Check if the record exists
                    string checkQuery = "SELECT COUNT(*) FROM DiemSV WHERE MaSV = @MaSV AND MaMon = @MaMon";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@MaSV", maSV);
                    checkCommand.Parameters.AddWithValue("@MaMon", maMon);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0) // Record exists, update it
                    {
                        string updateQuery = @"
                    UPDATE DiemSV 
                    SET D1_1 = @D1_1, D1_2 = @D1_2, D2_1 = @D2_1, D2_2 = @D2_2, DiemTB = @DiemTB 
                    WHERE MaSV = @MaSV AND MaMon = @MaMon";

                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@MaSV", maSV);
                        updateCommand.Parameters.AddWithValue("@MaMon", maMon);
                        updateCommand.Parameters.AddWithValue("@D1_1", d1_1);
                        updateCommand.Parameters.AddWithValue("@D1_2", d1_2);
                        updateCommand.Parameters.AddWithValue("@D2_1", d2_1);
                        updateCommand.Parameters.AddWithValue("@D2_2", d2_2);
                        updateCommand.Parameters.AddWithValue("@DiemTB", diemTB);

                        try
                        {
                            updateCommand.ExecuteNonQuery();
                        }
                        catch (SqlException sqlEx)
                        {
                            MessageBox.Show($"SQL Error when updating: {sqlEx.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else // Record does not exist, insert a new one
                    {
                        string insertQuery = @"
                    INSERT INTO DiemSV (MaSV, MaMon, MaHK, D1_1, D1_2, D2_1, D2_2, DiemTB) 
                    VALUES (@MaSV, @MaMon, @MaHK, @D1_1, @D1_2, @D2_1, @D2_2, @DiemTB)";

                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@MaSV", maSV);
                        insertCommand.Parameters.AddWithValue("@MaMon", maMon);
                        insertCommand.Parameters.AddWithValue("@MaHK", row.Cells["MaHK"].Value?.ToString());
                        insertCommand.Parameters.AddWithValue("@D1_1", d1_1);
                        insertCommand.Parameters.AddWithValue("@D1_2", d1_2);
                        insertCommand.Parameters.AddWithValue("@D2_1", d2_1);
                        insertCommand.Parameters.AddWithValue("@D2_2", d2_2);
                        insertCommand.Parameters.AddWithValue("@DiemTB", diemTB);

                        try
                        {
                            insertCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error when inserting: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

    }
}
