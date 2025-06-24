using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOTAS_APE.Models;
using NOTAS_APE.Services;

namespace NOTAS_APE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly EstudianteService _estudianteService;


        public EstudiantesController(EstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> Get()
        {
            var estudiantes = await _estudianteService.GetEstudiantesAsync();
            if (estudiantes == null || !estudiantes.Any())
            {
                return NotFound("No se encontraron estudiantes.");
            }
            return Ok(estudiantes);
        }


        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> Filtrar([FromQuery] string? cedula, [FromQuery] string? apellido)
        {
            if (string.IsNullOrEmpty(cedula) && string.IsNullOrEmpty(apellido))
            {
                return BadRequest("Debe enviar 'cedula' o 'apellido' como query param");
            }

            var estudiantes = await _estudianteService.FiltrarEstudiantesAsync(cedula, apellido);

            if (!estudiantes.Any())
            {
                return NotFound("No se encontraron estudiantes.");
            }

            return Ok(estudiantes);
        }

    }
}
