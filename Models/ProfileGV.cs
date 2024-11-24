using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradeManagementApplication.Models
{
    internal class ProfileGV
    {
        private Connect dbConnect;

        public ProfileGV()
        {
            dbConnect = new Connect();
        }

        public DataTable GetTeacherProfile(string maGV)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = @"
                                    SELECT 
                                        MaGV, 
                                        HoVaTen, 
                                        Khoa, 
                                        ViTriCongTac, 
                                        HinhAnh, 
                                        ChucDanh, 
                                        BangCap, 
                                        LinhVuc, 
                                        CongTrinh, 
                                        QueQuan, 
                                        LichSuCT, 
                                        DuAn, 
                                        HoatDong 
                                    FROM 
                                        GiaoVien 
                                    WHERE 
                                        MaGV = @MaGV";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaGV", maGV);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
