using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataMapper
{
    public class UserMapper : EntityTypeConfiguration<User>
    {
        public UserMapper()
        {
            this.HasKey(e => e.ID);
            this.Property(e => e.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(e => e.EmployeeSpellCode).IsOptional();

            // Relationships
            this.HasOptional(e => e.ReportsToUser)
                .WithMany(e => e.ManagedUsers)
                .HasForeignKey(e => e.ReportsTo)
                .WillCascadeOnDelete(false);

            //this.HasOptional(e => e.Department).WithMany().HasForeignKey(e => e.DepartmentID);
            this.HasOptional(e => e.Department).WithMany(d => d.Users).HasForeignKey(e => e.DepartmentID);

        }
    }
}
