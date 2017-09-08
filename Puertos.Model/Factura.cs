using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puertos.Model
{
    public class Factura
    {
        public string Numero_Factura { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Nit_Tercero { get; set; }

        public string Razon_Social_Tercero { get; set; }

        public string Cuenta_Contable { get; set; }

        public string Descripcion { get; set; }

        public decimal? Debito { get; set; }

        public decimal? Credito { get; set; }


        public Factura(string Numero_Factura, DateTime Fecha, decimal Nit_Tercero, string Razon_Social_Tercero, string Cuenta_Contable, string Descripcion, decimal? Debito, decimal? Credito)
        {
            this.Numero_Factura = Numero_Factura;
            this.Fecha = Fecha;
            this.Nit_Tercero = Nit_Tercero;
            this.Razon_Social_Tercero = Razon_Social_Tercero;
            this.Cuenta_Contable = Cuenta_Contable;
            this.Descripcion = Descripcion;
            this.Debito = Debito;
            this.Credito = Credito;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", this.Numero_Factura, this.Fecha.ToString("dd-MM-yyyy"), this.Nit_Tercero, this.Razon_Social_Tercero, this.Cuenta_Contable, this.Descripcion, this.Debito, this.Credito);
        }
    }
}
