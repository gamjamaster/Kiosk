using System.Windows;
using System.Windows.Controls;
using KioskApp.Models;
using KioskApp.ViewModels;

namespace KioskApp.Views
{
    public partial class OrderCompletePage : Page
    {
        private readonly MainViewModel _viewModel;
        private readonly Order _order;
        
        public OrderCompletePage(MainViewModel viewModel, Order order)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _order = order;
            
            DisplayOrderInfo();
        }
        
        private void DisplayOrderInfo()
        {
            OrderNumberText.Text = _order.OrderNumber.ToString("D3");
            OrderTimeText.Text = _order.OrderTime.ToString("yyyy-MM-dd HH:mm:ss");
            PaymentMethodText.Text = _order.PaymentMethod == PaymentMethod.Card 
                ? "Card" 
                : "Cash";
            TotalAmountText.Text = $"${_order.TotalAmount:N0}";
        }
        
        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CategorySelectionPage(_viewModel));
        }
    }
}
