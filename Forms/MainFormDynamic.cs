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
                
                Assembly authAsm = Assembly.LoadFrom("AuthLibrary.dll"); // Загружаем DLL.
                Assembly menuAsm = Assembly.LoadFrom("MenuLibrary.dll");

                _authType = authAsm.GetType("AuthLibrary.AuthManager");
                _menuType = menuAsm.GetType("MenuLibrary.MenuBuilder");

                
                _auth = Activator.CreateInstance(_authType, "users.txt"); // Создаём объекты.

                
                var loginForm = new LoginFormDynamic(_auth, _authType, "1.0"); // Показываем форму логина.

                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    Close();
                    return;
                }

                
                _menuBuilder = Activator.CreateInstance(_menuType, "menu.txt", this); // Создаём меню.

                MethodInfo buildMenu = _menuType.GetMethod("BuildMenu");
                buildMenu.Invoke(_menuBuilder, new object[] { menuStrip1 });

                
                MethodInfo apply = _authType.GetMethod("ApplyPermissions"); // Применяем права.
                apply.Invoke(_auth, new object[] { menuStrip1.Items });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}