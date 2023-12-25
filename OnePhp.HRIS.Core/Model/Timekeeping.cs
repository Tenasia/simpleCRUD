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
    public class TimeKeeping
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public DateTime DTRDate { get; set; }
        public DateTime SchedTimeIn { get; set; }
        public DateTime SchedTimeOut { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? AdjustedTimeIn { get; set; }
        public DateTime? AdjustedTimeOut { get; set; }
        public Double OverTime { get; set; }
        public Double Tardiness { get; set; }
        public bool IsLeave { get; set; }
        public bool IsOB { get; set; }
        public bool IsAbsent { get; set; }
        public long PayOutID { get; set; }
        public bool isProcessed { get; set; } // is PayPeiod is processed for payroll?
        public LeaveRequest LeaveRequest { get; set; }
        public OverTime OT { get; set; }
        public OfficialBusiness officialBusiness { get; set; }
        public string AddedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static List<TimeKeeping> GetTimekeepingReports()
        {
            var _list = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable TableResults = new DataTable();
                db.ExecuteCommandReader("Reports_TimeKeeping",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out affectedRows, ref TableResults, CommandType.StoredProcedure);
                foreach (DataRow ResultsRow in TableResults.Rows)
                {
                    TimeKeeping t = new TimeKeeping();
                    t.Employee = new Employee();
                    t.ID = Convert.ToInt64(ResultsRow["ID"]);
                    t.Employee.ID = Convert.ToInt64(ResultsRow["EmployeeID"]);
                    t.Employee.Firstname = ResultsRow["FirstName"].ToString();
                    t.Employee.Middlename = ResultsRow["MiddleName"].ToString();
                    t.Employee.Lastname = ResultsRow["LastName"].ToString();
                    t.Employee.Suffix = ResultsRow["Suffix"].ToString();
                    t.DTRDate = Convert.ToDateTime(ResultsRow["DTRDate"]);
                    t.SchedTimeIn = Convert.ToDateTime(ResultsRow["SchedTimeIn"]);
                    t.SchedTimeOut = Convert.ToDateTime(ResultsRow["SchedTimeOut"]);
                    //t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    //t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    //t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    //t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    if (ResultsRow["TimeIn"] == DBNull.Value)
                    {
                        t.TimeIn = null;
                    }
                    else
                    {
                        t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    }
                    if (ResultsRow["TimeOut"] == DBNull.Value)
                    {
                        t.TimeOut = null;
                    }
                    else
                    {
                        t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    }
                    if (ResultsRow["AdjustedTimeIn"] == DBNull.Value)
                    {
                        t.AdjustedTimeIn = null;
                    }
                    else
                    {
                        t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    }
                    if (ResultsRow["AdjustedTimeOut"] == DBNull.Value)
                    {
                        t.AdjustedTimeOut = null;
                    }
                    else
                    {
                        t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    }
                    t.IsLeave = Convert.ToBoolean(ResultsRow["IsLeave"]);
                    t.IsOB = Convert.ToBoolean(ResultsRow["IsOB"]);
                    t.IsAbsent = Convert.ToBoolean(ResultsRow["IsAbsent"]);
                    t.OverTime = Convert.ToDouble(ResultsRow["OverTime"]);
                    t.Tardiness = Convert.ToDouble(ResultsRow["Tardiness"]);
                    t.PayOutID = Convert.ToInt64(ResultsRow["PayOutID"]);
                    _list.Add(t);
                }
            }
            return _list;
        }
        public static TimeKeeping IsPayPeriodProcessed(Int64 pID)
        {
            var t = new TimeKeeping();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable TableResults = new DataTable();
                db.ExecuteCommandReader("HRIS_isPayPeriodProcessed_Timekeeping",
                    new string[] { "pId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { pID }, out x, ref TableResults, CommandType.StoredProcedure);
                if (TableResults.Rows.Count > 0)
                {
                    DataRow oRow = TableResults.Rows[0];
                    if (Convert.ToInt64(oRow["ID"]) > 0)
                    {
                        t.isProcessed = true;
                    }
                    else
                    {
                        t.isProcessed = false;
                    }
                }
            }
            return t;
        }
        public static List<TimeKeeping> GetTimekeepingReportsByEmpId(Int64 EmpId)
        {
            var _list = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable TableResults = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByEmpId_TimeKeeping",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out affectedRows, ref TableResults, CommandType.StoredProcedure);
                foreach (DataRow ResultsRow in TableResults.Rows)
                {
                    TimeKeeping t = new TimeKeeping();
                    t.Employee = new Employee();
                    t.ID = Convert.ToInt64(ResultsRow["ID"]);
                    t.Employee.ID = Convert.ToInt64(ResultsRow["EmployeeID"]);
                    t.Employee.Firstname = ResultsRow["FirstName"].ToString();
                    t.Employee.Middlename = ResultsRow["MiddleName"].ToString();
                    t.Employee.Lastname = ResultsRow["LastName"].ToString();
                    t.Employee.Suffix = ResultsRow["Suffix"].ToString();
                    t.DTRDate = Convert.ToDateTime(ResultsRow["DTRDate"]);
                    t.SchedTimeIn = Convert.ToDateTime(ResultsRow["SchedTimeIn"]);
                    t.SchedTimeOut = Convert.ToDateTime(ResultsRow["SchedTimeOut"]);
                    if (ResultsRow["TimeIn"] == DBNull.Value)
                    {
                        t.TimeIn = null;
                    }
                    else
                    {
                        t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    }
                    if (ResultsRow["TimeOut"] == DBNull.Value)
                    {
                        t.TimeOut = null;
                    }
                    else
                    {
                        t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    }
                    if (ResultsRow["AdjustedTimeIn"] == DBNull.Value)
                    {
                        t.AdjustedTimeIn = null;
                    }
                    else
                    {
                        t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    }
                    if (ResultsRow["AdjustedTimeOut"] == DBNull.Value)
                    {
                        t.AdjustedTimeOut = null;
                    }
                    else
                    {
                        t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    }
                    if (ResultsRow["IsLeave"] == DBNull.Value)
                    {
                        t.IsLeave = false;
                    }
                    else
                    {
                        t.IsLeave = Convert.ToBoolean(ResultsRow["IsLeave"]);
                    }
                    if (ResultsRow["IsOB"] == DBNull.Value)
                    {
                        t.IsOB = false;
                    }
                    else
                    {
                        t.IsOB = Convert.ToBoolean(ResultsRow["IsOB"]);
                    }
                    if (ResultsRow["IsAbsent"] == DBNull.Value)
                    {
                        t.IsAbsent = false;
                    }
                    else
                    {
                        t.IsAbsent = Convert.ToBoolean(ResultsRow["IsAbsent"]);
                    }
                    t.OverTime = Convert.ToDouble(ResultsRow["OverTime"]);
                    t.Tardiness = Convert.ToDouble(ResultsRow["Tardiness"]);
                    t.PayOutID = Convert.ToInt64(ResultsRow["PayOutID"]);
                    _list.Add(t);
                }
            }
            return _list;
        }
        public static List<TimeKeeping> GenerateReportByDTRDateAndPayMode(long PayMode, DateTime StartDate, DateTime EndDate)
        {
            var _list = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable TableResults = new DataTable();
                db.ExecuteCommandReader("Reports_GetByDTRDateAndPayRate_TimeKeeping",
                    new string[] { "tPayMode", "tDTRStartDate", "tDTREndDate" },
                    new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { PayMode, StartDate, EndDate }, out affectedRows, ref TableResults, CommandType.StoredProcedure);
                foreach (DataRow ResultsRow in TableResults.Rows)
                {
                    TimeKeeping t = new TimeKeeping();
                    t.Employee = new Employee();
                    t.ID = Convert.ToInt64(ResultsRow["ID"]);
                    t.Employee.ID = Convert.ToInt64(ResultsRow["EmployeeID"]);
                    t.Employee.Firstname = ResultsRow["FirstName"].ToString();
                    t.Employee.Middlename = ResultsRow["MiddleName"].ToString();
                    t.Employee.Lastname = ResultsRow["LastName"].ToString();
                    t.Employee.Suffix = ResultsRow["Suffix"].ToString();
                    t.DTRDate = Convert.ToDateTime(ResultsRow["DTRDate"]);
                    t.SchedTimeIn = Convert.ToDateTime(ResultsRow["SchedTimeIn"]);
                    t.SchedTimeOut = Convert.ToDateTime(ResultsRow["SchedTimeOut"]);
                    //t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    //t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    //t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    //t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    if (ResultsRow["TimeIn"] == DBNull.Value)
                    {
                        t.TimeIn = null;
                    }
                    else
                    {
                        t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    }
                    if (ResultsRow["TimeOut"] == DBNull.Value)
                    {
                        t.TimeOut = null;
                    }
                    else
                    {
                        t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    }
                    if (ResultsRow["AdjustedTimeIn"] == DBNull.Value)
                    {
                        t.AdjustedTimeIn = null;
                    }
                    else
                    {
                        t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    }
                    if (ResultsRow["AdjustedTimeOut"] == DBNull.Value)
                    {
                        t.AdjustedTimeOut = null;
                    }
                    else
                    {
                        t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    }
                    if (ResultsRow["IsLeave"] == DBNull.Value)
                    {
                        t.IsLeave = false;
                    }
                    else
                    {
                        t.IsLeave = Convert.ToBoolean(ResultsRow["IsLeave"]);
                    }
                    if (ResultsRow["IsOB"] == DBNull.Value)
                    {
                        t.IsOB = false;
                    }
                    else
                    {
                        t.IsOB = Convert.ToBoolean(ResultsRow["IsOB"]);
                    }
                    if (ResultsRow["IsAbsent"] == DBNull.Value)
                    {
                        t.IsAbsent = false;
                    }
                    else
                    {
                        t.IsAbsent = Convert.ToBoolean(ResultsRow["IsAbsent"]);
                    }
                    t.OverTime = Convert.ToDouble(ResultsRow["OverTime"]);
                    t.Tardiness = Convert.ToDouble(ResultsRow["Tardiness"]);
                    t.PayOutID = Convert.ToInt64(ResultsRow["PayOutID"]);
                    _list.Add(t);
                }
            }
            return _list;
        }
        public static List<TimeKeeping> GenerateReportByDateScheduleAndEmpId(long EmpId, DateTime StartDate, DateTime EndDate)
        {
            var _list = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable TableResults = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByDateRangeAndEmpId_TimeKeeping",
                    new string[] { "eEmpId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { EmpId, StartDate, EndDate }, out affectedRows, ref TableResults, CommandType.StoredProcedure);
                foreach (DataRow ResultsRow in TableResults.Rows)
                {
                    TimeKeeping t = new TimeKeeping();
                    t.Employee = new Employee();
                    t.ID = Convert.ToInt64(ResultsRow["ID"]);
                    t.Employee.ID = Convert.ToInt64(ResultsRow["EmployeeID"]);
                    t.Employee.Firstname = ResultsRow["FirstName"].ToString();
                    t.Employee.Middlename = ResultsRow["MiddleName"].ToString();
                    t.Employee.Lastname = ResultsRow["LastName"].ToString();
                    t.Employee.Suffix = ResultsRow["Suffix"].ToString();
                    t.DTRDate = Convert.ToDateTime(ResultsRow["DTRDate"]);
                    t.SchedTimeIn = Convert.ToDateTime(ResultsRow["SchedTimeIn"]);
                    t.SchedTimeOut = Convert.ToDateTime(ResultsRow["SchedTimeOut"]);
                    //t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    //t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    //t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    //t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    if (ResultsRow["TimeIn"] == DBNull.Value)
                    {
                        t.TimeIn = null;
                    }
                    else
                    {
                        t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    }
                    if (ResultsRow["TimeOut"] == DBNull.Value)
                    {
                        t.TimeOut = null;
                    }
                    else
                    {
                        t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    }
                    if (ResultsRow["AdjustedTimeIn"] == DBNull.Value)
                    {
                        t.AdjustedTimeIn = null;
                    }
                    else
                    {
                        t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    }
                    if (ResultsRow["AdjustedTimeOut"] == DBNull.Value)
                    {
                        t.AdjustedTimeOut = null;
                    }
                    else
                    {
                        t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    }
                    if (ResultsRow["IsLeave"] == DBNull.Value)
                    {
                        t.IsLeave = false;
                    }
                    else
                    {
                        t.IsLeave = Convert.ToBoolean(ResultsRow["IsLeave"]);
                    }
                    if (ResultsRow["IsOB"] == DBNull.Value)
                    {
                        t.IsOB = false;
                    }
                    else
                    {
                        t.IsOB = Convert.ToBoolean(ResultsRow["IsOB"]);
                    }
                    if (ResultsRow["IsAbsent"] == DBNull.Value)
                    {
                        t.IsAbsent = false;
                    }
                    else
                    {
                        t.IsAbsent = Convert.ToBoolean(ResultsRow["IsAbsent"]);
                    }
                    t.OverTime = Convert.ToDouble(ResultsRow["OverTime"]);
                    t.Tardiness = Convert.ToDouble(ResultsRow["Tardiness"]);
                    t.PayOutID = Convert.ToInt64(ResultsRow["PayOutID"]);
                    _list.Add(t);
                }
            }
            return _list;
        }
        public static List<TimeKeeping> GenerateReportByDateRange(DateTime StartDate, DateTime EndDate)
        {
            var _list = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable TableResults = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByDateRange_TimeKeeping",
                    new string[] { "eStartDate", "eEndDate" },
                    new DbType[] { DbType.DateTime, DbType.DateTime },
                    new object[] { StartDate, EndDate }, out affectedRows, ref TableResults, CommandType.StoredProcedure);
                foreach (DataRow ResultsRow in TableResults.Rows)
                {
                    TimeKeeping t = new TimeKeeping();
                    t.Employee = new Employee();
                    t.ID = Convert.ToInt64(ResultsRow["ID"]);
                    t.Employee.ID = Convert.ToInt64(ResultsRow["EmployeeID"]);
                    t.Employee.Firstname = ResultsRow["FirstName"].ToString();
                    t.Employee.Middlename = ResultsRow["MiddleName"].ToString();
                    t.Employee.Lastname = ResultsRow["LastName"].ToString();
                    t.Employee.Suffix = ResultsRow["Suffix"].ToString();
                    t.DTRDate = Convert.ToDateTime(ResultsRow["DTRDate"]);
                    t.SchedTimeIn = Convert.ToDateTime(ResultsRow["SchedTimeIn"]);
                    t.SchedTimeOut = Convert.ToDateTime(ResultsRow["SchedTimeOut"]);
                    //t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    //t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    //t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    //t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    if (ResultsRow["TimeIn"] == DBNull.Value)
                    {
                        t.TimeIn = null;
                    }
                    else
                    {
                        t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    }
                    if (ResultsRow["TimeOut"] == DBNull.Value)
                    {
                        t.TimeOut = null;
                    }
                    else
                    {
                        t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    }
                    if (ResultsRow["AdjustedTimeIn"] == DBNull.Value)
                    {
                        t.AdjustedTimeIn = null;
                    }
                    else
                    {
                        t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    }
                    if (ResultsRow["AdjustedTimeOut"] == DBNull.Value)
                    {
                        t.AdjustedTimeOut = null;
                    }
                    else
                    {
                        t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    }
                    if (ResultsRow["IsLeave"] == DBNull.Value)
                    {
                        t.IsLeave = false;
                    }
                    else
                    {
                        t.IsLeave = Convert.ToBoolean(ResultsRow["IsLeave"]);
                    }
                    if (ResultsRow["IsOB"] == DBNull.Value)
                    {
                        t.IsOB = false;
                    }
                    else
                    {
                        t.IsOB = Convert.ToBoolean(ResultsRow["IsOB"]);
                    }
                    if (ResultsRow["IsAbsent"] == DBNull.Value)
                    {
                        t.IsAbsent = false;
                    }
                    else
                    {
                        t.IsAbsent = Convert.ToBoolean(ResultsRow["IsAbsent"]);
                    }
                    t.OverTime = Convert.ToDouble(ResultsRow["OverTime"]);
                    t.Tardiness = Convert.ToDouble(ResultsRow["Tardiness"]);
                    t.PayOutID = Convert.ToInt64(ResultsRow["PayOutID"]);
                    _list.Add(t);
                }
            }
            return _list;
        }
        public static TimeKeeping GetTimeKeepingRecordsByID(Int64 Id)
        {
            var t = new TimeKeeping();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetTimeKeepingRecordByID_TimeKeeping",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                if (resultsTable.Rows.Count > 0)
                {
                    DataRow ResultsRow = resultsTable.Rows[0];
                    t.Employee = new Employee();
                    t.ID = Convert.ToInt64(ResultsRow["ID"]);
                    t.Employee.ID = Convert.ToInt64(ResultsRow["EmployeeID"]);
                    t.Employee.Firstname = ResultsRow["FirstName"].ToString();
                    t.Employee.Middlename = ResultsRow["MiddleName"].ToString();
                    t.Employee.Lastname = ResultsRow["LastName"].ToString();
                    t.Employee.Suffix = ResultsRow["Suffix"].ToString();
                    t.DTRDate = Convert.ToDateTime(ResultsRow["DTRDate"]);
                    t.SchedTimeIn = Convert.ToDateTime(ResultsRow["SchedTimeIn"]);
                    t.SchedTimeOut = Convert.ToDateTime(ResultsRow["SchedTimeOut"]);
                    //t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    //t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    //t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    //t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    if (ResultsRow["TimeIn"] == DBNull.Value)
                    {
                        t.TimeIn = null;
                    }
                    else
                    {
                        t.TimeIn = Convert.ToDateTime(ResultsRow["TimeIn"]);
                    }
                    if (ResultsRow["TimeOut"] == DBNull.Value)
                    {
                        t.TimeOut = null;
                    }
                    else
                    {
                        t.TimeOut = Convert.ToDateTime(ResultsRow["TimeOut"]);
                    }
                    if (ResultsRow["AdjustedTimeIn"] == DBNull.Value)
                    {
                        t.AdjustedTimeIn = null;
                    }
                    else
                    {
                        t.AdjustedTimeIn = Convert.ToDateTime(ResultsRow["AdjustedTimeIn"]);
                    }
                    if (ResultsRow["AdjustedTimeOut"] == DBNull.Value)
                    {
                        t.AdjustedTimeOut = null;
                    }
                    else
                    {
                        t.AdjustedTimeOut = Convert.ToDateTime(ResultsRow["AdjustedTimeOut"]);
                    }
                    t.IsLeave = Convert.ToBoolean(ResultsRow["IsLeave"]);
                    t.IsOB = Convert.ToBoolean(ResultsRow["IsOB"]);
                    t.IsAbsent = Convert.ToBoolean(ResultsRow["IsAbsent"]);
                    t.OverTime = Convert.ToDouble(ResultsRow["OverTime"]);
                    t.Tardiness = Convert.ToDouble(ResultsRow["Tardiness"]);
                    t.PayOutID = Convert.ToInt64(ResultsRow["PayOutID"]);
                }
            }
            return t;
        }
        public static void AdjustTimeKeepingRecords(TimeKeeping data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                db.ExecuteCommandNonQuery("HRIS_AdjustTimeInAndOut_TimeKeeping",
                    new string[] { "eID", "eTimeIn", "eTimeOut" },
                    new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { data.ID, data.AdjustedTimeIn, data.AdjustedTimeOut }, out affectedRows, CommandType.StoredProcedure);
                data.Logs.After = "ID:"+data.ID+ ", ID:" + data.AdjustedTimeIn + ", ID:" + data.AdjustedTimeOut + ",";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        //approved leave request
        public static List<TimeKeeping> GetAllLeaveRequestByDateRangeAndEmpId(Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var listRequest = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedByDateRangeAndEmpId_LeaveRequest",
                    new string[] { "eEmpId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TimeKeeping l = new TimeKeeping();
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
                    l.LeaveRequest = new LeaveRequest();
                    l.Employee = new Employee();
                    l.LeaveRequest.Leave = new LeaveTypeCode();
                    l.LeaveRequest.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.LeaveRequest.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.LeaveRequest.Leave.Description = aRow["LeaveDesc"].ToString();
                    l.LeaveRequest.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveRequest.StartTime = _stime12HrFormat;
                    l.LeaveRequest.EndTime = _etime12HrFormat;
                    l.LeaveRequest.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveRequest.Approver = new Employee();
                    l.LeaveRequest.Approver.Firstname = aRow["ApproverFirstName"].ToString();
                    l.LeaveRequest.Approver.Middlename = aRow["ApproverMiddleName"].ToString();
                    l.LeaveRequest.Approver.Lastname = aRow["ApproverLastName"].ToString();
                    l.LeaveRequest.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    l.LeaveRequest.FCLass = new EmployeeClassCode();
                    l.LeaveRequest.FCLass.Description = aRow["AClass"].ToString();
                    l.LeaveRequest.FPosition = new PositionCode();
                    l.LeaveRequest.FPosition.Position = aRow["FPosition"].ToString();
                    l.LeaveRequest.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveRequest.Remarks = aRow["Remarks"].ToString();
                    l.LeaveRequest.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    l.LeaveRequest.Status = Convert.ToInt32(aRow["Status"]);
                    l.LeaveRequest.FApproverRemarks = aRow["ApproverRemarks"].ToString();
                    listRequest.Add(l);
                }
            }
            return listRequest;
        }
        public static List<TimeKeeping> GetAllLeaveRequestByDateRange(DateTime StartDate, DateTime EndDate, Int64 Id)
        {
            var listRequest = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedByDateRange_LeaveRequest",
                    new string[] { "eStartDate", "eEndDate", "eId" },
                    new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int64 },
                    new object[] { StartDate, EndDate, Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TimeKeeping l = new TimeKeeping();
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
                    l.LeaveRequest = new LeaveRequest();
                    l.Employee = new Employee();
                    l.LeaveRequest.Leave = new LeaveTypeCode();
                    l.LeaveRequest.LeaveComputation = new LeaveComputation();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    l.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.LeaveRequest.Leave.ID = Convert.ToInt64(aRow["LeaveType"]);
                    l.LeaveRequest.Leave.Description = aRow["LeaveDesc"].ToString();
                    l.LeaveRequest.NumberOfHours = Convert.ToDouble(aRow["NumberOfHours"]);
                    l.LeaveRequest.StartTime = _stime12HrFormat;
                    l.LeaveRequest.EndTime = _etime12HrFormat;
                    l.LeaveRequest.LeaveDate = Convert.ToDateTime(aRow["LeaveDate"]);
                    l.LeaveRequest.Approver = new Employee();
                    l.LeaveRequest.Approver.Firstname = aRow["ApproverFirstName"].ToString();
                    l.LeaveRequest.Approver.Middlename = aRow["ApproverMiddleName"].ToString();
                    l.LeaveRequest.Approver.Lastname = aRow["ApproverLastName"].ToString();
                    l.LeaveRequest.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    l.LeaveRequest.FCLass = new EmployeeClassCode();
                    l.LeaveRequest.FCLass.Description = aRow["AClass"].ToString();
                    l.LeaveRequest.FPosition = new PositionCode();
                    l.LeaveRequest.FPosition.Position = aRow["FPosition"].ToString();
                    l.LeaveRequest.LeaveComputation.Available = Convert.ToDouble(aRow["Available"]);
                    l.LeaveRequest.Remarks = aRow["Remarks"].ToString();
                    l.LeaveRequest.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    l.LeaveRequest.Status = Convert.ToInt32(aRow["Status"]);
                    l.LeaveRequest.FApproverRemarks = aRow["ApproverRemarks"].ToString();
                    listRequest.Add(l);
                }
            }
            return listRequest;
        }
        public static List<TimeKeeping> GetApprovedOTByEmpIDAndDateRange(Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var OTList = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedByDateRangeandEmpId_OT",
                  new string[] { "eEmpID", "eStartDate", "eEndDate" },
                  new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                  new object[] { EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TimeKeeping o = new TimeKeeping();
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
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.OT = new OverTime();
                    o.Employee = new Employee();
                    o.OT.Employee = new Employee();
                    o.OT.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OT.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.OT.StartTime = _stime12HrFormat;
                    o.OT.EndTIme = _etime12HrFormat;
                    o.OT.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.OT.Approver = new Employee();
                    o.OT.Approver.Firstname = aRow["AFirstname"].ToString();
                    o.OT.Approver.Lastname = aRow["ALastname"].ToString();
                    o.OT.Approver.Middlename = aRow["AMiddlename"].ToString();
                    o.OT.Approver.Suffix = aRow["ASuffix"].ToString();
                    o.OT.ApproverClass = new EmployeeClassCode();
                    o.OT.ApproverClass.Description = aRow["AClass"].ToString();
                    o.OT.ApproverPosition = new PositionCode();
                    o.OT.ApproverPosition.Position = aRow["APosition"].ToString();
                    o.OT.Purpose = aRow["Purpose"].ToString();
                    o.OT.Remarks = aRow["Remarks"].ToString();
                    o.OT.Status = Convert.ToInt32(aRow["Status"]);
                    o.OT.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<TimeKeeping> GetApprovedOTByDateRange(DateTime StartDate, DateTime EndDate, Int64 Id)
        {
            var OTList = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedByDateRange_OT",
                  new string[] { "eStartDate", "eEndDate", "eId"},
                  new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int64 },
                  new object[] { StartDate, EndDate, Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TimeKeeping o = new TimeKeeping();
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
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.OT = new OverTime();
                    o.Employee = new Employee();
                    o.OT.Employee = new Employee();
                    o.OT.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    o.Employee.EmployeeID= aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OT.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.OT.StartTime = _stime12HrFormat;
                    o.OT.EndTIme = _etime12HrFormat;
                    o.OT.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.OT.Approver = new Employee();
                    o.OT.Approver.Firstname = aRow["AFirstname"].ToString();
                    o.OT.Approver.Lastname = aRow["ALastname"].ToString();
                    o.OT.Approver.Middlename = aRow["AMiddlename"].ToString();
                    o.OT.Approver.Suffix = aRow["ASuffix"].ToString();
                    o.OT.ApproverClass = new EmployeeClassCode();
                    o.OT.ApproverClass.Description = aRow["AClass"].ToString();
                    o.OT.ApproverPosition = new PositionCode();
                    o.OT.ApproverPosition.Position = aRow["APosition"].ToString();
                    o.OT.Purpose = aRow["Purpose"].ToString();
                    o.OT.Remarks = aRow["Remarks"].ToString();
                    o.OT.Status = Convert.ToInt32(aRow["Status"]);
                    o.OT.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<TimeKeeping> GetApprovedOBByEmpIDAndDateRange(Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var OBList = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedByDateRangeandEmpId_OB",
                    new string[] { "eEmpID", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TimeKeeping o = new TimeKeeping();
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
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.officialBusiness = new OfficialBusiness();
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.officialBusiness.StartTime = _stime12HrFormat;
                    o.officialBusiness.EndTime = _etime12HrFormat;
                    o.officialBusiness.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    o.officialBusiness.Remarks = aRow["Remarks"].ToString();
                    o.officialBusiness.Date = Convert.ToDateTime(aRow["Date"]);
                    o.officialBusiness.Details = aRow["Details"].ToString();
                    o.officialBusiness.Purpose = aRow["Purpose"].ToString();
                    o.officialBusiness.Destination = aRow["Destination"].ToString();
                    o.officialBusiness.Status = Convert.ToInt32(aRow["Status"]);
                    o.officialBusiness.Approver = new Employee();
                    o.officialBusiness.Approver.Firstname = aRow["AFirstName"].ToString();
                    o.officialBusiness.Approver.Lastname = aRow["ALastName"].ToString();
                    o.officialBusiness.Approver.Middlename = aRow["AMiddleName"].ToString();
                    o.officialBusiness.Approver.Suffix = aRow["ASuffix"].ToString();
                    o.officialBusiness.ApproverClass = new EmployeeClassCode();
                    o.officialBusiness.ApproverClass.Description = aRow["AClass"].ToString();
                    o.officialBusiness.ApproverPosition = new PositionCode();
                    o.officialBusiness.ApproverPosition.Position = aRow["APosition"].ToString();
                    o.officialBusiness.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.officialBusiness.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static List<TimeKeeping> GetApprovedOBByDateRange(DateTime StartDate, DateTime EndDate, Int64 Id)
        {
            var OBList = new List<TimeKeeping>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApprovedByDateRange_OB",
                    new string[] { "eStartDate", "eEndDate", "eId" },
                    new DbType[] { DbType.DateTime, DbType.DateTime, DbType.Int64 },
                    new object[] { StartDate, EndDate, Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    TimeKeeping o = new TimeKeeping();
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
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.officialBusiness = new OfficialBusiness();
                    o.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.officialBusiness.StartTime = _stime12HrFormat;
                    o.officialBusiness.EndTime = _etime12HrFormat;
                    o.officialBusiness.TotalHours = Convert.ToDouble(aRow["TotalHours"]);
                    o.officialBusiness.Remarks = aRow["Remarks"].ToString();
                    o.officialBusiness.Date = Convert.ToDateTime(aRow["Date"]);
                    o.officialBusiness.Details = aRow["Details"].ToString();
                    o.officialBusiness.Purpose = aRow["Purpose"].ToString();
                    o.officialBusiness.Destination = aRow["Destination"].ToString();
                    o.officialBusiness.Status = Convert.ToInt32(aRow["Status"]);
                    o.officialBusiness.Approver = new Employee();
                    o.officialBusiness.Approver.Firstname = aRow["AFirstName"].ToString();
                    o.officialBusiness.Approver.Lastname = aRow["ALastName"].ToString();
                    o.officialBusiness.Approver.Middlename = aRow["AMiddleName"].ToString();
                    o.officialBusiness.Approver.Suffix = aRow["ASuffix"].ToString();
                    o.officialBusiness.ApproverClass = new EmployeeClassCode();
                    o.officialBusiness.ApproverClass.Description = aRow["AClass"].ToString();
                    o.officialBusiness.ApproverPosition = new PositionCode();
                    o.officialBusiness.ApproverPosition.Position = aRow["APosition"].ToString();
                    o.officialBusiness.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.officialBusiness.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OBList.Add(o);
                }
            }
            return OBList;
        }
        public static long SaveDTRLogs(TimeKeeping data)
        {
            long _logID = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;

                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_CheckIfLogsExist_DTRLogs",
                    new string[] { "eBiometricID", "eLogDate" },
                    new DbType[] { DbType.String, DbType.DateTime },
                    new object[] { data.Employee.BiometricCode, data.DTRDate }, out b, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _logID = 0;
                }
                else
                {
                    db.ExecuteCommandReader("HRIS_Save_DTRLogs",
                          new string[] { "eBiometricID", "eLogDate", "eAddedBy" },
                          new DbType[] { DbType.String, DbType.DateTime, DbType.String },
                          new object[] { data.Employee.BiometricCode, data.DTRDate, data.AddedBy }, out a, ref aTable, CommandType.StoredProcedure);
                    if (aTable.Rows.Count > 0)
                    {
                        DataRow oRow = aTable.Rows[0];
                        if (oRow["ID"] != DBNull.Value)
                        {
                            _logID = Convert.ToInt64(oRow["ID"]);
                        }
                    }
                }
            }
            return _logID;
        }
        public static long CountDTRLogsSave(Int64 LastInsertedID)
        {
            long _count = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_CountEntries_DTRLogs",
                    new string[] { "eID"},
                    new DbType[] { DbType.Int64 },
                    new object[] { LastInsertedID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _count = Convert.ToInt64(aRow["EntryCount"]);
                }
            }
            return _count;
        }

        public static void GenerateSchedules()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Timekeeping_GetAllEmployeeSchedules",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);

                foreach (DataRow oRow in oTable.Rows)
                {
                    long EmployeeID = Convert.ToInt64(oRow["EmployeeID"]);
                    long ScheduleID = Convert.ToInt64(oRow["ScheduleID"]);
                    db.ExecuteCommandNonQuery("HRIS_Timekeeping_SaveDTRSchedule",
                    new string[] { "EEmployeeID", "SScheduleID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { EmployeeID, ScheduleID }, out x, CommandType.StoredProcedure);
                }
            }
        }

        public static void ProcessDTRLog()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Timekeeping_DTRLogsForProcesing",
                                new string[] { },
                                new DbType[] { },
                                new object[] { }, out x, ref oTable, CommandType.StoredProcedure);

                foreach (DataRow oRow in oTable.Rows)
                {
                    long dTRLogID = Convert.ToInt64(oRow["ID"]);
                    long biometricID = Convert.ToInt64(oRow["BiometricID"]);
                    DateTime logDate = Convert.ToDateTime(oRow["LogDate"]);

                    DataTable dTable = new DataTable();
                    db.ExecuteCommandReader("HRIS_Timekeeping_GetDTRforUpdate",
                                    new string[] { "BiometricsCode", "LogDate" },
                                    new DbType[] {  DbType.String, DbType.Date},
                                    new object[] { biometricID.ToString(), logDate.Date }, out x, ref dTable, CommandType.StoredProcedure);

                    if (dTable.Rows.Count > 0)
                    {
                        DataRow dRow = dTable.Rows[0];
                        DateTime _timeIn = logDate;
                        DateTime _timeOut = logDate;
                        long dTRID = Convert.ToInt64(dRow["ID"]);

                        if (dRow["TimeIn"] != DBNull.Value)
                        {
                            _timeIn = Convert.ToDateTime(dRow["TimeIn"]);
                        }

                        if (dRow["TimeOut"] != DBNull.Value)
                        {
                            DateTime _tempOut = Convert.ToDateTime(dRow["TimeOut"]);
                        }

                        db.ExecuteCommandNonQuery("HRIS_Timekeeping_UpdateDTR",
                        new string[] { "DTRID", "NTimeIn", "NTimeOUT" },
                        new DbType[] { DbType.Int64, DbType.DateTime2, DbType.DateTime2 },
                        new object[] { dTRID, _timeIn, _timeOut }, out x, CommandType.StoredProcedure);

                    }

                    db.Execute(String.Format("Update hris_dtrlogs set Processed = 1 where ID = {0};", dTRLogID));
                }
            }
        }

        public static void GenerateTimeIn()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Payroll_Get_TimeIn",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }

        public static void GenerateTimeOut()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Payroll_Get_TimeOut",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }

        public static void UpdateOvertime()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Payroll_Compute_Overtime",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }

        public static void GenerateOfficialBusiness()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Payroll_Populate_Official_Business",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }

        public static void UpdateLeaves()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Payroll_Populate_Leaves",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }


        public static void TAMSImport()
        {

            DateTime nLogDate = new DateTime(2020,01,01);

            using (AppDb db = new AppDb())
            {
                db.Open();
                DataTable nTable = db.Fetch("select max(LogDate) MaxDate from hris_dtrlogs where DeviceFlag = 1");
                DataRow nRow = (nTable.Rows[0]);
                if (DateTime.TryParse(nRow["MaxDate"].ToString(), out nLogDate))
                {
                    ///TODO
                }

                using (SQLServerDB msdb = new SQLServerDB())
                {
                    msdb.Open();
                    int x = 0;
                    DataTable oTable = msdb.Fetch(String.Format("select * from DeviceLog where RecordDate > '{0}'", nLogDate.ToString("yyy-MM-dd HH:mm")));

                    foreach (DataRow oRow in oTable.Rows)
                    {
                        try
                        {
                            DateTime dLog = Convert.ToDateTime(oRow["RecordDate"]);
                            string dSerial = oRow["SerialNumber"].ToString();
                            string dBioCode = oRow["AccessNumber"].ToString();

                            DataTable cTable = db.Fetch(string.Format("select * from hris_dtrlogs where LogDate = '{0}'", dLog.ToString("yyyy-MM-dd HH:mm")));
                            if (cTable.Rows.Count == 0)
                            {
                                TimeKeeping d = new TimeKeeping();
                                d.Employee = new Employee();
                                d.Employee.BiometricCode = dBioCode;
                                d.DTRDate = dLog;

                                //TimeKeeping.SaveDTRLogs(d);

                                db.Execute(String.Format("insert into hris_dtrlogs (BiometricID, LogDate, DateAdded, DeviceID, DeviceFlag) " +
                                    "select '{0}', '{1}', now(), '{2}', 1;", dBioCode, dLog.ToString("yyyy-MM-dd HH:mm"), dSerial));

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

        public static void SentryImport()
        {

            DateTime nLogDate = new DateTime(2020, 01, 01);

            using (AppDb db = new AppDb())
            {
                db.Open();
                DataTable nTable = db.Fetch("select max(LogDate) MaxDate from hris_dtrlogs where DeviceFlag = 1");
                DataRow nRow = (nTable.Rows[0]);
                if (DateTime.TryParse(nRow["MaxDate"].ToString(), out nLogDate))
                {
                    ///TODO
                }

                using (SQLServerDB msdb = new SQLServerDB())
                {
                    msdb.Open();
                    int x = 0;
                    DataTable oTable = msdb.Fetch(String.Format("select * from TimeLogs where TimeLogStamp > '{0}'", nLogDate.ToString("yyy-MM-dd HH:mm")));

                    foreach (DataRow oRow in oTable.Rows)
                    {
                        try
                        {
                            DateTime dLog = new DateTime();
                            DateTime.TryParse(oRow["TimeLogStamp"].ToString(), out dLog);
                            string dSerial = oRow["DeviceSerialNumber"].ToString();
                            string dBioCode = oRow["AccessNumber"].ToString();

                            DataTable cTable = db.Fetch(string.Format("select * from hris_dtrlogs where LogDate = '{0}'", dLog.ToString("yyyy-MM-dd HH:mm")));
                            if (cTable.Rows.Count == 0)
                            {
                                TimeKeeping d = new TimeKeeping();
                                d.Employee = new Employee();
                                d.Employee.BiometricCode = dBioCode;
                                d.DTRDate = dLog;

                                //TimeKeeping.SaveDTRLogs(d);

                                db.Execute(String.Format("insert into hris_dtrlogs (BiometricID, LogDate, DateAdded, DeviceID, DeviceFlag) " +
                                    "select '{0}', '{1}', now(), '{2}', 1;", dBioCode, dLog.ToString("yyyy-MM-dd HH:mm"), dSerial));

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }
    }


    public class FileUploadLogs {
        public long ID { get; set; }
        public string Filename { get; set; }
        public DateTime DateUploaded { get; set; }
        public Employee Employee { get; set; }
        public long NumberOfRecordsInserted { get; set; }
        public static void SaveFilename(FileUploadLogs details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("hris_save_timekeeping_UploadedFileslog",
                    new string[] { "eFilename", "eUploadedBy", "eNumberOfRecordsInserted" },
                    new DbType[] { DbType.String, DbType.Int64,DbType.Int64 },
                    new object[] { details.Filename, details.Employee.ID,details.NumberOfRecordsInserted }, out x, CommandType.StoredProcedure);
            }
        }
        public static List<FileUploadLogs> GetFileRecords()
        {
            var _list = new List<FileUploadLogs>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("hris_Get_timekeeping_UploadedFileslog",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    FileUploadLogs f = new FileUploadLogs();
                    f.Employee = new Employee();
                    f.ID = Convert.ToInt64(oRow["ID"]);
                    f.Filename = oRow["FileName"].ToString();
                    f.Employee = Employee.GetEmployeNameByID(Convert.ToInt64(oRow["UploadedBy"]));
                    f.DateUploaded = Convert.ToDateTime(oRow["DateUploaded"]);
                    f.NumberOfRecordsInserted = Convert.ToInt64(oRow["NumberOfRecordsInserted"]);
                    _list.Add(f);
                }
            }
            return _list;
        }
    }
}
