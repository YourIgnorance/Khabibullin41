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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Khabibullin41
{
    public partial class ProductPage : Page
    {
        List<Product> selectedProdList = new List<Product>();
        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        private bool _guestMode = false;
        private int _newOrderID = Khabibullin41Entities.getInstance().OrderProduct.Count() + 1;
        private int _clientID;
        public ProductPage(User user)
        {
            InitializeComponent();

            BtnOrder.Visibility = Visibility.Hidden;

            selectedOrderProducts = Khabibullin41Entities.getInstance().OrderProduct.ToList();

            TBUsername.Text = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";

            switch(user.UserRole)
            {
                case 1:
                    TBRole.Text = "Клиент";
                    break;
                case 2:
                    TBRole.Text = "Менеджер";
                    break;
                case 3:
                    TBRole.Text = "Администратор";
                    break;
            }

            var currentProducts = Khabibullin41Entities.getInstance().Product.ToList();

            ProductListView.ItemsSource = currentProducts;

            FiltrComboBox.SelectedIndex = 0;

            _clientID = user.UserID;

            UpdateProducts();
        }
        public ProductPage()
        {
            InitializeComponent();

            _guestMode = true;

            BtnOrder.Visibility = Visibility.Hidden;

            TBUsername.Text = $"Гость";

            var currentProducts = Khabibullin41Entities.getInstance().Product.ToList();

            TBRole.Text = $"Гость";

            ProductListView.ItemsSource = currentProducts;

            FiltrComboBox.SelectedIndex = 0;

            UpdateProducts();
        }
        public void UpdateProducts()
        {
            var currentAgents = Khabibullin41Entities.getInstance().Product.ToList();

            if (FiltrComboBox.SelectedIndex == 0)
            {
                currentAgents = currentAgents.Where(p => Convert.ToInt32(p.ProductDiscountAmount) <= 100).ToList();
            }
            if (FiltrComboBox.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => Convert.ToInt32(p.ProductDiscountAmount) < 10).ToList();
            }
            if (FiltrComboBox.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductDiscountAmount) < 15).ToList();
            }
            if (FiltrComboBox.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => Convert.ToInt32(p.ProductDiscountAmount) >= 15 && Convert.ToInt32(p.ProductDiscountAmount) <= 100).ToList();
            }

            if (SortComboBox.IsChecked == true)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductCostInt).ToList();
            }
            if (SortComboBox1.IsChecked == true)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.ProductCostInt).ToList();
            }

            currentAgents = currentAgents.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            ProductListView.ItemsSource = currentAgents;
            TBlockAllCount.Text = Convert.ToString(Khabibullin41Entities.getInstance().Product.ToList().Count());
            TBlockCount.Text = Convert.ToString(currentAgents.Count());
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void FiltrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void SortComboBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }
        private void SortComboBox1_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void ProductListView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            
            if (item != null)
            {
                (sender as ListView).ContextMenu.IsOpen = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_guestMode)
            {
                MessageBox.Show("Это функция доступна только авторизированным пользователям!");
                return;
            }
            if (ProductListView.SelectedIndex >= 0)
            {
                var selectedProduct = ProductListView.SelectedItem as Product;
                selectedProdList.Add(selectedProduct);

                var newOrderProd = new OrderProduct();
                newOrderProd.OrderID = _newOrderID;

                newOrderProd.ProductArticleNumber = selectedProduct.ProductArticleNumber;
                newOrderProd.ProductCount = 1; //кол-во orderproduct


                var selOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, selectedProduct.ProductArticleNumber));

                if (selOP.Count() == 0)
                {
                    //MessageBox.Show($"{_newOrderID}");
                    //_newOrderID+=1;
                    selectedOrderProducts.Add(newOrderProd);
                }
                else
                {
                    foreach (OrderProduct p in selectedOrderProducts)
                    {
                        if (p.ProductArticleNumber == selectedProduct.ProductArticleNumber)
                            p.ProductCount++;
                    }
                }
                BtnOrder.Visibility = Visibility.Visible;
                ProductListView.SelectedIndex = -1;
            }
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            selectedProdList = selectedProdList.Distinct().ToList();

            OrderWindow orderWindow = new OrderWindow(selectedOrderProducts, selectedProdList, TBUsername.Text, _clientID);
            orderWindow.ShowDialog();

        }
    }
}
