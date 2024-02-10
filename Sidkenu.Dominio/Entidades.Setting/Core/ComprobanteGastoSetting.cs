using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ComprobanteGastoSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ComprobanteGasto>
    {
        public void Configure(EntityTypeBuilder<ComprobanteGasto> builder)
        {
            // Propiedades
            builder.Property(x => x.TipoGastoId)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.TipoGasto)
                .WithMany(x => x.ComprobanteGastos)
                .HasForeignKey(x => x.TipoGastoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
