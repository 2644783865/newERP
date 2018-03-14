using FineUIPro;
using Infobasis.Data.DataEntity;
using Infobasis.Web.Data;
using Infobasis.Web.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infobasis.Data.DataAccess;
using System.Data;

namespace Infobasis.Web.Pages.Business
{
    public partial class ClientAdd : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                // 初始化设计所属部门
                InitUserDept();
                InitDropDownHouseStructType();
                InitDropDownDecorationStype();
                InitDropDownHouseType();
                InitDropDownClientStatus();
                InitDropDownListClientAge();
                InitDropDownDecorationColor();
                InitDropDownProvince();
                InitDropDownPackageName();
            }
        }

        private void LoadData()
        {
            labUserName.Text = UserInfo.Current.ChineseName;
            labCreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        #region
        private void InitDropDownPackageName()
        {
            DataTable table = GetEntityListTable("YSLX");
            DropDownPackageName.DataSource = table;
            DropDownPackageName.DataTextField = "Name";
            DropDownPackageName.DataValueField = "ID";
            DropDownPackageName.DataBind();

            DropDownPackageName.Items.Insert(0, new FineUIPro.ListItem("请选择一项", "-1"));
            DropDownPackageName.Items[0].Selected = true;
        }
        #endregion

        #region InitDropDownProvince
        private void InitDropDownProvince()
        {
            IQueryable table = DB.Provinces.Where(item => item.IsActive == true).OrderBy(item => item.DisplayOrder);
            DropDownProvince.DataSource = table;
            DropDownProvince.DataTextField = "Name";
            DropDownProvince.DataValueField = "ID";
            DropDownProvince.DataBind();
            DropDownProvince.Items.Insert(0, new FineUIPro.ListItem("请选择一项", "0"));
        }
        #endregion

        #region InitDropDownHouseStructType
        private void InitDropDownHouseStructType()
        {
            DataTable table = GetEntityListTable("HomeNum");
            DropDownHouseStructType.DataSource = table;
            DropDownHouseStructType.DataTextField = "Name";
            DropDownHouseStructType.DataValueField = "ID";
            DropDownHouseStructType.DataBind();

            DropDownHouseStructType.Items.Insert(0, new FineUIPro.ListItem("请选择一项", "-1"));
            DropDownHouseStructType.Items[0].Selected = true;
        }
        #endregion

        #region InitDropDownDecorationStype
        private void InitDropDownDecorationStype()
        {
            DataTable table = GetEntityListTable("Style");
            DropDownDecorationStype.DataSource = table;
            DropDownDecorationStype.DataTextField = "Name";
            DropDownDecorationStype.DataValueField = "ID";
            DropDownDecorationStype.DataBind();
        }
        #endregion

        #region InitDropDownHouseType
        private void InitDropDownHouseType()
        {
            DataTable table = GetEntityListTable("HomeType");
            DropDownHouseType.DataSource = table;
            DropDownHouseType.DataTextField = "Name";
            DropDownHouseType.DataValueField = "ID";
            DropDownHouseType.DataBind();
            DropDownHouseType.Items.Insert(0, new FineUIPro.ListItem("请选择一项", "-1"));
        }
        #endregion

        #region InitDropDownClientStatus
        private void InitDropDownClientStatus()
        {
            DataTable table = GetEntityListTable("ClientStatus");
            DropDownClientStatus.DataSource = table;
            DropDownClientStatus.DataTextField = "Name";
            DropDownClientStatus.DataValueField = "ID";
            DropDownClientStatus.DataBind();
        }
        #endregion

        #region InitDropDownDecorationColor
        private void InitDropDownDecorationColor()
        {
            DataTable table = GetEntityListTable("ColorReq");
            DropDownDecorationColor.DataSource = table;
            DropDownDecorationColor.DataTextField = "Name";
            DropDownDecorationColor.DataValueField = "ID";
            DropDownDecorationColor.DataBind();

            DropDownDecorationColor.Items.Insert(0, new FineUIPro.ListItem("可选项", "-1"));
        }
        #endregion


        #region InitDropDownListClientAge
        private void InitDropDownListClientAge()
        {
            DataTable table = GetEntityListTable("ClientAge");
            DropDownListClientAge.DataSource = table;
            DropDownListClientAge.DataTextField = "Name";
            DropDownListClientAge.DataValueField = "ID";
            DropDownListClientAge.DataBind();

            DropDownListClientAge.Items.Insert(0, new FineUIPro.ListItem("可选项", "-1"));
        }
        #endregion

        #region InitUserRole

        private void InitUserDept()
        {
            int provinceID = 0;  
            if (DropDownProvince.SelectedValue != null)
                provinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue);

            IInfobasisDataSource db = InfobasisDataSource.Create();
            DataTable table = db.ExecuteTable("EXEC usp_SY_GetDeptByType @CompanyID, @DepartmentControlType, @ProvinceID", UserInfo.Current.CompanyID,
                Infobasis.Data.DataEntity.DepartmentControlType.Design, provinceID);

            gridDept.DataSource = table;
            gridDept.DataBind();
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string tel = tbxTel.Text.Trim();
            if (string.IsNullOrEmpty(tel))
            {
                Alert.ShowInTop("请输入联系电话");
                return;
            }
            if (DB.Clients.Where(item => item.Tel == tel).Any())
            {
                Alert.ShowInTop("客户已经存在");
                return;
            }
            btnSave.Enabled = false;
            Client client = new Client()
            {
                Name = tbxProjectName.Text.Trim(),
                SpellCode = ChinesePinyin.GetPinyin(tbxProjectName.Text.Trim()),
                FirstSpellCode = ChinesePinyin.GetFirstPinyin(tbxProjectName.Text.Trim()),
                ProjectNo = GenerateNum("C"),
                Gender = ddlGender.SelectedValue,
                Email = tbxEmail.Text,
                Tel = tel,
                QQ = tbxQQ.Text,
                WeChat = tbxWeChat.Text,
                HousesName = tbxHousesName.Text,
                HouseInfoID = Infobasis.Web.Util.Change.ToInt(tbxHouseInfoID.Text.Trim()),
                SalesDeptID = 0,
                BuiltupArea = Infobasis.Web.Util.Change.ToDouble(tbxBuiltupArea.Text),
                Budget = Infobasis.Web.Util.Change.ToDouble(tbxBudget.Text),
                ClientFromName = DropDownClientFrom.SelectedValue,
                DecorationAddress = tbxDecorationAddress.Text,
                PackageName = DropDownPackageName.SelectedValue,
                //ProvinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue),
                DecorationStyleID = Infobasis.Web.Util.Change.ToInt(DropDownDecorationStype.SelectedValue),
                DecorationStyleName = DropDownDecorationStype.SelectedText,

                DecorationTypeID = Infobasis.Web.Util.Change.ToInt(DropDownDecorationType.SelectedValue),
                DecorationTypeName = DropDownDecorationType.SelectedText,

                HouseStructTypeID = Infobasis.Web.Util.Change.ToInt(DropDownHouseType.SelectedValue),
                HouseStructTypeName = DropDownHouseType.SelectedText,

                DecorationColorID = Infobasis.Web.Util.Change.ToInt(DropDownDecorationColor.SelectedValue),
                DecorationColorName = DropDownDecorationColor.SelectedText,

                //HouseUseTypeName = DropDownHouseUseType.SelectedValue,
                PlanStartDate = Infobasis.Web.Util.Change.ToDateTime(dpPlanStartDate.Text),
                PlanEndDate = Infobasis.Web.Util.Change.ToDateTime(dpPlanEndDate.Text),

                ClientProjectStatus = ClientProjectStatus.None,
                ClientProjectStatusUpdateDate = DateTime.Now,
                ClientTraceStatusID = Infobasis.Web.Util.Change.ToInt(DropDownClientStatus.SelectedValue),
                ClientTraceStatusName = DropDownClientStatus.SelectedText,

                DesignStatus = DesignStatus.None,
                Remark = tbxRemark.Text,
                CreateByID = UserInfo.Current.ID,
                CreateByName  = UserInfo.Current.ChineseName,
                CreateDatetime = DateTime.Now
            };
            if (ddbDesignerDept.Value != null && Infobasis.Web.Util.Change.ToInt(ddbDesignerDept.Value) > 0)
            { 
                client.DesignDeptID = Infobasis.Web.Util.Change.ToInt(ddbDesignerDept.Value);
                client.DesignDeptName = ddbDesignerDept.Text;

                if (ddbDesigner.Value != null && Infobasis.Web.Util.Change.ToInt(ddbDesigner.Value) > 0)
                {
                    client.DesignUserID = Infobasis.Web.Util.Change.ToInt(ddbDesigner.Value);
                    if (client.DesignUserID > 0)
                    {
                        Infobasis.Data.DataEntity.User designUser = DB.Users.Find(client.DesignUserID);
                        client.DesignUserDisplayName = designUser != null ? designUser.ChineseName : "";
                        client.AssignToDesignerDatetime = DateTime.Now;
                    }
                }
                else
                {
                    client.DesignUserID = null;
                    client.DesignUserDisplayName = "";
                }
            }

            if (DropDownProvince.SelectedValue != null)
            {
                client.ProvinceID = Infobasis.Web.Util.Change.ToInt(DropDownProvince.SelectedValue);
                client.ProvinceName = DropDownProvince.SelectedText;
            }
            else
            {
                client.ProvinceID = null;
                client.ProvinceName = null;            
            }

            if (DropDownBoxClientNeed.Values != null)
            {
                client.ClientNeedIDs = string.Join(",", DropDownBoxClientNeed.Values);
                client.ClientNeedName = string.Join(",", DropDownBoxClientNeed.Values);
            }
            else
            {
                client.ClientNeedIDs = "";
                client.ClientNeedName = "";
            }

            Infobasis.Data.DataEntity.ClientTrace clientTrace = new Infobasis.Data.DataEntity.ClientTrace()
            {
                ClientID = client.ID,
                CreateByID = UserInfo.Current.ID,
                UserID = UserInfo.Current.ID,
                UserDisplayName = UserInfo.Current.ChineseName,
                TraceDesc = client.ToString()
            };
            client.ClientTraces = new List<Infobasis.Data.DataEntity.ClientTrace>();
            client.ClientTraces.Add(clientTrace);
            DB.Clients.Add(client);
            if (SaveChanges())
            {
                ShowNotify("保存成功！");
                PageContext.RegisterStartupScript("refreshTopWindow();");
            }
            btnSave.Enabled = true;
        }

        protected void ddbDesignerDept_TextChanged(object sender, EventArgs e)
        {
            ddbDesigner.Value = "";
            ddbDesigner.Text = "";
            InitDesigner();
        }

        #region InitUserRole

        private void InitDesigner()
        {
            int deptID = Infobasis.Web.Util.Change.ToInt(ddbDesignerDept.Value);

            if (deptID > 0)
            {
                IInfobasisDataSource db = InfobasisDataSource.Create();
                DataTable table = db.ExecuteTable("EXEC usp_SY_GetEmployeeByDept @CompanyID, @DeptID", UserInfo.Current.CompanyID,
                    deptID);

                gridDesigner.DataSource = table;
                gridDesigner.DataBind();
                ddbDesigner.Enabled = true;
            }
            else
                ddbDesigner.Enabled = false;
        }

        #endregion

        protected void DropDownProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitUserDept();
            ddbDesignerDept.Value = null;
            ddbDesignerDept.Text = "";
        }
    }
}