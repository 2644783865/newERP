using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataMapper
{
    class EmployeeBankMapper : EntityTypeConfiguration<EmployeeBank>
    {
        public EmployeeBankMapper()
        {
            this.ToTable("SYtbEmployeeBank");
            this.HasKey(eb => eb.ID);
            //this.Property(eb => eb.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //this.Property(eb => eb.ID).IsRequired();
            this.Property(eb => eb.UserID).IsRequired();

            this.Property(eb => eb.BankName).IsOptional();
            this.Property(eb => eb.AccountHolder).IsOptional();
            this.Property(eb => eb.BankBranchCode).IsOptional();
            this.Property(eb => eb.BankBranchName).IsOptional();
            this.Property(eb => eb.BankAccount).IsOptional();
            this.Property(eb => eb.Remark).IsOptional();

            //this.HasRequired(eb => eb.Employee).WithOptional(e => e.EmployeeBank);
            this.HasRequired(ec => ec.User).WithMany(e => e.EmployeeBanks).HasForeignKey(ec => ec.UserID);
        }
    }
}
