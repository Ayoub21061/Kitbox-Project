using CommunityToolkit.Mvvm.ComponentModel;

namespace Kitbox.ViewModels
{
    public partial class Secretary : User
    {

        


        public Secretary()
        {
            username = "Secretary";
            password = "HelloWorld";
            windowId = 3; // 3 est l'Id de la fen�tre de l'interface graphique pour les secr�taires.
        }

    }
}
