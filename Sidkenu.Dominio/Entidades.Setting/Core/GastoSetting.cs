using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class GastoSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Gasto>
    {
        public void Configure(EntityTypeBuilder<Gasto> builder)
        {
            // Propiedades

            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.TipoGastoId)
                .IsRequired();

            builder.Property(x => x.Fecha)
                .IsRequired();

            builder.Property(x => x.Monto).HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.Gastos)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TipoGasto)
                .WithMany(x => x.Gastos)
                .HasForeignKey(x => x.TipoGastoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}