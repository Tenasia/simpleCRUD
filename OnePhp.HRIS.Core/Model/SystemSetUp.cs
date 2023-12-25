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
    public class SystemSetUp
    {
        public SystemSetUp()
        {
        }



    }
    public class JobLevel
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        //-Begin Job Level --
        //
        public static string SaveJobLevel(JobLevel Data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("BETA_isCodeExist_JobLevel",
                    new string[] { "jCode" },
                    new DbType[] { DbType.String},
                    new object[] { Data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Job level code in use.";
                    
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExist_JobLevel",
                        new string[] { "jDesc" },
                        new DbType[] { DbType.String },
                        new object[] { Data.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count == 1)
                    {
                        res = "Job level description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_JobLevel",
                        new string[] { "jCode", "jDescription", "jAddedBy", "jModifiedBy" },
                        new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { Data.Code, Data.Description, Data.AddedBy, Data.ModifiedBy }, out b, CommandType.StoredProcedure);
                        Data.Logs.After = "ID:"+Data.ID.ToString()+ ", Code:" + Data.Code + ", Description:" + Data.Description + "";
                        SystemLogs.SaveSystemLogs(Data.Logs);
                        res = "Job level save successfully!";
                    }
                }
            }
            return res;
        }
        public static string UpdateJobLevel(JobLevel data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable cTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("BETA_isCodeExist_Update_JobLevel",
                    new string[] { "jID", "jCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out c, ref cTable, CommandType.StoredProcedure);
                if (cTable.Rows.Count == 1)
                {
                    res = "Job level code in use.";

                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExist_Upadate_JobLevel",
                    new string[] { "jID", "jDesc" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        res = "Desc in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_JobLevel",
                           new string[] { "jId", "jCode", "jDescription", "jModifiedBy" },
                           new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                           new object[] { data.ID, data.Code, data.Description, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                        res = "Updated Successfully.";
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
                
            }
            return res;
        }
        public static void DeleteJobLevel(JobLevel data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_JobLevel",
                    new string[] { "jId ", "jCode", "jModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String },
                    new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<JobLevel> Get()
        {
            var res = new List<JobLevel>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_JobLevel",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in aTable.Rows)
                {
                    JobLevel j = new JobLevel();
                    j.ID = Convert.ToInt64(oRow["ID"]);
                    j.Code = oRow["Code"].ToString();
                    j.Description = oRow["Description"].ToString();
                    res.Add(j);
                }
            }
            return res;
        }
        public static JobLevel GetByID(Int64 Id)
        {
            var j = new JobLevel();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_JobLevel",
                    new string[] { "jId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow oRow = aTable.Rows[0];
                    j.ID = Convert.ToInt64(oRow["ID"]);
                    j.Code = j.Code = oRow["Code"].ToString();
                    j.Description = oRow["Description"].ToString();
                }
            }
            return j;
        }
        //-End Job Level --//
    }
    public class PositionCode
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Position { get; set; }
        public string JobDescription { get; set; }
        public JobLevel JobLevel { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string Save(PositionCode data)
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
                db.ExecuteCommandReader("BETA_isCodeExist_Poscode",
                    new string[] { "pCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Position code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isPosExist_Poscode",
                       new string[] { "pPosition" },
                       new DbType[] { DbType.String },
                       new object[] { data.Position }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Position name aready exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_Poscode",
                            new string[] { "pCode", "pPosition", "pJobDesc","pJobLevel", "pAddedBy", "pModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.String, DbType.String, },
                            new object[] { data.Code, data.Position, data.JobDescription,data.JobLevel.ID, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);

                        res = "Save Successfully.";
                        data.Logs.After = "ID:"+data.ID.ToString()+ ", Code:" + data.Code+ ", Position:" + data.Position+ ", JobDescription:" + data.JobDescription + ", " +
                                                    "JobLevel:" + data.JobLevel.ID+ "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
            }
            return res;
        }
        //Use this function for console application
        public static void SaveDetailsFromUpload(PositionCode data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Save_Poscode",
                           new string[] { "pCode", "pPosition", "pJobDesc","pJobLevel", "pAddedBy", "pModifiedBy" },
                           new DbType[] { DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.String, DbType.String, },
                           new object[] { data.Code, data.Position, data.JobDescription,data.JobLevel.ID, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static string Update(PositionCode data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int c = 0;
                int a = 0;
                int z = 0;
                DataTable aTable = new DataTable();
                DataTable zTable = new DataTable();
                //check first if code is in use/alredy exist in the db where the ID is not equal to the given ID
                db.ExecuteCommandReader("BETA_isCodeExist__Update_Poscode",
                   new string[] { "pID", "pCode" },
                   new DbType[] { DbType.Int64, DbType.String },
                   new object[] { data.ID, data.Code }, out z, ref zTable, CommandType.StoredProcedure);
                if (zTable.Rows.Count > 0)
                {
                    res = "Code in use.";
                    //return true
                }
                else
                {
                    //Then, check if position description is in use/alredy exist in the db where the ID is not equal to the given ID
                    db.ExecuteCommandReader("BETA_isPosExist_Update_Poscode",
                    new string[] { "pID", "pPos" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Position }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        res = "Desc In use.";
                        //return true
                    }
                    else
                    {
                        //if the above conditions return false, edit the data 
                        db.ExecuteCommandNonQuery("BETA_Edit_Poscode",
                                new string[] { "pID", "pCode","pPosition", "pJobDesc", "pJobLevel", "pAddedBy", "pModifiedBy" },
                                new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.Int64, DbType.String, DbType.String, DbType.String, },
                                new object[] { data.ID, data.Code, data.Position, data.JobDescription,data.JobLevel.ID, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);

                        res = "Updated successfully.";
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Position:" + data.Position + ", JobDescription:" + data.JobDescription + ", " +
                                                    "JobLevel:" + data.JobLevel.ID + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        //return true
                    }
                }
            }
            return res;
        }
        public static void Delete(PositionCode data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_Poscode",
                    new string[] { "pID", "pCode", "pModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<PositionCode> Get()
        {
            var res = new List<PositionCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_Poscode",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PositionCode p = new PositionCode();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Code = aRow["Code"].ToString();
                    p.Position = aRow["Position"].ToString();
                    p.JobDescription = aRow["JobDescription"].ToString();
                    p.JobLevel = new JobLevel();
                    p.JobLevel = JobLevel.GetByID(Convert.ToInt64(aRow["JobLevel"]));
                    res.Add(p);
                }
            }
            return res;
        }
        public static List<PositionCode> GetListByID(Int64 Id)
        {
            var res = new List<PositionCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetListByID_Position",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PositionCode p = new PositionCode();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Code = aRow["Code"].ToString();
                    p.Position = aRow["Position"].ToString();
                    p.JobDescription = aRow["JobDescription"].ToString();
                    p.JobLevel = new JobLevel();
                    p.JobLevel = JobLevel.GetByID(Convert.ToInt64(aRow["JobLevel"]));
                    res.Add(p);
                }
            }
            return res;
        }
        public static PositionCode GetByID (Int64 pId )
        {
            var p = new PositionCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_Poscode",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { pId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Code = aRow["Code"].ToString();
                    p.Position = aRow["Position"].ToString();
                    p.JobLevel = new JobLevel();
                    p.JobLevel = JobLevel.GetByID(Convert.ToInt64(aRow["JobLevel"]));
                    p.JobDescription = aRow["JobDescription"].ToString();
                }
            }
            return p;
        }
    }
    public class TaxCode
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string Save(TaxCode tax)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("BETA_isCodeExist_TaxCode",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { tax.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Tax code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isTaxDescExist_TaxCode",
                  new string[] { "tDescription" },
                  new DbType[] { DbType.String },
                  new object[] { tax.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Tax description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_TaxCode",
                            new string[] { "tCode", "tDescription", "tAddedBy", "tModifiedBy" },
                            new DbType[] {  DbType.String, DbType.String, DbType.String, DbType.String },
                            new object[] { tax.Code,tax.Description,tax.AddedBy,tax.ModifiedBy }, out c, CommandType.StoredProcedure);
                        res = "Tax code save successfully.";
                        tax.Logs.After = "ID:"+tax.ID.ToString()+ ", Code:" + tax.Code + ", Description:" + tax.Description + ", ";
                        SystemLogs.SaveSystemLogs(tax.Logs);
                    }
                }
            }
            return res;
        }
        public static string Update(TaxCode tax)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int z = 0;
                DataTable bTable = new DataTable();
                DataTable zTable = new DataTable();
                db.ExecuteCommandReader("BETA_isCodeExist__Update_TaxCode",
                    new string[] { "tID", "tCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { tax.ID, tax.Code }, out z, ref zTable, CommandType.StoredProcedure);
                if (zTable.Rows.Count == 1)
                {
                    res = "Tax code in use.";
                }
                else
                {
                    //Check if description is in use before updating
                    //to avoid duplicate description
                    db.ExecuteCommandReader("BETA_isDescInUse_TaxCode",
                        new string[] { "tID", "tDesc" },
                        new DbType[] { DbType.Int64, DbType.String },
                        new object[] { tax.ID, tax.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        res = "This description is currently in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_TaxCode",
                        new string[] { "tID", "tCode", "tDescription", "tModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                        new object[] { tax.ID, tax.Code, tax.Description, tax.ModifiedBy }, out a, CommandType.StoredProcedure);
                        res = "Updated successfully";
                        tax.Logs.After = "ID:" + tax.ID.ToString() + ", Code:" + tax.Code + ", Description:" + tax.Description + ", ";
                        SystemLogs.SaveSystemLogs(tax.Logs);
                    }

                }
            }
            return res;
        }
        public static void Delete(TaxCode tax)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_Code",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String },
                    new object[] { tax.ID, tax.Code, tax.ModifiedBy }, out a, CommandType.StoredProcedure);
                tax.Logs.After = "";
                SystemLogs.SaveSystemLogs(tax.Logs);
            }
        }
        public static List<TaxCode> Get()
        {
            var res = new List<TaxCode>();
            using (AppDb db= new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GET_TaxCode",
                    new string[] { },
                    new DbType[] { },
                    new object[] { },out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TaxCode t =new TaxCode();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    res.Add(t);
                }
            }
            return res;
        }
        public static TaxCode GetByID(Int64 Id)
        {
            var t = new TaxCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_TaxCode",
                    new string[] { "tID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id },out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class EmployeeClassCode
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string Save(EmployeeClassCode emp)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("BETA_isEmpCodeExist_EmpCode",
                    new string[] { "eCode" },
                    new DbType[] { DbType.String },
                    new object[] { emp.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Employee class code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isEmpDescExist_EmpCode",
                    new string[] { "eDesc" },
                    new DbType[] { DbType.String },
                    new object[] { emp.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Employee class description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_EmpCode",
                            new string[] { "eCode", "eDesc", "eAddedBy", "eModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                            new object[] { emp.Code, emp.Description, emp.AddedBy, emp.ModifiedBy }, out c, CommandType.StoredProcedure);
                        emp.Logs.After = "ID:"+emp.ID.ToString()+ ", Code:" + emp.Code+ ",Description:" + emp.Description+ "";
                        SystemLogs.SaveSystemLogs(emp.Logs);
                        res = "Save Successfully.";
                    }
                }
            }
            return res;
        }
        public static string Update(EmployeeClassCode emp)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                //check first if code is already in use by other description
                db.ExecuteCommandReader("BETA_isEmpCodeExist_Update_EmpCode",
                    new string[] { "eID","eCode" },
                    new DbType[] { DbType.Int64,DbType.String },
                    new object[] { emp.ID,emp.Code }, out c, ref cTable, CommandType.StoredProcedure);
                if (cTable.Rows.Count == 1)
                {
                    res = "Employee class code in use.";
                    //return true
                }
                else
                {
                   //if first statement returns false, check if description is in use by other code in the db table
                    db.ExecuteCommandReader("BETA_isEmpDescExist_Update_EmpCode",
                    new string[] { "eID", "eDesc" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { emp.ID, emp.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        res = "In use.";
                        //return true
                    }
                    else
                    {
                        //if the second statement return false, edit the data
                        db.ExecuteCommandNonQuery("BETA_Edit_EmpCode",
                        new string[] { "eID", "eCode", "eDesc", "eModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, },
                        new object[] { emp.ID, emp.Code, emp.Description, emp.ModifiedBy }, out a, CommandType.StoredProcedure);
                        res = "Updated Successfully.";
                        emp.Logs.After = "ID:" + emp.ID.ToString() + ", Code:" + emp.Code + ",Description:" + emp.Description + "";
                        SystemLogs.SaveSystemLogs(emp.Logs);
                    }
                }
                
            }
            return res;
        }
        public static void Delete(EmployeeClassCode emp)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_EmpCode",
                    new string[] { "eID", "eCode", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String },
                    new object[] { emp.ID, emp.Code,emp.ModifiedBy }, out a, CommandType.StoredProcedure);
                emp.Logs.After = "";
                SystemLogs.SaveSystemLogs(emp.Logs);
            }
        }
        public static List<EmployeeClassCode> Get()
        {
            var res = new List<EmployeeClassCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_EmpCode",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeClassCode e = new EmployeeClassCode();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Code = aRow["Code"].ToString();
                    e.Description = aRow["Description"].ToString();
                    res.Add(e);
                }
            }
            return res;
        }
        public static EmployeeClassCode GetByID(Int64 eId)
        {
            var e = new EmployeeClassCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_EmpCode",
                   new string[] { "eId" },
                   new DbType[] { DbType.Int64 },
                   new object[] { eId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Code = aRow["Code"].ToString();
                    e.Description = aRow["Description"].ToString();
                }
            }
            return e;
        }
    }
    public class EmployeeTypeCode
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string Save(EmployeeTypeCode type)
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
                db.ExecuteCommandReader("BETA_isCodeExist_EmpTypeCode",
                    new string[] { "eCode" },
                    new DbType[] { DbType.String },
                    new object[] { type.Code },out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Employee type code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExist_EmpTypeCode",
                       new string[] { "eDesc" },
                       new DbType[] { DbType.String },
                       new object[] { type.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Employee type description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_EMPTypeCode",
                            new string[] { "eCode","eDesc","eAddedBy","eModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                            new object[] { type.Code, type.Description,type.AddedBy,type.ModifiedBy}, out c, CommandType.StoredProcedure);
                        type.Logs.After = "ID:"+type.ID.ToString()+ ", Code:" + type.Code+ ", Description:" + type.Description+ "";
                        SystemLogs.SaveSystemLogs(type.Logs) ;
                        res = "Save Successfully.";
                    }
                }
            }
            return res;
        }
        public static string Update(EmployeeTypeCode type)
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
                db.ExecuteCommandReader("BETA_isCodeExist_Update_EmpTypeCode ",
                    new string[] { "eCode","eID" },
                    new DbType[] { DbType.String, DbType.Int64 },
                    new object[] { type.Code, type.ID }, out z, ref zTable, CommandType.StoredProcedure);
                if (zTable.Rows.Count == 1)
                {
                    res = "Employee type code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescInUse_EMPTypeCode",
                   new string[] { "eID", "eDesc" },
                   new DbType[] { DbType.Int64, DbType.String },
                   new object[] { type.ID, type.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        res = "In use.";
                    }
                    else 
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_EMPTypeCode",
                            new string[] { "eID", "eCode", "eDesc", "eModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                            new object[] { type.ID, type.Code, type.Description, type.ModifiedBy }, out b, CommandType.StoredProcedure);
                        type.Logs.After = "ID:" + type.ID.ToString() + ", Code:" + type.Code + ", Description:" + type.Description + "";
                        SystemLogs.SaveSystemLogs(type.Logs);
                        res = "Updated successfully.";
                    }
                }
            }
            return res;
        }
        public static void Delete(EmployeeTypeCode type)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_EMPTypeCode",
                    new string[] { "eID","eCode" },
                    new DbType[] { DbType.Int64, DbType.Int32 },
                    new object[] { type.ID, type.Code }, out a, CommandType.StoredProcedure);
                type.Logs.After = "";
                SystemLogs.SaveSystemLogs(type.Logs);
            }
        }
        public static List<EmployeeTypeCode> Get()
        {
            var res = new List<EmployeeTypeCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_EMPTypeCode",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeTypeCode e = new EmployeeTypeCode();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Code = aRow["Code"].ToString();
                    e.Description = aRow["Description"].ToString();
                    res.Add(e);
                }
            }
            return res;
        }
        public static EmployeeTypeCode GetByID(Int64 Id)
        {
            var e = new EmployeeTypeCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_EMPTypeCode",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64  },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Code = aRow["Code"].ToString();
                    e.Description = aRow["Description"].ToString();
                }
            }
            return e;
        }
    }
    public class PayCode //Pay Mode 
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string Save(PayCode pay)
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
                db.ExecuteCommandReader("BETA_isCodeExist_PayCode",
                    new string[] { "pCode" },
                    new DbType[] { DbType.String },
                    new object[] { pay.Code  },out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Pay mode code is in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isPayDescExist_PayCode",
                      new string[] { "pDesc" },
                      new DbType[] { DbType.Int64, DbType.String },
                      new object[] { pay.ID, pay.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Pay mode description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_PayCode",
                            new string[] { "pCode","pDesc","pAddedBy","pModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                            new object[] { pay.Code, pay.Description,pay.AddedBy,pay.ModifiedBy },out c, CommandType.StoredProcedure);
                        pay.Logs.After = "ID:"+pay.ID.ToString()+ ", Code:" + pay.Code+ ", Description:" + pay.Description+ ", ";
                        SystemLogs.SaveSystemLogs(pay.Logs);
                        res = "Save successfully.";
                    }
                }
            }
            return res;
        }
        public static string Update(PayCode pay)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("BETA_isCodeExist_Update_PayCode",
                    new string[] { "pID","pCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { pay.ID, pay.Code }, out c, ref cTable, CommandType.StoredProcedure);
                if (cTable.Rows.Count == 1)
                {
                    res = "Pay mode code is in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescInUse_PayCode",
                    new string[] { "pID", "pDesc" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { pay.ID, pay.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        res = "In use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_BETAPaycode",
                            new string[] { "pID", "pCode", "pDesc", "pModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                            new object[] { pay.ID, pay.Code, pay.Description, pay.ModifiedBy }, out b, CommandType.StoredProcedure);
                        pay.Logs.After = "ID:" + pay.ID.ToString() + ", Code:" + pay.Code + ", Description:" + pay.Description + "";
                        SystemLogs.SaveSystemLogs(pay.Logs);
                        res = "Updated successfully.";
                    }
                }
            }
            return res;
        }
        public static void Delete(PayCode pay)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_PayCode",
                    new string[] { "pID", "pCode"  },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { pay.ID, pay.Code }, out a, CommandType.StoredProcedure);
                pay.Logs.After = "";
                SystemLogs.SaveSystemLogs(pay.Logs);
            }
        }
        public static List<PayCode> Get()
        {
            var res = new List<PayCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_PayCode",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PayCode p = new PayCode();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Code = aRow["Code"].ToString();
                    p.Description = aRow["Description"].ToString();
                    res.Add(p);
                }
            }
            return res;
        }
        public static PayCode GetByID( Int64 Id)
        {
            var p = new PayCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_PayCode",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Code = aRow["Code"].ToString();
                    p.Description = aRow["Description"].ToString();
                }
            }
            return p;
        }
    }
    public class Assignments
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static List<Assignments> GetAssignments()
        {
            var list = new List<Assignments>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Assignments",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Assignments s = new Assignments();
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.Code = aRow["Code"].ToString();
                    s.Description = aRow["Description"].ToString();
                    list.Add(s);
                }
            }
            return list;
        }
       public static string SaveAssignment(Assignments data)
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
                db.ExecuteCommandReader("HRIS_isCodeExist_Assignment",
                    new string[] { "aCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code },out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Assignment code already exist";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Assignment",
                    new string[] { "aDesc" },
                    new DbType[] { DbType.String },
                    new object[] { data.Description}, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Desc in use.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_Assignment",
                       new string[] { "aCode", "aDescription", "aAddedBy", "aModifiedBy" },
                       new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                       new object[] { data.Code, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                       
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                        }
                        data.Logs.After = "ID:"+data.ID.ToString()+ ", Code:" + data.Code+ ", Description:" + data.Description+ "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
            }
            return result;
       }
        public static string EditAssignment(Assignments data)
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
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_Assignment",
                    new string[] { "aID","aCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_Assignment",
                       new string[] { "aID", "aDesc" },
                       new DbType[] { DbType.Int64, DbType.String },
                       new object[] { data.ID,data.Code }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Desc in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_Assignment",
                           new string[] { "aID", "aCode", "aDescription", "aModifiedBy" },
                           new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                           new object[] { data.ID, data.Code, data.Description, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        result = "ok.";
                    }
                }
            }
            return result;
        }
        public static void DeleteAssignment(Assignments data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Assignment",
                   new string[] { "aID", "aCode", "aModifiedBy" },
                   new DbType[] { DbType.Int64, DbType.String, DbType.String },
                   new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);

            }
        }
        public static Assignments GetAssignmentsByID(Int64 Id)
        {
            var s = new Assignments();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Assignment",
                    new string[] { "aID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.Code = aRow["Code"].ToString();
                    s.Description = aRow["Description"].ToString();
                }
            }
            return s;
        }
    }
    
    public class ManageUserAdmin
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class ManageUserEmployee
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Status { get; set; }
        public int NewHire { get; set; }
        public int AccountStatus { get; set; }
    }
    public class PayEligibility
    {
        public long ID { get; set; }
        public int EmploymentClass { get; set; }
        public int EmploymentType { get; set; }
        public int OT { get; set; }
        public int HOL { get; set; }
        public int GracePeriod { get; set; }
        public int LeaveCredits { get; set; }
        public int LateAbsences { get; set; }
        public int _13thMonth { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class Logs
    {
        public long ID { get; set; }
        public int EmployeeClass { get; set; }
        public int EmployeeType { get; set; }
        public int PayMode { get; set; }
        public int PayFrequency { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class SystemLogs
    {
        public long ID { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public int Module { get; set; }
        public string ModuleName { get; set; }
        public string TypeName { get; set; }
        public int Type { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set;}
        public DateTime Date { get; set; }
        public Employee Employee { get; set; }

        public static void SaveSystemLogs(SystemLogs data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_SystemLog",
                    new string[] { "eDataBefore", "eDataAfter", "eModule", "eType", "eUserID", "eUserName" },
                    new DbType[] { DbType.String, DbType.String, DbType.Int32, DbType.Int32,DbType.Int64, DbType.String },
                    new object[] {data.Before, data.After, data.Module, data.Type, data.UserID, data.UserName }, out x, CommandType.StoredProcedure);
            }
        }
    }
}
