using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kitbox.Models
{
    public class Stock : INotifyPropertyChanged
    {
        private string _productId;
        private string _articleName;
        private string _articleStatus;
        private int? _articleQuantity;

        public string ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }

        public string ArticleName
        {
            get => _articleName;
            set => SetProperty(ref _articleName, value);
        }

        public string ArticleStatus
        {
            get => _articleStatus;
            set => SetProperty(ref _articleStatus, value);
        }

        public int? ArticleQuantity
        {
            get => _articleQuantity;
            set => SetProperty(ref _articleQuantity, value);
        }

        // Implémentation de l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Méthode helper pour déclencher PropertyChanged
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
