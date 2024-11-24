using System;
using System.Data.SqlClient;

namespace StudentGradeManagementApplication.Models
{
    internal class Connect
    {
        private string connectionString;

        public string ConnectionString { get; internal set; }

        public Connect()
        {
            this.connectionString = DataConfig.ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public User AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    // Kiểm tra trong bảng User_Teacher
                    string teacherQuery = "SELECT MaGV FROM User_Teacher WHERE MaGV = @MaGV AND MatKhau = @MatKhau";
                    SqlCommand teacherCommand = new SqlCommand(teacherQuery, connection);
                    teacherCommand.Parameters.AddWithValue("@MaGV", username);
                    teacherCommand.Parameters.AddWithValue("@MatKhau", password);

                    // Ghi nhật ký truy vấn SQL và giá trị tham số
                    Console.WriteLine("Executing query: " + teacherQuery);
                    Console.WriteLine("Parameters: @MaGV = '" + username + "', @MatKhau = '" + password + "'");

                    SqlDataReader teacherReader = teacherCommand.ExecuteReader();
                    if (teacherReader.Read())
                    {
                        return new User
                        {
                            Username = teacherReader["MaGV"].ToString(),
                            UserType = "Teacher"
                        };
                    }
                    teacherReader.Close();

                    // Kiểm tra trong bảng [User] (Student)
                    string studentQuery = "SELECT MaSV FROM [User] WHERE MaSV = @MaSV AND MatKhau = @MatKhau";
                    SqlCommand studentCommand = new SqlCommand(studentQuery, connection);
                    studentCommand.Parameters.AddWithValue("@MaSV", username);
                    studentCommand.Parameters.AddWithValue("@MatKhau", password);

                    // Ghi nhật ký truy vấn SQL và giá trị tham số
                    Console.WriteLine("Executing query: " + studentQuery);
                    Console.WriteLine("Parameters: @MaSV = '" + username + "', @MatKhau = '" + password + "'");

                    SqlDataReader studentReader = studentCommand.ExecuteReader();
                    if (studentReader.Read())
                    {
                        return new User
                        {
                            Username = studentReader["MaSV"].ToString(),
                            UserType = "Student"
                        };
                    }
                    studentReader.Close();

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Đã xảy ra lỗi khi xác thực người dùng: " + ex.Message);
                }
            }
        }
    }
}
