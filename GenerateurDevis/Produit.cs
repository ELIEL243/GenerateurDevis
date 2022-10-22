using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace GenerateurDevis
{
    public class Produit
    {
        public int id { get; set; }
        public string name { get; set; }
        public int prix { get; set; }


        public List<Produit> GetProducts()
        {
            List<Produit> products = new List<Produit>();
            SQLiteConnection conn = MainWindow.GetConnexion();
            conn.Open();
            string query = "select * from product";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                products.Add(new Produit() { id = dr.GetInt32(0), name = dr.GetString(1), prix = dr.GetInt32(2) });

                
            }
           
            return products;
        }

        //public override string ToString()
        //{
        //    return this.name;
        //}
    }

    }
