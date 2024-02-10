using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ContadorSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Contador>
    {
        public void Configure(EntityTypeBuilder<Contador> builder)
        {
            // Propiedades
            builder.Property(x => x.EmpresaId)
                .IsRequired();

            builder.Property(x => x.TipoContador)
                .IsRequired();

            builder.Property(x => x.Numero)
                .IsRequired();
        }
    }
}