using BancoAPI.Models;
using BancoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 

    public class BancoController : ControllerBase
    {
        private readonly BancoService _service;

        public BancoController(IConfiguration config)
        {
            _service = new BancoService(config);
        }

        [HttpGet("cuenta-existe/{numero}")]
        public IActionResult CuentaExiste(string numero)
        {
            bool existe = _service.CuentaExiste(numero);
            return Ok(new { existe });
        }

        [HttpGet("saldo/{numero}")]
        public IActionResult ObtenerSaldo(string numero)
        {
            decimal saldo = _service.ObtenerSaldo(numero);
            return Ok(new { saldo });
        }

        [HttpPost("transferir")]
        public IActionResult Transferir([FromBody] Transferencia t)
        {
            try
            {
                t.Fecha = DateTime.Now;
                bool result = _service.Transferir(t);
                return Ok(new { ok = result, mensaje = "Transferencia exitosa" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
