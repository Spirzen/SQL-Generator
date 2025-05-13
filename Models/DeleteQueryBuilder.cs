using System.Text;

namespace SQL_Generator.Models
{
    /// <summary>
    /// Построитель SQL-запросов типа DELETE.
    /// Наследуется от базового класса <see cref="QueryBuilder"/>.
    /// </summary>
    public class DeleteQueryBuilder : QueryBuilder
    {
        /// <summary>
        /// Условие WHERE для запроса DELETE.
        /// </summary>
        private string WhereClause { get; set; }

        /// <summary>
        /// Создает новый экземпляр построителя запроса DELETE.
        /// </summary>
        /// <param name="tableName">Имя таблицы, из которой будут удаляться данные.</param>
        public DeleteQueryBuilder(string tableName) : base(tableName) { }

        /// <summary>
        /// Добавляет условие WHERE к запросу DELETE.
        /// </summary>
        /// <param name="condition">Условие для фильтрации данных (например, "Age > 18").</param>
        /// <returns>Текущий экземпляр <see cref="DeleteQueryBuilder"/> для цепочки вызовов.</returns>
        public DeleteQueryBuilder AddWhere(string condition)
        {
            WhereClause = condition;
            return this;
        }

        /// <summary>
        /// Строит и возвращает SQL-запрос DELETE в виде строки.
        /// </summary>
        /// <returns>SQL-запрос DELETE с указанными параметрами.</returns>
        public override string Build()
        {
            var query = new StringBuilder();

            // DELETE
            query.AppendLine($"DELETE FROM {TableName}");

            // WHERE
            if (!string.IsNullOrEmpty(WhereClause))
                query.AppendLine($"WHERE {WhereClause}");

            return query.ToString().Trim() + ";";
        }
    }
}