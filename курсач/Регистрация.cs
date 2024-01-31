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
    public partial class Регистрация : Form
    {
        public Регистрация()
        {
            InitializeComponent();
        }

        private void Regg_Click(object sender, EventArgs e)
        {
            string login = Reg.Text;
            string password = pas.Text;
            string confirmPassword = pas1.Text;

            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmPassword))
            {

                if (database.UserExists(login, password))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует. Пожалуйста, выберите другой логин.");
                    return;
                }

                if (password.Length < 6 || !IsLatinCharactersOnly(password))
                {
                    MessageBox.Show("Пароль должен состоять как минимум из 6 символов и содержать только латинские буквы и цифры.");
                }
                else if (password != confirmPassword)
                {
                    MessageBox.Show("Пароль и подтверждение пароля не совпадают. Пожалуйста, проверьте введенные данные.");
                }
                else
                {
                    if (database.RegisterUser(login, password, "user"))
                    {
                        MessageBox.Show("Регистрация прошла успешно!");
                        OpenLoginForm();
                        this.Hide();
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите все необходимые данные.");
            }
        }
    }

}