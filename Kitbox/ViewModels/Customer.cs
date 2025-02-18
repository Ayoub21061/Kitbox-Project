using CommunityToolkit.Mvvm.ComponentModel;

namespace Kitbox.Models
{
    public partial class Customer : ObservableObject
    {
        [ObservableProperty]
        private int height;

        [ObservableProperty]
        private int width;

        [ObservableProperty]
        private int depth;

        [ObservableProperty]
        private int doorCount;

        [ObservableProperty]
        private int shelfCount;

        [ObservableProperty]
        private string color;

        public Customer() // Constructeur par défaut 
        {
            Height = 0;
            Width = 0;
            Depth = 0;
            DoorCount = 0;
            ShelfCount = 0;
            Color = "Blanc";
        }
        public Customer(int height, int width, int depth, int doorCount, int shelfCount, string color) 
        {
            Height = height;
            Width = width;
            Depth = depth;
            DoorCount = doorCount;
            ShelfCount = shelfCount;
            Color = color;
        }

        
    }
}
