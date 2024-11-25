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

        public DataTable GetGrades(string selectedHK = null, string selectedMon = null, string selectedClass = null, string searchText = null, string maGV = null)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                StringBuilder queryBuilder = new StringBuilder(@"
                            SELECT DiemSV.*, SinhVien.HoVaTen, MonHoc.TenMon 
                            FROM DiemSV 
                            JOIN SinhVien ON DiemSV.MaSV = SinhVien.MaSV
                            JOIN MonHoc ON DiemSV.MaMon = MonHoc.MaMon
                            JOIN GiaoVien ON MonHoc.MaGV = GiaoVien.MaGV");

                if (!string.IsNullOrEmpty(selectedHK) || !string.IsNullOrEmpty(selectedMon) || !string.IsNullOrEmpty(selectedClass) || !string.IsNullOrEmpty(searchText) || !string.IsNullOrEmpty(maGV))
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
                        if (!firstCondition)
                        {
                            queryBuilder.Append(" AND");
                        }
                        queryBuilder.Append(" DiemSV.MaMon = @selectedMon");
                        firstCondition = false;
                    }
                    if (!string.IsNullOrEmpty(selectedClass))
                    {
                        if (!firstCondition)
                        {
                            queryBuilder.Append(" AND");
                        }
                        queryBuilder.Append(" SinhVien.MaLop = @selectedClass");
                        firstCondition = false;
                    }
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        if (!firstCondition)
                        {
                            queryBuilder.Append(" AND");
                        }
                        queryBuilder.Append(" DiemSV.MaSV LIKE @searchText");
                        firstCondition = false;
                    }
                    if (!string.IsNullOrEmpty(maGV))
                    {
                        if (!firstCondition)
                        {
                            queryBuilder.Append(" AND");
                        }
                        queryBuilder.Append(" GiaoVien.MaGV = @maGV");
                    }
                }

                SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection);
                if (!string.IsNullOrEmpty(selectedHK))
                {
                    command.Parameters.AddWithValue("@selectedHK", selectedHK);
                }
                if (!string.IsNullOrEmpty(selectedMon))
                {
                    command.Parameters.AddWithValue("@selectedMon", selectedMon);
                }
                if (!string.IsNullOrEmpty(selectedClass))
                {
                    command.Parameters.AddWithValue("@selectedClass", selectedClass);
                }
                if (!string.IsNullOrEmpty(searchText))
                {
                    command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                }
                if (!string.IsNullOrEmpty(maGV))
                {
                    command.Parameters.AddWithValue("@maGV", maGV);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Add STT and DiemTB columns
                if (!dataTable.Columns.Contains("STT"))
                {
                    dataTable.Columns.Add("STT", typeof(int));
                }
                if (!dataTable.Columns.Contains("DiemTB"))
                {
                    dataTable.Columns.Add("DiemTB", typeof(double));
                } 
                int stt = 1;
                foreach (DataRow row in dataTable.Rows)
                {
                    row["STT"] = stt++;
                    double D1_1 = Convert.IsDBNull(row["D1_1"]) ? 0 : Convert.ToDouble(row["D1_1"]);
                    double D1_2 = Convert.IsDBNull(row["D1_2"]) ? 0 : Convert.ToDouble(row["D1_2"]);
                    double D2_1 = Convert.IsDBNull(row["D2_1"]) ? 0 : Convert.ToDouble(row["D2_1"]);
                    double D2_2 = Convert.IsDBNull(row["D2_2"]) ? 0 : Convert.ToDouble(row["D2_2"]);
                    double DiemTB = (D1_1 + D1_2 + (D2_1 * 2) + (D2_2 * 3)) / 7;
                    row["DiemTB"] = DiemTB;
                }

                return dataTable;
            }
        }

        public DataTable GetHocKi(string maGV)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = @"
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
                return dataTable;
            }
        }

        public DataTable GetMonHoc(string maGV)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = "SELECT MaMon, TenMon FROM MonHoc WHERE MaGV = @maGV";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maGV", maGV);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetLop(string maGV, string maMon)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = @"
                            SELECT DISTINCT MonHoc.MaLop, Lop.TenLop 
                            FROM MonHoc
                            JOIN Lop ON MonHoc.MaLop = Lop.MaLop
                            WHERE MonHoc.MaGV = @maGV AND MonHoc.MaMon = @maMon";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maGV", maGV);
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
                    double d1_1 = Convert.IsDBNull(row.Cells["D1_1"].Value) ? 0 : Convert.ToDouble(row.Cells["D1_1"].Value);
                    double d1_2 = Convert.IsDBNull(row.Cells["D1_2"].Value) ? 0 : Convert.ToDouble(row.Cells["D1_2"].Value);
                    double d2_1 = Convert.IsDBNull(row.Cells["D2_1"].Value) ? 0 : Convert.ToDouble(row.Cells["D2_1"].Value);
                    double d2_2 = Convert.IsDBNull(row.Cells["D2_2"].Value) ? 0 : Convert.ToDouble(row.Cells["D2_2"].Value);

                    // Calculate the average score and round to 2 decimal places
                    double diemTB = Math.Round((d1_1 + d1_2 + (d2_1 * 2) + (d2_2 * 3)) / 7, 2);

                    string query = @"
                                    UPDATE DiemSV 
                                    SET D1_1 = @D1_1, D1_2 = @D1_2, D2_1 = @D2_1, D2_2 = @D2_2, DiemTB = @DiemTB 
                                    WHERE MaSV = @MaSV AND MaMon = @MaMon";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    command.Parameters.AddWithValue("@MaMon", maMon);
                    command.Parameters.AddWithValue("@D1_1", d1_1);
                    command.Parameters.AddWithValue("@D1_2", d1_2);
                    command.Parameters.AddWithValue("@D2_1", d2_1);
                    command.Parameters.AddWithValue("@D2_2", d2_2);
                    command.Parameters.AddWithValue("@DiemTB", diemTB);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show($"No rows updated for student {maSV} in subject {maMon}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving grades for student {maSV} in subject {maMon}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
