using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.ViewModels;
using System;


namespace Kitbox.ViewModels
{
    public class User
    {
        public string username { get; set; } = string.Empty;
    }

    public abstract class StockKeeper : AppUser
    {
        public StockKeeper()
        {
            Username = "StockKeeper";
        }
    }
}

