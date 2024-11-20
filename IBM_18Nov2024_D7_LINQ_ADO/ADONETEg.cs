using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

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

                if (choice==3)
                {
                    UpdateProduct(_cn);

                }


                if (choice == 4)
                {
                    DeleteProduct(_cn);

                }


                if (choice == 5)
                {
                    StoredProcEg(_cn);

                }



                if (choice == 6)
                {
                    DatasetCRUDOper(_cn);

                }

            }
            while (choice!=0);





        }

        private static void DatasetCRUDOper(SqlConnection _cn)
        {
            DataSet _ds = new DataSet();

            SqlDataAdapter _da = new SqlDataAdapter();

            _da.SelectCommand = new SqlCommand("select * from Product", _cn);
            // Query Data 
            _da.Fill(_ds);

            foreach (DataRow rw in _ds.Tables[0].Rows)
            {
                for (int c = 0; c < _ds.Tables[0].Columns.Count; c++)
                {
                    Console.Write($"{rw[c]}|");
                }
                Console.WriteLine();

            }

            DataTable _dt = _ds.Tables[0];

            DataRow _r = _dt.NewRow();
            _r[1] = "Tata Nexus "; _r["Qty"] = 1000;  _r["Rate"] = 10000;

            Console.WriteLine(  _r.RowState);

            _dt.Rows.Add(_r);
            Console.WriteLine(_r.RowState);

            _r = _dt.Select("productId=2")[0];

            Console.WriteLine(_r.RowState);


            _r[1] = "Tata HEXA";
            Console.WriteLine(_r.RowState);


            _r = _dt.Select("productId=5")[0];
            Console.WriteLine(_r.RowState);
            _r.Delete();
            Console.WriteLine(_r.RowState);


            // - DataAdaptor Config

            _da.InsertCommand = new SqlCommand("INSERT INTO [Product]  ([ProductName] ,[Qty],[Rate]) VALUES (@prd,@Qty,@rate)", _cn);

            _da.InsertCommand.Parameters.Add("@prd", SqlDbType.VarChar, 50).SourceColumn = "ProductName";
            _da.InsertCommand.Parameters.Add("@Qty", SqlDbType.Int).SourceColumn = "Qty";
            _da.InsertCommand.Parameters.Add("@rate", SqlDbType.Money).SourceColumn = "Rate";


            _da.UpdateCommand = new SqlCommand("UPDATE [Product]  SET [ProductName] = @prd  ,[Qty] = @Qty,[Rate] = @rate WHERE ProductID = @prdId", _cn);

            _da.UpdateCommand.Parameters.Add("@prd", SqlDbType.VarChar, 50).SourceColumn = "ProductName";
            _da.UpdateCommand.Parameters.Add("@Qty", SqlDbType.Int).SourceColumn = "Qty";
            _da.UpdateCommand.Parameters.Add("@rate", SqlDbType.Money).SourceColumn = "Rate";
            _da.UpdateCommand.Parameters.Add("@prdId", SqlDbType.Int).SourceColumn = "ProductID";


            _da.DeleteCommand = new SqlCommand("delete from [Product]  WHERE ProductID = @prdId", _cn);
            _da.DeleteCommand.Parameters.Add("@prdId", SqlDbType.Int).SourceColumn = "ProductID";


            _da.ContinueUpdateOnError = true;

            _da.Update(_dt);


            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();

        }

        private static void StoredProcEg(SqlConnection _cn)
        {   Console.Clear();
            Console.WriteLine("Call GetAllProducts Stored Proc:");
           
            SqlCommand _cmd = new SqlCommand("GetAllProducts", _cn);
            _cmd.CommandType = CommandType.StoredProcedure;
            _cn.Open();
            SqlDataReader _drdr = _cmd.ExecuteReader();

            if (_drdr.HasRows)
            {

                Console.WriteLine("Product ID".PadLeft(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadLeft(10, ' ') + "Rate".PadLeft(10, ' '));
                Console.WriteLine("_".PadRight(60, '_'));


                while (_drdr.Read())
                {
                    Console.WriteLine(
                        $"{_drdr.GetValue(0).ToString().PadLeft(10, ' ')}{_drdr.GetValue(1).ToString().PadRight(30, ' ')}{_drdr.GetValue(2).ToString().PadLeft(10, ' ')}{_drdr.GetValue(3).ToString().PadLeft(10, ' ')}");

                }


                _drdr.Close();



              

            }
            _cn.Close();
             Console.WriteLine("Press Any key to continue... ");
            Console.ReadKey();

            Console.WriteLine("Call Stored Proc With Param");



            Console.Write("Enter Product ID:");

             _cmd = new SqlCommand("GetProductNameByID", _cn);
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.Parameters.Add("@prdId", SqlDbType.Int).Value = Console.ReadLine();
            _cmd.Parameters.Add("@prdName", SqlDbType.VarChar, 50).Direction= ParameterDirection.Output;

            _cn.Open();
            _cmd.ExecuteNonQuery();
              _cn.Close();


            if (_cmd.Parameters["@prdName"].Value!=null)
            {

                Console.WriteLine($"Product Name is : {_cmd.Parameters["@prdName"].Value}" );
            }
         



            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }

        private static void UpdateProduct(SqlConnection _cn)
        {
            string prdid= SeekProductByName(_cn);

            if (prdid=="-1") { return; }


            Console.Clear();
            Console.WriteLine("New Value to Update Product ");

            Console.Write("Enter Product Name:");
            string _prd = Console.ReadLine();

            
            Console.Write("Enter Quantity:");
            string _qty = Console.ReadLine();

            Console.Write("Enter Rate:");
            string _Rate = Console.ReadLine();


            SqlCommand _cmd = new SqlCommand("UPDATE [Product]  SET [ProductName] = @prd  ,[Qty] = @Qty,[Rate] = @rate WHERE ProductID = @prdId", _cn);

            _cmd.Parameters.Add("@prdId", SqlDbType.Int).Value = prdid;
            _cmd.Parameters.Add("@prd", SqlDbType.VarChar, 50).Value = _prd;
            _cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = _qty;
            _cmd.Parameters.Add("@rate", SqlDbType.Money).Value = _Rate;
            _cn.Open();
            if (_cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Updated Successfully....");
            }
            else
                Console.WriteLine("OOPS Try Again....");
            _cn.Close();

            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();

        }


        private static void DeleteProduct(SqlConnection _cn)
        {
            string prdid = SeekProductByName(_cn);

            if (prdid == "-1") { return; }


            
            Console.WriteLine("Are you sure? Delete (Y/N) :");

           
            string _rst = Console.ReadLine();


          if (_rst.ToUpper() != "Y") { return; }


            SqlCommand _cmd = new SqlCommand("delete from  [Product]   WHERE ProductID = @prdId", _cn);

            _cmd.Parameters.Add("@prdId", SqlDbType.Int).Value = prdid;
         
            _cn.Open();
            if (_cmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("Deleted Successfully....");
            }
            else
                Console.WriteLine("OOPS Try Again....");
            _cn.Close();

            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();

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
                        $"{_drdr.GetValue(0).ToString().PadRight(10, ' ')}{_drdr.GetValue(1).ToString().PadRight(30, ' ')}{_drdr.GetValue(2).ToString().PadLeft(10, ' ')}{_drdr.GetValue(3).ToString().PadLeft(10,' ')}");
                }


                _drdr.Close();
              


            }
            else
                Console.WriteLine("No Products to Display...");

            _cn.Close();

            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }
 private static string SeekProductByName(SqlConnection _cn)
        {
            Console.Clear();

            SqlCommand _cmd = new SqlCommand( "select * from Product where productname like @filterCtr",_cn);
            Console.Write(  "Enter Product Name Containing :");
            _cmd.Parameters.Add("@filterCtr", SqlDbType.VarChar ,50).Value= "%" +   Console.ReadLine() + "%";


            Dictionary<string, string> _PrdIDs= new Dictionary<string, string>();
            int _SeqID = 1;


            _cn.Open();
            SqlDataReader _drdr = _cmd.ExecuteReader();

            if (_drdr.HasRows)
            {

                Console.WriteLine("Seq No".PadLeft(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadLeft(10, ' ') + "Rate".PadLeft(10, ' '));
                Console.WriteLine("_".PadRight(60, '_'));


                while (_drdr.Read())
                {
                    _PrdIDs.Add(_SeqID.ToString(), _drdr.GetValue(0).ToString());

                    Console.WriteLine(
                        $"{_SeqID.ToString().PadLeft(10, ' ')}{_drdr.GetValue(1).ToString().PadRight(30, ' ')}{_drdr.GetValue(2).ToString().PadLeft(10, ' ')}{_drdr.GetValue(3).ToString().PadLeft(10,' ')}");
                    _SeqID++;
                }


                _drdr.Close();
              


                _cn.Close();



                if (_PrdIDs.Count>1)
                {
                    Console.WriteLine( "Enter Product Seq ID to Update/Delete:");
                   return  _PrdIDs[Console.ReadLine()];

                }
                else
                    return _PrdIDs["1"];


            }
            else
            {
                 Console.WriteLine($"No Records found for '{_cmd.Parameters[0].Value}'");
                 Console.WriteLine("Press Any key to continue...");_cn.Close();
                            Console.ReadKey();
                return "-1";
            }
               

                        
        }
    }
}
