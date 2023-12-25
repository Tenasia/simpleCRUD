using System;
using System.Data;
using System.Threading.Tasks;
using OnePhp.HRIS.Core.Data;
using OnePhp.HRIS.Core.Model;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace OnePhp.HRIS.Core.Model
{
    public class SystemSetupOrganizational
    {


    }


    public class CompanyInfo
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public BarangayAutocompleteJSON Barangay { get; set; }
        public CityAutocompleteJSON City { get; set; }
        public ProvinceAutocompleteJSON Province { get; set; }

        public static void SaveCompanyInfo(CompanyInfo data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_CompanyInfo",
                            new string[]
                            {
                                "eName",
                                "eStreet",
                                "eCity",
                                "eProvince"
                            },
                            new DbType[]
                            {
                                DbType.String,
                                DbType.String,
                                DbType.Int32,
                                DbType.Int32
                            },
                            new object[]
                            {
                               data.Name,
                                data.Street,
                                data.City.Id,
                                data.Province.Id
                            },
                            out a,
                            CommandType.StoredProcedure
                    );
            }
        }
        public static void EditCompanyInfo(CompanyInfo data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery
                    (
                        "HRIS_Edit_CompanyInfo",
                            new string[]
                            {
                                "eID",
                                "eName",
                                "eStreet",
                                "eCity",
                                "eProvince"
                            },
                            new DbType[]
                            {
                                DbType.Int64,
                                DbType.String,
                                DbType.String,
                                DbType.Int32,
                                DbType.Int32
                            },
                            new object[]
                            {
                                data.ID,
                                data.Name,
                                data.Street,
                                data.City.Id,
                                data.Province.Id
                            },
                        out a,
                        CommandType.StoredProcedure
                    );
            }
        }
        public static void DeleteCompanyInfo(CompanyInfo data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery
                    (
                        "HRIS_Delete_CompanyInfo",
                            new string[]
                            {
                                "eID"
                            },
                            new DbType[]
                            {
                                DbType.Int64
                            },
                            new object[]
                            {
                                data.ID
                            },
                        out a,
                        CommandType.StoredProcedure
                    );
            }
        }
        public static List<CompanyInfo> GetList()
        {
            var _list = new List<CompanyInfo>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_CompanyInfo",
                    new string[] { },
                    new DbType[] { },
                    new object[] { },
                    out a,
                    ref aTable,
                    CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    CompanyInfo c = new CompanyInfo();
                    c.Barangay = new BarangayAutocompleteJSON();
                    c.City = new CityAutocompleteJSON();
                    c.Province = new ProvinceAutocompleteJSON();

                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Street = aRow["Street"].ToString();
                    c.Name = aRow["Name"].ToString();
                    c.City.Id = Convert.ToInt32(aRow["City"]);
                    c.City.Text = aRow["CityName"].ToString();
                    c.Province.Id = Convert.ToInt32(aRow["Province"]);
                    c.Province.Text = aRow["ProvinceName"].ToString();
                    _list.Add(c);
                }
            }
            return _list;
        }
        public static CompanyInfo GetListById(Int64 Id)
        {
            var c = new CompanyInfo();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader
                    (
                    "HRIS_GetById_CompanyInfo",
                        new string[]
                        {
                            "eID"
                        },
                        new DbType[]
                        {
                            DbType.Int64
                        },
                        new object[]
                        {
                              Id
                        },
                    out a,
                    ref aTable,
                    CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Barangay = new BarangayAutocompleteJSON();
                    c.City = new CityAutocompleteJSON();
                    c.Province = new ProvinceAutocompleteJSON();

                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Street = aRow["Street"].ToString();
                    c.Name = aRow["Name"].ToString();
                    c.City.Id = Convert.ToInt32(aRow["City"]);
                    c.City.Text = aRow["CityName"].ToString();
                    c.Province.Id = Convert.ToInt32(aRow["Province"]);
                    c.Province.Text = aRow["ProvinceName"].ToString();
                }
            }
            return c;
        }


    }
    public class Department
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Employee Head { get; set; }
        public PositionCode HeadPosition { get; set; }
        public PositionCode Position { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public int EmployeeCount { get; set; }
        public static string Save(Department data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("BETA_isDepCodeExist_Department",
                    new string[] { "dDepCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Department code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExist_Department",
                    new string[] { "dDesc" },
                    new DbType[] { DbType.String },
                    new object[] { data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Department description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_Department",
                            new string[] { "dDepCode", "dDescription", "dDepHead", "dPosition", "dAddedBy", "dModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.String, DbType.String },
                            new object[] { data.Code, data.Description, data.Head.ID, data.Position.ID, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + ", Head:" + data.Head.ID.ToString() + ", Position:" + data.Position.ID.ToString() + ", ";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "Save successfully.";
                    }
                }
            }
            return res;
        }
        //using this for file upload in console application
        //nothing to do in browser data (POST & GET)
        public static void SaveFileUpload(Department data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int c = 0;
                db.ExecuteCommandNonQuery("BETA_Save_Department",
                            new string[] { "dDepCode", "dDescription", "dDepHead", "dPosition", "dAddedBy", "dModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.String, DbType.String },
                            new object[] { data.Code, data.Description, data.Head.ID, data.Position.ID, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
            }
        }
        public static string Update(Department data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int z = 0;
                DataTable aTable = new DataTable();
                DataTable zTable = new DataTable();
                db.ExecuteCommandReader("BETA_isDepCodeExist_Update_Department",
                    new string[] { "dID", "dDepCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out a, ref zTable, CommandType.StoredProcedure);
                if (zTable.Rows.Count == 1)
                {
                    res = "Department code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescInuse_Department",
                    new string[] { "dID", "dDesc" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count == 1)
                    {
                        res = "In use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_Department",
                            new string[] { "dID", "dDepCode", "dDescription", "dDepHead", "dPosition", "dModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.String },
                            new object[] { data.ID, data.Code, data.Description, data.Head.ID, data.Position.ID, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + ", Head:" + data.Head.ID.ToString() + ", Position:" + data.Position.ID.ToString() + ", ";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "Updated Successfully.";
                    }
                }
            }
            return res;
        }
        public static void Delete(Department data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_Department",
                       new string[] { "dID", "dDepCode" },
                       new DbType[] { DbType.Int64, DbType.String },
                       new object[] { data.ID, data.Code }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<Department> Get()
        {
            var res = new List<Department>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_Department",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Department d = new Department();
                    d.Head = new Employee();
                    d.Position = new PositionCode();
                    if (aRow["DepHead"] != DBNull.Value)
                    {
                        d.Head.ID = Convert.ToInt64(aRow["DepHead"]);
                    }

                    d.Head.Firstname = aRow["FirstName"].ToString();
                    d.Head.Lastname = aRow["LastName"].ToString();
                    d.Head.Middlename = aRow["MiddleName"].ToString();
                    d.Head.Suffix = aRow["Suffix"].ToString();
                    if (aRow["Position"] != DBNull.Value)
                    {
                        d.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                    }

                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Code = aRow["DepCode"].ToString();
                    d.Description = aRow["Description"].ToString();
                    res.Add(d);
                }
            }
            return res;
        }
        public static Department GetByID(Int64 Id)
        {

            var d = new Department();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_Department",
                    new string[] { "dID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    d.Head = new Employee();
                    d.HeadPosition = new PositionCode();
                    d.Position = new PositionCode();
                    if (aRow["DepHead"] != DBNull.Value)
                    {
                        d.Head.ID = Convert.ToInt64(aRow["DepHead"]);
                    }

                    d.Head.Firstname = aRow["FirstName"].ToString();
                    d.Head.Lastname = aRow["LastName"].ToString();
                    d.Head.Middlename = aRow["MiddleName"].ToString();
                    d.Head.Suffix = aRow["Suffix"].ToString();
                    if (aRow["Position"] != DBNull.Value)
                    {
                        d.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                    }
                    d.HeadPosition.Position = aRow["HeadPosition"].ToString();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Code = aRow["DepCode"].ToString();
                    d.Description = aRow["Description"].ToString();
                }
            }
            return d;
        }
        public static PositionCode GetEmployeePosition(Int64 employeeID)
        {
            var p = new PositionCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeePosition_employee",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { employeeID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    p.ID = Convert.ToInt64(aRow["Position"]);
                    p.Position = aRow["PositionName"].ToString();
                }
            }
            return p;
        }
    }


    public class Division
    {
        public long ID { get; set; }
        public string Div_Code { get; set; }
        public string Div_Desc { get; set; }
        public string Code { get; set; }
        public string GroupType { get; set; }
        public Employee Head { get; set; }
        public PositionCode Position { get; set; }
        public PositionCode HeadPosition { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string Save(Division data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int c = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("BETA_isDivCodeExist_Division",
                    new string[] { "dDivCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Div_Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDivDescInUse_Division",
                    new string[] { "dDivDesc" },
                    new DbType[] { DbType.String },
                    new object[] { data.Div_Desc }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count == 1)
                    {
                        res = "Desc in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_Division",
                           new string[] { "dDivCode", "dDivDesc", "dCode", "dGroupType", "dDivHead", "dPosition", "dAddedBy", "dModifiedBy" },
                           new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.String, DbType.String },
                           new object[] { data.Div_Code, data.Div_Desc, data.Code, data.GroupType, data.Head.ID, data.Position.ID, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Div_Code + ", Description:" + data.Div_Desc + ",  Code:" + data.Code + ",  GroupType:" + data.GroupType + ", Head:" + data.Head.ID.ToString() + ", Position:" + data.Position.ID.ToString() + ", ";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "Save successfully.";
                    }
                }
            }
            return res;
        }
        //use this code for save details via file upload in 
        public static void SaveFromUploadedfile(Division data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int c = 0;

                db.ExecuteCommandNonQuery("BETA_Save_Division",
                           new string[] { "dDivCode", "dDivDesc", "dCode", "dGroupType", "dDivHead", "dPosition", "dAddedBy", "dModifiedBy" },
                           new DbType[] { DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String },
                           new object[] { data.Div_Code, data.Div_Desc, data.Code, data.GroupType, data.Head.ID, data.Position.ID, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
            }
        }
        public static string Update(Division data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int c = 0;
                int b = 0;
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("BETA_isDivCodeExist_Update_Division",
                    new string[] { "dID", "dDivCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Div_Code }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count == 1)
                {
                    res = "Code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDivDescInUse_Update_Division",
                        new string[] { "dID", "dDivDesc" },
                        new DbType[] { DbType.Int64, DbType.String },
                        new object[] { data.ID, data.Div_Desc }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count == 1)
                    {
                        res = "Desc in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_Division",
                       new string[] { "dID", "dDivCode", "dDivDesc", "dCode", "dGroupType", "dDivHead", "dPosition", "dModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.String },
                       new object[] { data.ID, data.Div_Code, data.Div_Desc, data.Code, data.GroupType, data.Head.ID, data.Position.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Div_Code + ", Description:" + data.Div_Desc + ",  Code:" + data.Code + ",  GroupType:" + data.GroupType + ", Head:" + data.Head.ID.ToString() + ", Position:" + data.Position.ID.ToString() + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "Updated Successfully.";
                    }
                }
            }
            return res;
        }
        public static void Delete(Division data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_Division",
                       new string[] { "dID", "dDivCode" },
                       new DbType[] { DbType.Int64, DbType.String },
                       new object[] { data.ID, data.Div_Code }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<Division> Get()
        {
            var res = new List<Division>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_Division",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Division d = new Division();
                    d.Head = new Employee();
                    d.Position = new PositionCode();
                    d.HeadPosition = new PositionCode();
                    d.Head.ID = Convert.ToInt64(aRow["DivHead"]);
                    d.Head.Firstname = aRow["FirstName"].ToString();
                    d.Head.Lastname = aRow["LastName"].ToString();
                    d.Head.Middlename = aRow["MiddleName"].ToString();
                    d.Head.Suffix = aRow["Suffix"].ToString();
                    d.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Div_Code = aRow["Div_Code"].ToString();
                    d.Div_Desc = aRow["Div_desc"].ToString();
                    d.Code = aRow["GroupCode"].ToString();
                    d.GroupType = aRow["GroupType"].ToString();

                    res.Add(d);
                }
            }
            return res;
        }
        public static Division GetByID(Int64 Id)
        {
            var d = new Division();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_Division",
                    new string[] { "dID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Head = new Employee();
                    d.HeadPosition = new PositionCode();
                    d.Position = new PositionCode();
                    d.Div_Code = aRow["Div_Code"].ToString();
                    d.Div_Desc = aRow["Div_desc"].ToString();
                    d.HeadPosition.Position = aRow["HeadPosition"].ToString();
                    d.Head.Firstname = aRow["FirstName"].ToString();
                    d.Head.Lastname = aRow["LastName"].ToString();
                    d.Head.Middlename = aRow["MiddleName"].ToString();
                    d.Head.Suffix = aRow["Suffix"].ToString();
                    d.Code = aRow["GroupCode"].ToString();
                    d.GroupType = aRow["GroupType"].ToString();
                    d.Head.ID = Convert.ToInt64(aRow["DivHead"]);
                    d.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                }
            }
            return d;
        }

    }

    public class ProjectCode
    {
        public long ID { get; set; }
        public string ProjectCodeNo { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
        public Province Province { get; set; }
        public City City { get; set; }
        public Trade Trade { get; set; }
        public int Active { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public long Region { get; set; }
        public string RegionName { get; set; }
        public int EmployeeCount { get; set; }
        public static string Save(ProjectCode proj)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isProjectExist_Project",
                    new string[] { "pPRCode" },
                    new DbType[] { DbType.String },
                    new object[] { proj.ProjectCodeNo }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 0)
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_Project",
                    new string[] { "pPRCode", "pName", "pAddress", "pProvince", "pRegion", "pCity", "pOwner", "pActive", "pTrade", "pAddedBy", "pModifiedBy" },
                    new DbType[] { DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.Int64, DbType.String, DbType.Int32, DbType.Int64, DbType.String, DbType.String },
                    new object[] {  proj.ProjectCodeNo, proj.ProjectName, proj.Address, proj.Province.ID, proj.Region, proj.City.ID,
                                            proj.Owner, proj.Active,proj.Trade.ID, proj.AddedBy, proj.ModifiedBy }, out b, CommandType.StoredProcedure);
                    proj.Logs.After = "ID:" + proj.ID.ToString() + ", ProjectName:" + proj.ProjectName + ", Address:" + proj.Address + ",  Region:" + proj.Region + ",Province:" + proj.Province.ID + ", " +
                                                "City:" + proj.City + ", Owner:" + proj.Owner + ", Active:" + proj.Active + ", Trade:" + proj.Trade.ID.ToString() + "";
                    SystemLogs.SaveSystemLogs(proj.Logs);
                    res = "ok";
                }
                else
                {
                    res = "failed";
                }
            }
            return res;
        }

        public static void SaveFromUploader(ProjectCode proj)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;

                db.ExecuteCommandNonQuery("HRIS_Save_Project",
                new string[] { "pPRCode", "pName", "pAddress", "pProvince", "pCity", "pOwner", "pActive", "pTrade", "pAddedBy", "pModifiedBy" },
                new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.Int32, DbType.Int64, DbType.String, DbType.String },
                new object[] { proj.ProjectCodeNo, proj.ProjectName, proj.Address, proj.Province.ID, proj.City.ID, proj.Owner, proj.Active, proj.Trade.ID, proj.AddedBy, proj.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void Update(ProjectCode proj)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Project",
                   new string[] { "pID", "pPRCode", "pName", "pAddress", "pProvince", "pRegion", "pCity", "pOwner", "pActive", "pTrade", "pModifiedBy" },
                   new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32, DbType.Int64, DbType.String },
                   new object[] { proj.ID, proj.ProjectCodeNo, proj.ProjectName, proj.Address, proj.Province.ID, proj.Region, proj.City.ID, proj.Owner, proj.Active, proj.Trade.ID, proj.ModifiedBy }, out a, CommandType.StoredProcedure);
                proj.Logs.After = "ID:" + proj.ID.ToString() + ", ProjectName:" + proj.ProjectName + ", Address:" + proj.Address + ",  Region:" + proj.Region + ", Province:" + proj.Province.ID + ", " +
                                              "City:" + proj.City + ", Owner:" + proj.Owner + ", Active:" + proj.Active + ", Trade:" + proj.Trade.ID.ToString() + "";
                SystemLogs.SaveSystemLogs(proj.Logs);
            }
        }
        public static void Delete(ProjectCode proj)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Project",
                   new string[] { "pID", "pPRCode", "pModifiedBy" },
                   new DbType[] { DbType.Int64, DbType.String, DbType.String },
                   new object[] { proj.ID, proj.ProjectCodeNo, proj.ModifiedBy }, out a, CommandType.StoredProcedure);
                proj.Logs.After = "";
                SystemLogs.SaveSystemLogs(proj.Logs);
            }
        }
        public static List<ProjectCode> Get()
        {
            var list = new List<ProjectCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_Project",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ProjectCode p = new ProjectCode();
                    p.Province = new Province();
                    p.City = new City();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.City.Name = aRow["CityName"].ToString();
                    p.Province.Name = aRow["ProvinceName"].ToString();
                    p.ProjectCodeNo = aRow["ProjectCode"].ToString();
                    p.ProjectName = aRow["ProjectName"].ToString();
                    p.Address = aRow["Address"].ToString();
                    p.Owner = aRow["Owner"].ToString();
                    p.Region = Convert.ToInt64(aRow["Region"]);
                    p.RegionName = aRow["RegionName"].ToString();
                    if (aRow["Active"] != DBNull.Value)
                    {
                        p.Active = Convert.ToInt32(aRow["Active"]);
                    }
                    else
                    {
                        p.Active = 0;
                    }
                    p.Trade = new Trade();
                    if (aRow["Trade"] != DBNull.Value)
                    {
                        p.Trade = Trade.GetTradeByID(Convert.ToInt64(aRow["Trade"]));
                    }
                    else
                    {
                        p.Trade.ID = 0;
                    }
                    list.Add(p);
                }
            }
            return list;
        }
        public static List<ProjectCode> GetListByID(Int64 Id)
        {
            var list = new List<ProjectCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetListByID_Project",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ProjectCode p = new ProjectCode();
                    p.Province = new Province();
                    p.City = new City();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.City.ID = Convert.ToInt64(aRow["City"]);
                    p.Province.ID = Convert.ToInt64(aRow["Province"]);
                    p.City.Name = aRow["CityName"].ToString();
                    p.Province.Name = aRow["ProvinceName"].ToString();
                    p.ProjectCodeNo = aRow["ProjectCode"].ToString();
                    p.ProjectName = aRow["ProjectName"].ToString();
                    p.Address = aRow["Address"].ToString();
                    p.Owner = aRow["Owner"].ToString();
                    p.Active = Convert.ToInt32(aRow["Active"]);
                    p.Trade = new Trade();
                    p.Region = Convert.ToInt64(aRow["Region"]);
                    p.RegionName = aRow["RegionName"].ToString();
                    p.Trade = Trade.GetTradeByID(Convert.ToInt64(aRow["Trade"]));
                    list.Add(p);
                }
            }
            return list;
        }
        public static ProjectCode GetByID(Int64 pID)
        {
            var p = new ProjectCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Projects",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { pID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    p.Province = new Province();
                    p.City = new City();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.City.ID = Convert.ToInt64(aRow["City"]);
                    p.Province.ID = Convert.ToInt64(aRow["Province"]);
                    p.City.Name = aRow["CityName"].ToString();
                    p.Province.Name = aRow["ProvinceName"].ToString();
                    p.ProjectCodeNo = aRow["ProjectCode"].ToString();
                    p.ProjectName = aRow["ProjectName"].ToString();
                    p.Address = aRow["Address"].ToString();
                    p.Owner = aRow["Owner"].ToString();
                    p.Region = Convert.ToInt64(aRow["Region"]);
                    p.RegionName = aRow["RegionName"].ToString();
                    if (aRow["Active"] != DBNull.Value)
                    {
                        p.Active = Convert.ToInt32(aRow["Active"]);
                    }
                    else
                    {
                        p.Active = 0;
                    }
                    p.Trade = new Trade();
                    if (aRow["Trade"] != DBNull.Value)
                    {
                        p.Trade = Trade.GetTradeByID(Convert.ToInt64(aRow["Trade"]));
                    }
                    else
                    {
                        p.Trade.ID = 0;
                    }
                }
            }
            return p;
        }
    }

    public class SalesOrder
    {
        public long ID { get; set; }
        public string SOCode { get; set; }
        public ProjectCode Project { get; set; }
        public string ProjectName { get; set; }
        public string Trade { get; set; }
        public Division Division { get; set; }
        public string DivisionHead { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectIncharge { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static List<SalesOrder> GetProjectSalesOrders()
        {
            var list = new List<SalesOrder>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_ProjectSO",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    SalesOrder so = new SalesOrder();
                    so.Project = new ProjectCode();
                    so.Division = new Division();
                    so.Division.Head = new Employee();
                    so.ID = Convert.ToInt64(aRow["ID"]);
                    so.SOCode = aRow["SOCode"].ToString();
                    so.Project.ID = Convert.ToInt64(aRow["Project"]);
                    so.ProjectName = aRow["ProjectName"].ToString();
                    so.Trade = aRow["Trade"].ToString();
                    if (aRow["Division"] != DBNull.Value)
                    {
                        so.Division.ID = Convert.ToInt64(aRow["Division"]);
                    }
                    else
                    {
                        so.Division.ID = 0;
                    }
                    so.Division = Division.GetByID(so.Division.ID);
                    so.DivisionHead = aRow["DivisionHead"].ToString();
                    so.ProjectManager = aRow["ProjectManager"].ToString();
                    so.ProjectIncharge = aRow["ProjectIncharge"].ToString();
                    list.Add(so);
                }
            }
            return list;
        }
        public static List<SalesOrder> GetProjectSalesOrdersListByID(Int64 Id)
        {
            var list = new List<SalesOrder>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetListByID_SalesOrder",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    SalesOrder so = new SalesOrder();
                    so.Project = new ProjectCode();
                    so.Division = new Division();
                    so.Division.Head = new Employee();
                    so.ID = Convert.ToInt64(aRow["ID"]);
                    so.SOCode = aRow["SOCode"].ToString();
                    so.Project.ID = Convert.ToInt64(aRow["Project"]);
                    so.ProjectName = aRow["ProjectName"].ToString();
                    so.Trade = aRow["Trade"].ToString();
                    if (aRow["Division"] != DBNull.Value)
                    {
                        so.Division.ID = Convert.ToInt64(aRow["Division"]);
                    }
                    else
                    {
                        so.Division.ID = 0;
                    }
                    so.Division = Division.GetByID(so.Division.ID);
                    so.DivisionHead = aRow["DivisionHead"].ToString();
                    so.ProjectManager = aRow["ProjectManager"].ToString();
                    so.ProjectIncharge = aRow["ProjectIncharge"].ToString();
                    list.Add(so);
                }
            }
            return list;
        }
        public static SalesOrder GetSalesOrderByID(Int64 Id)
        {
            var so = new SalesOrder();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetById_ProjectSO",
                    new string[] { "sID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    so.Project = new ProjectCode();
                    so.Division = new Division();
                    so.Division.Head = new Employee();
                    so.ID = Convert.ToInt64(aRow["ID"]);
                    so.SOCode = aRow["SOCode"].ToString();
                    so.Project.ID = Convert.ToInt64(aRow["Project"]);
                    so.ProjectName = aRow["ProjectName"].ToString();
                    so.Trade = aRow["Trade"].ToString();
                    so.Project.ID = Convert.ToInt64(aRow["Project"]);
                    if (aRow["Division"] != DBNull.Value)
                    {
                        so.Division.ID = Convert.ToInt64(aRow["Division"]);
                    }
                    else
                    {
                        so.Division.ID = 0;
                    }
                    so.DivisionHead = aRow["DivisionHead"].ToString();
                    so.ProjectManager = aRow["ProjectManager"].ToString();
                    so.ProjectIncharge = aRow["ProjectIncharge"].ToString();
                }
            }
            return so;
        }
        public static string SaveSalesOrder(SalesOrder trade)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                long sID = 0;
                DataTable bTable = new DataTable();
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_checkIfSOCodeExist_ProjectSO",
                    new string[] { "sSOCode" },
                    new DbType[] { DbType.String },
                    new object[] { trade.SOCode }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Sales Order code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_Save_ProjectSO",
                       new string[] { "sSoCode", "sProject", "sTrade", "sDivision", "sDivisionHead", "sProjectManager", "sProjectInCharge", "sAddedBy", "sModifiedBy" },
                       new DbType[] { DbType.String, DbType.Int64, DbType.String, DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                       new object[] { trade.SOCode, trade.Project.ID, trade.Trade, trade.Division.ID, trade.DivisionHead, trade.ProjectManager, trade.ProjectIncharge, trade.AddedBy, trade.ModifiedBy }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        DataRow aRow = aTable.Rows[0];
                        sID = Convert.ToInt64(aRow["ID"]);
                        if (sID != 0)
                        {
                            result = "ok";
                        }
                    }
                    trade.Logs.After = " ID:" + trade.ID.ToString() + ", SOCode :" + trade.SOCode + ", Project::" + trade.Project.ID.ToString() + ", " +
                                                   "Trade:" + trade.Trade + ", Division:" + trade.Division.ID + ", DivisionHead:" + trade.DivisionHead + ", " +
                                                   "Project Manager:" + trade.ProjectManager + ", ProjectInCharge:" + trade.ProjectIncharge + "";
                    SystemLogs.SaveSystemLogs(trade.Logs);
                }
            }
            return result;
        }
        public static void SaveFromUploader(SalesOrder trade)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_ProjectSO",
                      new string[] { "sSoCode", "sProject", "sTrade", "sDivision", "sDivisionHead", "sProjectManager", "sProjectInCharge", "sAddedBy", "sModifiedBy" },
                      new DbType[] { DbType.String, DbType.Int64, DbType.String, DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                      new object[] { trade.SOCode, trade.Project.ID, trade.Trade, trade.Division.ID, trade.DivisionHead, trade.ProjectManager, trade.ProjectIncharge, trade.AddedBy, trade.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void EditSalesOrder(SalesOrder trade)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_ProjectSO",
                    new string[] { "sID", "sSoCode", "sProject", "sTrade", "sDivision", "sDivisionHead", "sProjectManager", "sProjectInCharge", "sModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64, DbType.String, DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                    new object[] { trade.ID, trade.SOCode, trade.Project.ID, trade.Trade, trade.Division.ID, trade.DivisionHead, trade.ProjectManager, trade.ProjectIncharge, trade.AddedBy, trade.ModifiedBy }, out a, CommandType.StoredProcedure);
                trade.Logs.After = " ID:" + trade.ID.ToString() + ", SOCode :" + trade.SOCode + ", Project::" + trade.Project.ID.ToString() + ", " +
                                                   "Trade:" + trade.Trade + ", Division:" + trade.Division.ID + ", DivisionHead:" + trade.DivisionHead + ", " +
                                                   "Project Manager:" + trade.ProjectManager + ", ProjectInCharge:" + trade.ProjectIncharge + "";
                SystemLogs.SaveSystemLogs(trade.Logs);
            }
        }
        public static void DeleteSalesOrder(SalesOrder trade)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_ProjectSO",
                    new string[] { "sID", "sSOCode", "sModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String },
                    new object[] { trade.ID, trade.SOCode, trade.ModifiedBy }, out a, CommandType.StoredProcedure);
                trade.Logs.After = "";
                SystemLogs.SaveSystemLogs(trade.Logs);
            }
        }
    }
    public class ContractStatus
    {
        public long ID { get; set; }
        public string R_Code { get; set; }
        public string Type { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static List<ContractStatus> GetContractStatuses()
        {
            var Statuses = new List<ContractStatus>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_ContractStatus",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ContractStatus c = new ContractStatus();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.R_Code = aRow["R_Code"].ToString();
                    c.Type = aRow["Type"].ToString();
                    Statuses.Add(c);
                }
            }
            return Statuses;
        }
        public static List<APIContractStatusJSON> APIContractStatus(string query)
        {
            var res = new List<APIContractStatusJSON>();
            using (AppDb db = new AppDb())
            {
                string q = query;
                if (query == null || query.Length < 3)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_API_ContractStatus",
                    new string[] { "q" },
                    new DbType[] { DbType.String },
                    new object[] { q }, out x, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    APIContractStatusJSON cs = new APIContractStatusJSON();
                    cs.Id = Convert.ToInt64(aRow["ID"]);
                    cs.Text = aRow["Type"].ToString();
                    res.Add(cs);
                }

            }
            return res;
        }
        public static ContractStatus GetContractStatusByID(Int64 Id)
        {
            var c = new ContractStatus();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETByID_ContractStatus",
                    new string[] { "cID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.R_Code = aRow["R_Code"].ToString();
                    c.Type = aRow["Type"].ToString();
                }
            }
            return c;
        }
        public static string SaveContractStatus(ContractStatus data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_ContractStatus",
                    new string[] { "cCode" },
                    new DbType[] { DbType.Int32 },
                    new object[] { data.R_Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Contract status code is already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_ContractStatus",
                    new string[] { "cDesc" },
                    new DbType[] { DbType.String },
                    new object[] { data.Type }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        result = "Contract status type is already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_ContractStatus",
                        new string[] { "cCode", "cType", "cAddedBy", "cModifiedBy" },
                        new DbType[] { DbType.Int32, DbType.String, DbType.String, DbType.String },
                        new object[] { data.R_Code, data.Type, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                        }
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code::" + data.R_Code + ", Type:" + data.Type + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
            }
            return result;
        }
        public static string EditContractStatus(ContractStatus data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_ContractStatus",
                  new string[] { "cID", "cCode" },
                  new DbType[] { DbType.Int64, DbType.Int32 },
                  new object[] { data.ID, data.R_Code }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_ContractStatus",
                      new string[] { "cID", "cDesc" },
                      new DbType[] { DbType.Int64, DbType.Int32 },
                      new object[] { data.ID, data.Type }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Desc in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_ContractStatus",
                        new string[] { "cID", "cCode", "cType", "cModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                        new object[] { data.ID, data.R_Code, data.Type, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code::" + data.R_Code + ", Type:" + data.Type + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void DeleteContractStatus(ContractStatus data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_ContractStatus",
                    new string[] { "cID", "cCode", "cModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.R_Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = " ";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
    }
    public class APIContractStatusJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }
    public class APIContractStatusObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<APIContractStatusJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class Trade_ED
    {
        public long ID { get; set; }
        public string Div_Code { get; set; }
        public string TradeCode { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static string SaveTrade_EDCode(Trade_ED data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Trade_ED",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.TradeCode }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Trade ED code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_SAVE_Trade_ED",
                     new string[] { "tDivCode", "tCode", "tDescription", "tAddedBy", "tModifiedBy" },
                     new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                     new object[] { data.Div_Code, data.TradeCode, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void EditTrade_EDCode(Trade_ED data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Trade_ED",
                    new string[] { "tID", "tDivCode", "tCode", "tDescription", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Div_Code, data.TradeCode, data.Description, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteTrade_ED(Trade_ED data)
        {

            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Trade_ED",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.TradeCode, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<Trade_ED> GetTrade_EDCodes()
        {
            var res = new List<Trade_ED>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Trade_ED",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Trade_ED t = new Trade_ED();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    res.Add(t);
                }
            }
            return res;
        }
        public static Trade_ED GetTrade_EDCodesByID(Int64 Id)
        {
            var t = new Trade_ED();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Trade_ED",
                    new string[] { "tID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class Trade_MPFD
    {
        public long ID { get; set; }
        public string Div_Code { get; set; }
        public string TradeCode { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static string SaveTrade_MPFDCode(Trade_MPFD data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Trade_MPFD",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.TradeCode }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Trade MPFD code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_SAVE_Trade_MPFD",
                     new string[] { "tDivCode", "tCode", "tDescription", "tAddedBy", "tModifiedBy" },
                     new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                     new object[] { data.Div_Code, data.TradeCode, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void EditTrade_MPFDCode(Trade_MPFD data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Trade_MPFD",
                    new string[] { "tID", "tDivCode", "tCode", "tDescription", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Div_Code, data.TradeCode, data.Description, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteTrade_MPFD(Trade_MPFD data)
        {

            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Trade_MPFD",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.TradeCode, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<Trade_MPFD> GetTrade_MPFDCodes()
        {
            var res = new List<Trade_MPFD>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Trade_MPFD",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Trade_MPFD t = new Trade_MPFD();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    res.Add(t);
                }
            }
            return res;
        }
        public static Trade_MPFD GetTrade_MPFDCodesByID(Int64 Id)
        {
            var t = new Trade_MPFD();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Trade_MPFD",
                    new string[] { "tID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class Trade_ACSD
    {
        public long ID { get; set; }
        public string Div_Code { get; set; }
        public string TradeCode { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static string SaveTrade_ACSDCode(Trade_ACSD data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Trade_ACSD",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.TradeCode }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Trade ACSD code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_SAVE_Trade_ACSD",
                     new string[] { "tDivCode", "tCode", "tDescription", "tAddedBy", "tModifiedBy" },
                     new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                     new object[] { data.Div_Code, data.TradeCode, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void EditTrade_ACSDCode(Trade_ACSD data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Trade_ACSD",
                    new string[] { "tID", "tDivCode", "tCode", "tDescription", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Div_Code, data.TradeCode, data.Description, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteTrade_ACSD(Trade_ACSD data)
        {

            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Trade_ACSD",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.TradeCode, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<Trade_ACSD> GetTrade_ACSDCodes()
        {
            var res = new List<Trade_ACSD>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Trade_ACSD",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Trade_ACSD t = new Trade_ACSD();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    res.Add(t);
                }
            }
            return res;
        }
        public static Trade_ACSD GetTrade_ACSDCodesByID(Int64 Id)
        {
            var t = new Trade_ACSD();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Trade_ACSD",
                    new string[] { "tID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class Trade_IRD
    {
        public long ID { get; set; }
        public string Div_Code { get; set; }
        public string TradeCode { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static string SaveTrade_IRDCode(Trade_IRD data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Trade_IRD",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.TradeCode }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Trade IRDcode already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_SAVE_Trade_IRD",
                     new string[] { "tDivCode", "tCode", "tDescription", "tAddedBy", "tModifiedBy" },
                     new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                     new object[] { data.Div_Code, data.TradeCode, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void EditTrade_IRDCode(Trade_IRD data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Trade_IRD",
                    new string[] { "tID", "tDivCode", "tCode", "tDescription", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Div_Code, data.TradeCode, data.Description, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteTrade_IRD(Trade_IRD data)
        {

            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Trade_IRD",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.TradeCode, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<Trade_IRD> GetTrade_IRDCodes()
        {
            var res = new List<Trade_IRD>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Trade_IRD",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Trade_IRD t = new Trade_IRD();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    res.Add(t);
                }
            }
            return res;
        }
        public static Trade_IRD GetTrade_IRDCodesByID(Int64 Id)
        {
            var t = new Trade_IRD();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Trade_IRD",
                    new string[] { "tID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Div_Code = aRow["Div_Code"].ToString();
                    t.TradeCode = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class CompanySetUp
    {
        public string CompanyName { get; set; }
        public string TIN { get; set; }
        public string SSS { get; set; }
        public string PAGIBIG { get; set; }
        public string PHILHEALTH { get; set; }
        public string BusinessType { get; set; }
        public string Industry { get; set; }
        public string Bank { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string FaxNumber { get; set; }
        public string President { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string BDOAccountNumber { get; set; }
        public string MTBAccountNumber { get; set; }
        public string BDOCompanyCode { get; set; }
        public string MTBCompanyCode { get; set; }
        public string BDOCompanyBranchCode { get; set; }
        public string MTBCompanyBranchCode { get; set; }
        public string RDO { get; set; }
        public string ZipCode { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SaveCompanySetUp(CompanySetUp data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_CompanySetUp",
                    new string[] {  "tCompanyName", "tTin", "tSSS","tPAGIBIG","tPHILHEALTH", "tBusinessType", "tIndustry", "tBank", "tAddress",
                                    "tContactNumber", "tFaxNumber", "tPresident", "tBDOAccountNumber","tBDOCompanyCode", "tBDOCompanyBranchCode",
                        "tMTBAccountNumber","tMTBCompanyCode","tMTBCompanyBranchCode","tAddedBy","tRDO","tZipCode","tModifiedBy" },
                    new DbType[] {  DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String,DbType.String,DbType.String, DbType.String,
                                    DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.String},
                    new object[] {  data.CompanyName, data.TIN, data.SSS, data.PAGIBIG, data.PHILHEALTH, data.BusinessType, data.Industry, data.Bank,
                                    data.Address, data.ContactNumber, data.FaxNumber, data.President, data.BDOAccountNumber,data.BDOCompanyCode,
                                    data.BDOCompanyBranchCode,data.MTBAccountNumber, data.MTBCompanyCode, data.MTBCompanyBranchCode, data.AddedBy,
                                    data.RDO, data.ZipCode, data.ModifiedBy}, out a, CommandType.StoredProcedure);
                data.Logs.After = "CompanyName:" + data.CompanyName + ", TIN:" + data.TIN + ", SSS:" + data.SSS + ", PAGIBIG:" + data.PAGIBIG + ", " +
                    " Philhealth:" + data.PHILHEALTH + ",  BusinessType:" + data.BusinessType + ",  Industry:" + data.Industry + ",  Bank:" + data.Bank + ", " +
                    " Address:" + data.Address + ",  ContactNumber:" + data.ContactNumber + ",  FaxNumber:" + data.FaxNumber + ",  President:" + data.President + ", " +
                    " AccountNumber:" + data.BDOAccountNumber + ",  BDOCompanyCode:" + data.BDOCompanyBranchCode + ",  AddedBy:" + data.AddedBy + ", " +
                    " RDIO:" + data.RDO + ",  :" + data.ZipCode + ",  ModifiedBy:" + data.ModifiedBy + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }

        public static CompanySetUp GetCompanySetUpData()
        {
            var c = new CompanySetUp();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_CompanySetUp",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.CompanyName = aRow["CompanyName"].ToString();
                    c.BusinessType = aRow["BusinessType"].ToString();
                    c.TIN = aRow["TIN"].ToString();
                    c.RDO = aRow["RDO"].ToString();
                    c.SSS = aRow["SSS"].ToString();
                    c.PAGIBIG = aRow["PAGIBIG"].ToString();
                    c.PHILHEALTH = aRow["Philhealth"].ToString();
                    c.Industry = aRow["Industry"].ToString();
                    c.Bank = aRow["Bank"].ToString();
                    c.Address = aRow["Address"].ToString();
                    c.ZipCode = aRow["ZipCode"].ToString();
                    c.ContactNumber = aRow["ContactNumber"].ToString();
                    c.FaxNumber = aRow["FaxNumber"].ToString();
                    c.President = aRow["President"].ToString();
                    c.BDOAccountNumber = aRow["BDOAccountNumber"].ToString();
                    c.BDOCompanyCode = aRow["BDOCompanyCode"].ToString();
                    c.BDOCompanyBranchCode = aRow["BDOCompanyBranchCode"].ToString();
                    c.MTBAccountNumber = aRow["MetroBankAccountNumber"].ToString();
                    c.MTBCompanyCode = aRow["MetrobankCompanyCode"].ToString();
                    c.MTBCompanyBranchCode = aRow["MetrobankCompanyBranchCode"].ToString();
                }
            }
            return c;
        }

    }
    public class Trade
    {
        public long ID { get; set; }
        public Division Division { get; set; }
        public string T_Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveTrade(Trade data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Trade",
                    new string[] { "tTcode" },
                    new DbType[] { DbType.String },
                    new object[] { data.T_Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Code in use.";
                }
                else
                {

                    db.ExecuteCommandNonQuery("HRIS_Save_Trade",
                        new string[] { "tDiv", "tTCode", "tDescription", "tAddedBy", "tModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { data.Division.ID, data.T_Code, data.Description, data.AddedBy, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                    data.Logs.After = "ID:" + data.ID.ToString() + ",  Division :" + data.Division.ID.ToString() + ", Code::" + data.T_Code + ", Description:" + data.Description + "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    result = "ok";
                }
            }
            return result;
        }
        public static void SaveFromUploader(Trade data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_Trade",
                       new string[] { "tDiv", "tTCode", "tDescription", "tAddedBy", "tModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                       new object[] { data.Division.ID, data.T_Code, data.Description, data.AddedBy, data.ModifiedBy }, out b, CommandType.StoredProcedure);
            }
        }

        public static string EditTrade(Trade data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_Trade",
                    new string[] { "tID", "tTcode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.T_Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Code in use.";
                }
                else
                {

                    db.ExecuteCommandNonQuery("HRIS_Edit_Trade",
                       new string[] { "tID", "tDiv", "tTCode", "tDescription", "tModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String },
                       new object[] { data.ID, data.Division.ID, data.T_Code, data.Description, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                    data.Logs.After = "ID:" + data.ID.ToString() + ",  Division :" + data.Division.ID.ToString() + ", Code::" + data.T_Code + ", Description:" + data.Description + "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    result = "ok";
                }
            }
            return result;
        }
        public static void DeleteTrade(Trade Data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Trade",
                    new string[] { "tID", "tTCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String },
                    new object[] { Data.ID, Data.T_Code, Data.ModifiedBy }, out a, CommandType.StoredProcedure);
                Data.Logs.After = "";
                SystemLogs.SaveSystemLogs(Data.Logs);
            }
        }
        public static List<Trade> GetTrade()
        {
            var _trade = new List<Trade>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Trade",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Trade t = new Trade();
                    t.Division = new Division();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Division.Code = aRow["Code"].ToString();
                    t.T_Code = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    _trade.Add(t);
                }
            }
            return _trade;
        }
        public static Trade GetTradeByID(Int64 Id)
        {
            var t = new Trade();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Trade",
                    new string[] { "tID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.Division = new Division();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Division.ID = Convert.ToInt64(aRow["division"]);
                    t.T_Code = aRow["T_Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class ImmediateSuperior
    {
        public Employee Employee { get; set; }
        public Employee Superior { get; set; }
        public Department Department { get; set; }
        public EmployeeClassCode Class { get; set; }
        public List<ImmediateSuperior> AssignedSuperior { get; set; }
        public SystemLogs Logs { get; set; }
        public CompanyInfo Company { get; set; }
        public BranchEmployees BranchEmployees { get; set; }
        public Branches Branch { get; set; }
        public int Level { get; set; }
        public long IHeadId { get; set; }// receives immediate head id from the query result targeted from the table HRIS_ImmediateHeads

        public static List<ImmediateSuperior> GetEmployeesWithOutImmediateSuperiors_Departments()
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeesWithoutImmediateHeadDepartment_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Employee = new Employee();
                    i.Employee.Position = new PositionCode();
                    i.Employee.Class = new EmployeeClassCode();
                    i.Company = new CompanyInfo();
                    i.Branch = new Branches();
                    i.Employee.Department = new Department();
                    i.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    i.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    i.Employee.Firstname = aRow["FirstName"].ToString();
                    i.Employee.Middlename = aRow["MiddleName"].ToString();
                    i.Employee.Lastname = aRow["LastName"].ToString();
                    i.Employee.Suffix = aRow["Suffix"].ToString();
                    i.Employee.Position.Position = aRow["Position"].ToString();
                    i.Employee.Class.ID = Convert.ToInt64(aRow["ClassId"]);
                    i.Employee.Class.Description = aRow["Class"].ToString();
                    i.Employee.Department.ID = Convert.ToInt64(aRow["DepartmentId"]);
                    i.Employee.Department.Description = aRow["Department"].ToString();
                    i.Branch.BranchCode = aRow["BranchCode"].ToString();
                    i.Company.Name = aRow["CompanyName"].ToString();
                    i.Branch.BranchName = aRow["BranchName"].ToString();
                    _list.Add(i);
                }

            }
            return _list;
        }
        public static List<ImmediateSuperior> GetEmployeesWithImmediateSuperiors_Departments()
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeesWithImmediateHeadDepartment_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Employee = new Employee();
                    i.Employee.Position = new PositionCode();
                    i.Employee.Class = new EmployeeClassCode();
                    i.Employee.Department = new Department();
                    i.Company = new CompanyInfo();
                    i.Branch = new Branches();
                    i.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    i.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    i.Employee.Firstname = aRow["FirstName"].ToString();
                    i.Employee.Middlename = aRow["MiddleName"].ToString();
                    i.Employee.Lastname = aRow["LastName"].ToString();
                    i.Employee.Suffix = aRow["Suffix"].ToString();
                    i.Employee.Position.Position = aRow["Position"].ToString();
                    i.Employee.Class.ID = Convert.ToInt64(aRow["ClassId"]);
                    i.Employee.Class.Description = aRow["Class"].ToString();
                    i.Employee.Department.ID = Convert.ToInt64(aRow["DepartmentId"]);
                    i.Employee.Department.Description = aRow["Department"].ToString();
                    i.AssignedSuperior = new List<ImmediateSuperior>();
                    i.AssignedSuperior = ImmediateSuperior.GetAssignedSuperiors(i.Employee.ID);
                    i.Branch.BranchCode = aRow["BranchCode"].ToString();
                    i.Company.Name = aRow["CompanyName"].ToString();
                    i.Branch.BranchName = aRow["BranchName"].ToString();
                    _list.Add(i);
                }

            }
            return _list;
        }
        public static ImmediateSuperior GetEmployeesWithImmediateSuperiorsByEmpId(Int64 EmpId)
        {
            var i = new ImmediateSuperior();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmpWithSupById_Employee_ImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    i.Employee = new Employee();
                    i.Employee.Position = new PositionCode();
                    i.Employee.Class = new EmployeeClassCode();
                    i.Employee.Department = new Department();
                    i.Employee.Division = new Division();
                    i.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    i.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    i.Employee.Firstname = aRow["FirstName"].ToString();
                    i.Employee.Middlename = aRow["MiddleName"].ToString();
                    i.Employee.Lastname = aRow["LastName"].ToString();
                    i.Employee.Suffix = aRow["Suffix"].ToString();
                    i.Employee.Position.Position = aRow["Position"].ToString();
                    if (aRow["ClassId"] != DBNull.Value)
                    {
                        i.Employee.Class.ID = Convert.ToInt64(aRow["ClassId"]);
                    }
                    i.Employee.Class.Description = aRow["Class"].ToString();
                    i.Employee.Department.ID = Convert.ToInt64(aRow["DepartmentId"]);
                    i.Employee.Department.Description = aRow["Department"].ToString();
                    i.AssignedSuperior = new List<ImmediateSuperior>();
                    i.AssignedSuperior = ImmediateSuperior.GetAssignedSuperiors(i.Employee.ID);

                }
            }
            return i;
        }
        public static List<ImmediateSuperior> GetEmployeesWithOutImmediateSuperiors_Division()
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeesWithoutImmediateHeadDivision_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Employee = new Employee();
                    i.Employee.Position = new PositionCode();
                    i.Employee.Class = new EmployeeClassCode();
                    i.Employee.Division = new Division();
                    i.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    i.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    i.Employee.Firstname = aRow["FirstName"].ToString();
                    i.Employee.Middlename = aRow["MiddleName"].ToString();
                    i.Employee.Lastname = aRow["LastName"].ToString();
                    i.Employee.Suffix = aRow["Suffix"].ToString();
                    i.Employee.Position.Position = aRow["Position"].ToString();
                    i.Employee.Class.ID = Convert.ToInt64(aRow["ClassId"]);
                    i.Employee.Class.Description = aRow["Class"].ToString();
                    i.Employee.Division.ID = Convert.ToInt64(aRow["DivisionId"]);
                    i.Employee.Division.Div_Desc = aRow["Division"].ToString();

                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetEmployeesWithImmediateSuperiors_Division()
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeesWithImmediateHeadDivision_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Employee = new Employee();
                    i.Employee.Position = new PositionCode();
                    i.Employee.Class = new EmployeeClassCode();
                    i.Employee.Division = new Division();
                    i.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    i.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    i.Employee.Firstname = aRow["FirstName"].ToString();
                    i.Employee.Middlename = aRow["MiddleName"].ToString();
                    i.Employee.Lastname = aRow["LastName"].ToString();
                    i.Employee.Suffix = aRow["Suffix"].ToString();
                    i.Employee.Position.Position = aRow["Position"].ToString();
                    i.Employee.Class.ID = Convert.ToInt64(aRow["ClassId"]);
                    i.Employee.Class.Description = aRow["Class"].ToString();
                    i.Employee.Division.ID = Convert.ToInt64(aRow["DivisionId"]);
                    i.Employee.Division.Div_Desc = aRow["Division"].ToString();
                    i.AssignedSuperior = new List<ImmediateSuperior>();
                    i.AssignedSuperior = ImmediateSuperior.GetAssignedSuperiors(i.Employee.ID);
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetPossibleSupervisors_Department(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_API_ImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Department = new Department();
                    i.Superior.Position = new PositionCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentId"]));
                    i.Superior.Division = Division.GetByID(Convert.ToInt64(aRow["DivisionId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetPossibleSupervisors_Division(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_API_ImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Division = new Division();
                    i.Superior.Position = new PositionCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    i.Superior.Division = Division.GetByID(Convert.ToInt64(aRow["DivisionId"]));
                    i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetPossibleManagers_Department(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetPossibleManagers_Employee_DepartmentImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Department = new Department();
                    i.Superior.Position = new PositionCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetPossibleManagers_Division(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetPossibleManagers_Employee_DivisionImmediateHeads",
                    new string[] { "eEmpId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Division = new Division();
                    i.Superior.Position = new PositionCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    i.Superior.Division = Division.GetByID(Convert.ToInt64(aRow["DivisionId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetPossibleExecutiveHeads(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetPossibleExecutive_Employee_ImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Department = new Department();
                    i.Superior.Position = new PositionCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetPresidentAsImmediateHead(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetPPresident_Employee_ImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Department = new Department();
                    i.Superior.Position = new PositionCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    //i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static void SaveEmployeeWithImmediteSuperior(ImmediateSuperior data, long[] SupId, int[] SupLevel)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                long[] _items = SupId;
                int _counter = 0;
                string _after = "";
                foreach (long x in _items)
                {
                    // data.Superior = new Employee();
                    long _spID = x;
                    data.Level = SupLevel[_counter];
                    //data.Superior.ID =SupId[_counter];
                    db.ExecuteCommandNonQuery("HRIS_Save_Employee_ImmediateHeads",
                      new string[] { "eEmployeeId", "eSuperiorId", "eLevel" },
                      new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32 },
                      new object[] { data.Employee.ID, _spID, data.Level }, out a, CommandType.StoredProcedure);
                    _after += "[EmployeeID:" + data.Employee.ID.ToString() + ", SuperiorID:" + _spID + " Level:" + data.Level + "],";
                    _counter += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void AddImmediteSuperiorByUploader(ImmediateSuperior data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;

                db.ExecuteCommandNonQuery("HRIS_Save_Employee_ImmediateHeads",
                  new string[] { "eEmployeeId", "eSuperiorId", "eLevel" },
                  new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32 },
                  new object[] { data.Employee.ID, data.Superior.ID, data.Level }, out a, CommandType.StoredProcedure);

            }
        }
        public static void DeleteEmployeeWithImmediteSuperior(ImmediateSuperior data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Employee_ImmediateHeads",
                    new string[] { "eEmployeeId", "eSuperiorId", "eLevel" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32 },
                    new object[] { data.Employee.ID, data.Superior.ID, data.Level }, out a, CommandType.StoredProcedure);
                string _after = "";
                var ep = ImmediateSuperior.GetAssignedSuperiors(data.Employee.ID);
                if (ep != null)
                {
                    foreach (var e in ep)
                    {
                        _after += "[EmployeeID:" + data.Employee.ID.ToString() + ", SuperiorID:" + e.Superior.ID + "], Level:" + e.Level + "";
                    }
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<ImmediateSuperior> GetByEmpId_ImmediateHeads(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeesWithoutImmediateHeadDepartment_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Employee = new Employee();
                    i.Employee.Position = new PositionCode();
                    i.Employee.Class = new EmployeeClassCode();
                    i.Employee.Department = new Department();
                    i.Employee.Division = new Division();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Division = new Division();
                    i.Superior.Department = new Department();
                    i.Superior.Position = new PositionCode();

                    i.Superior.Firstname = aRow["SuperiorFirstName"].ToString();
                    i.Superior.Middlename = aRow["SuperiorMiddleName"].ToString();
                    i.Superior.Lastname = aRow["SuperiorLastName"].ToString();
                    i.Superior.Suffix = aRow["SuperiorSuffix"].ToString();
                    i.Superior.Position.Position = aRow["SuperiorPosition"].ToString();
                    i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["SuperiorDepartment"]));
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["SuperiorClass"]));
                    i.Superior.Division = Division.GetByID(Convert.ToInt64(aRow["SuperiorDivision"]));
                    i.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    i.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    i.Employee.Firstname = aRow["FirstName"].ToString();
                    i.Employee.Middlename = aRow["MiddleName"].ToString();
                    i.Employee.Lastname = aRow["LastName"].ToString();
                    i.Employee.Suffix = aRow["Suffix"].ToString();
                    i.Employee.Position.Position = aRow["Position"].ToString();
                    i.Employee.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["Class"]));
                    i.Employee.Class.Description = aRow["Class"].ToString();
                    i.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    i.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    _list.Add(i);
                }

            }
            return _list;
        }
        public static List<ImmediateSuperior> GetAssignedSuperiors(Int64 EmpId)
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAssignedSupperiorsByEmp_ImmediateHeads",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.Department = new Department();
                    i.Superior.Position = new PositionCode();
                    i.IHeadId = Convert.ToInt64(aRow["HeadId"]);
                    i.Level = Convert.ToInt32(aRow["Level"]);
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Position.Position = aRow["Position"].ToString();
                    i.Superior.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassId"]));
                    i.Superior.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentId"]));
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static List<ImmediateSuperior> GetReceiverSuperiors()
        {
            var _list = new List<ImmediateSuperior>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetHeadsEmail_ImmediateHeads",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    ImmediateSuperior i = new ImmediateSuperior();
                    i.Superior = new Employee();
                    i.Superior.Personal = new EmployeePersonal();
                    i.Superior.Class = new EmployeeClassCode();
                    i.Superior.ID = Convert.ToInt64(aRow["Id"]);
                    i.Superior.Firstname = aRow["FirstName"].ToString();
                    i.Superior.Middlename = aRow["MiddleName"].ToString();
                    i.Superior.Lastname = aRow["LastName"].ToString();
                    i.Superior.Suffix = aRow["Suffix"].ToString();
                    i.Superior.Personal.Email = aRow["ToEmail"].ToString();
                    _list.Add(i);
                }
            }
            return _list;
        }
        public static ImmediateSuperior GetSuperiorBySupID(Int64 SupId, Int64 EmpId)
        {
            var i = new ImmediateSuperior();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetBySuperiorID_ImmediateHeads",
                    new string[] { "eSupId", "eEmpId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { SupId, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    i.Level = Convert.ToInt32(aRow["Level"]);
                }

            }
            return i;
        }
    }

}

