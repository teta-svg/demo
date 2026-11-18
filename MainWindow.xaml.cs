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

        public string? SearchText { get; set; }

        public ObservableCollection<string> CategoryList { get; set; }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new LoginWindow();
            Application.Current.MainWindow.Show();

            this.Close();
        }

        private void Swap(int indexA, int indexB)
        {
            var tmp = Products[indexA];
            Products[indexA] = Products[indexB];
            Products[indexB] = tmp;
        }

        private void SortList(bool isAscOrder)
        {
            for(int i=0; i<Products.Count-1; i++)
            {
                for(int j=0; j<Products.Count-1-i; j++)
                {
                    if (isAscOrder)
                    {
                        if (Products[j].Price > Products[j + 1].Price)
                        {
                            Swap(j, j+1);
                        }
                    }
                    else
                    {
                        if (Products[j].Price < Products[j + 1].Price)
                        {
                            Swap(j, j + 1);
                        }
                    }
                }
            }
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

            if (SelectedSort != null)
            {
                if (SelectedSort.SelectedIndex == 0)
                {
                    SortList(isAscOrder: false);
                }
                else if (SelectedSort.SelectedIndex == 1)
                {
                    SortList(isAscOrder: true);
                }
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
    }
}