using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ArticuloPrecioSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ArticuloPrecio>
    {
        public void Configure(EntityTypeBuilder<ArticuloPrecio> builder)
        {
            // Propiedades

            builder.Property(x => x.ArticuloId)
                .IsRequired();

            builder.Property(x => x.ListaPrecioId)
                .IsRequired();

            builder.Property(x => x.FechaActualizacion)
                .IsRequired();

            builder.Property(x => x.Monto).HasPrecision(18, 6)
                .IsRequired();


            // Propiedades de Navegacion
            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.ArticuloPrecios)
                .HasForeignKey(x => x.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ListaPrecio)
                .WithMany(x => x.Precios)
                .HasForeignKey(x => x.ListaPrecioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}