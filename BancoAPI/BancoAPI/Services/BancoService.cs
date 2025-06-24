using BancoAPI.Models;
using Microsoft.Data.SqlClient;

namespace BancoAPI.Services
{
    public class BancoService
    {
        private readonly string _cadena;

        public BancoService(IConfiguration config)
        {
            _cadena = config.GetConnectionString("BancoDB");
        }

        public bool CuentaExiste(string numeroCuenta)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM CUENTAS WHERE NUM_CUE = @num", con);
            cmd.Parameters.AddWithValue("@num", numeroCuenta);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public decimal ObtenerSaldo(string numeroCuenta)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();
            using var cmd = new SqlCommand("SELECT SAL_CUE FROM CUENTAS WHERE NUM_CUE = @num", con);
            cmd.Parameters.AddWithValue("@num", numeroCuenta);
            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public bool Transferir(Transferencia t)
        {
            using var con = new SqlConnection(_cadena);
            con.Open();
            using var tran = con.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                var saldoCmd = new SqlCommand("SELECT SAL_CUE FROM CUENTAS WHERE NUM_CUE = @ori", con, tran);
                saldoCmd.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                object result = saldoCmd.ExecuteScalar();

                if (result == null)
                    throw new Exception("Cuenta origen no existe.");

                decimal saldo = Convert.ToDecimal(result);
                if (saldo < t.Valor)
                    throw new Exception($"Saldo insuficiente. Disponible: {saldo}");

                var debito = new SqlCommand("UPDATE CUENTAS SET SAL_CUE = SAL_CUE - @val WHERE NUM_CUE = @ori", con, tran);
                debito.Parameters.AddWithValue("@val", t.Valor);
                debito.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                debito.ExecuteNonQuery();

                var cuentaDestinoCmd = new SqlCommand("SELECT COUNT(*) FROM CUENTAS WHERE NUM_CUE = @des", con, tran);
                cuentaDestinoCmd.Parameters.AddWithValue("@des", t.CuentaDestino);
                int destinoExiste = (int)cuentaDestinoCmd.ExecuteScalar();
                if (destinoExiste == 0)
                    throw new Exception("Cuenta destino no existe.");

                var credito = new SqlCommand("UPDATE CUENTAS SET SAL_CUE = SAL_CUE + @val WHERE NUM_CUE = @des", con, tran);
                credito.Parameters.AddWithValue("@val", t.Valor);
                credito.Parameters.AddWithValue("@des", t.CuentaDestino);
                credito.ExecuteNonQuery();

                var insert = new SqlCommand(@"INSERT INTO TRANSFERENCIAS (FEC_TRA, VALOR_TRA, NUM_CUE_ORI, NUM_CUE_DES)
                                             VALUES (@fec, @val, @ori, @des)", con, tran);
                insert.Parameters.AddWithValue("@fec", t.Fecha);
                insert.Parameters.AddWithValue("@val", t.Valor);
                insert.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                insert.Parameters.AddWithValue("@des", t.CuentaDestino);
                insert.ExecuteNonQuery();

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
