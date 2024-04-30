using DataExporter.Model;
using Microsoft.EntityFrameworkCore;


namespace DataExporter
{
	public class ExporterDbContext : DbContext
	{
		public DbSet<Policy> Policies { get; set; }

		public ExporterDbContext(DbContextOptions<ExporterDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseInMemoryDatabase("ExporterDb");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var dummyNotes = new List<Note>()
			{
				new Note(){ Id= 1, PolicyId = 1, Text = "test" },
				new Note(){ Id= 2, PolicyId = 2, Text = "testing" },
				new Note(){ Id= 3, PolicyId = 3, Text = "testing more" },
			};

			var dummyPolicies = new List<Policy>()
			{
				new Policy() { Id = 1, PolicyNumber = "HSCX1001", Premium = 200, StartDate = new DateTime(2024, 4, 1) },
				new Policy() { Id = 2, PolicyNumber = "HSCX1002", Premium = 153, StartDate = new DateTime(2024, 4, 5) },
				new Policy() { Id = 3, PolicyNumber = "HSCX1003", Premium = 220, StartDate = new DateTime(2024, 3, 10) },
				new Policy() { Id = 4, PolicyNumber = "HSCX1004", Premium = 200, StartDate = new DateTime(2024, 5, 1) },
				new Policy() { Id = 5, PolicyNumber = "HSCX1005", Premium = 100, StartDate = new DateTime(2024, 4, 1) }
			};

			var policyBuilder = modelBuilder.Entity<Policy>();
			policyBuilder.HasData(dummyPolicies);


			var notesBuilder = modelBuilder.Entity<Note>();
			notesBuilder.HasData(dummyNotes);

			policyBuilder.HasMany(p => p.Notes).WithOne(n => n.Policy).HasForeignKey(n => n.PolicyId);

			base.OnModelCreating(modelBuilder);
		}
	}
}
