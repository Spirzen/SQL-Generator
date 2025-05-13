using System.Collections.Generic;
using System.Text;

namespace SQL_Generator.Models
{
    /// <summary>
    /// Построитель SQL-запросов типа INSERT.
    /// Наследуется от базового класса <see cref="QueryBuilder"/>.
    /// </summary>
    public class InsertQueryBuilder : QueryBuilder
    {
        /// <summary>
        /// Словарь, содержащий пары "столбец-значение" для вставки данных.
        /// </summary>
        private Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Условие WHERE для запроса INSERT (опционально).
        /// </summary>
        private string WhereClause { get; set; }

        /// <summary>
        /// Создает новый экземпляр построителя запроса INSERT.
        /// </summary>
        /// <param name="tableName">Имя таблицы, в которую будут вставляться данные.</param>
        public InsertQueryBuilder(string tableName) : base(tableName) { }

        /// <summary>
        /// Добавляет значение для вставки в указанный столбец.
        /// </summary>
        /// <param name="column">Имя столбца.</param>
        /// <param name="value">Значение для вставки в столбец.</param>
        /// <returns>Текущий экземпляр <see cref="InsertQueryBuilder"/> для цепочки вызовов.</returns>
        public InsertQueryBuilder AddValue(string column, string value)
        {
            Values[column] = value;
            return this;
        }

        /// <summary>
        /// Добавляет условие WHERE к запросу INSERT (если требуется).
        /// </summary>
        /// <param name="condition">Условие для фильтрации данных (например, "Age > 18").</param>
        /// <returns>Текущий экземпляр <see cref="InsertQueryBuilder"/> для цепочки вызовов.</returns>
        public InsertQueryBuilder AddWhere(string condition)
        {
            WhereClause = condition;
            return this;
        }

        /// <summary>
        /// Строит и возвращает SQL-запрос INSERT в виде строки.
        /// </summary>
        /// <returns>SQL-запрос INSERT с указанными параметрами.</returns>
        public override string Build()
        {
            var query = new StringBuilder();

            // INSERT INTO
            query.AppendLine($"INSERT INTO {TableName} ({string.Join(", ", Values.Keys)})");

            // VALUES
            query.AppendLine($"VALUES ({string.Join(", ", Values.Values)})");

            // WHERE (если есть)
            if (!string.IsNullOrEmpty(WhereClause))
                query.AppendLine($"WHERE {WhereClause}");

            return query.ToString().Trim() + ";";
        }
    }
}