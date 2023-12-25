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
    public class BranchManagement
    {
        public BranchManagement()
        {
        }

    }

    public class Branches
    {
        public long ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public CompanyInfo Company { get; set; }
        public Region Region { get; set; }
        public Province Province { get; set; }
        public City City { get; set; }
        public BARANGAY Barangay { get; set; }
        public BrandName Brand { get; set; }
        public string Street { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static void SaveBranch(Branches data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("hris_save_branch",
                 new string[]{ "ecompanyid", "eBrandName", "ebranchcode", "ebranchname", "eregion",
                     "eprovince", "ecity", "ebarangay", "estreet", "eaddedby", "emodifiedby"},
                 new DbType[] { DbType.Int64,DbType.Int64, DbType.String, DbType.String, DbType.Int64,
                 DbType.Int64,DbType.Int64,DbType.Int64,DbType.String,DbType.String,DbType.String},
                 new object[] { data.Company.ID, data.Brand.ID, data.BranchCode, data.BranchName, data.Region.ID, data.Province.ID, data.City.ID, data.Barangay.ID, data.Street, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);

            }
        }
        public static void EditBranch(Branches data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("hris_edit_branch",
               new string[]{ "eId","ecompanyid", "eBrandName", "ebranchcode", "ebranchname", "eregion",
                     "eprovince", "ecity", "ebarangay", "estreet",  "emodifiedby"},
               new DbType[] { DbType.Int64,  DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.Int64,
                 DbType.Int64,DbType.Int64,DbType.Int64,DbType.String,DbType.String},
               new object[] { data.ID, data.Company.ID, data.Brand.ID, data.BranchCode, data.BranchName, data.Region.ID, data.Province.ID, data.City.ID, data.Barangay.ID, data.Street, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static void DeleteBranch(Branches data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("hris_delete_branch",
               new string[] { "eId", "emodifiedby" },
               new DbType[] { DbType.Int64, DbType.String },
               new object[] { data.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<Branches> GetBranches(Int64 CompanyId)
        {
            var _list = new List<Branches>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_get_branch",
                    new string[] { "aCompanyId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { CompanyId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Branches b = new Branches();
                    b.Company = new CompanyInfo();
                    b.Barangay = new BARANGAY();
                    b.Province = new Province();
                    b.Region = new Region();
                    b.City = new City();
                    b.Brand = new BrandName();
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.Company.ID = Convert.ToInt64(aRow["CompanyID"]);
                    b.Company.Name = aRow["CompanyName"].ToString();
                    b.BranchCode = aRow["BranchCode"].ToString();
                    b.BranchName = aRow["BranchName"].ToString();
                    b.Province.ID = Convert.ToInt64(aRow["Province"]);
                    b.Region.ID = Convert.ToInt64(aRow["Region"]);
                    b.City.ID = Convert.ToInt64(aRow["City"]);
                    b.Barangay.ID = Convert.ToInt64(aRow["Barangay"]);
                    b.Street = aRow["Street"].ToString();
                    b.Region.Name = aRow["RegionName"].ToString();
                    b.City.Name = aRow["CityName"].ToString();
                    b.Province.Name = aRow["ProvName"].ToString();
                    b.Barangay.Name = aRow["BrgyName"].ToString();
                    b.Brand = BrandName.GetById(Convert.ToInt64(aRow["BrandName"]));
                    _list.Add(b);
                }
            }
            return _list;
        }
        public static Branches GetBranchesById(Int64 Id)
        {
            var b = new Branches();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_getById_branch",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    b.Company = new CompanyInfo();
                    b.Barangay = new BARANGAY();
                    b.Province = new Province();
                    b.Region = new Region();
                    b.City = new City();
                    b.Brand = new BrandName();
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.Company.ID = Convert.ToInt64(aRow["CompanyID"]);
                    b.Company.Name = aRow["CompanyName"].ToString();
                    b.BranchCode = aRow["BranchCode"].ToString();
                    b.BranchName = aRow["BranchName"].ToString();
                    b.Province.ID = Convert.ToInt64(aRow["Province"]);
                    b.Region.ID = Convert.ToInt64(aRow["Region"]);
                    b.City.ID = Convert.ToInt64(aRow["City"]);
                    b.Barangay.ID = Convert.ToInt64(aRow["Barangay"]);
                    b.Street = aRow["Street"].ToString();
                    b.Region.Name = aRow["RegionName"].ToString();
                    b.City.Name = aRow["CityName"].ToString();
                    b.Province.Name = aRow["ProvName"].ToString();
                    b.Barangay.Name = aRow["BrgyName"].ToString();
                    b.Brand = BrandName.GetById(Convert.ToInt64(aRow["BrandName"]));
                }
            }
            return b;
        }
    }
    public class BranchEmployees
    {
        public long ID { get; set; }
        public Branches Branch { get; set; }
        public Employee Employee { get; set; }
        public DateTime DateAssigned { get; set; }
        public Employee AssignedBy { get; set; }
        public DateTime DateEnd { get; set; }
        public CompanyInfo Company { get; set; }
        public List<BranchEmployees> History { get; set; }
        public ProjectWorkSchedule BranchSchedule { get; set; }
        public int Status { get; set; }

        public static void SaveBranchEmployees(BranchEmployees[] details, BranchEmployees sub)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                foreach (BranchEmployees data in details)
                {
                    data.AssignedBy = new Employee();
                    data.DateAssigned = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                    data.AssignedBy.ID = sub.AssignedBy.ID;
                    db.ExecuteCommandNonQuery("hris_save_branch_employees",
                     new string[] { "eBranchId", "eEmployeeId", "eDateAssigned", "eAssignedBy" },
                     new DbType[] { DbType.Int64, DbType.Int64, DbType.DateTime, DbType.Int64 },
                     new object[] { data.Branch.ID, data.Employee.ID, data.DateAssigned, data.AssignedBy.ID }, out a, CommandType.StoredProcedure);
                }
            }
        }
        public static void EditBranchEmployees(BranchEmployees data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                db.ExecuteCommandNonQuery("hris_unassigned_branch_employees",
                   new string[] { "eId", "eEmployeeId" },
                   new DbType[] { DbType.Int64, DbType.Int64 },
                   new object[] { data.ID, data.Employee.ID }, out a, CommandType.StoredProcedure);

                db.ExecuteCommandNonQuery("hris_save_branch_employees",
                     new string[] { "eBranchId", "eEmployeeId", "eDateAssigned", "eAssignedBy" },
                     new DbType[] { DbType.Int64, DbType.Int64, DbType.DateTime, DbType.Int64 },
                     new object[] { data.Branch.ID, data.Employee.ID, data.DateAssigned, data.AssignedBy.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static void UnAssignedBranchEmployees(BranchEmployees data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("hris_unassigned_branch_employees",
                    new string[] { "eId", "eEmployeeId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { data.ID, data.Employee.ID }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<BranchEmployees> GetAssignedEmployees(Int64 BranchId)
        {
            var _list = new List<BranchEmployees>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_GetAssigned_branch_employees",
                    new string[] { "eBranchId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { BranchId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    _list.Add(e);
                }
            }
            return _list;
        }
        public static BranchEmployees GetAssignedEmployeesWithNextWeekScheduleByEmpId(Int64 EmpId)
        {
            var e = new BranchEmployees();
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

                db.ExecuteCommandReader("hris_GetAssigned_branch_employeesid_withSchedule",
                    new string[] { "eEmployeeId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { EmpId, startDateOfNextWeek, endDateOfNextWeek }, out a, ref aTable, CommandType.StoredProcedure);
               if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    
                }
            }
            return e;
        }
        
        public static BranchEmployees GetAssignedEmployeesWithCurrentWeekScheduleByEmpId(Int64 EmpId)
        {
            var e = new BranchEmployees();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                DateTime today = DateTime.Today;
                DayOfWeek currentDayOfWeek = today.DayOfWeek;

                // Calculate the number of days until the previous Monday (start of current week)
                int daysUntilPreviousMonday = ((int)currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

                // Calculate the start date of the current week
                DateTime startDateOfCurrentWeek = today.AddDays(-daysUntilPreviousMonday);

                // Calculate the end date of the current week
                DateTime endDateOfCurrentWeek = startDateOfCurrentWeek.AddDays(6);

                db.ExecuteCommandReader("hris_GetAssigned_branch_employeesid_withSchedule",
                    new string[] { "eEmployeeid", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { EmpId, startDateOfCurrentWeek, endDateOfCurrentWeek }, out a, ref aTable, CommandType.StoredProcedure);
                if ( aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.BranchSchedule = new ProjectWorkSchedule();
                    e.BranchSchedule.Start = Convert.ToDateTime(aRow["Start"]);
                    e.BranchSchedule.End = Convert.ToDateTime(aRow["End"]);
                    e.BranchSchedule.Schedule = new WorkSchedule();
                    e.BranchSchedule.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));
                  
                }
            }
            return e;
        }
        public static BranchEmployees GetAssignedEmployeesWithScheduleId(Int64 eId)
        {
            var e = new BranchEmployees();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_GetAssigned_branch_Byid_withSchedule",
                    new string[] { "eid"},
                    new DbType[] { DbType.Int64 },
                    new object[] { eId }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.ID = Convert.ToInt64(aRow["BranchID"]);
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.BranchSchedule = new ProjectWorkSchedule();
                    e.BranchSchedule.ID = Convert.ToInt64(aRow["BranchSchedule"]);
                    e.BranchSchedule.Start = Convert.ToDateTime(aRow["Start"]);
                    e.BranchSchedule.End = Convert.ToDateTime(aRow["End"]);
                    e.BranchSchedule.Schedule = new WorkSchedule();
                    e.BranchSchedule.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));

                }
            }
            return e;
        }
        public static List<BranchEmployees> GetAssignedEmployeesWithNextWeekSchedule(Int64 BranchId, Int64 CompanyId)
        {
            var _list = new List<BranchEmployees>();
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

                db.ExecuteCommandReader("hris_GetAssigned_branch_employees_withSchedule",
                    new string[] { "eBranchId", "eCompanyId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { BranchId, CompanyId, startDateOfNextWeek, endDateOfNextWeek }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.BranchSchedule = new ProjectWorkSchedule();
                    e.BranchSchedule.Start = Convert.ToDateTime(aRow["Start"]);
                    e.BranchSchedule.End = Convert.ToDateTime(aRow["End"]);
                    e.BranchSchedule.Schedule = new WorkSchedule();
                    e.BranchSchedule.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));
                    e.BranchSchedule.ID = Convert.ToInt64(aRow["BranchSchedule"]);
                    _list.Add(e);
                }
            }
            return _list;
        }
        public static List<BranchEmployees> GetAssignedEmployeesWithCurrentWeekSchedule(Int64 BranchId, Int64 CompanyId)
        {
            var _list = new List<BranchEmployees>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                DateTime today = DateTime.Today;
                DayOfWeek currentDayOfWeek = today.DayOfWeek;

                // Calculate the number of days until the previous Monday (start of current week)
                int daysUntilPreviousMonday = ((int)currentDayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

                // Calculate the start date of the current week
                DateTime startDateOfCurrentWeek = today.AddDays(-daysUntilPreviousMonday);

                // Calculate the end date of the current week
                DateTime endDateOfCurrentWeek = startDateOfCurrentWeek.AddDays(6);

                db.ExecuteCommandReader("hris_GetAssigned_branch_employees_withSchedule",
                    new string[] { "eBranchId","eCompanyId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { BranchId,CompanyId, startDateOfCurrentWeek, endDateOfCurrentWeek }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.BranchSchedule = new ProjectWorkSchedule();
                    e.BranchSchedule.Start = Convert.ToDateTime(aRow["Start"]);
                    e.BranchSchedule.End = Convert.ToDateTime(aRow["End"]);
                    e.BranchSchedule.Schedule = new WorkSchedule();
                    e.BranchSchedule.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));
                    _list.Add(e);
                }
            }
            return _list;
        }
        public static List<BranchEmployees> GetAssignedEmployeesSchedule(Int64 BranchId)
        {
            var _list = new List<BranchEmployees>();
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

                db.ExecuteCommandReader("hris_GetAssigned_branch_employees_schedule",
                    new string[] { "eBranchId", "eStartDate", "eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Date, DbType.Date },
                    new object[] { BranchId, startDateOfNextWeek, endDateOfNextWeek }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    //e.BranchSchedule = new ProjectWorkSchedule();
                    //e.BranchSchedule.Start = Convert.ToDateTime(aRow["Start"]);
                    //e.BranchSchedule.End = Convert.ToDateTime(aRow["End"]);
                    //e.BranchSchedule.Schedule = new WorkSchedule();
                    //e.BranchSchedule.Schedule = WorkSchedule.GetWorkScheduleByID(Convert.ToInt64(aRow["ScheduleID"]));
                    _list.Add(e);
                }
            }
            return _list;
        }
        public static List<BranchEmployees> GetAssignedEmployeesByEmpdId(Int64 EmpId)
        {
            var _list = new List<BranchEmployees>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_GetAssigned_branch_per_employee",
                    new string[] { "eEmpId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    _list.Add(e);
                }
            }
            return _list;
        }
        public static List<BranchEmployees> GetAssignmentHistoryByEmpdId(Int64 EmpId)
        {
            var _list = new List<BranchEmployees>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_GetAssignmentHistory_branch_per_employee",
                    new string[] { "eEmpId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.Status = Convert.ToInt32(aRow["Status"]);
                    _list.Add(e);
                }
            }
            return _list;
        }
        public static BranchEmployees GetById(Int64 Id)
        {
            var e = new BranchEmployees();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_GetByIdAssigned_branch_employee",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Branch = new Branches();
                    e.Branch.Province = new Province();
                    e.Branch.City = new City();
                    e.Branch.Barangay = new BARANGAY();
                    e.Branch.Region = new Region();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmpID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Branch.Street = aRow["Street"].ToString();
                    e.Branch.BranchCode = aRow["BranchCode"].ToString();
                    e.Branch.BranchName = aRow["BranchName"].ToString();
                    e.Branch.Barangay.ID = Convert.ToInt64(aRow["ID"]);
                    e.Branch.Barangay.Name = aRow["BrgyName"].ToString();
                    e.Branch.City.ID = Convert.ToInt64(aRow["City"]);
                    e.Branch.City.Name = aRow["CityName"].ToString();
                    e.Branch.Region.ID = Convert.ToInt64(aRow["Region"]);
                    e.Branch.Region.Name = aRow["RegionName"].ToString();
                    e.Branch.Province.ID = Convert.ToInt64(aRow["Province"]);
                    e.Branch.Province.Name = aRow["ProvName"].ToString();
                    e.Company = new CompanyInfo();
                    e.Company.ID = Convert.ToInt64(aRow["CompanyId"]);
                    e.Company.Name = aRow["CompanyName"].ToString();
                    e.History = new List<BranchEmployees>();
                    e.History = BranchEmployees.GetAssignmentHistoryByEmpdId(e.Employee.ID);
                }
            }
            return e;
        }
        public static List<BranchEmployees> GetUnassigned(Int64 CompanyId)
        {
            var _list = new List<BranchEmployees>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("hris_GetUnAssigned_branch_employees",
                    new string[] { "CompanyId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { CompanyId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BranchEmployees e = new BranchEmployees();
                    e.Employee = new Employee();
                    e.Employee.Company = new CompanyInfo();
                    e.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.EmployeeID = aRow["EmployeeId"].ToString();
                    e.Employee.Firstname = aRow["Firstname"].ToString();
                    e.Employee.Middlename = aRow["Middlename"].ToString();
                    e.Employee.Lastname = aRow["Lastname"].ToString();
                    e.Employee.Suffix = aRow["Suffix"].ToString();
                    e.Employee.Company.ID = Convert.ToInt64(aRow["Company"]);
                    e.Employee.Company.Name = aRow["CompanyName"].ToString();
                    _list.Add(e);
                }
            }
            return _list;
        }
    }
}
