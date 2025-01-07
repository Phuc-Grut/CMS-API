using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VFi.Domain.CMS.Models;

namespace VFi.Infra.CMS.Mappings.Configurations;

public partial class ItemProductConfiguration : IEntityTypeConfiguration<ItemProduct>
{
    public void Configure(EntityTypeBuilder<ItemProduct> entity)
    {
        entity.ToTable("ItemProduct", "dam");

        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(250);


        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<ItemProduct> entity);
}
