using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudentGradeManagementApplication.Models
{
    internal class DangNhap
    {
        private Connect dbConnection;

        public DangNhap()
        {
            dbConnection = new Connect();
        }

        public User AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT MaSV AS Username, MatKhau AS Password, 'Student' AS UserType 
                        FROM [User] 
                        WHERE MaSV = @Username AND MatKhau = @Password
                        UNION
                        SELECT MaGV AS Username, MatKhau AS Password, 'Teacher' AS UserType 
                        FROM [User_Teacher] 
                        WHERE MaGV = @Username AND MatKhau = @Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                UserType = reader["UserType"].ToString()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi xác thực người dùng: " + ex.Message);
                    return null;
                }
            }
        }

        public string GetStudentId(string username)
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MaSV FROM [User] WHERE MaSV = @MaSV";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSV", username);

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy MaSV cho người dùng này.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi lấy MaSV: " + ex.Message);
                    return null;
                }
            }
        }

        public string GetTeacherId(string username)
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MaGV FROM [User_Teacher] WHERE MaGV = @MaGV";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaGV", username);

                    // Ghi nhật ký truy vấn SQL và giá trị tham số
                    Console.WriteLine("Executing query: " + query);
                    Console.WriteLine("Parameter: @MaGV = '" + username + "'");

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy MaGV cho người dùng này.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi lấy MaGV: " + ex.Message);
                    return null;
                }
            }
        }

        public bool ValidateStudentId(string studentId)
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSV", studentId);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi kiểm tra mã sinh viên: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
