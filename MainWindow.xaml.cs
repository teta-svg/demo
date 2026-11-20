using demo.Context;
using System.Collections.ObjectModel;
using System.Windows;

namespace demo
{
    public partial class MainWindow : Window
    {
        private readonly DemoDbContext _context;
        private ProductService _productService;

        public MainWindow(UserContext userContext)
        {
            InitializeComponent();

            _context = new();
            _productService = new(_context);

            CategoryList = _productService.GetCategories();

            SelectedCategory = CategoryList[0];

            Products = _productService.GetProductList(SelectedCategory);

            User = userContext;

            DataContext = this;
        }

        public ObservableCollection<ProductCard> Products { get; set; }

        public UserContext User {  get; set; }

        public string? SelectedCategory { get; set; }

        public string? SelectSort { get; set; }

        public string? SearchText { get; set; }

        public ObservableCollection<string> CategoryList { get; set; }

        public ProductCard SelectedProduct { get; set; }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new LoginWindow();
            Application.Current.MainWindow.Show();

            this.Close();
        }

        private void UpdateProductsList()
        {
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                MessageBox.Show("Не выбрана категория", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Products == null)
            {
                MessageBox.Show("Список пуст", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ObservableCollection<ProductCard> filteredProducts;

            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredProducts = _productService.GetProductList(SelectedCategory!, SearchText);
            }
            else
            {
                filteredProducts = _productService.GetProductList(SelectedCategory!);
            }

            if (!string.IsNullOrEmpty(SelectSort))
            {
                if (SelectSort.Contains("по возрастанию"))
                {
                    filteredProducts = new ObservableCollection<ProductCard>(filteredProducts.OrderBy(p => p.Price));
                }
                else if (SelectSort.Contains("по убыванию"))
                {
                    filteredProducts = new ObservableCollection<ProductCard>(filteredProducts.OrderByDescending(p => p.Price));
                }
            }

            Products.Clear();

            if (filteredProducts.Count == 0)
            {
                MessageBox.Show(" Не найдено", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }

        private void SelectedSort_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateProductsList();
        }

        private void SelectedCategoryBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateProductsList();
        }

        private void SearchText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateProductsList();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(SelectedProduct == null)
            {
                MessageBox.Show("Не выбран товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Application.Current.MainWindow = new ProductWindow(SelectedProduct.Id, User.Id);
            Application.Current.MainWindow.ShowDialog();

            _context.ChangeTracker.Clear();

            UpdateProductsList();
        }
    }
}