namespace MenuDemo
{
    /// <summary>
    /// Частичный класс формы входа. Содержит код, сгенерированный дизайнером Windows Forms.
    /// </summary>
    partial class LoginForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Text = "Вход";
        }

        #endregion
    }
}