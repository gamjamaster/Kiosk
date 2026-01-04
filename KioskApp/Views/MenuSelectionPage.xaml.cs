using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KioskApp.Models;
using KioskApp.ViewModels;
using MenuItem = KioskApp.Models.MenuItem;

namespace KioskApp.Views
{
    public partial class MenuSelectionPage : Page
    {
        private readonly MainViewModel _viewModel;
        private readonly Category _category;
        
        public MenuSelectionPage(MainViewModel viewModel, Category category)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _category = category;
            
            LoadMenuItems();
        }
        
        private void LoadMenuItems()
        {
            CategoryTitleText.Text = GetCategoryTitle(_category);
            var items = _viewModel.GetMenuItemsByCategory(_category);
            MenuItemsControl.ItemsSource = items;
        }
        
        private string GetCategoryTitle(Category category)
        {
            return category switch
            {
                Category.Burger => "Burgers",
                Category.Chicken => "Chicken",
                Category.Beverage => "Beverages",
                Category.Side => "Sides",
                Category.Dessert => "Desserts",
                _ => "Menu"
            };
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        
        private void MenuItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is MenuItem menuItem)
            {
                _viewModel.AddToCart(menuItem);
                MessageBox.Show($"{menuItem.Name} added to cart.", 
                              "Added", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }
    }
}
