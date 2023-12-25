using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OnePhp.HRIS.Core.Data;
using System.Globalization;

namespace OnePhp.HRIS.Core.Model
{
    public class OverTime
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string StartTime { get; set; }
        public string EndTIme { get; set; }
        public DateTime OTDate { get; set; }
        public double Hours { get; set; }
        public string Purpose { get; set; }
        public Employee Approver { get; set; }
        public string Remarks { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Employee ApprovedBy { get; set; }
        public PositionCode ApproverPosition { get; set; }
        public EmployeeClassCode ApproverClass { get; set; }
        public string ApproverRemarks { get; set; }
        public PositionCode Position { get; set; }
        public SalesOrder SO { get; set; }
        public Assignments Assignments { get; set; }
        public int Status { get; set; }
        public ProjectCode ProjectName { get; set; }
        public DateTime DateApproved { get; set; }
        public List<ImmediateSuperior> Superior { get; set; }
        public PayPeriod PayPeriod { get; set; }
        public ImmediateSuperior MySuperiorLevel { get; set; }
        public static long SaveOverTimeDetails(OverTime data)
        {
            long _OT_Id = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Save_OT",
                    new string[] { "oEmployeeID", "oOTDate", "oStartTime", "oEndTime",
                                    "oHours", "oPurpose",  "oAddedBy", "oModifiedBy" },
                    new DbType[] { DbType.String, DbType.Date, DbType.String, DbType.String, DbType.String,
                                   DbType.Double, DbType.String,DbType.String},
                    new object[] { data.Employee.ID, data.OTDate, data.StartTime, data.EndTIme,
                                   data.Hours, data.Purpose, data.AddedBy, data.ModifiedBy}, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _OT_Id = Convert.ToInt64(aRow["ID"]);
                }
            }
            return _OT_Id;
        }
        // save Group OT
        public static string SaveGroupOT(OverTime []Details, OverTime data )
        {
            var res = "";
            using (AppDb db= new AppDb())
            {
                db.Open();
                DataTable aTable = new DataTable();
                int x = 0;
                int m = 0;
                foreach (var e in Details)
                {
                    data.Employee = new Employee();
                    data.Employee.ID = e.Employee.ID;
                    data.StartTime = e.StartTime;
                    data.EndTIme = e.EndTIme;
                    data.Hours = e.Hours;
                    data.Purpose = e.Purpose;
                    data.OTDate = e.OTDate;
                    var OTId = OverTime.SaveOverTimeDetails(data);
                    int a = 0;
                    db.ExecuteCommandNonQuery("HRIS_Approved_OT",
                                 new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                 new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                 new object[] { OTId, data.Employee.ID, data.ApprovedBy.ID, data.ApproverRemarks }, out a, CommandType.StoredProcedure);
                    
                }
                res = "ok";
            }
            return res;
        }
        // ot applicants
        public static List<OverTime> GroupOTApplicants(Int64 SupId)
        {
            var _list = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                DataTable aTable = new DataTable();
                int x = 0;
                db.ExecuteCommandReader("HRIS_ApplicantsOfGoupOvertime_OverTime",
                    new string[] { "SuperiorID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { SupId }, out x, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime e = new OverTime();
                    e.Employee = new Employee();
                    e.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.Firstname = aRow["FirstName"].ToString();
                    e.Employee.Middlename = aRow["MiddleName"].ToString();
                    e.Employee.Lastname = aRow["LastName"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    _list.Add(e);
                }
                return _list;
            }
        }
        public static OverTime isApplicationValid(Int64 EmpId)
        {
            var l = new OverTime();
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
        public static void EditOverTimeDetails(OverTime data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_OT",
                   new string[] { "oID", "oEmployeeID", "oOTDate", "oStartTime", "oEndTime",
                                    "oHours", "oPurpose",  "oModifiedBy" },
                   new DbType[] { DbType.Int64, DbType.String, DbType.Date, DbType.String,  DbType.String,
                                   DbType.Double, DbType.String, DbType.String,DbType.String},
                   new object[] { data.ID, data.Employee.ID, data.OTDate, data.StartTime, data.EndTIme,
                                   data.Hours, data.Purpose, data.ModifiedBy}, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteOverTimeDetails(OverTime data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_OT",
                      new string[] { "oID", "oEmployeeID", "oModifiedBy" },
                      new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                      new object[] { data.ID, data.Employee.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void CancelOverTimeDetails(OverTime data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Cancel_OT",
                      new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks", "oModifiedBy" },
                      new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String, DbType.String },
                      new object[] { data.ID, data.Employee.ID, data.ApprovedBy.ID, data.ApproverRemarks, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void SentOverTimeDetails(OverTime data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_sent_OT",
                      new string[] { "oID", "oEmployeeID", "oModifiedBy" },
                      new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                      new object[] { data.ID, data.Employee.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                //if (data.Employee.Class.ID == 20)
                //{
                //    data.ApproverRemarks = "SUPERVISORY OT APPLICATION";
                //    db.ExecuteCommandNonQuery("HRIS_Approved_FirstLevel_OT",
                //               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                //               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                //               new object[] { data.ID, data.Employee.ID, data.ApprovedBy.ID, data.ApproverRemarks }, out a, CommandType.StoredProcedure);
                //}
            }
        }
        public static void RejectOverTimeDetails(OverTime details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Reject_OT",
                       new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                       new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                       new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }
        public static void ApprovedOverTimeDetails(OverTime details)
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
                new object[] { details.ApprovedBy.ID, details.Employee.ID }, out a, ref aTable, CommandType.StoredProcedure);
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
                            db.ExecuteCommandNonQuery("HRIS_Approved_FirstLevel_OT",
                               new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                               new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
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
                                db.ExecuteCommandNonQuery("HRIS_SecondLevelApproved_Overtime",
                                          new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                          new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                          new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                            }
                            else
                            {
                                db.ExecuteCommandNonQuery("HRIS_Approved_OT",
                                new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
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
                            db.ExecuteCommandNonQuery("HRIS_SecondLevelApproved_Overtime",
                                      new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                      new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                      new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                        else
                        {
                            db.ExecuteCommandNonQuery("HRIS_Approved_OT",
                                     new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                     new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                     new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                        }
                    }
                    else if (_SupLevel == 3)
                    {
                        db.ExecuteCommandNonQuery("HRIS_Approved_OT",
                                     new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                                     new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                                     new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
                    }
                }
            
            }
        }
        public static void ApprovedByHR_OT(OverTime details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Approved_OT",
                            new string[] { "oID", "oEmployeeID", "oApprovedBy", "oApproverRemarks" },
                            new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.String },
                            new object[] { details.ID, details.Employee.ID, details.ApprovedBy.ID, details.ApproverRemarks }, out a, CommandType.StoredProcedure);
            }
        }
        public static OverTime GetOverTimeDetailsByID(Int64 Id)
        {
            var o = new OverTime();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_OT",
                    new string[] { "oID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
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
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Approver.Firstname = aRow["ApproverFirstname"].ToString();
                    o.Approver.Lastname = aRow["ApproverLastname"].ToString();
                    o.Approver.Middlename = aRow["ApproverMiddlename"].ToString();
                    o.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                }
            }
            return o;
        }
        public static List<OverTime> GetOverTimeDetails(Int64 Id)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_OT",
                  new string[] { "oEmployeeID" },
                  new DbType[] { DbType.Int64 },
                  new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Approver = new Employee();
                    o.Approver.Firstname = aRow["ApproverFirstname"].ToString();
                    o.Approver.Lastname = aRow["ApproverLastname"].ToString();
                    o.Approver.Middlename = aRow["ApproverMiddlename"].ToString();
                    o.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetOverTimeDetailsWithResults(Int64 Id)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetApplicationResults_OT",
                  new string[] { "eEmployeeID" },
                  new DbType[] { DbType.Int64 },
                  new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Approver = new Employee();
                    o.Approver.Firstname = aRow["ApproverFirstname"].ToString();
                    o.Approver.Lastname = aRow["ApproverLastname"].ToString();
                    o.Approver.Middlename = aRow["ApproverMiddlename"].ToString();
                    o.Approver.Suffix = aRow["ApproverSuffix"].ToString();
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static void SetStatusToHasApplicationResult(Int64 Id)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetToSentApplicationResults_OT",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, CommandType.StoredProcedure);
            }
        }
        public static List<OverTime> GetOverTimeDetailsForFirstLevelApprovalDepartment(Int64 DepId, Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDepartmentOTForApproval_FirstLevel_OverTime",
                  new string[] { "eDepartmentID", "eSuperiorId" },
                  new DbType[] { DbType.Int64, DbType.Int64 },
                  new object[] { DepId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.ApprovedBy.Position = new PositionCode();
                    o.ApproverPosition = new PositionCode();
                    o.Position = new PositionCode();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    //o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    //o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    //o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    //o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    //o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    //o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetAllOvertimeForApproval(Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetOT_ForApproval",
                  new string[] { "eSuperiorId" },
                  new DbType[] { DbType.Int64},
                  new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApprovedBy = new Employee();
                    o.ApproverPosition = new PositionCode();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.MySuperiorLevel = new ImmediateSuperior();
                    o.Position = new PositionCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.MySuperiorLevel.Level = Convert.ToInt32(aRow["Level"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static OverTime GetByIDForApproval(Int64 Id)
        {
            var o = new OverTime();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetForApprovalById_OT",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.Position = new PositionCode();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApproverRemarks = aRow["ApproverRemarks"].ToString();
                }
            }
            return o;
        }
        public static List<OverTime> GetOverTimeDetailsForSecondLevelApprovalDepartment(Int64 DepId, Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDepartmentOTForApproval_SecondLevel_OverTime",
                  new string[] { "eDepartmentID", "eSuperiorId" },
                  new DbType[] { DbType.Int64, DbType.Int64 },
                  new object[] { DepId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApprovedBy = new Employee();
                    o.ApproverPosition = new PositionCode();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.Position = new PositionCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetOverTimeDetailsForThirdLevelApproval(Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDepartmentOTForApproval_ThirdLevel_OverTime",
                  new string[] { "eSuperiorId" },
                  new DbType[] {  DbType.Int64 },
                  new object[] {  SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApprovedBy = new Employee();
                    o.ApproverPosition = new PositionCode();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.Position = new PositionCode();

                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetOverTimeDetailsForHRApproval()
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_ForApproval_HR_OT",
                  new string[] { },
                  new DbType[] { },
                  new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApprovedBy = new Employee();
                    o.ApproverPosition = new PositionCode();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.Position = new PositionCode();
                    o.Employee.Department = new Department();
                    o.Employee.Division = new Division();
                    o.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    o.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.Superior = new List<ImmediateSuperior>();
                    o.Superior = ImmediateSuperior.GetAssignedSuperiors(o.Employee.ID);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetFilteredOverTimeDetailsForHRApproval(Int64 Div, Int64 Dep, Int64 Pos, Int64 EmpId, DateTime StartDate, DateTime EndDate)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_ForApprovalFiltered_HR_OT",
                  new string[] { "eDivision", "eDepartment", "ePosition", "eEmployeeId", "eStartDate", "eEndDate" },
                  new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                  new object[] { Div, Dep, Pos, EmpId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApprovedBy = new Employee();
                    o.ApproverPosition = new PositionCode();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.Position = new PositionCode();
                    o.Employee.Department = new Department();
                    o.Employee.Division = new Division();
                    o.Employee.Department = Department.GetByID(Convert.ToInt64(aRow["Department"]));
                    o.Employee.Division = Division.GetByID(Convert.ToInt64(aRow["Division"]));
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.Superior = new List<ImmediateSuperior>();
                    o.Superior = ImmediateSuperior.GetAssignedSuperiors(o.Employee.ID);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetOverTimeDetailsForFirstLevelApprovalDivision(Int64 DivId, Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDivisionOTForApproval_SecondLevel_OverTime",
                  new string[] { "eDivisionID", "eSuperiorId" },
                  new DbType[] { DbType.Int64, DbType.Int64 },
                  new object[] { DivId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.ApprovedBy = new Employee();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.ApproverPosition = new PositionCode();
                    o.ApprovedBy.Position = new PositionCode();
                    o.Position = new PositionCode();
                    o.SO = new SalesOrder();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static List<OverTime> GetOverTimeDetailsForSecondLevelApprovalDivision(Int64 DivId, Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDivisionOTForApproval_SecondLevel_OverTime",
                  new string[] { "eDivisionID", "eSuperiorId" },
                  new DbType[] { DbType.Int64, DbType.Int64 },
                  new object[] { DivId, SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApproverPosition = new PositionCode();
                    o.Position = new PositionCode();
                    o.ApprovedBy = new Employee();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        //public static OverTime GetApprover(Int64 Id)
        //{
        //    var o = new OverTime();
        //    using (AppDb db = new AppDb())
        //    {
        //        db.Open();
        //        int a = 0;
        //        DataTable aTable = new DataTable();
        //        db.ExecuteCommandReader("HRIS_GetApprover_OT",
        //         new string[] { "oEmployeeID" },
        //         new DbType[] { DbType.Int64},
        //         new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
        //        if (aTable.Rows.Count > 0)
        //        {
        //            DataRow aRow = aTable.Rows[0];
        //            o.Hours = Convert.ToDouble(aRow["Hours"]);
        //            o.Approver.Firstname = aRow["Firstname"].ToString();
        //            o.Approver.Lastname = aRow["Lastname"].ToString();
        //            o.Approver.Middlename = aRow["Middlename"].ToString();
        //            o.Approver.Suffix = aRow["Suffix"].ToString();
        //        }
        //    }
        //    return o;
        //}
        public static List<OverTime> GetToBeSentOvertime(Int64 SupId)
        {
            var OTList = new List<OverTime>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetToBeSent_OverTime",
                  new string[] { "eSuperiorId" },
                  new DbType[] { DbType.Int64 },
                  new object[] { SupId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    OverTime o = new OverTime();
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
                    o.Assignments = new Assignments();
                    o.ProjectName = new ProjectCode();
                    o.SO = new SalesOrder();
                    o.ApproverPosition = new PositionCode();
                    o.Position = new PositionCode();
                    o.ApprovedBy = new Employee();
                    o.ApprovedBy.Class = new EmployeeClassCode();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmpId"]);
                    o.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    o.Employee.Firstname = aRow["FirstName"].ToString();
                    o.Employee.Middlename = aRow["MiddleName"].ToString();
                    o.Employee.Lastname = aRow["LastName"].ToString();
                    o.Employee.Suffix = aRow["Suffix"].ToString();
                    o.OTDate = Convert.ToDateTime(aRow["OTDate"]);
                    o.StartTime = _stime12HrFormat;
                    o.EndTIme = _etime12HrFormat;
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Hours = Convert.ToDouble(aRow["Hours"]);
                    o.Purpose = aRow["Purpose"].ToString();
                    o.Remarks = aRow["Remarks"].ToString();
                    o.Status = Convert.ToInt32(aRow["Status"]);
                    o.ProjectName.ProjectName = aRow["ProjectName"].ToString();
                    o.SO.SOCode = aRow["SalesOrder"].ToString();
                    o.Assignments.Description = aRow["Assignment"].ToString();
                    o.Position.Position = aRow["Position"].ToString();
                    o.ApprovedBy.Firstname = aRow["fFirstname"].ToString();
                    o.ApprovedBy.Lastname = aRow["fLastname"].ToString();
                    o.ApprovedBy.Middlename = aRow["fMiddlename"].ToString();
                    o.ApprovedBy.Suffix = aRow["fSuffix"].ToString();
                    o.ApprovedBy.Class.Description = aRow["faClassification"].ToString();
                    o.ApproverPosition.Position = aRow["faPosition"].ToString();
                    o.DateApproved = Convert.ToDateTime(aRow["DateApproved"]);
                    OTList.Add(o);
                }
            }
            return OTList;
        }
        public static void SetToSentOverTimeNotification(Int64 Id)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetStatusToEmailSent_OverTime",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, CommandType.StoredProcedure);
            }
        }
    }
}
