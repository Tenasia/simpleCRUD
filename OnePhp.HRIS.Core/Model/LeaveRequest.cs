using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using OnePhp.HRIS.Core.Data;

namespace OnePhp.HRIS.Core.Model
{
    public class LeaveRequest
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public LeaveTypeCode Leave { get; set; }
        public string Remarks { get; set; }
        public DateTime LeaveDate { get; set; }
        public double NumberOfHours { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Employee Approver { get; set; }
        public int AM { get; set; }
        public int PM { get; set; }
        public int WholeDay { get; set; }
        public LeaveComputation LeaveComputation { get; set; }
        public int Status { get; set; }
        public PositionCode Position { get; set; }
        public AnnualLeaveCredits AnnualLeaveCredits { get; set; }
        public Employee FirstApprover { get; set; }
        public Employee SecondApprover { get; set; }
        public PositionCode FPosition { get; set; }
        public PositionCode SPosition { get; set; }
        public EmployeeClassCode FCLass { get; set; }
        public EmployeeClassCode SClass { get; set; }
        public string FApproverRemarks { get; set; }
        public string SApproverRemarks { get; set; }
        public int ApprovalStatus { get; set; }
        public int DayOfTheWeek { get; set; }
        public DateTime DateApproved { get; set; }
        public List<ImmediateSuperior> Superior { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public BreakSchedule Breaks { get; set; }
        public WorkSchedule Schedule { get; set; }
        public long RequestNumber { get; set; }
        public int TotalHours { get; set; }
        public double TotalDays { get; set; }
        public double TotalApprovedDays { get; set; }
        public int TotalApprovedHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PayPeriod PayPeriod { get; set; }
        public ImmediateSuperior MySuperiorLevel { get; set; }
        /*
         * NOTES:
         * LEAVE_APPROVAL STATUS
         * * * Status (0)=Rejected/Deleted, Status(1)=First Level Approval/Need Managers Approval, Status(2)=Approved
         * LEAVE COMPUTATION STATUS
         * * * Status (0)=Rejected/Deleted, Status(1)=NoT Approved Yet/On Process, Status(2)=Approved
         */
        public static LeaveRequest GetGeneratedRequestNumber()
        {
            var l = new LeaveRequest();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int o = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetGeneratedRequestNumber_LeaveRequest",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out o, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    l.RequestNumber = Convert.ToInt64(oRow["ID"]);
                }
            }
            return l;
        }
        public static LeaveRequest GetEmployeeDailyTimeOutForDisplay(Int64 EmpId, Int32 DOW, Int64 eAssignment)
        {
            var l = new LeaveRequest();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int o = 0;
                DataTable oTable = new DataTable();
                if (eAssignment == 2)
                {
                    db.ExecuteCommandReader("HRIS_GetActualHoursPerDay_MainOffice_Employee",
                        new string[] { "eEmployeeId", "eDay" },
                        new DbType[] { DbType.Int64, DbType.Int32 },
                        new object[] { EmpId, DOW }, out o, ref oTable, CommandType.StoredProcedure);
                    if (oTable.Rows.Count > 0)
                    {
                        DataRow oRow = oTable.Rows[0];
                        l.Breaks = new BreakSchedule();
                        l.Schedule = new WorkSchedule();
                        l.Schedule.ID = Convert.ToInt64(oRow["WorkSchedId"]);
                        l.StartTime = oRow["StartTime"].ToString();
                        l.EndTime = oRow["EndTime"].ToString();
                    }
                }
                else if (eAssignment == 1)
                {
                    db.ExecuteCommandReader("HRIS_GetEmployeeDaiySchedule_LeaveRequest",
                        new string[] { "eEmployeeId", "eDay" },
                        new DbType[] { DbType.Int64, DbType.Int32 },
                        new object[] { EmpId, DOW }, out o, ref oTable, CommandType.StoredProcedure);
                    if (oTable.Rows.Count > 0)
                    {
                        DataRow oRow = oTable.Rows[0];
                        l.Breaks = new BreakSchedule();
                        l.Schedule = new WorkSchedule();
                        l.Schedule.ID = Convert.ToInt64(oRow["WorkSchedId"]);
                        l.StartTime = oRow["StartTime"].ToString();
                        l.EndTime = oRow["EndTime"].ToString();
                    }
                }
            }
            return l;
        }
        public static LeaveRequest GetEmployeeDailyTimeInOut(Int64 EmpId, Int32 DOW, Int64 eAssignment, DateTime LDate)
        {
            var l = new LeaveRequest();
            using (AppDb db = new AppDb())
            {
                db.Open();

                int p = 0;
                DataTable pTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isLeaveDateApplied_LeaveRequest", //checking if leave date is already filed
                    new string[] { "eEmployeeID", "eLeaveDate" },
                    new DbType[] { DbType.Int64, DbType.Date },
                    new object[] { EmpId, LDate }, out p, ref pTable, CommandType.StoredProcedure);
                if (pTable.Rows.Count > 0)
                {
                    //do nothing
                }
                else
                {
                    int xp = 0;
                    DataTable xpTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_isFiledOnAHoliday_LeaveRequest", // checking if Leave Date Filed on a holiday
                        new string[] { "eDate" },
                        new DbType[] { DbType.Date },
                        new object[] { LDate }, out xp, ref xpTable, CommandType.StoredProcedure);
                    if (xpTable.Rows.Count > 0)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        int o = 0;
                        DataTable oTable = new DataTable();
                        db.ExecuteCommandReader("HRIS_GetActualHoursPerDay_ProjectBased_Employee",
                               new string[] { "eEmployeeId", "eDay" },
                               new DbType[] { DbType.Int64, DbType.Int32 },
                               new object[] { EmpId, DOW }, out o, ref oTable, CommandType.StoredProcedure);
                        if (oTable.Rows.Count > 0)
                        {
                            DataRow oRow = oTable.Rows[0];
                            l.Breaks = new BreakSchedule();
                            l.Schedule = new WorkSchedule();
                            l.Schedule.ID = Convert.ToInt64(oRow["WorkSchedId"]);
                            l.StartTime = oRow["StartTime"].ToString();
                            l.EndTime = oRow["EndTime"].ToString();
                        }
                    }
                }
            }
            return l;
        }
        public static string SaveLeaveRequest(LeaveRequest details, DateTime[] LDates, double[] DayValue, string[] Reasons, Int64[] WSchedule, Int32[] DOW)
        {
            string results = "";
            using (AppDb db = new AppDb())
            {
                db.Open();

                int m = 0;
                foreach (DateTime _leaveDates in LDates)
                {
                    int a = 0;
                    int b = 0;
                    int z = 0;
                    DataTable zTable = new DataTable();
                    details.Schedule = new WorkSchedule();
                    details.LeaveDate = _leaveDates;
                    details.NumberOfHours = DayValue[m];
                    details.Remarks = Reasons[m];
                    details.Schedule.ID = WSchedule[m];
                    details.DayOfTheWeek = DOW[m];
                    int p = 0;
                    DataTable pTable = new DataTable();
                    //check if leavedate is already in employees application
                    db.ExecuteCommandReader("HRIS_isLeaveDateApplied_LeaveRequest",
                        new string[] { "eEmployeeId", "eLeaveDate" },
                        new DbType[] { DbType.Int64, DbType.Date },
                        new object[] { details.Employee.ID, details.LeaveDate }, out p, ref pTable, CommandType.StoredProcedure);
                    if (pTable.Rows.Count > 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        //check and confirm first if employee is credited with the leave credit given
                        db.ExecuteCommandReader("HRIS_IsLeaveCredited_AnnualLeaveCredit",
                            new string[] { "oEmployeeID", "oLeaveType" },
                            new DbType[] { DbType.Int64, DbType.Int64 },
                            new object[] { details.Employee.ID, details.Leave.ID }, out z, ref zTable, CommandType.StoredProcedure);
                        if (zTable.Rows.Count > 0)
                        {
                            int y = 0;
                            DataTable yTable = new DataTable();
                            //is employee has enough credit?
                            db.ExecuteCommandReader("HRIS_Get_LeaveRequestComputation",
                                 new string[] { "oEmployeeID", "oLeaveType" },
                                 new DbType[] { DbType.Int64, DbType.Int64 },
                                 new object[] { details.Employee.ID, details.Leave.ID }, out y, ref yTable, CommandType.StoredProcedure);
                            if (yTable.Rows.Count > 0)
                            {

                                DataRow yRow = yTable.Rows[0];
                                int _checkAvailable = Convert.ToInt32(yRow["Available"]);
                                int _yx = 0;
                                DataTable _yxTable = new DataTable();
                                if (_checkAvailable != 0 && details.NumberOfHours <= _checkAvailable)
                                {

                                    //need to check if there are pending applications.
                                    DataTable aTable = new DataTable();
                                    db.ExecuteCommandReader("HRIS_Save_LeaveRequest",
                                        new string[] { "oRequestNumber", "oEmployeeID", "oLeaveType", "oLeaveDate", "oNumberOfHours", "oRemarks", "oApprover", "oAddedBy", "oModifiedBy" },
                                        new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Double, DbType.String, DbType.String, DbType.String, DbType.String },
                                        new object[] { details.RequestNumber, details.Employee.ID, details.Leave.ID, details.LeaveDate, details.NumberOfHours, details.Remarks, details.Approver, details.AddedBy, details.ModifiedBy }, out a, ref aTable, CommandType.StoredProcedure);
                                    if (aTable.Rows.Count > 0)
                                    {
                                        DataRow aRow = aTable.Rows[0];
                                        long _requestID = Convert.ToInt64(aRow["ID"]);
                                        int c = 0;
                                        DataTable cTable = new DataTable();
                                        /*formula : AVAILABLE
                                        * xy = total credits
                                        * za = Total used/ Approved credits
                                        * resultDiff= xy -za
                                        * AVAILABLE = result - NumberOfHours
                                        */
                                        /*xy*/
                                        double _totalCredits = 0;
                                        double _totalUsed = 0;
                                        double _resultDiff = 0;
                                        int xy = 0;
                                        DataTable xyTable = new DataTable();
                                        db.ExecuteCommandReader("HRIS_GetTotalAnnualCredits_leaves",//here 
                                          new string[] { "eEmployeeID", "eLeaveType" },
                                          new DbType[] { DbType.Int64, DbType.Int64 },
                                          new object[] { details.Employee.ID, details.Leave.ID }, out xy, ref xyTable, CommandType.StoredProcedure);
                                        if (xyTable.Rows.Count > 0)
                                        {
                                            DataRow xyRow = xyTable.Rows[0];
                                            _totalCredits = Convert.ToDouble(xyRow["Credits"]);
                                        }
                                        /*za*/
                                        int u = 0;
                                        DataTable uTable = new DataTable();
                                        db.ExecuteCommandReader("HRIS_GetUsedCredits_LeaveCredits",//here 
                                         new string[] { "eEmployeeID", "eLeaveType" },
                                         new DbType[] { DbType.Int64, DbType.Int64 },
                                         new object[] { details.Employee.ID, details.Leave.ID }, out u, ref uTable, CommandType.StoredProcedure);
                                        if (uTable.Rows.Count > 0)
                                        {
                                            DataRow uRow = uTable.Rows[0];
                                            _totalUsed = Convert.ToDouble(uRow["Used"]);
                                        }
                                        _resultDiff = _totalCredits - _totalUsed; //resultDiff= xy -za
                                        details.LeaveComputation = new LeaveComputation();
                                        details.LeaveComputation.Used = details.NumberOfHours;
                                        details.LeaveComputation.Available = _resultDiff - details.NumberOfHours;
                                        db.ExecuteCommandNonQuery("HRIS_Save_LeaveComputation",
                                        new string[] { "oLeaveType", "oEmployeeID", "oUsed", "oAvailable", "oRequestID", "oRequestNumber" },
                                        new DbType[] { DbType.Int64, DbType.Int64, DbType.Double, DbType.Double, DbType.Int64, DbType.Int64 },
                                        new object[] { details.Leave.ID, details.Employee.ID, details.LeaveComputation.Used, details.LeaveComputation.Available, _requestID, details.RequestNumber }, out b, CommandType.StoredProcedure);
                                        details.ID = _requestID;
                                        if (details.Employee.Class.ID == 23 && details.Employee.Position.ID == 1)
                                        {
                                            db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                               new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                                        }
                                        else
                                        {
                                            LeaveRequest.SentLeaveRequest(details);
                                        }
                                        results = "ok";
                                    }
                                }
                                else
                                {
                                    results = "You don't have enough number of leave credits.  Please contact Human Resources department for inquiries.";
                                }
                            }
                        }
                        else
                        {
                            results = "You have no numbers of leave credited for the leave you selected. Please contact Human Resources department for inquiries. ";
                        }
                    }
                    m += 1;
                }
            }
            return results;
        }
        public static List<LeaveRequest> GetAllLeaveRequestByEmployee(Int64 employeeID)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAllEmployee_LeaveRequest",
                    new string[] { "oEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { employeeID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedDays = Convert.ToDouble(aRow["TotalApprovedHours"]);
                    l.NumberOfHours = Convert.ToDouble(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetAllLeaveRequestByEmployeeAndRequestNumber(Int64 employeeID, Int64 RequestNo)
        {
            var listRequest = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetLeaveRequest_EmployeePortal",
                    new string[] { "oEmployeeID", "oRequestNumber" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { employeeID, RequestNo }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = aRow["StartTime"].ToString();
                    l.EndTime = aRow["EndTime"].ToString();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.LeaveComputation = new LeaveComputation();
                    l.Approver = new Employee();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Approver.Firstname = aRow["ApproverFirstName"].ToString();
                    l.Approver.Middlename = aRow["ApproverMiddleName"].ToString();
                    l.Approver.Lastname = aRow["ApproverLastName"].ToString();
                    l.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Leave.Description = aRow["Description"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.SApproverRemarks = aRow["ApproverRemarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    listRequest.Add(l);
                }
            }
            return listRequest;
        }
        public static List<LeaveRequest> GetApplicationResults(Int64 employeeID)
        {
            var listRequest = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApplicationResults_Leave",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { employeeID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.LeaveComputation = new LeaveComputation();
                    l.Approver = new Employee();
                    l.Approver.Firstname = aRow["ApproverFirstName"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Approver.Middlename = aRow["ApproverMiddleName"].ToString();
                    l.Approver.Lastname = aRow["ApproverLastName"].ToString();
                    l.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Leave.Description = aRow["Description"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    listRequest.Add(l);
                }
            }
            return listRequest;
        }
        public static void SetStatusToHasApplicationResult(Int64 Id)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToSentApplicationResults_Leave",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, CommandType.StoredProcedure);
            }
        }
        public static LeaveRequest GetByID(Int64 Id, Int64 empID)
        {
            var l = new LeaveRequest();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetLeaveRequestByID_EmployeePortal",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { Id, empID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    //string _sTime = aRow["StartTime"].ToString();
                    //DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                    //                    CultureInfo.InvariantCulture);

                    //string _stime12HrFormat = _stimeFromDB.ToString(
                    //"hh:mm tt",
                    //CultureInfo.InvariantCulture);
                    //string _eTime = aRow["EndTime"].ToString();
                    //DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                    //                    CultureInfo.InvariantCulture);
                    //string _etime12HrFormat = _etimeFromDB.ToString(
                    //"hh:mm tt",
                    //CultureInfo.InvariantCulture);
                    l.StartTime = aRow["StartTime"].ToString();
                    l.EndTime = aRow["EndTime"].ToString();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["Description"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.Approver = new Employee();
                    l.Approver.Firstname = aRow["ApproverFirstName"].ToString();
                    l.Approver.Middlename = aRow["ApproverMiddleName"].ToString();
                    l.Approver.Lastname = aRow["ApproverLastName"].ToString();
                    l.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    l.FApproverRemarks = aRow["ApproverRemarks"].ToString();
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                }
            }
            return l;
        }
        public static List<LeaveRequest> GetToBeSentLeaveRequest(Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetTobeSent_LeaveRequest",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);

                    _list.Add(l);
                }
            }
            return _list;
        }
        public static void SetToSentLeaveNotification(Int64 Id)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetStatusToEmailSent_LeaveRequest",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, CommandType.StoredProcedure);
            }
        }
        public static string EditLeaveRequest(LeaveRequest details, Int64[] LID, Int64[] LeaveTypeID, DateTime[] LDates, string[] STime, string[] ETime, string[] Reasons, Int64[] WSchedule, Int32[] DOW)
        {
            string results = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int m = 0;
                foreach (Int64 LeaveId in LID)
                {
                    int a = 0;
                    int b = 0;
                    int c = 0;
                    int z = 0;
                    details.Schedule = new WorkSchedule();
                    details.Leave = new LeaveTypeCode();
                    details.Leave.ID = LeaveTypeID[m];
                    details.ID = LeaveId;
                    details.LeaveDate = LDates[m];
                    details.StartTime = STime[m];
                    details.EndTime = ETime[m];
                    details.Remarks = Reasons[m];
                    details.Schedule.ID = WSchedule[m];
                    details.DayOfTheWeek = DOW[m];
                    DataTable zTable = new DataTable();
                    //check and confirm first if employee is credited with the leave credit given
                    db.ExecuteCommandReader("HRIS_IsLeaveCredited_AnnualLeaveCredit",
                        new string[] { "oEmployeeID", "oLeaveType" },
                        new DbType[] { DbType.Int64, DbType.Int64 },
                        new object[] { details.Employee.ID, details.Leave.ID }, out z, ref zTable, CommandType.StoredProcedure);
                    if (zTable.Rows.Count > 0)
                    {
                        int jk = 0;
                        DataTable jkTable = new DataTable();
                        //check if requested numbers of days is not more than the available credits
                        db.ExecuteCommandReader("HRIS_Get_leaveCreditsComputationForEditing",
                               new string[] { "oEmployeeID", "oLeaveType" },
                               new DbType[] { DbType.Int64, DbType.Int64 },
                               new object[] { details.Employee.ID, details.Leave.ID }, out jk, ref jkTable, CommandType.StoredProcedure);
                        if (jkTable.Rows.Count > 0)
                        {
                            DataRow jkRow = jkTable.Rows[0];
                            int _checkAvailable = Convert.ToInt32(jkRow["Available"]);
                            int _yx = 0;
                            DataTable _yxTable = new DataTable();
                            //get and calculate the credits
                            db.ExecuteCommandReader("HRIS_GetLeaveToBeCredited_LeaveRequest",
                                 new string[] { "eStartTime", "eEndTime", "eDay", "eWorkScheduleId" },
                                new DbType[] { DbType.Time, DbType.Time, DbType.Int32, DbType.Int64 },
                                new object[] { details.StartTime, details.EndTime, details.DayOfTheWeek, details.Schedule.ID }, out _yx, ref _yxTable, CommandType.StoredProcedure);
                            if (_yxTable.Rows.Count > 0)
                            {
                                DataRow _yxRow = _yxTable.Rows[0];
                                details.NumberOfHours = Convert.ToInt32(_yxRow["LeaveCreditsToBeDeducted"]);
                            }
                            if (_checkAvailable != 0 && details.NumberOfHours <= _checkAvailable)
                            {
                                DataTable cTable = new DataTable();
                                db.ExecuteCommandNonQuery("HRIS_Edit_LeaveRequest",
                                    new string[] { "oID", "oEmployeeID", "oLeaveType", "oLeaveDate", "oStartTime","oEndTime","oNumberOfHours", "oRemarks",
                                  "oApprover", "oAddedBy", "oModifiedBy" },
                                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Time,DbType.Time, DbType.Double, DbType.String,
                                   DbType.String, DbType.String, DbType.String },
                                    new object[] { details.ID, details.Employee.ID, details.Leave.ID, details.LeaveDate, details.StartTime, details.EndTime, details.NumberOfHours, details.Remarks, details.Approver, details.AddedBy, details.ModifiedBy }, out a, CommandType.StoredProcedure);
                                db.ExecuteCommandReader("HRIS_Get_leaveCreditsComputationForEditing",
                                        new string[] { "oEmployeeID", "oLeaveType" },
                                        new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                                        new object[] { details.Employee.ID, details.Leave.ID }, out c, ref cTable, CommandType.StoredProcedure);
                                if (cTable.Rows.Count > 0)
                                {
                                    DataRow cRow = cTable.Rows[0];
                                    double _used = 0;
                                    double _available = 0;

                                    _used += Convert.ToDouble(cRow["Used"]);
                                    _available += Convert.ToDouble(cRow["Available"]);//15
                                                                                      //compute the available and used
                                    _used += details.NumberOfHours;
                                    _available -= details.NumberOfHours;
                                    details.LeaveComputation = new LeaveComputation();
                                    details.LeaveComputation.LeaveRequest = new LeaveRequest();
                                    details.LeaveComputation.Used = details.NumberOfHours;
                                    details.LeaveComputation.Available = _available;
                                    //details.LeaveComputation.LeaveRequest.ID = details.ID;
                                    db.ExecuteCommandNonQuery("HRIS_Edit_LeaveCreditsComputation",
                                    new string[] { "oLeaveType", "oEmployeeID", "oUsed", "oAvailable", "oRequestID" },
                                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Double, DbType.Double, DbType.Int64 },
                                    new object[] { details.Leave.ID, details.Employee.ID, details.LeaveComputation.Used, details.LeaveComputation.Available, details.ID }, out b, CommandType.StoredProcedure);
                                    results = "ok";
                                }
                                else
                                {
                                    results = "You don't have enough number of leave credits. Please contact Human Resources department for inquiries.";
                                }
                            }
                            else
                            {
                                results = "You don't have enough number of leave credits. Please contact Human Resources department for inquiries.";
                            }
                        }
                    }
                    else
                    {
                        results = "You have no numbers of leave credited for the leave you selected. Please contact Human Resources department for inquiries.";
                    }
                    m += 1;
                }
            }
            return results;
        }
        public static void DeleteLeaveRequest(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_LeaveRequest",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { details.ID, details.Employee.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static void CancellLeaveRequest(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Cancell_LeaveRequest",
                    new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    /*formula : AVAILABLE
                                      * xy = total credits
                                      * za = Total used/ Approved credits
                                      * resultDiff= xy -za
                                      * AVAILABLE = result - NumberOfHours
                                      */
                    /*xy*/
                    double _totalCredits = 0;
                    double _totalUsed = 0;
                    double _resultDiff = 0;
                    int xy = 0;
                    DataTable xyTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_GetTotalAnnualCredits_leaves",//here 
                      new string[] { "eEmployeeID", "eLeaveType" },
                      new DbType[] { DbType.Int64, DbType.Int64 },
                      new object[] { details.Employee.ID, details.Leave.ID }, out xy, ref xyTable, CommandType.StoredProcedure);
                    if (xyTable.Rows.Count > 0)
                    {
                        DataRow xyRow = xyTable.Rows[0];
                        _totalCredits = Convert.ToDouble(xyRow["Credits"]);
                    }
                    /*za*/
                    int u = 0;
                    DataTable uTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_GetUsedCredits_LeaveCredits",//here 
                     new string[] { "eEmployeeID", "eLeaveType" },
                     new DbType[] { DbType.Int64, DbType.Int64 },
                     new object[] { details.Employee.ID, details.Leave.ID }, out u, ref uTable, CommandType.StoredProcedure);
                    if (uTable.Rows.Count > 0)
                    {
                        DataRow uRow = uTable.Rows[0];
                        _totalUsed = Convert.ToDouble(uRow["Used"]);
                    }
                    _resultDiff = _totalCredits - _totalUsed; //resultDiff= xy -za
                    int b = 0;
                    details.LeaveComputation = new LeaveComputation();
                    details.LeaveComputation.ID = Convert.ToInt64(aRow["ID"]);
                    details.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]) - details.NumberOfHours;
                    details.LeaveComputation.Available = _resultDiff + details.NumberOfHours;
                    db.ExecuteCommandNonQuery("HRIS_UpdateLeave_CreditsComputation",
                    new string[] { "eID", "eEmployeeID", "eLeaveType", "eAvailable", "eUsed" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Double, DbType.Double },
                    new object[] { details.LeaveComputation.ID, details.Employee.ID, details.Leave.ID, details.LeaveComputation.Available, details.LeaveComputation.Used },
                    out b, CommandType.StoredProcedure);
                }

            }
        }

        public static void sendAllLeaveRequest(LeaveRequest details, Int64[] LeaveId, Int64[] EmployeeId)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int _counter = 0;
                Int64[] _items = LeaveId;
                foreach (Int64 x in _items)
                {
                    details.ID = x;
                    details.Employee.ID = EmployeeId[_counter];
                    db.ExecuteCommandNonQuery("HRIS_Sent_LeaveRequest",
                   new string[] { "oID", "oEmployeeID" },
                   new DbType[] { DbType.Int64, DbType.Int64 },
                   new object[] { details.ID, details.Employee.ID }, out a, CommandType.StoredProcedure);
                    //if supervisor, approved the application as first level approval
                    if (details.Employee.Class.ID == 20)
                    {
                        details.FirstApprover = new Employee();
                        details.ID = details.ID;
                        details.FirstApprover.ID = details.Employee.ID;
                        details.FApproverRemarks = "";
                        db.ExecuteCommandNonQuery("HRIS_FirstLevelApproved_LeaveRequest",
                            new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                            new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                            new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                    }
                    else if (details.Employee.Class.ID == 21)
                    {
                        db.ExecuteCommandNonQuery("HRIS_Executive_Approval_Leaves",
                                   new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                   new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                    }
                }
                _counter += 1;
            }
        }
        public static void SentLeaveRequest(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Sent_LeaveRequest",
                    new string[] { "oID", "oEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { details.ID, details.Employee.ID }, out a, CommandType.StoredProcedure);
                //if supervisor, approved the application as first level approval
                //if (details.Employee.Class.ID == 20)
                //{
                //    details.FirstApprover = new Employee();
                //    details.ID = details.ID;
                //    details.FirstApprover.ID = details.Employee.ID;
                //    details.FApproverRemarks = "";
                //    db.ExecuteCommandNonQuery("HRIS_FirstLevelApproved_LeaveRequest",
                //        new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                //        new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                //        new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                //}
                //else if (details.Employee.Class.ID == 21)
                //{
                //    details.FirstApprover = new Employee();
                //    details.ID = details.ID;
                //    details.FirstApprover.ID = details.Employee.ID;
                //    details.FApproverRemarks = "";
                //    db.ExecuteCommandNonQuery("HRIS_Executive_Approval_Leaves",
                //               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                //               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                //               new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                //}
                if (details.Employee.Class.ID == 23 && details.Employee.Position.ID != 1)
                {
                    details.FirstApprover = new Employee();
                    details.ID = details.ID;
                    details.FirstApprover.ID = details.Employee.ID;
                    details.FApproverRemarks = "";
                    db.ExecuteCommandNonQuery("HRIS_SetToForPresidentsApproval_Leave",
                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                               new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                }
                else if (details.Employee.Class.ID == 23 && details.Employee.Position.ID == 1)
                {
                    details.FirstApprover = new Employee();
                    details.ID = details.ID;
                    details.FirstApprover.ID = details.Employee.ID;
                    details.FApproverRemarks = "";
                    db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                                 new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                 new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                 new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                }
            }
        }
        public static void RejectedLeaveRequest(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();

                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Rejected_LeaveRequest",
                  new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                  new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                  new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    /*formula : AVAILABLE
                                      * xy = total credits
                                      * za = Total used/ Approved credits
                                      * resultDiff= xy -za
                                      * AVAILABLE = result - NumberOfHours
                                      */
                    /*xy*/
                    double _totalCredits = 0;
                    double _totalUsed = 0;
                    double _resultDiff = 0;
                    int xy = 0;
                    DataTable xyTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_GetTotalAnnualCredits_leaves",//here 
                      new string[] { "eEmployeeID", "eLeaveType" },
                      new DbType[] { DbType.Int64, DbType.Int64 },
                      new object[] { details.Employee.ID, details.Leave.ID }, out xy, ref xyTable, CommandType.StoredProcedure);
                    if (xyTable.Rows.Count > 0)
                    {
                        DataRow xyRow = xyTable.Rows[0];
                        _totalCredits = Convert.ToDouble(xyRow["Credits"]);
                    }
                    /*za*/
                    int u = 0;
                    DataTable uTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_GetUsedCredits_LeaveCredits",//here 
                     new string[] { "eEmployeeID", "eLeaveType" },
                     new DbType[] { DbType.Int64, DbType.Int64 },
                     new object[] { details.Employee.ID, details.Leave.ID }, out u, ref uTable, CommandType.StoredProcedure);
                    if (uTable.Rows.Count > 0)
                    {
                        DataRow uRow = uTable.Rows[0];
                        _totalUsed = Convert.ToDouble(uRow["Used"]);
                    }
                    _resultDiff = _totalCredits - _totalUsed; //resultDiff= xy -za
                    int b = 0;
                    details.LeaveComputation = new LeaveComputation();
                    details.LeaveComputation.ID = Convert.ToInt64(aRow["ID"]);
                    details.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]) - details.NumberOfHours;
                    details.LeaveComputation.Available = _resultDiff + details.NumberOfHours;
                    db.ExecuteCommandNonQuery("HRIS_UpdateLeave_CreditsComputation",
                    new string[] { "eID", "eEmployeeID", "eLeaveType", "eAvailable", "eUsed" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Double, DbType.Double },
                    new object[] { details.LeaveComputation.ID, details.Employee.ID, details.Leave.ID, details.LeaveComputation.Available, details.LeaveComputation.Used },
                    out b, CommandType.StoredProcedure);
                }
            }
        }
        public static void SetToForCancellation(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToFORCANCELLATION_LeaveRequest",
                    new string[] { "eID", "eEmployeeID", "eRemarks" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { details.ID, details.Employee.ID, details.Remarks }, out a, CommandType.StoredProcedure);
                //if supervisor, approved the application as first level approval
                //if (details.Employee.Class.ID == 20)//if employee is a supervisor but need approval from manager's 
                //{
                //    details.FirstApprover = new Employee();
                //    details.ID = details.ID;
                //    db.ExecuteCommandNonQuery("HRIS_SetToFORForManager_CANCELLATION__LeaveRequest",
                //        new string[] { "eID", "eEmployeeID", "eRemarks" },
                //        new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                //        new object[] { details.ID, details.Employee.ID, details.Remarks }, out a, CommandType.StoredProcedure);
                //}
                //else if (details.Employee.Class.ID == 21)//if employee is a manager but need approval for executives, including president
                //{
                //    db.ExecuteCommandNonQuery("HRIS_SetToFORForExecutives_CANCELLATION__LeaveRequest",
                //        new string[] { "eID", "eEmployeeID", "eRemarks" },
                //        new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                //        new object[] { details.ID, details.Employee.ID, details.Remarks }, out a, CommandType.StoredProcedure);
                //}
                //else if (details.Employee.Class.ID == 23 && details.Employee.Position.ID != 1)//if employee is an executive but needs president's approval
                //{
                //    db.ExecuteCommandNonQuery("HRIS_SetToFORForPresident_CANCELLATION__LeaveRequest",
                //        new string[] { "eID", "eEmployeeID", "eRemarks" },
                //        new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                //        new object[] { details.ID, details.Employee.ID, details.Remarks }, out a, CommandType.StoredProcedure);
                //}
            }
        }
        public static void ApprovedLeaveRequestFirstLevel(LeaveRequest details)
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
                // check level of superior
                db.ExecuteCommandReader("HRIS_GetBySuperiorID_ImmediateHeads",
                    new string[] { "eSupId", "eEmpId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { details.FirstApprover.ID, details.Employee.ID }, out a, ref aTable, CommandType.StoredProcedure);
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
                            db.ExecuteCommandNonQuery("HRIS_FirstLevelApproved_LeaveRequest",
                                    new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                    new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
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
                                db.ExecuteCommandNonQuery("HRIS_SecondLevelApproved_LeaveRequest",
                                   new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                   new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                            }
                            else
                            {
                                db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                                       new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                       new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                       new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                            }

                        }
                    }
                    else if (_SupLevel == 2)
                    {

                        db.ExecuteCommandReader("hris_isEmployeehasLevel3Superior_ImmediateHeads",
                              new string[] { "eEmployeeID" },
                              new DbType[] { DbType.Int64 },
                              new object[] { details.Employee.ID }, out c, ref cTable, CommandType.StoredProcedure);
                        if (cTable.Rows.Count > 0)
                        {
                            db.ExecuteCommandNonQuery("HRIS_SecondLevelApproved_LeaveRequest",
                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                               new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                        else
                        {
                            db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                                   new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                   new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                    }
                    else if (_SupLevel == 3)
                    {
                        db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                                  new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                  new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                  new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
                    }
                }
            }
        }
        public static void RejectCancellationRequest(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                               new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }
        public static void HRApproval_Leaves(LeaveRequest details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Approved_LeaveRequest",
                           new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                           new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                           new object[] { details.ID, details.Employee.ID, details.FirstApprover.ID, details.FApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<LeaveRequest> GetLeavesForApprovalFirstLevelDep(Int64 SupId, Int64 DepId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_DEP_FirstLevel_Leave",
                    new string[] { "eSuperiorID", "eDepartmentID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, DepId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeavesForApprovalSecondLevelDep(Int64 SupId, Int64 DepId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_DEP_SecondLevel_Leave",
                    new string[] { "eSuperiorID", "eDepartmentID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, DepId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeavesForApprovalThirdLevelDep(Int64 SupId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_DEP_ThirdLevel_Leave",
                    new string[] { "eSuperiorID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeavesForApprovalPresidential(Int64 SupId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_PRESIDENTIAL_FourthLevel_Leave",
                    new string[] { "eSuperiorID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeavesForApprovalFirstLevelDiv(Int64 SupId, Int64 DivId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_DIV_FirstLevel_Leave",
                    new string[] { "eSuperiorID", "eDivisionID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, DivId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeavesForApprovalSecondLevelDiv(Int64 SupId, Int64 DivId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_DIV_SecondLevel_Leave",
                    new string[] { "eSuperiorID", "eDivisionID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, DivId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeavesForApprovalThirdLevelDiv(Int64 SupId, Int64 LeaveType, Int64 EmpId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApproval_DEP_ThirdLevel_Leave",
                    new string[] { "eSuperiorID", "eLeaveType", "eEmployeeID" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { SupId, LeaveType, EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetAllLeavesForApproval(Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_AllLeaveForApproval_Leave",
                    new string[] { "eSuperiorID", },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.MySuperiorLevel = new ImmediateSuperior();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedDays = Convert.ToDouble(aRow["TotalApprovedHours"]);
                    l.NumberOfHours = Convert.ToDouble(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    l.Status = Convert.ToInt32(aRow["MaxStatus"]);
                    l.MySuperiorLevel.Level = Convert.ToInt32(aRow["Level"]);
                    _list.Add(l);
                }
            }
            return _list;
        }

        public static List<LeaveRequest> GetLeaveForFirstLevelApprovalDepartment(Int64 DepId, Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForAprovalDepartment_firstLevel_Leaves",
                    new string[] { "eDepartmentId", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DepId, SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeaveForSecondLevelApprovalDepartment(Int64 DepId, Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForAprovalDepartment_SecondLevel_Leaves",
                    new string[] { "eDepartmentId", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DepId, SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeaveFoHRApproval(Int64 LeaveType, Int64 EmpId, Int64 Dep, Int64 Div)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader(" HRIS_ForHRApproval_HR_Leaves",
                    new string[] { "eLeaveType", "eEmployeeID", "eDepartment", "eDivision" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { LeaveType, EmpId, Dep, Div }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Superior = new List<ImmediateSuperior>();
                    l.Employee.Department = new Department();
                    l.Employee.Division = new Division();
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeId"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.TotalApprovedHours = Convert.ToInt32(aRow["TotalApprovedHours"]);
                    l.TotalHours = Convert.ToInt32(aRow["TotalHours"]);
                    l.StartDate = Convert.ToDateTime(aRow["StartDate"]);
                    l.EndDate = Convert.ToDateTime(aRow["EndDate"]);
                    l.Leave.Description = aRow["LeaveType"].ToString();
                    l.Superior = ImmediateSuperior.GetAssignedSuperiors(l.Employee.ID);
                    l.Employee.Department.Description = aRow["Department"].ToString();
                    l.Employee.Division.Div_Desc = aRow["Division"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetFilteredLeaveFoHRApproval(Int64 Div, Int64 Dep, Int64 Pos, Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_LeaveRequestFiltered_HRApproval",
                    new string[] { "eDivision", "eDepartment", "ePosition", "eEmployeeId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { Div, Dep, Pos, EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.Department = new Department();
                    l.Employee.Division = new Division();

                    l.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    l.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    l.Superior = new List<ImmediateSuperior>();
                    l.Superior = ImmediateSuperior.GetAssignedSuperiors(l.Employee.ID);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeaveForThirdLevelApprovalDepartment(Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForAprovalDepartment_ThirdLevel_Leaves",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeaveForFourthLevelApproval(Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForPresidentAproval_FourthLevel_Leaves",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static LeaveRequest GetLeaveRequestForApprovalById(Int64 Id)
        {
            var l = new LeaveRequest();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByIDForApproval_Leaves",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    //string _sTime = aRow["StartTime"].ToString();
                    //DateTime _stimeFromDB = DateTime.ParseExact(_sTime, "HH:mm:ss",
                    //                    CultureInfo.InvariantCulture);

                    //string _stime12HrFormat = _stimeFromDB.ToString(
                    //"hh:mm tt",
                    //CultureInfo.InvariantCulture);
                    //string _eTime = aRow["EndTime"].ToString();
                    //DateTime _etimeFromDB = DateTime.ParseExact(_eTime, "HH:mm:ss",
                    //                    CultureInfo.InvariantCulture);
                    //string _etime12HrFormat = _etimeFromDB.ToString(
                    //"hh:mm tt",
                    //CultureInfo.InvariantCulture);
                    l.StartTime = aRow["StartTime"].ToString();
                    l.EndTime = aRow["EndTime"].ToString();
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.FApproverRemarks = aRow["FirstApproverRemarks"].ToString();
                }
            }
            return l;
        }
        public static List<LeaveRequest> GetLeaveForFirstLevelApprovalDivision(Int64 DivId, Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForAprovalDivision_firstLevel_Leaves",
                    new string[] { "eDivisionId", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DivId, SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeaveForSecondLevelApprovalDivision(Int64 DivId, Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForAprovalDivision_SecondLevel_Leaves",
                    new string[] { "eDivisionId", "eSuperiorId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { DivId, SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<LeaveRequest> GetLeaveForThirdLevelApprovalDivision(Int64 SupId)
        {
            var _list = new List<LeaveRequest>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForAprovalDivision_ThirdLevel_Leaves",
                    new string[] { "eSuperiorId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);

                foreach (DataRow aRow in aTable.Rows)
                {
                    LeaveRequest l = new LeaveRequest();
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
                    l.StartTime = _stime12HrFormat;
                    l.EndTime = _etime12HrFormat;
                    l.Employee = new Employee();
                    l.Leave = new LeaveTypeCode();
                    l.Position = new PositionCode();
                    l.FirstApprover = new Employee();
                    l.FPosition = new PositionCode();
                    l.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.RequestNumber = Convert.ToInt64(aRow["RequestNumber"]);
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Position.Position = aRow["Position"].ToString();
                    l.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Leave.Description = aRow["LeaveDescription"].ToString();
                    l.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveComputation.Used = Convert.ToDouble(aRow["Used"]);
                    l.AnnualLeaveCredits = new AnnualLeaveCredits();
                    l.AnnualLeaveCredits.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    l.FirstApprover.Firstname = aRow["fApproverFirstName"].ToString();
                    l.FirstApprover.Middlename = aRow["fApproverMiddleName"].ToString();
                    l.FirstApprover.Lastname = aRow["fApproverLastName"].ToString();
                    l.FirstApprover.Suffix = aRow["fApproverSuffix"].ToString();
                    l.FPosition.Position = aRow["fAPosition"].ToString();
                    l.FCLass = new EmployeeClassCode();
                    l.FCLass.Description = aRow["fAClassification"].ToString();
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.Remarks = aRow["Remarks"].ToString();
                    l.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
    }

    public class LeaveComputation
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public Leave Leave { get; set; }
        public double Used { get; set; }
        public double Available { get; set; }
        public int Status { get; set; }
        public LeaveRequest LeaveRequest { get; set; }

        //this count of available credits will be displayed with the leave request
        //approved or not this will still mark as umber less the current available credits
        //see line 490 to 492
        public static LeaveComputation GetUsedAndAvailable(Int64 empId, Int64 leaveId)
        {
            var l = new LeaveComputation();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetCreditsComputationForRequestTable_EmployeePortal",
                    new string[] { "oEmployeeID", "oLeaveID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { empId, leaveId }, out a, ref aTable, CommandType.StoredProcedure);
            }
            return l;
        }

    }
    public class AnnualLeaveCredits
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public Leave Leave { get; set; }
        public string Description { get; set; }
        public double Credits { get; set; }
        public double Used { get; set; }
        public double Available { get; set; }
        public int Status { get; set; }

        public static List<AnnualLeaveCredits> GetAnnualLeaveCredits(Int64 employeeId)
        {
            var leaveList = new List<AnnualLeaveCredits>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();

                db.ExecuteCommandReader("HRIS_GetLeaveCredits_AnnualLeaveCredits",
                    new string[] { "oEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { employeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    AnnualLeaveCredits lc = new AnnualLeaveCredits();
                    lc.ID = Convert.ToInt64(aRow["ID"]);
                    lc.Description = aRow["Description"].ToString();
                    lc.Credits = Convert.ToDouble(aRow["TotalCredits"]);
                    lc.Used = Convert.ToDouble(aRow["Used"]);
                    lc.Available = Convert.ToDouble(aRow["Available"]);
                    leaveList.Add(lc);
                }
            }
            return leaveList;
        }
    }
}
