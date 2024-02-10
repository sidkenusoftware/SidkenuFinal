using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class IngresoBrutoSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<IngresoBruto>
    {
        public void Configure(EntityTypeBuilder<IngresoBruto> builder)
        {
            // Propiedades

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasMany(x => x.Empresas)
                .WithOne(x => x.IngresoBruto)
                .HasForeignKey(x => x.IngresoBrutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
