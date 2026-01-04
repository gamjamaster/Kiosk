using System;
using System.Windows;
using System.Windows.Threading;
using KioskApp.Views;
using KioskApp.ViewModels;

namespace KioskApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private readonly DispatcherTimer _sessionTimer;
        private int _secondsRemaining;
        private bool _isOrderActive;
        private int _lastCartItemCount;

        private const int SessionSeconds = 150;
        
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            
            // Subscribe to cart updates
            _viewModel.CartUpdated += UpdateCartDisplay;
            _viewModel.CartUpdated += HandleCartUpdated;

            // 150초 시간 제한(첫 상품 담을 때 시작, 결제 완료/카트 비면 종료)
            _secondsRemaining = SessionSeconds;
            _isOrderActive = false;
            _lastCartItemCount = _viewModel.CartItems.Count;
            UpdateRemainingTimeText();

            _sessionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _sessionTimer.Tick += SessionTimer_Tick;
            _sessionTimer.Start();
            
            // Navigate to category selection
            MainFrame.Navigate(new CategorySelectionPage(_viewModel));
        }

        private void HandleCartUpdated()
        {
            var currentCount = _viewModel.CartItems.Count;

            // 0 -> 1 : 주문 시작
            if (!_isOrderActive && _lastCartItemCount == 0 && currentCount > 0)
            {
                StartOrderTimer();
            }

            // >0 -> 0 : 주문 종료(결제 완료/전체삭제/마지막 아이템 제거)
            if (_isOrderActive && _lastCartItemCount > 0 && currentCount == 0)
            {
                StopOrderTimer();
            }

            _lastCartItemCount = currentCount;
        }

        private void StartOrderTimer()
        {
            _isOrderActive = true;
            _secondsRemaining = SessionSeconds;
            UpdateRemainingTimeText();
        }

        private void StopOrderTimer()
        {
            _isOrderActive = false;
            _secondsRemaining = SessionSeconds;
            UpdateRemainingTimeText();
        }

        private void SessionTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isOrderActive)
            {
                return;
            }

            if (_secondsRemaining > 0)
            {
                _secondsRemaining--;
                UpdateRemainingTimeText();
                return;
            }

            // 시간 초과: 홈으로 복귀 + 카트 비우기
            _viewModel.ClearCart();
            MainFrame.Navigate(new CategorySelectionPage(_viewModel));
            StopOrderTimer();
        }

        private void UpdateRemainingTimeText()
        {
            if (RemainingTimeText != null)
            {
                RemainingTimeText.Text = $"{_secondsRemaining}초";
            }
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
