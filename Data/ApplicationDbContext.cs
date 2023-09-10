using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
    
        public DbSet<Url> Urls { get; set; }
        public DbSet<AboutContentModel> AboutContentModels { get; set; }
    }
}