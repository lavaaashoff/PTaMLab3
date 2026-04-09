/// <summary>
/// Главная форма приложения с интеграцией авторизации и меню,
/// управляемого данными.
/// </summary>
/// <remarks>
/// Запускает окно авторизации. При успешном входе строит меню
/// из файла menu.txt и применяет права текущего пользователя
/// через <see cref="AuthLibrary.AuthManager"/>.
/// </remarks>

using AuthLibrary;
using MenuDemo.Classes;
using System;
using System.Windows.Forms;

namespace MenuDemo.Forms
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainForm : Form
    {
        // ----------------------------------------------------------------
        // Поля
        // ----------------------------------------------------------------

        /// <summary>Менеджер авторизации (загружается один раз).</summary>
        private readonly AuthManager _authManager;

        // ----------------------------------------------------------------
        // Конструктор
        // ----------------------------------------------------------------

        /// <summary>
        /// Инициализирует главную форму и запускает авторизацию.
        /// </summary>
        public MainForm()
        {
            InitializeComponent(); // вызов из Designer.cs

            // Загружаем менеджер авторизации.
            try
            {
                _authManager = new AuthManager("users.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки файла пользователей:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(1);
                return; // успокаиваем анализатор null-ссылок
            }

            // Показываем форму авторизации до появления главного окна.
            if (!ShowLoginDialog())
            {
                // Пользователь нажал «Отмена» — закрываем приложение.
                // Load ещё не вызван, поэтому используем отложенное закрытие.
                Load += (s, e) => Close();
                return;
            }

            // Строим меню и применяем права.
            LoadMenu();
            UpdateTitle();
        }

        // ----------------------------------------------------------------
        // Обработчики пунктов меню
        // ----------------------------------------------------------------

        /// <summary>Вызывается при выборе пункта «Разное».</summary>
        public void Others() => ShowHandler(nameof(Others));

        /// <summary>Вызывается при выборе пункта «Сотрудники».</summary>
        public void Stuff() => ShowHandler(nameof(Stuff));

        /// <summary>Вызывается при выборе пункта «Приказы».</summary>
        public void Orders() => ShowHandler(nameof(Orders));

        /// <summary>Вызывается при выборе пункта «Документы».</summary>
        public void Docs() => ShowHandler(nameof(Docs));

        /// <summary>Вызывается при выборе пункта «Отделы».</summary>
        public void Departs() => ShowHandler(nameof(Departs));

        /// <summary>Вызывается при выборе пункта «Города».</summary>
        public void Towns() => ShowHandler(nameof(Towns));

        /// <summary>Вызывается при выборе пункта «Должности».</summary>
        public void Posts() => ShowHandler(nameof(Posts));

        /// <summary>Вызывается при выборе пункта «Окно».</summary>
        public void Window() => ShowHandler(nameof(Window));

        /// <summary>Вызывается при выборе пункта «Оглавление».</summary>
        public void Content() => ShowHandler(nameof(Content));

        /// <summary>Вызывается при выборе пункта «О программе».</summary>
        public void About() => ShowHandler(nameof(About));

        // ----------------------------------------------------------------
        // Вспомогательные методы
        // ----------------------------------------------------------------

        /// <summary>
        /// Отображает диалог авторизации.
        /// </summary>
        /// <returns>
        /// <c>true</c> — пользователь успешно вошёл в систему;
        /// <c>false</c> — вход отменён.
        /// </returns>
        private bool ShowLoginDialog()
        {
            // Получаем версию из атрибутов сборки.
            string version = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version?
                .ToString(3) ?? "1.0.0";

            using var loginForm = new LoginForm(_authManager, version);
            return loginForm.ShowDialog() == DialogResult.OK;
        }

        /// <summary>
        /// Загружает меню из файла и применяет права текущего пользователя.
        /// </summary>
        private void LoadMenu()
        {
            try
            {
                var builder = new MenuBuilder("menu.txt", this);
                builder.BuildMenu(menuStrip1); // имя из Designer

                // Применяем права пользователя ко всем пунктам меню.
                _authManager.ApplyPermissions(menuStrip1.Items);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки меню:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обновляет заголовок главного окна, добавляя имя пользователя.
        /// </summary>
        private void UpdateTitle()
        {
            Text = $"MenuDemo — {_authManager.CurrentUsername}";
        }

        /// <summary>
        /// Выводит MessageBox с именем вызванного обработчика.
        /// </summary>
        private static void ShowHandler(string handlerName)
        {
            MessageBox.Show(
                $"Вызван обработчик: {handlerName}",
                "Обработчик пункта меню",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
}