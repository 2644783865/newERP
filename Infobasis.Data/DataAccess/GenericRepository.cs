using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using System.Security.Claims;
using System.Threading;
using System.Data.Entity.Infrastructure;

using Infobasis.Data.DataEntity;
using System.Data.Entity.Validation;
using System.Data.Common;

namespace Infobasis.Data.DataAccess
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

         public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {

            //query = unitOfWork.SocialBenefitRuleRepository.GetWithRawSql(
            //    "SELECT * FROM dbo.SocialBenefitRule WHERE CityID = {0}", cityId);

            return dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public virtual IQueryable<TEntity> Get(string includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool noTracking = false)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (noTracking)
                query = query.AsNoTracking();

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual bool Insert(TEntity entity, out string msg, bool isImmediateSave = true)
        {
            msg = "";
            try
            {
                dbSet.Add(entity);

                if (isImmediateSave)
                    context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                    msg = dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                else
                    msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

            return true;
        }

        public virtual bool Delete(object id, out string msg, bool isImmediateSave = true)
        {
            TEntity entityToDelete = dbSet.Find(id);
            return Delete(entityToDelete, out msg, isImmediateSave);
        }

        public virtual bool Delete(TEntity entityToDelete, out string msg, bool isImmediateSave = true)
        {
            msg = "";
            try
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);

                if (isImmediateSave)
                    context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                    msg = dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                else
                    msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public virtual bool Update(TEntity entityToUpdate, out string msg, bool isImmediateSave = true)
        {
            msg = "";
            try
            {
                dbSet.Attach(entityToUpdate);
                context.Entry(entityToUpdate).State = EntityState.Modified;

                if (isImmediateSave)
                    context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                    msg = dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                else
                    msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public virtual bool PartialUpdate(TEntity entity, out string msg, params Expression<Func<TEntity, object>>[] propsToUpdate)
        {
            //...PartialUpdate(Model, d=>d.Name, d=>d.SecondProperty, d=>d.AndSoOn);
            msg = "";
            try
            {
                dbSet.Attach(entity);
                var contextEntry = context.Entry(entity);
                foreach (var prop in propsToUpdate)
                    contextEntry.Property(prop).IsModified = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                    msg = dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                else
                    msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public virtual bool AddOrUpdate(TEntity[] EntityToAddOrUpdate, out string msg,bool isImmediateSave = true)
        {
            msg = "";
            try
            {
                dbSet.AddOrUpdate(EntityToAddOrUpdate);

                if (isImmediateSave)
                    context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                    msg = dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                else
                    msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        public virtual bool ExecuteSqlCommand(string sql, out string msg, params object[] parameters)
        {
            msg = "";
            try
            {
                //var sql = @"Update [User] SET FirstName = {0} WHERE Id = {1}";
                //ctx.Database.ExecuteSqlCommand(sql, firstName, id);
                context.Database.ExecuteSqlCommand(sql, parameters);
            }
            catch (DbEntityValidationException dbEx)
            {
                msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }
        
        public virtual bool ExecuteSqlCommand(string sql, out string msg, params IDbDataParameter[] parameters)
        {
            msg = "";
            try
            {
                //var sql = @"Update [User] SET FirstName = @FirstName WHERE Id = @Id";
                //ctx.Database.ExecuteSqlCommand(sql, new SqlParameter("firstName", firstname), new SqlParameter("id", id));
                context.Database.ExecuteSqlCommand(sql, parameters);
            }
            catch (DbEntityValidationException dbEx)
            {
                msg = dbEx.Message;
                return false;
            }
            catch (DbUpdateException dbUpdEx)
            {
                msg = dbUpdEx.InnerException.InnerException.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        ///     var name = new SqlParameter { ParameterName = "Name", Value = Name };
        ///     var currentpage = new SqlParameter { ParameterName = "PageIndex", Value = currentPage };
        ///     var pagesize = new SqlParameter { ParameterName = "PageSize", Value = pageSize };
        ///     var totalcount = new SqlParameter { ParameterName = "TotalCount", Value = 0, Direction = ParameterDirection.Output };
        ///     var list = ctx.ExecuteStoredProcedureList<Student>("Myproc", pageindex, pagesize, totalcount);
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        commandText += " output";
                    }
                }
            }

            var result = context.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
            bool acd = context.Configuration.AutoDetectChangesEnabled;
            try
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                for (int i = 0; i < result.Count; i++)
                    result[i] = context.Set<TEntity>().Attach(result[i]);
            }
            finally
            {
                context.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;        
        }
    }
}
