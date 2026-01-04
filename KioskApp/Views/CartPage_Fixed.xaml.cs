using System.Windows;
using System.Windows.Controls;
using KioskApp.Models;
using KioskApp.ViewModels;

namespace KioskApp.Views
{
    public partial class CartPage : Page
    {
        private readonly MainViewModel _viewModel;
        
        public CartPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            
            LoadCart();
        }
        
        private void LoadCart()
        {
            CartItemsControl.ItemsSource = _viewModel.CartItems;
            UpdateTotal();
            
            if (_viewModel.CartItems.Count == 0)
            {
                EmptyCartMessage.Visibility = Visibility.Visible;
                CartItemsControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyCartMessage.Visibility = Visibility.Collapsed;
                CartItemsControl.Visibility = Visibility.Visible;
            }
        }
        
        private void UpdateTotal()
        {
            TotalAmountText.Text = $"${_viewModel.TotalAmount:N0}";
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CartItem cartItem)
            {
                cartItem.Quantity++;
                UpdateTotal();
            }
        }
        
        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CartItem cartItem)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    UpdateTotal();
                }
                else
                {
                    var result = MessageBox.Show(
                        "Remove this item?", 
                        "Confirm", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        _viewModel.RemoveFromCart(cartItem);
                        LoadCart();
                    }
                }
            }
        }
        
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CartItem cartItem)
            {
                var result = MessageBox.Show(
                    $"Remove {cartItem.MenuItem.Name}?", 
                    "Confirm", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.RemoveFromCart(cartItem);
                    LoadCart();
                }
            }
        }
        
        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Clear the cart?", 
                "Confirm", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.Yes)
            {
                _viewModel.ClearCart();
                LoadCart();
            }
        }
        
        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            NavigationService?.Navigate(new PaymentPage(_viewModel));
        }
    }
}
