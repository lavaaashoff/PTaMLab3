using System;
using System.Windows.Forms;
using MenuLibrary;

namespace MenuDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent(); // вызов из Designer.cs
            LoadMenu();
        }

        // Обработчики пунктов меню
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

        private void LoadMenu()
        {
            try
            {
                var builder = new MenuBuilder("menu.txt", this);
                builder.BuildMenu(menuStrip1); // имя из Designer
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