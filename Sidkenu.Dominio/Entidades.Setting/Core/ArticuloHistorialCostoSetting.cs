using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ArticuloHistorialCostoSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ArticuloHistorialCosto>
    {
        public void Configure(EntityTypeBuilder<ArticuloHistorialCosto> builder)
        {
            // Propiedades

            builder.Property(x => x.ArticuloId)
                .IsRequired();

            builder.Property(x => x.PrecioCostoNuevo)
                .IsRequired();

            builder.Property(x => x.PrecioCostoAnterior)
                .IsRequired();

            builder.Property(x => x.FechaActualizacion)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.ArticuloHistorialCostos)
                .HasForeignKey(x => x.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}