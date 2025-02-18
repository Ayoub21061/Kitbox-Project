using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.ViewModels;
using System;

namespace Kitbox.ViewModels;


public class StockKeeper : User
{
   

   public StockKeeper() {
        username = "StockKeeper";
        password = "Merge42";
        windowId = 1;
    }
   


}
