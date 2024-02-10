using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ArticuloBajaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ArticuloBaja>
    {
        public void Configure(EntityTypeBuilder<ArticuloBaja> builder)
        {
            // Propiedades

            builder.Property(x => x.ArticuloId)
                .IsRequired();

            builder.Property(x => x.MotivoBajaId)
                .IsRequired();

            builder.Property(x => x.Fecha)
                .IsRequired();

            builder.Property(x => x.Cantidad).HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Observacion)
                .HasMaxLength(400)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.MotivoBaja)
                .WithMany(x => x.ArticuloBajas)
                .HasForeignKey(x => x.MotivoBajaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.ArticuloBajas)
                .HasForeignKey(x => x.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
