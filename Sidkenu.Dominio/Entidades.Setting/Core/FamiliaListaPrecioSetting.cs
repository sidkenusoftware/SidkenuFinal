using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class FamiliaListaPrecioSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<FamiliaListaPrecio>
    {
        public void Configure(EntityTypeBuilder<FamiliaListaPrecio> builder)
        {
            // Propiedades
            builder.Property(x => x.FamiliaId)
                .IsRequired();

            builder.Property(x => x.ListaPrecioId)
                .IsRequired();

            builder.Property(x => x.TipoValor)
                .IsRequired();

            builder.Property(x => x.Valor).HasPrecision(18, 6)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Familia)
                .WithMany(x => x.FamiliaListaPrecios)
                .HasForeignKey(x => x.FamiliaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ListaPrecio)
                .WithMany(x => x.FamiliaListaPrecios)
                .HasForeignKey(x => x.ListaPrecioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.FamiliaListaPrecios)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}