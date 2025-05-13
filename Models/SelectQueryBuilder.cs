using System.Collections.Generic;
using System.Text;

namespace SQL_Generator.Models
{
    /// <summary>
    /// Построитель SQL-запросов типа SELECT.
    /// Наследуется от базового класса <see cref="QueryBuilder"/>.
    /// </summary>
    public class SelectQueryBuilder : QueryBuilder
    {
        /// <summary>
        /// Список столбцов, которые будут выбираться в запросе.
        /// Если список пуст, выбираются все столбцы (*).
        /// </summary>
        private List<string> Columns { get; set; } = new List<string>();

        /// <summary>
        /// Условие WHERE для фильтрации данных.
        /// </summary>
        private string WhereClause { get; set; }

        /// <summary>
        /// Условие ORDER BY для сортировки данных.
        /// </summary>
        private string OrderByClause { get; set; }

        /// <summary>
        /// Лимит количества возвращаемых строк.
        /// </summary>
        private int? LimitValue { get; set; }

        /// <summary>
        /// Список условий JOIN для объединения таблиц.
        /// </summary>
        private List<string> JoinClauses { get; set; } = new List<string>();

        /// <summary>
        /// Создает новый экземпляр построителя запроса SELECT.
        /// </summary>
        /// <param name="tableName">Имя таблицы, из которой будут выбираться данные.</param>
        public SelectQueryBuilder(string tableName) : base(tableName) { }

        /// <summary>
        /// Добавляет один или несколько столбцов для выборки.
        /// </summary>
        /// <param name="columns">Коллекция имен столбцов.</param>
        /// <returns>Текущий экземпляр <see cref="SelectQueryBuilder"/> для цепочки вызовов.</returns>
        public SelectQueryBuilder AddColumns(IEnumerable<string> columns)
        {
            Columns.AddRange(columns);
            return this;
        }

        /// <summary>
        /// Добавляет условие WHERE для фильтрации данных.
        /// </summary>
        /// <param name="condition">Условие для фильтрации данных (например, "Age > 18").</param>
        /// <returns>Текущий экземпляр <see cref="SelectQueryBuilder"/> для цепочки вызовов.</returns>
        public SelectQueryBuilder AddWhere(string condition)
        {
            WhereClause = condition;
            return this;
        }

        /// <summary>
        /// Добавляет условие ORDER BY для сортировки данных.
        /// </summary>
        /// <param name="orderBy">Условие сортировки (например, "Name ASC").</param>
        /// <returns>Текущий экземпляр <see cref="SelectQueryBuilder"/> для цепочки вызовов.</returns>
        public SelectQueryBuilder AddOrderBy(string orderBy)
        {
            OrderByClause = orderBy;
            return this;
        }

        /// <summary>
        /// Добавляет лимит количества возвращаемых строк.
        /// </summary>
        /// <param name="limit">Максимальное количество строк для выборки.</param>
        /// <returns>Текущий экземпляр <see cref="SelectQueryBuilder"/> для цепочки вызовов.</returns>
        public SelectQueryBuilder AddLimit(int limit)
        {
            LimitValue = limit;
            return this;
        }

        /// <summary>
        /// Добавляет условие JOIN для объединения таблиц.
        /// </summary>
        /// <param name="joinType">Тип соединения (например, INNER, LEFT, RIGHT).</param>
        /// <param name="table">Имя таблицы для соединения.</param>
        /// <param name="column">Столбец из соединяемой таблицы.</param>
        /// <param name="mainTableColumn">Столбец из основной таблицы.</param>
        /// <returns>Текущий экземпляр <see cref="SelectQueryBuilder"/> для цепочки вызовов.</returns>
        public SelectQueryBuilder AddJoin(string joinType, string table, string column, string mainTableColumn)
        {
            var joinClause = $"{joinType} JOIN {table} ON {table}.{column} = {TableName}.{mainTableColumn}";
            JoinClauses.Add(joinClause);
            return this;
        }

        /// <summary>
        /// Строит и возвращает SQL-запрос SELECT в виде строки.
        /// </summary>
        /// <returns>SQL-запрос SELECT с указанными параметрами.</returns>
        public override string Build()
        {
            var query = new StringBuilder();

            // SELECT
            query.AppendLine($"SELECT {string.Join(", ", Columns.Count > 0 ? Columns : new[] { "*" })}");

            // FROM
            query.AppendLine($"FROM {TableName}");

            // JOIN
            if (JoinClauses.Count > 0)
                foreach (var joinClause in JoinClauses)
                    query.AppendLine(joinClause);

            // WHERE
            if (!string.IsNullOrEmpty(WhereClause))
                query.AppendLine($"WHERE {WhereClause}");

            // ORDER BY
            if (!string.IsNullOrEmpty(OrderByClause))
                query.AppendLine($"ORDER BY {OrderByClause}");

            // LIMIT
            if (LimitValue.HasValue)
                query.AppendLine($"LIMIT {LimitValue}");

            return query.ToString().Trim() + ";";
        }
    }
}