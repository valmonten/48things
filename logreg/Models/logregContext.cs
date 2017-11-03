using Microsoft.EntityFrameworkCore;

namespace logreg.Models
{
    public class logregContext : DbContext
    {
        public logregContext (DbContextOptions<logregContext> options) : base(options)
        {
        }
        public DbSet<Users> users {get;set;}
    }
}