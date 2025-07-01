using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademicoFORMS.Data
{
    class DatabaseHelper
    {
        private readonly string connectionString =
        @"Data Source=DESKTOP-NNV2NIG\SQLEXPRESS;Initial Catalog=StudentsDB;Integrated Security=True;TrustServerCertificate=True";
        public DataTable GetMatriculas()
        {
            string query = @"
            SELECT 
                m.Id AS MatriculaId,
                e.Cedula,
                e.Nombre + ' ' + e.Apellido AS Estudiante,
                c.Nombre AS Curso,
                m.FechaMatricula,
                m.Nota1,
                m.Nota2,
                m.NotaSupletorio,
                COALESCE(m.NotaSupletorio, ((ISNULL(m.Nota1, 0) + ISNULL(m.Nota2, 0)) / 2)) AS NotaFinal,
                CASE 
                    WHEN COALESCE(m.NotaSupletorio, ((ISNULL(m.Nota1, 0) + ISNULL(m.Nota2, 0)) / 2)) >= 7 THEN 'Aprobado'
                    ELSE 'Reprobado'
                END AS Estado
            FROM Matriculas m
            JOIN Estudiantes e ON m.EstudianteId = e.Id
            JOIN Cursos c ON m.CursoId = c.Id";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
