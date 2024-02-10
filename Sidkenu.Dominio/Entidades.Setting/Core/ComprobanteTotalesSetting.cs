using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class ComprobanteTotalesSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<ComprobanteTotales>
    {
        public void Configure(EntityTypeBuilder<ComprobanteTotales> builder)
        {
            // Propiedades
            builder.Property(x => x.ComprobanteId)
                .IsRequired();

            builder.Property(x => x.Neto)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Alicuota)
                .HasPrecision(18, 6)
                .IsRequired();

            builder.Property(x => x.Iva)
                .HasPrecision(18, 6)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Comprobante)
                .WithMany(x => x.Totales)
                .HasForeignKey(x => x.ComprobanteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}