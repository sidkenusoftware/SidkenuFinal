using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MedioPagoTarjetaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<MedioPagoTarjeta>
    {
        public void Configure(EntityTypeBuilder<MedioPagoTarjeta> builder)
        {
            // Propiedades

            builder.Property(x => x.PlanTarjetaId)
                .IsRequired();

            builder.Property(x => x.NumeroCupon)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.PlanTarjeta)
                .WithMany(x => x.MedioPagos)
                .HasForeignKey(x => x.PlanTarjetaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
