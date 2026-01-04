using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using KioskApp.Models;

namespace KioskApp.ViewModels
{
    public class MainViewModel
    {
        private static int _orderCounter = 1;
        private readonly List<MenuItem> _allMenuItems;
        
        public ObservableCollection<CartItem> CartItems { get; set; }
        
        public event Action? CartUpdated;
        
        public MainViewModel()
        {
            CartItems = new ObservableCollection<CartItem>();
            _allMenuItems = InitializeMenuItems();

            CartItems.CollectionChanged += CartItems_CollectionChanged;
        }

        private void CartItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    if (oldItem is CartItem oldCartItem)
                    {
                        oldCartItem.PropertyChanged -= CartItem_PropertyChanged;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    if (newItem is CartItem newCartItem)
                    {
                        newCartItem.PropertyChanged += CartItem_PropertyChanged;
                    }
                }
            }

            CartUpdated?.Invoke();
        }

        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity) || e.PropertyName == nameof(CartItem.TotalPrice))
            {
                CartUpdated?.Invoke();
            }
        }
        
        private List<MenuItem> InitializeMenuItems()
        {
            return new List<MenuItem>
            {
                // Burgers
                new MenuItem { Id = 1, Name = "Cheeseburger", Description = "Classic beef patty with cheese", Price = 55, Category = Category.Burger },
                new MenuItem { Id = 2, Name = "Double Burger", Description = "Two beef patties with cheese", Price = 75, Category = Category.Burger },
                new MenuItem { Id = 3, Name = "Bacon Burger", Description = "Burger with crispy bacon", Price = 65, Category = Category.Burger },
                new MenuItem { Id = 4, Name = "Bulgogi Burger", Description = "Korean style BBQ burger", Price = 60, Category = Category.Burger },
                
                // Chicken
                new MenuItem { Id = 5, Name = "Fried Chicken", Description = "Crispy fried chicken", Price = 80, Category = Category.Chicken },
                new MenuItem { Id = 6, Name = "Seasoned Chicken", Description = "Spicy glazed chicken", Price = 85, Category = Category.Chicken },
                new MenuItem { Id = 7, Name = "Chicken Nuggets", Description = "6 piece nuggets", Price = 45, Category = Category.Chicken },
                new MenuItem { Id = 8, Name = "Chicken Tenders", Description = "4 piece tenders", Price = 55, Category = Category.Chicken },
                
                // Beverages
                new MenuItem { Id = 9, Name = "Coca Cola", Description = "Classic Coke", Price = 20, Category = Category.Beverage },
                new MenuItem { Id = 10, Name = "Sprite", Description = "Lemon-lime soda", Price = 20, Category = Category.Beverage },
                new MenuItem { Id = 11, Name = "Orange Juice", Description = "Fresh orange juice", Price = 30, Category = Category.Beverage },
                new MenuItem { Id = 12, Name = "Americano", Description = "Hot or iced coffee", Price = 35, Category = Category.Beverage },
                new MenuItem { Id = 13, Name = "Milkshake", Description = "Vanilla milkshake", Price = 40, Category = Category.Beverage },
                
                // Sides
                new MenuItem { Id = 14, Name = "French Fries", Description = "Crispy golden fries", Price = 25, Category = Category.Side },
                new MenuItem { Id = 15, Name = "Mozzarella Sticks", Description = "6 cheese sticks", Price = 40, Category = Category.Side },
                new MenuItem { Id = 16, Name = "Onion Rings", Description = "Crispy onion rings", Price = 35, Category = Category.Side },
                new MenuItem { Id = 17, Name = "Salad", Description = "Fresh garden salad", Price = 45, Category = Category.Side },
                
                // Desserts
                new MenuItem { Id = 18, Name = "Ice Cream", Description = "Vanilla ice cream", Price = 25, Category = Category.Dessert },
                new MenuItem { Id = 19, Name = "Apple Pie", Description = "Warm apple pie", Price = 30, Category = Category.Dessert },
                new MenuItem { Id = 20, Name = "Chocolate Cake", Description = "Rich chocolate cake", Price = 45, Category = Category.Dessert },
                new MenuItem { Id = 21, Name = "Cookie", Description = "Chocolate chip cookie", Price = 20, Category = Category.Dessert }
            };
        }
        
        public List<MenuItem> GetMenuItemsByCategory(Category category)
        {
            return _allMenuItems.Where(m => m.Category == category).ToList();
        }
        
        public void AddToCart(MenuItem menuItem)
        {
            var existingItem = CartItems.FirstOrDefault(ci => ci.MenuItem.Id == menuItem.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var cartItem = new CartItem
                {
                    MenuItem = menuItem,
                    Quantity = 1
                };
                CartItems.Add(cartItem);
            }
        }
        
        public void RemoveFromCart(CartItem cartItem)
        {
            CartItems.Remove(cartItem);
        }
        
        public void ClearCart()
        {
            foreach (var cartItem in CartItems)
            {
                cartItem.PropertyChanged -= CartItem_PropertyChanged;
            }
            CartItems.Clear();
        }
        
        public decimal TotalAmount => CartItems.Sum(ci => ci.TotalPrice);
        
        public int TotalItemCount => CartItems.Sum(ci => ci.Quantity);
        
        public Order CreateOrder(PaymentMethod paymentMethod)
        {
            var order = new Order
            {
                OrderNumber = _orderCounter++,
                Items = new List<CartItem>(CartItems.Select(ci => new CartItem 
                { 
                    MenuItem = ci.MenuItem, 
                    Quantity = ci.Quantity 
                })),
                TotalAmount = TotalAmount,
                OrderTime = DateTime.Now,
                PaymentMethod = paymentMethod
            };
            
            ClearCart();
            return order;
        }
    }
}
