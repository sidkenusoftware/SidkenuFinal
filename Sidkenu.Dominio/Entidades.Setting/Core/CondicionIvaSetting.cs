using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class CondicionIvaSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<CondicionIva>
    {
        public void Configure(EntityTypeBuilder<CondicionIva> builder)
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

            builder.Property(x => x.Valor).HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.AplicaParaFacturaElectronica)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasMany(x => x.Articulos)
                .WithOne(x => x.CondicionIva)
                .HasForeignKey(x => x.CondicionIvaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.CondicionIvas)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}