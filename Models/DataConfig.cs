using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentGradeManagementApplication.Models
{
    internal class DataConfig
    {
        public static string ConnectionString { get; } = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\QLDiem.mdf;Integrated Security=True;Connect Timeout=30";

    }
}
