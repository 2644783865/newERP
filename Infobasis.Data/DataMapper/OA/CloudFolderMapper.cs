using Infobasis.Data.DataEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataMapper
{
    public class CloudFolderMapper : EntityTypeConfiguration<CloudFolder>
    {
        public CloudFolderMapper()
        {
            // Relationships
            this.HasOptional(d => d.ParentCloudFolder)
                .WithMany(d => d.ChildrenCloudFolders)
                .HasForeignKey(d => d.ParentID);
        }
    }
}
