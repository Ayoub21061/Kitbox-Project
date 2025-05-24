<<<<<<< HEAD
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
=======
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.ViewModels;
using System;

namespace Kitbox.ViewModels;


public class Seller : User
{


    public Seller()
    {
        username = "StockKeeper";
        password = "Merge42";
        windowId = 1;
    }



>>>>>>> main
}
