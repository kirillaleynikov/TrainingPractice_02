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
    enum RowState
    {
        ModifiedNew,
        Existed,
        Deleted
    }
    public partial class FormForAdmins : Form
    {
        DataBase dataBase = new DataBase();
        int selectedRow;
        public FormForAdmins()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_user", "id");
            dataGridView1.Columns.Add("login_user", "ФИО пользователя");
            dataGridView1.Columns.Add("mark", "Оценка");
            dataGridView1.Columns.Add("isNew", String.Empty);
        } 
        private void ReadSingleRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), RowState.ModifiedNew);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from results";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRows(dgw, reader);
            }
            reader.Close();
        }

        private void FormForAdmins_Load(object sender, EventArgs e)
        {
            CreateColumns();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns["id_user"].Visible = false;
            RefreshDataGrid(dataGridView1);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }
        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from results where concat (login_user, mark) like '%" + textBoxSearch.Text + "%'";
            SqlCommand command = new SqlCommand(searchString, dataBase.GetConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                ReadSingleRows(dgw, reader);
            }
            reader.Close();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }
        private void DeleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[3].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[3].Value = RowState.Deleted;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRow();
            Update();
        }
        private void Update()
        {
            dataBase.openConnection();
            for(int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[3].Value;
                if(rowState == RowState.Existed)
                {
                    continue;
                }
                if(rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from results where id_user = '{id}'";
                    var command = new SqlCommand(deleteQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Правила добавления/изменения/удаления вопросов:\n" +
            "1. Вопросы должны содержать 1 правильный ответ из 4-х вариантов.\n" +
            "2. Правильный вариант ответа должен быть на первом месте сразу после вопроса.\n" +
            "3. В текстовом документе не должно быть отступов.\n" +
            "4. Перед закрытием файла не забудьте его сохранить (Ctrl + S).");
            string filename = @"t.txt";
            System.Diagnostics.Process.Start(filename);
        }
    }
}