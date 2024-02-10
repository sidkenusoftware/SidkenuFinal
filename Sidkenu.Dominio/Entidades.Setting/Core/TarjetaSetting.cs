using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class TarjetaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Tarjeta>
    {
        public void Configure(EntityTypeBuilder<Tarjeta> builder)
        {
            // Propiedades
            builder.Property(x => x.Codigo)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasMany(x => x.PlanesTarjetas)
                .WithOne(x => x.Tarjeta)
                .HasForeignKey(x => x.TarjetaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}