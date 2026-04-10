namespace MenuDemo
{
    /// <summary>
    /// Частичный класс формы входа с динамической привязкой. Содержит код, сгенерированный дизайнером Windows Forms.
    /// </summary>
    partial class LoginFormDynamic
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlStripes;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label lblApp;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsLang;
        private System.Windows.Forms.ToolStripStatusLabel tsCaps;

        /// <summary>
        /// Освобождает все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">True, если управляемые ресурсы должны быть освобождены; иначе false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Метод, необходимый для поддержки конструктора форм.
        /// Не изменяйте содержимое этого метода в редакторе кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlStripes = new System.Windows.Forms.Panel();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.lblApp = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsLang = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsCaps = new System.Windows.Forms.ToolStripStatusLabel();

            this.pnlHeader.SuspendLayout();
            this.pnlStripes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.pnlStripes);
            this.pnlHeader.Controls.Add(this.pbIcon);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 65;
            this.pnlHeader.Name = "pnlHeader";

            // pnlStripes
            this.pnlStripes.BackColor = System.Drawing.Color.Transparent;
            this.pnlStripes.Controls.Add(this.lblHint);
            this.pnlStripes.Controls.Add(this.lblVersion);
            this.pnlStripes.Controls.Add(this.lblApp);
            this.pnlStripes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStripes.Name = "pnlStripes";

            // pbIcon
            this.pbIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbIcon.Image = global::DDProgram.Properties.Resources.KeysIcon;
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbIcon.Width = 70;

            // lblApp
            this.lblApp.BackColor = System.Drawing.Color.FromArgb(255, 253, 215);
            this.lblApp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblApp.Height = 22;
            this.lblApp.Name = "lblApp";
            this.lblApp.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.lblApp.Text = "АИС Отдел кадров";
            this.lblApp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // lblVersion — текст устанавливается в конструкторе после передачи строки версии.
            this.lblVersion.BackColor = System.Drawing.Color.Gold;
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVersion.Height = 21;
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // lblHint
            this.lblHint.BackColor = System.Drawing.Color.White;
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHint.Height = 22;
            this.lblHint.Name = "lblHint";
            this.lblHint.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.lblHint.Text = "Введите имя пользователя и пароль";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // lblUser
            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Location = new System.Drawing.Point(15, 85);
            this.lblUser.Name = "lblUser";
            this.lblUser.Text = "Имя пользователя";

            // txtUser
            this.txtUser.Location = new System.Drawing.Point(130, 82);
            this.txtUser.Name = "txtUser";
            this.txtUser.Width = 230;

            // lblPass
            this.lblPass.AutoSize = true;
            this.lblPass.BackColor = System.Drawing.Color.Transparent;
            this.lblPass.Location = new System.Drawing.Point(15, 115);
            this.lblPass.Name = "lblPass";
            this.lblPass.Text = "Пароль";

            // txtPass
            this.txtPass.Location = new System.Drawing.Point(130, 112);
            this.txtPass.Name = "txtPass";
            this.txtPass.UseSystemPasswordChar = true;
            this.txtPass.Width = 230;

            // btnLogin
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLogin.Location = new System.Drawing.Point(35, 150);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(80, 24);
            this.btnLogin.Text = "Вход";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            // btnCancel
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(260, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;

            // tsLang — разделитель справа создаёт визуальную границу между индикаторами.
            this.tsLang.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsLang.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsLang.Name = "tsLang";
            this.tsLang.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);

            // tsCaps
            this.tsCaps.Name = "tsCaps";
            this.tsCaps.Spring = true;
            this.tsCaps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // statusStrip
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.tsLang, this.tsCaps });
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.SizingGrip = false;

            // LoginFormDynamic
            this.AcceptButton = this.btnLogin;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(185, 209, 234);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = System.Drawing.Icon.FromHandle(global::DDProgram.Properties.Resources.icon.GetHicon());
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginFormDynamic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход";
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblUser, this.txtUser,
                this.lblPass, this.txtPass,
                this.btnLogin, this.btnCancel,
                this.pnlHeader, this.statusStrip
            });

            this.pnlHeader.ResumeLayout(false);
            this.pnlStripes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}