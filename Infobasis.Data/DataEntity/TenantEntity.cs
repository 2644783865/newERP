using System;
using Newtonsoft.Json;
using Infobasis.Data.DataMultitenant;

namespace Infobasis.Data.DataEntity
{
    /// <summary>
    /// Base class that all Entity which support multitenancy should derive from.
    /// </summary>
    [TenantAware("CompanyID")]
    public class TenantEntity : BaseEntity
    {
        /// <summary>
        /// In this case this is the Company Id. as each Company is able to access only his own Entity
        /// </summary>
        [JsonIgnoreAttribute]
        public int CompanyID { get; set; }
    }
}