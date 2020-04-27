using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Interop;
using Eventmaker.Common;
using McOrder.Annotations;

namespace McOrder
{
    class McViewModel
    {
        

        #region Properties
        public static ObservableCollection<string> CategoryButtons{ get; set; } // Knapper i bunden
        public static ObservableCollection<Product> LoadedProducts { get; set; } // Produkterne i gridviewet
        public static ObservableCollection<Product> CurrentOrder { get; set; } // Produkter igangværende order
        public static ObservableCollection<Order> Orderlist { get; set; } // Listen af ordre
        public static int ItemToRemoveFromCurrentOrder { get; set; }
        public static int IdOfSelectedOrderInOrderList { get; set; }

        public static double TotalPriceOfCurrentOrder { get; set; } //et tal der skal opdateres hver gang en ny ting bliver tilføjet til current order

        public static ICommand DeliverOrderCommand { get; set; } // Command til at udleverer order
        public static ICommand SelectOrderCommand { get; set; } // Vi kan vælge en order fra orderlisten til redigering


        #endregion

        #region Constructor

        public McViewModel()
        {
            CategoryButtons = new ObservableCollection<string>();
            LoadedProducts = new ObservableCollection<Product>();
            CurrentOrder = new ObservableCollection<Product>();
            Orderlist = new ObservableCollection<Order>();

            DeliverOrderCommand = new RelayCommand(DeliverOrder);
            //SelectOrderCommand = new RelayCommand(StartEditOfOrderInOrderList);

            LoadButtons();
        }

        #endregion

        #region Metode/Funktion

        #region FileMethods

       public async static void LoadProductsFromFile(string fileName)
       {

            LoadedProducts.Clear();

            ObservableCollection<Product> loading = await JsonSerilizer.ReadProducts(fileName);

            foreach (Product product in loading)
            {
                LoadedProducts.Add(product);
            }

          

       }

        public async void LoadButtons()
        {
            List<string> buttonList = await JsonSerilizer.GetButtons();

            foreach (string button in buttonList)
            {
                CategoryButtons.Add(button);
            }

            LoadProductsFromFile(CategoryButtons[0]);

        }

        #endregion

        #region CurrentOrderMethods

        public static double UpdateTotalPriceInCurrentOrder()
        {
            TotalPriceOfCurrentOrder = 0;
            foreach (Product item in CurrentOrder)
            {
                TotalPriceOfCurrentOrder += item.Price;
            }
            return TotalPriceOfCurrentOrder; 
        }


         public static void AddToCurrentOrder(Product product)
        {
            CurrentOrder.Add(product);
        }

        public static void RemoveFromCurrentOrder()
        {
            if (CurrentOrder.Count>0)
            {
                CurrentOrder.RemoveAt(ItemToRemoveFromCurrentOrder);
                ItemToRemoveFromCurrentOrder = 0;
            }
            
        }

        #endregion

        #region OrderListMethods

        public static void AddCurrentOrderToOrderList()
        {
            ObservableCollection<Product> productsInNewOrder = new ObservableCollection<Product>();

            foreach (Product product in CurrentOrder)
            {
                productsInNewOrder.Add(product);
            }

            Order order = new Order(productsInNewOrder);

            CurrentOrder.Clear();
            Orderlist.Add(order);
        }

        public static void StartEditOfOrderInOrderList()
        {
            CurrentOrder.Clear();
            foreach (Product product in Orderlist[IdOfSelectedOrderInOrderList].Products)
            {
                CurrentOrder.Add(product);
            }

        }

        public static void EditOrderFromOrderList()
        {

            Order theOrderToEdit = Orderlist[IdOfSelectedOrderInOrderList];

            theOrderToEdit.Products.Clear();

            foreach (Product product in CurrentOrder)
            {
                theOrderToEdit.Products.Add(product);
            }

            theOrderToEdit.TotalPrice = UpdateTotalPriceInCurrentOrder();

            CurrentOrder.Clear();
        }

        public void DeliverOrder()
        {

            if (Orderlist.Count>0)
            {
                Orderlist.RemoveAt(IdOfSelectedOrderInOrderList);
                IdOfSelectedOrderInOrderList = 0;

            }

        }

        #endregion

        #endregion
    }
}
