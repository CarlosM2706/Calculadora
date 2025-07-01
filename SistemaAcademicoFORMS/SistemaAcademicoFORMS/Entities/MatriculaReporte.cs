using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademicoFORMS.Entities
{
    class MatriculaReporte
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string NombreCompleto { get; set; }
        public string Curso { get; set; }
        public DateTime FechaMatricula { get; set; }
        public string Nota1 { get; set; }
        public string Nota2 { get; set; }
        public string NotaSupletorio { get; set; }
        public string Promedio { get; set; }
        public string Estado { get; set; }
    }
}
