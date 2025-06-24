using BancoAPI.Models;
using BancoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClientesController(IConfiguration config)
        {
            _service = new ClienteService(config);
        }

        [HttpGet]
        public IActionResult GetClientes()
        {
            var clientes = _service.ObtenerClientesConCuentas();
            return Ok(clientes);
        }

        [HttpGet("{cedula}")]
        public IActionResult GetCliente(string cedula)
        {
            var cliente = _service.ObtenerClientePorCedula(cedula);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(cliente);
        }

        [HttpPut("{cedula}")]
        public IActionResult ActualizarCliente(string cedula, [FromBody] Cliente cliente)
        {
            if (cedula != cliente.Cedula)
                return BadRequest(new { mensaje = "Cédula no coincide." });

            var actualizado = _service.ActualizarCliente(cliente);
            if (actualizado)
                return Ok(new { mensaje = "Cliente actualizado correctamente." });

            return NotFound(new { mensaje = "Cliente no encontrado." });
        }

        [HttpDelete("{cedula}")]
        public IActionResult EliminarCliente(string cedula)
        {
            var eliminado = _service.EliminarCliente(cedula);
            if (eliminado)
                return Ok(new { mensaje = "Cliente eliminado correctamente." });

            return NotFound(new { mensaje = "Cliente no encontrado o ya eliminado." });
        }

    }
}
