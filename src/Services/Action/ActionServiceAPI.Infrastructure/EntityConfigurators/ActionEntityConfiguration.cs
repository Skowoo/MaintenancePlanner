using ActionServiceAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActionServiceAPI.Infrastructure.EntityConfigurators
{
    class ActionEntityConfiguration : IEntityTypeConfiguration<ActionEntity>
    {
        public void Configure(EntityTypeBuilder<ActionEntity> builder)
        {
            builder.HasOne(e => e.CreatedBy)
                .WithOne()
                .HasForeignKey<ActionEntity>(e => e.CreatedById);

            builder.HasOne(e => e.ConductedBy)
                .WithOne()
                .HasForeignKey<ActionEntity>(e => e.ConductedById);
        }
    }
}
