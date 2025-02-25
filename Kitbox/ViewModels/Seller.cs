using System;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kitbox.Models
{
    

    public partial class Seller : ObservableObject
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        

        
        private string priceProduct;

        public Seller() {
            Email = string.Empty;
            PriceProduct = string.Empty;
         }

        public Seller(string email, string priceProduct)
        {
            Email = email;
            PriceProduct = priceProduct;
            
        }
    }
}
