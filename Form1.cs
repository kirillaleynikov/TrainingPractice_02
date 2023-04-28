using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainingPractice_02
{
    public partial class Form1 : Form
    {
        int totalSeconds = 0;
        DataBase dataBase = new DataBase();

        string file_name = "t.txt";
        string[,] array;
        int total, kolvoVoprosov, pravilOtvetov, nepravilOtvetov, mark;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            startTest();
        }
        private void startTest()
        {
            int minutes = 05;
            int seconds = 00;
            totalSeconds = (minutes * 60) + seconds;
            label1.Text = "05:00";
            timer1.Enabled = true;
            label1.Visible = true;
            label2.Visible = true;
            timer1.Start();
            button1.Text = "Следующий вопрос";
            kolvoVoprosov = 10;
            pravilOtvetov = 0;
            nepravilOtvetov = 0;
            loadFile();
            rbChecked();
            rbTurnOnOrOff();
            labelResult.Text = "";
            kolvoVoprosov--;
            showQuestion();
        }
        private void loadFile()
        {
            try
            {
                string[] lines = File.ReadAllLines(file_name, Encoding.UTF8);
                total = lines.Length / 5;
                array = new string[kolvoVoprosov, 5];
                int[] temp = new int[kolvoVoprosov];
                int j;
                int k = 0;
                do
                {
                    j = rnd.Next(0, total) * 5;
                    if (!temp.Contains(j))
                    {
                        array[k, 0] = lines[j];
                        for (int i = 1; i < 5; i++)
                        {
                            array[k, i] = lines[j + i];
                        }
                        temp[k] = j;
                        k++;
                    }
                }
                while (!(k == kolvoVoprosov));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (kolvoVoprosov < 0)
            {
                startTest();
                return;
            }
            else if (!(radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked))
            {
                MessageBox.Show("Выберите вариант ответа");
                return;
            }
            else if ((radioButton1.Checked && Convert.ToInt32(radioButton1.Tag) == 1) ||
                    (radioButton2.Checked && Convert.ToInt32(radioButton2.Tag) == 1) ||
                    (radioButton3.Checked && Convert.ToInt32(radioButton3.Tag) == 1) ||
                    (radioButton4.Checked && Convert.ToInt32(radioButton4.Tag) == 1))
            {
                pravilOtvetov++;
                kolvoVoprosov--;
            }
            else
            {
                MessageBox.Show("Правильный вариант ответа: " + array[kolvoVoprosov, 1]);
                nepravilOtvetov++;
                kolvoVoprosov--;
            }
            if (kolvoVoprosov < 0)
            {
                showResult();
                timer1.Stop();
                label1.Visible = false;
                labelQuestion.Text = "Результат теста";
                button1.Text = "Пройти тест заново";
                rbTurnOnOrOff();
                return;
            }
            showQuestion();
        }

        private void showQuestion()
        {
            rbChecked();
            labelQuestion.Text = array[kolvoVoprosov, 0];
            radioTags(rnd.Next(1, 25));
            radioButton1.Text = array[kolvoVoprosov, Convert.ToInt32(radioButton1.Tag)];
            radioButton2.Text = array[kolvoVoprosov, Convert.ToInt32(radioButton2.Tag)];
            radioButton3.Text = array[kolvoVoprosov, Convert.ToInt32(radioButton3.Tag)];
            radioButton4.Text = array[kolvoVoprosov, Convert.ToInt32(radioButton4.Tag)];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (totalSeconds > 0)
            {
                totalSeconds--;
                int minutes = totalSeconds / 60;
                int seconds = totalSeconds - (minutes * 60);
                if (seconds < 10)
                {
                    label1.Text = "0" + minutes.ToString() + ":" + "0" + seconds.ToString();
                }
                else
                {
                    label1.Text = "0" + minutes.ToString() + ":" + seconds.ToString();
                }
            }
            else
            {
                timer1.Stop();
                timer1.Enabled = false;
                kolvoVoprosov = -1;
                MessageBox.Show("Время вышло!");
                showResult();
                label1.Visible = false;
                labelQuestion.Text = "Результат теста";
                button1.Text = "Пройти тест заново";
                rbTurnOnOrOff();
            }
        }

        private void rbTurnOnOrOff()
        {
            if (kolvoVoprosov < 0)
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
            }
            else if (timer1.Enabled == false)
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
            }
            else
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                radioButton4.Visible = true;
            }
        }
        private void rbChecked()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }
        private void radioTags(int rndValue)
        {
            switch (rndValue)
            {
                case 1: radioButton1.Tag = 1; radioButton2.Tag = 2; radioButton3.Tag = 3; radioButton4.Tag = 4; break;
                case 2: radioButton1.Tag = 1; radioButton2.Tag = 2; radioButton3.Tag = 4; radioButton4.Tag = 3; break;
                case 3: radioButton1.Tag = 1; radioButton2.Tag = 4; radioButton3.Tag = 3; radioButton4.Tag = 2; break;
                case 4: radioButton1.Tag = 1; radioButton2.Tag = 4; radioButton3.Tag = 2; radioButton4.Tag = 3; break;
                case 5: radioButton1.Tag = 1; radioButton2.Tag = 3; radioButton3.Tag = 4; radioButton4.Tag = 2; break;
                case 6: radioButton1.Tag = 1; radioButton2.Tag = 3; radioButton3.Tag = 2; radioButton4.Tag = 4; break;

                case 7: radioButton1.Tag = 2; radioButton2.Tag = 1; radioButton3.Tag = 3; radioButton4.Tag = 4; break;
                case 8: radioButton1.Tag = 2; radioButton2.Tag = 1; radioButton3.Tag = 4; radioButton4.Tag = 3; break;
                case 9: radioButton1.Tag = 2; radioButton2.Tag = 3; radioButton3.Tag = 1; radioButton4.Tag = 4; break;
                case 10: radioButton1.Tag = 2; radioButton2.Tag = 3; radioButton3.Tag = 4; radioButton4.Tag = 1; break;
                case 11: radioButton1.Tag = 2; radioButton2.Tag = 4; radioButton3.Tag = 1; radioButton4.Tag = 3; break;
                case 12: radioButton1.Tag = 2; radioButton2.Tag = 4; radioButton3.Tag = 3; radioButton4.Tag = 1; break;

                case 13: radioButton1.Tag = 3; radioButton2.Tag = 1; radioButton3.Tag = 2; radioButton4.Tag = 4; break;
                case 14: radioButton1.Tag = 3; radioButton2.Tag = 1; radioButton3.Tag = 4; radioButton4.Tag = 2; break;
                case 15: radioButton1.Tag = 3; radioButton2.Tag = 2; radioButton3.Tag = 1; radioButton4.Tag = 4; break;
                case 16: radioButton1.Tag = 3; radioButton2.Tag = 2; radioButton3.Tag = 4; radioButton4.Tag = 1; break;
                case 17: radioButton1.Tag = 3; radioButton2.Tag = 4; radioButton3.Tag = 1; radioButton4.Tag = 2; break;
                case 18: radioButton1.Tag = 3; radioButton2.Tag = 4; radioButton3.Tag = 2; radioButton4.Tag = 1; break;

                case 19: radioButton1.Tag = 4; radioButton2.Tag = 1; radioButton3.Tag = 2; radioButton4.Tag = 3; break;
                case 20: radioButton1.Tag = 4; radioButton2.Tag = 1; radioButton3.Tag = 3; radioButton4.Tag = 2; break;
                case 21: radioButton1.Tag = 4; radioButton2.Tag = 2; radioButton3.Tag = 1; radioButton4.Tag = 3; break;
                case 22: radioButton1.Tag = 4; radioButton2.Tag = 2; radioButton3.Tag = 3; radioButton4.Tag = 1; break;
                case 23: radioButton1.Tag = 4; radioButton2.Tag = 3; radioButton3.Tag = 1; radioButton4.Tag = 2; break;
                case 24: radioButton1.Tag = 4; radioButton2.Tag = 3; radioButton3.Tag = 2; radioButton4.Tag = 1; break;
            }
        }
        private void showResult()
        {
            label2.Visible = false;
            switch (pravilOtvetov)
            {
                case 10: mark = 5; break;
                case 9: mark = 5; break;
                case 8: mark = 4; break;
                case 7: mark = 4; break;
                case 6: mark = 3; break;
                case 5: mark = 3; break;
                case 4: mark = 3; break;
                case 3: mark = 2; break;
                case 2: mark = 2; break;
                case 1: mark = 2; break;
                case 0: mark = 2; break;
            }
            labelResult.Text = $"Правильных ответов: {pravilOtvetov}\n" +
                               $"Неправильных ответов: {nepravilOtvetov}\n" +
                               $"Ваша оценка: {mark}";



            string userName = DataBank.text;
            string queryString = $"insert into results (login_user, mark) values ('{userName}','{mark}')";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.openConnection();
            command.ExecuteNonQuery();
            dataBase.closeConnection();
        }
    }
}