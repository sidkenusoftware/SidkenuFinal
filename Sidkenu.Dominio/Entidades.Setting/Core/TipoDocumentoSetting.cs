using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class TipoDocumentoSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<TipoDocumento>
    {
        public void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            // Propiedades

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}