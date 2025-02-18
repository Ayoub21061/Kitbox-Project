using System;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;



public partial class Seller
{
    public string email;
    public enum Status {totalypaid, partiallypaid, notpaid};
    public Status status;
    public int priceproduct ; 

    public Seller(string email, Status status){
        this.email = email;
        this.status = status;

        
         

          
    }




}