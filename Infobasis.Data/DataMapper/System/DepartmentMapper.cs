using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataMapper.System
{
    public class DepartmentMapper : EntityTypeConfiguration<Department>
    {
        public DepartmentMapper()
        {
            this.ToTable("SYtbDepartment");
            this.HasKey(d => d.ID);
            this.Property(d => d.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(d => d.ID).IsRequired();
            this.Property(d => d.CompanyID).IsRequired();
            this.Property(d => d.Name).IsRequired();
            this.Property(d => d.Name).HasMaxLength(255);
            this.Property(d => d.FullName).HasMaxLength(255);
            this.Property(d => d.Code).HasMaxLength(30);
            this.Property(d => d.DisplayOrder).IsOptional();

            // Relationships
            this.HasOptional(d => d.ParentDepartment)
                .WithMany(d => d.ChildrenDepartments)
                .HasForeignKey(d => d.ParentID);

            this.HasOptional(d => d.User).WithMany().HasForeignKey(d => d.LeaderID);

        }
    }
}
