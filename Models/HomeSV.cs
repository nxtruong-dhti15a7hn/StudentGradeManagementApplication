using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradeManagementApplication.Models
{
    internal class HomeSV
    {
        private Connect dbConnect;

        public HomeSV()
        {
            dbConnect = new Connect();
        }

        public DataTable GetStudentInfo(string studentId)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                                    SELECT 
                                        u.MaSV, 
                                        sv.HoVaTen, 
                                        l.TenLop AS Lop, 
                                        h.TenHe, 
                                        sv.Nganh, 
                                        sv.HinhAnh 
                                    FROM 
                                        [User] u
                                    JOIN 
                                        SinhVien sv ON u.MaSV = sv.MaSV
                                    JOIN 
                                        Lop l ON sv.MaLop = l.MaLop
                                    JOIN 
                                        HeDaoTao h ON sv.MaHe = h.MaHe
                                    WHERE 
                                        u.MaSV = @MaSV";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSV", studentId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("Đã xảy ra lỗi khi tải thông tin sinh viên: " + ex.Message);
                }
            }
        }

        public DataTable GetSemesters()
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT DISTINCT TenHK FROM HocKi";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("Đã xảy ra lỗi khi tải danh sách học kỳ: " + ex.Message);
                }
            }
        }

        public DataTable GetSubjectsAndScores(string studentId, string semester)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                            SELECT mh.TenMon, d.DiemTB 
                            FROM MonHoc mh
                            LEFT JOIN DiemSv d ON mh.MaMon = d.MaMon
                            LEFT JOIN HocKi hk ON d.MaHK = hk.MaHK
                            WHERE d.MaSV = @MaSV AND hk.TenHK = @TenHK";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSV", studentId);
                    command.Parameters.AddWithValue("@TenHK", semester);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("Đã xảy ra lỗi khi tải danh sách môn học và điểm: " + ex.Message);
                }
            }
        }
    }
}
