using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQL_Generator.Models
{
    /// <summary>
    /// Построитель SQL-запросов типа UPDATE.
    /// Наследуется от базового класса <see cref="QueryBuilder"/>.
    /// </summary>
    public class UpdateQueryBuilder : QueryBuilder
    {
        /// <summary>
        /// Словарь, содержащий пары "столбец-значение" для обновления данных.
        /// </summary>
        private Dictionary<string, string> SetClauses { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Условие WHERE для фильтрации данных.
        /// </summary>
        private string WhereClause { get; set; }

        /// <summary>
        /// Создает новый экземпляр построителя запроса UPDATE.
        /// </summary>
        /// <param name="tableName">Имя таблицы, в которой будут обновляться данные.</param>
        public UpdateQueryBuilder(string tableName) : base(tableName) { }

        /// <summary>
        /// Добавляет пару "столбец-значение" для обновления данных.
        /// </summary>
        /// <param name="column">Имя столбца, который будет обновлен.</param>
        /// <param name="value">Новое значение для столбца.</param>
        /// <returns>Текущий экземпляр <see cref="UpdateQueryBuilder"/> для цепочки вызовов.</returns>
        public UpdateQueryBuilder AddSet(string column, string value)
        {
            SetClauses[column] = value;
            return this;
        }

        /// <summary>
        /// Добавляет условие WHERE для фильтрации данных.
        /// </summary>
        /// <param name="condition">Условие для фильтрации данных (например, "Age > 18").</param>
        /// <returns>Текущий экземпляр <see cref="UpdateQueryBuilder"/> для цепочки вызовов.</returns>
        public UpdateQueryBuilder AddWhere(string condition)
        {
            WhereClause = condition;
            return this;
        }

        /// <summary>
        /// Строит и возвращает SQL-запрос UPDATE в виде строки.
        /// </summary>
        /// <returns>SQL-запрос UPDATE с указанными параметрами.</returns>
        public override string Build()
        {
            var query = new StringBuilder();

            // UPDATE
            query.AppendLine($"UPDATE {TableName}");

            // SET
            var setClauses = string.Join(", ", SetClauses.Select(kvp => $"{kvp.Key} = {kvp.Value}"));
            query.AppendLine($"SET {setClauses}");

            // WHERE
            if (!string.IsNullOrEmpty(WhereClause))
                query.AppendLine($"WHERE {WhereClause}");

            return query.ToString().Trim() + ";";
        }
    }
}