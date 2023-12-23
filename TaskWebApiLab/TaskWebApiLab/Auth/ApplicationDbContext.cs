using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskWebApiLab.Auth
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<Category> Categories => Set<Category>();
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
