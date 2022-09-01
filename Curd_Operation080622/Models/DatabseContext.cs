using Microsoft.EntityFrameworkCore;
using Curd_Operation080622.Models;

namespace Curd_Operation080622.Models
{
    public class DatabseContext:DbContext
    {
        public DatabseContext(DbContextOptions<DatabseContext>option):base(option)
        {

        }
        public DbSet<Teacher>teachers { get; set; }
       public DbSet<Country> Countries { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Hobbie> Hobbies{ get; set; }
        public DbSet<TblState> tblStates { get; set; }

    }
}
