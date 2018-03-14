using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataMapper
{
    public class ModuleMapper : EntityTypeConfiguration<Module>
    {
        public ModuleMapper()
        {
            this.ToTable("SYtbModule");
            this.HasKey(item => item.ID);
        }
    }
}
