using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaAcademicoFORMS.Data;
using SistemaAcademicoFORMS.Entities;

namespace SistemaAcademicoFORMS
{
    public partial class Form1: Form
    {
        public Form1()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var listaMatriculas = ObtenerDatosReporteDesdeGrid();

            if (listaMatriculas.Count == 0)
            {
                MessageBox.Show("No hay datos para generar el reporte.");
                return;
            }

            var pdfBytes = GenerarReporteMatriculas(listaMatriculas);

            var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteMatriculas.pdf");
            File.WriteAllBytes(ruta, pdfBytes);
            MessageBox.Show("PDF generado exitosamente en el escritorio.");

            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }

        private byte[] GenerarReporteMatriculas(List<MatriculaReporte> matriculas)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape()); 
                    page.Margin(10);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.PageColor(Colors.White);

                    page.Header().Text("REPORTE DE MATRÍCULAS")
                        .Bold().FontSize(20).FontColor(Colors.Blue.Darken3).AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);   
                            columns.RelativeColumn(2);   
                            columns.RelativeColumn(3);   
                            columns.RelativeColumn(3);   
                            columns.RelativeColumn(2);   
                            columns.RelativeColumn(1);   
                            columns.RelativeColumn(1);   
                            columns.RelativeColumn(1);   
                            columns.RelativeColumn(1);   
                            columns.RelativeColumn(2);   
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("ID").Bold();
                            header.Cell().Text("Cédula").Bold();
                            header.Cell().Text("Nombre Completo").Bold();
                            header.Cell().Text("Curso").Bold();
                            header.Cell().Text("Fecha Matrícula").Bold();
                            header.Cell().Text("Nota 1").Bold();
                            header.Cell().Text("Nota 2").Bold();
                            header.Cell().Text("Nota Supletorio").Bold();
                            header.Cell().Text("Promedio").Bold();
                            header.Cell().Text("Estado").Bold();
                        });

                        foreach (var m in matriculas)
                        {
                            table.Cell().Text(m.Id.ToString());
                            table.Cell().Text(m.Cedula);
                            table.Cell().Text(m.NombreCompleto);
                            table.Cell().Text(m.Curso);
                            table.Cell().Text(m.FechaMatricula.ToString("yyyy-MM-dd"));
                            table.Cell().Text(m.Nota1);
                            table.Cell().Text(m.Nota2);
                            table.Cell().Text(m.NotaSupletorio);
                            table.Cell().Text(m.Promedio);
                            table.Cell().Text(m.Estado);
                        }
                    });

                    page.Footer().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            });

            return document.GeneratePdf();
        }



        private List<MatriculaReporte> ObtenerDatosReporteDesdeGrid()
        {
            var lista = new List<MatriculaReporte>();

            foreach (DataGridViewRow row in dataGridViewMatriculas.Rows)
            {
                if (row.IsNewRow) continue;

                lista.Add(new MatriculaReporte
                {
                    Id = Convert.ToInt32(row.Cells["MatriculaId"].Value),     
                    Cedula = row.Cells["Cedula"].Value?.ToString(),
                    NombreCompleto = row.Cells["Estudiante"].Value?.ToString(),  
                    Curso = row.Cells["Curso"].Value?.ToString(),
                    FechaMatricula = Convert.ToDateTime(row.Cells["FechaMatricula"].Value),
                    Nota1 = row.Cells["Nota1"].Value == DBNull.Value ? "" : row.Cells["Nota1"].Value.ToString(),
                    Nota2 = row.Cells["Nota2"].Value == DBNull.Value ? "" : row.Cells["Nota2"].Value.ToString(),
                    NotaSupletorio = row.Cells["NotaSupletorio"].Value == DBNull.Value ? "" : row.Cells["NotaSupletorio"].Value.ToString(),
                    Promedio = row.Cells["NotaFinal"].Value == DBNull.Value
                    ? ""
                    : $"{Convert.ToDecimal(row.Cells["NotaFinal"].Value):0.00}",

                    Estado = row.Cells["Estado"].Value?.ToString()
                });
            }

            return lista;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            try
            {
                DataTable dtMatriculas = dbHelper.GetMatriculas();
                dataGridViewMatriculas.DataSource = dtMatriculas;

                dataGridViewMatriculas.ReadOnly = true;
                dataGridViewMatriculas.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dataGridViewMatriculas.AllowUserToAddRows = false;
                dataGridViewMatriculas.AllowUserToDeleteRows = false;
                dataGridViewMatriculas.AllowUserToOrderColumns = false;
                dataGridViewMatriculas.AllowUserToResizeRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }
    }
}
