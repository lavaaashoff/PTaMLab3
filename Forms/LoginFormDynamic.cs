using System;
using System.Drawing;
using System.Reflection;
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

        private TextBox txtUser;
        private TextBox txtPass;
        private ToolStripStatusLabel tsCaps;
        private ToolStripStatusLabel tsLang;

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

            // Настройка основных свойств формы.
            Text = "Вход";
            Size = new Size(390, 250);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Microsoft Sans Serif", 8.25F);
            BackColor = Color.FromArgb(185, 209, 234);
            Icon = SystemIcons.Application;

            // Верхняя панель с иконкой и информационными полосами.
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.Transparent
            };

            var pbIcon = new PictureBox
            {
                Dock = DockStyle.Left,
                Width = 70,
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
                Image = global::DDProgram.Properties.Resources.KeysIcon
            };

            var pnlStripes = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Три цветовые полосы: название системы, версия, подсказка.
            var lblApp = new Label
            {
                Text = "АИС Отдел кадров",
                Dock = DockStyle.Top,
                Height = 22,
                BackColor = Color.FromArgb(255, 253, 215),
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0)
            };

            var lblVersion = new Label
            {
                Text = "Версия " + (version ?? "1.0.0.0"),
                Dock = DockStyle.Top,
                Height = 21,
                BackColor = Color.Gold,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0)
            };

            var lblHint = new Label
            {
                Text = "Введите имя пользователя и пароль",
                Dock = DockStyle.Top,
                Height = 22,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0)
            };

            pnlStripes.Controls.Add(lblHint);
            pnlStripes.Controls.Add(lblVersion);
            pnlStripes.Controls.Add(lblApp);
            pnlHeader.Controls.Add(pnlStripes);
            pnlHeader.Controls.Add(pbIcon);

            // Поля ввода учётных данных.
            var lblUser = new Label { Text = "Имя пользователя", Location = new Point(15, 85), AutoSize = true, BackColor = Color.Transparent };
            txtUser = new TextBox { Location = new Point(130, 82), Width = 230, Text = "" };

            var lblPass = new Label { Text = "Пароль", Location = new Point(15, 115), AutoSize = true, BackColor = Color.Transparent };
            txtPass = new TextBox { Location = new Point(130, 112), Width = 230, UseSystemPasswordChar = true, Text = "" };

            // Кнопки подтверждения и отмены входа.
            var btnLogin = new Button { Text = "Вход", Location = new Point(35, 150), Size = new Size(80, 24), UseVisualStyleBackColor = true };
            var btnCancel = new Button { Text = "Отмена", Location = new Point(260, 150), Size = new Size(80, 24), UseVisualStyleBackColor = true, DialogResult = DialogResult.Cancel };

            // Строка состояния с индикаторами языка ввода и CapsLock.
            var statusStrip = new StatusStrip { BackColor = Color.Transparent, SizingGrip = false };

            // Разделитель между индикатором языка и индикатором CapsLock создаётся стилем границы Etched.
            tsLang = new ToolStripStatusLabel
            {
                BorderSides = ToolStripStatusLabelBorderSides.Right,
                BorderStyle = Border3DStyle.Etched,
                Padding = new Padding(0, 0, 5, 0)
            };

            tsCaps = new ToolStripStatusLabel { Spring = true, TextAlign = ContentAlignment.MiddleRight };

            statusStrip.Items.Add(tsLang);
            statusStrip.Items.Add(tsCaps);

            Controls.AddRange(new Control[] { lblUser, txtUser, lblPass, txtPass, btnLogin, btnCancel, pnlHeader, statusStrip });

            // Первоначальное заполнение индикаторов строки состояния.
            UpdateLanguageLabel();
            UpdateCapsLockLabel();

            // Подписка на события изменения языка и нажатия клавиш.
            InputLanguageChanged += (s, e) => UpdateLanguageLabel();
            KeyPreview = true;
            KeyDown += (s, e) => UpdateCapsLockLabel();

            AcceptButton = btnLogin;
            CancelButton = btnCancel;
            btnLogin.Click += BtnLogin_Click;
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