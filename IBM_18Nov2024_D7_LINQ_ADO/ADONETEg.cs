using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace IBM_18Nov2024_D7_LINQ_ADO
{
    internal class ADONETEg
    {

        static void Main()
        {

            SqlConnection _cn = new SqlConnection();
            //_cn.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IBM08Nov2024Db;Integrated Security=True;"; ;

            SqlConnectionStringBuilder _cnb = new SqlConnectionStringBuilder();

            _cnb.DataSource = "(localdb)\\MSSQLLocalDB";
            _cnb.IntegratedSecurity= true;
            _cnb.InitialCatalog = "IBM08Nov2024Db";

            _cn.ConnectionString = _cnb.ConnectionString;

            //_cn.Open();


            int choice = -1;

            do
            {

                Console.Clear();
              

                Console.WriteLine("1. List Product(Connection & Command with Reader)\n2. Add New Product\n3. Update Product By ID \n4. Delete Product By ID \n5. Invoke Stored Proc\n6. CRUD using Dataset   \n0. Exit ");

                Console.Write("Enter Choice :");
                choice=  int.Parse( Console.ReadLine());


                if (choice==1)
                {
                    ListProductsUsingConComRdr(_cn);

                }

                if (choice==2)
                {
                    AddNewProduct(_cn);

                }


            }
            while (choice!=0);





        }

        private static void AddNewProduct(SqlConnection _cn)
        {
            Console.Clear();
            Console.WriteLine("Add New Product ");

            Console.Write("Enter Product Name:");
            string _prd = Console.ReadLine();

            Console.Write("Enter Quantity:");
            string _qty = Console.ReadLine();

            Console.Write("Enter Rate:");
            string _Rate = Console.ReadLine();


            SqlCommand _cmd = new SqlCommand("INSERT INTO [Product]  ([ProductName] ,[Qty],[Rate]) VALUES (@prd,@Qty,@rate)", _cn);

            _cmd.Parameters.Add("@prd", SqlDbType.VarChar, 50).Value = _prd;
            _cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = _qty;
            _cmd.Parameters.Add("@rate", SqlDbType.Money).Value = _Rate;
            _cn.Open();
            if (_cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Inserted Successfully....");
            }
            else
                Console.WriteLine("OOPS Try Again....");
            _cn.Close();

            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }

        private static void ListProductsUsingConComRdr(SqlConnection _cn)
        {
            Console.Clear();

            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _cn;
            _cmd.CommandText = "select * from Product"; ;
            _cmd.CommandType = CommandType.Text;


            _cn.Open();
            SqlDataReader _drdr = _cmd.ExecuteReader();

            if (_drdr.HasRows)
            {

                Console.WriteLine("ProductID".PadRight(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadRight(10, ' ') + "Rate");
                Console.WriteLine("_".PadRight(60, '_'));


                while (_drdr.Read())
                {
                    Console.WriteLine(
                        $"{_drdr.GetValue(0).ToString().PadRight(10, ' ')}{_drdr.GetValue(1).ToString().PadRight(30, ' ')}{_drdr.GetValue(2).ToString().PadRight(10, ' ')}{_drdr.GetValue(3).ToString()}");
                }


                _drdr.Close();
                _cn.Close();


            }
            else
                Console.WriteLine("No Products to Display...");


            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }
    }
}
