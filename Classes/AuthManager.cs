/*
 * Закомментировано для корректной сборки проекта, именно это содержится в dll файле
/// <summary>
/// Библиотека классов для авторизации пользователей и управления
/// доступом к пунктам меню на основе данных из внешнего файла USERS.
/// </summary>
/// <remarks>
/// Назначение: реализация системы ролей в программе, управляемой данными.
/// Язык: C# (.NET 8.0), Windows Forms.
/// Стиль кода: рекомендации Microsoft (PascalCasing, XML-комментарии,
/// стиль отступов Allman).
/// </remarks>

using System;
using System.Collections.Generic;
using System.IO;

namespace AuthLibraryClass
{
    /// <summary>
    /// Перечисление возможных статусов видимости пункта меню для пользователя.
    /// </summary>
    public enum MenuItemStatus
    {
        /// <summary>Пункт виден и доступен (по умолчанию).</summary>
        VisibleEnabled = 0,

        /// <summary>Пункт виден, но недоступен (серый).</summary>
        VisibleDisabled = 1,

        /// <summary>Пункт полностью скрыт.</summary>
        Hidden = 2
    }

    // ====================================================================
    // Внутренние типы данных
    // ====================================================================

    /// <summary>
    /// Представляет одну запись о правах пользователя на пункт меню.
    /// </summary>
    internal sealed class UserMenuRule
    {
        /// <summary>Отображаемое название пункта меню.</summary>
        public string ItemTitle { get; }

        /// <summary>Статус пункта для данного пользователя.</summary>
        public MenuItemStatus Status { get; }

        /// <summary>Инициализирует запись.</summary>
        public UserMenuRule(string itemTitle, MenuItemStatus status)
        {
            ItemTitle = itemTitle;
            Status = status;
        }
    }

    /// <summary>
    /// Представляет учётную запись пользователя вместе с правами на пункты меню.
    /// </summary>
    internal sealed class UserAccount
    {
        /// <summary>Имя пользователя (логин).</summary>
        public string Username { get; }

        /// <summary>Пароль в открытом виде.</summary>
        public string Password { get; }

        /// <summary>
        /// Словарь прав: ключ — название пункта меню, значение — статус.
        /// </summary>
        public Dictionary<string, MenuItemStatus> Rules { get; }
            = new Dictionary<string, MenuItemStatus>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Инициализирует учётную запись.</summary>
        public UserAccount(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    // ====================================================================
    // Основной публичный класс
    // ====================================================================

    /// <summary>
    /// Менеджер авторизации: читает файл пользователей и предоставляет
    /// методы проверки учётных данных и получения статуса пунктов меню.
    /// </summary>
    /// <remarks>
    /// Формат файла пользователей (по умолчанию users.txt):
    /// <code>
    /// #Имя_пользователя Пароль
    /// Название_пункта Статус
    /// Другие_пункты    Статус
    ///
    /// #Следующий_пользователь Пароль
    /// ...
    /// </code>
    ///
    /// Строки, начинающиеся с «#», открывают блок нового пользователя.
    /// Пустые строки и строки-комментарии (начинаются с «//») игнорируются.
    ///
    /// Статусы пунктов:
    ///   0 — виден и доступен (по умолчанию для отсутствующих пунктов);
    ///   1 — виден, но недоступен;
    ///   2 — скрыт.
    /// </remarks>
    public class AuthManager
    {
        /// <summary>Словарь учётных записей: ключ — имя в нижнем регистре.</summary>
        private readonly Dictionary<string, UserAccount> _accounts = new Dictionary<string, UserAccount>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Текущий авторизованный пользователь; null — не авторизован.</summary>
        private UserAccount? _currentUser;

        /// <summary>
        /// Читает и разбирает файл пользователей.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        private void ParseUsersFile(string filePath)
        {
            UserAccount? currentAccount = null;
            int lineNumber = 0;

            foreach (string rawLine in File.ReadLines(filePath))
            {
                lineNumber++;

                // Пропускаем пустые строки и строки-комментарии (//).
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("//")) continue;

                if (line.StartsWith('#'))
                {
                    // Новый блок пользователя: #Имя Пароль
                    string[] parts = line.Substring(1).Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2)
                    {
                        throw new FormatException($"Строка {lineNumber}: блок пользователя должен содержать " + $"имя и пароль: «{rawLine}»");
                    }

                    currentAccount = new UserAccount(parts[0], parts[1]);
                    _accounts[currentAccount.Username] = currentAccount;
                }
                else
                {
                    // Строка прав: Название_пункта Статус
                    if (currentAccount == null)
                    {
                        throw new FormatException($"Строка {lineNumber}: запись прав без блока пользователя: " + $"«{rawLine}»");
                    }

                    string[] parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2)
                    {
                        throw new FormatException($"Строка {lineNumber}: запись прав должна содержать " + $"название пункта и статус: «{rawLine}»");
                    }

                    if (!int.TryParse(parts[1], out int statusCode) || statusCode < 0 || statusCode > 2)
                    {
                        throw new FormatException($"Строка {lineNumber}: статус «{parts[1]}» должен быть " + "числом 0, 1 или 2.");
                    }

                    currentAccount.Rules[parts[0]] = (MenuItemStatus)statusCode;
                }
            }
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="AuthManager"/> и загружает данные из файла пользователей.
        /// </summary>
        /// <param name="usersFilePath">
        /// Путь к файлу пользователей. По умолчанию «users.txt».
        /// </param>
        /// <exception cref="FileNotFoundException">
        /// Выбрасывается, если файл не найден.
        /// </exception>
        /// <exception cref="FormatException">
        /// Выбрасывается при обнаружении некорректной строки в файле.
        /// </exception>
        public AuthManager(string usersFilePath = "users.txt")
        {
            if (!File.Exists(usersFilePath))
            {
                throw new FileNotFoundException($"Файл пользователей не найден: {usersFilePath}", usersFilePath);
            }

            ParseUsersFile(usersFilePath);
        }

        /// <summary>
        /// Возвращает имя текущего авторизованного пользователя
        /// или <c>null</c>, если авторизация не выполнена.
        /// </summary>
        public string? CurrentUsername => _currentUser?.Username;

        /// <summary>
        /// Возвращает <c>true</c>, если пользователь авторизован.
        /// </summary>
        public bool IsAuthenticated => _currentUser != null;

        /// <summary>
        /// Проверяет учётные данные и, если они корректны, устанавливает
        /// текущего пользователя.
        /// </summary>
        /// <param name="username">Введённое имя пользователя.</param>
        /// <param name="password">Введённый пароль.</param>
        /// <returns>
        /// <c>true</c> — авторизация успешна; <c>false</c> — данные неверны.
        /// </returns>
        public bool Login(string username, string password)
        {
            if (_accounts.TryGetValue(username, out UserAccount? account) && account.Password == password)
            {
                _currentUser = account;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Завершает сеанс текущего пользователя.
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Возвращает статус пункта меню для текущего авторизованного пользователя.
        /// </summary>
        /// <param name="itemTitle">Название пункта меню.</param>
        /// <returns>
        /// Статус пункта. Если пользователь не авторизован или пункт
        /// не упомянут в файле прав — возвращает
        /// <see cref="MenuItemStatus.VisibleEnabled"/>.
        /// </returns>
        public MenuItemStatus GetItemStatus(string itemTitle)
        {
            if (_currentUser == null) return MenuItemStatus.VisibleEnabled;

            if (_currentUser.Rules.TryGetValue(itemTitle, out MenuItemStatus status)) return status;

            // Пункт не упомянут по умолчанию виден и доступен.
            return MenuItemStatus.VisibleEnabled;
        }

        /// <summary>
        /// Применяет права текущего пользователя ко всем пунктам
        /// коллекции <see cref="System.Windows.Forms.ToolStripItemCollection"/>.
        /// </summary>
        /// <param name="items">
        /// Коллекция пунктов <see cref="System.Windows.Forms.ToolStripItem"/>.
        /// </param>
        /// <remarks>
        /// Метод рекурсивно обходит вложенные подменю.
        /// </remarks>
        public void ApplyPermissions(System.Windows.Forms.ToolStripItemCollection items)
        {
            foreach (System.Windows.Forms.ToolStripItem item in items)
            {
                MenuItemStatus status = GetItemStatus(item.Text);

                switch (status)
                {
                    case MenuItemStatus.VisibleEnabled:
                        item.Visible = true;
                        item.Enabled = true;
                        break;

                    case MenuItemStatus.VisibleDisabled:
                        item.Visible = true;
                        item.Enabled = false;
                        break;

                    case MenuItemStatus.Hidden:
                        item.Visible = false;
                        break;
                }

                // Рекурсивно обрабатываем подпункты.
                if (item is System.Windows.Forms.ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                {
                    ApplyPermissions(menuItem.DropDownItems);
                }
            }
        }
    }
}
*/