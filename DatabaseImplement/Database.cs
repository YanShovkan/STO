using DatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseImplement
{
    public class Database : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-KCR4TIA;Initial Catalog=STODatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<CostItem> CostItems { set; get; }
        public virtual DbSet<Payment> Payments { set; get; }
        public virtual DbSet<RepairRequest> RepairRequests { set; get; }
        public virtual DbSet<Work> Works { set; get; }
        public virtual DbSet<RepairRequestCostItem> RepairRequestCostItems { set; get; }
        public virtual DbSet<RepairRequestWork> RepairRequestWorks { set; get; }
        public virtual DbSet<Worker> Workers { set; get; }
    }
}
