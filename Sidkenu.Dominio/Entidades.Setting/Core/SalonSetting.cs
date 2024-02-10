using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class SalonSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Salon>
    {
        public void Configure(EntityTypeBuilder<Salon> builder)
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

            // Propiedades de Navegacion
            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.Salones)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}