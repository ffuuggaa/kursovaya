using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
namespace курсач
{

    public class Database
    {
        private readonly string dataSource;

        public Database(string dataSource)
        {
            this.dataSource = dataSource;
        }

        public bool InitializeDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                try
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='users';";
                            object result = cmd.ExecuteScalar();

                            if (result == null)
                            {
                                string sql_command = "CREATE TABLE users("
                                    + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                                    + "login TEXT, "
                                    + "password TEXT, "
                                    + "role TEXT); ";
                                cmd.CommandText = sql_command;
                                cmd.ExecuteNonQuery();
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error initializing database: {ex.Message}");
                    return false;
                }

                return true;
            }
        }

        public bool UserExists(string login, string password)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM users WHERE login = @login AND password = @password;";
                            cmd.Parameters.AddWithValue("@login", login);
                            cmd.Parameters.AddWithValue("@password", password);

                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            return count > 0;
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {

                Console.WriteLine($"SQLite Exception: {ex.Message}");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error checking user existence: {ex.Message}");
            }

            return false;
        }

        public bool RegisterUser(string login, string password, string role)
        {



            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                try
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO users (login, password, role) VALUES (@login, @password, @role);";
                            cmd.Parameters.AddWithValue("@login", login);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@role", role);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error registering user: {ex.Message}");
                }

                return false;
            }
        }

    }


    public partial class Авторизация : Form
    {
        private readonly Database database;

        public Авторизация(Database database)
        {
            InitializeComponent();
            this.database = database;
        }

        
        

        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            Application.Exit();
        }


    }

    public partial class Регистрация : Form
    {
        private readonly Database database;

        public Регистрация(Database database)
        {
            InitializeComponent();
            this.database = database;
            this.FormClosed += Регистрация_FormClosed;
        }
        private void Регистрация_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        
        
        private bool IsLatinCharactersOnly(string input)
        {
            bool containsLetter = false;
            bool containsDigit = false;

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {

                    if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    {
                        containsLetter = true;
                    }
                }
                else if (char.IsDigit(c))
                {

                    containsDigit = true;
                }
                else
                {

                    return false;
                }
            }


            return containsLetter && containsDigit;
        }


        private void OpenLoginForm()
        {

            Авторизация loginForm = new Авторизация(database);
            loginForm.Show();
            this.Hide();
        }

    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Database database = new Database("Data Source=WindowsFormsApp2");

            if (database.InitializeDatabase())
            {

                Авторизация loginForm = new Авторизация(database);
                Application.Run(loginForm);
            }
            else
            {
                MessageBox.Show("Ошибка инициализации базы данных.");
            }
        }
    }
}
