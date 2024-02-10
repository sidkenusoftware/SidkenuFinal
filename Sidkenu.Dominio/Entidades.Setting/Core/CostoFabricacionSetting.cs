using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class CostoFabricacionSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<CostoFabricacion>
    {
        public void Configure(EntityTypeBuilder<CostoFabricacion> builder)
        {
            // Propiedades

            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Monto)
                .HasPrecision(18, 6)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.CostoFabricaciones)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}