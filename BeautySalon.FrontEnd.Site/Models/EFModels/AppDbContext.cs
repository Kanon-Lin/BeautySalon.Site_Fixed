using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BeautySalon.FrontEnd.Site.Models.EFModels
{
	public partial class AppDbContext : DbContext
	{
		public AppDbContext()
			: base("name=AppDbContext")
		{
		}

		public virtual DbSet<Appointment> Appointments { get; set; }
		public virtual DbSet<CustomerInquiry> CustomerInquiries { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }
		public virtual DbSet<Member> Members { get; set; }
		public virtual DbSet<OrderDetail> OrderDetails { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
		public virtual DbSet<Service> Services { get; set; }
		public virtual DbSet<UsageRecord> UsageRecords { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CustomerInquiry>()
				.Property(e => e.TelephoneNumber)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.Property(e => e.EmployeeNo)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.Property(e => e.Password)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.Property(e => e.Phone)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.Property(e => e.Image)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.HasMany(e => e.EmployeeSkills)
				.WithRequired(e => e.Employee)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.PhoneNumber)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Account)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.EncryptedPassword)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.ConfirmCode)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Appointments)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Orders)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<OrderDetail>()
				.Property(e => e.UnitPrice)
				.HasPrecision(10, 0);

			modelBuilder.Entity<OrderDetail>()
				.Property(e => e.Amount)
				.HasPrecision(10, 0);

			modelBuilder.Entity<OrderDetail>()
				.Property(e => e.DiscountApplied)
				.HasPrecision(10, 0);

			modelBuilder.Entity<OrderDetail>()
				.Property(e => e.NetAmount)
				.HasPrecision(10, 0);

			modelBuilder.Entity<OrderDetail>()
				.HasMany(e => e.Appointments)
				.WithRequired(e => e.OrderDetail)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order>()
				.Property(e => e.TotalAmount)
				.HasPrecision(10, 0);

			modelBuilder.Entity<Order>()
				.Property(e => e.TotalNetAmount)
				.HasPrecision(10, 0);

			modelBuilder.Entity<Order>()
				.Property(e => e.DiscountedAmount)
				.HasPrecision(10, 0);

			modelBuilder.Entity<Order>()
				.HasMany(e => e.Appointments)
				.WithRequired(e => e.Order)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order>()
				.HasMany(e => e.OrderDetails)
				.WithRequired(e => e.Order)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ServiceCategory>()
				.Property(e => e.Description)
				.IsUnicode(false);

			modelBuilder.Entity<ServiceCategory>()
				.Property(e => e.Image)
				.IsUnicode(false);

			modelBuilder.Entity<ServiceCategory>()
				.HasMany(e => e.EmployeeSkills)
				.WithRequired(e => e.ServiceCategory)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ServiceCategory>()
				.HasMany(e => e.Services)
				.WithRequired(e => e.ServiceCategory)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Service>()
				.Property(e => e.Price)
				.HasPrecision(10, 0);

			modelBuilder.Entity<Service>()
				.HasMany(e => e.Appointments)
				.WithRequired(e => e.Service)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Service>()
				.HasMany(e => e.OrderDetails)
				.WithRequired(e => e.Service)
				.WillCascadeOnDelete(false);
		}
	}
}
