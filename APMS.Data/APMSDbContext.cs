using APMS.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace APMS.Data;

public partial class APMSDbContext : DbContext
{
    public APMSDbContext()
    {
    }

    public APMSDbContext(DbContextOptions<APMSDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Register> Registers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ASHISH;Database=APMS;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;");

   
}
