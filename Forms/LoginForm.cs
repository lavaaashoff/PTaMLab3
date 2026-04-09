/// <summary>
/// Форма авторизации пользователя для оконного приложения Windows Forms.
/// </summary>
/// <remarks>
/// Использует <see cref="AuthLibrary.AuthManager"/> для проверки
/// учётных данных. После успешного входа закрывается с
/// <see cref="System.Windows.Forms.DialogResult.OK"/>.
/// </remarks>

using System;
using System.Drawing;
using System.Windows.Forms;
using AuthLibrary;

namespace MenuDemo
{
    /// <summary>
    /// Диалоговое окно авторизации пользователя.
    /// </summary>
    public sealed class LoginForm : Form
    {
        // ----------------------------------------------------------------
        // Элементы управления
        // ----------------------------------------------------------------

        private readonly Label _lblTitle;
        private readonly Label _lblUsername;
        private readonly Label _lblPassword;
        private readonly Label _lblVersion;
        private readonly TextBox _txtUsername;
        private readonly TextBox _txtPassword;
        private readonly Button _btnLogin;
        private readonly Button _btnCancel;
        private readonly PictureBox _picIcon;

        // ----------------------------------------------------------------
        // Поля
        // ----------------------------------------------------------------

        /// <summary>Менеджер авторизации, переданный извне.</summary>
        private readonly AuthManager _authManager;

        // ----------------------------------------------------------------
        // Конструктор
        // ----------------------------------------------------------------

        /// <summary>
        /// Инициализирует форму авторизации.
        /// </summary>
        /// <param name="authManager">
        /// Экземпляр <see cref="AuthManager"/> для проверки учётных данных.
        /// </param>
        /// <param name="appVersion">
        /// Строка версии приложения, отображаемая в окне.
        /// </param>
        public LoginForm(AuthManager authManager, string appVersion = "1.0.0")
        {
            _authManager = authManager
                ?? throw new ArgumentNullException(nameof(authManager));

            // ---- Настройка формы ----
            Text = "Авторизация";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            Size = new Size(340, 240);
            BackColor = Color.White;

            // ---- Заголовок ----
            _lblTitle = new Label
            {
                Text = "Вход в систему",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 80, 160),
                AutoSize = true,
                Location = new Point(20, 16)
            };

            // ---- Версия ----
            _lblVersion = new Label
            {
                Text = $"Версия {appVersion}",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 44)
            };

            // ---- Метка «Пользователь» ----
            _lblUsername = new Label
            {
                Text = "Пользователь:",
                AutoSize = true,
                Location = new Point(20, 76),
                Font = new Font("Segoe UI", 9F)
            };

            // ---- Поле ввода логина ----
            _txtUsername = new TextBox
            {
                Location = new Point(140, 72),
                Width = 160,
                Font = new Font("Segoe UI", 9F)
            };

            // ---- Метка «Пароль» ----
            _lblPassword = new Label
            {
                Text = "Пароль:",
                AutoSize = true,
                Location = new Point(20, 108),
                Font = new Font("Segoe UI", 9F)
            };

            // ---- Поле ввода пароля ----
            _txtPassword = new TextBox
            {
                Location = new Point(140, 104),
                Width = 160,
                PasswordChar = '●',
                Font = new Font("Segoe UI", 9F)
            };

            // ---- Кнопка «Войти» ----
            _btnLogin = new Button
            {
                Text = "Войти",
                Location = new Point(140, 144),
                Width = 75,
                Height = 28,
                BackColor = Color.FromArgb(30, 80, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F)
            };
            _btnLogin.FlatAppearance.BorderSize = 0;
            _btnLogin.Click += BtnLogin_Click;

            // ---- Кнопка «Отмена» ----
            _btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(224, 144),
                Width = 76,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                DialogResult = DialogResult.Cancel
            };

            // ---- Значок (заглушка — системная иконка «информация») ----
            _picIcon = new PictureBox
            {
                Image = SystemIcons.Shield.ToBitmap(),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(32, 32),
                Location = new Point(290, 10)
            };

            // ---- Enter / Escape ----
            AcceptButton = _btnLogin;
            CancelButton = _btnCancel;

            // ---- Добавление элементов ----
            Controls.AddRange(new Control[]
            {
                _lblTitle, _lblVersion,
                _lblUsername, _txtUsername,
                _lblPassword, _txtPassword,
                _btnLogin, _btnCancel,
                _picIcon
            });
        }

        // ----------------------------------------------------------------
        // Обработчики событий
        // ----------------------------------------------------------------

        /// <summary>
        /// Обрабатывает нажатие кнопки «Войти».
        /// </summary>
        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string username = _txtUsername.Text.Trim();
            string password = _txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show(
                    "Введите имя пользователя.",
                    "Ошибка ввода",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtUsername.Focus();
                return;
            }

            if (_authManager.Login(username, password))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Неверное имя пользователя или пароль.",
                    "Ошибка авторизации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                _txtPassword.Clear();
                _txtPassword.Focus();
            }
        }
    }
}
