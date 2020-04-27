using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using McOrder.Annotations;

namespace McOrder
{
    class Order:INotifyPropertyChanged
    {
        #region properties

        #region OrderNr 

        private static int _nextOrderNr = 1; // Klassefelt (static instance field)
        private double _totalPrice;


        public static int NextOrderNr // Prop med backing field
        {
            get { return _nextOrderNr; }
            set { _nextOrderNr = value; }
        }

        public int OrderNr { get; set; } // prop
        #endregion

        public double TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; OnPropertyChanged(); }
        } // prop

        public ObservableCollection<Product> Products { get; set; } // prop af en liste 
        #endregion

        #region constructor

        public Order(ObservableCollection<Product> products) // Det er en constructor på hvad listen skal indeholde
        {
            OrderNr = NextOrderNr++; // Nyt nummer per ordre
            Products = products;
            TotalPrice = CalculateTotalPrice();
        }

        #endregion
        
        #region Calculate price
        
        /// <summary>
        /// Udregner totalprisen på alle de produkter der er i Products
        /// </summary>
        /// <returns>Totalprisen for produkterne</returns>
        public double CalculateTotalPrice()
        {
            double totalPrice = 0; // sætter vores start totalprice til 0
            foreach (Product product in Products)
            {
                totalPrice += product.Price; // Henter pris på hver product (prikker sig ind på prisen til hver produkt i ordren)
            }

            return totalPrice;
        } 
        #endregion

        public override string ToString()
        {
            return $"OrderNr: {OrderNr}, TotalPrice: {TotalPrice}, Products: {Products}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
