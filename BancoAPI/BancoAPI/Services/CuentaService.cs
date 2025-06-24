using BancoAPI.Models;
using Microsoft.Data.SqlClient;

namespace BancoAPI.Services
{
    public class CuentaService
    {
        private readonly string _cadena;

        public CuentaService(IConfiguration config)
        {
            _cadena = config.GetConnectionString("BancoDB");
        }

        public List<Cuenta> ObtenerCuentas()
        {
            var cuentas = new List<Cuenta>();
            using var con = new SqlConnection(_cadena);
            con.Open();

            string sql = "SELECT NUM_CUE, TIP_CUE, SAL_CUE, CED_CLI_PER FROM CUENTAS";
            using var cmd = new SqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cuentas.Add(new Cuenta
                {
                    Numero = reader["NUM_CUE"].ToString(),
                    Tipo = reader["TIP_CUE"].ToString(),
                    Saldo = Convert.ToDecimal(reader["SAL_CUE"]),
                    CedulaCliente = reader["CED_CLI_PER"].ToString()
                });
            }

            return cuentas;
        }

        public Cuenta ObtenerCuenta(string numero)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();

            string sql = "SELECT * FROM CUENTAS WHERE NUM_CUE = @num";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@num", numero);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Cuenta
                {
                    Numero = reader["NUM_CUE"].ToString(),
                    Tipo = reader["TIP_CUE"].ToString(),
                    Saldo = Convert.ToDecimal(reader["SAL_CUE"]),
                    CedulaCliente = reader["CED_CLI_PER"].ToString()
                };
            }

            return null;
        }

        public bool CrearCuenta(Cuenta cuenta)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();

            string sql = @"INSERT INTO CUENTAS (NUM_CUE, TIP_CUE, SAL_CUE, CED_CLI_PER)
                           VALUES (@num, @tipo, @saldo, @ced)";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@num", cuenta.Numero);
            cmd.Parameters.AddWithValue("@tipo", cuenta.Tipo);
            cmd.Parameters.AddWithValue("@saldo", cuenta.Saldo);
            cmd.Parameters.AddWithValue("@ced", cuenta.CedulaCliente);

            int filas = cmd.ExecuteNonQuery();
            return filas > 0;
        }

        public bool ActualizarCuenta(Cuenta cuenta)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();

            string sql = @"UPDATE CUENTAS 
                           SET TIP_CUE = @tipo, SAL_CUE = @saldo, CED_CLI_PER = @ced
                           WHERE NUM_CUE = @num";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@tipo", cuenta.Tipo);
            cmd.Parameters.AddWithValue("@saldo", cuenta.Saldo);
            cmd.Parameters.AddWithValue("@ced", cuenta.CedulaCliente);
            cmd.Parameters.AddWithValue("@num", cuenta.Numero);

            int filas = cmd.ExecuteNonQuery();
            return filas > 0;
        }

        public bool EliminarCuenta(string numero)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();

            string sql = "DELETE FROM CUENTAS WHERE NUM_CUE = @num";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@num", numero);

            int filas = cmd.ExecuteNonQuery();
            return filas > 0;
        }
    }
}

