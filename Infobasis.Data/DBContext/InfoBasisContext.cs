using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Infobasis.Data.DataEntity;
//using Infobasis.Data.DataMapper;
using Infobasis.Data.DataMultitenant;
using Infobasis.Data.DataEntity.Import;
using Infobasis.Data.DataMapper.System;
using Infobasis.Data.DataMapper;
using System.Data.SqlTypes;

namespace Infobasis.Data
{
    public class InfobasisContext : DbContext
    {
        public InfobasisContext() : base("InfobasisConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            //this.Database.Connection.ConnectionString = "";

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InfobasisContext, InfobasisContextMigrationConfiguration>());
        }

        public DbSet<Company> Companys { get; set; }
        public DbSet<EmployeeBank> EmployeeBanks { get; set; }
        public DbSet<EmployeeContract> EmployeeContracts { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public DbSet<EmployeeWorkExperience> EmployeeWorkExperiences { get; set; }
        public DbSet<EmployeeAdjust> EmployeeAdjusts { get; set; }
        public DbSet<EmployeeFixPay> EmployeeFixPays { get; set; }
        public DbSet<EmployeeFixPayAdjust> EmployeeFixPayAdjusts { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }

        public DbSet<Department> Departments { get; set; }

        //导入相关
        public DbSet<Import> Imports { get; set; }
        public DbSet<ImportHoldData> ImportHoldDatas { get; set; }
        public DbSet<ImportTemplateColumn> ImportTemplateColumns { get; set; }


        //权限相关
        public DbSet<Module> Modules { get; set; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }
        public DbSet<UserPermissionRole> UserPermissionRoles { get; set; }
        public DbSet<ModulePermissionRole> ModulePermissionRoles { get; set; }


        //发送短信消息
        public DbSet<MessageHistory> MessageHistorys { get; set; }
        //邮件
        public DbSet<Email> Emails { get; set; }
        //消息队列,需要处理的消息,将来考虑其他like redis
        public DbSet<MessageQueue> MessageQueues { get; set; }
        //消息代办事项
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSetting> NotificationSettings { get; set; }
        public DbSet<NotificationSettingReceiver> NotificationSettingReceivers { get; set; }
        public DbSet<NotificationReceiver> NotificationReceivers { get; set; }

        public DbSet<DBErrorLog> DBErrorLogs { get; set; }
        public DbSet<ThirdpartyAccessTokenInfo> ThirdpartyAccessTokenInfos { get; set; }
        public DbSet<FindPassword> FindPasswords { get; set; }

        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<SystemNum> SystemNums { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientTrace> ClientTraces { get; set; }
        public DbSet<BudgetTemplateData> BudgetTemplateDatas { get; set; }
        public DbSet<BudgetTemplateSpace> BudgetTemplateSpaces { get; set; }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingTask> MeetingTasks { get; set; }

        public DbSet<UserGoal> UserGoals { get; set; }
        public DbSet<HouseInfo> HouseInfos { get; set; }

        public DbSet<EntityList> EntityLists { get; set; }
        public DbSet<EntityListValue> EntityListValues { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Citys { get; set; }

        //public DbSet<MaterialUnit> MaterialUnits { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorContact> VendorContacts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Material> Materials { get; set; }

        public DbSet<BudgetTemplate> BudgetTemplates { get; set; }
        public DbSet<BudgetTemplateItem> BudgetTemplateItems { get; set; }   
        public DbSet<BudgetTemplateItemMaterial> BudgetTemplateItemMaterials { get; set; }
        public DbSet<BudgetTemplateInclude> BudgetTemplateIncludes { get; set; }
        public DbSet<BudgetTemplateRate> BudgetTemplateRates { get; set; }
        public DbSet<BudgetTemplateBasePrice> BudgetTemplateBasePrices { get; set; }
        public DbSet<BudgetTemplateWaterElec> BudgetTemplateWaterElecs { get; set; }

        public DbSet<CloudFolder> CloudFolders { get; set; }
        public DbSet<CloudFile> CloudFiles { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<SMSTemplate> SMSTemplates { get; set; }
        public DbSet<SMSSendHistory> SMSSendHistorys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region apply the custom mapping rules for each column of the tables.

            /*
            * TODO: the statement of the rules of other tables.
            */
            modelBuilder.Configurations.Add(new ModuleMapper());
            modelBuilder.Configurations.Add(new UserMapper());
            modelBuilder.Configurations.Add(new DepartmentMapper());
            modelBuilder.Configurations.Add(new EmployeeBankMapper());
            modelBuilder.Configurations.Add(new CloudFolderMapper());

            //分表
            /*
            modelBuilder.Entity<User>()
                .HasKey(t => t.ID);

            modelBuilder.Entity<Employee>()
                .HasKey(t => t.ID);

            modelBuilder.Entity<Employee>()
                .HasRequired(t => t.User)
                .WithRequiredPrincipal(t => t.Employee);
            */
            #endregion

            modelBuilder.Entity<BudgetTemplateSpace>().Property(item => item.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<BudgetTemplateSpace>().Property(item => item.Size).HasPrecision(4, 1);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var conv = new AttributeToTableAnnotationConvention<TenantAwareAttribute, string>(
                TenantAwareAttribute.TenantAnnotation, (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(conv);

            //DbInterception.Add()
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateDates();
            return base.SaveChanges();
        }

        private void UpdateDates()
        {
            foreach (var change in ChangeTracker.Entries<TenantEntity>())
            {
                var values = change.CurrentValues;
                foreach (var name in values.PropertyNames)
                {
                    var value = values[name];
                    if (name == "CreateDatetime" && value == null)
                    {
                        values[name] = DateTime.Now;
                    }

                    if (value is DateTime)
                    {
                        var date = (DateTime)value;
                        if (date < SqlDateTime.MinValue.Value)
                        {
                            values[name] = SqlDateTime.MinValue.Value;
                        }
                        else if (date > SqlDateTime.MaxValue.Value)
                        {
                            values[name] = SqlDateTime.MaxValue.Value;
                        }
                    }
                }
            }
        }
    }
}
