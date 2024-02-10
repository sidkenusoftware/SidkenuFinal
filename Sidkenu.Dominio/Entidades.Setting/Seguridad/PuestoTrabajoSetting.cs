using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class PuestoTrabajoSetting : EntidadBaseSetting, 
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<PuestoTrabajo>
    {
        public void Configure(EntityTypeBuilder<PuestoTrabajo> builder)
        {
            // Propiedades
            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.SerialNumber)
                .HasMaxLength(500)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.Puestos)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Cajas)
                .WithOne(x => x.PuestoTrabajo)
                .HasForeignKey(x => x.PuestoTrabajoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

