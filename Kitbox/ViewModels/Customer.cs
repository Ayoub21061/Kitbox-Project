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
        private int lockers;


        public Customer() // Constructeur par défaut 
        {
            Height = 0;
            Width = 0;
            Depth = 0;
            Lockers = 0;
        }
        public Customer(int height, int width, int depth, int lockers) 
        {
            Height = height;
            Width = width;
            Depth = depth;
            Lockers = lockers;
        }

        
    }
}
