using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradeManagementApplication.Models
{
    internal class ProfileSV
    {
        private Connect dbConnect;

        public ProfileSV()
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
                                    sv.MaSV, 
                                    sv.HoVaTen, 
                                    sv.NgaySinh, 
                                    sv.Que, 
                                    sv.HinhAnh, 
                                    sv.GioiTinh, 
                                    sv.KhoaHoc, 
                                    sv.Nganh, 
                                    sv.Email, 
                                    sv.CCCD, 
                                    l.TenLop, 
                                    h.TenHe 
                                FROM 
                                    SinhVien sv
                                JOIN 
                                    Lop l ON sv.MaLop = l.MaLop
                                JOIN 
                                    HeDaoTao h ON sv.MaHe = h.MaHe
                                WHERE 
                                    sv.MaSV = @MaSV";
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
    }
}
