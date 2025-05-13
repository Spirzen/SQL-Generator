using System;
using System.Globalization;
using System.Windows.Data;

namespace SQL_Generator.Converters
{
    /// <summary>
    /// Конвертер для проверки, является ли строка не null и не пустой.
    /// Используется в XAML для привязки данных, например, для управления состоянием элементов интерфейса.
    /// </summary>
    public class StringIsNotNullOrEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует входное значение (строку) в логическое значение,
        /// указывающее, является ли строка не null и не пустой.
        /// </summary>
        /// <param name="value">Входное значение (ожидается строка).</param>
        /// <param name="targetType">Тип целевого свойства (не используется).</param>
        /// <param name="parameter">Дополнительный параметр (не используется).</param>
        /// <param name="culture">Информация о культуре (не используется).</param>
        /// <returns>True, если строка не null и не пустая; иначе False.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        /// <summary>
        /// Обратное преобразование не поддерживается.
        /// </summary>
        /// <param name="value">Входное значение (не используется).</param>
        /// <param name="targetType">Тип целевого свойства (не используется).</param>
        /// <param name="parameter">Дополнительный параметр (не используется).</param>
        /// <param name="culture">Информация о культуре (не используется).</param>
        /// <returns>Всегда выбрасывает исключение NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}