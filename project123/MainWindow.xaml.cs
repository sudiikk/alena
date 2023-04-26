using project123.Forms;
using project123.Models;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
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

namespace project123
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> ListProduct { get; set; }
        public Product SelectedProduct { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ListProduct = new();
            DataContext = this;
            this.Loaded += gg_Loaded;

        }
        private void gg_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseContext database = new DatabaseContext();
            database.Database.Migrate();
            List<Product> products = database.Products.ToList();
            Products.ItemsSource = products;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            foreach (Product product in products)
            {
                string combined = "Уникальный идентификатор: " + product.ID + "\r\n" + "Имя товара: " + product.Name + "\r\n" + "Описание товара: " + product.Description + "\r\n" + "Цена товара: " + product.Price + " RUB";
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(combined, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                BitmapImage qrCodeImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode.GetGraphic(20).Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    qrCodeImage.BeginInit();
                    qrCodeImage.CacheOption = BitmapCacheOption.OnLoad;
                    qrCodeImage.StreamSource = stream;
                    qrCodeImage.EndInit();
                }

                ListProduct.Add(new Product { Name = product.Name, Price = product.Price, QRCode = qrCodeImage, Description = product.Description, ID = product.ID });
            }
            Products.ItemsSource = ListProduct;
        }
        private void btn_sohr_Click(object sender, RoutedEventArgs e)
        {
            Forms.Dialog add = new Forms.Dialog();
            add.ShowDialog();
            ListProduct.Add(add.Product);

        }


        private void btn_red_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Products.SelectedItem != null)
                {

                    var product = Products.SelectedItem as Product;
                    if (new Forms.Edit_win(product).ShowDialog() == true)
                    {
                        using (var context = new DatabaseContext())
                        {
                            context.Entry(product).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        Products.Items.Refresh();
                    }
                }
                MainWindow mainWindow = new MainWindow();
                Close();
                mainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Пожалуйста заполните все поля");
                return;
            }
        }

        private void btn_yd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Products.SelectedItem != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены?", "Удалить", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var product = Products.SelectedItem as Product;
                        using (var context = new DatabaseContext())
                        {
                            context.Products.Remove(product);
                            context.SaveChanges();
                            Products.ItemsSource = context.Products.ToList();


                        }
                    }
                }
                MainWindow mainWindow = new MainWindow();
                Close();
                mainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("   Пожалуйста заполните все поля c помощью кнопки Редактировать  ");
                return;
            }
        }


    }
}
