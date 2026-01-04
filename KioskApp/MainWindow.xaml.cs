using System.Windows;
using KioskApp.Views;
using KioskApp.ViewModels;

namespace KioskApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            
            // Subscribe to cart updates
            _viewModel.CartUpdated += UpdateCartDisplay;
            
            // Navigate to category selection
            MainFrame.Navigate(new CategorySelectionPage(_viewModel));
        }
        
        private void UpdateCartDisplay()
        {
            CartItemCountText.Text = $"{_viewModel.TotalItemCount} items";
            CartTotalText.Text = $"${_viewModel.TotalAmount:N0}";
        }
        
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategorySelectionPage(_viewModel));
        }
        
        private void ViewCartButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CartPage(_viewModel));
        }
        
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MainFrame.Navigate(new PaymentPage(_viewModel));
        }
    }
}
