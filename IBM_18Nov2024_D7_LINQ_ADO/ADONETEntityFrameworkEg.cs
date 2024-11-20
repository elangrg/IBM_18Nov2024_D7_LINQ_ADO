using IBM_18Nov2024_D7_LINQ_ADO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBM_18Nov2024_D7_LINQ_ADO
{
    internal class ADONETEntityFrameworkEg
    {

        static void Main()
        {
            Models.IBM08Nov2024DbEntities _db = new Models.IBM08Nov2024DbEntities();


            //_cn.Open();


            int choice = -1;

            do
            {

                Console.Clear();


                Console.WriteLine("1. List Product(Connection & Command with Reader)\n2. Add New Product\n3. Update Product By ID \n4. Delete Product By ID \n5. Invoke Stored Proc   \n0. Exit ");

                Console.Write("Enter Choice :");
                choice = int.Parse(Console.ReadLine());


                if (choice == 1)
                {
                    ListProductsUsingConComRdr(_db);

                }

                if (choice == 2)
                {
                   AddNewProduct(_db);

                }

                if (choice == 3)
                {
                  //  UpdateProduct(_cn);

                }


                if (choice == 4)
                {
                    //DeleteProduct(_cn);

                }


                if (choice == 5)
                {
                    //StoredProcEg(_cn);

                }




            }
            while (choice != 0);





        }

        private static void AddNewProduct(IBM08Nov2024DbEntities _db)
        {
            Models.Product product = new Models.Product();
            Console.Clear();
            Console.WriteLine("Add New Product ");

            Console.Write("Enter Product Name:");
            product.ProductName = Console.ReadLine();

            Console.Write("Enter Quantity:");
            product.Qty = int.Parse( Console.ReadLine());

            Console.Write("Enter Rate:");
            product.Rate = int.Parse(Console.ReadLine());


            _db.Products.Add(product);
            _db.SaveChanges();


           
          
                Console.WriteLine("Inserted Successfully....");
        
           
            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }

        private static void ListProductsUsingConComRdr(IBM08Nov2024DbEntities db)
        {


            Console.WriteLine("Product ID".PadLeft(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadLeft(10, ' ') + "Rate".PadLeft(10, ' '));
            Console.WriteLine("_".PadRight(60, '_'));

            foreach (var item in db.Products) 
            {
                Console.WriteLine(
                        $"{item.ProductID.ToString().PadLeft(10, ' ')}{item.ProductName.PadRight(30, ' ')}{item.Qty.ToString().PadLeft(10, ' ')}{item.Rate.ToString().PadLeft(10, ' ')}");
            }


            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();

        }
    }
}
