using demo.Context;
using System.Collections.ObjectModel;
using System.Windows;

namespace demo
{
    public partial class MainWindow : Window
    {
        private readonly DemoDbContext _context;
        private ProductService _productService;

        public MainWindow()
        {
            InitializeComponent();

            _context = new();
            _productService = new(_context);

            Products = _productService.GetProductList();

            DataContext = this;
        }

        public ObservableCollection<ProductCard> Products { get; set; }
    }
}