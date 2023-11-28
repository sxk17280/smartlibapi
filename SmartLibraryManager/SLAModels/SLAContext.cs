using Microsoft.EntityFrameworkCore;

namespace SmartLibraryManager.SLAModels
{
    public class SLAContext :DbContext
    {
        public SLAContext(DbContextOptions<SLAContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}
