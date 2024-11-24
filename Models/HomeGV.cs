using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradeManagementApplication.Models
{
    internal class HomeGV
    {
        private Connect dbConnect;

        public HomeGV()
        {
            dbConnect = new Connect();
        }

        public DataTable GetTeacherInfo(string teacherId)
        {
            using (SqlConnection connection = dbConnect.GetConnection())
            {
                string query = @"
                                    SELECT 
                                        ut.MaGV, 
                                        gv.HoVaTen, 
                                        gv.Khoa, 
                                        gv.ViTriCongTac, 
                                        gv.HinhAnh 
                                    FROM 
                                        User_Teacher ut
                                    JOIN 
                                        GiaoVien gv ON ut.MaGV = gv.MaGV
                                    WHERE 
                                        ut.MaGV = @MaGV";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaGV", teacherId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
