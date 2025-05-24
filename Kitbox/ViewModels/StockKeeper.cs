using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.ViewModels;
using System;


<<<<<<< HEAD
namespace Kitbox.ViewModels
{
    public class User
    {
        public string username { get; set; } = string.Empty;
=======

public class StockKeeper : User
{
   

   public StockKeeper() {

        username = "StockKeeper";
        password = "Merge42";
        windowId = 1;
>>>>>>> main
    }

<<<<<<< HEAD
    public abstract class StockKeeper : AppUser
    {
        public StockKeeper()
        {
            Username = "StockKeeper";
        }
    }
=======

>>>>>>> main
}

