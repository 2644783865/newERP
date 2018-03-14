using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataAccess
{
    public class InfobasisDataSource : SqlDataSource, IInfobasisDataSource
    {
        static string connectionString = null;

		public static InfobasisDataSource Create()
        {
            if (connectionString == null)
            {
                InfobasisContext ctx = new InfobasisContext();
                connectionString = ctx.Database.Connection.ConnectionString;
            }
			return new InfobasisDataSource();
		}

        private InfobasisDataSource()
            : base(connectionString)
		{
		}

    }
}
