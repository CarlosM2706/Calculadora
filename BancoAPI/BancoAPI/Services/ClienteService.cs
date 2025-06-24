using BancoAPI.Models;
using Microsoft.Data.SqlClient;

namespace BancoAPI.Services
{
    public class ClienteService
    {
        private readonly string _cadena;

        public ClienteService(IConfiguration config)
        {
            _cadena = config.GetConnectionString("BancoDB");
        }

        public List<Cliente> ObtenerClientesConCuentas()
        {
            var clientes = new Dictionary<string, Cliente>();

            using var con = new SqlConnection(_cadena);
            con.Open();
            string query = @"
                SELECT c.CED_CLI, c.NOM_CLI, c.APE_CLI,
                       cu.NUM_CUE, cu.TIP_CUE, cu.SAL_CUE
                FROM CLIENTES c
                LEFT JOIN CUENTAS cu ON c.CED_CLI = cu.CED_CLI_PER";

            using var cmd = new SqlCommand(query, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string cedula = reader["CED_CLI"].ToString();

                if (!clientes.ContainsKey(cedula))
                {
                    clientes[cedula] = new Cliente
                    {
                        Cedula = cedula,
                        Nombre = reader["NOM_CLI"].ToString(),
                        Apellido = reader["APE_CLI"].ToString()
                    };
                }

                if (reader["NUM_CUE"] != DBNull.Value)
                {
                    var cuenta = new Cuenta
                    {
                        Numero = reader["NUM_CUE"].ToString(),
                        Tipo = reader["TIP_CUE"].ToString(),
                        Saldo = Convert.ToDecimal(reader["SAL_CUE"])
                    };
                    clientes[cedula].Cuentas.Add(cuenta);
                }
            }

            return clientes.Values.ToList();
        }

        public Cliente ObtenerClientePorCedula(string cedula)
        {
            return ObtenerClientesConCuentas().FirstOrDefault(c => c.Cedula == cedula);
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();
            string sql = @"
        UPDATE CLIENTES
        SET NOM_CLI = @nom, APE_CLI = @ape
        WHERE CED_CLI = @ced";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@ced", cliente.Cedula);
            cmd.Parameters.AddWithValue("@nom", cliente.Nombre);
            cmd.Parameters.AddWithValue("@ape", cliente.Apellido);

            int filas = cmd.ExecuteNonQuery();
            return filas > 0;
        }

        public bool EliminarCliente(string cedula)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();

            var deleteCuentas = new SqlCommand("DELETE FROM CUENTAS WHERE CED_CLI_PER = @ced", con);
            deleteCuentas.Parameters.AddWithValue("@ced", cedula);
            deleteCuentas.ExecuteNonQuery();

            var deleteCliente = new SqlCommand("DELETE FROM CLIENTES WHERE CED_CLI = @ced", con);
            deleteCliente.Parameters.AddWithValue("@ced", cedula);
            int filas = deleteCliente.ExecuteNonQuery();

            return filas > 0;
        }



    }
}
