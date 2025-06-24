using NOTAS_APE.Models;
using NOTAS_APE.Repositories;

namespace NOTAS_APE.Services
{
    public class EstudianteService
    {
        private readonly IEstudianteRepository _repository;

        public EstudianteService(IEstudianteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Estudiante>> GetEstudiantesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Estudiante>> FiltrarEstudiantesAsync(string? cedula, string? apellido)
        {
            return await _repository.FiltrarAsync(cedula, apellido);
        }
    }
}
