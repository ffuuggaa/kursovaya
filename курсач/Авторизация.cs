using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсач
{
    public partial class Авторизация : Form
    {
        public Авторизация()
        {
            InitializeComponent();
            
        }


        private void Reg_Click(object sender, EventArgs e)
        {
            OpenRegistrationForm();
            this.Hide();
        }


       
        private void OpenRegistrationForm()
        {
            this.Hide();
            Регистрация registrationForm = new Регистрация(database);
            registrationForm.ShowDialog();

        }

        private void Авторизация_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Voiti_Click(object sender, EventArgs e)
        {
            string login = Logg.Text;
            string password = pass.Text;

            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                if (database.UserExists(login, password))
                {
                    MessageBox.Show("Вход выполнен успешно!");
                    OpenMainForm();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден. Пожалуйста, попробуйте снова.");

                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
            }
        }
        private void OpenMainForm()
        {
            Атбаш mainForm = new Атбаш();
            mainForm.FormClosed += MainForm_FormClosed;
            mainForm.Show();
            this.Hide();
        }
    }
}
