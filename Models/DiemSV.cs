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
    internal class DiemSV
    {
        private Connect dbConnect;

        public DiemSV()
        {
            dbConnect = new Connect();
        }

        public DataTable GetHocKi()
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = "SELECT MaHK, TenHK FROM HocKi";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetTranscriptData(string maHK, string maSV)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
                        SELECT 
                            d.MaSV,
                          
                            d.MaMon, 
                            m.TenMon, 
                            d.MaHK,
                            d.D1_1, 
                            d.D1_2, 
                            d.D2_1, 
                            d.D2_2, 
                            d.DiemTB 
                        FROM DiemSV d
                        JOIN MonHoc m ON d.MaMon = m.MaMon
                        JOIN SinhVien sv ON d.MaSV = sv.MaSV
                        WHERE d.MaSV = @MaSV";

                if (!string.IsNullOrEmpty(maHK))
                {
                    query += " AND d.MaHK = @MaHK";
                }

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@MaSV", maSV);
                if (!string.IsNullOrEmpty(maHK))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaHK", maHK);
                }

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
