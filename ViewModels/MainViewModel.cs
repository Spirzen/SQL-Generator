using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SQL_Generator.Helpers;

namespace SQL_Generator.ViewModels
{
  public class MainViewModel : INotifyPropertyChanged
  {
    private string _tableName = string.Empty;
    private ObservableCollection<string> _columns = new();
    private ObservableCollection<string> _values = new();
    private string _generatedQuery = string.Empty;
    private string _columnsString = string.Empty;
    private string _valuesString = string.Empty;
    private bool _isValuesEnabled;
    private string _selectedQueryType = "SELECT";
    private string _selectedFilterColumn = string.Empty;
    private string _selectedOrderByColumn = string.Empty;
    private string _selectedOperator = "=";
    private string _filterValue = string.Empty;
    private string _joinTable = string.Empty;
    private string _joinColumn = string.Empty;
    private string _joinMainTableColumn = string.Empty;
    private string _selectedJoinType = "INNER";
    private string _selectedOrderDirection = "ASC";
    private string _limitText = string.Empty;
    private bool _isDistinct;
    private bool _isFilterValueEnabled = true;
    private bool _isSelectOptionsVisible = true;
    private string _statusMessage = "Укажите параметры и нажмите «Сгенерировать»";
    private ICommand? _copyToClipboardCommand;
    private ICommand? _clearFormCommand;

    private readonly List<string> _queryTypes = ["SELECT", "INSERT", "UPDATE", "DELETE"];
    private readonly List<string> _operators = ["=", "<>", ">", "<", ">=", "<=", "LIKE", "IS NULL", "IS NOT NULL"];
    private readonly List<string> _joinTypes = ["INNER", "LEFT", "RIGHT", "FULL"];
    private readonly List<string> _orderDirections = ["ASC", "DESC"];

    public string TableName
    {
      get => _tableName;
      set { _tableName = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> Columns
    {
      get => _columns;
      private set { _columns = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> Values
    {
      get => _values;
      private set { _values = value; OnPropertyChanged(); }
    }

    public string GeneratedQuery
    {
      get => _generatedQuery;
      set { _generatedQuery = value; OnPropertyChanged(); }
    }

    public string ColumnsString
    {
      get => _columnsString;
      set
      {
        _columnsString = value;
        Columns = new ObservableCollection<string>(
          _columnsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(col => col.Trim()));
        OnPropertyChanged();
      }
    }

    public string ValuesString
    {
      get => _valuesString;
      set
      {
        _valuesString = value;
        Values = new ObservableCollection<string>(
          _valuesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(val => val.Trim()));
        OnPropertyChanged();
      }
    }

    public bool IsValuesEnabled
    {
      get => _isValuesEnabled;
      set { _isValuesEnabled = value; OnPropertyChanged(); }
    }

    public string SelectedQueryType
    {
      get => _selectedQueryType;
      set
      {
        _selectedQueryType = value;
        IsValuesEnabled = value is "INSERT" or "UPDATE";
        IsSelectOptionsVisible = value == "SELECT";
        OnPropertyChanged();
        OnPropertyChanged(nameof(IsSelectOptionsVisible));
      }
    }

    public bool IsSelectOptionsVisible
    {
      get => _isSelectOptionsVisible;
      set { _isSelectOptionsVisible = value; OnPropertyChanged(); }
    }

    public string SelectedFilterColumn
    {
      get => _selectedFilterColumn;
      set { _selectedFilterColumn = value; OnPropertyChanged(); }
    }

    public string SelectedOrderByColumn
    {
      get => _selectedOrderByColumn;
      set { _selectedOrderByColumn = value; OnPropertyChanged(); }
    }

    public string SelectedOperator
    {
      get => _selectedOperator;
      set
      {
        _selectedOperator = value;
        IsFilterValueEnabled = !value.Contains("NULL", StringComparison.Ordinal);
        OnPropertyChanged();
        OnPropertyChanged(nameof(IsFilterValueEnabled));
      }
    }

    public bool IsFilterValueEnabled
    {
      get => _isFilterValueEnabled;
      set { _isFilterValueEnabled = value; OnPropertyChanged(); }
    }

    public string FilterValue
    {
      get => _filterValue;
      set { _filterValue = value; OnPropertyChanged(); }
    }

    public string JoinTable
    {
      get => _joinTable;
      set { _joinTable = value; OnPropertyChanged(); }
    }

    public string JoinColumn
    {
      get => _joinColumn;
      set { _joinColumn = value; OnPropertyChanged(); }
    }

    public string JoinMainTableColumn
    {
      get => _joinMainTableColumn;
      set { _joinMainTableColumn = value; OnPropertyChanged(); }
    }

    public string SelectedJoinType
    {
      get => _selectedJoinType;
      set { _selectedJoinType = value; OnPropertyChanged(); }
    }

    public string SelectedOrderDirection
    {
      get => _selectedOrderDirection;
      set { _selectedOrderDirection = value; OnPropertyChanged(); }
    }

    public string LimitText
    {
      get => _limitText;
      set { _limitText = value; OnPropertyChanged(); }
    }

    public bool IsDistinct
    {
      get => _isDistinct;
      set { _isDistinct = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
      get => _statusMessage;
      set { _statusMessage = value; OnPropertyChanged(); }
    }

    public List<string> JoinTypes => _joinTypes;
    public List<string> OrderDirections => _orderDirections;
    public Command GenerateQueryCommand { get; }
    public List<string> QueryTypes => _queryTypes;
    public List<string> Operators => _operators;

    public ICommand CopyToClipboardCommand => _copyToClipboardCommand ??= new Command(CopyToClipboard, () => !string.IsNullOrEmpty(GeneratedQuery));

    public ICommand ClearFormCommand => _clearFormCommand ??= new Command(ClearForm);

    public MainViewModel()
    {
      GenerateQueryCommand = new Command(GenerateQuery);
    }

    private void GenerateQuery()
    {
      if (string.IsNullOrWhiteSpace(TableName))
      {
        StatusMessage = "Укажите имя таблицы.";
        GeneratedQuery = string.Empty;
        return;
      }

      GeneratedQuery = SelectedQueryType switch
      {
        "SELECT" => GenerateSelectQuery(),
        "INSERT" => GenerateInsertQuery(),
        "UPDATE" => GenerateUpdateQuery(),
        "DELETE" => GenerateDeleteQuery(),
        _ => "-- Неизвестный тип запроса"
      };

      StatusMessage = string.IsNullOrEmpty(GeneratedQuery)
        ? "Не удалось сформировать запрос."
        : "Запрос готов. Можно скопировать в буфер.";
    }

    private string GenerateSelectQuery()
    {
      var selectQuery = QueryFactory.CreateSelect(TableName.Trim())
        .AddColumns(Columns);

      if (IsDistinct)
        selectQuery.AddDistinct();

      if (!string.IsNullOrEmpty(JoinTable) &&
          !string.IsNullOrEmpty(JoinColumn) &&
          !string.IsNullOrEmpty(JoinMainTableColumn) &&
          !string.IsNullOrEmpty(SelectedJoinType))
      {
        selectQuery.AddJoin(SelectedJoinType, JoinTable.Trim(), JoinColumn.Trim(), JoinMainTableColumn.Trim());
      }

      var where = BuildWhereCondition();
      if (!string.IsNullOrEmpty(where))
        selectQuery.AddWhere(where);

      if (!string.IsNullOrWhiteSpace(SelectedOrderByColumn))
        selectQuery.AddOrderBy($"{SelectedOrderByColumn.Trim()} {SelectedOrderDirection}");

      if (TryParseLimit(out var limit))
        selectQuery.AddLimit(limit);

      return selectQuery.Build();
    }

    private string GenerateInsertQuery()
    {
      if (Columns.Count == 0)
      {
        StatusMessage = "Для INSERT укажите столбцы.";
        return string.Empty;
      }

      var insertQuery = QueryFactory.CreateInsert(TableName.Trim());

      for (var i = 0; i < Columns.Count; i++)
      {
        var value = i < Values.Count ? Values[i] : string.Empty;
        insertQuery.AddValue(Columns[i], SqlValueFormatter.Format(value));
      }

      return insertQuery.Build();
    }

    private string GenerateUpdateQuery()
    {
      if (Columns.Count == 0)
      {
        StatusMessage = "Для UPDATE укажите столбцы и значения.";
        return string.Empty;
      }

      var updateQuery = QueryFactory.CreateUpdate(TableName.Trim());

      for (var i = 0; i < Columns.Count; i++)
      {
        var value = i < Values.Count ? Values[i] : string.Empty;
        updateQuery.AddSet(Columns[i], SqlValueFormatter.Format(value));
      }

      var where = BuildWhereCondition();
      if (!string.IsNullOrEmpty(where))
        updateQuery.AddWhere(where);

      return updateQuery.Build();
    }

    private string GenerateDeleteQuery()
    {
      var deleteQuery = QueryFactory.CreateDelete(TableName.Trim());

      var where = BuildWhereCondition();
      if (!string.IsNullOrEmpty(where))
        deleteQuery.AddWhere(where);

      return deleteQuery.Build();
    }

    private string? BuildWhereCondition()
    {
      if (string.IsNullOrWhiteSpace(SelectedFilterColumn) || string.IsNullOrWhiteSpace(SelectedOperator))
        return null;

      if (SelectedOperator.Contains("NULL", StringComparison.Ordinal))
        return $"{SelectedFilterColumn.Trim()} {SelectedOperator}";

      if (string.IsNullOrWhiteSpace(FilterValue))
        return null;

      return $"{SelectedFilterColumn.Trim()} {SelectedOperator} {SqlValueFormatter.Format(FilterValue)}";
    }

    private bool TryParseLimit(out int limit)
    {
      limit = 0;
      return !string.IsNullOrWhiteSpace(LimitText) &&
             int.TryParse(LimitText.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out limit) &&
             limit > 0;
    }

    private void CopyToClipboard()
    {
      if (string.IsNullOrEmpty(GeneratedQuery))
        return;

      Clipboard.SetText(GeneratedQuery);
      StatusMessage = "Скопировано в буфер обмена.";
    }

    private void ClearForm()
    {
      TableName = string.Empty;
      ColumnsString = string.Empty;
      ValuesString = string.Empty;
      SelectedFilterColumn = string.Empty;
      SelectedOrderByColumn = string.Empty;
      SelectedOperator = "=";
      FilterValue = string.Empty;
      JoinTable = string.Empty;
      JoinColumn = string.Empty;
      JoinMainTableColumn = string.Empty;
      SelectedJoinType = "INNER";
      SelectedOrderDirection = "ASC";
      LimitText = string.Empty;
      IsDistinct = false;
      GeneratedQuery = string.Empty;
      SelectedQueryType = "SELECT";
      StatusMessage = "Форма очищена.";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
