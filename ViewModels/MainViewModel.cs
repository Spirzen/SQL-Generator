using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SQL_Generator.Helpers;

namespace SQL_Generator.ViewModels
{
    /// <summary>
    /// ViewModel для главного окна приложения.
    /// Содержит логику для генерации SQL-запросов и управления интерфейсом.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Поля

        private string _tableName;
        private ObservableCollection<string> _columns = new();
        private ObservableCollection<string> _values = new();
        private string _whereCondition;
        private string _orderBy;
        private int? _limit;
        private string _generatedQuery;
        private string _columnsString;
        private string _valuesString;
        private bool _isValuesEnabled;
        private readonly List<string> _queryTypes = new() { "SELECT", "INSERT", "UPDATE", "DELETE" };
        private string _selectedQueryType;
        private string _selectedFilterColumn;
        private string _selectedOrderByColumn;
        private readonly List<string> _operators = new() { "=", "<>", ">", "<", ">=", "<=", "LIKE", "IS NULL", "IS NOT NULL" };
        private string _selectedOperator;
        private string _filterValue;
        private string _joinTable;
        private string _joinColumn;
        private string _joinMainTableColumn;
        private string _selectedJoinType;
        private ICommand _copyToClipboardCommand;
        private readonly List<string> _joinTypes = new() { "INNER", "LEFT", "RIGHT", "FULL" };

        #endregion

        #region Свойства

        /// <summary>
        /// Имя таблицы, к которой применяется запрос.
        /// </summary>
        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция столбцов для запроса.
        /// </summary>
        public ObservableCollection<string> Columns
        {
            get => _columns;
            private set
            {
                _columns = new ObservableCollection<string>(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция значений для запроса.
        /// </summary>
        public ObservableCollection<string> Values
        {
            get => _values;
            private set
            {
                _values = new ObservableCollection<string>(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Условие WHERE для фильтрации данных.
        /// </summary>
        public string WhereCondition
        {
            get => _whereCondition;
            set
            {
                _whereCondition = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Условие ORDER BY для сортировки данных.
        /// </summary>
        public string OrderBy
        {
            get => _orderBy;
            set
            {
                _orderBy = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Лимит количества возвращаемых строк.
        /// </summary>
        public int? Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Сгенерированный SQL-запрос.
        /// </summary>
        public string GeneratedQuery
        {
            get => _generatedQuery;
            set
            {
                _generatedQuery = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Строка с перечислением столбцов через запятую.
        /// </summary>
        public string ColumnsString
        {
            get => _columnsString;
            set
            {
                _columnsString = value;
                // Разбиваем строку на столбцы и обновляем коллекцию
                Columns = new ObservableCollection<string>(
                    _columnsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(col => col.Trim())
                );
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Строка с перечислением значений через запятую.
        /// </summary>
        public string ValuesString
        {
            get => _valuesString;
            set
            {
                _valuesString = value;
                // Разбиваем строку на значения и обновляем коллекцию
                Values = new ObservableCollection<string>(
                    _valuesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(val => val.Trim())
                );
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Флаг, указывающий, доступны ли поля для ввода значений.
        /// </summary>
        public bool IsValuesEnabled
        {
            get => _isValuesEnabled;
            set
            {
                _isValuesEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный тип запроса (например, SELECT, INSERT, UPDATE, DELETE).
        /// </summary>
        public string SelectedQueryType
        {
            get => _selectedQueryType;
            set
            {
                _selectedQueryType = value;
                IsValuesEnabled = value == "INSERT" || value == "UPDATE";
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный столбец для фильтрации данных (WHERE).
        /// </summary>
        public string SelectedFilterColumn
        {
            get => _selectedFilterColumn;
            set
            {
                _selectedFilterColumn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный столбец для сортировки данных (ORDER BY).
        /// </summary>
        public string SelectedOrderByColumn
        {
            get => _selectedOrderByColumn;
            set
            {
                _selectedOrderByColumn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный оператор для фильтрации данных (например, =, LIKE, IS NULL).
        /// </summary>
        public string SelectedOperator
        {
            get => _selectedOperator;
            set
            {
                _selectedOperator = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Значение для фильтрации данных (WHERE).
        /// </summary>
        public string FilterValue
        {
            get => _filterValue;
            set
            {
                _filterValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Имя таблицы для соединения (JOIN).
        /// </summary>
        public string JoinTable
        {
            get => _joinTable;
            set
            {
                _joinTable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Имя столбца для соединения (JOIN).
        /// </summary>
        public string JoinColumn
        {
            get => _joinColumn;
            set
            {
                _joinColumn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Имя столбца из основной таблицы для соединения (JOIN).
        /// </summary>
        public string JoinMainTableColumn
        {
            get => _joinMainTableColumn;
            set
            {
                _joinMainTableColumn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбранный тип соединения (например, INNER, LEFT, RIGHT).
        /// </summary>
        public string SelectedJoinType
        {
            get => _selectedJoinType;
            set
            {
                _selectedJoinType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Команда для копирования сгенерированного запроса в буфер обмена.
        /// </summary>
        public ICommand CopyToClipboardCommand => _copyToClipboardCommand ??= new Command(() =>
        {
            if (!string.IsNullOrEmpty(GeneratedQuery))
            {
                Clipboard.SetText(GeneratedQuery);
            }
        });

        /// <summary>
        /// Доступные типы соединений (JOIN).
        /// </summary>
        public List<string> JoinTypes => _joinTypes;

        /// <summary>
        /// Команда для генерации SQL-запроса.
        /// </summary>
        public Command GenerateQueryCommand { get; }

        /// <summary>
        /// Доступные типы запросов (например, SELECT, INSERT, UPDATE, DELETE).
        /// </summary>
        public List<string> QueryTypes => _queryTypes;

        /// <summary>
        /// Доступные операторы для фильтрации данных.
        /// </summary>
        public List<string> Operators => _operators;

        #endregion

        #region Конструктор

        /// <summary>
        /// Создает новый экземпляр класса <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel()
        {
            Columns = new ObservableCollection<string>();
            GenerateQueryCommand = new Command(GenerateQuery);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Генерирует SQL-запрос на основе выбранных параметров.
        /// </summary>
        private void GenerateQuery()
        {
            string query = string.Empty;

            switch (SelectedQueryType)
            {
                case "SELECT":
                    query = GenerateSelectQuery();
                    break;

                case "INSERT":
                    query = GenerateInsertQuery();
                    break;

                case "UPDATE":
                    query = GenerateUpdateQuery();
                    break;

                case "DELETE":
                    query = GenerateDeleteQuery();
                    break;

                default:
                    query = "Неизвестный тип запроса";
                    break;
            }

            GeneratedQuery = query;
        }

        /// <summary>
        /// Генерирует SQL-запрос типа SELECT.
        /// </summary>
        /// <returns>SQL-запрос SELECT.</returns>
        private string GenerateSelectQuery()
        {
            var selectQuery = QueryFactory.CreateSelect(TableName)
                .AddColumns(Columns)
                .AddOrderBy(SelectedOrderByColumn);

            // Добавляем JOIN
            if (!string.IsNullOrEmpty(JoinTable) &&
                !string.IsNullOrEmpty(JoinColumn) &&
                !string.IsNullOrEmpty(JoinMainTableColumn) &&
                !string.IsNullOrEmpty(SelectedJoinType))
            {
                selectQuery.AddJoin(SelectedJoinType, JoinTable, JoinColumn, JoinMainTableColumn);
            }

            // Добавляем WHERE
            if (!string.IsNullOrEmpty(SelectedFilterColumn) &&
                !string.IsNullOrEmpty(SelectedOperator) &&
                (!string.IsNullOrEmpty(FilterValue) || SelectedOperator.Contains("NULL")))
            {
                var whereCondition = SelectedOperator.Contains("NULL")
                    ? $"{SelectedFilterColumn} {SelectedOperator}"
                    : $"{SelectedFilterColumn} {SelectedOperator} {FormatValueForSql(FilterValue)}";
                selectQuery.AddWhere(whereCondition);
            }

            if (Limit.HasValue)
                selectQuery.AddLimit(Limit.Value);

            return selectQuery.Build();
        }

        /// <summary>
        /// Генерирует SQL-запрос типа INSERT.
        /// </summary>
        /// <returns>SQL-запрос INSERT.</returns>
        private string GenerateInsertQuery()
        {
            var insertQuery = QueryFactory.CreateInsert(TableName);

            // Добавляем значения
            for (int i = 0; i < Columns.Count; i++)
            {
                if (i < Values.Count)
                    insertQuery.AddValue(Columns[i], FormatValueForSql(Values[i]));
            }

            // Добавляем условие WHERE
            if (!string.IsNullOrEmpty(SelectedFilterColumn) &&
                !string.IsNullOrEmpty(SelectedOperator) &&
                (!string.IsNullOrEmpty(FilterValue) || SelectedOperator.Contains("NULL")))
            {
                var whereCondition = SelectedOperator.Contains("NULL")
                    ? $"{SelectedFilterColumn} {SelectedOperator}"
                    : $"{SelectedFilterColumn} {SelectedOperator} {FormatValueForSql(FilterValue)}";
                insertQuery.AddWhere(whereCondition);
            }

            return insertQuery.Build();
        }

        /// <summary>
        /// Генерирует SQL-запрос типа UPDATE.
        /// </summary>
        /// <returns>SQL-запрос UPDATE.</returns>
        private string GenerateUpdateQuery()
        {
            var updateQuery = QueryFactory.CreateUpdate(TableName);

            foreach (var column in Columns)
            {
                if (Values.Contains(column))
                    updateQuery.AddSet(column, FormatValueForSql(Values[Columns.IndexOf(column)]));
            }

            // Добавляем условие WHERE
            if (!string.IsNullOrEmpty(SelectedFilterColumn) &&
                !string.IsNullOrEmpty(SelectedOperator) &&
                (!string.IsNullOrEmpty(FilterValue) || SelectedOperator.Contains("NULL")))
            {
                var whereCondition = SelectedOperator.Contains("NULL")
                    ? $"{SelectedFilterColumn} {SelectedOperator}"
                    : $"{SelectedFilterColumn} {SelectedOperator} {FormatValueForSql(FilterValue)}";
                updateQuery.AddWhere(whereCondition);
            }

            return updateQuery.Build();
        }

        /// <summary>
        /// Генерирует SQL-запрос типа DELETE.
        /// </summary>
        /// <returns>SQL-запрос DELETE.</returns>
        private string GenerateDeleteQuery()
        {
            var deleteQuery = QueryFactory.CreateDelete(TableName);

            // Добавляем условие WHERE
            if (!string.IsNullOrEmpty(SelectedFilterColumn) &&
                !string.IsNullOrEmpty(SelectedOperator) &&
                (!string.IsNullOrEmpty(FilterValue) || SelectedOperator.Contains("NULL")))
            {
                var whereCondition = SelectedOperator.Contains("NULL")
                    ? $"{SelectedFilterColumn} {SelectedOperator}"
                    : $"{SelectedFilterColumn} {SelectedOperator} {FormatValueForSql(FilterValue)}";
                deleteQuery.AddWhere(whereCondition);
            }

            return deleteQuery.Build();
        }

        /// <summary>
        /// Форматирует значение для использования в SQL-запросе.
        /// </summary>
        /// <param name="value">Значение для форматирования.</param>
        /// <returns>Форматированное значение (числа без кавычек, строки в кавычках).</returns>
        private string FormatValueForSql(string value)
        {
            // Если значение - число, возвращаем его без кавычек
            if (double.TryParse(value, out _))
                return value;

            // Если значение - строка, добавляем кавычки
            return $"'{value}'";
        }

        #endregion

        #region Реализация INotifyPropertyChanged

        /// <summary>
        /// Событие, возникающее при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged для указанного свойства.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}