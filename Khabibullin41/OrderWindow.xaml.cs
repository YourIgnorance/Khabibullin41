using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Khabibullin41
{

    public partial class OrderWindow : Window
    {
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>(); 
        List<Product> selectedProducts = new List<Product>();
        private Order currentOrder = new Order();
        private OrderProduct currentOrderProduct = new OrderProduct();
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string FIO)
        {
            InitializeComponent();
            //var currentPickups = Khabibullin41Entities.getInstance().PickupPoint.ToList();
            //PickupCombo.ItemsSource = currentPickups;

            //ClientTB.Text = FIO;
            //TBOrderID.Text = selectedOrderProducts.First().OrderID.ToString();

            //ShoeListView.ItemsSource = selectedProducts;
            //foreach (Product p in selectedProducts)
            //{
            //    p.ProductQuantityInStock = 1;
            //    foreach (OrderProduct q in selectedOrderProducts)
            //    {
            //        if (p.ProductArticleNumber == q.ProductArticleNumber)
            //            p.ProductQuantityInStock = q.ProductCount;
            //    }
            //}
            //this.selectedOrderProducts = selectedOrderProducts;
            //this.selectedProducts = selectedProducts;
            //OrderDP.Text = DateTime.Now.ToString();
            //SetDeliveryDate();
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
           // currentOrder.OrderClientID = ClientTB.Text;
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            //selectedProducts = selectedProducts.Distinct().ToList();
            //OrderWindow orderWindow = new OrderWindow(selectedOrderProducts, selectedProducts, FIOTB.Text);
            //orderWindow.ShowDialog();
        }
        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            //var prod = (sender as Button).DataContext as Product;
            //prod.ProductQuantityInStock++;
            //var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber); //вытащить индекс найденного элемента
            //int index = selectedOrderProducts.IndexOf(selectedOP);
            //selectedOrderProducts[index].ProductCount++;
            //SetDeliveryDate();
            //ShoeListView.Items.Refresh();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //if (ShoeListView.SelectedIndex >= 0)
            //{
            //    var prod = ShoeListView.SelectedItem as Product;
            //    selectedProducts.Add(prod);

            //    var newOrderProd = new OrderProduct();
            //    newOrderProd.OrderID = newOrderID;

            //    newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
            //    newOrderProd.ProductCount = 1;


            //    var selOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));

            //    if (selOP.Count() == 0)
            //    {
            //        selectedOrderProducts.Add(newOrderProd);
            //    }
            //    else
            //    {
            //        foreach (OrderProduct p in selectedOrderProducts)
            //        {
            //            if (p.ProductArticleNumber == prod.ProductArticleNumber)
            //                p.ProductCount++;
            //        }
            //    }
            //    OrderBtn.Visibility = Visibility.Visible;
            //    ShoeListView.SelectedIndex = -1;
            //}
        }

        private void ShoeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}