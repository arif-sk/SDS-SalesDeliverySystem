using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SDS.Data.Seed;
using SDS.Domain.Entities;

namespace Caretaskr.Data
{
    public class ApplicationContext : DbContext
    {
        public bool _ignoreDataKeys;
        public List<string> _patientKey;
        public List<string> _tenantKey;
        public int? _loggedInUserId;

        //private readonly StreamWriter _logStream = new StreamWriter("mylog.txt", append: true);


        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options)
        {

            //_loggedInUserId = userData?.LoggedInUserId;
            //_tenantKey = userData?.TenantKey;
            //_patientKey = userData?.PatientKey;

            _ignoreDataKeys = true;
        }


        public ApplicationContext(DbContextOptions<ApplicationContext> options,
            IConfiguration configuration) : base(options)
        {
            //_tenantKey = userData?.TenantKey;
            //_patientKey = userData?.PatientKey;
            //_loggedInUserId = userData?.LoggedInUserId;

            //_ignoreDataKeys = configuration.GetValue<bool>("Data:IgnoreDataKeys");
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderApprovalChain> OrderApprovalChains { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedManager seedManager = new SeedManager(modelBuilder);
            seedManager.Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.LogTo(_logStream.WriteLine);
        }

        //I only have to override these two version of SaveChanges, as the other two versions call these
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {

            //this.SetCreatedModifiedDates(_loggedInUserId);
            //this.MarkCreatedItemWithDataKeys();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            //this.SetCreatedModifiedDates(_loggedInUserId);
            //this.MarkCreatedItemWithDataKeys();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public async System.Threading.Tasks.Task ExecuteNonQuery(string sql, params object[] parameters)
        {
            await this.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        /*
        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }
        public override ValueTask DisposeAsync()
        {
            _logStream.Dispose();
            return base.DisposeAsync();
        }
        */
    }
}