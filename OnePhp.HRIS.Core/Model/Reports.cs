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
    public class Reports
    {
        public Employee Employee { get; set; }
        public PositionCode Position { get; set; }
        public Division Division { get; set; }
        public Department Department { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public ProjectCode Projects { get; set; }
        public EmployeePersonal Personal { get; set; }
        public EmployeeClassCode Class { get; set; }
        public EmployeeTypeCode Type { get; set; }
        public EmployeeContract Contract { get; set; }
        public List<EmployeeLicense> License { get; set; }
        public List<EmployeeEducation> Education { get; set; }
        public List<EmployeeCertifications> Certifications { get; set; }
        public List<SkillsLanguages> Skills { get; set; }
        public Assignments Assignments { get; set; }
        public RecordStatus Active { get; set; }
        public double FromSalary { get; set; }
        public string FromPosition { get; set; }
        public double ToSalary { get; set; }
        public string ToPosition { get; set; }
        public string ToJL { get; set; }
        public string FromJL { get; set; }
        public string FromDepartment { get; set; }
        public string ToDepartment { get; set; }
        public string FromSO { get; set; }
        public string ToSO { get; set; }
        public string FromAssignment { get; set; }
        public string ToAssignment { get; set; }
        public string FromDivision { get; set; }
        public string ToDivision { get; set; }
        public string VLCredit { get; set; }
        public string VLUsed { get; set; }
        public string VLAvailable { get; set; }
        public string SLCredit { get; set; }
        public string SLUsed { get; set; }
        public string SLAvailable { get; set; }
        public string BLCredit { get; set; }
        public string BLUsed { get; set; }
        public string BLAvailable { get; set; }
        public string MLCredit { get; set; }
        public string MLUsed { get; set; }
        public string MLAvailable { get; set; }
        public string PLCredit { get; set; }
        public string PLUsed { get; set; }
        public string PLAvailable { get; set; }
        public string SPLCredit { get; set; }
        public string SPLUsed { get; set; }
        public string SPLAvailable { get; set; }
        public string VAWCLredit { get; set; }
        public string VAWCLUsed { get; set; }
        public string VAWCLAvailable { get; set; }
        public string RegularManHours { get; set; }
        public string TotalOverTime { get; set; }
        public string TotalHoursLate { get; set; }
        public string TotalAbsent { get; set; }
        public decimal Tardiness { get; set; }
        public decimal TardinessFrequency { get; set; }
        public decimal HoursLate { get; set; }
        public decimal MinutesLate { get; set; }
        public List<Reports> TardinessAmount { get; set; }
        public long EmpId { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime DTRDate { get; set; }
        public List<Employee> Employees { get; set; }
        public SystemLogs Logs { get; set; }
        
        //NOTES:
        // For employee master list report, the existing Employee.Get() function is used. See ReportsController.cs/EmployeeMasterlist.
        
        //Attrition Report
        public static List<Reports> AttritionReports()
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable  aTable = new DataTable();
                db.ExecuteCommandReader("Report_Attrition",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Active = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID= aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Employee.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    r.Employee.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    r.Class.Description = aRow["Class"].ToString(); 
                    _list.Add(r);

                }
            }
                return _list;
        }
        //Attrition Report
        public static List<Reports> FilteredAttritionReports( long PosId, long ClassId, long TypeId, long DivId, long DepId)
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_Filtered_Attrition",
                    new string[] { "ePosition", "eClass", "eType", "eDivision", "eDepartment" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { PosId, ClassId, TypeId, DivId, DepId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Active = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Employee.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    r.Employee.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        //Attrition Report
        public static List<Reports> FilterByEmpIdAttritionReports(Int64 EmployeeId)
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_GetByIdOrName_Attrition",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmployeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Active = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Employee.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    r.Employee.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        //Demographics Report
        public static List<Reports> DemographicsReports()
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_Demographics",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Education = new List<EmployeeEducation>();
                    r.License = new List<EmployeeLicense>();
                    r.Contract = new EmployeeContract();
                    r.Certifications = new List<EmployeeCertifications>();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Skills = new List<SkillsLanguages>();
                    r.Personal.PCity = new City();
                    r.Personal.PProvince = new Province();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Employee.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    r.Employee.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.Personal.City.Code = aRow["City"].ToString();
                    r.Personal.PCity.Code = aRow["PCity"].ToString();
                    r.License = EmployeeLicense.GetByEmployeeID(r.Employee.ID);
                    r.Education = EmployeeEducation.GetByEmployeeID(r.Employee.ID);
                    r.Certifications = EmployeeCertifications.GetCertifications(r.Employee.ID);
                    r.Personal.Province.Code = aRow["Province"].ToString();
                    r.Personal.PProvince.Code = aRow["PProvince"].ToString();
                    r.Contract.SO.SOCode = aRow["SOCODE"].ToString();
                    r.Assignments.Description = aRow["Assignment"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.Skills = SkillsLanguages.GetSkills(r.Employee.ID);
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilteredDemographicsReports(long PosId, long ClassId, long TypeId, long DivId, long DepId)
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_Filtered_Demographics",
                  new string[] { "ePosition", "eClass", "eType", "eDivision", "eDepartment" },
                  new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64 },
                  new object[] { PosId, ClassId, TypeId, DivId, DepId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Education = new List<EmployeeEducation>();
                    r.License = new List<EmployeeLicense>();
                    r.Contract = new EmployeeContract();
                    r.Certifications = new List<EmployeeCertifications>();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Skills = new List<SkillsLanguages>();
                    r.Personal.PCity = new City();
                    r.Personal.PProvince = new Province();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Employee.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    r.Employee.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.Personal.City.Code = aRow["City"].ToString();
                    r.Personal.PCity.Code = aRow["PCity"].ToString();
                    r.License = EmployeeLicense.GetByEmployeeID(r.Employee.ID);
                    r.Education = EmployeeEducation.GetByEmployeeID(r.Employee.ID);
                    r.Certifications = EmployeeCertifications.GetCertifications(r.Employee.ID);
                    r.Personal.Province.Code = aRow["Province"].ToString();
                    r.Personal.PProvince.Code = aRow["PProvince"].ToString();
                    r.Contract.SO.SOCode = aRow["SOCODE"].ToString();
                    r.Assignments.Description = aRow["Assignment"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.Skills = SkillsLanguages.GetSkills(r.Employee.ID);
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilterByEmpIdDemographicsReports(Int64 EmployeeId)
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_GetByIdOrName_Demographics",
                  new string[] { "eEmployeeId"},
                  new DbType[] { DbType.Int64},
                  new object[] { EmployeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Education = new List<EmployeeEducation>();
                    r.License = new List<EmployeeLicense>();
                    r.Contract = new EmployeeContract();
                    r.Certifications = new List<EmployeeCertifications>();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Skills = new List<SkillsLanguages>();
                    r.Personal.PCity = new City();
                    r.Personal.PProvince = new Province();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Employee.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    r.Employee.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.Personal.City.Code = aRow["City"].ToString();
                    r.Personal.PCity.Code = aRow["PCity"].ToString();
                    r.License = EmployeeLicense.GetByEmployeeID(r.Employee.ID);
                    r.Education = EmployeeEducation.GetByEmployeeID(r.Employee.ID);
                    r.Certifications = EmployeeCertifications.GetCertifications(r.Employee.ID);
                    r.Personal.Province.Code = aRow["Province"].ToString();
                    r.Personal.PProvince.Code = aRow["PProvince"].ToString();
                    r.Contract.SO.SOCode = aRow["SOCODE"].ToString();
                    r.Assignments.Description = aRow["Assignment"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.Skills = SkillsLanguages.GetSkills(r.Employee.ID);
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> PromotionReport()
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_EmployeePromotion",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Education = new List<EmployeeEducation>();
                    r.License = new List<EmployeeLicense>();
                    r.Contract = new EmployeeContract();
                    r.Certifications = new List<EmployeeCertifications>();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Skills = new List<SkillsLanguages>();
                    r.Personal.PCity = new City();
                    r.Personal.PProvince = new Province();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID= aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.FromPosition = aRow["FromPosition"].ToString();
                    r.FromSalary = Convert.ToInt64(aRow["FromSalary"]);
                    r.ToPosition = aRow["ToPosition"].ToString();
                    r.ToSalary = Convert.ToInt64(aRow["ToSalary"]);
                    r.FromJL = aRow["FromJL"].ToString();
                    r.ToJL = aRow["ToJL"].ToString();
                    r.Contract.ApprovedDate = Convert.ToDateTime(aRow["ApprovedDate"]);


                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilteredPromotionReport(Int64 PosId, Int64 DivId, Int64 DepId)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_Filtered_EmployeePromotion",
                    new string[] { "ePosition", "eDivision","eDepartment" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { PosId, DivId, DepId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Education = new List<EmployeeEducation>();
                    r.License = new List<EmployeeLicense>();
                    r.Contract = new EmployeeContract();
                    r.Certifications = new List<EmployeeCertifications>();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Skills = new List<SkillsLanguages>();
                    r.Personal.PCity = new City();
                    r.Personal.PProvince = new Province();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.FromPosition = aRow["FromPosition"].ToString();
                    r.FromSalary = Convert.ToInt64(aRow["FromSalary"]);
                    r.ToPosition = aRow["ToPosition"].ToString();
                    r.ToSalary = Convert.ToInt64(aRow["ToSalary"]);
                    r.FromJL = aRow["FromJL"].ToString();
                    r.ToJL = aRow["ToJL"].ToString();
                    r.Contract.ApprovedDate = Convert.ToDateTime(aRow["ApprovedDate"]);


                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilterByEmpIdPromotionReport(Int64 EmployeeId)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_GetByIdOrName_EmployeePromotion",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmployeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Education = new List<EmployeeEducation>();
                    r.License = new List<EmployeeLicense>();
                    r.Contract = new EmployeeContract();
                    r.Certifications = new List<EmployeeCertifications>();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Skills = new List<SkillsLanguages>();
                    r.Personal.PCity = new City();
                    r.Personal.PProvince = new Province();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.FromPosition = aRow["FromPosition"].ToString();
                    r.FromSalary = Convert.ToInt64(aRow["FromSalary"]);
                    r.ToPosition = aRow["ToPosition"].ToString();
                    r.ToSalary = Convert.ToInt64(aRow["ToSalary"]);
                    r.FromJL = aRow["FromJL"].ToString();
                    r.ToJL = aRow["ToJL"].ToString();
                    r.Contract.ApprovedDate = Convert.ToDateTime(aRow["ApprovedDate"]);


                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> TransferReport()
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_TransferReassignment",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Contract = new EmployeeContract();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Position.Position = aRow["ToPosition"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.ToPosition = aRow["ToPosition"].ToString();
                    r.Contract.ApprovedDate = Convert.ToDateTime(aRow["ApprovedDate"]);
                    r.FromDepartment = aRow["FromDepartment"].ToString();
                    r.FromDivision = aRow["FromDivision"].ToString();
                    r.FromSO = aRow["FromSO"].ToString();
                    r.FromAssignment = aRow["FromAssignment"].ToString();
                    r.ToDepartment = aRow["ToDepartment"].ToString();
                    r.ToDivision = aRow["ToDivision"].ToString();
                    r.ToSO = aRow["ToSO"].ToString();
                    r.ToAssignment = aRow["FromAssignment"].ToString();
                    
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilterTransferReport(Int64 PosId, Int64 DivId, Int64 DepId)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_Filtered_TransferReassignment",
                    new string[] { "ePosition", "eDivision", "eDepartment" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { PosId, DivId, DepId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Contract = new EmployeeContract();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Position.Position = aRow["ToPosition"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.ToPosition = aRow["ToPosition"].ToString();
                    r.Contract.ApprovedDate = Convert.ToDateTime(aRow["ApprovedDate"]);
                    r.FromDepartment = aRow["FromDepartment"].ToString();
                    r.FromDivision = aRow["FromDivision"].ToString();
                    r.FromSO = aRow["FromSO"].ToString();
                    r.FromAssignment = aRow["FromAssignment"].ToString();
                    r.ToDepartment = aRow["ToDepartment"].ToString();
                    r.ToDivision = aRow["ToDivision"].ToString();
                    r.ToSO = aRow["ToSO"].ToString();
                    r.ToAssignment = aRow["FromAssignment"].ToString();

                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilterByEmpIdTransferReport(Int64 EmployeeId)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_GeByEdOrName_TransferReassignment",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmployeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Contract = new EmployeeContract();
                    r.Personal.City = new City();
                    r.Contract.SO = new SalesOrder();
                    r.Personal.Province = new Province();
                    r.Personal.City = new City();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    r.Position.Position = aRow["ToPosition"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.ToPosition = aRow["ToPosition"].ToString();
                    r.Contract.ApprovedDate = Convert.ToDateTime(aRow["ApprovedDate"]);
                    r.FromDepartment = aRow["FromDepartment"].ToString();
                    r.FromDivision = aRow["FromDivision"].ToString();
                    r.FromSO = aRow["FromSO"].ToString();
                    r.FromAssignment = aRow["FromAssignment"].ToString();
                    r.ToDepartment = aRow["ToDepartment"].ToString();
                    r.ToDivision = aRow["ToDivision"].ToString();
                    r.ToSO = aRow["ToSO"].ToString();
                    r.ToAssignment = aRow["FromAssignment"].ToString();

                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> LeaveUtilizationReport()
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_LeaveUtilization",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID= aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.VLCredit = aRow["VLCredit"].ToString();
                    r.VLUsed = aRow["VLUsed"].ToString();
                    r.VLAvailable = aRow["VLAvailable"].ToString();
                    r.SLCredit = aRow["SLCredit"].ToString();
                    r.SLUsed = aRow["SLUsed"].ToString();
                    r.SLAvailable = aRow["SLAvailable"].ToString();
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilteredLeaveUtilizationReport(long ClassId)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_Filtered_LeaveUtilization",
                 new string[] { "eClass"},
                 new DbType[] { DbType.Int64 },
                 new object[] { ClassId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.VLCredit = aRow["VLCredit"].ToString();
                    r.VLUsed = aRow["VLUsed"].ToString();
                    r.VLAvailable = aRow["VLAvailable"].ToString();
                    r.SLCredit = aRow["SLCredit"].ToString();
                    r.SLUsed = aRow["SLUsed"].ToString();
                    r.SLAvailable = aRow["SLAvailable"].ToString();
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> FilterByEmpIdLeaveUtilizationReport(Int64 EmployeeId)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_GetByIdOrName_LeaveUtilization",
                 new string[] { "eEmployeeId"},
                 new DbType[] { DbType.Int64 },
                 new object[] { EmployeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.VLCredit = aRow["VLCredit"].ToString();
                    r.VLUsed = aRow["VLUsed"].ToString();
                    r.VLAvailable = aRow["VLAvailable"].ToString();
                    r.SLCredit = aRow["SLCredit"].ToString();
                    r.SLUsed = aRow["SLUsed"].ToString();
                    r.SLAvailable = aRow["SLAvailable"].ToString();
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> NewHiresReports()
        {
            //every new hire has only one contract and contract is in active status.
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_NewHires",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    if (aRow["DateOfBirth"] != DBNull.Value)
                    {
                        r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    }
                    else
                    {
                        r.Personal.DateOfBirth = Convert.ToDateTime("1901/01/01");
                    }
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Class.Description = aRow["Class"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        public static List<Reports> FilteredNewHiresReports(Int64 PositionId, Int64 ClassId, DateTime StartTime, DateTime EndTime)
        {

            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Report_Filtered_NewHires",
                    new string[] {"ePosition","eClass", "eStartDate","eEndDate" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { PositionId, ClassId, StartTime, EndTime }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    if (aRow["DateOfBirth"] != DBNull.Value)
                    {
                        r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    }
                    else
                    {
                        r.Personal.DateOfBirth = Convert.ToDateTime("1901/01/01");
                    }
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Class.Description = aRow["Class"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        public static List<Reports> FilterByEmpIdNewHiresReports(Int64 EmployeeId)
        {

            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_GetByIdOrName_NewHires",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmployeeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Personal.Gender = aRow["Gender"].ToString();
                    if (aRow["DateOfBirth"] != DBNull.Value)
                    {
                        r.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    }
                    else
                    {
                        r.Personal.DateOfBirth = Convert.ToDateTime("1901/01/01");
                    }
                    r.Position.Position = aRow["Position"].ToString();
                    r.Employee.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    r.Class.Description = aRow["Class"].ToString();
                    r.Active.Description = aRow["ActiveStatus"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        public static List<Reports> ManHoursReports(Int64 PayRateId)
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("REPORTS_MANHOURS",
                    new string[] { "ePayRateID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { PayRateId  }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Active = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    //r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Position.Position = aRow["Position"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.SalesOrder.SOCode = aRow["SalesOrder"].ToString();
                    r.Assignments.Description = aRow["Assignment"].ToString();
                    //r.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    r.RegularManHours = aRow["RegularManHours"].ToString();
                    r.TotalAbsent = aRow["TotalAbsent"].ToString();
                    r.TotalHoursLate = aRow["TotalHoursLate"].ToString();
                    r.TotalOverTime = aRow["TotalOverTime"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        public static List<Reports> ManHoursFilteredReports(Int64 PayRateId, DateTime StartDate,DateTime EndDate )
        {
            //target column for attrition report is the date of separation column in employee table
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("REPORTS_Filtered_MANHOURS",
                    new string[] { "ePayRateId", "eStartDate","eEndDate" },
                    new DbType[] { DbType.Int64, DbType.DateTime, DbType.DateTime },
                    new object[] { PayRateId, StartDate, EndDate }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Department = new Department();
                    r.Division = new Division();
                    r.Position = new PositionCode();
                    r.SalesOrder = new SalesOrder();
                    r.Class = new EmployeeClassCode();
                    r.Personal = new EmployeePersonal();
                    r.Active = new RecordStatus();
                    r.Assignments = new Assignments();
                    r.Active = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    //r.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    r.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                    r.Employee.Firstname = aRow["FirstName"].ToString();
                    r.Employee.Lastname = aRow["LastName"].ToString();
                    r.Employee.Middlename = aRow["MiddleName"].ToString();
                    r.Employee.Suffix = aRow["Suffix"].ToString();
                    r.Department.Description = aRow["Department"].ToString();
                    r.Division.Div_Desc = aRow["Division"].ToString();
                    r.Position.Position = aRow["Position"].ToString();
                    r.Class.Description = aRow["Class"].ToString();
                    r.SalesOrder.SOCode = aRow["SalesOrder"].ToString();
                    r.Assignments.Description=aRow["Assignment"].ToString();
                    //r.Employee.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    r.RegularManHours = aRow["RegularManHours"].ToString();
                    r.TotalAbsent = aRow["TotalAbsent"].ToString();
                    r.TotalHoursLate = aRow["TotalHoursLate"].ToString();
                    r.TotalOverTime = aRow["TotalOverTime"].ToString();
                    _list.Add(r);

                }
            }
            return _list;
        }
        public static List<Reports> EmployeeInDepartments(Int64 eDep, Int64 eClass)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable xTable = new DataTable();
                db.ExecuteCommandReader("Reports_getEmployeeDepartments_AttendanceSummary",
                    new string[] { "eDepartment", "eEmpClass" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { eDep, eClass }, out x, ref xTable, CommandType.StoredProcedure);
                foreach (DataRow xRow in xTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Employee.ID = Convert.ToInt64(xRow["ID"]);
                    r.Employee.Firstname = xRow["Firstname"].ToString();
                    r.Employee.Lastname = xRow["Lastname"].ToString();
                    r.Employee.Middlename = xRow["Middlename"].ToString();
                    r.Employee.Suffix = xRow["Suffix"].ToString();
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> EmployeeInDivision(Int64 eDiv, Int64 eClass)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable xTable = new DataTable();
                db.ExecuteCommandReader("Reports_getEmployeeDivisions_AttendanceSummary",
                    new string[] { "edivision", "eEmpClass" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { eDiv, eClass }, out x, ref xTable, CommandType.StoredProcedure);
                foreach (DataRow xRow in xTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Employee.ID = Convert.ToInt64(xRow["ID"]);
                    r.Employee.Firstname = xRow["Firstname"].ToString();
                    r.Employee.Lastname = xRow["Lastname"].ToString();
                    r.Employee.Middlename = xRow["Middlename"].ToString();
                    r.Employee.Suffix = xRow["Suffix"].ToString();
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Department> GetDepartmentsForReports(Int64 Id)
        {
            var _list = new List<Department>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_getDepartment",
                    new string[] { "dId" },
                    new DbType[]{ DbType.Int64 },
                    new object[] {Id }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    Department d = new Department();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Code = aRow["DepCode"].ToString();
                    d.Description = aRow["Description"].ToString();
                    d.EmployeeCount = Convert.ToInt32(aRow["EmpCount"]);
                    _list.Add(d);
                }
            }
            return _list;
        }
        public static List<Division> GetDivisionsForReports(Int64 Id)
        {
            var _list = new List<Division>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_getDivision",
                    new string[] { "dId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    Division d = new Division();
                    d.ID = Convert.ToInt64(aRow["ID"]);
                    d.Div_Code = aRow["Div_Code"].ToString();
                    d.Div_Desc = aRow["Div_desc"].ToString();
                    d.Code = aRow["GroupCode"].ToString();
                    d.GroupType = aRow["GroupType"].ToString();
                    _list.Add(d);
                }
            }
            return _list;
        }
        public static List<Reports> GetTardinessDaily(Int64 month, Int32 year, Int64 dep, Int64 div, Int64 eClass)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                var _emp = new List<Employee>();
                var _tardinessList = new List<Reports>();
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_TimeKeepingSummaryPerDivisionDepartment",
                    new string[] {"rMonth", "rYear","DepartmentID", "DivisionID","EmpClass" },
                    new DbType[] { DbType.Int32, DbType.Int32, DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] {month, year, dep, div, eClass}, out x, ref oTable, CommandType.StoredProcedure) ;
                foreach (DataRow oRow in oTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Tardiness = Convert.ToDecimal(oRow["Tardiness"]) * 60;
                    r.DTRDate = Convert.ToDateTime(oRow["DTRDate"]);

                    //table headers
                    Employee e = new Employee();
                    if (oRow["EmployeeID"] != DBNull.Value)
                    {
                        r.EmpId = Convert.ToInt64(oRow["EmployeeID"]);
                        e.ID = Convert.ToInt64(oRow["EmployeeID"]);
                    }
                    else
                    {
                        e.ID = 0;
                        r.EmpId = 0;
                    }
                    e.Firstname = oRow["Firstname"].ToString();
                    e.Lastname = oRow["Lastname"].ToString();
                    e.Middlename = oRow["Middlename"].ToString();
                    e.Suffix = oRow["Suffix"].ToString();
                    _emp.Add(e);
                    _tardinessList.Add(r);
                    //filtered list
                    var _empList = new List<Employee>();
                    var _tardinessCountPerEmployee = new List<Reports>();
                    var _groupResults = _emp.GroupBy(x => x.ID).Select(y => y.First());
                    var _filteredCountOfTardiness=_tardinessList.GroupBy(x => x.EmpId).Select( y => y.First());
                    foreach (var o in _groupResults)
                    {
                        Employee ee = new Employee();
                        ee.Firstname = o.Firstname.ToString();
                        ee.Lastname = o.Lastname.ToString();
                        ee.Middlename = o.Middlename.ToString();
                        ee.Suffix = o.Suffix.ToString();
                        _empList.Add(ee);
                    }
                    var _tardinessSum = _tardinessList.GroupBy(
                        y => new { y.EmpId, y.Tardiness},
                        (key, values) => new { key.EmpId, Sum = values.Sum(y => y.Tardiness), Frequency = values.Count(w => w.Tardiness > 0) });
                    foreach (var z in _tardinessSum)
                    {
                        Reports rr = new Reports();
                        rr.HoursLate = z.Sum /60;
                        rr.MinutesLate = z.Sum;
                        rr.TardinessFrequency = z.Frequency;
                        _tardinessCountPerEmployee.Add(rr);
                    }
                    r.Employees = _empList;
                    r.TardinessAmount = _tardinessCountPerEmployee;
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<Reports> GetDTRDatesPerMonth(Int32 eMonth,Int32 eYear)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    Reports r = new Reports();
                    r.DTRDate = Convert.ToDateTime(oRow["DTRDate"]);
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static List<DateTime> GetStaticDates(int year, int month)
        {
            var dates = new List<DateTime>();

            // Loop from the first day of the month until we hit the next month, moving forward a day at a time
            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }
        public static List<Reports> SystemLogsReport(DateTime StartDate, DateTime EndDate)
        {
            var _list = new List<Reports>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_SystemLogs",
                    new string[] { "StartDate","EndDate" },
                    new DbType[] { DbType.Date, DbType.Date },
                    new object[] { StartDate, EndDate }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    Reports r = new Reports();
                    r.Employee = new Employee();
                    r.Logs = new SystemLogs();
                    r.Logs.Before = oRow["DataBefore"].ToString();
                    r.Logs.After = oRow["DataBefore"].ToString();
                    r.Logs.ModuleName = oRow["Module"].ToString();
                    r.Logs.TypeName = oRow["Type"].ToString();
                    r.Logs.Date = Convert.ToDateTime(oRow["Date"]);
                    r.Employee.EmployeeID = oRow["EmployeeID"].ToString();
                    r.Employee.Firstname = oRow["Firstname"].ToString();
                    r.Employee.Lastname = oRow["Lastname"].ToString();
                    r.Employee.Middlename = oRow["Middlename"].ToString();
                    r.Employee.Suffix = oRow["Suffix"].ToString();
                    _list.Add(r);
                }
            }
            return _list;
        }
    }
    public class Signatories
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public PositionCode Position { get; set; }
        public int Type { get; set; }
        public int isCurrentSignatory { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static string SaveNewSignatory(Signatories details)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable yTable = new DataTable();
                db.ExecuteCommandReader("hris_checkIfSignatory_signatories",
                    new string[] { "eEmployeeID", "eType" },
                    new DbType[] { DbType.Int32, DbType.Int32 },
                    new object[] { details.Employee.ID, details.Type }, out y, ref yTable, CommandType.StoredProcedure);
                if (yTable.Rows.Count > 0)
                {
                    res = "Error";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_Signatories",
                           new string[] { "eEmployeeID", "eType", "eIsCurrentSignatory", "eAddedBy", "eModifiedBy" },
                           new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                           new object[] { details.Employee.ID, details.Type, details.isCurrentSignatory, details.AddedBy, details.ModifiedBy }, out x, CommandType.StoredProcedure);
                     res = "ok";
                }
            }
            return res;
        }
        public static void EditSignatory(Signatories details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_Signatories",
                    new string[] { "eID", "eEmployeeID", "eType", "eIsCurrentSignatory", "eAddedBy", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                    new object[] { details.ID, details.Employee.ID, details.Type, details.isCurrentSignatory, details.AddedBy, details.ModifiedBy }, out x, CommandType.StoredProcedure);
            }
        }
        public static void DeleteSignatory(Signatories details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_SIGNATORIES",
                    new string[] { "eID", "eEmployeeID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { details.ID, details.Employee.ID, details.ModifiedBy }, out x, CommandType.StoredProcedure);
            }
        }
        public static List<Signatories> GetSignatories()
        {
            var _list = new List<Signatories>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GET_SIGNATORIES",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    Signatories s = new Signatories();
                    s.ID = Convert.ToInt64(oRow["ID"]);
                    s.Employee = new Employee();
                    s.Position = new PositionCode();
                    s.Employee.ID = Convert.ToInt64(oRow["EmployeeID"]);
                    s.Employee.Firstname = oRow["FirstName"].ToString();
                    s.Employee.Lastname = oRow["LastName"].ToString();
                    s.Employee.Middlename = oRow["MiddleName"].ToString();
                    s.Employee.Suffix = oRow["Suffix"].ToString();
                    s.Position = PositionCode.GetByID(Convert.ToInt64(oRow["Position"]));
                    s.Type = Convert.ToInt32(oRow["Type"]);
                    s.isCurrentSignatory = Convert.ToInt32(oRow["isCurrentSignatory"]);
                    _list.Add(s);
                }
            }
            return _list;
        }
        public static Signatories GetSignatoriesByID(Int64 Id)
        {
            var s = new Signatories();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GETBYID_SIGNATORIES",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    s.Employee = new Employee();
                    s.Position = new PositionCode();
                    s.ID = Convert.ToInt64(oRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(oRow["EmployeeID"]);
                    s.Employee.Firstname = oRow["FirstName"].ToString();
                    s.Employee.Lastname = oRow["LastName"].ToString();
                    s.Employee.Middlename = oRow["MiddleName"].ToString();
                    s.Employee.Suffix = oRow["Suffix"].ToString();
                    s.Position = PositionCode.GetByID(Convert.ToInt64(oRow["Position"]));
                    s.isCurrentSignatory = Convert.ToInt32(oRow["isCurrentSignatory"]);
                    s.Type = Convert.ToInt32(oRow["Type"]);
                }
            }
            return s;
        }
        public static Signatories GetCurrentSignatories(Int64 Type)
        {
            var s = new Signatories();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetCurrentSignatories_Signatories",
                    new string[] { "eType" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Type }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    s.Employee = new Employee();
                    s.Position = new PositionCode();
                    s.ID = Convert.ToInt64(oRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(oRow["EmployeeID"]);
                    s.Employee.Firstname = oRow["FirstName"].ToString();
                    s.Employee.Lastname = oRow["LastName"].ToString();
                    s.Employee.Middlename = oRow["MiddleName"].ToString();
                    s.Employee.Suffix = oRow["Suffix"].ToString();
                    s.Position = PositionCode.GetByID(Convert.ToInt64(oRow["Position"]));
                    s.isCurrentSignatory = Convert.ToInt32(oRow["isCurrentSignatory"]);
                    s.Type = Convert.ToInt32(oRow["Type"]);
                }
            }
            return s;
        }
        public static List<Signatories> GetSignatoriesByType(Int64 Type)
        {
            var _list = new List<Signatories>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByType_Signatories",
                   new string[] { "eType" },
                   new DbType[] { DbType.Int64 },
                   new object[] { Type }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    Signatories s = new Signatories();
                    s.Employee = new Employee();
                    s.Position = new PositionCode();
                    s.ID = Convert.ToInt64(oRow["ID"]);
                    s.Employee.ID = Convert.ToInt64(oRow["EmployeeID"]);
                    s.Employee.Firstname = oRow["FirstName"].ToString();
                    s.Employee.Lastname = oRow["LastName"].ToString();
                    s.Employee.Middlename = oRow["MiddleName"].ToString();
                    s.Employee.Suffix = oRow["Suffix"].ToString();
                    s.Position = PositionCode.GetByID(Convert.ToInt64(oRow["Position"]));
                    s.isCurrentSignatory = Convert.ToInt32(oRow["isCurrentSignatory"]);
                    s.Type = Convert.ToInt32(oRow["Type"]);
                    _list.Add(s);
                }
            }
            return _list;
        }
    }
    public class BIR2316
    {
        public decimal OverTimePay { get; set; }
        public decimal HolidayPay { get; set; }
        public decimal NightShiftDiff { get; set; }
        public decimal _13thMonthPay { get; set; }
        public decimal DeMenimisBenefits { get; set; }
        public decimal Deductions{ get; set; }
        public decimal SalariesAndOtherFormsOfCompensation { get; set; }
        public string RegisteredAddress { get; set; }
        public string LocalHomeAddress { get; set; }
        public Employee Employee {get;set;}
        public GovernmentID GovernmentID { get; set; }
        public string ZipCode { get; set; }
        public Decimal BasicSalary { get; set; }
        public decimal ECOLA { get; set; }
        public decimal DailyRate { get; set; }

        public static List<BIR2316> GetEmployeesForBIR2316Report(Int32 Year, Int32 FromMonth, Int32 ToMonth)
        {
            var _list = new List<BIR2316>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_GetEmployeesForBIR_2316",
                    new string[] { "eYear", "eFromMonth","eToMonth" },
                    new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32},
                    new object[] { Year, FromMonth, ToMonth}, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    BIR2316 b = new BIR2316();
                    b.Employee = new Employee();
                    b.Employee.ID = Convert.ToInt64(oRow["EmployeeID"]);
                    b.Employee.EmployeeID = oRow["EmpNum"].ToString();
                    b.Employee.Firstname = oRow["FirstName"].ToString();
                    b.Employee.Lastname = oRow["LastName"].ToString();
                    b.Employee.Middlename = oRow["MiddleName"].ToString();
                    b.Employee.Suffix = oRow["Suffix"].ToString();
                    _list.Add(b);
                }
            }
            return _list;
        }
        public static BIR2316 GetBIR2316OfEmployee(Int32 Year, Int32 FromMonth, Int32 ToMonth,  Int64 EmployeeID)
        {
            var b = new BIR2316();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_2316_byEmployeeID_statutory",
                    new string[] { "eYear", "eFromMonth", "eToMonth", "eEmployeeID" },
                    new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { Year, FromMonth, ToMonth, EmployeeID}, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    b.Employee = new Employee();
                    b.Employee.Firstname = oRow["FirstName"].ToString();
                    b.Employee.Lastname = oRow["LastName"].ToString();
                    b.Employee.Middlename = oRow["MiddleName"].ToString();
                    b.Employee.Suffix = oRow["Suffix"].ToString();
                    b.RegisteredAddress = oRow["RegisteredAddress"].ToString();
                    b.OverTimePay = Convert.ToDecimal(oRow["OverTime"]);
                    b.ZipCode = oRow["ZipCode"].ToString();
                    b.Employee.Personal = new EmployeePersonal();
                    b.Employee.Personal.DateOfBirth = Convert.ToDateTime(oRow["DateOfBirth"]);
                    b.Employee.Personal.MobileNumber = oRow["MobileNumber"].ToString();
                    b.GovernmentID = new GovernmentID();
                    b.GovernmentID.TIN = oRow["TINNO"].ToString().Replace("-", "");
                    b.BasicSalary = Convert.ToDecimal(oRow["BasicSalary"]);
                    b.ECOLA = Convert.ToDecimal(oRow["ECOLA"]);
                    b.Deductions = Convert.ToDecimal(oRow["Deductions"]);
                    b.HolidayPay = Convert.ToDecimal(oRow["Holiday"]);
                    b.NightShiftDiff = Convert.ToDecimal(oRow["NightDiff"]);
                    b._13thMonthPay= Convert.ToDecimal(oRow["13thMonthPay"]);
                }
            }
            return b;
        }

        
    }
    
}
