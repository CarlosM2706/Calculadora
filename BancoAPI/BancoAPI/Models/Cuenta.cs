namespace BancoAPI.Models
{
    public class Cuenta
    {
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public decimal Saldo { get; set; }
        public string CedulaCliente { get; set; }
    }
}

