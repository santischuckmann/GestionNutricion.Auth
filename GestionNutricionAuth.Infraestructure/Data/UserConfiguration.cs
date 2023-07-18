﻿using GestionNutricionAuth.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionNutricionAuth.Infraestructure.Data
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasIndex(u => u.Id);

            builder.Property(u => u.Username).IsRequired();

            builder.Property(u => u.FullName).IsRequired();

            builder.Property(u => u.FullName).IsRequired();

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}