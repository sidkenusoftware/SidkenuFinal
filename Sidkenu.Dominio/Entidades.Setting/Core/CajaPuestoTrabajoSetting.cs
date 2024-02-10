using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class CajaPuestoTrabajoSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<CajaPuestoTrabajo>
    {
        public void Configure(EntityTypeBuilder<CajaPuestoTrabajo> builder)
        {
            // Propiedades

            builder.Property(x => x.CajaId)
                .IsRequired();

            builder.Property(x => x.PuestoTrabajoId)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Caja)
                .WithMany(x => x.Puestos)
                .HasForeignKey(x => x.CajaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PuestoTrabajo)
                .WithMany(x => x.Cajas)
                .HasForeignKey(x => x.PuestoTrabajoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}