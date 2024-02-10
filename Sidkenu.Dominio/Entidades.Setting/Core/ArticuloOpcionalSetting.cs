using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ArticuloOpcionalSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ArticuloOpcional>
    {
        public void Configure(EntityTypeBuilder<ArticuloOpcional> builder)
        {
            // Propiedades

            builder.Property(x => x.ArticuloPadreId)
                .IsRequired();

            builder.Property(x => x.ArticuloHijoId)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.ArticuloPadre)
                .WithMany(x => x.ArticuloPadreOpcionales)
                .HasForeignKey(x => x.ArticuloPadreId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.ArticuloHijo)
                .WithMany(x => x.ArticuloHijoOpcionales)
                .HasForeignKey(x => x.ArticuloHijoId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}