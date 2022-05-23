using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace SQLiteDemo
{
    class Program
    {
         //I was here 
         // It will be my addition  
         //3rd line
         
         //I was here  
         // I am in branch ap01
         // line 3 
        static void Main(string[] args)
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            CreateTable(sqlite_conn);

            Console.WriteLine("Select option: ");
            Console.WriteLine("              1 -> display all data ");
            Console.WriteLine("              2 -> display places ");
            Console.WriteLine("              3 -> insert data ");
            Console.WriteLine("              4 -> delete data ");
            Console.WriteLine("              0 -> exit ");
            string key = Console.ReadLine();
            string city = null; 

            if (key == "1")
            {
                ReadData(sqlite_conn, 0, city);

            } else if(key == "2")
            {
                Console.WriteLine("Enter the place: ");
                city = Console.ReadLine();
                city = city.ToUpper();

                ReadData(sqlite_conn, 1, city);

            } else if(key == "3")
            {
                InsertData(sqlite_conn);
                //ReadData(sqlite_conn, 0, city);
            }else if(key == "4")
            {
                ReadData(sqlite_conn, 2, city);
                Console.WriteLine("Select the id to delete: ");
                string id = Console.ReadLine();
                int refe = Convert.ToInt32(id);

                DeleteData(sqlite_conn, refe);
                Console.WriteLine(" ");
                ReadData(sqlite_conn, 0, city);


            }
            else if(key == "0")
            {
                Console.WriteLine("Exiting ...");
            }
            
            //InsertData(sqlite_conn);
            //ReadData(sqlite_conn);




        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database no found");
            }
            return sqlite_conn;
        }

        static void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string strCosts = "CREATE TABLE if not exists  Costs(id INTEGER PRIMARY KEY AUTOINCREMENT, cost double, place_id INT)";
            string strPlaces = "CREATE TABLE if not exists Places(id INTEGER PRIMARY KEY AUTOINCREMENT, place VARCHAR(20), date VARCHAR(20))";
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = strCosts;
            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = strPlaces;
            sqlite_cmd.ExecuteNonQuery();

        }

        static void InsertData(SQLiteConnection conn)
        {
            Console.Write("Enter the place: ");
            string place = Console.ReadLine();
            place = place.ToUpper();

            Console.Write("Enter the date: ");
            string date1 = Console.ReadLine();
            date1 = date1.ToUpper();

            Console.Write("Enter the cost: ");
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double cost = Convert.ToDouble(Console.ReadLine(), provider);

            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO Places(place, date) VALUES('" + place + "','" + date1 + "' ); ";
            //Console.WriteLine(sqlite_cmd.CommandText);
            sqlite_cmd.ExecuteNonQuery();
            //------------------------
            SQLiteDataReader sqlite_datareader;
            //SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Places ";
            int place_id = 0;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                place_id = (int)sqlite_datareader.GetInt64(0);
            }
            //------------------------
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO Costs(cost, place_id) VALUES(" + cost + "," + place_id + " ); ";
            Console.WriteLine(sqlite_cmd.CommandText);
            sqlite_cmd.ExecuteNonQuery();

        }

        static void ReadData(SQLiteConnection conn, int item, string city)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT places.id, places.place, places.date, costs.cost FROM Places, Costs WHERE places.id = costs.place_id ";

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            if (item == 0)
            {
                while (sqlite_datareader.Read())
                {
                    int col0 = (int)sqlite_datareader.GetInt32(0);
                    string col1 = sqlite_datareader.GetString(1);
                    col1 = col1.ToUpper();
                    string col2 = sqlite_datareader.GetString(2);
                    col2 = col2.ToUpper();
                    double col3 = sqlite_datareader.GetDouble(3);

                    Console.WriteLine("{0} {1} {2} {3}", col0, col1, col2, col3);
                    /*
                     * int col0 = sqlite_datareader.GetInt32(0);
                        string col1= sqlite_datareader.GetString(1);
                         string col2 = sqlite_datareader.GetString(2);
                         Console.WriteLine("{0} - {1} - {2}" , col0, col1, col2);
                     */
                }
                conn.Close();
            } else if (item == 1)
            {
                while (sqlite_datareader.Read())
                {
                    //int col0 = (int)sqlite_datareader.GetInt64(0);
                    string col1 = sqlite_datareader.GetString(0);
                    col1 = col1.ToUpper();

                    string col2 = sqlite_datareader.GetString(1);
                    col2 = col2.ToUpper();

                    double col3 = sqlite_datareader.GetDouble(2);
                    if (col1 == city)
                    {
                        Console.WriteLine("{0} {1} {2}", col1, col2, col3);

                    }
                }
                //Console.WriteLine("It doesnt find the name ...");
                conn.Close();

            } else if (item == 2){
                while (sqlite_datareader.Read())
                {
                    int col0 = (int)sqlite_datareader.GetInt32(0);
                    string col1 = sqlite_datareader.GetString(1);
                    col1 = col1.ToUpper();
                    string col2 = sqlite_datareader.GetString(2);
                    col2 = col2.ToUpper();
                    double col3 = sqlite_datareader.GetDouble(3);

                    Console.WriteLine("{0} {1} {2} {3}", col0, col1, col2, col3);
                }         
            }
        }

        static void DeleteData(SQLiteConnection conn, int refe)
        {/*
            Console.Write("Enter the place: ");
            string place = Console.ReadLine();
            place = place.ToUpper();

            Console.Write("Enter the date: ");
            string date1 = Console.ReadLine();
            date1 = date1.ToUpper();

            Console.Write("Enter the cost: ");
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double cost = Convert.ToDouble(Console.ReadLine(), provider);
            */
            SQLiteCommand sqlite_cmd;
            SQLiteCommand sqlite_cmd_2;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd_2 = conn.CreateCommand();

            sqlite_cmd.CommandText = "Delete FROM Places WHERE places.id = " + refe + ";";
            sqlite_cmd_2.CommandText = "Delete FROM Costs WHERE costs.place_id = " + refe + ";";
            //Console.WriteLine(sqlite_cmd.CommandText);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd_2.ExecuteNonQuery();
            //------------------------
            
            /*
            SQLiteDataReader sqlite_datareader;
            //SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Places ";
            int place_id = 0;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                place_id = (int)sqlite_datareader.GetInt64(0);
            }
            //------------------------
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO Costs(cost, place_id) VALUES(" + cost + "," + place_id + " ); ";
            Console.WriteLine(sqlite_cmd.CommandText);
            sqlite_cmd.ExecuteNonQuery();
            */
        }
    }
}