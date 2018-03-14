using System;
using System.Data.Entity;
using Infobasis.Data.DataEntity;

namespace Infobasis.Data.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private DbContext context;
        private GenericRepository<Province> provinceRepository;

        public UnitOfWork()
        {
            this.context = new InfobasisContext();
        }
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public GenericRepository<T> Repository<T>() where T : class
        {
            return new GenericRepository<T>(context);
        }

        public GenericRepository<Province> ProvinceRepository
        {
            get
            {
                if (this.provinceRepository == null)
                {
                    this.provinceRepository = new GenericRepository<Province>(context);
                }
                return provinceRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool Save(out string msg)
        {
            msg = "";
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public bool ExecuteSqlCommand(string sql, out string msg, params object[] parameters)
        {
            msg = "";
            try
            {
                //var sql = @"Update [User] SET FirstName = {0} WHERE Id = {1}";
                //ctx.Database.ExecuteSqlCommand(sql, firstName, id);
                context.Database.ExecuteSqlCommand(sql, parameters);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            int result = 0;
            try
            {
                result = context.Database.ExecuteSqlCommand(sql, parameters);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
