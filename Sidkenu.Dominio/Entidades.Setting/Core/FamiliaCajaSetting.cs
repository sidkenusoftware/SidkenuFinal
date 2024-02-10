using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class FamiliaCajaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<FamiliaCaja>
    {
        public void Configure(EntityTypeBuilder<FamiliaCaja> builder)
        {
            // Propiedades
            builder.Property(x => x.FamiliaId)
                .IsRequired();

            builder.Property(x => x.CajaId)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Familia)
                .WithMany(x => x.FamiliaCajas)
                .HasForeignKey(x => x.FamiliaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Caja)
                .WithMany(x => x.FamiliaCajas)
                .HasForeignKey(x => x.CajaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}