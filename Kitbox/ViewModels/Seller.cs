using System;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pong.ViewModels;

public partial class Seller{
    public string email;
    public enum Status {totalypaid, partiallypaid, notpaid};
    public Status status;
    public 

    public Seller(string email, Status status){
        this.email = email;
        this.status = status;

        
         

          
    }




}