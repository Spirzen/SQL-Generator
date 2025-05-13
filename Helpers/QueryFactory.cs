using SQL_Generator.Models;

namespace SQL_Generator.Helpers
{
    /// <summary>
    /// Фабрика для создания экземпляров построителей SQL-запросов.
    /// Предоставляет методы для создания запросов SELECT, INSERT, UPDATE и DELETE.
    /// </summary>
    public static class QueryFactory
    {
        /// <summary>
        /// Создает новый экземпляр построителя запроса SELECT.
        /// </summary>
        /// <param name="tableName">Имя таблицы, к которой будет выполняться запрос.</param>
        /// <returns>Экземпляр <see cref="SelectQueryBuilder"/>.</returns>
        public static SelectQueryBuilder CreateSelect(string tableName)
        {
            return new SelectQueryBuilder(tableName);
        }

        /// <summary>
        /// Создает новый экземпляр построителя запроса INSERT.
        /// </summary>
        /// <param name="tableName">Имя таблицы, в которую будут вставляться данные.</param>
        /// <returns>Экземпляр <see cref="InsertQueryBuilder"/>.</returns>
        public static InsertQueryBuilder CreateInsert(string tableName)
        {
            return new InsertQueryBuilder(tableName);
        }

        /// <summary>
        /// Создает новый экземпляр построителя запроса UPDATE.
        /// </summary>
        /// <param name="tableName">Имя таблицы, в которой будут обновляться данные.</param>
        /// <returns>Экземпляр <see cref="UpdateQueryBuilder"/>.</returns>
        public static UpdateQueryBuilder CreateUpdate(string tableName)
        {
            return new UpdateQueryBuilder(tableName);
        }

        /// <summary>
        /// Создает новый экземпляр построителя запроса DELETE.
        /// </summary>
        /// <param name="tableName">Имя таблицы, из которой будут удаляться данные.</param>
        /// <returns>Экземпляр <see cref="DeleteQueryBuilder"/>.</returns>
        public static DeleteQueryBuilder CreateDelete(string tableName)
        {
            return new DeleteQueryBuilder(tableName);
        }
    }
}