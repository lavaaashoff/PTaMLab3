using AuthLibrary;
using MenuLibrary;
using System;
using System.Windows.Forms;

namespace MenuDemo.Forms
{
    public partial class MainForm : Form
    {
        private readonly AuthManager _authManager;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                _authManager = new AuthManager("users.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла пользователей:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }

            if (!ShowLoginDialog())
            {
                Load += (s, e) => Close();
                return;
            }

            LoadMenu();
            UpdateTitle();
        }

        public void Others() => ShowHandler(nameof(Others));
        public void Stuff() => ShowHandler(nameof(Stuff));
        public void Orders() => ShowHandler(nameof(Orders));
        public void Docs() => ShowHandler(nameof(Docs));
        public void Departs() => ShowHandler(nameof(Departs));
        public void Towns() => ShowHandler(nameof(Towns));
        public void Posts() => ShowHandler(nameof(Posts));
        public void Window() => ShowHandler(nameof(Window));
        public void Content() => ShowHandler(nameof(Content));
        public void About() => ShowHandler(nameof(About));

        private bool ShowLoginDialog()
        {
            string version = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version?
                .ToString(3) ?? "1.0.0";

            using var loginForm = new LoginForm(_authManager, version);
            return loginForm.ShowDialog() == DialogResult.OK;
        }

        private void LoadMenu()
        {
            try
            {
                var builder = new MenuBuilder("menu.txt", this);
                builder.BuildMenu(menuStrip1);

                _authManager.ApplyPermissions(menuStrip1.Items);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки меню:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void UpdateTitle()
        {
            Text = $"MenuDemo — {_authManager.CurrentUsername}";
        }

        private static void ShowHandler(string handlerName)
        {
            MessageBox.Show($"Вызван обработчик: {handlerName}", "Обработчик пункта меню", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
}