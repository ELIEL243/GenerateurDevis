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
using System.Text.RegularExpressions;

namespace GenerateurDevis
{
    /// <summary>
    /// Logique d'interaction pour Qts.xaml
    /// </summary>
    public partial class Qts : Window
    {
        public static Qts instance;
        public static int value_int = 0;
        public static Produit select_prod;
        public LineItem line = new LineItem();
        public MainWindow m = new MainWindow();
        public Qts()
        {
            InitializeComponent();
            instance = this;
        }

        private void quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Soustraire(object sender, KeyEventArgs e)
        {
            
            if(e.Key == Key.Down)
            {

                if (value_int > 1)
                {
                    value_int -= 1;
                    quantity.Text = value_int.ToString();
                }
            }

            if (e.Key == Key.Up)
            {
               value_int += 1;
                                    
               quantity.Text = value_int.ToString();
                
            }

            if (e.Key == Key.Enter)
            {
                line.AddItem(select_prod, value_int);
                value_int = 0;
                foreach(LineItem l in line.GetLineItems())
                {
                    Console.WriteLine(l.description);
                }
                ((MainWindow)Application.Current.MainWindow).line_items_table.ItemsSource = null;
                ((MainWindow)Application.Current.MainWindow).line_items_table.ItemsSource = line.GetLineItems();
                ((MainWindow)Application.Current.MainWindow).total.Content = line.GetAbsoluteTotal().ToString();

                this.Close();
                //this.Hide();

            }


        }

    }
}
