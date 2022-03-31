using System.Data.Entity;

namespace RecipeManager.Model
{
    public partial class DbModel :DbContext
    {
        public DbModel()
            : base("name=OrganizerPrzepisówKuchennych")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
