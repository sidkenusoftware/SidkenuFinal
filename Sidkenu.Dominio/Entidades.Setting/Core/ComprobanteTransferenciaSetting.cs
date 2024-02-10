using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ComprobanteTransferenciaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ComprobanteTransferencia>
    {
        public void Configure(EntityTypeBuilder<ComprobanteTransferencia> builder)
        {
            // Propiedades
            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
