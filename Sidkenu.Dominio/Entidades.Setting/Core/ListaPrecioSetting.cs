using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ListaPrecioSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ListaPrecio>
    {
        public void Configure(EntityTypeBuilder<ListaPrecio> builder)
        {
            // Propiedades
            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.Codigo)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.Rentabilidad).HasPrecision(18, 6)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.ListaPrecios)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Clientes)
                .WithOne(x => x.ListaPrecio)
                .HasForeignKey(x => x.ListaPrecioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MarcaListaPrecios)
                .WithOne(x => x.ListaPrecio)
                .HasForeignKey(x => x.ListaPrecioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}