using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace курсач
{
    public partial class Атбаш : Form
    {
        private bool isTranslated = false;
        private string alphabet = "";
        

        public Атбаш()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            button2.Click += SaveButton_Click;
            button2.Enabled = false;

        }

        public string Encrypt(string text, string alphabet)
        {
            string encryptedText = "";
            int alphabetLength = alphabet.Length;

            foreach (char c in text)
            {
                int charIndex = alphabet.IndexOf(char.ToUpper(c));

                if (charIndex >= 0)
                {
                    char encryptedChar = alphabet[alphabetLength - 1 - charIndex];
                    encryptedText += char.IsLower(c) ? char.ToLower(encryptedChar) : encryptedChar;
                }
                else
                {
                    encryptedText += c;
                }
            }

            return encryptedText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputText = tb.Text;
            if (!string.IsNullOrWhiteSpace(inputText))
            {
                int languageChoice = AutomaticallyDetectLanguage(inputText); // Автоматическое определение языка
                InitializeAlphabet(languageChoice); // Метод для инициализации алфавита

                string encryptedText = Encrypt(inputText, alphabet);
                Atbash.Text = encryptedText;
                isTranslated = true;
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("Введите текст для шифрования.", "Ошибка");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (isTranslated)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы|*.txt";
                saveFileDialog.Title = "Сохранить переведенное выражение";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    File.WriteAllText(saveFileDialog.FileName, Atbash.Text);
                }
            }
            else
            {
                MessageBox.Show("Сначала выполните шифрование текста.", "Ошибка");
            }
        }
        private void InitializeAlphabet(int languageChoice)
        {
            switch (languageChoice)
            {
                case 1:
                    alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                    break;
                case 2:
                    alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                default:
                    MessageBox.Show("Не удалось определить язык.", "Ошибка");
                    break;
            }
        }

        private int AutomaticallyDetectLanguage(string text)
        {
            // Создаем словари для подсчета частоты букв в тексте для разных языков
            Dictionary<char, int> russianFreq = new Dictionary<char, int>();
            Dictionary<char, int> englishFreq = new Dictionary<char, int>();

            // Заполняем словари нулями
            for (char c = 'а'; c <= 'я'; c++)
            {
                russianFreq[c] = 0;
            }

            for (char c = 'a'; c <= 'z'; c++)
            {
                englishFreq[c] = 0;
            }

            // Подсчитываем частоту букв в тексте
            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (russianFreq.ContainsKey(c))
                    {
                        russianFreq[c]++;
                    }
                    else if (englishFreq.ContainsKey(c))
                    {
                        englishFreq[c]++;
                    }
                }
            }

            // Находим букву с максимальной частотой в каждом языке
            char maxRussianChar = russianFreq.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            char maxEnglishChar = englishFreq.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            // Сравниваем частоту букв с наибольшей частотой для русского и английского языка
            if (russianFreq[maxRussianChar] > englishFreq[maxEnglishChar])
            {
                return 1; // Русский язык
            }
            else
            {
                return 2; // Английский язык
            }
        }


        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void back_Click_1(object sender, EventArgs e)
        {
            Авторизация LoginForm = new Авторизация();
            LoginForm.Show();
            this.Hide();
        }
    }
}
