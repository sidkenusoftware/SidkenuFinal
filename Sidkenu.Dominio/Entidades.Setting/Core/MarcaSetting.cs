using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MarcaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Marca>
    {
        public void Configure(EntityTypeBuilder<Marca> builder)
        {
            // Propiedades

            builder.Property(x => x.EmpresaId)
                .IsRequired(false);

            builder.Property(x => x.Codigo)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            // ---------------------------------------------------------------- //

            builder.Property(x => x.ActivarAumentoPrecioPublico)
                .IsRequired();

            builder.Property(x => x.AumentoPrecioPublico)
                .IsRequired(false);

            builder.Property(x => x.TipoValorPublico)
                .IsRequired(false);

            // ---------------------------------------------------------------- //

            builder.Property(x => x.ActivarAumentoPrecioPublicoListaPrecio)
                .IsRequired();

            builder.Property(x => x.AumentoPrecioPublicoListaPrecio)
                .IsRequired(false);

            builder.Property(x => x.TipoValorPublicoListaPrecio)
                .IsRequired(false);

            // Propiedades de Navegacion
            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.Marcas)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}