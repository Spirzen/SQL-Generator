using System.Windows;

namespace SQL_Generator.Views
{
    /// <summary>
    /// Логика взаимодействия для главного окна приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="MainWindow"/>.
        /// Инициализирует компоненты интерфейса и устанавливает контекст данных.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainViewModel();
        }
    }
}