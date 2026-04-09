/// <summary>
/// Библиотека классов для построения меню оконного приложения
/// на основе данных из внешнего текстового файла.
/// </summary>
/// <remarks>
/// Назначение: реализация программы, управляемой данными.
/// Язык: C# (.NET 6+), Windows Forms.
/// Стиль кода: рекомендации Microsoft (PascalCasing, XML-комментарии,
/// стиль отступов Allman).
/// </remarks>

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MenuLibrary
{
    /// <summary>
    /// Строитель меню: читает описание меню из текстового файла
    /// и формирует объект <see cref="MenuStrip"/> для Windows Forms.
    /// </summary>
    /// <remarks>
    /// Формат каждой строки файла меню:
    ///   Уровень Название [ИмяМетода]
    ///
    /// Уровень  — целое число (0 = пункт главного меню, 1 = подпункт и т.д.).
    /// Название — отображаемый текст пункта.
    /// ИмяМетода — необязательное имя обработчика клика; если не задано,
    ///             пункт считается родительским и открывает подменю.
    ///
    /// Пример строк:
    ///   0 Справочники
    ///   1 Отделы Departs
    ///   1 Города Towns
    /// </remarks>
    public class MenuBuilder
    {
        // ----------------------------------------------------------------
        // Поля
        // ----------------------------------------------------------------

        /// <summary>Путь к файлу с описанием меню.</summary>
        private readonly string _menuFilePath;

        /// <summary>
        /// Объект-получатель вызовов обработчиков пунктов меню.
        /// Если <c>null</c>, используется стандартный вывод через MessageBox.
        /// </summary>
        private readonly object? _handlerTarget;

        // ----------------------------------------------------------------
        // Конструкторы
        // ----------------------------------------------------------------

        /// <summary>
        /// Инициализирует экземпляр <see cref="MenuBuilder"/>.
        /// </summary>
        /// <param name="menuFilePath">
        /// Имя (путь) файла с описанием меню. По умолчанию «menu.txt».
        /// </param>
        /// <param name="handlerTarget">
        /// Объект, методы которого вызываются при выборе пунктов меню.
        /// Если <c>null</c>, вместо вызова метода отображается MessageBox.
        /// </param>
        /// <exception cref="FileNotFoundException">
        /// Выбрасывается, если указанный файл меню не найден.
        /// </exception>
        public MenuBuilder(string menuFilePath = "menu.txt", object? handlerTarget = null)
        {
            if (!File.Exists(menuFilePath))
            {
                throw new FileNotFoundException(
                    $"Файл меню не найден: {menuFilePath}", menuFilePath);
            }

            _menuFilePath = menuFilePath;
            _handlerTarget = handlerTarget;
        }

        // ----------------------------------------------------------------
        // Открытые методы
        // ----------------------------------------------------------------

        /// <summary>
        /// Читает файл меню и заполняет переданный <see cref="MenuStrip"/>
        /// сформированными пунктами.
        /// </summary>
        /// <param name="menuStrip">
        /// Элемент управления <see cref="MenuStrip"/>, который будет заполнен.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="menuStrip"/> равен <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// Выбрасывается при обнаружении некорректной строки в файле меню.
        /// </exception>
        public void BuildMenu(System.Windows.Forms.MenuStrip menuStrip)
        {
            if (menuStrip == null)
                throw new ArgumentNullException(nameof(menuStrip));

            menuStrip.Items.Clear();

            List<MenuRecord> records = ParseMenuFile(_menuFilePath);
            BuildMenuItems(menuStrip, records);
        }

        // ----------------------------------------------------------------
        // Вспомогательные методы
        // ----------------------------------------------------------------

        /// <summary>
        /// Разбирает файл меню и возвращает список записей
        /// <see cref="MenuRecord"/>.
        /// </summary>
        /// <param name="filePath">Путь к файлу меню.</param>
        /// <returns>Список разобранных записей.</returns>
        private static List<MenuRecord> ParseMenuFile(string filePath)
        {
            var records = new List<MenuRecord>();
            int lineNumber = 0;

            foreach (string rawLine in File.ReadLines(filePath))
            {
                lineNumber++;

                // Пропускаем пустые строки и строки-комментарии (начинаются с #).
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith('#'))
                    continue;

                // Разбиваем строку на части: Уровень Название [ИмяМетода]
                string[] parts = line.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 2)
                {
                    throw new FormatException(
                        $"Строка {lineNumber} файла меню имеет неверный формат: «{rawLine}»");
                }

                if (!int.TryParse(parts[0], out int level) || level < 0)
                {
                    throw new FormatException(
                        $"Строка {lineNumber}: «{parts[0]}» не является корректным номером уровня.");
                }

                string title = parts[1];
                string? handlerName = parts.Length >= 3 ? parts[2] : null;

                records.Add(new MenuRecord(level, title, handlerName));
            }

            return records;
        }

        /// <summary>
        /// Рекурсивно строит иерархию <see cref="ToolStripMenuItem"/>
        /// и добавляет корневые пункты в <paramref name="menuStrip"/>.
        /// </summary>
        /// <param name="menuStrip">Целевой MenuStrip.</param>
        /// <param name="records">Плоский список разобранных записей.</param>
        private void BuildMenuItems(MenuStrip menuStrip, List<MenuRecord> records)
        {
            // Стек хранит пары (уровень, родительский элемент меню).
            // Используем стек для отслеживания текущего пути в иерархии.
            var parentStack = new Stack<(int Level, ToolStripMenuItem Item)>();

            foreach (MenuRecord record in records)
            {
                ToolStripMenuItem menuItem = CreateMenuItem(record);

                if (record.Level == 0)
                {
                    // Пункт верхнего уровня — добавляем в MenuStrip.
                    menuStrip.Items.Add(menuItem);
                    parentStack.Clear();
                    parentStack.Push((0, menuItem));
                }
                else
                {
                    // Поднимаемся по стеку до подходящего родителя.
                    while (parentStack.Count > 0 && parentStack.Peek().Level >= record.Level)
                        parentStack.Pop();

                    if (parentStack.Count == 0)
                    {
                        throw new FormatException(
                            $"Пункт «{record.Title}» уровня {record.Level} " +
                            "не имеет подходящего родителя.");
                    }

                    parentStack.Peek().Item.DropDownItems.Add(menuItem);
                    parentStack.Push((record.Level, menuItem));
                }
            }
        }

        /// <summary>
        /// Создаёт <see cref="ToolStripMenuItem"/> на основе записи
        /// <see cref="MenuRecord"/> и назначает обработчик события Click.
        /// </summary>
        /// <param name="record">Запись файла меню.</param>
        /// <returns>Готовый элемент меню.</returns>
        private ToolStripMenuItem CreateMenuItem(MenuRecord record)
        {
            var item = new ToolStripMenuItem(record.Title)
            {
                // Сохраняем имя обработчика в теге для возможного позднейшего использования.
                Tag = record.HandlerName
            };

            if (!string.IsNullOrEmpty(record.HandlerName))
            {
                // Захватываем значения для замыкания.
                string handlerName = record.HandlerName;
                string title = record.Title;

                item.Click += (sender, args) => InvokeHandler(handlerName, title);
            }

            return item;
        }

        /// <summary>
        /// Вызывает метод с именем <paramref name="methodName"/> у объекта
        /// <see cref="_handlerTarget"/> или отображает MessageBox, если
        /// целевой объект не задан или метод не найден.
        /// </summary>
        /// <param name="methodName">Имя метода-обработчика.</param>
        /// <param name="itemTitle">Отображаемое название пункта меню.</param>
        private void InvokeHandler(string methodName, string itemTitle)
        {
            if (_handlerTarget != null)
            {
                Type targetType = _handlerTarget.GetType();
                MethodInfo? method = targetType.GetMethod(
                    methodName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

                if (method != null)
                {
                    method.Invoke(_handlerTarget, null);
                    return;
                }
            }

            // Метод не найден или объект не задан — вывод уведомления.
            MessageBox.Show(
                $"Выбран пункт: {itemTitle}\nОбработчик: {methodName}",
                "Обработчик пункта меню",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ----------------------------------------------------------------
        // Вложенный тип
        // ----------------------------------------------------------------

        /// <summary>
        /// Представляет одну разобранную запись файла меню.
        /// </summary>
        private sealed class MenuRecord
        {
            /// <summary>Уровень вложенности (0 — главное меню).</summary>
            public int Level { get; }

            /// <summary>Отображаемый текст пункта.</summary>
            public string Title { get; }

            /// <summary>
            /// Имя метода-обработчика клика.
            /// <c>null</c> или пустая строка означает, что пункт открывает подменю.
            /// </summary>
            public string? HandlerName { get; }

            /// <summary>
            /// Инициализирует запись меню.
            /// </summary>
            public MenuRecord(int level, string title, string? handlerName)
            {
                Level = level;
                Title = title;
                HandlerName = string.IsNullOrWhiteSpace(handlerName) ? null : handlerName;
            }
        }
    }
}