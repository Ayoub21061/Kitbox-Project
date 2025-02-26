using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kitbox.ViewModels;

public partial class LockerViewModel : ObservableObject
{
    [ObservableProperty]
    private string couleur = string.Empty;

    [ObservableProperty]
    private int longueur;

    [ObservableProperty]
    private bool hasPorte;

    public LockerViewModel()
    {
        // Initialisation si nécessaire
    }
}


