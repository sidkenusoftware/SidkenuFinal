using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MedioPagoChequeSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<MedioPagoCheque>
    {
        public void Configure(EntityTypeBuilder<MedioPagoCheque> builder)
        {
            // Propiedades
            builder.Property(x => x.BancoId)
                .IsRequired();

            builder.Property(x => x.NumeroCheque)
                .IsRequired();

            builder.Property(x => x.FechaVencimiento)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Banco)
                .WithMany(x => x.Cheques)
                .HasForeignKey(x => x.BancoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
