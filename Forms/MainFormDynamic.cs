using System;
using System.Reflection;
using System.Windows.Forms;

namespace MenuDemo
{
    public partial class MainFormDynamic : Form
    {
        private object _auth;
        private object _menuBuilder;

        private Type _authType;
        private Type _menuType;

        public MainFormDynamic()
        {
            InitializeComponent();

            try
            {
                // Загружаем DLL
                Assembly authAsm = Assembly.LoadFrom("AuthLibrary.dll");
                Assembly menuAsm = Assembly.LoadFrom("MenuLibrary.dll");

                _authType = authAsm.GetType("AuthLibrary.AuthManager");
                _menuType = menuAsm.GetType("MenuLibrary.MenuBuilder");

                // Создаём объекты
                _auth = Activator.CreateInstance(_authType, "users.txt");

                // Показываем форму логина
                var loginForm = new LoginFormDynamic(_auth, _authType, "1.0");

                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    Close();
                    return;
                }

                // Создаём меню
                _menuBuilder = Activator.CreateInstance(_menuType, "menu.txt", this);

                MethodInfo buildMenu = _menuType.GetMethod("BuildMenu");
                buildMenu.Invoke(_menuBuilder, new object[] { menuStrip1 });

                // Применяем права
                MethodInfo apply = _authType.GetMethod("ApplyPermissions");
                apply.Invoke(_auth, new object[] { menuStrip1.Items });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}