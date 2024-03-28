using Microsoft.EntityFrameworkCore;
using TesteMazza.Api.Models;

namespace TesteMazza.Api.Data
{
    public class TesteDataContext : DbContext
    {
        public TesteDataContext(DbContextOptions<TesteDataContext> options) : base(options)
        {

        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
     
        
    }
}
