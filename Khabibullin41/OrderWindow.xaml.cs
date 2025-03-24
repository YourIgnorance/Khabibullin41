using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
        //private Order _currentOrder = new Order();
        //private OrderProduct _currentOrderProduct = new OrderProduct();
        public bool isClosed = false;
        private bool _guestMode;
        private int _clientID, _orderCode, _orderID, _orderPickupPoint, SumProduct = 0, SumProductWithDiscount = 0;
        private DateTime _orderDate = DateTime.Now, _orderDeliveryDate;
        public OrderWindow(List<OrderProduct> selectedOrderProducts, List<Product> selectedProducts, string FIO, int clientId, bool guestMode)
        {
            InitializeComponent();

            var currentPickups = Khabibullin41Entities.getInstance().PickupPoint.Select(p => p.PickupPointIndex + p.PickupPointCity + p.PickupPointStreet + " " + p.PickupPointHouse).ToList();
            PickupCombo.ItemsSource = currentPickups;

            isClosed = false;
            _clientID = clientId;
            _orderCode = Khabibullin41Entities.getInstance().Order.ToList().Last().OrderCode + 1;
            _orderID = selectedOrderProducts.Last().OrderID + 1;
            ClientTB.Text = FIO;
            TBOrderID.Text = _orderID.ToString();
            _guestMode = guestMode;
            
            ShoeListView.ItemsSource = selectedProducts;
            this.selectedOrderProducts = selectedOrderProducts;
            this.selectedProducts = selectedProducts;
            Refresh();
            SetDeliveryDate();
            foreach (Product product in selectedProducts)
            {
                SumProduct += product.ProductCostInt;
                SumProductWithDiscount += product.ProductPriceWithDiscount;
            }
            TBSumProduct.Text = SumProduct.ToString();
            TBSumProductDiscount.Text = SumProductWithDiscount.ToString();
        }
        private void Refresh()
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
            newOrder.OrderCode = _orderCode;
            newOrder.OrderStatus = "Новый";
            if(_guestMode)
                newOrder.OrderClientID = null;
            else
                newOrder.OrderClientID = _clientID;

            //MessageBox.Show($"id: {_orderID} \norderDate: {_orderDate}\norderDeliveryDate:{_orderDeliveryDate}\norderPickupPoint:{_orderPickupPoint}\norderClientID:{_clientID}\norderCode:{_orderCode}");

            foreach (Product selectprod in selectedProducts)    
            {
                //MessageBox.Show($"OrderPRoduct:\nid: {_orderID}\narcticle:{selectprod.ProductArticleNumber}\nprodcount:{selectprod.OrderProductCount}");
                OrderProduct newOrderProd = new OrderProduct();
                newOrderProd.OrderID = _orderID;
                newOrderProd.ProductArticleNumber = selectprod.ProductArticleNumber;
                newOrderProd.ProductCount = selectprod.GetOrderProductCount;

                newOrderProducts.Add(newOrderProd);
            }

            Khabibullin41Entities.getInstance().Order.Add(newOrder);
            foreach (OrderProduct ordprod in newOrderProducts)
                Khabibullin41Entities.getInstance().OrderProduct.Add(ordprod);
            try
            {
                Khabibullin41Entities.getInstance().SaveChanges();
                List<Product> prodList = new List<Product>(); 
                MessageBox.Show("Заказ успешно оформлен");
                this.Close(); //удаляется только на этой форме в другом классе нет
                foreach (Product product in selectedProducts)
                {
                    prodList.Add(product);
                }
                foreach (Product product in prodList)
                {
                    selectedProducts.Remove(product);
                }
                ProductPage.getInstance().ProductListView.SelectedItems.Clear();
                ProductPage.getInstance().UpdateVisiabilityOrder();
                isClosed = true;
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
                    if (prod.OrderProductCount >= origProd.ProductQuantityInStock)
                    {
                        MessageBox.Show($"Выбрано максимальное кол-во товаров!");
                        return;
                    }
                }
            }

            prod.OrderProductCount++;
            //prod.ProductQuantityInStock--;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].ProductCount = prod.OrderProductCount;
            
            SumProduct += prod.ProductCostInt;
            SumProductWithDiscount += prod.ProductPriceWithDiscount;
            TBSumProduct.Text = SumProduct.ToString();
            TBSumProductDiscount.Text = SumProductWithDiscount.ToString();
            
            SetDeliveryDate();
            Refresh();
            ShoeListView.Items.Refresh();
        }

        private void minusBt_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Product;
            prod.OrderProductCount--;
            var selectedOP = selectedOrderProducts.FirstOrDefault(p => p.ProductArticleNumber == prod.ProductArticleNumber);
            int index = selectedOrderProducts.IndexOf(selectedOP);
            selectedOrderProducts[index].ProductCount = prod.OrderProductCount;
            
            if (prod.OrderProductCount <= 0)
                selectedProducts.Remove(prod);
            //else
            //    prod.ProductQuantityInStock++; если не удаляем то прибавляем в продукт (возвращаем)
            SumProduct -= prod.ProductCostInt;
            SumProductWithDiscount -= prod.ProductPriceWithDiscount;
            TBSumProduct.Text = SumProduct.ToString();
            TBSumProductDiscount.Text = SumProductWithDiscount.ToString();

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