using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NOTAS_APE.Models;

namespace NOTAS_APE.Configuration
{
    public class PromedioConfiguration : IEntityTypeConfiguration<Promedio>
    {
        public void Configure(EntityTypeBuilder<Promedio> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CedulaEst)
                .HasColumnName("cedula_est")
                .IsRequired();

            builder.Property(p => p.ValorPromedio)
                .HasColumnName("promedio")
                .IsRequired();

            builder.HasOne(p => p.Estudiante)
                .WithMany() 
                .HasForeignKey(p => p.CedulaEst)
                .HasConstraintName("FK_Promedios_Estudiantes")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
