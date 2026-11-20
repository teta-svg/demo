using demo.Context;
using System.Windows;
using System.Windows.Controls;

namespace demo
{
    public partial class ProductWindow : Window
    {
        private readonly DemoDbContext _context;
        private readonly ProductService _productService;


        public ProductWindow(int productId, int userId)
        {
            InitializeComponent();

            _productId = productId;
            _userId = userId;

            _context = new();
            _productService = new(_context);

            Product = _productService.GetProductById(_productId);

            if (Product == null)
            {
                MessageBox.Show("Не выбран товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            foreach (var size in Product!.Pairs)
            {
                SizeRange += $"{size.Size} ({size.QuantityAvailable} шт.) ";
                if(size.QuantityAvailable > 0)
                {
                    AvailablePairs.Add(size);
                }
            }

            DataContext = this;
        }

        private decimal? _totalPrice;

        public PairCount SelectedPair {  get; set; }

        public OrderCreateCard? Product {  get; set; }

        public string SizeRange { get; set; } = string.Empty;

        private readonly int _productId;
        private readonly int _userId;

        public List<PairCount> AvailablePairs { get; set; } = [];

        private void CountTotalPrice()
        {
            if (!int.TryParse(CountBox.Text, out int count) || SelectedPair == null)
            {
                TotalPriceRun.Text = "0.00 руб.";
                _totalPrice = 0;
                return;
            }

            decimal total = (Product?.Price ?? 0) * count;

            TotalPriceRun.Text = $"{total:F2} руб.";

            _totalPrice = total;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MakeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(CountBox.Text, out int count))
            {
                MessageBox.Show("Неверно введено число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedPair == null)
            {
                MessageBox.Show("Выберите размер", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (count <=0 || SelectedPair.QuantityAvailable < count)
            {
                MessageBox.Show("Недостаточно товара на складе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _productService.CreateOrder(_userId, _productId, count, SelectedPair.IdSize);

            MessageBox.Show("Заказ успешно создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void CountBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CountTotalPrice();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountTotalPrice();
        }
    }
}
