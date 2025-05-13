namespace SQL_Generator.Models
{
    /// <summary>
    /// Базовый абстрактный класс для построителей SQL-запросов.
    /// Предоставляет общую функциональность для всех типов запросов.
    /// </summary>
    public abstract class QueryBuilder
    {
        /// <summary>
        /// Имя таблицы, к которой применяется запрос.
        /// </summary>
        protected string TableName { get; set; }

        /// <summary>
        /// Создает новый экземпляр построителя SQL-запроса.
        /// </summary>
        /// <param name="tableName">Имя таблицы, к которой будет выполняться запрос.</param>
        public QueryBuilder(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// Абстрактный метод для построения SQL-запроса.
        /// Должен быть реализован в производных классах.
        /// </summary>
        /// <returns>SQL-запрос в виде строки.</returns>
        public abstract string Build();
    }
}