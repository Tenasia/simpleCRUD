using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using OnePhp.HRIS.Core.Data;

namespace OnePhp.HRIS.Core.Model
{
    public class OfficialBusiness
    {
        public long ID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double TotalHours { get; set; }
        public string Remarks { get; set; }
        public DateTime Date { get; set; }
        public string Details { get; set; }
        public string Purpose { get; set; }
        public int Status { get; set; }
        public string AddedBy { get; set; }
        public string Destination { get; set; }
        public string ModifiedBy { get; set; }
        public Employee Employee { get; set; }
        public Employee Approver { get; set; }
        public Employee ApprovedBy { get; set; }
        public PositionCode Position { get; set; }
        public PositionCode ApproverPosition { get; set; }
        public EmployeeClassCode Class { get; set; }
        public SalesOrder SO { get; set; }
        public ProjectCode ProjectName { get; set; }
        public Assignments Assignments { get; set; }
        public string ApproverRemarks { get; set; }
        public DateTime DateApproved { get; set; }
        public EmployeeClassCode ApproverClass { get; set; }
        public List<ImmediateSuperior> Superior { get; set; }
        public PayPeriod PayPeriod { get; set; }
        public ImmediateSuperior MySuperiorLevel { get; set; }
        public static long SaveOfficialBusinessDetails(OfficialBusiness data)
        {
            long _postID = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_SAVE_OB_EMPLOYEEPORTAL",
                    new string[] { "oEmployeeID", "oStartTime", "oEndTime", "oTotalHours", "oDate", "oDetails", "oPurpose", "oDestination", "oAddedBy", "oModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.Double, DbType.DateTime, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                    new object[] { data.Employee.ID, data.StartTime, data.EndTime, data.TotalHours, data.Date, data.Details, data.Purpose, data.Destination, data.AddedBy, data.ModifiedBy }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _postID = Convert.ToInt64(aRow["ID"]);
                }

            }
            return _postID;
        }
        public static OfficialBusiness isApplicationValid(Int64 EmpId)
        {
            var l = new OfficialBusiness();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int py = 0;
                DataTable pyTable = new DataTable();
                db.ExecuteCommandReader("hris_get_payperiod_employeerequest",
                new string[] { "eEmpID" },
                new DbType[] { DbType.Int64 },
                new object[] { EmpId }, out py, ref pyTable, CommandType.StoredProcedure);
                if (pyTable.Rows.Count > 0)
                {
                    DataRow pyRow = pyTable.Rows[0];
                    l.PayPeriod = new PayPeriod();
                    l.PayPeriod.Start = Convert.ToDateTime(pyRow["Start"]);
                    l.PayPeriod.End = Convert.ToDateTime(pyRow["End"]);
                    l.PayPeriod.PayDate = Convert.ToDateTime(pyRow["PayDate"]);
                }
            }
            return l;
        }
        public static void EditOfficialBusinessDetails(OfficialBusiness data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_OB_EMPLOYEEPORTAL",
                   new string[] { "oID", "oEmployeeID", "oStartTime", "oEndTime", "oTotalHours", "oDate", "oDetails", "oPurpose", "oDestination", "oAddedBy", "oModifiedBy" },
                   new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.Double, DbType.DateTime, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                   new object[] { data.ID, data.Employee.ID, data.StartTime, data.EndTime, data.TotalHours, data.Date, data.Details, data.Purpose, data.Destination, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }

        public static void OBSent(OfficialBusiness details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandNonQuery("HRIS_Sent_OB",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { details.ID, details.Employee.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static void OBReject(OfficialBusiness details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Reject_OB",
                    new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }

        public static void OBApproved(OfficialBusiness details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                int _SupLevel = 0;

                db.ExecuteCommandReader("HRIS_GetBySuperiorID_ImmediateHeads",
                new string[] { "eSupId", "eEmpId" },
                new DbType[] { DbType.Int64, DbType.Int64 },
                new object[] { details.Approver.ID, details.Employee.ID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _SupLevel = Convert.ToInt32(aRow["Level"]);
                    if (_SupLevel == 1)
                    {
                        db.ExecuteCommandReader("hris_isEmployeehasLevel2Superior_ImmediateHeads",
                             new string[] { "eEmployeeID" },
                             new DbType[] { DbType.Int64 },
                             new object[] { details.Employee.ID }, out b, ref bTable, CommandType.StoredProcedure);
                        //check if requestor has level 2 superior
                        //if true change status to Checked
                        if (bTable.Rows.Count > 0)
                        {
                            db.ExecuteCommandNonQuery("HRIS_FirstLevel_Approved_OB",
                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                               new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                        else
                        {
                            //check if has level 3 superior
                            db.ExecuteCommandReader("hris_isEmployeehasLevel3Superior_ImmediateHeads",
                               new string[] { "eEmployeeID" },
                               new DbType[] { DbType.Int64 },
                               new object[] { details.Employee.ID }, out c, ref cTable, CommandType.StoredProcedure);
                            if (cTable.Rows.Count > 0)
                            {
                                db.ExecuteCommandNonQuery("HRIS_SecondLevelApproved_OfficialBusiness",
                                 new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                 new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                 new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                            }
                            else
                            {
                                db.ExecuteCommandNonQuery("HRIS_Approved_OB",
                                   new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                   new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                            }

                        }
                    }
                    else if (_SupLevel == 2)
                    {

                        //check if has level 3 superior
                        db.ExecuteCommandReader("hris_isEmployeehasLevel3Superior_ImmediateHeads",
                           new string[] { "eEmployeeID" },
                           new DbType[] { DbType.Int64 },
                           new object[] { details.Employee.ID }, out c, ref cTable, CommandType.StoredProcedure);
                        if (cTable.Rows.Count > 0)
                        {
                            db.ExecuteCommandNonQuery("HRIS_SecondLevelApproved_OfficialBusiness",
                             new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                             new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                             new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                        else
                        {
                            db.ExecuteCommandNonQuery("HRIS_Approved_OB",
                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                               new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                    }
                    else if (_SupLevel == 3)
                    {
                        db.ExecuteCommandNonQuery("HRIS_Approved_OB",
                                new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                    }
                }
            }
        }
        public static void HRApproved_OB(OfficialBusiness details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Approved_OB",
                            new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                            new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                            new object[] { details.ID, details.Employee.ID, details.Approver.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }
        public static void OBCancel(OfficialBusiness data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Cancel_OB",
                    new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks", },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new Object[] { data.ID, data.Employee.ID, data.ApprovedBy.ID, data.ApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }
        public static void OBDelete(OfficialBusiness data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_OB_EMPLOYEEPORTAL",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new Object[] { data.ID, data.Employee.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<OfficialBusiness> GetOfficialBusinessesDetails(Int64 employeeID)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_OB_EmployeePortal",
                    new string[] { "oEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { employeeID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOfficialBusinessesDetailsWithResuls(Int64 employeeID)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApplicationResults_OB",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { employeeID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static void SetStatusToHasApplicationResult(Int64 Id)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToSentApplicationResults_OB",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, CommandType.StoredProcedure);
            }
        }
        public static OfficialBusiness GetOfficialBusinessDetailByID(Int64 Id, Int64 EmployeeID)
        {
            var o = new OfficialBusiness();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_OBById_EmployeePortal",
                 new string[] { "oID", "oEmployeeID" },
                 new DbType[] { DbType.Int64, DbType.Int64 },
                 new object[] { Id, EmployeeID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments = new Assignments();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                }
            }
            return o;
        }
        public static List<OfficialBusiness> GetAllOBForApproval (Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetOB_ForApproval",
                    new string[] { "eSuperiorId" },
                    new DbType[] {  DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.ApproverClass = new EmployeeClassCode();
                    o.ApproverPosition = new PositionCode();
                    o.Assignments = new Assignments();
                    o.MySuperiorLevel = new ImmediateSuperior();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.MySuperiorLevel.Level = Convert.ToInt32(aRow["Level"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForFirstLevelApprovalDepartment(Int64 DepId, Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalDepartment_FirstLevel_OB",
                    new string[] { "eDepartmentID", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DepId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.ApproverClass = new EmployeeClassCode();
                    o.ApproverPosition = new PositionCode();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Assignments = new Assignments();
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForSecondLevelApprovalDepartment(Int64 DepId, Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalDepartment_SecondLevel_OB",
                    new string[] { "eDepartmentID", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DepId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.ApproverClass = new EmployeeClassCode();
                    o.ApproverPosition = new PositionCode();
                    o.Assignments = new Assignments();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForHRApproval()
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_ForApproval_HR_OB",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.Assignments = new Assignments();
                    o.SO = new SalesOrder();
                    o.Employee.Department = new Department();
                    o.Employee.Division = new Division();
                    o.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    o.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.ApproverClass = new EmployeeClassCode();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.Superior = new List<ImmediateSuperior>();
                    o.Superior = ImmediateSuperior.GetAssignedSuperiors(o.Employee.ID);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetFilteredOBDetailsForHRApproval(Int64 Div, Int64 Dep, Int64 Pos, Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_ForApprovalFiltered_HR_OB",
                    new string[] { "eDivision", "eDepartment", "ePosition", "eEmployeeId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { Div, Dep, Pos, EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.Assignments = new Assignments();
                    o.SO = new SalesOrder();
                    o.Employee.Department = new Department();
                    o.Employee.Division = new Division();
                    o.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    o.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.ApproverClass = new EmployeeClassCode();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.Superior = new List<ImmediateSuperior>();
                    o.Superior = ImmediateSuperior.GetAssignedSuperiors(o.Employee.ID);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForFourthLevelApproval(Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForPresidentApproval_FourthLevel_OB",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.Assignments = new Assignments();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.ApproverClass = new EmployeeClassCode();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForThirdLevelApprovalDepartment(Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalDepartment_ThirdLevel_OB",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.Assignments = new Assignments();
                    o.SO = new SalesOrder();
                    o.ApproverClass = new EmployeeClassCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForFirstLevelApprovalDivision(Int64 DivId, Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalDivision_FirstLevel_OB",
                    new string[] { "eDivisionID", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DivId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.Assignments = new Assignments();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    //o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetOBDetailsForSecondLevelApprovalDivision(Int64 DivId, Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalDivision_SecondLevel_OB",
                    new string[] { "eDivisionID", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DivId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.ApproverClass = new EmployeeClassCode();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.SO = new SalesOrder();
                    o.Assignments = new Assignments();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<OfficialBusiness> GetToBeSentOBDetails(Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetToBeSent_OfficialBusiness",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.ApproverClass = new EmployeeClassCode();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.SO = new SalesOrder();
                    o.Assignments = new Assignments();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static void SetToSentOfficialBusinessNotification(Int64 Id)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetStatusToEmailSent_OfficialBusiness",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, CommandType.StoredProcedure);
            }
        }
        public static List<OfficialBusiness> GetOBDetailsForThirdLevelApprovalDivision(Int64 SupId)
        {
            var OBList = new List<OfficialBusiness>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalDivision_ThirdLevel_OB",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OfficialBusiness o = new OfficialBusiness();
                    string _sTime = aRow["StartTime"].ToString();
                    DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                    string _stime12HrFormat = _stimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    string _eTime = aRow["EndTime"].ToString();
                    DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                    string _etime12HrFormat = _etimeFromDB.ToString(
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);
                    o.Employee = new Employee();
                    o.Approver = new Employee();
                    o.Position = new PositionCode();
                    o.ApproverClass = new EmployeeClassCode();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.SO = new SalesOrder();
                    o.Assignments = new Assignments();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.EmployeeID = aRow["EmpId"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.StartTime = _stime12HrFormat;
                    o.EndTime = _etime12HrFormat;
                    o.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    //o.Remarks = aRow["Remarks"].ToString();
                    o.Date = Convert.ToDateTime(aRow["Date"]);
                    o.Details = aRow["Details"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Destination = aRow["Destination"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.Position.Position = aRow["Position"].ToString();
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SOCode"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.ApprovedBy.Firstname = aRow["ApproverFirstName"].ToString();
                    o.ApprovedBy.Middlename = aRow["ApproverMiddleName"].ToString();
                    o.ApprovedBy.Lastname = aRow["ApproverLastName"].ToString();
                    o.ApprovedBy.Suffix = aRow["ApproverSuffix"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApproverClass.Description = aRow["class"].ToString();
                    o.ApproverPosition = new PositionCode();
                    o.ApproverPosition.Position = aRow["ApproverPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
    }
}
