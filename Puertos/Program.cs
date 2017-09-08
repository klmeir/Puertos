using Puertos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Puertos
{
    class Program
    {
        // private const String con_config = "Server=127.0.0.1;Database=Puertos;Trusted_Connection=True;";
        private const String con_config = "Server=ServerName\\SQLEXPRESS;Database=Puertos;User Id=sa;Password=MyPassword;";
        public static void RecorrerFactura(List<Factura> lista_fac)
        {
            foreach (Factura f in lista_fac)
            {
                Console.WriteLine(f.ToString());
            }
        }

        static void Main(string[] args)
        {
            string op = "";
            while (op != "5") {
                Console.Clear();
                Console.WriteLine("1. Información de Factura\n2. Mostrar Débitos y Créditos\n3. Facturas Descuadradas\n4. Total Debitos y Creditos de Cuenta\n5. Salir");
                op = Console.ReadLine();
                switch (op) { 
                    case "1":
                        InformacionFactura();
                        break;
                    case "2":
                        DebitoCreditoFactura();
                        break;
                    case "3":
                        DescuadreFactura();
                        break;       
                    case "4":
                        DCCuenta();
                        break;
                    case "5":                        
                        break;
                }
               
            }
                      
        
        }

        public static void InformacionFactura()
        {

            Console.WriteLine("Ingrese número de factura :");
            string num = Console.ReadLine();

            List<Factura> lista_fac = new List<Factura>();
            using (SqlConnection con = new SqlConnection())
            {

                con.ConnectionString = con_config;

                con.Open();

                SqlCommand cmd = new SqlCommand();

                String query = string.Format("SELECT Numero_Factura,Fecha,Nit_Tercero,Razon_Social_Tercero,Cuenta_Contable,Descripcion,Debito,Credito FROM Facturas  where Numero_Factura = '{0}'", num);

                cmd.CommandText = query;

                cmd.Connection = con;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string Numero_Factura = Convert.ToString(reader["Numero_Factura"]);
                    DateTime Fecha = Convert.ToDateTime(reader["Fecha"]);
                    decimal Nit_Tercero = Convert.ToDecimal(reader["Nit_Tercero"]);
                    string Razon_Social_Tercero = Convert.ToString(reader["Razon_Social_Tercero"]);
                    string Cuenta_Contable = Convert.ToString(reader["Cuenta_Contable"]);
                    string Descripcion = Convert.ToString(reader["Descripcion"]);
                    //decimal? Debito = Convert.ToDecimal(reader["Debito"]);
                    //decimal? Credito = Convert.ToDecimal(reader["Credito"]);
                    String d = Convert.ToString(reader["Debito"]);
                    String c = Convert.ToString(reader["Credito"]);

                    decimal? Debito = (String.IsNullOrEmpty(d) ? 0 : (decimal?)Convert.ToDecimal(d));
                    decimal? Credito = (String.IsNullOrEmpty(c) ? 0 : (decimal?)Convert.ToDecimal(c));


                    Factura e = new Factura(Numero_Factura, Fecha, Nit_Tercero, Razon_Social_Tercero, Cuenta_Contable, Descripcion, Debito, Credito);

                    lista_fac.Add(e);

                }

                RecorrerFactura(lista_fac);

                Console.Read();

            }
               
        }

        public static void DebitoCreditoFactura() {
            Console.WriteLine("Ingrese número de factura :");
            string num = Console.ReadLine();
            using (SqlConnection con = new SqlConnection())
            {

                con.ConnectionString = con_config;

                con.Open();

                SqlCommand cmd = new SqlCommand();

                String query = string.Format("SELECT Numero_Factura,SUM(Debito) AS Debito,SUM(Credito) AS Credito FROM dbo.Facturas where Numero_Factura = '{0}' group by Numero_Factura", num);

                cmd.CommandText = query;

                cmd.Connection = con;

                SqlDataReader reader = cmd.ExecuteReader();

                string Numero = "";
                decimal? Debito = null;
                decimal? Credito = null;

                while (reader.Read())
                {
                    //string Numero_Factura = Convert.ToString(reader["Numero_Factura"]);
                    Numero = Convert.ToString(reader["Numero_Factura"]);
                    String d = Convert.ToString(reader["Debito"]);
                    String c = Convert.ToString(reader["Credito"]);

                    Debito = (String.IsNullOrEmpty(d) ? 0 : (decimal?)Convert.ToDecimal(d));
                    Credito = (String.IsNullOrEmpty(c) ? 0 : (decimal?)Convert.ToDecimal(c)); 
                }

                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                Console.WriteLine("#Factura        Debito     Credito");
                Console.WriteLine("{0}        {1}    {2}", Numero, Debito, Credito);

                Console.Read();
            }
        }

        public static void DescuadreFactura()
        {
            //Console.WriteLine("Ingrese número de factura :");
            //string num = Console.ReadLine();
            using (SqlConnection con = new SqlConnection())
            {

                con.ConnectionString = con_config;

                con.Open();

                SqlCommand cmd = new SqlCommand();

                String query = string.Format("SELECT * FROM (SELECT Numero_Factura,SUM(Debito) AS Debito,SUM(Credito) AS Credito, (SUM(Debito) - SUM(Credito)) AS Descuadre FROM dbo.Facturas GROUP BY Numero_Factura) mytable WHERE Descuadre > 0");

                cmd.CommandText = query;

                cmd.Connection = con;

                SqlDataReader reader = cmd.ExecuteReader();

                decimal? Debito = null;
                decimal? Credito = null;
                decimal? Descuadre = null;

                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                Console.WriteLine("#Factura        Debito     Credito     Descuadre");
                while (reader.Read())
                {
                    string Numero_Factura = Convert.ToString(reader["Numero_Factura"]);
                    String d = Convert.ToString(reader["Debito"]);
                    String c = Convert.ToString(reader["Credito"]);
                    String ds = Convert.ToString(reader["Descuadre"]);

                    Debito = (String.IsNullOrEmpty(d) ? 0 : (decimal?)Convert.ToDecimal(d));
                    Credito = (String.IsNullOrEmpty(c) ? 0 : (decimal?)Convert.ToDecimal(c));
                    Descuadre = (String.IsNullOrEmpty(ds) ? 0 : (decimal?)Convert.ToDecimal(ds));

                    Console.WriteLine("{0}        {1}     {2}     {3}", Numero_Factura, Debito, Credito, Descuadre);
                }

                Console.Read();
            }
        }


        public static void DCCuenta()
        {
            Console.WriteLine("Ingrese código de la cuenta contable :");
            string cod = Console.ReadLine();
            using (SqlConnection con = new SqlConnection())
            {

                con.ConnectionString = con_config;

                con.Open();

                SqlCommand cmd = new SqlCommand();

                //String query = string.Format("SELECT * FROM (SELECT Cuenta_Contable,Descripcion,(isnull (SUM(Debito),0) - isnull(SUM(Credito),0)) AS Total FROM dbo.Facturas GROUP BY Cuenta_Contable,Descripcion) mytable WHERE Cuenta_Contable = '{0}'",cod);
                String query = string.Format("SELECT * FROM (SELECT Cuenta_Contable,Descripcion,isnull(SUM(Debito),SUM(Credito)) AS Total FROM dbo.Facturas GROUP BY Cuenta_Contable,Descripcion) mytable WHERE Cuenta_Contable = '{0}'", cod);

                cmd.CommandText = query;

                cmd.Connection = con;

                SqlDataReader reader = cmd.ExecuteReader();

                string Cuenta = null;
                string Descripcion = null;
                decimal? Total = null;

                Console.WriteLine("--------------------------------------------------------------------------------------------------");
                Console.WriteLine("#Cuenta        Descripción                  Total");
                while (reader.Read())
                {
                   
                    String t = Convert.ToString(reader["Total"]);


                    Cuenta = Convert.ToString(reader["Cuenta_Contable"]);
                    Descripcion = Convert.ToString(reader["Descripcion"]);
                    Total = (String.IsNullOrEmpty(t) ? 0 : (decimal?)Convert.ToDecimal(t));

                    Console.WriteLine("{0}       {1}          {2}", Cuenta, Descripcion, Total);
                }

                Console.Read();

            }
        }
    }
}
