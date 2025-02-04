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
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            
            var currentProducts = Khabibullin41Entities.getInstance().Product.ToList();

            ProductListView.ItemsSource = currentProducts;

            SortComboBox.SelectedIndex = 0;
            FiltrComboBox.SelectedIndex = 0;

            UpdateProducts();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }
        public void UpdateProducts()
        {
            var currentAgents = Khabibullin41Entities.getInstance().Product.ToList();


            if (SortComboBox.SelectedIndex == 0)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductArticleNumber).ToList();
            }
            if (SortComboBox.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductName).ToList();
            }
            if (SortComboBox.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.ProductName).ToList();
            }
            if (SortComboBox.SelectedIndex == 3)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductCostInt).ToList();
            }
            if (SortComboBox.SelectedIndex == 4)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.ProductCostInt).ToList();
            }
            if (SortComboBox.SelectedIndex == 5)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductDiscountAmount).ToList();
            }
            if (SortComboBox.SelectedIndex == 6)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.ProductDiscountAmount).ToList();
            }
            if (SortComboBox.SelectedIndex == 7)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductQuantityInStock).ToList();
            }
            if (SortComboBox.SelectedIndex == 8)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.ProductQuantityInStock).ToList();
            }
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

            currentAgents = currentAgents.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            ProductListView.ItemsSource = currentAgents;
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
    }
}
