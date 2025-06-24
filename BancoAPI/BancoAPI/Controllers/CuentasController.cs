using BancoAPI.Models;
using BancoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : ControllerBase
    {
        private readonly CuentaService _service;

        public CuentasController(IConfiguration config)
        {
            _service = new CuentaService(config);
        }

        [HttpGet]
        public IActionResult GetCuentas()
        {
            var cuentas = _service.ObtenerCuentas();
            return Ok(cuentas);
        }

        [HttpGet("{numero}")]
        public IActionResult GetCuenta(string numero)
        {
            var cuenta = _service.ObtenerCuenta(numero);
            if (cuenta == null)
                return NotFound(new { mensaje = "Cuenta no encontrada." });

            return Ok(cuenta);
        }

        [HttpPost]
        public IActionResult CrearCuenta([FromBody] Cuenta cuenta)
        {
            bool creada = _service.CrearCuenta(cuenta);
            if (creada)
                return Ok(new { mensaje = "Cuenta creada correctamente." });

            return BadRequest(new { mensaje = "No se pudo crear la cuenta." });
        }

        [HttpPut("{numero}")]
        public IActionResult ActualizarCuenta(string numero, [FromBody] Cuenta cuenta)
        {
            if (numero != cuenta.Numero)
                return BadRequest(new { mensaje = "Número de cuenta no coincide." });

            bool actualizada = _service.ActualizarCuenta(cuenta);
            if (actualizada)
                return Ok(new { mensaje = "Cuenta actualizada correctamente." });

            return NotFound(new { mensaje = "Cuenta no encontrada." });
        }

        [HttpDelete("{numero}")]
        public IActionResult EliminarCuenta(string numero)
        {
            bool eliminada = _service.EliminarCuenta(numero);
            if (eliminada)
                return Ok(new { mensaje = "Cuenta eliminada correctamente." });

            return NotFound(new { mensaje = "Cuenta no encontrada." });
        }
    }
}
