using IntegrationServices.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationServices.DataBase.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.InvoiceId);
            builder.Property(i => i.InvoiceHTMLDocumentPath).IsRequired();
            builder.Property(i => i.TicketSerial).IsRequired().HasMaxLength(9);
            builder.Property(i => i.FromDate).IsRequired();
            builder.Property(i => i.ToDate).IsRequired();
            builder.Property(i => i.NumberofHours).IsRequired();
            builder.Property(i => i.PlateNumber).IsRequired().HasMaxLength(20);
            builder.Property(i => i.TotalAmountWithOutTax).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(i => i.TaxAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(i => i.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}