using System.ComponentModel;

namespace KioskApp.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;
        
        public MenuItem MenuItem { get; set; } = new MenuItem();
        
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        
        public decimal TotalPrice => MenuItem.Price * Quantity;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
