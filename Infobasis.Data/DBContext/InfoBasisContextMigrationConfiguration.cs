using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace Infobasis.Data
{
    class InfobasisContextMigrationConfiguration : DbMigrationsConfiguration<InfobasisContext>
    {
        public InfobasisContextMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            
        }

#if DEBUG
        protected override void Seed(InfobasisContext context)
        {
            //new InfobasisDataSeeder(context).Seed();
        }
#endif
    }
}
