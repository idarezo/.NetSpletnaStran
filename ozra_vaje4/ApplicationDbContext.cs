
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ozra_vaje4.Models;
using Microsoft.EntityFrameworkCore.Sqlite;
namespace ozra_vaje4
{
    public class ApplicationDbContext : IdentityDbContext<UserViewModel>
    {
        public DbSet<UserViewModel> uporabniki { get; set; }
        public DbSet<CommentViewModel> komentarji { get; set; }
        


        public string DbPath { get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();

            DbPath = String.Join(startupPath, "OzraZadnjeVaje.db");
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
     => options.UseSqlite($"Data Source={DbPath}");
    }
}
