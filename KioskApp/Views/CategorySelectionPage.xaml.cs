using System;
using System.Windows;
using System.Windows.Controls;
using KioskApp.Models;
using KioskApp.ViewModels;

namespace KioskApp.Views
{
    public partial class CategorySelectionPage : Page
    {
        private readonly MainViewModel _viewModel;
        
        public CategorySelectionPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }
        
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string categoryString)
            {
                if (Enum.TryParse<Category>(categoryString, out var category))
                {
                    NavigationService?.Navigate(new MenuSelectionPage(_viewModel, category));
                }
            }
        }
    }
}
