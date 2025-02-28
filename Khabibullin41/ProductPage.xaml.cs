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
        public ProductPage(User user)
        {
            InitializeComponent();

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

            UpdateProducts();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
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
                currentAgents = currentAgents.OrderByDescending(p => p.ProductCostInt).ToList();
            }
            if (SortComboBox1.IsChecked == true)
            {
                currentAgents = currentAgents.OrderBy(p => p.ProductCostInt).ToList();
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

        private void SortComboBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }
        private void SortComboBox1_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }
    }
}
