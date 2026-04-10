using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using AuthLibrary;

namespace MenuDemo
{
    /// <summary>
    /// Форма аутентификации пользователя в системе АИС Отдел кадров.
    /// Предоставляет поля ввода имени пользователя и пароля, отображает
    /// текущий язык ввода и состояние клавиши CapsLock в строке состояния.
    /// </summary>
    public sealed partial class LoginForm : Form
    {
        private readonly AuthManager _authManager;

        /// <summary>
        /// Инициализирует новый экземпляр формы входа.
        /// </summary>
        /// <param name="authManager">Менеджер аутентификации, выполняющий проверку учётных данных.</param>
        /// <param name="version">Строка версии приложения, отображаемая в заголовке формы.</param>
        public LoginForm(AuthManager authManager, string version)
        {
            _authManager = authManager;

            InitializeComponent();

            // Текст версии задаётся здесь, так как зависит от параметра конструктора.
            lblVersion.Text = "Версия " + (version ?? "1.0.0.0");

            // Первоначальное заполнение индикаторов строки состояния.
            UpdateLanguageLabel();
            UpdateCapsLockLabel();

            // Подписка на события изменения языка и нажатия клавиш.
            InputLanguageChanged += (s, e) => UpdateLanguageLabel();
            KeyPreview = true;
            KeyDown += (s, e) => UpdateCapsLockLabel();

            Shown += (s, e) => txtUser.Focus();
        }

        /// <summary>
        /// Обновляет метку языка ввода в строке состояния.
        /// Отображает «Русский» или «Английский» для соответствующих раскладок,
        /// для остальных языков используется английское название культуры.
        /// </summary>
        private void UpdateLanguageLabel()
        {
            string lang = InputLanguage.CurrentInputLanguage.Culture.Parent.EnglishName;
            if (lang.Contains("Russian")) lang = "Русский";
            else if (lang.Contains("English")) lang = "Английский";

            tsLang.Text = $"Язык ввода {lang}";
        }

        /// <summary>
        /// Обновляет метку состояния CapsLock в строке состояния.
        /// </summary>
        private void UpdateCapsLockLabel()
        {
            tsCaps.Text = Control.IsKeyLocked(Keys.CapsLock) ? "Клавиша CapsLock нажата" : "";
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки «Вход».
        /// При успешной аутентификации закрывает форму с результатом OK,
        /// при неудаче отображает сообщение об ошибке и возвращает фокус на поле имени пользователя.
        /// </summary>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (_authManager.Login(txtUser.Text, txtPass.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUser.Focus();
                txtUser.SelectAll();
            }
        }
    }
}