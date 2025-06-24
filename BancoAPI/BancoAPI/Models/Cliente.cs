namespace BancoAPI.Models
{
    public class Cliente
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public List<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    }
}
