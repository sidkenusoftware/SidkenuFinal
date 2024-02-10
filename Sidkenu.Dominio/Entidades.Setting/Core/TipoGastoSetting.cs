using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class TipoGastoSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<TipoGasto>
    {
        public void Configure(EntityTypeBuilder<TipoGasto> builder)
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

            // Propiedades de Navegacion

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.TipoGastos)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ComprobanteGastos)
               .WithOne(x => x.TipoGasto)
               .HasForeignKey(x => x.TipoGastoId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}