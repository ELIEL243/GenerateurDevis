using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using iTextSharp.text.pdf.draw;

namespace GenerateurDevis
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static string cs = "Data Source=Store.db;Version=3;New=True;Compress=True;";
        public static MainWindow instance;
        public Produit prod = new Produit();
        public static int qts = 0;
        public static List<Produit> products;
        public LineItem line = new LineItem();

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            CreateTable(GetConnexion());
            PrintProductList();
            
        }
        public static SQLiteConnection GetConnexion()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection(cs);
            return sqlite_conn;
        }

        public static void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            conn.Open();
            string Createsql = "CREATE TABLE IF NOT EXISTS product(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, price INTEGER)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();

        }

        public void PrintProductList()
        {
            product_list.ItemsSource = prod.GetProducts();
        }

        public void AddCell(string str, Font f, BaseColor c, PdfPTable t)
        {
            PdfPCell cell1 = new PdfPCell(new Phrase(str, f));
            cell1.BackgroundColor = c;
            cell1.Padding = 7;
            t.AddCell(cell1);

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
        }

        private void SelectProd(object sender, MouseButtonEventArgs e)
        {
            Qts qts = new Qts();
            qts.Show();
            FocusManager.SetFocusedElement(this, Qts.instance.quantity);
            int index = product_list.SelectedIndex;
            products = prod.GetProducts();
            Produit produit = products[index];
            Qts.select_prod = produit;
            //PrintLineItems();
            
        }

        private void line_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(clt_name.Text))
            {
                MessageBox.Show("Veuillez entrer le nom du client", "Champ vide", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {

                var id = DateTime.Now.Ticks;
                string outFile = Environment.CurrentDirectory + $"/devis{id}.pdf";
                Document doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream(outFile, FileMode.Create));
                doc.Open();
                //

                BaseColor blue = new BaseColor(0, 75, 155);
                BaseColor gris = BaseColor.DARK_GRAY;
                BaseColor blanc = new BaseColor(255, 255, 255);

                Font police = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18f, iTextSharp.text.Font.NORMAL, blue);
                Font police2 = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18f, iTextSharp.text.Font.NORMAL, gris);
                Font police3 = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18f, iTextSharp.text.Font.NORMAL, blanc);


                Paragraph p = new Paragraph("Eliel Store, Golf 3456\n", police);
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);

                Paragraph p2 = new Paragraph("Client: " + clt_name.Text + "\n", police);
                p2.Alignment = Element.ALIGN_LEFT;
                doc.Add(p2);

                Paragraph p5 = new Paragraph( DateTime.Now.ToString("dddd dd MMMM yyyy")+ "\n\n", police);
                p5.Alignment = Element.ALIGN_LEFT;
                doc.Add(p5);


                Paragraph p3 = new Paragraph($"DEVIS N°{id}\n\n", police);
                p3.Alignment = Element.ALIGN_CENTER; 
                doc.Add(p3);

                iTextSharp.text.pdf.draw.LineSeparator line = new LineSeparator(1f, 50f, blue, Element.ALIGN_CENTER, 1);
                doc.Add(line);
                doc.Add(new Paragraph("\n\n"));

                //Creation de la table

                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;

                AddCell("Designation", police3, blue, table);
                AddCell("Prix", police3, blue, table);
                AddCell("Qts", police3, blue, table);
                AddCell("Total", police3, blue, table);

                //Lister les produits

                foreach(LineItem l in LineItem.lines)
                {
                    AddCell(l.description, police2, blanc, table);
                    AddCell(l.price.ToString(), police2, blanc, table);
                    AddCell(l.quantity.ToString(), police2, blanc, table);
                    AddCell(l.total.ToString(), police2, blanc, table);

                }
                doc.Add(table);
                doc.Add(new Phrase("\n\n"));
                Paragraph p4 = new Paragraph($"Total: {total.Content}\n", police);
                p4.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p4);
                doc.Close();
                Console.WriteLine(outFile);
                Process.Start(outFile);
                line_items_table.ItemsSource = null;
                clt_name.Clear();
                LineItem.lines.Clear();


            }
        }
    }
}
