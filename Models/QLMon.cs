using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradeManagementApplication.Models
{
    internal class QLMon
    {
        private Connect dbConnect;

        public QLMon()
        {
            dbConnect = new Connect();
        }

        public DataTable GetAllSubjects()
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
                            SELECT MonHoc.MaMon, MonHoc.MaGV, MonHoc.MaLop, MonHoc.MaHK, MonHoc.TenMon, MonHoc.SoHTrinh, HocKi.TenHK 
                            FROM MonHoc 
                            LEFT JOIN HocKi ON MonHoc.MaHK = HocKi.MaHK";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetAllHocKi()
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = "SELECT MaHK, TenHK FROM HocKi";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add an "All" option to the ComboBox
                DataRow row = dt.NewRow();
                row["MaHK"] = DBNull.Value;
                row["TenHK"] = "All";
                dt.Rows.InsertAt(row, 0);

                return dt;
            }
        }

        public DataTable GetSubjectsByHocKi(string maHK)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query;
                SqlDataAdapter da;
                if (string.IsNullOrEmpty(maHK))
                {
                    query = @"
                                SELECT MonHoc.MaMon, MonHoc.MaGV, MonHoc.MaLop, MonHoc.MaHK, MonHoc.TenMon, MonHoc.SoHTrinh, HocKi.TenHK 
                                FROM MonHoc 
                                LEFT JOIN HocKi ON MonHoc.MaHK = HocKi.MaHK";
                    da = new SqlDataAdapter(query, conn);
                }
                else
                {
                    query = @"
                                SELECT MonHoc.MaMon, MonHoc.MaGV, MonHoc.MaLop, MonHoc.MaHK, MonHoc.TenMon, MonHoc.SoHTrinh, HocKi.TenHK 
                                FROM MonHoc 
                                LEFT JOIN HocKi ON MonHoc.MaHK = HocKi.MaHK
                                WHERE MonHoc.MaHK = @MaHK";
                    da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@MaHK", maHK);
                }
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable SearchSubjects(string searchText)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
                            SELECT MonHoc.MaMon, MonHoc.MaGV, MonHoc.MaLop, MonHoc.MaHK, MonHoc.TenMon, MonHoc.SoHTrinh, HocKi.TenHK 
                            FROM MonHoc 
                            LEFT JOIN HocKi ON MonHoc.MaHK = HocKi.MaHK
                            WHERE MonHoc.MaMon LIKE @SearchText OR MonHoc.TenMon LIKE @SearchText";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void UpdateSubject(DataRow row)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
            UPDATE MonHoc
            SET MaGV = @MaGV, MaLop = @MaLop, MaHK = @MaHK, TenMon = @TenMon, SoHTrinh = @SoHTrinh
            WHERE MaMon = @MaMon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaMon", row["MaMon"]);
                cmd.Parameters.AddWithValue("@MaGV", row["MaGV"]);
                cmd.Parameters.AddWithValue("@MaLop", row["MaLop"]);
                cmd.Parameters.AddWithValue("@MaHK", row["MaHK"]);
                cmd.Parameters.AddWithValue("@TenMon", row["TenMon"]);
                cmd.Parameters.AddWithValue("@SoHTrinh", row["SoHTrinh"]);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertSubject(DataRow row)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
            INSERT INTO MonHoc (MaMon, MaGV, MaLop, MaHK, TenMon, SoHTrinh)
            VALUES (@MaMon, @MaGV, @MaLop, @MaHK, @TenMon, @SoHTrinh)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaMon", row["MaMon"]);
                cmd.Parameters.AddWithValue("@MaGV", row["MaGV"]);
                cmd.Parameters.AddWithValue("@MaLop", row["MaLop"]);
                cmd.Parameters.AddWithValue("@MaHK", row["MaHK"]);
                cmd.Parameters.AddWithValue("@TenMon", row["TenMon"]);
                cmd.Parameters.AddWithValue("@SoHTrinh", row["SoHTrinh"]);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSubject(string maMon)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = "DELETE FROM MonHoc WHERE MaMon = @MaMon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaMon", maMon);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
