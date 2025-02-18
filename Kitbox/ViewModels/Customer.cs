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


        public Customer() // Constructeur par défaut 
        {
            Height = 0;
            Width = 0;
            Depth = 0;
        }
        public Customer(int height, int width, int depth) 
        {
            Height = height;
            Width = width;
            Depth = depth;
        }

        
    }
}
