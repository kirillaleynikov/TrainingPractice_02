using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TrainingPractice_02
{
    public partial class Enter : Form
    {
        DataBase dataBase = new DataBase();
        public Enter()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Enter_Load(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            textBoxLogin.MaxLength = 50;
            textBoxPassword.MaxLength = 50;
            pictureBox2.Visible = false;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            var loginUser = textBoxLogin.Text;
            var passUser = md5.hashPassword(textBoxPassword.Text);
            if (passUser == md5.hashPassword("admin") && loginUser == "admin")
            {
                FormForAdmins formForAdmins = new FormForAdmins();
                formForAdmins.Show();
            }
            else
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                string queryString = $"select * from register where login_user = '{loginUser}' and password_user = '{passUser}'";
                SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count == 1)
                {
                    MessageBox.Show("Вход выполнен успешно!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DataBank.text = textBoxLogin.Text;
                    Form1 form1 = new Form1();

                    this.Hide();
                    form1.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void buttonGoToRegistration_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }
    }
}