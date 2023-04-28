using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingPractice_02
{
    public partial class Registration : Form
    { 
        DataBase dataBase = new DataBase();
        public Registration()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            textBoxPasswordRegistration.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            if (checkUser())
            {
                return;
            }
            var login = textBoxLoginRegistration.Text;
            var password = md5.hashPassword(textBoxPasswordRegistration.Text);
            string queryString = $"insert into register(login_user, password_user) values('{login}', '{password}')";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!","Успешно!");
                Enter enter = new Enter();
                this.Close();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }
            dataBase.closeConnection();
        }
        private Boolean checkUser()
        {
            var loginUser = textBoxLoginRegistration.Text;
            var passUser = md5.hashPassword(textBoxPasswordRegistration.Text);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string queryString = $"select * from register where login_user = '{loginUser}' and password_user = '{passUser}'";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь уже существует!");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBoxPasswordRegistration.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBoxPasswordRegistration.UseSystemPasswordChar = false;
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }
    }
}
