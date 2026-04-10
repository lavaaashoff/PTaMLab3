using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MenuDemo
{
    /// <summary>
    /// Форма аутентификации пользователя с динамической привязкой к менеджеру авторизации.
    /// Использует рефлексию для вызова метода Login, что позволяет работать с любой
    /// реализацией менеджера аутентификации без прямой зависимости от конкретного типа.
    /// </summary>
    public sealed partial class LoginFormDynamic : Form
    {
        private readonly object _auth;
        private readonly MethodInfo _loginMethod;

        /// <summary>
        /// Инициализирует новый экземпляр формы входа с динамической привязкой.
        /// </summary>
        /// <param name="auth">Экземпляр объекта аутентификации.</param>
        /// <param name="authType">Тип объекта аутентификации для получения метода Login через рефлексию.</param>
        /// <param name="version">Строка версии приложения, отображаемая в заголовке формы.</param>
        public LoginFormDynamic(object auth, Type authType, string version)
        {
            _auth = auth;

            // Получение метода Login через рефлексию для последующего динамического вызова.
            _loginMethod = authType.GetMethod("Login");

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
        /// Вызывает метод Login через рефлексию. При успешной аутентификации закрывает форму
        /// с результатом OK, при неудаче отображает сообщение об ошибке и возвращает фокус
        /// на поле имени пользователя.
        /// </summary>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if ((bool)_loginMethod.Invoke(_auth, new object[] { txtUser.Text, txtPass.Text }))
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