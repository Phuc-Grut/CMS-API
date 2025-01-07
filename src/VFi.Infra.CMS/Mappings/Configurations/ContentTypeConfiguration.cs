using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VFi.Domain.CMS.Models;

namespace VFi.Infra.CMS.Mappings.Configurations;

public partial class ContentTypeConfiguration : IEntityTypeConfiguration<ContentType>
{
    public void Configure(EntityTypeBuilder<ContentType> entity)
    {
        entity.ToTable("ContentType", "dam");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Code)
            .HasMaxLength(50)
            .IsUnicode(false);
        entity.Property(e => e.Name).HasMaxLength(255);
        entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        OnConfigurePartial(entity);
    }
    partial void OnConfigurePartial(EntityTypeBuilder<ContentType> entity);
}
