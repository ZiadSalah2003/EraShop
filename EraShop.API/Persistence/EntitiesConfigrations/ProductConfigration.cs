﻿using EraShop.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EraShop.API.Persistence.EntitiesConfigrations
{
	public class ProductConfigration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(p => p.Description)
				.IsRequired();

			builder.Property(p => p.Price)
				.HasColumnType("decimal(18, 2)");

			builder.Property(p => p.Quantity)
				.IsRequired();

			builder.HasOne(b => b.Brand)
				.WithMany(b => b.Products)
				.HasForeignKey(p => p.BrandId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(b => b.Category)
				.WithMany(c => c.Products)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}
