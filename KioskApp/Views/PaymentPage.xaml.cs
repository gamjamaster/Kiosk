using System;
using System.Windows;
using System.Windows.Controls;
using KioskApp.Models;
using KioskApp.ViewModels;

namespace KioskApp.Views
{
    public partial class PaymentPage : Page
    {
        private readonly MainViewModel _viewModel;
        
        public PaymentPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            
            LoadOrderSummary();
        }
        
        private void LoadOrderSummary()
        {
            OrderSummaryControl.ItemsSource = _viewModel.CartItems;
            TotalAmountText.Text = $"${_viewModel.TotalAmount:N0}";
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        
        private void PaymentMethod_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string paymentMethodString)
            {
                if (Enum.TryParse<PaymentMethod>(paymentMethodString, out var paymentMethod))
                {
                    // Simulate payment processing
                    var result = MessageBox.Show(
                        $"Proceed with payment?\n\nAmount: ${_viewModel.TotalAmount:N0}", 
                        "Confirm Payment", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        var order = _viewModel.CreateOrder(paymentMethod);
                        NavigationService?.Navigate(new OrderCompletePage(_viewModel, order));
                    }
                }
            }
        }
    }
}
