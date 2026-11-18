using demo.Context;
using System.Windows;

namespace demo
{
    public partial class LoginWindow : Window
    {
        private readonly DemoDbContext _context;
        private readonly LoginService _loginServise;

        public LoginWindow()
        {
            InitializeComponent();

            _context = new();
            _loginServise = new(_context);

            DataContext = this;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTxt.Text))
            {
                MessageBox.Show("Заполните поле логина", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _loginServise.GetUser(LoginTxt.Text);

            if (user == null)
            {
                MessageBox.Show("Неправильный логин", "Ошибка", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            Application.Current.MainWindow = new MainWindow(user);
            Application.Current.MainWindow.Show();

            this.Close();
        }
    }
}
