using OnePhp.HRIS.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace OnePhp.HRIS.Core.Model
{
    public class Employee
    {
        //This block holds COST_CENTER
        public long ID { get; set; }
        public string EmployeeID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Suffix { get; set; }
        public Department Department { get; set; }
        public Division Division { get; set; }
        public PositionCode Position { get; set; }
        public string Number { get; set; }
        public DateTime DateHired { get; set; }
        public DateTime DateResigned { get; set; }
        public RecordStatus ActiveStatus { get; set; }
        public DateTime ProbationDate { get; set; }
        public DateTime DateRegularized { get; set; }
        public DateTime DateOfSeparation { get; set; }
        public string ReasonForSeparation { get; set; }
        public string Addedby { get; set; }
        public string Modifiedby { get; set; }
        public EmployeeClassCode Class { get; set; }
        public EmployeeTypeCode Type { get; set; }
        public EmployeePersonal Personal { get; set; }
        public BarangayAutocompleteJSON PBarangay { get; set; }
        public City PCity { get; set; }
        public Province PProvince { get; set; }
        public Province Province { get; set; }
        public City City { get; set; }
        public BarangayAutocompleteJSON Barangay { get; set; }
        public List<EmployeeDependents> Dependents { get; set; }
        public EmployeeEmergencyDetails EDetails { get; set; }
        public List<EmployeeEducation> Education { get; set; }
        public List<EmployeeLicense> License { get; set; }
        public List<EmployeeCertifications> Certifications { get; set; }
        public List<SkillsLanguages> Skills { get; set; }
        public List<SkillsLanguages> Languages { get; set; }
        public EmployeeContract Contract { get; set; }
        public List<EmployeeContract> ExistingContract { get; set; }
        public EmployeeContract OldSalary { get; set; }
        public EmployeeContract OldAllowance { get; set; }
        public EmployeeContract GeneratedPRNumber { get; set; }
        public GovernmentID GovernmentID { get; set; }
        public BankAccounts BankAccount { get; set; }
        public string BiometricCode { get; set; }
        public JobLevel JL { get; set; }
        public int Status { get; set; }
        public Employee ProcessedBy { get; set; }
        public int UpdateType { get; set; }
        public string Remarks { get; set; }
        public DateTime LogDate { get; set; }
        public List<Employee> UpdateLogs { get; set; }
        public bool isSuperior { get; set; }
        public bool isFirstLevelSuperior { get; set; }
        public SystemLogs logs { get; set; }
        public bool isNew { get; set; }
        public CompanyInfo Company { get; set; }
        public Branches Branch { get; set; }
        public List<EmployeeRequirements> Requirements { get; set; }
        public ProjectWorkSchedule BranchSchedule { get; set; }

        public static string Save(Employee data)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                DataTable bTable = new DataTable();
                db.ExecuteCommandReader
                    (
                        "HRIS_isEmpIDExist_Employee",
                        new string[]
                        {
                            "eEmployeeID"
                        },
                        new DbType[]
                        {
                            DbType.String
                        },
                        new object[]
                        {
                            data.EmployeeID
                        },
                        out b,
                        ref bTable,
                        CommandType.StoredProcedure
                    );
                if (bTable.Rows.Count > 0)
                {

                    res = "Employee ID already exist.";
                }
                else
                {
                    //note: If saving from the BETA_HRIS_Fileuploader, you have to comment down /**/ "eProcessedBY",
                    //dbtype.Int64, data.processedBy.ID, to prevent null exception error, etc.
                    db.ExecuteCommandNonQuery
                        (
                            "HRIS_Save_Employee",
                                new string[]
                                {
                                    "eID", "eLastName", "eFirstName", "eMiddleName", "eSuffix", "ePosition",
                                    "eDateHired", "eActiveStatus", "eProbationDate", "eDateRegularized",
                                    "eAddedBy", "eModifiedBy" ,"eProcessedBy"
                                },
                                new DbType[]
                                {
                                    DbType.String, DbType.String, DbType.String,DbType.String, DbType.String,DbType.Int64,
                                    DbType.Date,DbType.Int64, DbType.Date,DbType.Date,
                                    DbType.String,DbType.String , DbType.Int64
                                },
                                new object[]
                                {
                                    data.EmployeeID,  data.Lastname, data.Firstname, data.Middlename,data.Suffix,data.Position.ID,
                                    data.DateHired,data.ActiveStatus.ID,data.ProbationDate,data.DateRegularized,
                                    data.Addedby,data.Modifiedby ,data.ProcessedBy.ID
                                },
                                out a,
                                CommandType.StoredProcedure
                                );
                    res = "Employee save successfully.";
                    //Log the changes made.
                    data.logs.UserID = data.ProcessedBy.ID;
                    data.logs.Module = 1;
                    data.logs.Type = 1;
                    data.logs.Before = "";
                    data.logs.After =
                        "EmployeeID:" + data.EmployeeID + ", LastName:" + data.Lastname + ", FirstName:" + data.Firstname + ", MiddleName:" + data.Middlename + ", Suffix:" + data.Suffix + ", Position:" + data.Position.ID.ToString() +
                        ", Datehired:" + data.DateHired.ToString("MM-dd-yyyyy") + ", ActiveStatus:" + data.ActiveStatus.ID.ToString() + ", DateRegularized:" + data.DateRegularized.ToString("MM-dd-yyyyy") +
                        ", AddedBy:" + data.Addedby.ToString() + ", ModifiedBy:" + data.Modifiedby.ToString() + ", ProcessedBy:" + data.ProcessedBy.ToString() + "";
                    SystemLogs.SaveSystemLogs
                        (
                            data.logs
                        );
                }
            }
            return res;
        }
        public static string YearsOfService(Int64 EmpId)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("hris_yearsofservice_employee",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    res = aRow["years_and_months_since_hire"].ToString();
                }
                else
                {
                    res = "0 years, 0 Months";
                }
            }
            return res;
        }
        public static Employee GetEmployeeByPosID(Int64 PosID)
        {
            var e = new Employee();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("hris_getByPosition_Employee",
                    new string[] { "ePosition" },
                    new DbType[] { DbType.Int64 },
                    new object[] { PosID }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    e.Position = new PositionCode();
                    e.Firstname = oRow["FirstName"].ToString();
                    e.Middlename = oRow["MiddleName"].ToString();
                    e.Lastname = oRow["LastName"].ToString();
                    e.Suffix = oRow["Suffix"].ToString();
                    e.Position.Position = oRow["PositionName"].ToString();
                }
            }
            return e;
        }
        public static void SaveChangeLog(Employee details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SaveChangeLog_Employee",
                   new string[] { "eUpdateType", "eEmployeeID", "eRemarks", "eProcessedBy" },
                   new DbType[] { DbType.Int32, DbType.Int64, DbType.String, DbType.Int64 },
                   new object[] { details.UpdateType, details.ID, details.Remarks, details.ProcessedBy.ID }, out x, CommandType.StoredProcedure);

            }
        }
        public static void SetToSentStatus(Employee details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToSent_Employee",
                   new string[] { "eID", "eEmployeeID", "eUpdateType", "eRemarks", "eProcessedBy" },
                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.Int64 },
                   new object[] { details.ID, details.EmployeeID, details.UpdateType, details.Remarks, details.ProcessedBy.ID }, out x, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(details.logs);
            }
        }
        public static void SetToDraftStatus(Employee details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToDraft_Employee",
                   new string[] { "eID", "eEmployeeID", "eUpdateType", "eRemarks", "eProcessedBy" },
                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.Int64 },
                   new object[] { details.ID, details.EmployeeID, details.UpdateType, details.Remarks, details.ProcessedBy.ID }, out x, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(details.logs);
            }
        }
        public static void SetToApprovedStatus(Employee details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToApproved_Employee",
                   new string[] { "eID", "eEmployeeID", "eUpdateType", "eRemarks", "eProcessedBy" },
                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.Int64 },
                   new object[] { details.ID, details.EmployeeID, details.UpdateType, details.Remarks, details.ProcessedBy.ID }, out x, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(details.logs);
            }
        }
        public static List<Employee> GetChangeLogs(String EmpId)
        {
            var _list = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_getChangeLogs_Employee",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    Employee l = new Employee();
                    l.UpdateType = Convert.ToInt32(aRow["UpdateType"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    l.LogDate = Convert.ToDateTime(aRow["Date"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static void Update(Employee data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                string _after = "";


                db.ExecuteCommandNonQuery("HRIS_Edit_Employee",
                    new string[] { "eID","eEmployeeID","eBioCode", "eLastName", "eFirstName", "eMiddleName", "eSuffix", "eDepartment",
                        "ePosition", "eEmpClass", "eEmpType", "eDateHired", "eDateResigned", "eActiveStatus", "eProbationDate", "eDateRegularized",
                        "eDateOfSeparation", "eReasonForSeparation",  "eModifiedBy" },
                    new DbType[] {  DbType.Int64, DbType.Int64,DbType.String, DbType.String, DbType.String, DbType.String, DbType.String,DbType.Int64,
                        DbType.Int64,DbType.Int64,DbType.Int64,DbType.Date,DbType.Date,DbType.Int64, DbType.Date,DbType.Date,
                        DbType.Date,DbType.String,DbType.String},
                    new object[] { data.ID,data.EmployeeID,data.BiometricCode, data.Lastname, data.Firstname, data.Middlename,data.Suffix,data.Department.ID,
                        data.Position.ID,data.Class.ID,data.Type.ID,data.DateHired,data.DateResigned,data.ActiveStatus.ID,data.ProbationDate,data.DateRegularized,
                        data.DateOfSeparation, data.ReasonForSeparation,data.Modifiedby}, out a, CommandType.StoredProcedure);
                //System Logs
                if (data.ReasonForSeparation == null)
                {
                    data.ReasonForSeparation = "";
                }
                _after += "ID:" + data.EmployeeID + ", EmployeeID:" + data.EmployeeID + ", BiometricID:" + data.BiometricCode +
                                          ",LastName:" + data.Lastname + ", FirstName:" + data.Firstname + ", MiddleName:" +
                                          data.Middlename + ", Suffix:" + data.Suffix + ", Department:" + data.Department.ID.ToString() +
                                         ", Position:" + data.Position.ID.ToString() + ", Class:" + data.Class.ID.ToString() + ", Type:" + data.Type.ID.ToString() +
                                         ", Datehired:" + data.DateHired.ToString("MM-dd-yyyyy") + ", ActiveStatus:" + data.ActiveStatus.ID.ToString() + ", ProbationDate:" +
                                          data.ProbationDate.ToString() + ", DateRegularized:" + data.DateRegularized.ToString("MM-dd-yyyyy") +
                                          ", DateOfSeparation:" + data.DateOfSeparation.ToString("MM-dd-yyyyy") + ", ReasonForSeparation:" + data.ReasonForSeparation + "";
                data.logs.After = _after;
                SystemLogs.SaveSystemLogs(data.logs);
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 2;
                string _setStat = "";
                if (data.Status == 1)
                {
                    _setStat = "Draft";
                }
                else if (data.Status == 2)
                {
                    _setStat = "Sent";
                }
                else if (data.Status == 3)
                {
                    _setStat = "Approved";
                }
                ss.Before = "EmployeeID: " + data.ID + ", Status :" + _setStat + "";
                ss.After = "EmployeeID: " + data.ID + ", Status :Draft";
                ss.UserID = data.logs.UserID;
                ss.UserName = data.logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
            }
        }
        public static void Delete(Employee data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Employee",
                    new string[] { "eID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.EmployeeID, data.Modifiedby }, out a, CommandType.StoredProcedure);
            }
        }
        public static Employee GetGeneratedEmployeeNumber()
        {
            var _eID = new Employee();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetGeneratedNumber_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _eID.ID = Convert.ToInt64(aRow["ID"]);
                }
            }
            return _eID;
        }
        public static Employee GetEmployeNameByID(Int64 eID)
        {
            var e = new Employee();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeeName_Employee",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { eID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                }
            }
            return e;
        }
        public static List<Employee> Get()
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    e.UpdateLogs = new List<Employee>();
                    e.Company = new CompanyInfo();
                    if (aRow["ActiveStatus"] != DBNull.Value)
                    {
                        e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    }
                    else
                    {
                        e.ActiveStatus.ID = 0;
                    }
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.BiometricCode = aRow["BioMetricCode"].ToString();
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    e.UpdateLogs = Employee.GetChangeLogs(e.EmployeeID);
                    e.ProcessedBy = new Employee();
                    e.Company.ID = Convert.ToInt64(aRow["Company"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    if (aRow["ProcessedBy"] != DBNull.Value)
                    {
                        e.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    }
                    else
                    {
                        e.ProcessedBy.ID = 0;
                    }
                    res.Add(e);
                }
            }
            return res;
        }
        public static List<Employee> GetForApproval()
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_ForApproval",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    e.UpdateLogs = new List<Employee>();
                    e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.BiometricCode = aRow["BioMetricCode"].ToString();
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    e.UpdateLogs = Employee.GetChangeLogs(e.EmployeeID);
                    e.ProcessedBy = new Employee();
                    if (aRow["ProcessedBy"] != DBNull.Value)
                    {
                        //e.ProcessedBy.ID = Convert.ToInt64(aRow["ProcessedBy"]);
                        e.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    }
                    else
                    {
                        e.ProcessedBy.ID = 0;
                    }
                    res.Add(e);
                }
            }
            return res;
        }
        public static List<Employee> GetFilteredForReports(long PosId, long ClassId, long TypeId, long DivId, long DepId)
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_Filtered_EmployeeMasterList",
                    new string[] { "ePosition", "eClass", "eType", "eDivision", "eDepartment" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { PosId, ClassId, TypeId, DivId, DepId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    if (aRow["ActiveStatus"] != DBNull.Value)
                    {
                        e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    }
                    else
                    {
                        e.ActiveStatus.ID = 0;
                    }
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.BiometricCode = aRow["BioMetricCode"].ToString();
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    res.Add(e);
                }
            }
            return res;
        }

        public static List<Employee> GetListBy(Int64 eID, Int64 CompanyId, Int64 BranchId)
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetListByID_Employee",
                    new string[] { "eID", "eCompanyId", "eBranchId" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { eID, CompanyId, BranchId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    e.Company = new CompanyInfo();
                    e.Requirements = new List<EmployeeRequirements>();
                    e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.DateResigned = Convert.ToDateTime(aRow["DateResigned"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    e.ProcessedBy = new Employee();
                    e.Company.ID = Convert.ToInt64(aRow["Company"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    if (aRow["ProcessedBy"] != DBNull.Value)
                    {
                        //e.ProcessedBy.ID = Convert.ToInt64(aRow["ProcessedBy"]);
                        e.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    }
                    else
                    {
                        e.ProcessedBy.ID = 0;
                    }
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    res.Add(e);
                }
            }
            return res;
        }
        public static List<Employee> GetListByCompanyForSchedule(Int64 CompanyId)
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
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

                db.ExecuteCommandReader("HRIS_GetListByCompany_Schedule",
                    new string[] { "eCompanyId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { CompanyId, startDateOfNextWeek, endDateOfNextWeek }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    e.Company = new CompanyInfo();
                    e.Requirements = new List<EmployeeRequirements>();
                    e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.DateResigned = Convert.ToDateTime(aRow["DateResigned"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    e.ProcessedBy = new Employee();
                    e.Company.ID = Convert.ToInt64(aRow["Company"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    if (aRow["ProcessedBy"] != DBNull.Value)
                    {
                        //e.ProcessedBy.ID = Convert.ToInt64(aRow["ProcessedBy"]);
                        e.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    }
                    else
                    {
                        e.ProcessedBy.ID = 0;
                    }
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    res.Add(e);
                }
            }
            return res;
        }
        public static List<Employee> GetListByCompany(Int64 CompanyId)
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetListByCompany_Employee",
                    new string[] { "eCompanyId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { CompanyId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    e.Company = new CompanyInfo();
                    e.Requirements = new List<EmployeeRequirements>();
                    e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.DateResigned = Convert.ToDateTime(aRow["DateResigned"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    e.ProcessedBy = new Employee();
                    e.Company.ID = Convert.ToInt64(aRow["Company"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    if (aRow["ProcessedBy"] != DBNull.Value)
                    {
                        //e.ProcessedBy.ID = Convert.ToInt64(aRow["ProcessedBy"]);
                        e.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    }
                    else
                    {
                        e.ProcessedBy.ID = 0;
                    }
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    res.Add(e);
                }
            }
            return res;
        }
        public static List<Employee> GetListForApprovalBy(Int64 eID)
        {
            var res = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetListByIDForApproval_Employee",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { eID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Employee e = new Employee();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.DateResigned = Convert.ToDateTime(aRow["DateResigned"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    e.ProcessedBy = new Employee();
                    if (aRow["ProcessedBy"] != DBNull.Value)
                    {
                        //e.ProcessedBy.ID = Convert.ToInt64(aRow["ProcessedBy"]);
                        e.ProcessedBy = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["ProcessedBy"]));
                    }
                    else
                    {
                        e.ProcessedBy.ID = 0;
                    }
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    res.Add(e);
                }
            }
            return res;
        }
        public static List<EmployeeAutocompleteJSON> EmployeeAutoComplete(string query)
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

                db.ExecuteCommandReader("HRIS_AutoComplete_Employee",
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
        public static List<EmployeeAutocompleteJSON> ApprovedEmployeeAutoComplete(string query)
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

                db.ExecuteCommandReader("HRIS_AutoComplete_approved_Employee",
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
        public static Employee GetEmployeeDetailsByID(Int64 Id)
        {
            var e = new Employee();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETByID_Employee",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.Personal = new EmployeePersonal();
                    e.Personal.Barangay = new BarangayAutocompleteJSON();
                    e.Personal.City = new City();
                    e.Personal.Province = new Province();
                    e.Personal.PBarangay = new BarangayAutocompleteJSON();
                    e.Personal.PCity = new City();
                    e.Personal.PProvince = new Province();
                    e.Personal.MaritalStatus = new RefMaritalStatus();
                    e.EDetails = new EmployeeEmergencyDetails();
                    e.Dependents = new List<EmployeeDependents>();
                    e.Personal.BloodType = new BloodType();
                    e.ActiveStatus = new RecordStatus();
                    e.Contract = new EmployeeContract();
                    e.Contract.Department = new Department();
                    e.Contract.Division = new Division();
                    e.GovernmentID = new GovernmentID();
                    e.BankAccount = new BankAccounts();
                    e.Department.Head = new Employee();
                    e.Division.Head = new Employee();
                    e.JL = new JobLevel();
                    e.Department.HeadPosition = new PositionCode();
                    e.Division.HeadPosition = new PositionCode();
                    e.Company = new CompanyInfo();
                    e.Requirements = new List<EmployeeRequirements>();
                    //e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));

                    if (aRow["ActiveStatus"] != DBNull.Value)
                    {
                        e.ActiveStatus.ID = Convert.ToInt64(aRow["ActiveStatus"]);
                    }
                    else
                    {
                        e.ActiveStatus.ID = 0;
                    }
                    e.ActiveStatus.Description = aRow["ActiveStatusDesc"].ToString();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.BiometricCode = aRow["BiometricCode"].ToString();
                    e.GovernmentID = GovernmentID.GetGovernmentIDByEmployeeID(e.ID);
                    e.BankAccount = BankAccounts.GeBankAccountByEmployeeID(e.ID);
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Contract.CheckBy = new Employee();
                    e.Contract.Assignment = new Assignments();
                    e.Contract.ApprovedBy = new Employee();
                    e.Contract.Preparedby = new Employee();
                    e.Contract.ImmediateHead = new Employee();
                    e.Department.ID = Convert.ToInt64(aRow["DepartmentID"]);
                    e.Division.ID = Convert.ToInt64(aRow["DivisionID"]);
                    e.Department = Department.GetByID(e.Department.ID);
                    e.Division = Division.GetByID(e.Division.ID);
                    e.JL.Description = aRow["JobLevel"].ToString();
                    e.Position.JobLevel = new JobLevel();
                    e.Position = PositionCode.GetByID(Convert.ToInt64(aRow["PositionID"]));
                    e.Position.ID = Convert.ToInt64(aRow["PositionID"]);
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.ID = Convert.ToInt64(aRow["ClassID"]);
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.ID = Convert.ToInt64(aRow["TypeID"]);
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.DateResigned = Convert.ToDateTime(aRow["DateResigned"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    e.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    e.Personal.ID = Convert.ToInt64(aRow["PersonalID"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.Email = aRow["Email"].ToString();
                    e.Personal.MobileNumber = aRow["MobileNumber"].ToString();
                    e.Personal.Relatives = aRow["Relatives"].ToString();
                    e.Personal.Relation = aRow["Relation"].ToString();
                    e.Personal.SpouseName = aRow["SpouseName"].ToString();
                    e.Personal.MaritalStatus.ID = Convert.ToInt32(aRow["MaritalStatus"]);
                    e.Personal.MotherFirstName = aRow["MotherFIrstName"].ToString();
                    e.Personal.MotherMiddleName = aRow["MotherMiddleName"].ToString();
                    e.Personal.MotherLastName = aRow["MotherLastName"].ToString();
                    e.Personal.MotherOccupation = aRow["MotherOccupation"].ToString();
                    e.Personal.FatherFirstName = aRow["FatherFIrstName"].ToString();
                    e.Personal.FatherMiddleName = aRow["FatherMiddleName"].ToString();
                    e.Personal.FatherLastName = aRow["FatherLastName"].ToString();
                    e.Personal.FatherOccupation = aRow["FatherOccupation"].ToString();
                    e.Personal.Religion = aRow["Religion"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.Personal.BirthPlace = aRow["BirthPlace"].ToString();
                    e.Personal.BloodType.ID = Convert.ToInt32(aRow["BloodType"]);
                    e.Personal.DateOfMedical = Convert.ToDateTime(aRow["DateOfMedical"]);
                    e.Personal.MedicalClassification = aRow["MedicalClassification"].ToString();
                    e.Personal.BuildingNumber = aRow["BuildingNumber"].ToString();
                    e.Personal.StreetName = aRow["StreetName"].ToString();
                    e.Personal.Barangay.Id = Convert.ToInt64(aRow["Barangay"]);
                    e.Personal.Barangay.Text = aRow["BName"].ToString();
                    e.Personal.City.ID = Convert.ToInt32(aRow["City"]);
                    e.Personal.PostalCode = aRow["PostalCode"].ToString();
                    e.Personal.Province.ID = Convert.ToInt32(aRow["Province"]);
                    e.Personal.YearsOfResidency = Convert.ToInt32(aRow["YearsOfResidency"]);
                    e.Personal.PBuildingNumber = aRow["PPBuildingNumber"].ToString();
                    e.Personal.PStreetName = aRow["PStreetName"].ToString();
                    e.Personal.PBarangay.Id = Convert.ToInt64(aRow["Barangay"]);
                    e.Personal.PBarangay.Text = aRow["PBName"].ToString();
                    e.Personal.PCity.ID = Convert.ToInt32(aRow["City"]); ;
                    e.Personal.PPostalCode = aRow["PPostalCode"].ToString();
                    e.Personal.PProvince.ID = Convert.ToInt32(aRow["PProvince"]);
                    e.Personal.PYearsOfResidency = Convert.ToInt32(aRow["PYearsOfResidency"]);
                    e.Dependents = EmployeeDependents.Get(e.ID);
                    e.EDetails.ID = Convert.ToInt64(aRow["EDetailsID"]);
                    e.EDetails.Name = aRow["EName"].ToString();
                    e.EDetails.Relationship = aRow["ERelationship"].ToString();
                    e.EDetails.ContactNumber = aRow["EContactNumber"].ToString();
                    e.Education = EmployeeEducation.GetByEmployeeID(e.ID);
                    e.License = EmployeeLicense.GetByEmployeeID(e.ID);
                    e.Certifications = EmployeeCertifications.GetCertifications(e.ID);
                    e.Languages = SkillsLanguages.GetLangugaes(e.ID);
                    e.Skills = SkillsLanguages.GetSkills(e.ID);
                    e.Contract = EmployeeContract.GetContract(e.ID);
                    e.ExistingContract = EmployeeContract.GetExistingContracts(e.ID);
                    e.OldSalary = EmployeeContract.GetOldCompensationDetails(e.ID);
                    //e.OldAllowance = EmployeeContract.GetOldAllowance(e.ID);
                    e.UpdateLogs = new List<Employee>();
                    e.UpdateLogs = Employee.GetChangeLogs(e.EmployeeID);
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    e.Company.ID = Convert.ToInt64(aRow["Company"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.Requirements = EmployeeRequirements.GetEmployeeRequirements(e.ID);
                    int s = 0;
                    DataTable sTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_isSuperior_ImmediateHeads",
                        new string[] { "eSuperiorId" },
                        new DbType[] { DbType.Int64 },
                        new object[] { e.ID }, out s, ref sTable, CommandType.StoredProcedure);
                    if (sTable.Rows.Count > 0)
                    {
                        e.isSuperior = true;
                    }
                    else
                    {
                        e.isSuperior = false;
                    }
                    int t = 0;
                    DataTable tTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_isSuperiorFirstLevel_Approval",
                        new string[] { "eEmpId" },
                        new DbType[] { DbType.Int64 },
                        new object[] { e.ID }, out t, ref tTable, CommandType.StoredProcedure);
                    if (tTable.Rows.Count > 0)
                    {
                        e.isFirstLevelSuperior = true;
                    }
                    else
                    {
                        e.isFirstLevelSuperior = false;
                    }
                    int _z = 0;
                    e.isNew = false;
                    DataTable _zTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_isNewUser_Employee",
                        new string[] { "eId" },
                        new DbType[] { DbType.Int64 },
                        new object[] { e.ID }, out _z, ref _zTable, CommandType.StoredProcedure);
                    if (_zTable.Rows.Count > 0)
                    {
                        DataRow _zRow = _zTable.Rows[0];
                        if (_zRow["MustChangePassword"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(_zRow["MustChangePassword"]) == 0)
                            {
                                e.isNew = true;
                            }
                        }
                    }
                    e.Branch = new Branches();
                    e.Branch = Branches.GetBranchesById(Convert.ToInt64(aRow["Branch"]));
                }
            }
            return e;
        }
        public static List<APIEmployeeJSON> APIEmployeeName(string query)
        {
            var res = new List<APIEmployeeJSON>();
            using (AppDb db = new AppDb())
            {
                string q = query;
                if (query == null || query.Length < 3)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();

                db.ExecuteCommandReader("HRIS_API_Employee",
                    new string[] { "q" },
                    new DbType[] { DbType.String },
                    new object[] { q }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    APIEmployeeJSON e = new APIEmployeeJSON();
                    e.Id = Convert.ToInt64(oRow["ID"]);
                    e.Text = oRow["Name"].ToString();

                    res.Add(e);
                }
            }
            return res;
        }
        public static List<Employee> GetAllEmployeesWithDotNetEmailAddress()
        {
            var _list = new List<Employee>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeeWithDotNetEmail_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    Employee e = new Employee();
                    e.Personal = new EmployeePersonal();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Personal.Email = aRow["ToEmail"].ToString();
                    _list.Add(e);
                }
            }
            return _list;
        }
    }
    public class SkillsLanguages
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string Skill { get; set; }
        public string Language { get; set; }
        public int Reading { get; set; }
        public int Speaking { get; set; }
        public int Understanding { get; set; }
        public int Writing { get; set; }
        public string Addedby { get; set; }
        public string Modifiedby { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveSkill(SkillsLanguages data, string[] Skill)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                //int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteData_Skill",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID }, out a, CommandType.StoredProcedure);
                int x = 0;
                string _after = "";
                foreach (string i in Skill)
                {
                    data.Skill = Skill[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_Skill",
                         new string[] { "eEmployeeID", "eSkills", "eAddedBy", "eModifiedBy" },
                         new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String },
                         new object[] { data.Employee.ID, data.Skill, data.Addedby, data.Modifiedby }, out a, CommandType.StoredProcedure);
                    _after += "[ ID:" + data.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + ", Skill:" + data.Skill.ToString() + ",],";
                    x += 1;
                }

                data.Logs.After = _after;
                data.Logs.Module = 52;
                SystemLogs.SaveSystemLogs(data.Logs);
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 5;
                string _setStat = "";
                if (data.Employee.Status == 1)
                {
                    _setStat = "Draft";
                }
                else if (data.Employee.Status == 2)
                {
                    _setStat = "Sent";
                }
                else if (data.Employee.Status == 3)
                {
                    _setStat = "Approved";
                }
                ss.Before = "EmployeeID: " + data.Employee.ID + ", Status :" + _setStat + "";
                ss.After = "EmployeeID: " + data.Employee.ID + ", Status :Draft";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
            }
        }
        public static void DeleteSkill(SkillsLanguages data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Skill",
                    new string[] { "eEmpID", "eID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.Employee.ID, data.ID, data.Modifiedby }, out a, CommandType.StoredProcedure);
                data.Logs.Type = 3;
                data.Logs.Module = 52;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<SkillsLanguages> GetSkills(Int64 Id)
        {
            var list = new List<SkillsLanguages>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Skills",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    SkillsLanguages s = new SkillsLanguages();
                    s.Employee = new Employee();
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    s.Skill = aRow["Skill"].ToString();
                    list.Add(s);
                }
            }
            return list;
        }
        public static SkillsLanguages GetSkillsByID(Int64 Id)
        {
            var s = new SkillsLanguages();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Skills",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    s.Employee = new Employee();
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    s.Skill = aRow["Skill"].ToString();
                }
            }
            return s;
        }
        public static void SaveLanguages(SkillsLanguages data, string[] Language, Int32[] Reading, Int32[] Understanding, Int32[] Speaking, Int32[] Writing)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteData_Languages",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID }, out a, CommandType.StoredProcedure);
                int x = 0;
                string _after = "";
                foreach (string i in Language)
                {
                    data.Language = Language[x];
                    data.Reading = Reading[x];
                    data.Understanding = Understanding[x];
                    data.Speaking = Speaking[x];
                    data.Writing = Writing[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_Languages",
                         new string[] { "eEmployeeID", "eLanguage", "eReading", "eUnderstanding", "eSpeaking", "eWriting", "eAddedBy", "eModifiedBy " },
                         new DbType[] { DbType.Int64, DbType.String, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.String, DbType.String },
                         new object[] { data.Employee.ID, data.Language, data.Reading, data.Understanding, data.Speaking, data.Writing, data.Addedby, data.Modifiedby }, out a, CommandType.StoredProcedure);
                    _after += "[ ID:" + data.ID.ToString() + ",  Language:" + data.Language.ToString() + ",  Reading:" + data.Reading.ToString() + ", " +
                                    "Understanding:" + data.Understanding.ToString() + ",  Speaking:" + data.Speaking.ToString() + ", Writing:" + data.Writing.ToString() + " ], ";
                    x += 1;
                }
                data.Logs.After = _after;
                data.Logs.Module = 52;
                SystemLogs.SaveSystemLogs(data.Logs);
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 5;
                string _setStat = "";
                if (data.Employee.Status == 1)
                {
                    _setStat = "Draft";
                }
                else if (data.Employee.Status == 2)
                {
                    _setStat = "Sent";
                }
                else if (data.Employee.Status == 3)
                {
                    _setStat = "Approved";
                }
                ss.Before = "EmployeeID: " + data.Employee.ID + ", Status :" + _setStat + "";
                ss.After = "EmployeeID: " + data.Employee.ID + ", Status :Draft";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);

            }
        }
        public static void DeleteLanguage(SkillsLanguages data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Languages",
                    new string[] { "eEmpID", "eID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.Employee.ID, data.ID, data.Modifiedby }, out a, CommandType.StoredProcedure);
                data.Logs.Type = 2;
                data.Logs.Module = 52;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<SkillsLanguages> GetLangugaes(Int64 Id)
        {
            var list = new List<SkillsLanguages>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Languages",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    SkillsLanguages s = new SkillsLanguages();
                    s.Employee = new Employee();
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    s.Language = aRow["Language"].ToString();
                    s.Reading = Convert.ToInt32(aRow["Reading"]);
                    s.Understanding = Convert.ToInt32(aRow["Understanding"]);
                    s.Speaking = Convert.ToInt32(aRow["Speaking"]);
                    s.Writing = Convert.ToInt32(aRow["Writing"]);
                    list.Add(s);
                }
            }
            return list;
        }
        public static SkillsLanguages GetLanguagesByID(Int64 Id)
        {
            var s = new SkillsLanguages();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetById_Languages",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    s.Employee = new Employee();
                    s.ID = Convert.ToInt64(aRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    s.Language = aRow["Language"].ToString();
                    s.Reading = Convert.ToInt32(aRow["Reading"]);
                    s.Understanding = Convert.ToInt32(aRow["Understanding"]);
                    s.Speaking = Convert.ToInt32(aRow["Speaking"]);
                    s.Writing = Convert.ToInt32(aRow["Writing"]);
                }
            }
            return s;
        }
    }
    public class Pagination
    {
        public string More { get; set; }
    }
    public class EmployeeAutocompleteJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }
    public class EmployeeAutocompleteObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<EmployeeAutocompleteJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class APIEmployeeJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }
    public class APIEmployeeObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<APIEmployeeJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class EmployeeDashboardCount
    {
        public int Regular { get; set; }
        public int Contractual { get; set; }
        public int Probationary { get; set; }
        public int Project { get; set; }
        public int Trainee { get; set; }
        public int AgeCount { get; set; }
        public int FemaleCount { get; set; }
        public int MaleCount { get; set; }
        public PositionCode Position { get; set; }


        public static EmployeeDashboardCount GetPositionCount(Int64 pID)
        {
            var c = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetCountPerPosition_Employee",
                  new string[] { "pID" },
                  new DbType[] { DbType.Int64 },
                  new object[] { pID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Position = new PositionCode();
                    c.Position.ID = Convert.ToInt64(aRow["ID"]);
                    c.Position.Position = aRow["Position"].ToString();
                    c.Contractual = Convert.ToInt32(aRow["Count"]);
                }
            }
            return c;
        }
        public static EmployeeDashboardCount GetGenderCount()
        {
            var c = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_GenderCount_Employee",
                  new string[] { },
                  new DbType[] { },
                  new object[] { }, out a, ref aTable, CommandType.StoredProcedure);

                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];

                    c.FemaleCount = Convert.ToInt32(aRow["FemaleCount"]);
                    c.MaleCount = Convert.ToInt32(aRow["MaleCount"]);
                }
            }
            return c;
        }
        public static EmployeeDashboardCount GetContractualEmployeesCount()
        {
            var c = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETContractual_Employees",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Contractual = Convert.ToInt32(aRow["Contractual"]);
                }
            }
            return c;
        }
        public static EmployeeDashboardCount GetRegularEmployeesCount()
        {
            var c = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETRegular_Employees",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Regular = Convert.ToInt32(aRow["Regular"]);
                }
            }
            return c;
        }
        public static EmployeeDashboardCount GetProbationaryEmployeesCount()
        {
            var c = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETProbationary_Employees",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Probationary = Convert.ToInt32(aRow["Probationary"]);
                }
            }
            return c;
        }
        public static EmployeeDashboardCount GetProjectEmployeesCount()
        {
            var c = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETProjectEmployee_Employees",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    c.Project = Convert.ToInt32(aRow["Project"]);
                }
            }
            return c;
        }
        public static EmployeeDashboardCount GetAge0To20()
        {
            var e = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAge_0To20_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.AgeCount = Convert.ToInt32(aRow["AgeCount"]);
                }
            }
            return e;
        }
        public static EmployeeDashboardCount GetAge21To30()
        {
            var e = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAge_21To30_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.AgeCount = Convert.ToInt32(aRow["AgeCount"]);
                }
            }
            return e;
        }
        public static EmployeeDashboardCount GetAge31To40()
        {
            var e = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAge_31To40_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.AgeCount = Convert.ToInt32(aRow["AgeCount"]);
                }
            }
            return e;
        }
        public static EmployeeDashboardCount GetAge41To50()
        {
            var e = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAge_41To50_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.AgeCount = Convert.ToInt32(aRow["AgeCount"]);
                }
            }
            return e;
        }
        public static EmployeeDashboardCount GetAge51To60()
        {
            var e = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAge_51To60_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.AgeCount = Convert.ToInt32(aRow["AgeCount"]);
                }
            }
            return e;
        }
        public static EmployeeDashboardCount GetAge61To100()
        {
            var e = new EmployeeDashboardCount();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAge_61To100_Employee",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.AgeCount = Convert.ToInt32(aRow["AgeCount"]);
                }
            }
            return e;
        }
    }
    public class Announcements
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static void SaveAnnouncement(Announcements data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_Announcements",
                    new string[] { "eDate", "eType", "eMessage", "eAddedBy", "eModifiedBy" },
                    new DbType[] { DbType.Date, DbType.String, DbType.String, DbType.String, DbType.String },
                    new object[] { data.Date, data.Type, data.Message, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);

            }
        }
        public static void EditAnnouncement(Announcements data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Announcement",
                    new string[] { "eID", "eDate", "eType", "eMessage", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Date, DbType.String, DbType.String, DbType.String },
                    new object[] { data.Id, data.Date, data.Type, data.Message, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteAnnouncement(Announcements data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Announcement",
                    new string[] { "eID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.Id, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<Announcements> GetAnnouncements()
        {
            var announcementList = new List<Announcements>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Announcements",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Announcements e = new Announcements();
                    e.Id = Convert.ToInt64(aRow["Id"]);
                    e.Date = Convert.ToDateTime(aRow["Date"]);
                    e.Type = aRow["Type"].ToString();
                    e.Message = aRow["Message"].ToString();
                    announcementList.Add(e);
                }
            }
            return announcementList;
        }
        public static Announcements GetAnnouncementsByID(Int64 Id)
        {
            var e = new Announcements();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Announcements",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);

                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Id = Convert.ToInt64(aRow["Id"]);
                    e.Date = Convert.ToDateTime(aRow["Date"]);
                    e.Type = aRow["Type"].ToString();
                    e.Message = aRow["Message"].ToString();

                }
            }
            return e;
        }
    }
    public class GovernmentID
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string TIN { get; set; }
        public string SSS { get; set; }
        public string PHILHEALTH { get; set; }
        public string PAGIBIG { get; set; }
        public string MP2 { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public TaxCode TaxData { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveGovernmentID(GovernmentID data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_GovernmentID",
                    new string[] { "gEmployeeID", "gTIN", "gSSS", "gPhilHealth", "gPAGIBIG", "gMP2", "gTaxData", "gAddedBy", "gmodifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.String, DbType.String, },
                    new object[] { data.Employee.ID, data.TIN, data.SSS, data.PHILHEALTH, data.PAGIBIG, data.MP2, data.TaxData.ID, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "EmployeeID: " + data.Employee.ID + ",  TIN: " + data.TIN + ", SSS: " + data.SSS + ",  PHILHEALTH: " + data.PHILHEALTH + ", PAGIBIG: " + data.PAGIBIG + ", MP2: " + data.MP2 + ", TaxData: " + data.TaxData.ID + "";

                SystemLogs.SaveSystemLogs(data.Logs);
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 3;
                string _setStat = "";
                if (data.Employee.Status == 1)
                {
                    _setStat = "Draft";
                }
                else if (data.Employee.Status == 2)
                {
                    _setStat = "Sent";
                }
                else if (data.Employee.Status == 3)
                {
                    _setStat = "Approved";
                }
                ss.Before = "EmployeeID: " + data.Employee.ID + ", Status :" + _setStat + "";
                ss.After = "EmployeeID: " + data.Employee.ID + ", Status :Draft";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
            }
        }
        public static GovernmentID GetGovernmentIDByEmployeeID(Int64 EmpId)
        {
            var g = new GovernmentID();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByEmployeeID_GovernmentID",
                    new string[] { "gEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    g.Employee = new Employee();
                    g.TaxData = new TaxCode();
                    g.ID = Convert.ToInt64(aRow["ID"]);
                    g.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    g.TIN = aRow["TIN"].ToString();
                    g.SSS = aRow["SSS"].ToString();
                    g.PHILHEALTH = aRow["PhilHealth"].ToString();
                    g.PAGIBIG = aRow["PAGIBIG"].ToString();
                    g.MP2 = aRow["MP2"].ToString();
                    if (aRow["TaxData"] != DBNull.Value)
                    {
                        g.TaxData = TaxCode.GetByID(Convert.ToInt64(aRow["TaxData"]));
                    }
                    else
                    {
                        g.TaxData.ID = 0;
                    }
                }
            }
            return g;
        }
    }
    public class BankAccounts
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public BankCode Bank { get; set; }
        public string AccountNo { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveBankAccounts(BankAccounts data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Save_BankAccount",
                    new string[] { "bEmployeeID", "bBank", "bAccountNo", "bAddedBy", "bmodifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String },
                    new object[] { data.Employee.ID, data.Bank.ID, data.AccountNo, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.After = "ID" + data.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + ",BankID:" + data.Bank.ID + ", BankAccountNo:" + data.AccountNo + "";
                SystemLogs.SaveSystemLogs(data.Logs);
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 4;
                string _setStat = "";
                if (data.Employee.Status == 1)
                {
                    _setStat = "Draft";
                }
                else if (data.Employee.Status == 2)
                {
                    _setStat = "Sent";
                }
                else if (data.Employee.Status == 3)
                {
                    _setStat = "Approved";
                }
                ss.Before = "EmployeeID: " + data.Employee.ID + ", Status :" + _setStat + "";
                ss.After = "EmployeeID: " + data.Employee.ID + ", Status :Draft";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);
            }
        }
        public static BankAccounts GeBankAccountByEmployeeID(Int64 EmpId)
        {
            var b = new BankAccounts();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByEmployeeID_BankAccount",
                    new string[] { "bEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    b.Employee = new Employee();
                    b.Bank = new BankCode();
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    b.AccountNo = aRow["AccountNo"].ToString();
                    //b.Bank.ID = Convert.ToInt64(aRow["Bank"]);
                    b.Bank = BankCode.GetByID(Convert.ToInt64(aRow["Bank"]));
                }
            }
            return b;
        }
    }

}
