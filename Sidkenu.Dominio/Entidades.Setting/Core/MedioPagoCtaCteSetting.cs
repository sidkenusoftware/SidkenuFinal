using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MedioPagoCtaCteSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<MedioPagoCtaCte>
    {
        public void Configure(EntityTypeBuilder<MedioPagoCtaCte> builder)
        {
            // Propiedades
            builder.Property(x => x.ClienteId)
                .IsRequired();


            // Propiedades de Navegacion

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.MedioPagos)
                .HasForeignKey(x => x.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
