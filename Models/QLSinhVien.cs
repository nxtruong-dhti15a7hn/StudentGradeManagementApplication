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
    internal class QLSinhVien
    {
        private Connect dbConnect;

        public QLSinhVien()
        {
            dbConnect = new Connect();
        }

        public DataTable GetAllStudents()
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
                        SELECT 
                            SinhVien.MaSV, SinhVien.MaLop, SinhVien.MaHe, SinhVien.HoVaTen, 
                            SinhVien.NgaySinh, SinhVien.Que, SinhVien.HinhAnh, SinhVien.GioiTinh, 
                            SinhVien.KhoaHoc, SinhVien.Nganh, SinhVien.Email, SinhVien.CCCD, 
                            [User].MatKhau 
                        FROM SinhVien 
                        INNER JOIN [User] ON SinhVien.MaSV = [User].MaSV";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetStudentsByClass(string maLop)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
                        SELECT 
                            SinhVien.MaSV, SinhVien.MaLop, SinhVien.MaHe, SinhVien.HoVaTen, 
                            SinhVien.NgaySinh, SinhVien.Que, SinhVien.HinhAnh, SinhVien.GioiTinh, 
                            SinhVien.KhoaHoc, SinhVien.Nganh, SinhVien.Email, SinhVien.CCCD, 
                            [User].MatKhau 
                        FROM SinhVien 
                        INNER JOIN [User] ON SinhVien.MaSV = [User].MaSV 
                        WHERE SinhVien.MaLop = @MaLop";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@MaLop", maLop);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetAllClasses()
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = "SELECT MaLop, MaHe, TenLop FROM Lop";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void SaveStudents(DataGridView dataGridView)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string maSV = row.Cells["MaSV"].Value?.ToString();
                            string tenSV = row.Cells["HoVaTen"].Value?.ToString();
                            string maLop = row.Cells["MaLop"].Value?.ToString();
                            string maHe = row.Cells["MaHe"].Value?.ToString();
                            string ngaySinh = row.Cells["NgaySinh"].Value?.ToString();
                            string que = row.Cells["Que"].Value?.ToString();
                            string hinhAnh = row.Cells["HinhAnh"].Value?.ToString();
                            string gioiTinh = row.Cells["GioiTinh"].Value?.ToString();
                            string khoaHoc = row.Cells["KhoaHoc"].Value?.ToString();
                            string nganh = row.Cells["Nganh"].Value?.ToString();
                            string email = row.Cells["Email"].Value?.ToString();
                            string cccd = row.Cells["CCCD"].Value?.ToString();
                            string matKhau = row.Cells["MatKhau"].Value?.ToString();

                            string query = @"
                                    IF EXISTS (SELECT 1 FROM SinhVien WHERE MaSV = @MaSV)
                                    BEGIN
                                        UPDATE SinhVien 
                                        SET HoVaTen = @HoVaTen, MaLop = @MaLop, MaHe = @MaHe, NgaySinh = @NgaySinh, 
                                            Que = @Que, HinhAnh = @HinhAnh, GioiTinh = @GioiTinh, KhoaHoc = @KhoaHoc, 
                                            Nganh = @Nganh, Email = @Email, CCCD = @CCCD 
                                        WHERE MaSV = @MaSV;
                                        UPDATE [User] SET MatKhau = @MatKhau WHERE MaSV = @MaSV;
                                    END
                                    ELSE
                                    BEGIN
                                        INSERT INTO SinhVien (MaSV, HoVaTen, MaLop, MaHe, NgaySinh, Que, HinhAnh, GioiTinh, KhoaHoc, Nganh, Email, CCCD) 
                                        VALUES (@MaSV, @HoVaTen, @MaLop, @MaHe, @NgaySinh, @Que, @HinhAnh, @GioiTinh, @KhoaHoc, @Nganh, @Email, @CCCD);
                                        INSERT INTO [User] (MaSV, MatKhau) VALUES (@MaSV, @MatKhau);
                                    END";

                            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaSV", maSV);
                                cmd.Parameters.AddWithValue("@HoVaTen", tenSV);
                                cmd.Parameters.AddWithValue("@MaLop", maLop);
                                cmd.Parameters.AddWithValue("@MaHe", maHe);
                                cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                                cmd.Parameters.AddWithValue("@Que", que);
                                cmd.Parameters.AddWithValue("@HinhAnh", hinhAnh);
                                cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                                cmd.Parameters.AddWithValue("@KhoaHoc", khoaHoc);
                                cmd.Parameters.AddWithValue("@Nganh", nganh);
                                cmd.Parameters.AddWithValue("@Email", email);
                                cmd.Parameters.AddWithValue("@CCCD", cccd);
                                cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error saving students: " + ex.Message);
                    }
                }
            }
        }

        public DataTable SearchStudents(string searchText)
        {
            using (SqlConnection conn = dbConnect.GetConnection())
            {
                string query = @"
                        SELECT 
                            SinhVien.MaSV, SinhVien.MaLop, SinhVien.MaHe, SinhVien.HoVaTen, 
                            SinhVien.NgaySinh, SinhVien.Que, SinhVien.HinhAnh, SinhVien.GioiTinh, 
                            SinhVien.KhoaHoc, SinhVien.Nganh, SinhVien.Email, SinhVien.CCCD, 
                            [User].MatKhau 
                        FROM SinhVien 
                        INNER JOIN [User] ON SinhVien.MaSV = [User].MaSV
                        WHERE SinhVien.HoVaTen LIKE @SearchText";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
