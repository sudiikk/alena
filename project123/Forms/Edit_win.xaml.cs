using project123.Models;
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

namespace project123.Forms
{
    /// <summary>
    /// Логика взаимодействия для Edit_win.xaml
    /// </summary>
    public partial class Edit_win : Window
    {
        public Product Product { get; set; }
        public Edit_win(Product product)
        {
            InitializeComponent();
            Product = product;
            DataContext = Product;
        }

        private void btn_sohran_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Product product = new Product
            {
                Price = Product.Price,
                Description = Product.Description,
                ID = Product.ID,
                Name = Product.Name
            };
        }
        private void btn_otmena_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
