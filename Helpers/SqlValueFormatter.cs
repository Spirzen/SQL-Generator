using System.Globalization;

namespace SQL_Generator.Helpers
{
    /// <summary>
    /// Форматирует значения для подстановки в сгенерированный SQL.
    /// </summary>
    public static class SqlValueFormatter
    {
        public static string Format(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            var trimmed = value.Trim();

            if (trimmed.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                return "NULL";

            if (double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                return trimmed;

            if (bool.TryParse(trimmed, out var boolean))
                return boolean ? "TRUE" : "FALSE";

            return $"'{trimmed.Replace("'", "''", StringComparison.Ordinal)}'";
        }
    }
}
