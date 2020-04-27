using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace McOrder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

       

        private void Add_To_Current_Order(object sender, ItemClickEventArgs e)
        {

            Product product = (Product)e.ClickedItem;


            McViewModel.AddToCurrentOrder(product);

            UpdatePriceInCurrentOrder();

        }

        private void RemoveItemFromCurrentOrder(object sender, RoutedEventArgs e)
        {

            McViewModel.RemoveFromCurrentOrder();

            UpdatePriceInCurrentOrder();
        }

        private void AddOrderToOrderList(object sender, RoutedEventArgs e)
        {

            McViewModel.AddCurrentOrderToOrderList();

            UpdatePriceInCurrentOrder();
        }

        private void LoadProductsFromButtonFile(object sender, ItemClickEventArgs e)
        {

            string button = (string)e.ClickedItem;

            McViewModel.LoadProductsFromFile(button);

        }

        private void StartEditOfOrderInOrderList(object sender, RoutedEventArgs e)
        {
            
            McViewModel.StartEditOfOrderInOrderList();

            UpdatePriceInCurrentOrder();

            addOrder.Visibility = Visibility.Collapsed;
            editOrder.Visibility = Visibility.Visible;

        }

        private void EditOrderInOrderList(object sender, RoutedEventArgs e)
        {
            McViewModel.EditOrderFromOrderList();

            editOrder.Visibility = Visibility.Collapsed;
            addOrder.Visibility = Visibility.Visible;
            UpdatePriceInCurrentOrder();
        }

        private void UpdatePriceInCurrentOrder()
        {
            CurrentTotalPrice.Text = McViewModel.UpdateTotalPriceInCurrentOrder().ToString();
        }
    }
}
