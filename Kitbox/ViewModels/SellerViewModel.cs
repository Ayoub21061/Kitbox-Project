using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.Models;

namespace Kitbox.ViewModels
{
    public class SellerViewModel : ObservableObject
    {
        
        public Seller Seller { get; } = new Seller();
        


    }
}