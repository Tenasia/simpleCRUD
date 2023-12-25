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
    public class StatutoryReports
    {
        public StatutoryReports()
        {
        }
        
    }
    public class SSS_R3
    {
        public long ID { get; set; }
        public CompanySetUp Company { get; set; }
        public Employee Employee { get; set; }
        public GovernmentID GovernmentID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal EmployeeShareM1{get;set;}
        public decimal EmployeeShareM2 { get; set; }
        public decimal EmployeeShareM3 { get; set; }
        public decimal EmployerShareM1 { get; set; }
        public decimal EmployerShareM2 { get; set; }
        public decimal EmployerShareM3 { get; set; }
        public int Count { get; set; }
        public List<SSS_R3> R3 { get; set; }


        public static List<SSS_R3> GetSSSR3(Int32 M1, Int32 M2, Int32 M3, Int32 Year, Int32 eOffset)
        {
            var _list = new List<SSS_R3>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_Get_R1_Statutory",
                    new string[] { "eMonth1", "eMonth2", "eMonth3", "eYear", "eOffSet" },
                    new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32 },
                    new object[] { M1, M2, M3, Year, eOffset}, out x, ref oTable, CommandType.StoredProcedure);
                int _counter = 1;
                foreach (DataRow oRow in oTable.Rows)
                {
                    SSS_R3 s = new SSS_R3();
                    s.Employee = new Employee();
                    s.GovernmentID = new GovernmentID();
                    s.Employee = Employee.GetEmployeNameByID(Convert.ToInt64(oRow["EmployeeID"]));
                    if (oRow["EEMonth1"] != DBNull.Value)
                    {
                        s.EmployeeShareM1 = Convert.ToDecimal(oRow["EEMonth1"]);
                    }
                    else
                    {
                        s.EmployeeShareM1 = 0;
                    }
                    if (oRow["EEMonth2"] != DBNull.Value)
                    {
                        s.EmployeeShareM2 = Convert.ToDecimal(oRow["EEMonth2"]);
                    }
                    else
                    {
                        s.EmployeeShareM2 = 0;
                    }
                    if (oRow["EEMonth3"] != DBNull.Value)
                    {
                        s.EmployeeShareM3 = Convert.ToDecimal(oRow["EEMonth3"]);
                    }
                    else
                    {
                        s.EmployeeShareM3 = 0;
                    }
                    if (oRow["ERMonth1"] != DBNull.Value)
                    {
                        s.EmployerShareM1 = Convert.ToDecimal(oRow["ERMonth1"]);
                    }
                    else
                    {
                        s.EmployerShareM1 = 0;
                    }

                    if (oRow["ERMonth2"] != DBNull.Value)
                    {
                        s.EmployerShareM2 = Convert.ToDecimal(oRow["ERMonth2"]);
                    }
                    else
                    {
                        s.EmployerShareM2 = 0;
                    }
                    if (oRow["ERMonth3"] != DBNull.Value)
                    {
                        s.EmployerShareM3 = Convert.ToDecimal(oRow["ERMonth3"]);
                    }
                    else
                    {
                        s.EmployerShareM3 = 0;
                    }
                    s.GovernmentID = GovernmentID.GetGovernmentIDByEmployeeID(Convert.ToInt64(oRow["EmployeeID"]));
                    s.Count = _counter;
                    _list.Add(s);
                    _counter += 1;
                }
            }
            return _list;
        }//1 page
        public static SSS_R3 GetCountSSSR3(Int32 M1, Int32 M2, Int32 M3, Int32 Year)
        {
            var s = new SSS_R3();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("Reports_getCount_R3_Statutory",
                      new string[] { "eMonth1", "eMonth2", "eMonth3", "eYear" },
                    new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32 },
                    new object[] { M1, M2, M3, Year }, out x, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {

                    DataRow oRow = aTable.Rows[0];
                    s.Count = Convert.ToInt32(oRow["NewCount"]);
                }
            }
            return s;
        }
        public static SSS_R3 GetSSS_R3Totals(Int32 M1, Int32 M2, Int32 M3, Int32 Year)
        {
            var s = new SSS_R3();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_Get_R1_Totals_Statutory",
                   new string[] { "eMonth1", "eMonth2", "eMonth3", "eYear" },
                   new DbType[] { DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int32 },
                   new object[] { M1, M2, M3, Year }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    if (oRow["EEMonth1Total"] != DBNull.Value)
                    {
                        s.EmployeeShareM1 = Convert.ToDecimal(oRow["EEMonth1Total"]);
                    }
                    else
                    {
                        s.EmployeeShareM1 = 0;
                    }
                    if (oRow["EEMonth2Total"] != DBNull.Value)
                    {
                        s.EmployeeShareM2 = Convert.ToDecimal(oRow["EEMonth2Total"]);
                    }
                    else
                    {
                        s.EmployeeShareM2 = 0;
                    }
                    if (oRow["EEMonth3Total"] != DBNull.Value)
                    {
                        s.EmployeeShareM3 = Convert.ToDecimal(oRow["EEMonth3Total"]);
                    }
                    else
                    {
                        s.EmployeeShareM3 = 0;
                    }
                    if (oRow["ERMonth1Total"] != DBNull.Value)
                    {
                        s.EmployerShareM1 = Convert.ToDecimal(oRow["ERMonth1Total"]);
                    }
                    else
                    {
                        s.EmployerShareM1 = 0;
                    }

                    if (oRow["ERMonth2Total"] != DBNull.Value)
                    {
                        s.EmployerShareM2 = Convert.ToDecimal(oRow["ERMonth2Total"]);
                    }
                    else
                    {
                        s.EmployerShareM2 = 0;
                    }
                    if (oRow["ERMonth3Total"] != DBNull.Value)
                    {
                        s.EmployerShareM3 = Convert.ToDecimal(oRow["ERMonth3Total"]);
                    }
                    else
                    {
                        s.EmployerShareM3 = 0;
                    }
                }
            }
            return s;
        }
    }
    public class PAGIBIG_MCRF
    {
        public int ID { get; set; }
        public Employee Employee{ get; set; }
        public GovernmentID GovernmentID { get; set; }
        public EmployeePersonal Personal { get; set; }
        public string AccountNumber { get; set; }
        public decimal MonthlyCompensation { get; set; }
        public decimal EmployeeShare { get; set; }
        public decimal EmployerShare { get; set; }
        public string Remarks { get; set; }

        public static List<PAGIBIG_MCRF> GetPPAGIBIGMCRF(int Month, int Year)
        {
            var _list = new List<PAGIBIG_MCRF>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_get_mcrf_Statutory",
                    new string[] { "eMonth", "eYear" },
                    new DbType[] {DbType.Int32, DbType.Int32 },
                    new object[] { Month, Year }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PAGIBIG_MCRF p = new PAGIBIG_MCRF();
                    p.Employee = new Employee();
                    p.GovernmentID = new GovernmentID();
                    p.Employee.Personal = new EmployeePersonal();
                    p.Employee = Employee.GetEmployeeDetailsByID(Convert.ToInt64(aRow["EmployeeID"]));
                    p.GovernmentID.PAGIBIG = aRow["PAGIBIG"].ToString();
                    p.GovernmentID.TIN = aRow["TIN"].ToString();
                    p.AccountNumber = aRow["AccountNumber"].ToString();
                    p.MonthlyCompensation = Convert.ToDecimal(aRow["MonthlyCompensation"]);
                    p.EmployeeShare = Convert.ToDecimal(aRow["EmployeeShare"]);
                    p.EmployerShare = Convert.ToDecimal(aRow["EmployerShare"]);
                    p.Remarks = aRow["Remarks"].ToString();
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static List<PAGIBIG_MCRF> GetPPAGIBIGMCRFMP2(int Month, int Year)
        {
            var _list = new List<PAGIBIG_MCRF>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_get_mcrf_mp2_Statutory",
                    new string[] { "eMonth", "eYear" },
                    new DbType[] { DbType.Int32, DbType.Int32 },
                    new object[] { Month, Year }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    PAGIBIG_MCRF p = new PAGIBIG_MCRF();
                    p.Employee = new Employee();
                    p.GovernmentID = new GovernmentID();
                    p.Employee.Personal = new EmployeePersonal();
                    p.Employee = Employee.GetEmployeNameByID(Convert.ToInt64(aRow["EmployeeID"]));
                    p.GovernmentID.PAGIBIG = aRow["MP2"].ToString();
                    p.AccountNumber = aRow["AccountNumber"].ToString();
                    p.MonthlyCompensation = Convert.ToDecimal(aRow["MonthlyCompensation"]);
                    p.EmployeeShare = Convert.ToDecimal(aRow["EmployeeShare"]);
                    p.EmployerShare = Convert.ToDecimal(aRow["EmployerShare"]);
                    p.GovernmentID.TIN = aRow["TIN"].ToString();
                    p.Remarks = aRow["Remarks"].ToString();
                    _list.Add(p);
                }
            }
            return _list;
        }
    }
    public class PHILHEALTH_RF1
    {
        public Employee Employee { get; set;}
        public GovernmentID GovernmentID { get; set; }
        public EmployeePersonal Personal { get; set; }
        public decimal MonthlySalaryBracket { get; set; }
        public decimal PS { get; set; }
        public decimal ES { get; set; }

        public  static List<PHILHEALTH_RF1> GetPHILHEALTH_RF1(Int32 month, Int32 year)
        {
            var _list = new List<PHILHEALTH_RF1>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Philhealth_RF1_Statutory",
                    new string[] { "eMonth","eYear" },
                    new DbType[] { DbType.Int32, DbType.Int32 },
                    new object[] { month, year }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    PHILHEALTH_RF1 p = new PHILHEALTH_RF1();
                    p.Employee = new Employee();
                    p.Employee.Personal = new EmployeePersonal();
                    p.Employee.Contract = new EmployeeContract();
                    p.Personal = new EmployeePersonal();
                    p.GovernmentID = new GovernmentID();
                    p.Employee = Employee.GetEmployeeDetailsByID(Convert.ToInt64(oRow["EmployeeID"]));
                    p.GovernmentID = GovernmentID.GetGovernmentIDByEmployeeID(Convert.ToInt64(oRow["EmployeeID"]));
                    p.Personal.Gender = oRow["Gender"].ToString();
                    p.Employee.DateOfSeparation = Convert.ToDateTime(oRow["DateOfSeparation"]);
                    p.Personal.DateOfBirth = Convert.ToDateTime(oRow["DateOfBirth"]);
                    p.MonthlySalaryBracket = Convert.ToDecimal(oRow["MonthlySalaryBracket"]);
                    p.PS = Convert.ToDecimal(oRow["PS"]);
                    p.ES = Convert.ToDecimal(oRow["ES"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
    }
}
