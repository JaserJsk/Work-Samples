using Microsoft.EntityFrameworkCore;

namespace BookLibrary.API.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            // If database is created with "EnsureCreated" then migrations will not work.
            //Database.EnsureCreated();

            /*
             * Use this approach if need to use migrations.
             * When there is no database, open "Package Manager Console" and type:
             * PM> "Add-Migration InitialMigration".
             */

            // This will enable automatic migrations.
            Database.Migrate();
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
