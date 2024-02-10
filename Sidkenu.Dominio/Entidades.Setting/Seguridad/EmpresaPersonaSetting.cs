using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Entidades.Setting.Base;

namespace Sidkenu.Dominio.Entidades.Setting.Seguridad
{
    public class EmpresaPersonaSetting : EntidadBaseSetting, Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<EmpresaPersona>
    {
        public void Configure(EntityTypeBuilder<EmpresaPersona> builder)
        {
            // Propiedades
            builder.Property(x => x.PersonaId)
                .IsRequired();

            builder.Property(x => x.EmpresaId)
                .IsRequired();

            // Propiedades de Navegacion
            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.EmpresasPersonas)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Persona)
                .WithMany(x => x.EmpresaPersonas)
                .HasForeignKey(x => x.PersonaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
