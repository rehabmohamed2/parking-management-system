using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationServices.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationServices.DataBase.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Email); 

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("root");
            builder.HasData(new User
            {
                Email = "admin@gmail.com",
                PasswordHash = passwordHash
            });
        }
    }
}