using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class FormularioSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Formulario>
    {
        public void Configure(EntityTypeBuilder<Formulario> builder)
        {
            // Propiedades
            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.DescripcionCompleta)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.EstaVigente)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasMany(x => x.GruposFormularios)
                .WithOne(x => x.Formulario)
                .HasForeignKey(x => x.FormularioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
