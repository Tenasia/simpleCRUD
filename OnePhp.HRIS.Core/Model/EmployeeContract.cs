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
    public class EmployeeContract
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public long ContractNumber { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public DateTime PreparedDate { get; set; }
        public string ContractName { get; set; }
        public string PRNumber { get; set; }
        public DateTime PRDate { get; set; }
        public Assignments Assignment { get; set; }
        public EmployeeClassCode EmployeeClass { get; set; }
        public EmployeeTypeCode EmploymentType { get; set; }
        public SalesOrder SO { get; set; }
        public JobLevel JL { get; set; }
        public ProjectCode Project { get; set; }
        public PositionCode Position { get; set; }
        public Division Division { get; set; }
        public Department Department { get; set; }
        public string Description { get; set; }
        public Employee ImmediateHead { get; set; }
        public string Remarks { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double NewSalary { get; set; }
        public double OldSalary { get; set; }
        public string File { get; set; }
        public double Ecola { get; set; }
        public double OldEcola { get; set; }
        public Employee Preparedby { get; set; }
        public Employee CheckBy { get; set; }
        public Employee ApprovedBy { get; set; }
        public string PayrollAuthorization { get; set; }
        public RefUpdateType UpdateType { get; set; }
        public string Addedby { get; set; }
        public int Status { get; set; }
        public string ModifedBy { get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<EmployeeContract> ExistingContract { get; set; }
        public EmployeePersonal Personal { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime CheckDate { get; set; }
        public Ref_PaySchedule PayRate { get; set; }
        public SystemLogs Logs { get; set; }
        public CompanyInfo Company { get; set; }
        public double Allowance { get; set; }
        public double OldAllowance { get; set; }
        public double TotalCompensation { get; set; }
        public double OldTotalCompensation { get; set; }
        public Department OldDepartment { get; set; }
        public CompanyInfo OldCompany { get; set; }
        public EmployeeClassCode OldLevel { get; set; }
        public EmployeeTypeCode OldEmpStatus { get; set; }
        public PositionCode OldPosition { get; set; }
        public string YearsOfService { get; set; }

        public static void SaveContract(EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_Contract",
                   new string[]
                   {
                       "eContractNumber","eCompany","eEmployeeID","eEmployeeIDNumber","ePreparedDate",
                       "eName",
                       "eEmployeeClass", // LEVEL
                       "eEmploymentType", // EMPLOYMENT STATUS
                       "ePosition","eDepartment","eRemarks", "eEffectiveDate","eNewSalary",
                       "eOldSalary", "ePreparedBy","eCheckBy","eApprovedBy", "eAddedBy",
                       "eModifiedBy", "eTotalCompensation","eOldTotalCompensation","eAllowance","eOldAllowance", "ePayrate"
                   },
                   new DbType[]
                   {
                       DbType.Int64,DbType.Int64,DbType.Int64,DbType.String,DbType.Date,
                       DbType.String,
                       DbType.Int64,
                       DbType.Int64,
                       DbType.Int64,DbType.Int64,DbType.String,DbType.Date,DbType.Double,
                       DbType.Double,DbType.Int64,DbType.Int64,DbType.Int64,DbType.String,
                       DbType.String,DbType.Double,DbType.Double,DbType.Double,DbType.Double, DbType.Int64
                   },
                   new object[]
                   {
                       data.ContractNumber,data.Company.ID,data.Employee.ID,data.Employee.Number,data.PreparedDate,
                       data.ContractName,
                       data.EmployeeClass.ID, //LEVEL
                       data.EmploymentType.ID, // EMPLOYMENT STATUS
                       data.Position.ID,data.Department.ID,data.Remarks,data.EffectiveDate,data.NewSalary,
                       data.OldSalary,data.Preparedby.ID,data.CheckBy.ID,data.ApprovedBy.ID,data.Addedby,
                       data.ModifedBy,data.TotalCompensation,data.OldTotalCompensation,data.Allowance,data.OldAllowance, data.PayRate.ID
                   },
                   out a,
                   CommandType.StoredProcedure
                   );
                data.Logs.After = "ID:" + data.ID.ToString() + ", ContractNumber:" + data.ContractNumber.ToString() + ",  EmployeeID:" + data.Employee.ID.ToString() + ", " +
                                             "EmployeeNumber:" + data.Employee.Number + ",  PreparedDate:" + data.PreparedDate.ToString("MM-dd-yyyy") + ",  " +
                                             "ContractName:" + data.ContractName + ", LEVEL:" + data.EmployeeClass.ID.ToString() + ",EmployeeStatus:" + data.EmploymentType.ID.ToString() + "," +
                                              ", Position:" + data.Position.ID.ToString() + "," +
                                            ", Department:" + data.Department.ID.ToString() + "," +
                                             "Remarks:" + data.Remarks + ", EffectiveDate:" + data.EffectiveDate.ToString("MM-dd-yyyy") + ", " +
                                              ", NewSalary:" + data.NewSalary.ToString() + ", OldSalary:" + data.OldSalary.ToString() + ", OldAllowance:" + data.OldAllowance.ToString() + "," +
                                             "NewAllowance:" + data.Allowance + ", PreparedBy:" + data.Preparedby.ID + ", CheckBy:" + data.CheckBy.ID.ToString() + ", ApprovedBy:" + data.ApprovedBy.ID.ToString() + "," +
                                             "PayrollAuthorization:" + data.PayrollAuthorization + ", AddedBy:" + data.Addedby + ", ModifiedBy:" + data.ModifedBy + "";

                data.Logs.Before = "";
                data.Logs.Type = 1;
                data.Logs.Module = 7;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        //Use this function for manual uploading only
        //this block is used for uploader
        public static void SaveContractForUploader(EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();

                //select the latest insert of this given id
                db.ExecuteCommandReader("HRIS_GetMAxID_ContractDetails",
                    new string[] { "EmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID }, out b, ref bTable, CommandType.StoredProcedure);
                if (bTable.Rows.Count > 0)
                {
                    long _empID;
                    long _Id;
                    // if true...
                    // set the status = existing
                    DataRow aRow = bTable.Rows[0];

                    if (aRow["EmployeeID"] != null && aRow["ID"] != null)
                    {

                        _empID = Convert.ToInt64(aRow["EmployeeID"]);
                        _Id = Convert.ToInt64(aRow["ID"]);
                        int x = 0;
                        if (data.EffectiveDate > Convert.ToDateTime(aRow["EffectiveDate"]))
                        {
                            EmployeeContract.SetToExistingContract(_empID, _Id, data);
                            data.Status = 4;
                        }
                        else
                        {
                            data.Status = 5;
                        }

                        db.ExecuteCommandNonQuery("HRIS_Save_Contract_Uploader",
                         new string[] {    "eContractNumber","eEmployeeID", "eEmployeeIDNumber", "eContractStatus", "ePRDate","ePRNumber","ePreparedDate", "eName", "eAssignment","eEmployeeClass","eEmploymentType", "eSO", "eJL", "eProject",
                                            "ePosition", "eDivision","eDepartment","eImmediateHead", "eRemarks", "eFrom", "eTo","eEffectiveDate", "eNewSalary", "eOldSalary", "eEcola", "eOldEcola", "ePreparedBy",
                                            "eCheckBy", "eApprovedBy", "ePayrollAuthorization", "eAddedBy", "eModifiedBy","eStatus","eApprovedDate","eCheckDate"},
                         new DbType[] {    DbType.Int64,DbType.Int64, DbType.String,  DbType.Int64, DbType.Date,DbType.String, DbType.Date, DbType.String,DbType.Int64,DbType.Int64, DbType.Int64,DbType.Int64,DbType.Int64,DbType.Int64,
                                            DbType.Int64, DbType.Int64,DbType.Int64, DbType.Int64,DbType.String, DbType.Date, DbType.Date, DbType.Date, DbType.Double, DbType.Double,DbType.Double, DbType.Double, DbType.Int64,
                                             DbType.Int64, DbType.Int64,DbType.String,DbType.String,DbType.String,DbType.Int32,DbType.Date,DbType.Date,},
                         new object[] {    data.ContractNumber, data.Employee.ID, data.Employee.Number, data.ContractStatus.ID,data.PRDate,data.PRNumber, data.PreparedDate,data.ContractName,data.Assignment.ID, data.EmployeeClass.ID,data.EmploymentType.ID, data.SO.ID,data.JL.ID, data.Project.ID,
                                            data.Position.ID,data.Division.ID, data.Department.ID,data.ImmediateHead, data.Remarks,data.From,data.To,data.EffectiveDate, data.NewSalary, data.OldSalary, data.Ecola, data.OldEcola, data.Preparedby.ID,
                                            data.CheckBy.ID,data.ApprovedBy.ID,data.PayrollAuthorization,data.Addedby,data.ModifedBy,data.Status,data.ApprovedDate, data.CheckDate}, out a, CommandType.StoredProcedure);
                    }
                    else
                    {
                        data.Status = 4;
                        db.ExecuteCommandNonQuery("HRIS_Save_Contract_Uploader",
                         new string[] {    "eContractNumber","eEmployeeID", "eEmployeeIDNumber", "eContractStatus", "ePRDate","ePRNumber","ePreparedDate", "eName", "eAssignment","eEmployeeClass","eEmploymentType", "eSO", "eJL", "eProject",
                                            "ePosition", "eDivision","eDepartment","eImmediateHead", "eRemarks", "eFrom", "eTo","eEffectiveDate", "eNewSalary", "eOldSalary", "eEcola", "eOldEcola", "ePreparedBy",
                                            "eCheckBy", "eApprovedBy", "ePayrollAuthorization", "eAddedBy", "eModifiedBy","eStatus","eApprovedDate","eCheckDate"},
                         new DbType[] {    DbType.Int64,DbType.Int64, DbType.String,  DbType.Int64, DbType.Date,DbType.String, DbType.Date, DbType.String,DbType.Int64,DbType.Int64, DbType.Int64,DbType.Int64,DbType.Int64,DbType.Int64,
                                            DbType.Int64, DbType.Int64,DbType.Int64, DbType.Int64,DbType.String, DbType.Date, DbType.Date, DbType.Date,  DbType.Double, DbType.Double,DbType.Double, DbType.Double, DbType.Int64,
                                             DbType.Int64, DbType.Int64,DbType.String,DbType.String,DbType.String,DbType.Int32,DbType.Date,DbType.Date,},
                         new object[] {    data.ContractNumber, data.Employee.ID, data.Employee.Number, data.ContractStatus.ID,data.PRDate,data.PRNumber, data.PreparedDate,data.ContractName,data.Assignment.ID, data.EmployeeClass.ID,data.EmploymentType.ID, data.SO.ID,data.JL.ID, data.Project.ID,
                                            data.Position.ID,data.Division.ID, data.Department.ID,data.ImmediateHead, data.Remarks,data.From,data.To,data.EffectiveDate, data.NewSalary, data.OldSalary, data.Ecola, data.OldEcola, data.Preparedby.ID,
                                            data.CheckBy.ID,data.ApprovedBy.ID,data.PayrollAuthorization,data.Addedby,data.ModifedBy,data.Status,data.ApprovedDate, data.CheckDate}, out a, CommandType.StoredProcedure);
                    }

                }
                else
                {
                    data.Status = 4;
                    db.ExecuteCommandNonQuery("HRIS_Save_Contract_Uploader",
                            new string[] {      "eContractNumber","eEmployeeID", "eEmployeeIDNumber", "eContractStatus", "ePRDate","ePRNumber","ePreparedDate", "eName", "eAssignment","eEmployeeClass","eEmploymentType", "eSO", "eJL", "eProject",
                                                "ePosition", "eDivision","eDepartment","eImmediateHead", "eRemarks", "eFrom", "eTo","eEffectiveDate", "eNewSalary", "eOldSalary", "eEcola", "eOldEcola", "ePreparedBy",
                                                "eCheckBy", "eApprovedBy", "ePayrollAuthorization", "eAddedBy", "eModifiedBy","eStatus","eApprovedDate","eCheckDate"},
                            new DbType[] {      DbType.Int64,DbType.Int64, DbType.String,  DbType.Int64, DbType.Date,DbType.String, DbType.Date, DbType.String,DbType.Int64,DbType.Int64, DbType.Int64,DbType.Int64,DbType.Int64,DbType.Int64,
                                                DbType.Int64, DbType.Int64,DbType.Int64, DbType.Int64,DbType.String, DbType.Date, DbType.Date, DbType.Date, DbType.Double, DbType.Double,DbType.Double, DbType.Double, DbType.Int64,
                                                DbType.Int64, DbType.Int64,DbType.String,DbType.String,DbType.String,DbType.Int32,DbType.Date,DbType.Date,},
                            new object[] {      data.ContractNumber, data.Employee.ID, data.Employee.Number, data.ContractStatus.ID,data.PRDate,data.PRNumber, data.PreparedDate,data.ContractName,data.Assignment.ID, data.EmployeeClass.ID,data.EmploymentType.ID, data.SO.ID,data.JL.ID, data.Project.ID,
                                                data.Position.ID,data.Division.ID, data.Department.ID,data.ImmediateHead, data.Remarks,data.From,data.To,data.EffectiveDate, data.NewSalary, data.OldSalary, data.Ecola, data.OldEcola, data.Preparedby.ID,
                                                data.CheckBy.ID,data.ApprovedBy.ID,data.PayrollAuthorization,data.Addedby,data.ModifedBy,data.Status,data.ApprovedDate, data.CheckDate}, out a, CommandType.StoredProcedure);
                }

            }
        }
        public static void UpdateContractsPaySchedule(EmployeeContract details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Update_PaySchedule_Contract",
                    new string[] { "eEmployeeID", "ePaySchedule" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { details.Employee.ID, details.PayRate.ID }, out x, CommandType.StoredProcedure);
            }
        }
        public static string CheckIfEmployeeHasActiveContract(Int64 eEmployeeID)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_CheckIfContractActive_Contract",
                   new string[] { "eEmployeeID" },
                   new DbType[] { DbType.Int64 },
                   new object[] { eEmployeeID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    res = "Employee has active contract.";
                }
            }
            return res;
        }

        public static string UpdateContract(EmployeeContract data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {

                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable bTable = new DataTable();
                db.ExecuteCommandNonQuery
                            (
                                "HRIS_SetToDraft_Contract",
                                    new string[]
                                    {
                                        "eEmpID",
                                        "eID",
                                        "eModifiedBy"
                                    },
                                    new DbType[]
                                    {
                                        DbType.Int64,
                                        DbType.Int64,
                                        DbType.String
                                    },
                                    new object[]
                                    {
                                        data.Employee.ID,
                                        data.ID,
                                        data.ModifedBy
                                    },
                                    out c,
                                    CommandType.StoredProcedure
                            );

                db.ExecuteCommandNonQuery(
                    "HRIS_Edit_Contract",
                    new string[]
                    {
                        "eID","eEmployeeID", "eCompany", "eEmployeeIDNumber", "ePreparedDate", "eName", "eEmployeeClass",
                        "eEmploymentType", "ePosition","eDepartment","eRemarks", "eEffectiveDate", "eNewSalary",
                        "eOldSalary",  "eModifiedBy","eTotalCompensation", "eoldTotalCompensation", "eAllowance",
                        "eOldAllowance", "ePayrate"
                    },
                    new DbType[]
                    {
                        DbType.Int64,DbType.Int64, DbType.Int64,DbType.String,  DbType.DateTime, DbType.String,DbType.Int64,
                        DbType.Int64, DbType.Int64,DbType.Int64,DbType.String, DbType.DateTime,DbType.Double,
                       DbType.Double,DbType.String,DbType.Double,  DbType.Double,DbType.Double, DbType.Double, DbType.Int64
                    },
                    new object[]
                    {
                        data.ID, data.Employee.ID, data.Company.ID, data.Employee.Number, data.PreparedDate,data.ContractName, data.EmployeeClass.ID,
                        data.EmploymentType.ID,  data.Position.ID,data.Department.ID, data.Remarks, data.EffectiveDate, data.NewSalary,
                        data.OldSalary, data.ModifedBy, data.TotalCompensation, data.OldTotalCompensation, data.Allowance, data.OldAllowance, data.PayRate.ID
                    },
                    out a,
                    CommandType.StoredProcedure
                    );

                res = "ok";
                data.Logs.After = "ID:" + data.ID.ToString() + ",  EmployeeID:" + data.Employee.ID.ToString() + ", " + ", Company:" + data.Company.ID.ToString() +
                                     "EmployeeNumber:" + data.Employee.Number + " ,  PreparedDate:" + data.PreparedDate.ToString("MM-dd-yyyy") + ",  " +
                                     "ContractName:" + data.ContractName + ", Level:" + data.EmployeeClass.ID.ToString() + ", EmploymentStatus:" + data.EmploymentType.ID.ToString() + "," +
                                     ", Position:" + data.Position.ID.ToString() + "," + ", Department:" + data.Department.ID.ToString() +
                                     "Remarks:" + data.Remarks + ", EffectiveDate:" + data.EffectiveDate.ToString("MM-dd-yyyy") + ", " +
                                     ", NewSalary:" + data.NewSalary.ToString() + ", OldSalary:" + data.OldSalary.ToString() + ", ModifiedBy:" + data.ModifedBy.ToString() + "," +
                                    ", PreparedBy:" + data.Preparedby.ID + ", CheckBy:" + data.CheckBy.ID.ToString() +
                                     ", ApprovedBy:" + data.ApprovedBy.ID.ToString() + ", TotalCompensation:" + data.TotalCompensation.ToString() +
                                     ", OldTotalCompensation:" + data.OldTotalCompensation.ToString() + ", Allowance:" + data.Allowance.ToString() +
                                     ", OldAllowance:" + data.OldAllowance.ToString();

                SystemLogs.SaveSystemLogs(data.Logs);
                //log for setting to draft onced update type is changed.
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 7;
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                ss.Before = "ID: " + data.ID + ", EmployeeID: " + data.Employee.ID + ", Status :" + _setStat + "";
                ss.After = "ID: " + data.ID + ", EmployeeID: " + data.Employee.ID + ", Status :Draft";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
            }
            return res;
        }
        public static void SetToForCheckingContract(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToForChecking_Contract",
                    new string[] { "eEmpid", "eID", "eModifiedBy", "ePreparedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64 },
                    new object[] { EmpID, eID, data.ModifedBy, data.Preparedby.ID }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :For Checking";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void SetToForApprovalContract(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToForApproval_Contract",
                    new string[] { "eEmpid", "eID", "eModifiedBy", "eCheckby" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64 },
                    new object[] { EmpID, eID, data.ModifedBy, data.CheckBy.ID }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :For Approval";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void ApprovedContract(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Approved_Contract",
                    new string[] { "eEmpid", "eID", "eModifiedBy", "eApprovedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64 },
                    new object[] { EmpID, eID, data.ModifedBy, data.ApprovedBy.ID }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :Approved";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void SetToDraftContract(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToDraft_Contract",
                    new string[] { "eEmpid", "eID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { EmpID, eID, data.ModifedBy }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :Draft";

                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void SetToExistingContract(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToExisting_Contract",
                    new string[] { "empid", "eID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { EmpID, eID, data.ModifedBy }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :Existing";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void CancellContractFromChecking(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_CancelFromChecking_Contract",
                    new string[] { "eempid", "eID", "eCheckBy", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { EmpID, eID, data.CheckBy.ID, data.ModifedBy }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :Cancelled";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void CancellContractFromApproval(Int64 EmpID, Int64 eID, EmployeeContract data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_CancelFromApproval_Contract",
                    new string[] { "eempid", "eID", "eApprovedBy", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { EmpID, eID, data.ApprovedBy.ID, data.ModifedBy }, out a, CommandType.StoredProcedure);
                string _setStat = "";
                switch (data.Status)
                {
                    case 1:
                        _setStat = "Draft";
                        break;
                    case 2:
                        _setStat = "For Checking";
                        break;
                    case 3:
                        _setStat = "For Approval";
                        break;
                    case 4:
                        _setStat = "Approved";
                        break;
                    case 5:
                        _setStat = "Existing";
                        break;
                    case 6:
                        _setStat = "Cancelled";
                        break;
                }
                data.Logs.Before = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :" + _setStat + "";
                data.Logs.After = "ID: " + eID + ", EmployeeID: " + EmpID + ", Status :Cancelled";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static EmployeeContract GetOldCompensationDetails(Int64 EmpID)
        {
            var c = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetOldSalary_Contract",
                    new string[] { "EmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count != 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.SO = new SalesOrder();
                    c.Project = new ProjectCode();
                    c.Company = new CompanyInfo();
                    c.OldSalary = Convert.ToDouble(aRow["OldSalary"]);
                    c.EffectiveDate = Convert.ToDateTime(aRow["EffectiveDate"]);
                    c.Remarks = aRow["Remarks"].ToString();
                    c.OldTotalCompensation = Convert.ToDouble(aRow["OldCompensation"]);
                    c.OldAllowance = Convert.ToDouble(aRow["OldAllowance"]);
                    c.Company.Name = aRow["CompanyName"].ToString();
                }
            }
            return c;
        }
        public static EmployeeContract GetOldEcola(Int64 EmpID)
        {
            var c = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetOldEcola_Contract",
                    new string[] { "EmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count != 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.OldEcola = Convert.ToDouble(aRow["OldEcola"]);
                }
            }
            return c;
        }
        public static List<EmployeeContract> GetExistingContracts(Int64 Empid)
        {
            var list = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetExisting_Contract",
                    new string[] { "EmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Empid }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.ContractStatus = new ContractStatus();
                    c.Department = new Department();
                    c.Division = new Division();
                    c.CheckBy = new Employee();
                    c.ApprovedBy = new Employee();
                    c.ImmediateHead = new Employee();
                    c.Preparedby = new Employee();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.Company = new CompanyInfo();
                    c.CheckBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CheckBy"]));
                    c.ApprovedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ApprovedBy"]));
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.ContractNumber = Convert.ToInt64(aRow["ContractNumber"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.PreparedDate = Convert.ToDateTime(aRow["PreparedDate"]);
                    c.ContractName = aRow["Name"].ToString();
                    c.Position.Code = aRow["Position"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position.Position = aRow["PositionName"].ToString();
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    c.Company = new CompanyInfo();
                    c.Company.Name = aRow["CompanyName"].ToString();
                    c.EffectiveDate = Convert.ToDateTime(aRow["EffectiveDate"]);
                    c.EmployeeClass.Description = aRow["Level"].ToString();
                    c.EmploymentType.Description = aRow["EmpStatus"].ToString();
                    c.Company.Name = aRow["CompanyName"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public static EmployeeContract GetNewGeneratedPRNumber()
        {
            var g = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetGeneratedPRNumber_Contract",
                   new string[] { },
                   new DbType[] { },
                   new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    g.PRNumber = aRow["GeneratedPRNumber"].ToString();
                }
            }
            return g;
        }
        public static EmployeeContract GetGeneratedContractNumber()
        {
            var g = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetGeneratedNumber_Contract",
                   new string[] { },
                   new DbType[] { },
                   new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    g.ID = Convert.ToInt32(aRow["ID"]);
                }
                else
                {
                    g.ID = 0;
                }
            }
            return g;
        }
        //for employeeDetails 
        public static EmployeeContract GetContract(Int64 Id)
        {
            var c = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetCurrent_Contract",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.ContractStatus = new ContractStatus();
                    c.Division = new Division();
                    c.Department = new Department();
                    c.Preparedby = new Employee();
                    c.Assignment = new Assignments();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.ImmediateHead = new Employee();
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    //c.ContractNumber = aRow["ID"].ToString();
                    if (aRow["ContractStatus"] != DBNull.Value)
                    {
                        c.ContractStatus.ID = Convert.ToInt64(aRow["ContractStatus"]);
                    }
                    else
                    {
                        c.ContractStatus.ID = 0;
                    }
                    c.PreparedDate = Convert.ToDateTime(aRow["PreparedDate"]);
                    c.ContractName = aRow["Name"].ToString();
                    c.SO.ID = Convert.ToInt64(aRow["SO"]);
                    c.JL.ID = Convert.ToInt64(aRow["JL"]);
                    c.Project.ID = Convert.ToInt64(aRow["Project"]);
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    c.ImmediateHead = Employee.GetEmployeeDetailsByID(Convert.ToInt64(aRow["ImmediateHead"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    c.Remarks = aRow["Remarks"].ToString();
                    if (aRow["From"] != DBNull.Value && aRow["To"] != DBNull.Value)
                    {
                        c.From = Convert.ToDateTime(aRow["From"]);
                        c.To = Convert.ToDateTime(aRow["To"]);
                    }
                    c.NewSalary = Convert.ToDouble(aRow["NewSalary"]);
                    c.OldSalary = Convert.ToDouble(aRow["OldSalary"]);
                    if (aRow["Ecola"] != DBNull.Value)
                    {
                        c.Ecola = Convert.ToDouble(aRow["Ecola"]);
                    }
                    c.Preparedby = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["PreparedBy"]));
                    if (aRow["Assignment"] != DBNull.Value)
                    {
                        c.Assignment = Assignments.GetAssignmentsByID(Convert.ToInt64(aRow["Assignment"]));
                    }

                    c.CheckBy = new Employee();
                    c.ApprovedBy = new Employee();
                    c.CheckBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CheckBy"]));
                    c.ApprovedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ApprovedBy"]));
                    c.PayrollAuthorization = aRow["PayrollAuthorization"].ToString();
                    c.Status = Convert.ToInt32(aRow["Status"]);
                }
                else
                {
                    //DataRow aRow = aTable.Rows[0];
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.ContractStatus = new ContractStatus();
                    c.Division = new Division();
                    c.Department = new Department();
                    c.ContractStatus = new ContractStatus();
                    c.Preparedby = new Employee();
                    c.CheckBy = new Employee();
                    c.ApprovedBy = new Employee();
                    c.ImmediateHead = new Employee();
                    c.ID = 0;
                    c.Employee.ID = 0;
                    //c.ContractNumber = "";
                    c.ContractStatus.ID = 0;
                    c.PreparedDate = Convert.ToDateTime("01/01/0001");
                    c.ContractName = "";
                    c.SO.ID = 0;
                    c.JL.ID = 0;
                    c.Project.ID = 0;
                    c.Position.ID = 0;
                    c.Department.ID = 0;
                    c.Division.ID = 0;
                    //c.DivisionDepartment ="";
                    c.Remarks = "";
                    c.From = Convert.ToDateTime("01/01/0001");
                    c.To = Convert.ToDateTime("01/01/0001");
                    c.NewSalary = 0;
                    c.OldSalary = 0;
                    c.Ecola = 0;
                    c.Preparedby.ID = 0;
                    c.CheckBy.ID = 0;
                    c.ApprovedBy.ID = 0;
                    c.PayrollAuthorization = "";
                }
                return c;
            }
        }
        public static EmployeeContract GetContractForUpdate(Int64 Id)
        {
            var c = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Contract",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.ContractStatus = new ContractStatus();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.Division = new Division();
                    c.Department = new Department();
                    c.Assignment = new Assignments();
                    c.ImmediateHead = new Employee();
                    c.PayRate = new Ref_PaySchedule();
                    c.Employee = new Employee();
                    c.Company = new CompanyInfo();
                    c.PayRate.ID = Convert.ToInt64(aRow["PaySchedule"]);
                    c.PayRate.Description = aRow["PayScheduleDesc"].ToString();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.EmployeeClass.ID = Convert.ToInt64(aRow["EmployeeClass"]);
                    c.EmploymentType.ID = Convert.ToInt64(aRow["EmploymentType"]);
                    c.PreparedDate = Convert.ToDateTime(aRow["PreparedDate"]);
                    c.ContractNumber = Convert.ToInt64(aRow["ContractNumber"]);
                    c.ContractName = aRow["Name"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    c.Remarks = aRow["Remarks"].ToString();
                    c.NewSalary = Convert.ToDouble(aRow["NewSalary"]);
                    c.OldSalary = Convert.ToDouble(aRow["OldSalary"]);
                    c.Preparedby = new Employee();
                    c.Preparedby = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["PreparedBy"]));
                    c.EffectiveDate = Convert.ToDateTime(aRow["EffectiveDate"]);
                    c.CheckBy = new Employee();
                    c.ApprovedBy = new Employee();
                    c.CheckBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CheckBy"]));
                    c.ApprovedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ApprovedBy"]));
                    c.PayrollAuthorization = aRow["PayrollAuthorization"].ToString();
                    c.UpdateType.ID = Convert.ToInt32(aRow["UpdateType"]);
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    c.OldAllowance = Convert.ToDouble(aRow["OldAllowance"]);
                    c.Allowance = Convert.ToDouble(aRow["Allowance"]);
                    c.TotalCompensation = Convert.ToDouble(aRow["TotalCompensation"]);
                    c.OldTotalCompensation = Convert.ToDouble(aRow["OldTotalCompensation"]);
                    c.ExistingContract = EmployeeContract.GetExistingContracts(c.Employee.ID);
                    c.Company.Name = aRow["CompanyName"].ToString();
                    c.Company.ID = Convert.ToInt64(aRow["CompanyID"]);
                    c.PayRate = new Ref_PaySchedule();
                    c.PayRate = Ref_PaySchedule.GetTaxTableCodesByID(Convert.ToInt64(aRow["PayRate"]));
                }
                return c;
            }
        }
        public static EmployeeContract GetContractForPrinting(Int64 Id)
        {
            var c = new EmployeeContract();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForPrintingByID_Contract",
                    new string[] { "pID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.ContractStatus = new ContractStatus();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.Assignment = new Assignments();
                    c.Division = new Division();
                    c.Department = new Department();
                    c.ImmediateHead = new Employee();
                    c.PayRate = new Ref_PaySchedule();
                    c.Company = new CompanyInfo();
                    c.PayRate = Ref_PaySchedule.GetTaxTableCodesByID(Convert.ToInt64(aRow["PaySchedule"]));
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Personal = new EmployeePersonal();
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Personal.Province = new Province();
                    c.Personal.City = new City();
                    c.Personal.Barangay = new BarangayAutocompleteJSON();
                    c.Company.Name = aRow["CompanyName"].ToString();
                    c.Personal.StreetName = aRow["StreetName"].ToString();
                    c.Personal.BuildingNumber = aRow["BuildingNumber"].ToString();
                    c.Personal.Province.Name = aRow["ProvinceDesc"].ToString();
                    c.Personal.City.Name = aRow["CityDesc"].ToString();
                    c.Personal.Barangay.Text = aRow["BarangayDesc"].ToString();
                    c.ContractNumber = Convert.ToInt64(aRow["ContractNumber"]);
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.EmployeeClass.ID = Convert.ToInt64(aRow["EmployeeClass"]);
                    c.EmployeeClass.Description = aRow["EmployeeClassDesc"].ToString();
                    c.EmploymentType.ID = Convert.ToInt64(aRow["EmploymentType"]);
                    c.EmploymentType.Description = aRow["EmpTypeName"].ToString();
                    c.ContractStatus.ID = Convert.ToInt64(aRow["ContractStatus"]);
                    c.PreparedDate = Convert.ToDateTime(aRow["PreparedDate"]);
                    c.ContractName = aRow["Name"].ToString();
                    c.PRNumber = aRow["PRNumber"].ToString();
                    c.PRDate = Convert.ToDateTime(aRow["PRDate"]);
                    c.Assignment.ID = Convert.ToInt64(aRow["Assignment"]);
                    c.SO.Trade = aRow["SOName"].ToString();
                    c.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    c.SO = SalesOrder.GetSalesOrderByID(Convert.ToInt64(aRow["SO"])); ;
                    c.JL = JobLevel.GetByID(Convert.ToInt64(aRow["JL"]));
                    c.Project.ID = Convert.ToInt64(aRow["Project"]);
                    c.Project.ProjectName = aRow["ProjectName"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position.Position = aRow["PositionName"].ToString();
                    c.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    c.ImmediateHead = Employee.GetEmployeeDetailsByID(Convert.ToInt64(aRow["ImmediateHead"]));
                    c.Remarks = aRow["Remarks"].ToString();
                    c.From = Convert.ToDateTime(aRow["From"]);
                    c.To = Convert.ToDateTime(aRow["To"]);
                    //c.Allowance = Convert.ToDouble(aRow["Allowance"]);
                    c.OldSalary = Convert.ToDouble(aRow["OldSalary"]);
                    c.NewSalary = Convert.ToDouble(aRow["NewSalary"]);
                    c.Preparedby = new Employee();
                    c.Preparedby = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["PreparedBy"]));
                    c.EffectiveDate = Convert.ToDateTime(aRow["EffectiveDate"]);
                    c.ExistingContract = EmployeeContract.GetExistingContracts(c.Employee.ID);
                    c.OldCompany = new CompanyInfo();
                    c.OldDepartment = new Department();
                    c.OldLevel = new EmployeeClassCode();
                    c.OldEmpStatus = new EmployeeTypeCode();
                    c.OldPosition = new PositionCode();
                    c.OldCompany.Name = aRow["OldCompany"].ToString();
                    c.OldDepartment.Description = aRow["OldDepartment"].ToString();
                    c.OldLevel.Description = aRow["OldLevel"].ToString();
                    c.OldEmpStatus.Description = aRow["OldEmpStatus"].ToString();
                    c.OldPosition.Position = aRow["OldPosition"].ToString();
                    if (aRow["OldTotalCompensation"] != DBNull.Value)
                    {
                        c.OldTotalCompensation = Convert.ToDouble(aRow["OldTotalCompensation"]);
                    }
                    else
                    {
                        c.OldTotalCompensation = 0;
                    }
                    c.YearsOfService = Employee.YearsOfService(c.Employee.ID);
                }
                return c;
            }
        }
        public static List<EmployeeContract> GetForCheckingContracts()
        {
            var ForCheckingList = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_ForChecking_Contract",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.Company = new CompanyInfo();
                    c.JL = new JobLevel();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.ApprovedBy = new Employee();
                    c.Preparedby = new Employee();
                    c.CheckBy = new Employee();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.EmployeeClass.Description = aRow["LevelCode"].ToString();
                    c.EmploymentType.Description = aRow["EmpStatus"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position = PositionCode.GetByID(c.Position.ID);
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.Suffix = aRow["Suffix"].ToString();
                    c.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    if (aRow["PrepID"] != DBNull.Value)
                    {
                        c.Preparedby.ID = Convert.ToInt64(aRow["PrepID"]);
                    }
                    c.Preparedby.Firstname = aRow["prepFname"].ToString();
                    c.Preparedby.Middlename = aRow["prepMname"].ToString();
                    c.Preparedby.Lastname = aRow["prepLname"].ToString();
                    c.Preparedby.Suffix = aRow["prepSuffix"].ToString();
                    if (aRow["CheckID"] != DBNull.Value)
                    {
                        c.CheckBy.ID = Convert.ToInt64(aRow["CheckID"]);
                    }

                    c.CheckBy.Firstname = aRow["CheckFname"].ToString();
                    c.CheckBy.Middlename = aRow["CheckMname"].ToString();
                    c.CheckBy.Lastname = aRow["CheckLname"].ToString();
                    c.CheckBy.Suffix = aRow["CheckSuffix"].ToString();
                    if (aRow["AppID"] != DBNull.Value)
                    {
                        c.ApprovedBy.ID = Convert.ToInt64(aRow["AppID"]);
                    }

                    c.ApprovedBy.Firstname = aRow["AppFname"].ToString();
                    c.ApprovedBy.Middlename = aRow["AppMname"].ToString();
                    c.ApprovedBy.Lastname = aRow["AppLname"].ToString();
                    c.ApprovedBy.Suffix = aRow["AppSuffix"].ToString();
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    c.Company.Name = aRow["CompanyName"].ToString();
                    ForCheckingList.Add(c);
                }
            }
            return ForCheckingList;
        }
        public static List<EmployeeContract> GetForApprovalContracts()
        {
            var ForApprovalList = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_ForApproval_Contract",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.Company = new CompanyInfo();
                    c.JL = new JobLevel();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.ApprovedBy = new Employee();
                    c.Preparedby = new Employee();
                    c.CheckBy = new Employee();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.EmployeeClass.Description = aRow["LevelCode"].ToString();
                    c.EmploymentType.Description = aRow["EmpStatus"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position = PositionCode.GetByID(c.Position.ID);
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.Suffix = aRow["Suffix"].ToString();
                    c.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    if (aRow["PrepID"] != DBNull.Value)
                    {
                        c.Preparedby.ID = Convert.ToInt64(aRow["PrepID"]);
                    }
                    c.Preparedby.Firstname = aRow["prepFname"].ToString();
                    c.Preparedby.Middlename = aRow["prepMname"].ToString();
                    c.Preparedby.Lastname = aRow["prepLname"].ToString();
                    c.Preparedby.Suffix = aRow["prepSuffix"].ToString();
                    if (aRow["CheckID"] != DBNull.Value)
                    {
                        c.CheckBy.ID = Convert.ToInt64(aRow["CheckID"]);
                    }

                    c.CheckBy.Firstname = aRow["CheckFname"].ToString();
                    c.CheckBy.Middlename = aRow["CheckMname"].ToString();
                    c.CheckBy.Lastname = aRow["CheckLname"].ToString();
                    c.CheckBy.Suffix = aRow["CheckSuffix"].ToString();
                    if (aRow["AppID"] != DBNull.Value)
                    {
                        c.ApprovedBy.ID = Convert.ToInt64(aRow["AppID"]);
                    }
                    c.ApprovedBy.Firstname = aRow["AppFname"].ToString();
                    c.ApprovedBy.Middlename = aRow["AppMname"].ToString();
                    c.ApprovedBy.Lastname = aRow["AppLname"].ToString();
                    c.ApprovedBy.Suffix = aRow["AppSuffix"].ToString();
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    c.Company.Name = aRow["CompanyName"].ToString();
                    ForApprovalList.Add(c);
                }
            }
            return ForApprovalList;
        }
        public static List<EmployeeContract> GetApprovedDraftContracts()
        {
            var AprrovedList = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedAndDraft_Contract",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.Company = new CompanyInfo();
                    c.JL = new JobLevel();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.ApprovedBy = new Employee();
                    c.Preparedby = new Employee();
                    c.CheckBy = new Employee();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.EmployeeClass.Description = aRow["LevelCode"].ToString();
                    c.EmploymentType.Description = aRow["EmpStatus"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position = PositionCode.GetByID(c.Position.ID);
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.Suffix = aRow["Suffix"].ToString();
                    c.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    if (aRow["PrepID"] != DBNull.Value)
                    {
                        c.Preparedby.ID = Convert.ToInt64(aRow["PrepID"]);
                    }
                    c.Preparedby.Firstname = aRow["prepFname"].ToString();
                    c.Preparedby.Middlename = aRow["prepMname"].ToString();
                    c.Preparedby.Lastname = aRow["prepLname"].ToString();
                    c.Preparedby.Suffix = aRow["prepSuffix"].ToString();
                    if (aRow["CheckID"] != DBNull.Value)
                    {
                        c.CheckBy.ID = Convert.ToInt64(aRow["CheckID"]);
                    }
                    c.CheckBy.Firstname = aRow["CheckFname"].ToString();
                    c.CheckBy.Middlename = aRow["CheckMname"].ToString();
                    c.CheckBy.Lastname = aRow["CheckLname"].ToString();
                    c.CheckBy.Suffix = aRow["CheckSuffix"].ToString();
                    if (aRow["AppID"] != DBNull.Value)
                    {
                        c.ApprovedBy.ID = Convert.ToInt64(aRow["AppID"]);
                    }
                    c.ApprovedBy.Firstname = aRow["AppFname"].ToString();
                    c.ApprovedBy.Middlename = aRow["AppMname"].ToString();
                    c.ApprovedBy.Lastname = aRow["AppLname"].ToString();
                    c.ApprovedBy.Suffix = aRow["AppSuffix"].ToString();
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    c.Company.Name = aRow["CompanyName"].ToString();
                    AprrovedList.Add(c);
                }
            }
            return AprrovedList;
        }
        public static List<EmployeeContract> GetAllContractsByEmployeeID(Int64 Id)
        {
            var AprrovedList = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAllByID_Contract",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.Company = new CompanyInfo();
                    c.JL = new JobLevel();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.ApprovedBy = new Employee();
                    c.Preparedby = new Employee();
                    c.CheckBy = new Employee();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.EmployeeClass.Description = aRow["LevelCode"].ToString();
                    c.EmploymentType.Description = aRow["EmpStatus"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position = PositionCode.GetByID(c.Position.ID);
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.Suffix = aRow["Suffix"].ToString();
                    c.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    c.Preparedby.ID = Convert.ToInt64(aRow["PrepID"]);
                    c.Preparedby.Firstname = aRow["prepFname"].ToString();
                    c.Preparedby.Middlename = aRow["prepMname"].ToString();
                    c.Preparedby.Lastname = aRow["prepLname"].ToString();
                    c.Preparedby.Suffix = aRow["prepSuffix"].ToString();
                    c.CheckBy.ID = Convert.ToInt64(aRow["CheckID"]);
                    c.CheckBy.Firstname = aRow["CheckFname"].ToString();
                    c.CheckBy.Middlename = aRow["CheckMname"].ToString();
                    c.CheckBy.Lastname = aRow["CheckLname"].ToString();
                    c.CheckBy.Suffix = aRow["CheckSuffix"].ToString();
                    c.ApprovedBy.ID = Convert.ToInt64(aRow["AppID"]);
                    c.ApprovedBy.Firstname = aRow["AppFname"].ToString();
                    c.ApprovedBy.Middlename = aRow["AppMname"].ToString();
                    c.ApprovedBy.Lastname = aRow["AppLname"].ToString();
                    c.ApprovedBy.Suffix = aRow["AppSuffix"].ToString();
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    c.Company.Name = aRow["CompanyName"].ToString();
                    AprrovedList.Add(c);
                }
            }
            return AprrovedList;
        }
        public static List<EmployeeContract> SearchByNameResult(Int64 Id, DateTime eFrom, DateTime eTo, Int64 CId, Int64 BId)//contract list/landing page
        {
            var searchResult = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_APISearchByName_Contract",
                    new string[] { "eID", "eFrom", "eTo", "eCompany", "eBranch" },
                    new DbType[] { DbType.Int64, DbType.Date, DbType.Date, DbType.Int64, DbType.Int64, },
                    new object[] { Id, eFrom, eTo, CId, BId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.Company = new CompanyInfo();
                    c.JL = new JobLevel();
                    c.UpdateType = new RefUpdateType();
                    c.EmploymentType = new EmployeeTypeCode();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.ApprovedBy = new Employee();
                    c.Preparedby = new Employee();
                    c.CheckBy = new Employee();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.EmployeeClass.Description = aRow["LevelCode"].ToString();
                    c.EmploymentType.Description = aRow["EmpStatus"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.Position = PositionCode.GetByID(c.Position.ID);
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.Suffix = aRow["Suffix"].ToString();
                    c.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    if (aRow["PrepID"] != DBNull.Value)
                    {
                        c.Preparedby.ID = Convert.ToInt64(aRow["PrepID"]);
                    }
                    c.Preparedby.Firstname = aRow["prepFname"].ToString();
                    c.Preparedby.Middlename = aRow["prepMname"].ToString();
                    c.Preparedby.Lastname = aRow["prepLname"].ToString();
                    c.Preparedby.Suffix = aRow["prepSuffix"].ToString();
                    if (aRow["CheckID"] != DBNull.Value)
                    {
                        c.CheckBy.ID = Convert.ToInt64(aRow["CheckID"]);
                    }
                    c.CheckBy.Firstname = aRow["CheckFname"].ToString();
                    c.CheckBy.Middlename = aRow["CheckMname"].ToString();
                    c.CheckBy.Lastname = aRow["CheckLname"].ToString();
                    c.CheckBy.Suffix = aRow["CheckSuffix"].ToString();
                    if (aRow["AppID"] != DBNull.Value)
                    {
                        c.ApprovedBy.ID = Convert.ToInt64(aRow["AppID"]);
                    }
                    c.ApprovedBy.Firstname = aRow["AppFname"].ToString();
                    c.ApprovedBy.Middlename = aRow["AppMname"].ToString();
                    c.ApprovedBy.Lastname = aRow["AppLname"].ToString();
                    c.ApprovedBy.Suffix = aRow["AppSuffix"].ToString();
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    c.Company.Name = aRow["CompanyName"].ToString();
                    searchResult.Add(c);
                }
            }
            return searchResult;
        }
        public static List<EmployeeContract> SearchByDateResult(DateTime StartDate, DateTime EndDate)//contract list/landing page
        {
            var searchResult = new List<EmployeeContract>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_APISearchByDate_Contract",
                    new string[] { "StartDate", "EndDate" },
                    new DbType[] { DbType.DateTime, DbType.DateTime },
                    new object[] { StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeContract c = new EmployeeContract();
                    c.Employee = new Employee();
                    c.Project = new ProjectCode();
                    c.Position = new PositionCode();
                    c.SO = new SalesOrder();
                    c.JL = new JobLevel();
                    c.UpdateType = new RefUpdateType();
                    c.EmployeeClass = new EmployeeClassCode();
                    c.ID = Convert.ToInt64(aRow["ID"]);
                    c.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    c.Employee.EmployeeID = aRow["EmployeeIDNumber"].ToString();
                    c.Employee.Firstname = aRow["FirstName"].ToString();
                    c.Employee.Lastname = aRow["LastName"].ToString();
                    c.EmployeeClass.Description = aRow["ClassCode"].ToString();
                    c.Position.ID = Convert.ToInt64(aRow["Position"]);
                    c.JL.ID = Convert.ToInt64(aRow["JL"]);
                    c.JL = JobLevel.GetByID(c.JL.ID);
                    c.Position = PositionCode.GetByID(c.Position.ID);
                    c.Employee.Middlename = aRow["MiddleName"].ToString();
                    c.Employee.Suffix = aRow["Suffix"].ToString();
                    c.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    c.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    c.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    c.UpdateType.Type = aRow["UpdateTypeName"].ToString();
                    c.Preparedby = new Employee();
                    c.Preparedby = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["PreparedBy"]));
                    c.CheckBy = new Employee();
                    c.ApprovedBy = new Employee();
                    c.CheckBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["CheckBy"]));
                    c.ApprovedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ApprovedBy"]));
                    c.Status = Convert.ToInt32(aRow["Status"]);
                    searchResult.Add(c);
                }
            }
            return searchResult;
        }
        public static List<EmployeeAutocompleteJSON> ContractAutoCompleteQuery(string query)
        {
            var res = new List<EmployeeAutocompleteJSON>();
            using (AppDb db = new AppDb())
            {
                string q = query;
                if (query == null || query.Length < 1)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();

                db.ExecuteCommandReader("HRIS_API_Contract",
                    new string[] { "q" },
                    new DbType[] { DbType.String },
                    new object[] { q }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    EmployeeAutocompleteJSON pc = new EmployeeAutocompleteJSON();
                    pc.Id = Convert.ToInt64(oRow["ID"]);
                    pc.Text = oRow["Name"].ToString();
                    res.Add(pc);
                }
            }
            return res;
        }
    }


    public class RefUpdateType
    {
        public int ID { get; set; }
        public string Type { get; set; }

        public static List<RefUpdateType> GetUpdateType()
        {
            var UpdateTypes = new List<RefUpdateType>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_UpdateType",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    RefUpdateType u = new RefUpdateType();
                    u.ID = Convert.ToInt32(aRow["ID"]);
                    u.Type = aRow["Type"].ToString();
                    UpdateTypes.Add(u);
                }
            }
            return UpdateTypes;
        }
    }
    public class Attachments
    {
        public long ID { get; set; }
        public string FileName { get; set; }
        public int Type { get; set; }
        public List<string> Files { get; set; }
        public Employee Employee { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static long SaveAttachments(Attachments data)
        {
            long eID = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Save_Attachment",
                    new string[] { "eType", "eEmployeeID", "eFileName", "eAddedBy", "eModifiedBy" },
                    new DbType[] { DbType.Int32, DbType.Int64, DbType.String, DbType.String, DbType.String },
                    new object[] { data.Type, data.Employee.ID, data.FileName, data.AddedBy, data.ModifiedBy }, out x, ref aTable, CommandType.StoredProcedure
                    );
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    eID = Convert.ToInt64(aRow["ID"]);
                }
            }
            return eID;
        }
        public static void DeleteAttachment(Attachments data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Attachment",
                   new string[] { "eEmployeeID", "eID" },
                   new DbType[] { DbType.Int64, DbType.Int64 },
                   new object[] { data.Employee.ID, data.ID }, out x, CommandType.StoredProcedure
                   );
            }
        }
        public static List<Attachments> GetAttachmentsByEmployeeID(Int64 EmployeeID)
        {
            var AttachmentList = new List<Attachments>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByEmployeeIDContract_Attachments",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmployeeID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Attachments b = new Attachments();
                    b.Employee = new Employee();
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    b.FileName = aRow["FileName"].ToString();
                    b.Type = Convert.ToInt32(aRow["Type"]);
                    AttachmentList.Add(b);
                }

            }
            return AttachmentList;
        }
        public static Attachments GetAttachmentsByID(Int64 Id)
        {
            var a = new Attachments();
            using (AppDb db = new AppDb())
            {
                int b = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByIDContract_Attachment",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out b, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    a.Employee = new Employee();
                    a.ID = Convert.ToInt64(aRow["ID"]);
                    a.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    a.FileName = aRow["FileName"].ToString();
                    a.Type = Convert.ToInt32(aRow["Type"]);
                }
            }
            return a;
        }

    }
}
