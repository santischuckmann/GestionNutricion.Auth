using GestionNutricionAuth.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GestionNutricionAuth.Api
{
    public class GestionNutricionAuthContext: DbContext
    {
        public GestionNutricionAuthContext() { }
        public GestionNutricionAuthContext(DbContextOptions<GestionNutricionAuthContext> options): base(options) { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=GestionNutricion;Integrated Security=true;Trust Server Certificate=true;");
        }
    }
}
