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

namespace OnePhp.HRIS.Core.Model
{
    public class SystemSetupPayData
    {
        public SystemSetupPayData()
        {
        }
    }
    public class PayParameter
    {
        public int ID { get; set; }

        //get work schedule for pay parameters
        public static List<WorkSchedule> WorkSched()
        {
            var res = new List<WorkSchedule>();
            using (AppDb db = new AppDb())
            {
                //something to do...

            }
            return res;
        }

    }
    public class BankCode
    {
        public long ID { get; set; }
        public String BankID { get; set; }
        public string BankNo { get; set; }
        public string BankName { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string Save(BankCode bank)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                int d = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("BETA_isBankIDExist_BankID",
                    new string[] { "bId" },
                    new DbType[] { DbType.String },
                    new object[] { bank.BankID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Bank ID is already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isBankNoExist_BankID",
                       new string[] { "bNo" },
                       new DbType[] { DbType.String },
                       new object[] { bank.BankNo }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Bank Number is already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("BETA_isBankNameExist_BankID",
                          new string[] { "bName" },
                          new DbType[] { DbType.String },
                          new object[] { bank.BankName }, out c, ref cTable, CommandType.StoredProcedure);
                        if (cTable.Rows.Count == 1)
                        {
                            res = "Bank Name is already exist.";
                        }
                        else
                        {
                            db.ExecuteCommandNonQuery("BETA_Save_BankID",
                                new string[] { "bID", "bNo", "bName", "bAddedBy", "bModifiedBy" },
                                new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                                new object[] { bank.BankID, bank.BankNo, bank.BankName, bank.AddedBy, bank.ModifiedBy }, out d, CommandType.StoredProcedure);
                            bank.Logs.After = "ID:" + bank.ID.ToString() + ", BankID:" + bank.BankID + ", Bank:" + bank.BankName + "";
                            SystemLogs.SaveSystemLogs(bank.Logs);
                            res = "Save successfully.";
                        }
                    }
                }
            }
            return res;
        }
        public static string Update(BankCode bank)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_isNameInUse_BankID",
                    new string[] { "bbID", "bName" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { bank.ID, bank.BankName }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    res = "In use.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("BETA_Edit_BankID",
                        new string[] { "bbID", "bID", "bNo", "bName", "bModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { bank.ID, bank.BankID, bank.BankNo, bank.BankName, bank.ModifiedBy }, out b, CommandType.StoredProcedure);
                    bank.Logs.After = "ID:" + bank.ID.ToString() + ", BankID:" + bank.BankID + ", Bank:" + bank.BankName + "";
                    SystemLogs.SaveSystemLogs(bank.Logs);
                    res = "Updated successfully.";
                }
            }
            return res;
        }
        public static void Delete(BankCode bank)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_BankID",
                    new string[] { "bbID", "bID" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { bank.ID, bank.BankID }, out a, CommandType.StoredProcedure);
                bank.Logs.After = "";
                SystemLogs.SaveSystemLogs(bank.Logs);
            }
        }
        public static List<BankCode> Get()
        {
            var res = new List<BankCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_BankID",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BankCode b = new BankCode();
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.BankID = aRow["BankID"].ToString();
                    b.BankNo = aRow["BankNo"].ToString();
                    b.BankName = aRow["BankName"].ToString();
                    res.Add(b);
                }
            }
            return res;
        }
        public static BankCode GetByID(Int64 Id)
        {
            var b = new BankCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_BankID",
                    new string[] { "bbID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.BankID = aRow["BankID"].ToString();
                    b.BankNo = aRow["BankNo"].ToString();
                    b.BankName = aRow["BankName"].ToString();

                }
            }
            return b;
        }
    }
    public class RecordStatus
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string Save(RecordStatus rec)
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
                db.ExecuteCommandReader("BETA_isCodeExist_RecordStatus",
                    new string[] { "rCode" },
                    new DbType[] { DbType.String },
                    new object[] { rec.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    res = "Record status code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExist_RecordStatus",
                       new string[] { "rDesc" },
                       new DbType[] { DbType.String },
                       new object[] { rec.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count == 1)
                    {
                        res = "Record status description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_RecordStatus",
                            new string[] { "rCode", "rDesc", "rAddedBy", "rModifiedBy" },
                            new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                            new object[] { rec.Code, rec.Description, rec.AddedBy, rec.ModifiedBy }, out c, CommandType.StoredProcedure);
                        rec.Logs.After = "ID:" + rec.ID.ToString() + ", Code:" + rec.Code + ", Description:" + rec.Description + "";
                        SystemLogs.SaveSystemLogs(rec.Logs);
                        res = "Save successfully.";
                    }
                }
            }
            return res;
        }
        public static string Update(RecordStatus rec)
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
                db.ExecuteCommandReader("BETA_isCodeExistUpdate_RecordStatus",
                    new string[] { "rID", "rCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { rec.ID, rec.Code }, out c, ref cTable, CommandType.StoredProcedure);
                if (cTable.Rows.Count == 1)
                {
                    res = "Record status code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExistUpdates_RecordStatus",
                    new string[] { "rID", "rDesc" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { rec.ID, rec.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count == 1)
                    {
                        res = "In use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_RecordStatus",
                            new string[] { "rID", "rCode", "rDesc", "rModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                            new object[] { rec.ID, rec.Code, rec.Description, rec.ModifiedBy }, out b, CommandType.StoredProcedure);
                        rec.Logs.After = "ID:" + rec.ID.ToString() + ", Code:" + rec.Code + ", Description:" + rec.Description + "";
                        SystemLogs.SaveSystemLogs(rec.Logs);
                        res = "Updated successfully.";
                    }
                }
            }
            return res;
        }
        public static void Delete(RecordStatus rec)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_RecordStatus",
                       new string[] { "rID", "rCode", "rModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.String, DbType.String },
                       new object[] { rec.ID, rec.Code, rec.ModifiedBy }, out a, CommandType.StoredProcedure);
                rec.Logs.After = "";
                SystemLogs.SaveSystemLogs(rec.Logs);
            }
        }
        public static List<RecordStatus> Get()
        {
            var res = new List<RecordStatus>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_RecordStatus",
                   new string[] { },
                   new DbType[] { },
                   new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    RecordStatus r = new RecordStatus();
                    r.ID = Convert.ToInt64(aRow["ID"]);
                    r.Code = aRow["Code"].ToString();
                    r.Description = aRow["Description"].ToString();
                    res.Add(r);
                }
            }
            return res;
        }
        public static RecordStatus GetByID(Int64 Id)
        {
            var r = new RecordStatus();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_RecordStatus",
                   new string[] { "rID" },
                   new DbType[] { DbType.Int64 },
                   new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    r.ID = Convert.ToInt64(aRow["ID"]);
                    r.Code = aRow["Code"].ToString();
                    r.Description = aRow["Description"].ToString();
                }
            }
            return r;
        }
    }
    public class GracePeriod
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string Save(GracePeriod time)
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
                db.ExecuteCommandReader("BETA_isCodeExist_GracePeriod",
                    new string[] { "gCode" },
                    new DbType[] { DbType.String },
                    new object[] { time.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    res = "Grace period code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescExist_GracePeriod",
                        new string[] { "gDesc" },
                        new DbType[] { DbType.String },
                        new object[] { time.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        res = "Grace Period description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Save_GracePeiod",
                            new string[] { "gCode", "gDesc", "gAddedBy", "gModifiedBy", },
                            new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                            new object[] { time.Code, time.Description, time.AddedBy, time.ModifiedBy }, out c, CommandType.StoredProcedure);
                        res = "Save successfully.";
                    }
                }
            }
            return res;
        }
        public static string Update(GracePeriod time)
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
                db.ExecuteCommandReader("BETA_isCodeExist_Update_GracePeriod",
                    new string[] { "gID", "gCode" },
                    new DbType[] { DbType.String, DbType.String },
                    new object[] { time.ID, time.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    res = "Code in use.";
                }
                else
                {
                    db.ExecuteCommandReader("BETA_isDescInUse_GracePeriod",
                        new string[] { "gID", "gDesc" },
                        new DbType[] { DbType.Int64, DbType.String },
                        new object[] { time.ID, time.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        res = "Desc  in use.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("BETA_Edit_GracePeriod",
                            new string[] { "gID", "gCode", "gDesc", "gModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                            new object[] { time.ID, time.Code, time.Description, time.ModifiedBy }, out b, CommandType.StoredProcedure);
                        res = "Updated successfully";
                    }
                }
            }
            return res;
        }
        public static void Delete(GracePeriod time)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("BETA_Delete_GracePeriod",
                    new string[] { "gID", "gCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { time.ID, time.Code }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<GracePeriod> Get()
        {
            var res = new List<GracePeriod>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_Get_GracePeriod",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    GracePeriod g = new GracePeriod();
                    g.ID = Convert.ToInt64(aRow["ID"]);
                    g.Code = aRow["Code"].ToString();
                    g.Description = aRow["Description"].ToString();
                    res.Add(g);
                }
            }
            return res;
        }
        public static GracePeriod GetByID(Int64 Id)
        {
            var g = new GracePeriod();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("BETA_GetByID_GracePeriod",
                    new string[] { "gID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    g.ID = Convert.ToInt64(aRow["ID"]);
                    g.Code = aRow["Code"].ToString();
                    g.Description = aRow["Description"].ToString();
                }
            }
            return g;
        }

    }
    public class WorkSchedule
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public Employee CreatedBy { get; set; }
        public Assignments Assignment { get; set; }
        public List<DailyWorkSchedule> Daily { get; set; }
        public SystemLogs Logs { get; set; }
        public static long SaveWorkSchedule(WorkSchedule work)
        {
            long result = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isRefNoExist_WorkSchedule",
                    new string[] { "rRefNo" },
                    new DbType[] { DbType.String },
                    new object[] { work.ReferenceNo }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    //result = "Reference no. already exist.";
                    result = 0;
                }
                else
                {
                    //db.ExecuteCommandReader("HRIS_isDescExist_WorkSchedule",
                    //    new string[] { "rDesc" },
                    //    new DbType[] { DbType.String },
                    //    new object[] { work.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    //if (cTable.Rows.Count > 0)
                    //{
                    //    result = 002;
                    //}
                    //else
                    //{


                    //}

                    DataTable zTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_Save_WorkSchedule",
                       new string[] { "wReferenceNo", "wName", "wDescription", "wCreatedBy" },
                       new DbType[] { DbType.String, DbType.String, DbType.String, DbType.Int64 },
                       new object[] { work.ReferenceNo, work.Name, work.Description, work.CreatedBy.ID }, out a, ref zTable, CommandType.StoredProcedure);
                    if (zTable.Rows.Count > 0)
                    {
                        DataRow aRow = zTable.Rows[0];
                        result = Convert.ToInt64(aRow["ID"]);
                        var CreatedByEmp = Employee.GetEmployeNameByID(work.CreatedBy.ID);
                        work.Logs.After = "ID:" + work.ID.ToString() + ", ReferenceNo.:" + work.ReferenceNo + ", Name::" + work.Name + ", Description:" + work.Description + "," +
                                                    "CretedBy :" + CreatedByEmp.ID.ToString() + ", " + CreatedByEmp.Firstname + "" + CreatedByEmp.Middlename + " " + CreatedByEmp.Lastname + " " + CreatedByEmp.Suffix + "";
                        SystemLogs.SaveSystemLogs(work.Logs);
                    }

                }
            }
            return result;
        }
        public static string EditWorkSchedule(WorkSchedule work)
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
                db.ExecuteCommandReader("HRIS_isRefNoExist_Update_WorkSchedule",
                    new string[] { "rID", "rRefNo" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { work.ID, work.ReferenceNo }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Reference no. already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_WorkSchedule",
                        new string[] { "rID", "rDesc" },
                        new DbType[] { DbType.Int64, DbType.String },
                        new object[] { work.ID, work.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_WorkSchedule",
                            new string[] { "wID", "wReferenceNo", "wName", "wDescription", "wCreatedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.Int64 },
                            new object[] { work.ID, work.ReferenceNo, work.Name, work.Description, work.CreatedBy.ID }, out a, CommandType.StoredProcedure);
                        var CreatedByEmp = Employee.GetEmployeNameByID(work.CreatedBy.ID);
                        work.Logs.After = "ID:" + work.ID.ToString() + ", ReferenceNo.:" + work.ReferenceNo + ", Name::" + work.Name + ", Description:" + work.Description + "," +
                                                    "CretedBy :" + CreatedByEmp.ID.ToString() + ", " + CreatedByEmp.Firstname + "" + CreatedByEmp.Middlename + " " + CreatedByEmp.Lastname + " " + CreatedByEmp.Suffix + "";
                        SystemLogs.SaveSystemLogs(work.Logs);
                        result = "ok";
                    }

                }

            }
            return result;
        }
        public static void DeleteWorkSchedule(WorkSchedule work)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_WorkSchedule",
                    new string[] { "wID" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { work.ID }, out a, CommandType.StoredProcedure);
                work.Logs.After = "";
                SystemLogs.SaveSystemLogs(work.Logs);
            }
        }
        public static List<WorkSchedule> GetWorkSchedules()
        {
            var ScheduleList = new List<WorkSchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_WorkSchedule",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    WorkSchedule w = new WorkSchedule();
                    w.ID = Convert.ToInt64(aRow["ID"]);
                    w.ReferenceNo = aRow["ReferenceNo"].ToString();
                    w.Description = aRow["Description"].ToString();
                    w.Name = aRow["Name"].ToString();
                    w.CreatedBy = new Employee();
                    w.CreatedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CreatedBy"]));
                    w.Daily = DailyWorkSchedule.GetDailyWorkScheduleByScheduleID(w.ID);
                    ScheduleList.Add(w);
                }
            }
            return ScheduleList;
        }
        public static List<WorkSchedule> GetWorkSchedulesForProjectSchedule()
        {
            var ScheduleList = new List<WorkSchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETProjectBase_WorkSchedule",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    WorkSchedule w = new WorkSchedule();
                    w.ID = Convert.ToInt64(aRow["ID"]);
                    w.ReferenceNo = aRow["ReferenceNo"].ToString();
                    w.Description = aRow["Description"].ToString();
                    w.Name = aRow["Name"].ToString();
                    w.CreatedBy = new Employee();
                    w.CreatedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CreatedBy"]));
                    w.Daily = DailyWorkSchedule.GetDailyWorkScheduleByScheduleID(w.ID);
                    ScheduleList.Add(w);
                }
            }
            return ScheduleList;
        }
        public static WorkSchedule GetWorkScheduleByID(Int64 Id)
        {
            var w = new WorkSchedule();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_WorkSchedule",
                    new string[] { "wID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    w.ID = Convert.ToInt64(aRow["ID"]);
                    w.ReferenceNo = aRow["ReferenceNo"].ToString();
                    w.Description = aRow["Description"].ToString();
                    w.CreatedBy = new Employee();
                    w.CreatedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CreatedBy"]));
                    w.Daily = DailyWorkSchedule.GetDailyWorkScheduleByScheduleID(w.ID);
                    w.Name = aRow["Name"].ToString();
                }
            }
            return w;
        }
        public static WorkSchedule GetWorkScheduleForEmployeeById(Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var w = new WorkSchedule();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Employee_WorkSchedule",
                       new string[] { "eEmployeeID", "eStart", "eEnd" },
                       new DbType[] { DbType.Int64, DbType.Date, DbType.Date },
                       new object[] { EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    w.ID = Convert.ToInt64(aRow["ID"]);
                    w.ReferenceNo = aRow["ReferenceNo"].ToString();
                    w.Description = aRow["Description"].ToString();
                    w.Daily = DailyWorkSchedule.GetDailyWorkScheduleByScheduleID(w.ID);
                    w.Name = aRow["Name"].ToString();
                }
            }
            return w;
        }
    }
    public class Ref_PaySchedule
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public PayrollFrequency Frequency { get; set; }
        public SystemLogs Logs { get; set; }

        public static string SaveTaxTableCode(Ref_PaySchedule data)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_TaxTable",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Tax table code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_TaxTable",
                    new string[] { "tDesc" },
                    new DbType[] { DbType.String },
                    new object[] { data.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        result = "Tax table description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_Ref_PaySchedule",
                             new string[] { "tCode", "tDescription", "tFrequencyID", "tAddedBy", "tModifiedBy", },
                             new DbType[] { DbType.String, DbType.String, DbType.Int64, DbType.String, DbType.String },
                             new object[] { data.Code, data.Description, data.Frequency.ID, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                        }
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description::" + data.Description + ", Frequency:" + data.Frequency.ID + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
            }
            return result;
        }
        public static string EditTaxTableCode(Ref_PaySchedule data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable cTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_TaxTable",
                    new string[] { "tID", "tCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out c, ref cTable, CommandType.StoredProcedure);
                if (cTable.Rows.Count > 0)
                {
                    result = "Tax table code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_TaxTable",
                    new string[] { "tID", "tDesc" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "Tax table description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_Ref_PaySchedule",
                            new string[] { "tID", "tCode", "tDescription", "tFrequencyID", "tModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.Int64, DbType.String, },
                            new object[] { data.ID, data.Code, data.Description, data.Frequency.ID, data.AddedBy }, out a, CommandType.StoredProcedure);

                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description::" + data.Description + ", Frequency:" + data.Frequency.ID + "";
                        SystemLogs.SaveSystemLogs(data.Logs);

                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void DeleteTaxTableCode(Ref_PaySchedule data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_TaxTable",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Code, data.AddedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<Ref_PaySchedule> GetTabTableCodes()
        {
            var codes = new List<Ref_PaySchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_TaxTable",
                      new string[] { },
                      new DbType[] { },
                      new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Ref_PaySchedule t = new Ref_PaySchedule();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    t.Frequency = new PayrollFrequency();
                    t.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(aRow["PayrollFrequencyID"]));
                    codes.Add(t);
                }
            }
            return codes;
        }
        public static Ref_PaySchedule GetTaxTableCodesByID(Int64 Id)
        {
            var t = new Ref_PaySchedule();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_TaxTable",
                      new string[] { "tID" },
                      new DbType[] { DbType.Int64 },
                      new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    t.Frequency = new PayrollFrequency();
                    t.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(aRow["PayrollFrequencyID"]));
                }
            }
            return t;
        }
    }
    public class DeductionSchedule
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string SaveDeductionTableCode(DeductionSchedule data)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_DeductionSchedule",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Deduction schedule code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_DeductionSchedule",
                    new string[] { "tDesc" },
                    new DbType[] { DbType.String },
                    new object[] { data.Description }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        result = "Deduction schedule description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_DeductionSchedule",
                        new string[] { "tCode", "tDescription", "tAddedBy", "tModifiedBy", },
                        new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { data.Code, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                        }
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }

                }
            }
            return result;
        }
        public static string EditDeductionTableCode(DeductionSchedule data)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_DeductionSchedule",
                    new string[] { "tID", "tCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Deduction schedule code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_DeductionSchedule",
                        new string[] { "tID", "tDesc" },
                        new DbType[] { DbType.Int64, DbType.String },
                        new object[] { data.ID, data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "Deduction schedule description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_DeductionSchedule",
                            new string[] { "tID", "tCode", "tDescription", "tModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, },
                            new object[] { data.ID, data.Code, data.Description, data.AddedBy }, out a, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void DeleteDeductionTableCode(DeductionSchedule data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_DeductionSchedule",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Code, data.AddedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<DeductionSchedule> GetDeductionTableCode()
        {
            var codes = new List<DeductionSchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_DeductionSchedule",
                      new string[] { },
                      new DbType[] { },
                      new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    DeductionSchedule t = new DeductionSchedule();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    codes.Add(t);
                }
            }
            return codes;
        }
        public static DeductionSchedule GetDeductionTableCodeByID(Int64 Id)
        {
            var t = new DeductionSchedule();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_DeductionSchedule",
                      new string[] { "tID" },
                      new DbType[] { DbType.Int64 },
                      new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
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
    public class PayRate
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public PayrollFrequency Frequency { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static string SavePayrollSchedTypeCode(PayRate data)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_PayRate",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Payroll Sched Type code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_PayRate",
                       new string[] { "tDesc" },
                       new DbType[] { DbType.String },
                       new object[] { data.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Payroll Sched Type description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_PayRate",
                          new string[] { "tFrequencyID", "tCode", "tDescription", "tAddedBy", "tModifiedBy", },
                          new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                          new object[] { data.Frequency.ID, data.Code, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                        }
                    }
                }
            }
            return result;
        }
        public static string EditPayrollSchedTypeCode(PayRate data)
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
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_PayRate",
                    new string[] { "tID", "tCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Payroll Sched Type code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_PayRate",
                       new string[] { "tID", "tDesc" },
                       new DbType[] { DbType.Int64, DbType.String },
                       new object[] { data.ID, data.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Payroll Sched Type description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_PayRate",
                           new string[] { "tID", "tFrequencyID", "tCode", "tDescription", "tModifiedBy" },
                           new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String, },
                           new object[] { data.ID, data.Frequency.ID, data.Code, data.Description, data.AddedBy }, out a, CommandType.StoredProcedure);
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void DeletePayrollSchedTypeCode(PayRate data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_PayRate",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Code, data.AddedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<PayRate> GetPayrollSchedTypeCode()
        {
            var codes = new List<PayRate>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_PayRate",
                      new string[] { },
                      new DbType[] { },
                      new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PayRate t = new PayRate();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(aRow["FrequencyID"]));
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    codes.Add(t);
                }
            }
            return codes;
        }
        public static PayRate GetPayrollSchedTypeCodeByID(Int64 Id)
        {
            var t = new PayRate();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_PayRate",
                      new string[] { "tID" },
                      new DbType[] { DbType.Int64 },
                      new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(aRow["FrequencyID"]));
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class PayrollFrequency
    {
        public long ID { get; set; }
        public int NumberOfCutOff { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string SavePayrollFrequencyCode(PayrollFrequency data)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_PayrollFrequencies",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Payroll frequency code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_PayrollFrequencies",
                       new string[] { "tDesc" },
                       new DbType[] { DbType.String },
                       new object[] { data.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Payroll frequency description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_PayrollFrequencies",
                          new string[] { "tCode", "tNumberOfCutOff", "tDescription", "tAddedBy", "tModifiedBy", },
                          new DbType[] { DbType.String, DbType.Int32, DbType.String, DbType.String, DbType.String },
                          new object[] { data.Code, data.NumberOfCutOff, data.Description, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                            data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ",  NumberOfCutOFF's:" + data.NumberOfCutOff + ", Description:" + data.Description + "";
                            SystemLogs.SaveSystemLogs(data.Logs);
                        }
                    }
                }
            }
            return result;
        }
        public static string EditPayrollFrequencyCode(PayrollFrequency data)
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
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_PayrollFrequencies",
                   new string[] { "tID", "tCode" },
                   new DbType[] { DbType.Int64, DbType.String },
                   new object[] { data.ID, data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Payroll frequency code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_PayrollFrequencies",
                       new string[] { "tID", "tDesc" },
                       new DbType[] { DbType.Int64, DbType.String },
                       new object[] { data.ID, data.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Payroll frequency description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_PayrollFrequencies",
                         new string[] { "tID", "tNumberOfCutOff", "tCode", "tDescription", "tModifiedBy" },
                         new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String, },
                         new object[] { data.ID, data.NumberOfCutOff, data.Code, data.Description, data.AddedBy }, out a, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ",  NumberOfCutOFF's:" + data.NumberOfCutOff + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        result = "ok";
                    }
                }
            }
            return result;
        }
        public static void DeletePayrollFrequencyCode(PayrollFrequency data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_PayrollFrequencies",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Code, data.AddedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<PayrollFrequency> GetPayrollFrequencyCode()
        {
            var codes = new List<PayrollFrequency>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_PayrollFrequencies",
                      new string[] { },
                      new DbType[] { },
                      new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PayrollFrequency t = new PayrollFrequency();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.NumberOfCutOff = Convert.ToInt32(aRow["NumberOfCutOff"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                    codes.Add(t);
                }
            }
            return codes;
        }
        public static PayrollFrequency GetPayrollFrequencyByID(Int64 Id)
        {
            var t = new PayrollFrequency();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_PayrollFrequencies",
                      new string[] { "tID" },
                      new DbType[] { DbType.Int64 },
                      new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.NumberOfCutOff = Convert.ToInt32(aRow["NumberOfCutOff"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class BatchAdjustment
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Classification { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string SaveBatchAdjustmentCode(BatchAdjustment data)
        {
            var result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int z = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable zTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_BatchAdjustment",
                    new string[] { "tCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Batch adjustment code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_BatchAdjustment",
                       new string[] { "tDesc" },
                       new DbType[] { DbType.String },
                       new object[] { data.Description }, out z, ref zTable, CommandType.StoredProcedure);
                    if (zTable.Rows.Count > 0)
                    {
                        result = "Batch adjustment description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_BatchAdjustment",
                          new string[] { "tCode", "tDescription", "tClassification", "tAddedBy", "tModifiedBy", },
                          new DbType[] { DbType.String, DbType.String, DbType.Int32, DbType.String, DbType.String },
                          new object[] { data.Code, data.Description, data.Classification, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                        if (bTable.Rows.Count > 0)
                        {
                            result = "ok";
                            data.Logs.After = " ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + ", Classification:" + data.Classification + ", ";
                            SystemLogs.SaveSystemLogs(data.Logs);
                        }
                    }

                }
            }
            return result;
        }
        public static string EditBatchAdjustmentCode(BatchAdjustment data)
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
                db.ExecuteCommandReader("HRIS_isCodeExist_Update_BatchAdjustment",
                    new string[] { "tID", "tCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    result = "Batch adjustment code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_Update_BatchAdjustment",
                      new string[] { "tID", "tDesc" },
                      new DbType[] { DbType.Int64, DbType.String },
                      new object[] { data.ID, data.Description }, out c, ref cTable, CommandType.StoredProcedure);
                    if (cTable.Rows.Count > 0)
                    {
                        result = "Batch adjustment description already exist.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_BatchAdjustment",
                         new string[] { "tID", "tCode", "tDescription", "tClassification", "tModifiedBy" },
                         new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.Int32, DbType.String, },
                         new object[] { data.ID, data.Code, data.Description, data.Classification, data.AddedBy }, out a, CommandType.StoredProcedure);
                        data.Logs.After = " ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + ", Classification:" + data.Classification + ", ";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        result = "ok";
                    }
                }

            }
            return result;
        }
        public static void DeleteBatchAdjustmentCode(BatchAdjustment data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_BatchAdjustment",
                    new string[] { "tID", "tCode", "tModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.ID, data.Code, data.AddedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = " ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + ", Classification:" + data.Classification + ", ";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<BatchAdjustment> GetBatchAdjustmentCode()
        {
            var codes = new List<BatchAdjustment>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_BatchAdjustment",
                      new string[] { },
                      new DbType[] { },
                      new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BatchAdjustment t = new BatchAdjustment();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Code = aRow["Code"].ToString();
                    t.Classification = Convert.ToInt32(aRow["Classification"]);
                    t.Description = aRow["Description"].ToString();
                    codes.Add(t);
                }
            }
            return codes;
        }
        public static BatchAdjustment GetBatchAdjustmentByID(Int64 Id)
        {
            var t = new BatchAdjustment();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_BatchAdjustment",
                      new string[] { "tID" },
                      new DbType[] { DbType.Int64 },
                      new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Classification = Convert.ToInt32(aRow["Classification"]);
                    t.Code = aRow["Code"].ToString();
                    t.Description = aRow["Description"].ToString();
                }
            }
            return t;
        }
    }
    public class LeaveTypeCode
    {
        public long ID { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string SaveLeaveTypeCode(LeaveTypeCode data)
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
                db.ExecuteCommandReader("HRIS_isCodeExist_LeaveType",
                    new string[] { "oCode" },
                    new DbType[] { DbType.Int32 },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Leave type code already exist";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_isDescExist_LeaveType",
                    new string[] { "oDescription" },
                    new DbType[] { DbType.String },
                    new object[] { data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "Leave type code description already exist";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_Save_LeaveType",
                            new string[] { "oCode", "oDescription", "oAddedBy", "oModifiedBy" },
                            new DbType[] { DbType.Int32, DbType.String, DbType.String, DbType.String },
                            new object[] { data.Code, data.Description, data.AddedBy, data.ModifiedBy }, out c, ref cTable, CommandType.StoredProcedure);
                        if (cTable.Rows.Count > 0)
                        {
                            result = "ok";
                            data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                            SystemLogs.SaveSystemLogs(data.Logs);
                        }
                    }
                }
            }
            return result;
        }
        public static string EditLeaveTypeCode(LeaveTypeCode data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isDescInUse_LeaveType",
                    new string[] { "oID", "oDescription" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Description }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Leave type code description is currently in use";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Edit_LeaveType",
                        new string[] { "oID", "oCode", "oDescription", "oModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                        new object[] { data.ID, data.Code, data.Description, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                    data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    result = "ok";
                }
            }
            return result;
        }
        public static void DeleteLeaveTypeCode(LeaveTypeCode data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_LeaveType",
                    new string[] { "oID", "oCode", "oModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<LeaveTypeCode> GetLeaveTypeCodes()
        {
            var list = new List<LeaveTypeCode>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_LeaveType",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveTypeCode o = new LeaveTypeCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Code = Convert.ToInt32(aRow["Code"]);
                    o.Description = aRow["Description"].ToString();
                    list.Add(o);
                }
            }
            return list;
        }
        public static LeaveTypeCode GetLeaveTypeCodeByID(Int64 Id)
        {
            var o = new LeaveTypeCode();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETByID_LeaveType",
                    new string[] { "oID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Code = Convert.ToInt32(aRow["Code"]);
                    o.Description = aRow["Description"].ToString();
                }
            }
            return o;
        }

    }
    public class HOL_OT
    {
        public long ID { get; set; }
        public string TypeName { get; set; }
        public string RegularHoliday { get; set; }
        public string RestDayAndSpecialHoliday { get; set; }
        public string RestDay { get; set; }
        public string RegularDay { get; set; }
        public string SpecialHoliday { get; set; }
        public string RestDayAndRegularHoliday { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SaveHOL_OT(HOL_OT data, string[] TypeName, string[] RegularHoliday, string[] RestDayAndSpecialHoliday,
            string[] RestDay, string[] RegularDay, string[] SpecialHoliday, string[] RestDayAndRegularHoliday)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int m = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_HOLOT",
                  new string[] { },
                  new DbType[] { },
                  new object[] { }, out x, CommandType.StoredProcedure);
                string _after = "";
                foreach (string o in TypeName)
                {
                    data.TypeName = TypeName[m];
                    if (RegularHoliday[m] != null)
                    {
                        data.RegularHoliday = RegularHoliday[m];
                    }
                    else
                    {
                        data.RegularHoliday = "";
                    }
                    if (RestDayAndSpecialHoliday[m] != null)
                    {
                        data.RestDayAndRegularHoliday = RestDayAndSpecialHoliday[m];
                    }
                    else
                    {
                        data.RestDayAndRegularHoliday = "";
                    }
                    if (RestDay[m] != null)
                    {
                        data.RestDay = RestDay[m];
                    }
                    else
                    {
                        data.RestDay = "";
                    }
                    if (RegularDay[m] != null)
                    {
                        data.RegularDay = RegularDay[m];
                    }
                    else
                    {
                        data.RegularDay = "";
                    }
                    if (SpecialHoliday[m] != null)
                    {
                        data.SpecialHoliday = SpecialHoliday[m];
                    }
                    else
                    {
                        data.SpecialHoliday = "";
                    }
                    if (RestDayAndRegularHoliday[m] != null)
                    {
                        data.RestDayAndRegularHoliday = RestDayAndRegularHoliday[m];
                    }
                    else
                    {
                        data.RestDayAndRegularHoliday = "";
                    }

                    db.ExecuteCommandNonQuery("HRIS_Save_HOLOT",
                     new string[] { "oTypeName", "oRegularDay", "oRestDaySpecialHoliday",
                                    "oRestDay","oRegularHoliday","oSpecialHoliday","oRegularHolidayRestDay",
                                    "oAddedBy","oModifiedBy" },
                     new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, },
                     new object[] { data.TypeName,data.RegularDay,data.RestDayAndSpecialHoliday,
                                    data.RestDay,data.RegularHoliday,data.SpecialHoliday,data.RestDayAndRegularHoliday,
                                    data.AddedBy,data.ModifiedBy}, out x, CommandType.StoredProcedure);
                    _after = "[ID:" + data.ID.ToString() + ", TypeName:" + data.TypeName + ", RegularDay:" + data.RegularDay + ", " +
                        "RestDay&SpecialHoliday:" + data.RestDayAndSpecialHoliday + ", RestDay:" + data.RestDay + ", RegularHoliday:" + data.RegularHoliday + ",  " +
                        "SpecialHoliday:" + data.SpecialHoliday + ", RestDayAndRegularHoliday:" + data.RestDayAndRegularHoliday + "], ";
                    m += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<HOL_OT> GetHOL_OTs()
        {
            var _list = new List<HOL_OT>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_HOLOT",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    HOL_OT o = new HOL_OT();
                    o.TypeName = aRow["TypeName"].ToString();
                    o.RegularDay = aRow["RegularDay"].ToString();
                    o.RestDayAndSpecialHoliday = aRow["RestDaySpecialHoliday"].ToString();
                    o.RestDay = aRow["RestDay"].ToString();
                    o.RegularHoliday = aRow["RegularHoliday"].ToString();
                    o.SpecialHoliday = aRow["SpecialHoliday"].ToString();
                    o.RestDayAndRegularHoliday = aRow["RegularHolidayRestDay"].ToString();
                    _list.Add(o);
                }
            }
            return _list;
        }
    }
    public class Leave
    {
        public long ID { get; set; }
        public int Code { get; set; }
        public EmployeeClassCode EmpClass { get; set; }
        public EmployeeTypeCode EmpType { get; set; }
        public EmployeeClassCode EmpClassCode { get; set; }
        public LeaveTypeCode LeaveType { get; set; }
        public int LeaveCredits { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveLeaveSetupCode(Leave data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_LeaveSetup",
                    new string[] { "oCode" },
                    new DbType[] { DbType.Int32 },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Leave setup code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_Save_LeaveSetup",
                   new string[] { "oCode", "oEmpType", "oEmpClass", "oEmpClassCode", "oLeaveType", "oLeaveCredits", "oAddedBy", "oModifiedBy", },
                   new DbType[] { DbType.Int32, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                   new object[] { data.Code, data.EmpType.ID, data.EmpClass.ID, data.EmpClassCode.ID, data.LeaveType.ID, data.LeaveCredits, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "ok";

                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", EploymentType:" + data.EmpType.ID.ToString() + ", " +
                            "EmployeeClass::" + data.EmpClass.ID.ToString() + ", EmpClassCode:" + data.EmpClassCode.ID.ToString() + ", " +
                            "LeaveType:" + data.LeaveType.ID.ToString() + ", LeaveCredits:" + data.LeaveCredits + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
            }
            return result;
        }
        public static void EditLeaveSetupCode(Leave data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_LeaveSetup",
                    new string[] { "oID", "oCode", "oEmpType", "oEmpClass", "oEmpClassCode", "oLeaveType", "oLeaveCredits", "oModifiedBy", },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Code, data.EmpType.ID, data.EmpClass.ID, data.EmpClassCode.ID, data.LeaveType.ID, data.LeaveCredits, data.ModifiedBy }, out a, CommandType.StoredProcedure);

                data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", EploymentType:" + data.EmpType.ID.ToString() + ", " +
                            "EmployeeClass::" + data.EmpClass.ID.ToString() + ", EmpClassCode:" + data.EmpClassCode.ID.ToString() + ", " +
                            "LeaveType:" + data.LeaveType.ID.ToString() + ", LeaveCredits:" + data.LeaveCredits + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeleteLeaveSetupCode(Leave data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_LeaveSetup",
                    new string[] { "oID", "oCode", "oModifiedBy", },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<Leave> GetLeaveSetupCodes()
        {
            var list = new List<Leave>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_LeaveSetup",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Leave o = new Leave();
                    o.EmpClass = new EmployeeClassCode();
                    o.EmpClassCode = new EmployeeClassCode();
                    o.EmpType = new EmployeeTypeCode();
                    o.LeaveType = new LeaveTypeCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Code = Convert.ToInt32(aRow["Code"]);
                    o.EmpClass = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmpClass"]));
                    o.EmpClassCode = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassCode"]));
                    o.EmpType = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmpType"]));
                    o.LeaveType = LeaveTypeCode.GetLeaveTypeCodeByID(Convert.ToInt64(aRow["LeaveType"]));
                    o.LeaveCredits = Convert.ToInt32(aRow["LeaveCredits"]);
                    list.Add(o);
                }
            }
            return list;
        }
        public static Leave GetLeaveSetupCodeByID(Int64 Id)
        {
            var o = new Leave();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETByID_LeaveSetup",
                    new string[] { "oID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    o.EmpClass = new EmployeeClassCode();
                    o.EmpClassCode = new EmployeeClassCode();
                    o.EmpType = new EmployeeTypeCode();
                    o.LeaveType = new LeaveTypeCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Code = Convert.ToInt32(aRow["Code"]);
                    o.EmpClass = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmpClass"]));
                    o.EmpClassCode = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassCode"]));
                    o.EmpType = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmpType"]));
                    o.LeaveType = LeaveTypeCode.GetLeaveTypeCodeByID(Convert.ToInt64(aRow["LeaveType"]));
                    o.LeaveCredits = Convert.ToInt32(aRow["LeaveCredits"]);
                }
            }
            return o;
        }
    }
    public class GraceperiodSetUp
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public EmployeeClassCode EmpClass { get; set; }
        public EmployeeTypeCode EmpType { get; set; }
        public EmployeeClassCode EmpClassCode { get; set; }
        public int GracePeriod { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveGracePeriodSetupCode(GraceperiodSetUp data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isCodeExist_GracePeriodsetup",
                    new string[] { "oCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    result = "Grace period setup code already exist.";
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_Save_GracePeriodSetup",
                    new string[] { "oCode", "oEmpType", "oEmpClass", "oEmpClassCode", "oGracePeriod", "oAddedBy", "oModifiedBy" },
                    new DbType[] { DbType.String, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                    new object[] { data.Code, data.EmpType.ID, data.EmpClass.ID, data.EmpClassCode.ID, data.GracePeriod, data.AddedBy, data.ModifiedBy }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        result = "ok";
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Type:" + data.EmpType.ID.ToString() + ", Class::" + data.EmpClass.ID + ", " +
                            "EmpCLassCOde::" + data.EmpClassCode.ID + ", GracePeriod:" + data.GracePeriod + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                    }
                }
            }
            return result;
        }
        public static void EditGracePeriodSetupCode(GraceperiodSetUp data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_GracePeriodSetup",
                    new string[] { "oID", "oCode", "oEmpType", "oEmpClass", "oEmpClassCode", "oGracePeriod", "oModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Code, data.EmpType.ID, data.EmpClass.ID, data.EmpClassCode.ID, data.GracePeriod, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Type:" + data.EmpType.ID.ToString() + ", Class::" + data.EmpClass.ID + ", " +
                            "EmpCLassCOde::" + data.EmpClassCode.ID + ", GracePeriod:" + data.GracePeriod + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeleteGracePeriodSetupCode(GraceperiodSetUp data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_GracePeriodSetup",
                    new string[] { "oID", "oCode", "oModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<GraceperiodSetUp> GetGraceperiodSetUpCodes()
        {
            var list = new List<GraceperiodSetUp>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_GracePeriodSetup",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    GraceperiodSetUp o = new GraceperiodSetUp();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Code = aRow["Code"].ToString();
                    o.EmpClass = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmpClass"]));
                    o.EmpClassCode = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassCode"]));
                    o.EmpType = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmpType"]));
                    o.GracePeriod = Convert.ToInt32(aRow["GracePeriod"]);
                    list.Add(o);
                }

            }
            return list;
        }
        public static GraceperiodSetUp GetGraceperiodSetUpCodeByID(Int64 Id)
        {
            var o = new GraceperiodSetUp();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_GracePeriodSetup",
                   new string[] { "oID" },
                   new DbType[] { DbType.Int64 },
                   new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Code = aRow["Code"].ToString();
                    o.EmpClass = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmpClass"]));
                    o.EmpClassCode = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["ClassCode"]));
                    o.EmpType = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmpType"]));
                    o.GracePeriod = Convert.ToInt32(aRow["GracePeriod"]);
                }
            }
            return o;
        }
    }
    public class _13thMonth
    {
        public long ID { get; set; }
        public int EmployeeClass { get; set; }
        public int EmployeeType { get; set; }
        public int MonthlyRate { get; set; }
        public int ProRated { get; set; }
        public int PayoutFrequency { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    //Statutory
    //word is not so common
    public class Deductions
    {
        public long ID { get; set; }
        public int EmployeeClass { get; set; }
        public int EmployeeType { get; set; }
        public enum PAGIBIG { _1stPayroll = 1, _2ndPayroll = 2 }
        public enum PHILHEALTH { _1stPayroll = 1, _2ndPayroll = 2 }
        public enum SSS { _1stPayroll = 1, _2ndPayroll = 2 }
        public enum TAX { _1stPayroll = 1, _2ndPayroll = 2 }
        public int FixContribution { get; set; } //(1=Fix, 0=Not Fix)
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class PayPeriod
    {
        public long ID { get; set; }
        public PayFrequencyDetails Frequency { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime ActualPayDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public PayOut PayOut { get; set; }
        public static void SavePayPeriod(PayPeriod data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_PayPeriod",
                    new string[] { "pFrequencyID", "pYear", "pMonth", "pStart", "pEnd", "pPaydate", "pActualPayDate", "pAddedBy", "pModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Date, DbType.Date, DbType.Date, DbType.Date, DbType.String, DbType.String },
                    new object[] { data.Frequency.ID, data.Year, data.Month, data.Start, data.End, data.PayDate, data.ActualPayDate, data.AddedBy, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Year:" + data.Year.ToString() + ", Month:" + data.Month.ToString() + "," +
                    "Start:" + data.Start.ToString("MM-dd-yyyy") + ",  End:" + data.End.ToString("MM-dd-yyyy") + ", PayDate:" + data.PayDate.ToString("MM-dd-yyyy") + "," +
                    "ActualPayDate:" + data.ActualPayDate.ToString("MM-dd-yyyy") + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void EditPayPeriod(PayPeriod data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_PayPeriod",
                   new string[] { "pID", "pFrequencyID", "pYear", "pMonth", "pStart", "pEnd", "pPaydate", "pActualPayDate", "pModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.Int32, DbType.Date, DbType.Date, DbType.Date, DbType.Date, DbType.String },
                    new object[] { data.ID, data.Frequency.ID, data.Year, data.Month, data.Start, data.End, data.PayDate, data.ActualPayDate, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Year:" + data.Year.ToString() + ", Month:" + data.Month.ToString() + "," +
                   "Start:" + data.Start.ToString("MM-dd-yyyy") + ",  End:" + data.End.ToString("MM-dd-yyyy") + ", PayDate:" + data.PayDate.ToString("MM-dd-yyyy") + "," +
                   "ActualPayDate:" + data.ActualPayDate.ToString("MM-dd-yyyy") + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeletePayPeriod(PayPeriod data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_PayPeriod",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.ID }, out x, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<PayPeriod> GetPayPeriods()
        {
            var _list = new List<PayPeriod>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_PayPeriod",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PayPeriod p = new PayPeriod();
                    p.Frequency = new PayFrequencyDetails();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Frequency = PayFrequencyDetails.GetFrequencyDetailsByID(Convert.ToInt64(aRow["FrequencyID"]));
                    p.Year = Convert.ToInt32(aRow["Year"]);
                    p.Month = Convert.ToInt32(aRow["Month"]);
                    p.Start = Convert.ToDateTime(aRow["Start"]);
                    p.End = Convert.ToDateTime(aRow["End"]);
                    p.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    p.ActualPayDate = Convert.ToDateTime(aRow["ActualPayDate"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static List<PayPeriod> GetFilteredPayPeriods(Int64 eFrequencyID, Int32 eYear, Int32 eMonth)
        {
            var _list = new List<PayPeriod>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_filter_Payperiod",
                    new string[] { "eFrequencyid", "eyear", "emonth" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32 },
                    new object[] { eFrequencyID, eYear, eMonth }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PayPeriod p = new PayPeriod();
                    p.Frequency = new PayFrequencyDetails();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Frequency = PayFrequencyDetails.GetFrequencyDetailsByID(Convert.ToInt64(aRow["FrequencyID"]));
                    p.Year = Convert.ToInt32(aRow["Year"]);
                    p.Month = Convert.ToInt32(aRow["Month"]);
                    p.Start = Convert.ToDateTime(aRow["Start"]);
                    p.End = Convert.ToDateTime(aRow["End"]);
                    p.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    p.ActualPayDate = Convert.ToDateTime(aRow["ActualPayDate"]);
                    p.PayOut = new PayOut();
                    p.PayOut.IsLocked = Convert.ToBoolean(aRow["isLocked"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static List<PayPeriod> GetPayPeriodsByFrequencyID(Int64 frequencyID, Int32 Year, Int32 Month)
        {
            var _list = new List<PayPeriod>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByFrequency_PayPeriod",
                    new string[] { "pFrequencyID", "pYear", "pMonth" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32 },
                    new object[] { frequencyID, Year, Month }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PayPeriod p = new PayPeriod();
                    p.Frequency = new PayFrequencyDetails();
                    p.Frequency.Frequency = new PayrollFrequency();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    //p.Frequency = PayFrequencyDetails.GetFrequencyDetailsByID(Convert.ToInt64(aRow["FrequencyID"]));
                    p.Frequency.ID = Convert.ToInt64(aRow["PF_Id"]);
                    p.Frequency.Frequency.Description = aRow["Description"].ToString();
                    p.Year = Convert.ToInt32(aRow["Year"]);
                    p.Month = Convert.ToInt32(aRow["Month"]);
                    p.Start = Convert.ToDateTime(aRow["Start"]);
                    p.End = Convert.ToDateTime(aRow["End"]);
                    p.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    p.ActualPayDate = Convert.ToDateTime(aRow["ActualPayDate"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static List<PayPeriod> GetPayPeriodsByYear(Int32 year)
        {
            var _list = new List<PayPeriod>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByYear_PayPeriod",
                    new string[] { "pYear" },
                    new DbType[] { DbType.Int32 },
                    new object[] { year }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PayPeriod p = new PayPeriod();
                    p.Frequency = new PayFrequencyDetails();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Frequency = PayFrequencyDetails.GetFrequencyDetailsByID(Convert.ToInt64(aRow["FrequencyID"]));
                    p.Year = Convert.ToInt32(aRow["Year"]);
                    p.Month = Convert.ToInt32(aRow["Month"]);
                    p.Start = Convert.ToDateTime(aRow["Start"]);
                    p.End = Convert.ToDateTime(aRow["End"]);
                    p.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    p.ActualPayDate = Convert.ToDateTime(aRow["ActualPayDate"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static List<PayPeriod> GetPayPeriodsByMonth(Int32 month)
        {
            var _list = new List<PayPeriod>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByMonth_PayPeriod",
                    new string[] { "pMonth" },
                    new DbType[] { DbType.Int32 },
                    new object[] { month }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PayPeriod p = new PayPeriod();
                    p.Frequency = new PayFrequencyDetails();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Frequency = PayFrequencyDetails.GetFrequencyDetailsByID(Convert.ToInt64(aRow["FrequencyID"]));
                    p.Year = Convert.ToInt32(aRow["Year"]);
                    p.Month = Convert.ToInt32(aRow["Month"]);
                    p.Start = Convert.ToDateTime(aRow["Start"]);
                    p.End = Convert.ToDateTime(aRow["End"]);
                    p.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static PayPeriod GetPayPeriodByID(Int64 Id)
        {
            var p = new PayPeriod();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetById_PayPeriod",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    p.Frequency = new PayFrequencyDetails();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Frequency = PayFrequencyDetails.GetFrequencyDetailsByID(Convert.ToInt64(aRow["FrequencyID"]));
                    p.Year = Convert.ToInt32(aRow["Year"]);
                    p.Month = Convert.ToInt32(aRow["Month"]);
                    p.Start = Convert.ToDateTime(aRow["Start"]);
                    p.End = Convert.ToDateTime(aRow["End"]);
                    p.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    p.ActualPayDate = Convert.ToDateTime(aRow["ActualPayDate"]);
                }
            }
            return p;
        }
    }
    public class Tardiness_Absences
    {
        public long ID { get; set; }
        public int EmployeeClass { get; set; }
        public int EmployeeType { get; set; }
        public int DeductionType { get; set; }
        public int Rate { get; set; }
        public int DeductionFrequency { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class AnnualTaxTable
    {
        public long ID { get; set; }
        public int Type { get; set; }
        public double From { get; set; }
        public double NotOver { get; set; }
        public double BaseAmount { get; set; }
        public string RateOfExcess { get; set; }
        public double ExcessFrom { get; set; }
        public string AddedBy { get; set; }
        public int Status { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveAnnualTaxTableData(AnnualTaxTable data, double[] From, double[] NotOver, double[] BaseAmount, string[] RateOfExcess, double[] ExcessFrom)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteAll_AnnualTaxTable",
                    new string[] { "tType" },
                    new DbType[] { DbType.Int32 },
                    new object[] { data.Type }, out b, CommandType.StoredProcedure);
                var ss = new SystemLogs();
                ss.Type = 3;
                ss.Module = 46;
                ss.Before = data.Logs.Before;
                ss.After = "";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
                int a = 0;
                int x = 0;
                string _after = "";
                foreach (double counter in From)
                {
                    data.From = counter;
                    data.NotOver = NotOver[x];
                    data.BaseAmount = BaseAmount[x];
                    data.RateOfExcess = RateOfExcess[x];
                    data.ExcessFrom = ExcessFrom[x];
                    //data.Type = Type[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_AnnualTaxTable",
                    new string[] { "tFrom", "tNotOver", "tBaseAmount", "tRateOfExcess", "tExcessFrom", "tType", "tAddedBy", "tModifiedBy" },
                    new DbType[] { DbType.Double, DbType.Double, DbType.Double, DbType.Double, DbType.String, DbType.Int32, DbType.String, DbType.String },
                    new object[] { data.From, data.NotOver, data.BaseAmount, data.RateOfExcess, data.ExcessFrom, data.Type, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                    _after += "[ID:" + data.ID.ToString() + ", From::" + data.From + ", NotOver:" + data.NotOver + ", BaseAmount:" + data.BaseAmount + ", " +
                                    "RateOfExcess:" + data.RateOfExcess + ", ExcessFrom:" + data.ExcessFrom + ", Type:" + data.Type + "],";
                    x += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<AnnualTaxTable> GetAnnualTaxTableData(Int32 Type)
        {
            var listData = new List<AnnualTaxTable>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_AnnualTaxTable",
                    new string[] { "tType" },
                    new DbType[] { DbType.Int32 },
                    new object[] { Type }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    AnnualTaxTable t = new AnnualTaxTable();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.From = Convert.ToDouble(aRow["From"]);
                    t.NotOver = Convert.ToDouble(aRow["NotOver"]);
                    t.BaseAmount = Convert.ToDouble(aRow["BaseAmount"]);
                    t.RateOfExcess = aRow["RateOfExcess"].ToString();
                    t.ExcessFrom = Convert.ToDouble(aRow["ExcessFrom"]);
                    t.Type = Convert.ToInt32(aRow["Type"]);
                    t.Status = Convert.ToInt32(aRow["Status"]);
                    listData.Add(t);
                }
            }
            return listData;
        }
        public static void DeleteAnnualTaxTableData(AnnualTaxTable data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_AnnualTaxTable",
                    new string[] { "tID", "tType" },
                    new DbType[] { DbType.Int64, DbType.Int32 },
                    new object[] { data.ID, data.Type }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
    }
    public class SSSContributionTable
    {
        public long ID { get; set; }
        public double RangeOfCompensationFrom { get; set; }
        public double RangeOfCompensationTo { get; set; }
        public double MSC_RSS_EmployeesCompensation { get; set; }
        public double MSC_MandatoryProvidentFund { get; set; }
        public double MSC_Total { get; set; }
        public double AC_RSS_Employers { get; set; }
        public double AC_RSS_Employees { get; set; }
        public double AC_RSS_Total { get; set; }
        public double AC_EC_Employers { get; set; }
        public double AC_EC_Employees { get; set; }
        public double AC_EC_Total { get; set; }
        public double AC_MPF_Employers { get; set; }
        public double AC_MPF_Employees { get; set; }
        public double AC_MPF_Total { get; set; }
        public double AC_Total_Employers { get; set; }
        public double AC_Total_Employees { get; set; }
        public double AC_Total { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SaveSSSContributinTableData(SSSContributionTable data, double[] RangeOfCompensationFrom, double[] RangeOfCompensationTo, double[] MSC_RSS_EmployeesCompensation,
            double[] MSC_MandatoryProvidentFund, double[] AC_RSS_Employers, double[] AC_RSS_Employees,
            double[] AC_EC_Employers, double[] AC_EC_Employees,
            double[] AC_MPF_Employers, double[] AC_MPF_Employees)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                //delete first all the data in SSSContributionTable
                db.ExecuteCommandNonQuery("HRIS_DeleteAll_SSS",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out b, CommandType.StoredProcedure);
                var ss = new SystemLogs();
                ss.Type = 3;
                ss.Module = 47;
                ss.After = "";
                ss.Before = data.Logs.Before;
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
                //Then save the data in SSSContributionTable
                //saving is in loop
                int x = 0;
                string _after = "";
                foreach (double s in RangeOfCompensationFrom)
                {
                    data.RangeOfCompensationFrom = RangeOfCompensationFrom[x];
                    data.RangeOfCompensationTo = RangeOfCompensationTo[x];
                    data.MSC_RSS_EmployeesCompensation = MSC_RSS_EmployeesCompensation[x];
                    data.MSC_MandatoryProvidentFund = MSC_MandatoryProvidentFund[x];
                    data.AC_RSS_Employees = AC_RSS_Employees[x];
                    data.AC_RSS_Employers = AC_RSS_Employers[x];
                    data.AC_EC_Employees = AC_EC_Employees[x];
                    data.AC_EC_Employers = AC_EC_Employers[x];
                    data.AC_MPF_Employees = AC_MPF_Employees[x];
                    data.AC_MPF_Employers = AC_MPF_Employers[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_SSS",
                    new string[] {  "sRangeOfCompensationFrom", "sRangeOfCompensationTo","sMSC_MandatoryProvidentFund", "sMSC_RSS_EmployeesCompensation","sAC_RSS_Employees",
                                    "sAC_RSS_Employers", "sAC_EC_Employers", "sAC_EC_Employees",
                                    "sAC_MPF_Employers", "sAC_MPF_Employees","sAddedBy", "sModifiedBy", },
                    new DbType[] {  DbType.Double, DbType.Double, DbType.Double,DbType.Double, DbType.Double,
                                    DbType.Double, DbType.Double, DbType.Double,
                                    DbType.Double, DbType.Double,  DbType.String, DbType.String},
                    new object[] {  data.RangeOfCompensationFrom, data.RangeOfCompensationTo,data.MSC_MandatoryProvidentFund,
                                    data.MSC_RSS_EmployeesCompensation,data.AC_RSS_Employees,
                                    data.AC_RSS_Employers,data.AC_EC_Employers,data.AC_EC_Employees,
                                    data.AC_MPF_Employers, data.AC_MPF_Employees,data.AddedBy, data.ModifiedBy}, out a, CommandType.StoredProcedure);
                    _after += "[ID:" + data.ID.ToString() + ", RangeOfCompensationFrom:" + data.RangeOfCompensationFrom + ", RangeOfCompensationTo:" + data.RangeOfCompensationTo + ", " +
                                                 "MSC_MandatoryProvidentFund:" + data.MSC_MandatoryProvidentFund + ", " +
                                                 "MSC_RSS_EmployeesCompensation:" + data.MSC_RSS_EmployeesCompensation + ", " +
                                                 "AC_RSS_Employees:" + data.AC_RSS_Employees + ", AC_RSS_Employers:" + data.AC_RSS_Employers + ", " +
                                                 "AC_EC_Employers:" + data.AC_EC_Employees + ", AC_EC_Employees:" + data.AC_EC_Employers + ", " +
                                                 "AC_MPF_Employers:" + data.AC_MPF_Employees + ", AC_MPF_Employees:" + data.AC_EC_Employers + "],";
                    x += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void Delete(SSSContributionTable data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_SSS",
                   new string[] { "sID" },
                   new DbType[] { DbType.Int64 },
                   new object[] { data.ID }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<SSSContributionTable> GetSSSContributionTableData()
        {
            var listSSS = new List<SSSContributionTable>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_SSS",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    SSSContributionTable s = new SSSContributionTable();
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.RangeOfCompensationFrom = Convert.ToDouble(aRow["RangeOfCompensationFrom"]);
                    s.RangeOfCompensationTo = Convert.ToDouble(aRow["RangeOfCompensationTo"]);
                    s.MSC_RSS_EmployeesCompensation = Convert.ToDouble(aRow["MSC_RSS_EmployeesCompensation"]);
                    s.MSC_MandatoryProvidentFund = Convert.ToDouble(aRow["MSC_MandatoryProvidentFund"]);
                    s.AC_RSS_Employers = Convert.ToDouble(aRow["AC_RSS_Employers"]);
                    s.AC_RSS_Employees = Convert.ToDouble(aRow["AC_RSS_Employees"]);
                    s.AC_EC_Employees = Convert.ToDouble(aRow["AC_EC_Employees"]);
                    s.AC_EC_Employers = Convert.ToDouble(aRow["AC_EC_Employers"]);
                    s.AC_MPF_Employers = Convert.ToDouble(aRow["AC_MPF_Employers"]);
                    s.AC_MPF_Employees = Convert.ToDouble(aRow["AC_MPF_Employees"]);
                    listSSS.Add(s);
                }
            }
            return listSSS;
        }
    }
    public class PHILHealthContributionTable
    {
        public long ID { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public string Percent { get; set; }
        public double BaseAmount { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SavePHILHealthContributionTableData(PHILHealthContributionTable data, double[] to, double[] from, string[] percent, double[] baseamount)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteAll_PhilhealthData",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, CommandType.StoredProcedure);
                var ss = new SystemLogs();
                ss.Type = 3;
                ss.Module = 48;
                ss.After = "";
                ss.Before = data.Logs.Before;
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
                int c = 0;
                string _after = "";
                foreach (double counter in from)
                {
                    data.From = from[c];
                    data.To = to[c];
                    data.Percent = percent[c];
                    data.BaseAmount = baseamount[c];
                    db.ExecuteCommandNonQuery("HRIS_Save_PhilhealthData",
                    new string[] { "pTo", "pFrom", "pPercent", "pBaseAmount", "pAddedBy", "pModifiedBy" },
                    new DbType[] { DbType.Double, DbType.Double, DbType.String, DbType.Double, DbType.String, DbType.String },
                    new object[] { data.To, data.From, data.Percent, data.BaseAmount, data.AddedBy, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                    _after += "[ID:" + data.ID.ToString() + ", To:" + data.To + ", From:" + data.From + ", Percent:" + data.Percent + ", BaseAmount:" + data.BaseAmount + " ],";
                    c += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeletePHILHealthContributionTableData(PHILHealthContributionTable data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_PhilhealthData",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.ID }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<PHILHealthContributionTable> GetPHILHealthContributionTableData()
        {
            var philHealthList = new List<PHILHealthContributionTable>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_PhilhealthData",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PHILHealthContributionTable p = new PHILHealthContributionTable();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.To = Convert.ToDouble(aRow["To"]);
                    p.From = Convert.ToDouble(aRow["From"]);
                    p.Percent = aRow["Percent"].ToString();
                    p.BaseAmount = Convert.ToDouble(aRow["BaseAmount"]);
                    philHealthList.Add(p);
                }
            }
            return philHealthList;
        }
    }
    public class PagibigContributionTable
    {
        public long ID { get; set; }
        public double MC_From { get; set; }
        public double MC_To { get; set; }
        public string PMC_EmployeeShare { get; set; }
        public string PMC_EmployerShare { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SavePagibigTableData(PagibigContributionTable data, double[] MC_From, double[] MC_To, string[] PMC_EmployeeShare, string[] PMC_EmployerShare)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int x = 0;
                //delete first all data before saving the new data
                db.ExecuteCommandNonQuery("HRIS_DeleteAll_PAGIBIG",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, CommandType.StoredProcedure);
                var ss = new SystemLogs();
                ss.Type = 3;
                ss.Module = 49;
                ss.After = "";
                ss.Before = data.Logs.Before;
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
                //saving is in array that can be accessed by index [x]
                string _after = "";
                foreach (double counter in MC_From)
                {
                    data.MC_From = MC_From[x];
                    data.MC_To = MC_To[x];
                    data.PMC_EmployeeShare = PMC_EmployeeShare[x];
                    data.PMC_EmployerShare = PMC_EmployerShare[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_PAGIBIG",
                      new string[] { "pMC_From", "pMC_To", "pPMC_EmployeeShare", "pPMC_EmployerShare", "pAddedBy", "pModifiedBy" },
                      new DbType[] { DbType.Double, DbType.Double, DbType.String, DbType.String, DbType.String, DbType.String },
                      new object[] { data.MC_From, data.MC_To, data.PMC_EmployeeShare, data.PMC_EmployerShare, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                    _after += "[ID:" + data.ID.ToString() + ",MC_From:" + data.MC_From + ",  MC_To:" + data.MC_To + ", " +
                        "PMC_EmployeeShare:" + data.PMC_EmployeeShare + ", PMC_EmployerShare:" + data.PMC_EmployerShare + "], ";
                    x += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeletePagibigTableDataByID(PagibigContributionTable data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_PAGIBIG",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.ID }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<PagibigContributionTable> GetPagibigTableDatas()
        {
            var pagibigList = new List<PagibigContributionTable>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_PAGIBIG",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    PagibigContributionTable p = new PagibigContributionTable();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.MC_From = Convert.ToDouble(aRow["MC_From"]);
                    p.MC_To = Convert.ToDouble(aRow["MC_To"]);
                    p.PMC_EmployeeShare = aRow["PMC_EmployeeShare"].ToString();
                    p.PMC_EmployerShare = aRow["PMC_EmployerShare"].ToString();
                    pagibigList.Add(p);
                }
            }
            return pagibigList;
        }
    }
    //Leave Credits
    public class EmployeeLeaveCredits
    {
        public long ID { get; set; }
        public DateTime OTDate { get; set; }
        public Employee Employee { get; set; }
        public LeaveTypeCode LeaveType { get; set; }
        public int UsedCredit { get; set; }
        public int AvailableCredit { get; set; }
        public int NumberOfCredit { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Year { get; set; }
        public Leave Leave { get; set; }
        public EmployeeClassCode Class { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SaveEmployeeLeaveCredits(EmployeeLeaveCredits data)
        {
            using (AppDb db = new AppDb())
            {
                var empList = new List<string>();//initialize list for the saving of employee id
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                //first search for the employees who has emplcass code  of the given parameters
                db.ExecuteCommandReader("HRIS_GetEmployeeByClass_EmployeePortal",
                    new string[] { "oClass" },
                    new DbType[] { DbType.String },
                    new object[] { data.Class.Code }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    empList.Add(aRow["EmployeeID"].ToString());//fetch results here
                }
                string _after = "";
                int b = 0;
                int x = 0;
                foreach (string e in empList)
                {
                    data.Employee.EmployeeID = empList[x];

                    db.ExecuteCommandNonQuery("HRIS_Save_StartingLeaveCredits",
                        new string[] { "oEmployeeID", "oYear", "oLeaveType", "oNumberOfCredits", "oAddedBy", "oModifiedBy" },
                        new DbType[] { DbType.String, DbType.Int32, DbType.Int32, DbType.Int32, DbType.String, DbType.String },
                        new object[] { data.Employee.EmployeeID, data.Year, data.Leave.Code, data.NumberOfCredit, data.AddedBy, data.ModifiedBy }, out b, CommandType.StoredProcedure);

                    x += 1;
                }
            }
        }

    }
    public class LeaveForCrediting
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public EmployeeClassCode Class { get; set; }
        public LeaveTypeCode LeaveType { get; set; }
        public Leave Leave { get; set; }
        public int Year { get; set; }
        public DateTime DateAdded { get; set; }
        public EmployeeTypeCode Type { get; set; }
        public string AddedBy { get; set; }
        public LeaveComputation Computation { get; set; }
        public AnnualLeaveCredits AnnualCredits { get; set; }
        public SystemLogs Logs { get; set; }
        public static void CreditNewLeave(LeaveForCrediting details)
        {
            long _leaveId = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_SaveNew_AnnualLeaveCredits",
                    new string[] { "oEmployeeID", "oEmployeeClass", "oLeaveType", "oCredits", "oYear", "oAddedBy", "oEmployeeType" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Double, DbType.Int32, DbType.String, DbType.Int64 },
                    new object[] { details.Employee.ID, details.Class.ID, details.LeaveType.ID, details.Leave.LeaveCredits, details.Year, details.AddedBy, details.Type.ID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _leaveId = Convert.ToInt64(aRow["ID"]);
                    db.ExecuteCommandNonQuery("HRIS_Save_LeaveCreditsComputation",
                        new string[] { "oLeaveType", "oEmployeeID", "oAvailable" },
                        new DbType[] { DbType.Int64, DbType.Int64, DbType.Double },
                        new object[] { details.LeaveType.ID, details.Employee.ID, details.Computation.Available }, out b, CommandType.StoredProcedure);
                    details.Logs.After = "AnnualLeaveCredits[ID:" + details.ID.ToString() + ", EmployeeID:" + details.Employee.ID.ToString() + ", " +
                        "LeaveType:" + details.LeaveType.ID.ToString() + ", Credits:" + details.Leave.LeaveCredits + ", Year:" + details.Year + "], " +
                        "Computation:[ LeaveType:" + details.LeaveType.ID.ToString() + ", EmployeeID:" + details.Employee.ID.ToString() + ", AvailableCredits:" + details.Computation.Available.ToString() + ",  ]";
                    SystemLogs.SaveSystemLogs(details.Logs);
                }
            }
        }
        public static List<LeaveForCrediting> GetEmployeesWithoutLeaveCredits(Int64 EmpClass, Int32 year, Int64 LeaveType)
        {
            var _oList = new List<LeaveForCrediting>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeeForLeaveDistribution_AnnualLeaveCredits",
                    new string[] { "oEmpClass", "oYear", "oLeaveType" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64 },
                    new object[] { EmpClass, year, LeaveType }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveForCrediting l = new LeaveForCrediting();
                    l.Employee = new Employee();
                    l.Leave = new Leave();
                    l.LeaveType = new LeaveTypeCode();
                    l.Type = new EmployeeTypeCode();
                    l.Class = new EmployeeClassCode();
                    l.Employee.Department = new Department();
                    l.Employee.Division = new Division();
                    l.Employee.Position = new PositionCode();
                    l.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Type = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmploymentType"]));
                    l.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmploymentClassification"]));
                    l.Employee.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                    l.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    l.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    l.Employee.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    l.Employee.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    _oList.Add(l);
                }
            }
            return _oList;
        }
        public static LeaveForCrediting etEmployeesWithoutLeaveCreditsByID(Int64 EmpID, Int64 leaveType)
        {
            var l = new LeaveForCrediting();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeeByIDForLeaveDistribution_AnnualLeaveCredits",
                    new string[] { "oEmpID", "oLeaveType" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64 },
                    new object[] { EmpID, leaveType }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    l.Employee = new Employee();
                    l.Leave = new Leave();
                    l.LeaveType = new LeaveTypeCode();
                    l.Type = new EmployeeTypeCode();
                    l.Class = new EmployeeClassCode();
                    l.Employee.Department = new Department();
                    l.Employee.Division = new Division();
                    l.Employee.Position = new PositionCode();
                    l.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Type = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmploymentType"]));
                    l.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmploymentClassification"]));
                    l.Employee.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                    l.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    l.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    //l.Employee.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    // l.Employee.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    l.Leave.LeaveCredits = Convert.ToInt32(aRow["LeaveCredits"]);

                }
            }
            return l;
        }
        public static List<Leave> GetLeaveTypeFromSetUpbyClass(Int64 empClass)
        {
            var _list = new List<Leave>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTabe = new DataTable();
                db.ExecuteCommandReader("HRIS_GetLeaveTypeByClassCode_LeaveCredits",
                    new string[] { "oEmpClass" },
                    new DbType[] { DbType.Int64 },
                    new object[] { empClass }, out a, ref aTabe, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTabe.Rows)
                {
                    Leave v = new Leave();
                    v.LeaveType = new LeaveTypeCode();
                    v.ID = Convert.ToInt64(aRow["ID"]);
                    v.LeaveType.Description = aRow["Description"].ToString();
                    _list.Add(v);
                }

            }
            return _list;
        }
        public static List<LeaveForCrediting> GetAccountsWithLeaveCreditedBy(Int64 EmpClass, Int32 year, Int64 leaveType)
        {
            var _list = new List<LeaveForCrediting>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetCredited_AnnualLeaveCredits",
                    new string[] { "oEmpClass", "oYear", "oLeaveType" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64 },
                    new object[] { EmpClass, year, leaveType }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveForCrediting l = new LeaveForCrediting();
                    l.Employee = new Employee();
                    l.Leave = new Leave();
                    l.LeaveType = new LeaveTypeCode();
                    l.Type = new EmployeeTypeCode();
                    l.Class = new EmployeeClassCode();
                    l.Employee.Department = new Department();
                    l.Computation = new LeaveComputation();
                    l.Employee.Division = new Division();
                    l.AnnualCredits = new AnnualLeaveCredits();
                    l.Employee.Position = new PositionCode();
                    l.AnnualCredits.ID = Convert.ToInt64(aRow["AnnualLeaveID"]);
                    l.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Type = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmploymentType"]));
                    l.Class = EmployeeClassCode.GetByID(Convert.ToInt64(aRow["EmploymentClassification"]));
                    l.Employee.Position = PositionCode.GetByID(Convert.ToInt64(aRow["Position"]));
                    l.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    l.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    l.Employee.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    l.Employee.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    //l.Leave.LeaveCredits = Convert.ToInt32(aRow["LeaveCredits"]);
                    l.Computation.Used = Convert.ToDouble(aRow["Used"]);
                    l.Computation.Available = Convert.ToDouble(aRow["Available"]);
                    l.Year = Convert.ToInt32(aRow["Year"]);
                    l.DateAdded = Convert.ToDateTime(aRow["DateAdded"]);
                    l.LeaveType = LeaveTypeCode.GetLeaveTypeCodeByID(Convert.ToInt64(aRow["LeaveType"]));
                    l.AnnualCredits.Credits = Convert.ToDouble(aRow["Credits"]);
                    _list.Add(l);
                }

            }
            return _list;
        }
        public static void CancelLeaveCredits(LeaveForCrediting details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_CancelLeaveCreditation_AnnualLeaveCredits",
                    new string[] { "oID", "oEmpclass", "oEmployeeID", "oLeaveType" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { details.AnnualCredits.ID, details.Class.ID, details.Employee.ID, details.LeaveType.ID }, out a, CommandType.StoredProcedure);
                details.Logs.After = "";
                SystemLogs.SaveSystemLogs(details.Logs);
            }
        }
    }
    public class DepartmentApprovers
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Employee Employee { get; set; }
        public Department Department { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static void AddApprover(DepartmentApprovers data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_AddApprover_Department",
                    new string[] { "oEmployeeID", "oDepartmentID", "oAddedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.Employee.ID, data.Department.ID, data.AddedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void ChangeApprover(DepartmentApprovers data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_ChangeApprover_Department",
                    new string[] { "oID", "oEmployeeID", "oDepartmentID", "oModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Employee.ID, data.Department.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteApprover(DepartmentApprovers data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_DeleteApprover_Department",
                    new string[] { "oID", "oEmployeeID", "oDepartmentID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { data.ID, data.Employee.ID, data.Department.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<DepartmentApprovers> GetApprovers()
        {
            var _listD = new List<DepartmentApprovers>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovers_Department",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    DepartmentApprovers d = new DepartmentApprovers();
                    d.Employee = new Employee();
                    d.Department = new Department();
                    d.Employee.Position = new PositionCode();
                    d.Employee.Position.Position = aRow["Position"].ToString();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Name = aRow["Name"].ToString();
                    d.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    d.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentID"]));
                    _listD.Add(d);
                }
            }
            return _listD;
        }
        public static List<DepartmentApprovers> GetApproversFilterByID(Int64 depID)
        {
            var _listD = new List<DepartmentApprovers>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApproverByDepartmentID_Department",
                    new string[] { "oDepartmentID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { depID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    DepartmentApprovers d = new DepartmentApprovers();
                    d.Employee = new Employee();
                    d.Department = new Department();
                    d.Employee.Position = new PositionCode();
                    d.Employee.Position.Position = aRow["Position"].ToString();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Name = aRow["Name"].ToString();
                    d.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    d.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentID"]));
                    _listD.Add(d);
                }
            }
            return _listD;
        }
        public static DepartmentApprovers GetApproversByID(Int64 Id, Int64 empID)
        {
            var d = new DepartmentApprovers();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApproversByID_Department",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { Id, empID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    d.Employee = new Employee();
                    d.Department = new Department();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Name = aRow["Name"].ToString();
                    d.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    d.Department = Department.GetByID(Convert.ToInt64(aRow["DepartmentID"]));

                }
            }
            return d;
        }
    }
    public class DivisionApprovers
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Employee Employee { get; set; }
        public Division Division { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static void AddApprover(DivisionApprovers data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_AddApprover_Division",
                    new string[] { "oEmployeeID", "oDivisionID", "oAddedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.Employee.ID, data.Division.ID, data.AddedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void ChangeApprover(DivisionApprovers data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_ChangeApprover_Division",
                    new string[] { "oID", "oEmployeeID", "oDivisionID", "oModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Employee.ID, data.Division.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteApprover(DivisionApprovers data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_DeleteApprover_Division",
                    new string[] { "oID", "oEmployeeID", "oDivisionID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { data.ID, data.Employee.ID, data.Division.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<DivisionApprovers> GetApprovers()
        {
            var _listD = new List<DivisionApprovers>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovers_Division",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    DivisionApprovers d = new DivisionApprovers();
                    d.Employee = new Employee();
                    d.Division = new Division();
                    d.Employee.Position = new PositionCode();
                    d.Employee.Position.Position = aRow["Position"].ToString();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Name = aRow["Name"].ToString();
                    d.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    d.Division = Division.GetByID(Convert.ToInt64(aRow["DivisionID"]));
                    _listD.Add(d);
                }
            }
            return _listD;
        }
        public static List<DivisionApprovers> GetApproversFilterByID(Int64 depID)
        {
            var _listD = new List<DivisionApprovers>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApproverByDepartmentID_Division",
                    new string[] { "oDivisionID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { depID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    DivisionApprovers d = new DivisionApprovers();
                    d.Employee = new Employee();
                    d.Division = new Division();
                    d.Employee.Position = new PositionCode();
                    d.Employee.Position.Position = aRow["Position"].ToString();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Name = aRow["Name"].ToString();
                    d.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    d.Division = Division.GetByID(Convert.ToInt64(aRow["DivisionID"]));
                    _listD.Add(d);
                }
            }
            return _listD;
        }
        public static DivisionApprovers GetApproversByID(Int64 Id, Int64 empID)
        {
            var d = new DivisionApprovers();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApproversByID_Division",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { Id, empID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    d.Employee = new Employee();
                    d.Division = new Division();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Name = aRow["Name"].ToString();
                    d.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    d.Division = Division.GetByID(Convert.ToInt64(aRow["DivisionID"]));
                }
            }
            return d;
        }
    }
    public class ProjectWorkSchedule
    {
        public long ID { get; set; }
        public WorkSchedule Schedule { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public ProjectCode Project { get; set; }
        public EmployeeTypeCode Type { get; set; }
        public Employee Employee { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public CompanyInfo Company { get; set; }
        public Branches Branch { get; set; }

        /*
         Project = Company
         SalesOrder = Branch
         */
        public static string SaveBranchWorkSchedule(ProjectWorkSchedule data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                int z = 0;
                DataTable zTable = new DataTable();
                DataTable yTable = new DataTable();
                DateTime today = DateTime.Today;
                DayOfWeek currentDayOfWeek = today.DayOfWeek;
                // Calculate the number of days until the next Monday (start of next week)
                int daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDayOfWeek + 7) % 7;
                // Calculate the start date of next week
                DateTime startDateOfNextWeek = today.AddDays(daysUntilNextMonday);
                // Calculate the end date of next week
                DateTime endDateOfNextWeek = startDateOfNextWeek.AddDays(6);
                if (daysUntilNextMonday == 0)
                {
                    startDateOfNextWeek = today.AddDays(7);
                    endDateOfNextWeek = startDateOfNextWeek.AddDays(6);
                }
                // current week

                // Calculate the number of days until the previous Monday (start of current week)
                int daysUntilPreviousMonday = ((int)currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

                // Calculate the start date of the current week
                DateTime startDateOfCurrentWeek = today.AddDays(-daysUntilPreviousMonday);

                // Calculate the end date of the current week
                DateTime endDateOfCurrentWeek = startDateOfCurrentWeek.AddDays(6);

                db.ExecuteCommandReader("hris_isEmployeeHasCurrentSchedule_WorkSchedule",
                    new string[] { "eEmpId","eStart", "eEnd" },
                    new DbType[] { DbType.Int64, DbType.Date, DbType.Date },
                    new object[] {data.Employee.ID, startDateOfCurrentWeek, endDateOfCurrentWeek }, out z, ref zTable, CommandType.StoredProcedure);
                if (zTable.Rows.Count > 0)
                {
                    if (data.Start == startDateOfNextWeek && data.End == endDateOfNextWeek)
                    {
                        res = "Schedule for this Employee is already set.";
                    }
                    else if (data.Start > endDateOfNextWeek)
                    {
                        res = "Weekly schedule for this Employee  is already set.";
                    }
                    else
                    {
                        db.ExecuteCommandReader("HRIS_CheckIFHasBreakSchedule_WorkSchedule",
                        new string[] { "wWorkSchedule" },
                        new DbType[] { DbType.Int64 },
                        new object[] { data.Schedule.ID }, out y, ref yTable, CommandType.StoredProcedure);
                        if (yTable.Rows.Count > 0)
                        {
                            res = "Break schedules are not set. Please check if break schedule for this work schedule is set for all the days and try again.";
                        }
                        else
                        {
                            db.ExecuteCommandNonQuery("HRIS_Save_ProjectWorkSchedule",
                                 new string[] { "wScheduleID", "wCompany", "wBranch", "wEmployeeId", "wStart", "wEnd", "wAddedBy", "wModifiedBy" },
                                 new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Date, DbType.String, DbType.String },
                                 new object[] { data.Schedule.ID, data.Company.ID, data.Branch.ID, data.Employee.ID, startDateOfNextWeek, endDateOfNextWeek, data.AddedBy, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                            var emp = Employee.GetEmployeNameByID(data.Employee.ID);
                            var branch = Branches.GetBranchesById(data.Branch.ID);
                            var company = CompanyInfo.GetListById(data.Company.ID);
                            data.Logs.After = "ID:" + data.ID.ToString() + ", ScheduleID::" + data.Schedule.ID.ToString() + ", Company:" + company.Name.ToString() + ", " +
                                                        "Employment:" + emp.ID.ToString() + " " + emp.Firstname.ToString() + " " + emp.Middlename.ToString() + " " + emp.Lastname.ToString() + " " + emp.Suffix.ToString() + "" +
                                                        " Branch:" + branch.BranchCode.ToString() + "-" + branch.BranchName.ToString() + ", StartDate: " + data.Start.ToString("MM-dd-yyyy") + ", " +
                                                        " EndDate:" + data.End.ToString("MM-dd-yyyy") + ".";
                            SystemLogs.SaveSystemLogs(data.Logs);
                            res = "ok";
                        }
                    }
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_CheckIFHasBreakSchedule_WorkSchedule",
                        new string[] { "wWorkSchedule" },
                        new DbType[] { DbType.Int64 },
                        new object[] { data.Schedule.ID }, out y, ref yTable, CommandType.StoredProcedure);
                    if (yTable.Rows.Count > 0)
                    {
                        res = "Break schedules are not set. Please check if break schedule for this work schedule is set for all the days and try again.";
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Save_ProjectWorkSchedule",
                             new string[] { "wScheduleID", "wCompany", "wBranch", "wEmployeeId", "wStart", "wEnd", "wAddedBy", "wModifiedBy" },
                             new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Date, DbType.String, DbType.String },
                             new object[] { data.Schedule.ID, data.Company.ID, data.Branch.ID, data.Employee.ID, startDateOfCurrentWeek, endDateOfCurrentWeek, data.AddedBy, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                        var emp = Employee.GetEmployeNameByID(data.Employee.ID);
                        var branch = Branches.GetBranchesById(data.Branch.ID);
                        var company = CompanyInfo.GetListById(data.Company.ID);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", ScheduleID::" + data.Schedule.ID.ToString() + ", Company:" + company.Name.ToString() + ", " +
                                                    "Employment:" + emp.ID.ToString() + " " + emp.Firstname.ToString() + " " + emp.Middlename.ToString() + " " + emp.Lastname.ToString() + " " + emp.Suffix.ToString() + "" +
                                                    " Branch:" + branch.BranchCode.ToString() + "-" + branch.BranchName.ToString() + ", StartDate: " + data.Start.ToString("MM-dd-yyyy") + ", " +
                                                    " EndDate:" + data.End.ToString("MM-dd-yyyy") + ".";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "ok";
                    }
                }
            }
            return res;
        }
        public static string EditBranchWorkSchedule(ProjectWorkSchedule data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_ProjectWOrkSchedule", 
                          new string[] { "wID", "wScheduleID", "wCompany", "wBranch", "wEmployeeId",  "wModifiedBy" },
                          new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Date, DbType.String },
                          new object[] { data.ID, data.Schedule.ID, data.Company.ID, data.Branch.ID, data.Employee.ID, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                var emp = Employee.GetEmployeNameByID(data.Employee.ID);
                var branch = Branches.GetBranchesById(data.Branch.ID);
                var company = CompanyInfo.GetListById(data.Company.ID);
                data.Logs.After = "ID:" + data.ID.ToString() + ", ScheduleID::" + data.Schedule.ID.ToString() + ", Company:" + company.Name.ToString() + ", " +
                                            "Employment:" + emp.ID.ToString() + " " + emp.Firstname.ToString() + " " + emp.Middlename.ToString() + " " + emp.Lastname.ToString() + " " + emp.Suffix.ToString() + "" +
                                            " Branch:" + branch.BranchCode.ToString() + "-" + branch.BranchName.ToString() + ".";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
            return res;
        }
        public static void DeleteProjectWorkSchedule(ProjectWorkSchedule data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_ProjectWorkSchedule",
                    new string[] { "wID", "wModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<ProjectWorkSchedule> GetProjectWorkSchedules()
        {
            var _list = new List<ProjectWorkSchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_ProjectWorkSchedule",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    ProjectWorkSchedule p = new ProjectWorkSchedule();
                    p.SalesOrder = new SalesOrder();
                    p.Project = new ProjectCode();
                    p.Schedule = new WorkSchedule();
                    p.Type = new EmployeeTypeCode();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    
                    p.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));
                    p.Employee = new Employee();
                    p.Branch = new Branches();
                    p.Company = new CompanyInfo();
                    p.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    p.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    p.Employee.Firstname = aRow["Firstname"].ToString();
                    p.Employee.Lastname = aRow["Lastname"].ToString();
                    p.Employee.Middlename = aRow["MiddleName"].ToString();
                    p.Employee.Suffix = aRow["Suffix"].ToString();
                    p.Branch.ID = Convert.ToInt64(aRow["Branch"]);
                    p.Branch.BranchCode = aRow["BranchCode"].ToString();
                    p.Branch.BranchName = aRow["BranchName"].ToString();
                    p.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    p.Company.Name = aRow["CompanyName"].ToString();
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static ProjectWorkSchedule GetProjectWorkScheduleByID(Int64 Id)
        {
            var p = new ProjectWorkSchedule();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_ProjectWorkSchedule",
                 new string[] { "wID" },
                 new DbType[] { DbType.Int64 },
                 new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    p.SalesOrder = new SalesOrder();
                    p.Project = new ProjectCode();
                    p.Schedule = new WorkSchedule();
                    p.Type = new EmployeeTypeCode();
                    p.ID = Convert.ToInt64(aRow["ID"]);
                    p.Type = EmployeeTypeCode.GetByID(Convert.ToInt64(aRow["EmployeeType"]));
                    p.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));
                    p.Employee = new Employee();
                    p.Branch = new Branches();
                    p.Company = new CompanyInfo();
                    p.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    p.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    p.Employee.Firstname = aRow["Firstname"].ToString();
                    p.Employee.Lastname = aRow["Lastname"].ToString();
                    p.Employee.Middlename = aRow["MiddleName"].ToString();
                    p.Employee.Suffix = aRow["Suffix"].ToString();
                    p.Branch.ID = Convert.ToInt64(aRow["Branch"]);
                    p.Branch.BranchCode = aRow["BranchCode"].ToString();
                    p.Branch.BranchName = aRow["BranchName"].ToString();
                    p.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    p.Company.Name = aRow["CompanyName"].ToString();
                }
            }
            return p;
        }
    }
    public class DailyWorkSchedule
    {
        public WorkSchedule Schedule { get; set; }
        public long ID { get; set; }
        public int Day { get; set; }
        public string StartTime { get; set; }
        public string EndtTime { get; set; }
        public int Status { get; set; }
        public BreakSchedule BreakSchedule { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveDailyWorkSchedule(DailyWorkSchedule data, Int64 ScheduleID, Int32[] Days, string[] STime, string[] ETime)
        {
            //numbered the days from 1-7 to insert in the dbtable eg. Monday = 1, Tuesday = 2...
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int m = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_DailyWorkSchedule",
                  new string[] { "wScheduleID" },
                  new DbType[] { DbType.Int64 },
                  new object[] { ScheduleID }, out x, CommandType.StoredProcedure);
                //log the process of deletion
                var ss = new SystemLogs();
                ss.Type = 3;
                ss.Module = 56;
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                ss.Before = data.Logs.Before;
                ss.After = "";
                SystemLogs.SaveSystemLogs(ss);
                string _after = "";
                foreach (long z in Days)
                {
                    data.Schedule = new WorkSchedule();
                    data.Schedule.ID = ScheduleID;
                    data.Day = Days[m];

                    if (STime[m] != null && ETime[m] != null)
                    {
                        data.StartTime = STime[m];
                        data.EndtTime = ETime[m];
                        data.Status = 1;
                    }
                    else
                    {
                        data.StartTime = "00:00";
                        data.EndtTime = "00:00";
                        data.Status = 0;
                    }
                    DataTable xTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_Save_DailyWorkSchedule",
                     new string[] { "wScheduleID", "wDay", "wStartTime", "wEndTime", "wStatus" },
                     new DbType[] { DbType.Int64, DbType.Int32, DbType.Time, DbType.Time, DbType.Int32 },
                     new object[] { data.Schedule.ID, data.Day, data.StartTime, data.EndtTime, data.Status }, out x, ref xTable, CommandType.StoredProcedure);
                    if (xTable.Rows.Count > 0)
                    {
                        DataRow xRow = xTable.Rows[0];
                        data.BreakSchedule = new BreakSchedule();
                        data.BreakSchedule.Schedule = new DailyWorkSchedule();
                        data.BreakSchedule.Schedule.ID = Convert.ToInt64(xRow["ID"]);
                        data.BreakSchedule.BreakStartTime = "00:00:00";
                        data.BreakSchedule.BreakEndTime = "00:00:00";
                        db.ExecuteCommandNonQuery("HRIS_Save_BreakSchedule",
                            new string[] { "bDailyWorkScheduleID", "bStartTIme", "bEndTIme" },
                            new DbType[] { DbType.Int64, DbType.Time, DbType.Time },
                            new object[] { data.BreakSchedule.Schedule.ID, data.BreakSchedule.BreakStartTime, data.BreakSchedule.BreakEndTime }, out x, CommandType.StoredProcedure);
                        _after += "[WorkSchedule:(ID:" + data.ID.ToString() + ", ScheduleID:" + data.Schedule.ID.ToString() + ",  Day:" + data.Day.ToString() + ", StartTime:" + data.StartTime + ", EndTime:" +
                            "" + data.EndtTime + ",  Status::" + data.Status + ")],";
                    }
                    m += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);

            }
        }
        public static void EditDailyWorkSchedule(DailyWorkSchedule data, Int64[] WID, Int32[] WDays, string[] WSTime, string[] WETime)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int m = 0;
                string _after = "";
                foreach (long z in WID)
                {
                    data.ID = WID[m];
                    data.Day = WDays[m];
                    if ((WSTime[m] != null && WETime[m] != null) && (WSTime[m] != "00:00" && WETime[m] != "00:00"))
                    {
                        data.StartTime = WSTime[m];
                        data.EndtTime = WETime[m];
                        data.Status = 1;
                    }
                    else
                    {
                        data.StartTime = "00:00";
                        data.EndtTime = "00:00";
                        data.Status = 0;
                    }

                    db.ExecuteCommandNonQuery("HRIS_Edit_DailyWorkSchedule",
                        new string[] { "wID", "wDay", "wStartTime", "wEndTime", "wStatus" },
                        new DbType[] { DbType.Int64, DbType.Int32, DbType.Time, DbType.Time, DbType.Int32 },
                        new object[] { data.ID, data.Day, data.StartTime, data.EndtTime, data.Status }, out x, CommandType.StoredProcedure);
                    _after += "[WorkSchedule:(ID:" + data.ID.ToString() + ",   Day:" + data.Day.ToString() + ", StartTime:" + data.StartTime + ", EndTime:" +
                         "" + data.EndtTime + ",  Status::" + data.Status + ")],";
                    m += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<DailyWorkSchedule> GetDailyWorkScheduleByScheduleID(Int64 SchedID)
        {
            var _list = new List<DailyWorkSchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByScheduleID_DailyWorkSchedule",
                    new string[] { "wScheduleID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SchedID }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    DailyWorkSchedule d = new DailyWorkSchedule();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Day = Convert.ToInt32(aRow["Day"]);
                    d.StartTime = aRow["StartTime"].ToString();
                    d.EndtTime = aRow["EndTime"].ToString();
                    d.Status = Convert.ToInt32(aRow["Status"]);
                    _list.Add(d);
                }
            }
            return _list;
        }

    }
    public class BreakSchedule
    {
        public long BreakID { get; set; }
        public DailyWorkSchedule Schedule { get; set; }
        public string BreakStartTime { get; set; }
        public string BreakEndTime { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveBreakSchedule(BreakSchedule data, Int64 SchedID, string[] BStartTime, string[] BEndTime)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                int m = 0;
                data.Schedule = new DailyWorkSchedule();
                db.ExecuteCommandNonQuery("HRIS_Delete_BreakSchedule",
                     new string[] { "bScheduleID" },
                     new DbType[] { DbType.Int64 },
                     new object[] { SchedID }, out y, CommandType.StoredProcedure);
                var ss = new SystemLogs();
                ss.Type = 3;
                ss.After = "";
                ss.Module = 58;
                ss.Before = data.Logs.Before;
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
                string _after = "";
                foreach (string w in BStartTime)
                {
                    data.Schedule.ID = SchedID;
                    if ((BStartTime[m] != null && BStartTime[m] != null) && (BStartTime[m] != "00:00" && BStartTime[m] != "00:00"))
                    {
                        data.BreakStartTime = BStartTime[m];
                        data.BreakEndTime = BEndTime[m];
                    }
                    else
                    {
                        data.BreakStartTime = "00:00";
                        data.BreakStartTime = "00:00";
                    }

                    db.ExecuteCommandNonQuery("HRIS_Save_BreakSchedule",
                       new string[] { "bDailyWorkScheduleID", "bStartTIme", "bEndTIme" },
                       new DbType[] { DbType.Int64, DbType.Time, DbType.Time },
                       new object[] { data.Schedule.ID, data.BreakStartTime, data.BreakEndTime }, out x, CommandType.StoredProcedure);
                    _after += "[ScheduleID:" + data.Schedule.ID.ToString() + ", BreakStartTime:" + data.BreakStartTime + ", BreakEndTime :" + data.BreakEndTime + "], ";

                    m += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<BreakSchedule> GetBreakSchedulesByDailyWorkScheduleID(Int64 Id)// this Id is the DailyWorkSchedule table ID
        {
            var _list = new List<BreakSchedule>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETBYDailyWorkSchedule_BreakSchedule",
                    new string[] { "bWorkSchedule" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    BreakSchedule b = new BreakSchedule();
                    b.BreakID = Convert.ToInt64(aRow["ID"]);
                    b.BreakStartTime = aRow["StartTime"].ToString();
                    b.BreakEndTime = aRow["EndTime"].ToString();
                    _list.Add(b);
                }
            }
            return _list;
        }
        public static void DeleteBrekSchedule(Int64 Id, Int64 SchedID, BreakSchedule data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteByID_BreakSchedule",
                    new string[] { "bID", "bWorkschedule" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { Id, SchedID }, out x, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
    }
    public class HolidaySetUp
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string AddedBy { get; set; }
        public Province Province { get; set; }
        public City City { get; set; }
        public int Scope { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveHoliday(HolidaySetUp data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_Holiday",
                    new string[] { "hName", "hScope", "hCity", "hProvince", "hDate", "hType", "hAddedBy", "hModifiedBy" },
                    new DbType[] { DbType.String, DbType.Int32, DbType.Int64, DbType.Int64, DbType.Date, DbType.Int32, DbType.String, DbType.String },
                    new object[] { data.Name, data.Scope, data.City.ID, data.Province.ID, data.Date, data.Type, data.AddedBy, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Name::" + data.Name + ", Scope:" + data.Scope + ",  City:" + data.City.ID.ToString() + ",Province:" + data.Province.ID.ToString() + ", " +
                                            "Date:" + data.Date.ToString("MM-dd-yyyy") + ", Type:" + data.Type.ToString() + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void EditHoliday(HolidaySetUp data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Holiday",
                    new string[] { "hID", "hName", "hScope", "hCity", "hProvince", "hDate", "hType", "hModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.Int32, DbType.Int64, DbType.Int64, DbType.Date, DbType.Int32, DbType.String, DbType.String },
                    new object[] { data.ID, data.Name, data.Scope, data.City.ID, data.Province.ID, data.Date, data.Type, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Name::" + data.Name + ", Scope:" + data.Scope + ", City:" + data.City.ID.ToString() + ", Province:" + data.Province.ID.ToString() + ", " +
                                           "Date:" + data.Date.ToString("MM-dd-yyyy") + ", Type:" + data.Type.ToString() + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeleteHoliday(HolidaySetUp data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Holiday",
                    new string[] { "hID", "hModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<HolidaySetUp> GetHolidays()
        {
            var _list = new List<HolidaySetUp>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Holiday",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    HolidaySetUp h = new HolidaySetUp();
                    h.Province = new Province();
                    h.City = new City();
                    h.ID = Convert.ToInt64(aRow["ID"]);
                    h.Name = aRow["Name"].ToString();
                    h.Type = Convert.ToInt32(aRow["Type"]);
                    h.Date = Convert.ToDateTime(aRow["Date"]);
                    h.Scope = Convert.ToInt32(aRow["Scope"]);
                    h.City.ID = Convert.ToInt64(aRow["City"]);
                    h.City.Name = aRow["CityName"].ToString();
                    h.Province.ID = Convert.ToInt64(aRow["Province"]);
                    h.Province.Name = aRow["ProvinceName"].ToString();
                    _list.Add(h);
                }
            }
            return _list;
        }
        public static List<HolidaySetUp> GetHolidaysByYear(Int32 Year)
        {
            var _list = new List<HolidaySetUp>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_SearchByYear_Holiday",
                    new string[] { "hYear" },
                    new DbType[] { DbType.Int32 },
                    new object[] { Year }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    HolidaySetUp h = new HolidaySetUp();
                    h.City = new City();
                    h.Province = new Province();
                    h.ID = Convert.ToInt64(aRow["ID"]);
                    h.Scope = Convert.ToInt32(aRow["Scope"]);
                    h.City.ID = Convert.ToInt64(aRow["City"]);
                    h.City.Name = aRow["CityName"].ToString();
                    h.Province.ID = Convert.ToInt64(aRow["Province"]);
                    h.Province.Name = aRow["ProvinceName"].ToString();
                    h.Name = aRow["Name"].ToString();
                    h.Type = Convert.ToInt32(aRow["Type"]);
                    h.Scope = Convert.ToInt32(aRow["Scope"]);
                    h.Date = Convert.ToDateTime(aRow["Date"]);
                    _list.Add(h);
                }
            }
            return _list;
        }
        public static HolidaySetUp GetHolidayByID(Int64 Id)
        {
            var h = new HolidaySetUp();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Holiday",
                    new string[] { "hID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    h.City = new City();
                    h.Province = new Province();
                    h.ID = Convert.ToInt64(aRow["ID"]);
                    h.City.ID = Convert.ToInt64(aRow["City"]);
                    h.City.Name = aRow["CityName"].ToString();
                    h.Province.ID = Convert.ToInt64(aRow["Province"]);
                    h.Province.Name = aRow["ProvinceName"].ToString();
                    h.Name = aRow["Name"].ToString();
                    h.Type = Convert.ToInt32(aRow["Type"]);
                    h.Date = Convert.ToDateTime(aRow["Date"]);
                    h.Scope = Convert.ToInt32(aRow["Scope"]);
                }
            }
            return h;
        }
    }
    public class StatutoryDeduction
    {
        public long ID { get; set; }
        public PayrollFrequency Frequency { get; set; }
        public int DeductionType { get; set; }
        //public List<int> TypeList { get; set; }
        //public List<StatutoryDeduction> DataSet { get; set; }
        //public int CutOffSet1 { get; set; }
        //public int CutOffSet2 { get; set; }
        //public int CutOffSet3 { get; set; }
        //public int CutOffSet4 { get; set; }
        public int _1st { get; set; }
        public int _2nd { get; set; }
        public int _3rd { get; set; }
        public int _4th { get; set; }
        public int Distribute { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveStatutoryDeductionRef(StatutoryDeduction data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affeectedRows = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_IsDeductionListed_StatutoryDeduction",
                    new string[] { "dFrequencyID", "dDeductionType" },
                    new DbType[] { DbType.Int64, DbType.Int32 },
                    new object[] { data.Frequency.ID, data.DeductionType }, out affeectedRows, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    result = "This deduction is currently listed.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_StatutoryDeduction",
                       new string[] { "dFrequencyID", "dDeductionType", "d1st", "d2nd", "d3rd", "d4th", "dDistribute", "dAddedBy", "dModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.String, DbType.String },
                       new object[] { data.Frequency.ID, data.DeductionType, data._1st, data._2nd, data._3rd, data._4th, data.Distribute, data.AddedBy, data.ModifiedBy }, out affeectedRows, CommandType.StoredProcedure);
                    data.Logs.After = "ID:" + data.ID.ToString() + ", FrequencyID:" + data.Frequency.ID.ToString() + ", DeductionType:" + data.DeductionType.ToString() + ", " +
                        "1st::" + data._1st + ", 2nd:" + data._2nd + ", 3rd:" + data._3rd + ", 4th:" + data._4th + "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    result = "ok";
                }

            }
            return result;
        }
        public static string EditStatutoryDeductionRef(StatutoryDeduction data)
        {
            string result = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affeectedRows = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_IsDeductionListed__Update_StatutoryDeduction",
                   new string[] { "dID", "dFrequencyID", "dDeductionType" },
                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32 },
                   new object[] { data.ID, data.Frequency.ID, data.DeductionType }, out affeectedRows, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    result = "This deduction is currently listed.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Edit_StatutoryDeduction",
                    new string[] { "dID", "dFrequencyID", "dDeductionType", "d1st", "d2nd", "d3rd", "d4th", "dDistribute", "dModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.String },
                    new object[] { data.ID, data.Frequency.ID, data.DeductionType, data._1st, data._2nd, data._3rd, data._4th, data.Distribute, data.ModifiedBy }, out affeectedRows, CommandType.StoredProcedure);
                    data.Logs.After = "ID:" + data.ID.ToString() + ", FrequencyID:" + data.Frequency.ID.ToString() + ", DeductionType:" + data.DeductionType.ToString() + ", " +
                        "1st::" + data._1st + ", 2nd:" + data._2nd + ", 3rd:" + data._3rd + ", 4th:" + data._4th + "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    result = "ok";
                }
            }
            return result;
        }
        public static void DeleteStatutoryDeductionRef(StatutoryDeduction data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affeectedRows = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_StatutoryDeduction",
                    new string[] { "dID", "dModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.ModifiedBy }, out affeectedRows, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<StatutoryDeduction> GetStatutoryDeductionRefs()
        {
            var _list = new List<StatutoryDeduction>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affeectedRows = 0;
                DataTable ResultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_StatutoryDeduction",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out affeectedRows, ref ResultsTable, CommandType.StoredProcedure
                    );
                foreach (DataRow RowResults in ResultsTable.Rows)
                {
                    StatutoryDeduction d = new StatutoryDeduction();
                    d.Frequency = new PayrollFrequency();
                    d.ID = Convert.ToInt64(RowResults["ID"]);
                    d.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(RowResults["FrequencyID"]));
                    d.DeductionType = Convert.ToInt32(RowResults["DeductionType"]);
                    d._1st = Convert.ToInt32(RowResults["1st"]);
                    d._2nd = Convert.ToInt32(RowResults["2nd"]);
                    d._3rd = Convert.ToInt32(RowResults["3rd"]);
                    d._4th = Convert.ToInt32(RowResults["4th"]);
                    d.Distribute = Convert.ToInt32(RowResults["Distribute"]);
                    _list.Add(d);
                }

            }
            return _list;
        }
        public static List<StatutoryDeduction> GetStatutoryDeductionRefsByFrequency(Int64 Id)
        {
            var _list = new List<StatutoryDeduction>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affeectedRows = 0;
                DataTable ResultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByFrequency_StatutoryDeduction",
                    new string[] { "dFrequencyID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out affeectedRows, ref ResultsTable, CommandType.StoredProcedure
                    );
                foreach (DataRow RowResults in ResultsTable.Rows)
                {
                    StatutoryDeduction d = new StatutoryDeduction();
                    d.Frequency = new PayrollFrequency();
                    d.ID = Convert.ToInt64(RowResults["ID"]);
                    d.Frequency = d.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(RowResults["FrequencyID"]));
                    d.DeductionType = Convert.ToInt32(RowResults["DeductionType"]);
                    d._1st = Convert.ToInt32(RowResults["1st"]);
                    d._2nd = Convert.ToInt32(RowResults["2nd"]);
                    d._3rd = Convert.ToInt32(RowResults["3rd"]);
                    d._4th = Convert.ToInt32(RowResults["4th"]);
                    d.Distribute = Convert.ToInt32(RowResults["Distribute"]);
                    _list.Add(d);
                }

            }
            return _list;
        }
        public static StatutoryDeduction GetStatutoryDeductionsRefsByID(Int64 Id)
        {
            var d = new StatutoryDeduction();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affeectedRows = 0;
                DataTable ResultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_StatutoryDeduction",
                    new string[] { "dID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out affeectedRows, ref ResultsTable, CommandType.StoredProcedure
                    );
                if (ResultsTable.Rows.Count > 0)
                {
                    DataRow RowResults = ResultsTable.Rows[0];
                    d.Frequency = new PayrollFrequency();
                    d.ID = Convert.ToInt64(RowResults["ID"]);
                    d.Frequency = d.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(RowResults["FrequencyID"]));
                    d.DeductionType = Convert.ToInt32(RowResults["DeductionType"]);
                    d._1st = Convert.ToInt32(RowResults["1st"]);
                    d._2nd = Convert.ToInt32(RowResults["2nd"]);
                    d._3rd = Convert.ToInt32(RowResults["3rd"]);
                    d._4th = Convert.ToInt32(RowResults["4th"]);
                    d.Distribute = Convert.ToInt32(RowResults["Distribute"]);
                }
            }
            return d;
        }
    }
    public class PayFrequencyDetails
    {
        public long ID { get; set; }
        public PayrollFrequency Frequency { get; set; }
        public PayrollCutOff CutOff { get; set; }
        public int PayDay { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SaveFrequencyDetails(PayFrequencyDetails data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_PayFrequencyDetails",
                    new string[] { "dFrequencyID", "dCutOff", "dPayDay" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32 },
                    new object[] { data.Frequency.ID, data.CutOff.ID, data.PayDay }, out affectedRows, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Frequency:" + data.Frequency.ID + ", CutOff:" + data.CutOff.ID + ", PayDay:" + data.PayDay.ToString() + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void EditFrequencyDetails(PayFrequencyDetails data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_PayFrequencyDetails",
                    new string[] { "dID", "dFrequencyID", "dCutOff", "dPayDay" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32 },
                    new object[] { data.ID, data.Frequency.ID, data.CutOff.ID, data.PayDay }, out affectedRows, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", Frequency:" + data.Frequency.ID + ", CutOff:" + data.CutOff.ID + ", PayDay:" + data.PayDay.ToString() + "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeleteFrequencyDetails(PayFrequencyDetails data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_PayFrequencyDetails",
                    new string[] { "dID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.ID }, out affectedRows, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<PayFrequencyDetails> GetFrequencyDetails()
        {
            var _detailsList = new List<PayFrequencyDetails>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_PayFrequencyDetails",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in resultsTable.Rows)
                {
                    PayFrequencyDetails p = new PayFrequencyDetails();
                    p.Frequency = new PayrollFrequency();
                    p.CutOff = new PayrollCutOff();
                    p.ID = Convert.ToInt64(resultsRow["ID"]);
                    p.Frequency = PayrollFrequency.GetPayrollFrequencyByID(Convert.ToInt64(resultsRow["FrequencyID"]));
                    p.CutOff = PayrollCutOff.GetCutOffByID(Convert.ToInt64(resultsRow["CutOff"]));
                    p.PayDay = Convert.ToInt32(resultsRow["PayDay"]);
                    _detailsList.Add(p);
                }
            }
            return _detailsList;
        }
        public static PayFrequencyDetails GetFrequencyDetailsByID(Int64 Id)
        {
            var p = new PayFrequencyDetails();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_PayFrequencyDetails",
                    new string[] { "dID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                if (resultsTable.Rows.Count > 0)
                {
                    DataRow resultsRow = resultsTable.Rows[0];
                    p.Frequency = new PayrollFrequency();
                    p.CutOff = new PayrollCutOff();
                    p.ID = Convert.ToInt64(resultsRow["ID"]);
                    p.Frequency.ID = Convert.ToInt64(resultsRow["PayrollFrequencyID"]);
                    p.Frequency.Code = resultsRow["Code"].ToString();
                    p.Frequency.Description = resultsRow["Description"].ToString();
                    p.Frequency.NumberOfCutOff = Convert.ToInt32(resultsRow["NumberOfCutOff"]);
                    p.CutOff.ID = Convert.ToInt64(resultsRow["CutOffID"]);
                    p.CutOff.DayFrom = Convert.ToInt32(resultsRow["DayFrom"]);
                    p.CutOff.DayFrom = Convert.ToInt32(resultsRow["DayTo"]);
                    p.CutOff.CutoffCount = resultsRow["CutOffCount"].ToString();
                    p.PayDay = Convert.ToInt32(resultsRow["PayDay"]);
                }
            }
            return p;
        }
    }
    public class PayrollCutOff
    {
        public long ID { get; set; }
        public int DayFrom { get; set; }
        public int DayTo { get; set; }
        public string CutoffCount { get; set; }

        public static List<PayrollCutOff> GetCutOffs()
        {
            var _cutOffList = new List<PayrollCutOff>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_CutOff",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);

                foreach (DataRow resultsRow in resultsTable.Rows)
                {
                    PayrollCutOff p = new PayrollCutOff();
                    p.ID = Convert.ToInt64(resultsRow["ID"]);
                    p.DayFrom = Convert.ToInt32(resultsRow["DayFrom"]);
                    p.DayTo = Convert.ToInt32(resultsRow["DayTo"]);
                    p.CutoffCount = resultsRow["CutOffCount"].ToString();
                    _cutOffList.Add(p);
                }
            }
            return _cutOffList;
        }
        public static PayrollCutOff GetCutOffByID(Int64 Id)
        {
            var p = new PayrollCutOff();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_CutOff",
                     new string[] { "dID" },
                     new DbType[] { DbType.Int64 },
                     new object[] { Id }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                if (resultsTable.Rows.Count > 0)
                {
                    DataRow resultsRow = resultsTable.Rows[0];
                    p.ID = Convert.ToInt64(resultsRow["ID"]);
                    p.DayFrom = Convert.ToInt32(resultsRow["DayFrom"]);
                    p.DayTo = Convert.ToInt32(resultsRow["DayTo"]);
                    p.CutoffCount = resultsRow["CutOffCount"].ToString();
                }
            }
            return p;
        }
    }


    public class LoanTypes
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string SaveLoanType(LoanTypes data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                long aId = 0;
                long bId = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                //check if code exist
                db.ExecuteCommandReader("HRIS_IsCodeExist_LoanType",
                    new string[] { "eCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)// with result
                {
                    DataRow aROw = aTable.Rows[0];
                    aId = Convert.ToInt64(aROw["ID"]);
                    if (aId != 0)//if true
                    {
                        res = "Code in use.";
                    }
                }
                else
                {
                    //check if description is exist and in use by other types.
                    db.ExecuteCommandReader("HRIS_IsDescExist_LoanType",
                        new string[] { "eDescription" },
                        new DbType[] { DbType.String },
                        new object[] { data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        DataRow bROw = bTable.Rows[0];
                        bId = Convert.ToInt64(bROw["ID"]);
                        if (bId != 0)
                        {
                            res = "Description in use.";
                        }
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Save_LoanType",
                               new string[] { "eCode", "eDescription", "eAddedBy", "eModifiedBy" },
                               new DbType[] { DbType.String, DbType.String, DbType.String, DbType.String },
                               new object[] { data.Code, data.Description, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "Save Successfully.";
                    }
                }
            }
            return res;
        }
        public static string UpdateLoanType(LoanTypes data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                long aId = 0;
                long bId = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                //check if code in use
                db.ExecuteCommandReader("HRIS_IsCodeInUse_LoanType",
                    new string[] { "eId", "eCode" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aROw = aTable.Rows[0];
                    aId = Convert.ToInt64(aROw["ID"]);
                    if (aId != 0)//if true
                    {
                        res = "Code in use.";
                    }
                }
                else
                {
                    //check if description is exist and in use by other types.
                    db.ExecuteCommandReader("HRIS_IsDescInUse_LoanType",
                        new string[] { "eId", "eDescription" },
                        new DbType[] { DbType.Int64, DbType.String },
                        new object[] { data.ID, data.Description }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)
                    {
                        DataRow bROw = bTable.Rows[0];
                        bId = Convert.ToInt64(bROw["ID"]);
                        if (bId != 0)
                        {
                            res = "Description in use.";
                        }
                    }
                    else
                    {
                        db.ExecuteCommandNonQuery("HRIS_Edit_LoanType",
                               new string[] { "eId", "eCode", "eDescription", "eModifiedBy" },
                               new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                               new object[] { data.ID, data.Code, data.Description, data.ModifiedBy }, out c, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", Code:" + data.Code + ", Description:" + data.Description + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "Updated Successfully.";
                    }
                }
            }
            return res;
        }
        public static void DeleteLoanType(LoanTypes data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_LoanType",
                    new string[] { "eId", "eCode", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String },
                    new object[] { data.ID, data.Code, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<LoanTypes> GetLoantypes()
        {
            var _list = new List<LoanTypes>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_LoanType",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in resultsTable.Rows)
                {
                    LoanTypes l = new LoanTypes();
                    l.ID = Convert.ToInt64(resultsRow["ID"]);
                    l.Code = resultsRow["Code"].ToString();
                    l.Description = resultsRow["Description"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static LoanTypes GetLoanTypeById(Int64 Id)
        {
            var l = new LoanTypes();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_LoanType",
                    new string[] { "eId" },
                    new DbType[] { DbType.String },
                    new object[] { Id }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                if (resultsTable.Rows.Count > 0)
                {
                    DataRow resultsRow = resultsTable.Rows[0];
                    l.ID = Convert.ToInt64(resultsRow["ID"]);
                    l.Code = resultsRow["Code"].ToString();
                    l.Description = resultsRow["Description"].ToString();
                }
            }
            return l;
        }
    }

    public class MinimumWageIndicators
    {
        public long ID { get; set; }
        public string Indicator { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveMinimumWageIndicator(MinimumWageIndicators details)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_IsIndicatorNameExist_WageIndicator",
                    new string[] { "eIndicator" },
                    new DbType[] { DbType.String },
                    new object[] { details.Indicator }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    res = "Indicator is already exist.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_WageIndicator",
                       new string[] { "eIndicator" },
                       new DbType[] { DbType.String },
                       new object[] { details.Indicator }, out y, CommandType.StoredProcedure);
                    details.Logs.After = "Indicator:" + details.Indicator + "";
                    SystemLogs.SaveSystemLogs(details.Logs);
                    res = "ok";
                }
            }
            return res;
        }
        public static string EditMinimumWageIndicator(MinimumWageIndicators details)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_IsIndicatorNameExist_Update_WageIndicator",
                    new string[] { "eID", "eIndicator" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { details.ID, details.Indicator }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    res = "Indicator is already exist.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Edit_WageIndicator",
                       new string[] { "eID", "eIndicator" },
                       new DbType[] { DbType.Int64, DbType.String },
                       new object[] { details.ID, details.Indicator }, out y, CommandType.StoredProcedure);
                    details.Logs.After = "Indicator:" + details.Indicator + "";
                    SystemLogs.SaveSystemLogs(details.Logs);
                    res = "ok";
                }
            }
            return res;
        }
        public static void DeleteMinimumWageIndicator(MinimumWageIndicators details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_WageIndicator",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { details.ID }, out x, CommandType.StoredProcedure);
                details.Logs.After = "";
                SystemLogs.SaveSystemLogs(details.Logs);
            }
        }
        public static List<MinimumWageIndicators> GetMinimumWageIndicators()
        {
            var _list = new List<MinimumWageIndicators>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_WageIndicator",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    MinimumWageIndicators m = new MinimumWageIndicators();
                    m.ID = Convert.ToInt64(aRow["ID"]);
                    m.Indicator = aRow["Indicator"].ToString();
                    _list.Add(m);
                }
            }
            return _list;
        }
        public static MinimumWageIndicators GetMinimumWageIndicatorByID(Int64 Id)
        {
            var m = new MinimumWageIndicators();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_WageIndicator",
                  new string[] { "eID" },
                  new DbType[] { DbType.Int64 },
                  new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    m.ID = Convert.ToInt64(aRow["ID"]);
                    m.Indicator = aRow["Indicator"].ToString();
                }
            }
            return m;
        }
    }
    public class MinimumWage
    {
        public long ID { get; set; }
        public MinimumWageIndicators Indicator { get; set; }
        public DateTime DateOfIssuance { get; set; }
        public decimal Wage { get; set; }
        public Region Region { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveMinimumWage(MinimumWage details)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_IsWageExist_MinimumWage",
                    new string[] { "eRegion", "eWaGe" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Decimal },
                    new object[] { details.Region.ID, details.Wage }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    res = "Wage for this sector already assigned";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_MinimumWage",
                          new string[] { "eRegion", "eDateOfIssuance", "eWaGe" },
                          new DbType[] { DbType.Int64, DbType.DateTime, DbType.Decimal },
                          new object[] { details.Region.ID, details.DateOfIssuance, details.Wage }, out x, CommandType.StoredProcedure);
                    details.Logs.After = "ID:" + details.ID.ToString() + ", Region:" + details.Region.ID + ", DateOfIssuance:" + details.DateOfIssuance.ToString("MM-dd-yyyy") + ", Wage:" + details.Wage + "";
                    SystemLogs.SaveSystemLogs(details.Logs);
                    res = "ok";
                }
            }
            return res;
        }
        public static string EditMinimumWage(MinimumWage details)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_IsWageExist_Update_MinimumWage",
                    new string[] { "eID", "eRegion", "eWaGe" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Decimal },
                    new object[] { details.ID, details.Region.ID, details.Wage }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    res = "Wage for this sector already assigned";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_edit_MinimumWage",
                        new string[] { "eID", "eRegion", "eDateOfIssuance", "eWaGe" },
                        new DbType[] { DbType.Int64, DbType.Int64, DbType.DateTime, DbType.Decimal },
                        new object[] { details.ID, details.Region.ID, details.DateOfIssuance, details.Wage }, out x, CommandType.StoredProcedure);
                    details.Logs.After = "ID:" + details.ID.ToString() + ", Region:" + details.Region.ID + ", DateOfIssuance:" + details.DateOfIssuance.ToString("MM-dd-yyyy") + ", Wage:" + details.Wage + "";
                    SystemLogs.SaveSystemLogs(details.Logs);
                    res = "ok";
                }
            }
            return res;
        }
        public static void DeleteMinimumWage(MinimumWage details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_delete_MinimumWage",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { details.ID }, out x, CommandType.StoredProcedure);
                details.Logs.After = "";
                SystemLogs.SaveSystemLogs(details.Logs);
            }
        }
        public static List<MinimumWage> GetWages()
        {
            var _list = new List<MinimumWage>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_MinimumWage",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MinimumWage m = new MinimumWage();
                    m.ID = Convert.ToInt64(oRow["ID"]);
                    m.Indicator = new MinimumWageIndicators();
                    m.Region = new Region();
                    m.DateOfIssuance = Convert.ToDateTime(oRow["DateOfIssuance"]);
                    m.Wage = Convert.ToDecimal(oRow["Wage"]);
                    m.Region.ID = Convert.ToInt64(oRow["Region"]);
                    m.Region.Name = oRow["RegDesc"].ToString();
                    _list.Add(m);
                }
            }
            return _list;
        }
        public static MinimumWage GetWageByID(Int64 Id)
        {
            var m = new MinimumWage();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_MinimumWage",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    m.ID = Convert.ToInt64(oRow["ID"]);
                    m.Region = new Region();
                    m.Indicator = new MinimumWageIndicators();
                    m.Wage = Convert.ToDecimal(oRow["Wage"]);
                    m.DateOfIssuance = Convert.ToDateTime(oRow["DateOfIssuance"]);
                    m.Region.ID = Convert.ToInt64(oRow["Region"]);
                    m.Region.Name = oRow["RegDesc"].ToString();
                }
            }
            return m;
        }
    }
}
