using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private int _clientID, _orderCode, _orderID, _orderPickupPoint;
        private DateTime _orderDate = DateTime.Now, _orderDeliveryDate;
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string FIO, int clientId)
        {
            InitializeComponent();

            var currentPickups = Khabibullin41Entities.getInstance().PickupPoint.Select(p => p.PickupPointIndex + p.PickupPointCity + p.PickupPointStreet + " " + p.PickupPointHouse).ToList();
            PickupCombo.ItemsSource = currentPickups;

            _clientID = clientId;
            _orderCode = Khabibullin41Entities.getInstance().Order.ToList().Last().OrderCode + 1;
            _orderID = selectedOrderProducts.Last().OrderID + 1;
            ClientTB.Text = FIO;
            TBOrderID.Text = _orderID.ToString();

            

            foreach (Product p in selectedProducts)
            {
                p.ProductQuantityInStock = 1;
                foreach (OrderProduct q in selectedOrderProducts)
                {
                    if (p.ProductArticleNumber == q.ProductArticleNumber)
                        p.ProductQuantityInStock = q.ProductCount;
                } 
            }
            //??????!
            //ShoeListView.ItemsSource = selectedProducts;
            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            Refresh();
            SetDeliveryDate();
        }
        public void Refresh()
        {
            ShoeListView.ItemsSource = selectedProducts;
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            List<OrderProduct> newOrderProducts = new List<OrderProduct>();
            Order newOrder = new Order();

            _orderDate = (DateTime)OrderDP.SelectedDate;
            _orderDeliveryDate = (DateTime)OrderDeliveryDP.SelectedDate;
            _orderPickupPoint = PickupCombo.SelectedIndex + 1;

            SetDeliveryDate();
            if (selectedProducts.Count == 0)
            {
                MessageBox.Show("Ни одного продукта нет в корзине!");
                return;
            }
            if (_orderPickupPoint == 0)
            {
                MessageBox.Show("Ни выбрана точка вывоза!");
                return;
            }

            newOrder.OrderID = _orderID;
            newOrder.OrderDate = _orderDate;
            newOrder.OrderDeliveryDate = _orderDeliveryDate;
            newOrder.OrderPickupPoint = _orderPickupPoint;
            newOrder.OrderClientID = _clientID;
            newOrder.OrderCode = _orderCode;
            newOrder.OrderStatus = "Новый";

            //MessageBox.Show($"id: {_orderID} \norderDate: {orderDate}\norderDeliveryDate:{orderDeliveryDate}\norderPickupPoint:{_orderPickupPoint}\norderClientID:{_clientID}\norderCode:{_orderCode}");

            foreach (Product selectprod in selectedProducts)
            {
            //    MessageBox.Show($"OrderPRoduct:\nid: {_orderID}\narcticle:{selectprod.ProductArticleNumber}\nprodcount:{selectprod.ProductQuantityInStock}");
                OrderProduct newOrderProd = new OrderProduct();
                newOrderProd.OrderID = _orderID;
                newOrderProd.ProductArticleNumber = selectprod.ProductArticleNumber;
                newOrderProd.ProductCount = selectprod.ProductQuantityInStock;

                newOrderProducts.Add(newOrderProd);
            }
            //foreach (OrderProduct selprod in selectedOrderProducts)
            //    Khabibullin41Entities.getInstance().OrderProduct.Add(selprod);

            //Khabibullin41Entities.getInstance().Order.Add()
            
            foreach (OrderProduct ordprod in newOrderProducts)
                Khabibullin41Entities.getInstance().OrderProduct.Add(ordprod);
            Khabibullin41Entities.getInstance().Order.Add(newOrder);
            try
            {
                Khabibullin41Entities.getInstance().SaveChanges();

                MessageBox.Show("Заказ успешно оформлен");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            _orderCode++;
            _orderID++;
        }

        private void plusBt_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Product;
            foreach (Product origProd in Khabibullin41Entities.getInstance().Product)
            {
                if (prod.ProductArticleNumber == origProd.ProductArticleNumber)
                {
                    if (prod.ProductQuantityInStock >= origProd.ProductQuantityInStock)
                    {
                        MessageBox.Show("Выбрано максимальное кол-во товаров!");
                        return;
                    }
                }
            }
            prod.ProductQuantityInStock++;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].ProductCount++;
            SetDeliveryDate();
            ShoeListView.Items.Refresh();
        }

        private void minusBt_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Product;
            prod.ProductQuantityInStock--;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].ProductCount--;

            if (prod.ProductQuantityInStock <= 0)
                selectedProducts.Remove(prod);
            Refresh();
            SetDeliveryDate();
            ShoeListView.Items.Refresh();
        }
        private void SetDeliveryDate()
        {
            _orderDeliveryDate = _orderDate;
            bool isFastDelivery = true;
            foreach (Product prod in selectedProducts)
            {
                if (prod.ProductQuantityInStock <= 3)
                {
                    isFastDelivery = false;
                }
            }
            if (!isFastDelivery)
                _orderDeliveryDate = _orderDeliveryDate.AddDays(6);
            else
                _orderDeliveryDate = _orderDeliveryDate.AddDays(3);
            OrderDP.Text = _orderDate.ToString();
            OrderDeliveryDP.Text = _orderDeliveryDate.ToString();

        }
    }
}