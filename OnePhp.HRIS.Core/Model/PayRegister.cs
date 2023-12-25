using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OnePhp.HRIS.Core.Data;

namespace OnePhp.HRIS.Core.Model
{
    public class PayRegister
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public EmployeeContract Contract { get; set; }
        public decimal BasicPay { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal Overtime { get; set; }
        public decimal OvertimeAmount { get; set; }
        public decimal NightDiff { get; set; }
        public decimal NightDiffAmount { get; set; }
        public decimal NightDiffOT { get; set; }
        public decimal NightDiffOTAmount { get; set; }
        public decimal RegularHoliday { get; set; }
        public decimal RegularHolidayAmount { get; set; }
        public decimal RegularHolidayOT { get; set; }
        public decimal RegularHolidayOTAmount { get; set; }
        public decimal RegularHolidayNightDiff { get; set; }
        public decimal RegularHolidayNightDiffAmount { get; set; }
        public decimal RegularHolidayNightDiffOT { get; set; }
        public decimal RegularHolidayNightDiffOTAmount { get; set; }
        public decimal SpecialHoliday { get; set; }
        public decimal SpecialHolidayAmount { get; set; }
        public decimal SpecialHolidayOT { get; set; }
        public decimal SpecialHolidayOTAmount { get; set; }
        public decimal SpecialHolidayNightDiff { get; set; }
        public decimal SpecialHolidayNightDiffAmount { get; set; }
        public decimal SpecialHolidayNightDiffOT { get; set; }
        public decimal SpecialHolidayNightDiffOTAmount { get; set; }
        public decimal WDO { get; set; }
        public decimal WDOAmount { get; set; }
        public decimal WDOOT { get; set; }
        public decimal WDOOTAmount { get; set; }
        public decimal WDONightDiff { get; set; }
        public decimal WDONightDiffAmount { get; set; }
        public decimal WDONightDiffOT { get; set; }
        public decimal WDONightDiffOTAmount { get; set; }
        public decimal WDORegularHoliday { get; set; }
        public decimal WDORegularHolidayAmount { get; set; }
        public decimal WDORegularHolidayOT { get; set; }
        public decimal WDORegularHolidayOTAmount { get; set; }
        public decimal WDORegularHolidayNightDiff { get; set; }
        public decimal WDORegularHolidayNightDiffAmount { get; set; }
        public decimal WDORegularHolidayNightDiffOT { get; set; }
        public decimal WDORegularHolidayNightDiffOTAmount { get; set; }
        public decimal WDOSpecialHoliday { get; set; }
        public decimal WDOSpecialHolidayAmount { get; set; }
        public decimal WDOSpecialHolidayOT { get; set; }
        public decimal WDOSpecialHolidayOTAmount { get; set; }
        public decimal WDOSpecialHolidayNightDiff { get; set; }
        public decimal WDOSpecialHolidayNightDiffAmount { get; set; }
        public decimal WDOSpecialHolidayNightDiffOT { get; set; }
        public decimal WDOSpecialHolidayNightDiffOTAmount { get; set; }
        public decimal VLDays { get; set; }
        public decimal VLAmount { get; set; }
        public decimal SLDays { get; set; }
        public decimal SLAmount { get; set; }
        public decimal BLDays { get; set; }
        public decimal BLAmount { get; set; }
        public decimal ELDays { get; set; }
        public decimal ELAmount { get; set; }
        public decimal Meals { get; set; }
        public decimal TaxableAllowance { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal AbsentAmount { get; set; }
        public decimal TardyDays { get; set; }
        public decimal TardyAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SSS { get; set; }
        public decimal PHILHEALTH { get; set; }
        public decimal PAGIBIG { get; set; }
        public decimal ECOLA { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal NetIncome { get; set; }
        public decimal OverDeductSSSLoan { get; set; }
        public decimal OverDeductPagibigLoan { get; set; }
        public decimal VoluntaryContribution { get; set; }
        public TaxCode TaxData { get; set; }
        public decimal Canteen { get; set; }
        public decimal RCBCLoan { get; set; }
        public decimal SSSLoan { get; set; }
        public decimal PAGIBIGLoan { get; set; }
        public decimal Rate { get; set; }
        public decimal PesonalAccounts { get; set; }// this is the sum of all the deductions only, additions are excluded
        
        public static List<PayRegister> GetPayRegisterReport(Int64 FrequencyID, Int64 Year, Int64 PayPeriod, Int32 Month)
        {
            var _list = new List<PayRegister>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_PayRegister",
                    new string[] { "eFrequencyID","eYear", "ePayPeriod","eMonth" },
                    new DbType[] {DbType.Int64,DbType.Int32, DbType.Int64, DbType.Int32 },
                    new object[] { FrequencyID, Year, PayPeriod, Month }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    PayRegister r = new PayRegister();
                    r.Employee = new Employee();
                    r.ID = Convert.ToInt64(oRow["ID"]);
                    r.Rate = Convert.ToDecimal(oRow["Rate"]);
                    r.Employee = Employee.GetEmployeNameByID(Convert.ToInt64(oRow["EmployeeID"]));
                    r.BasicPay = Convert.ToDecimal(oRow["BasicPay"]);
                    r.HoursWorked = Convert.ToDecimal(oRow["HoursWorked"]);
                    r.Overtime = Convert.ToDecimal(oRow["Overtime"]);
                    r.OvertimeAmount = Convert.ToDecimal(oRow["OvertimeAmount"]);
                    r.NightDiff = Convert.ToDecimal(oRow["NightDiff"]);
                    r.NightDiffAmount = Convert.ToDecimal(oRow["NightDiffAmount"]);
                    r.NightDiffOT = Convert.ToDecimal(oRow["NightDiffOT"]);
                    r.NightDiffOTAmount = Convert.ToDecimal(oRow["NightDiffOTAmount"]);
                    r.RegularHoliday = Convert.ToDecimal(oRow["RegularHoliday"]);
                    r.RegularHolidayAmount = Convert.ToDecimal(oRow["RegularHolidayAmount"]);
                    r.RegularHolidayOT = Convert.ToDecimal(oRow["RegularHolidayOT"]);
                    r.RegularHolidayOTAmount = Convert.ToDecimal(oRow["RegularHolidayOTAmount"]);
                    r.RegularHolidayNightDiff = Convert.ToDecimal(oRow["RegularHolidayNightDiff"]);
                    r.RegularHolidayNightDiffAmount = Convert.ToDecimal(oRow["RegularHolidayNightDiffAmount"]);
                    r.RegularHolidayNightDiffOT = Convert.ToDecimal(oRow["RegularHolidayNightDiffOT"]);
                    r.RegularHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["RegularHolidayNightDiffOTAmount"]);
                    r.SpecialHoliday = Convert.ToDecimal(oRow["SpecialHoliday"]);
                    r.SpecialHolidayAmount = Convert.ToDecimal(oRow["SpecialHolidayAmount"]);
                    r.SpecialHolidayOT = Convert.ToDecimal(oRow["SpecialHolidayOT"]);
                    r.SpecialHolidayOTAmount = Convert.ToDecimal(oRow["SpecialHolidayOTAmount"]);
                    r.SpecialHolidayNightDiff = Convert.ToDecimal(oRow["SpecialHolidayNightDiff"]);
                    r.SpecialHolidayNightDiffAmount = Convert.ToDecimal(oRow["SpecialHolidayNightDiffAmount"]);
                    r.SpecialHolidayNightDiffOT = Convert.ToDecimal(oRow["SpecialHolidayNightDiffOT"]);
                    r.SpecialHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["SpecialHolidayNightDiffOTAmount"]);
                    r.WDO = Convert.ToDecimal(oRow["WDO"]);
                    r.WDOAmount = Convert.ToDecimal(oRow["WDOAmount"]);
                    r.WDOOT = Convert.ToDecimal(oRow["WDOOT"]);
                    r.WDOOTAmount = Convert.ToDecimal(oRow["WDOOTAmount"]);
                    r.WDONightDiff = Convert.ToDecimal(oRow["WDONightDiff"]);
                    r.WDONightDiffAmount = Convert.ToDecimal(oRow["WDONightDiffAmount"]);
                    r.WDONightDiffOT = Convert.ToDecimal(oRow["WDONightDiffOT"]);
                    r.WDONightDiffOTAmount = Convert.ToDecimal(oRow["WDONightDiffOTAmount"]);
                    r.WDORegularHoliday = Convert.ToDecimal(oRow["WDORegularHoliday"]);
                    r.WDORegularHolidayAmount = Convert.ToDecimal(oRow["WDORegularHolidayAmount"]);
                    r.WDORegularHolidayOT = Convert.ToDecimal(oRow["WDORegularHolidayOT"]);
                    r.WDORegularHolidayOTAmount = Convert.ToDecimal(oRow["WDORegularHolidayOTAmount"]);
                    r.WDORegularHolidayNightDiff = Convert.ToDecimal(oRow["WDORegularHolidayNightDiff"]);
                    r.WDORegularHolidayNightDiffAmount = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffAmount"]);
                    r.WDORegularHolidayNightDiffOT = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffOT"]);
                    r.WDORegularHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffOTAmount"]);
                    r.WDOSpecialHoliday = Convert.ToDecimal(oRow["WDOSpecialHoliday"]);
                    r.WDOSpecialHolidayAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayAmount"]);
                    r.WDOSpecialHolidayOT = Convert.ToDecimal(oRow["WDOSpecialHolidayOT"]);
                    r.WDOSpecialHolidayOTAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayOTAmount"]);
                    r.WDOSpecialHolidayNightDiff = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiff"]);
                    r.WDOSpecialHolidayNightDiffAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffAmount"]);
                    r.WDOSpecialHolidayNightDiffOT = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffOT"]);
                    r.WDOSpecialHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffOTAmount"]);
                    r.VLDays = Convert.ToDecimal(oRow["VLDays"]);
                    r.VLAmount = Convert.ToDecimal(oRow["VLAmount"]);
                    r.SLDays = Convert.ToDecimal(oRow["SLDays"]);
                    r.SLAmount = Convert.ToDecimal(oRow["SLAmount"]);
                    r.BLDays = Convert.ToDecimal(oRow["BLDays"]);
                    r.BLAmount = Convert.ToDecimal(oRow["BLAmount"]);
                    r.ELDays = Convert.ToDecimal(oRow["ELDays"]);
                    r.ELAmount = Convert.ToDecimal(oRow["ELAmount"]);
                    r.Meals = Convert.ToDecimal(oRow["Meals"]);
                    r.TaxableAllowance = Convert.ToDecimal(oRow["TaxableAllowance"]);
                    r.AbsentDays = Convert.ToDecimal(oRow["AbsentDays"]);
                    r.AbsentAmount = Convert.ToDecimal(oRow["AbsentAmount"]);
                    r.TotalTaxableAmount = Convert.ToDecimal(oRow["TotalTaxableAmount"]);
                    r.TaxAmount = Convert.ToDecimal(oRow["TaxAmount"]);
                    r.SSS = Convert.ToDecimal(oRow["SSS"]);
                    r.PHILHEALTH = Convert.ToDecimal(oRow["Philhealth"]);
                    r.PAGIBIG = Convert.ToDecimal(oRow["Pagibig"]);
                    r.ECOLA = Convert.ToDecimal(oRow["ECOLA"]);
                    r.GrossIncome = Convert.ToDecimal(oRow["GrossIncome"]);
                    r.NetIncome = Convert.ToDecimal(oRow["NetIncome"]);
                    r.OverDeductSSSLoan = Convert.ToDecimal(oRow["OverDeductSSSLoan"]);
                    r.OverDeductPagibigLoan = Convert.ToDecimal(oRow["OverDeductPagibigLoan"]);
                    r.VoluntaryContribution = Convert.ToDecimal(oRow["VoluntaryContribution"]);
                    r.Canteen = Convert.ToDecimal(oRow["Canteen"]);
                    r.RCBCLoan = Convert.ToDecimal(oRow["RCBCLoan"]);
                    r.PesonalAccounts = Convert.ToDecimal(oRow["PersonalAccount"]);
                    r.SSSLoan = Convert.ToDecimal(oRow["SSSLoan"]);
                    r.PAGIBIGLoan = Convert.ToDecimal(oRow["PAGIBIGLoan"]);
                    r.TaxData = new TaxCode();
                    if (oRow["TaxData"] != DBNull.Value)
                    {
                        r.TaxData = TaxCode.GetByID(Convert.ToInt64(oRow["TaxData"]));
                    }
                    else
                    {
                        r.TaxData.ID = 0;
                    }
                    _list.Add(r);
                }
            }
            return _list;
        }
        public static PayRegister GetTotalsInPayRegisterReport(Int64 FrequencyID, Int64 Year, Int64 PayPeriod, Int32 Month)
        {
            var r = new PayRegister();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Reports_TotalSum_PayRegister",
                    new string[] { "eFrequencyID", "eYear", "ePayPeriod", "eMonth" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64, DbType.Int32 },
                    new object[] { FrequencyID, Year, PayPeriod, Month }, out x, ref oTable, CommandType.StoredProcedure);
               if ( oTable.Rows.Count>0)
                {
                    DataRow oRow = oTable.Rows[0];
                    r.Employee = new Employee();
                    r.BasicPay = Convert.ToDecimal(oRow["BasicPay"]);
                    r.OvertimeAmount = Convert.ToDecimal(oRow["OvertimeAmount"]);
                    r.NightDiffAmount = Convert.ToDecimal(oRow["NightDiffAmount"]);
                    r.NightDiffOTAmount = Convert.ToDecimal(oRow["NightDiffOTAmount"]);
                    r.RegularHolidayAmount = Convert.ToDecimal(oRow["RegularHolidayAmount"]);
                    r.RegularHolidayOTAmount = Convert.ToDecimal(oRow["RegularHolidayOTAmount"]);
                    r.RegularHolidayNightDiffAmount = Convert.ToDecimal(oRow["RegularHolidayNightDiffAmount"]);
                    r.RegularHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["RegularHolidayNightDiffOTAmount"]);
                    r.SpecialHolidayAmount = Convert.ToDecimal(oRow["SpecialHolidayAmount"]);
                    r.SpecialHolidayOTAmount = Convert.ToDecimal(oRow["SpecialHolidayOTAmount"]);
                    r.SpecialHolidayNightDiffAmount = Convert.ToDecimal(oRow["SpecialHolidayNightDiffAmount"]);
                    r.SpecialHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["SpecialHolidayNightDiffOTAmount"]);
                    r.WDOAmount = Convert.ToDecimal(oRow["WDOAmount"]);
                    r.WDOOTAmount = Convert.ToDecimal(oRow["WDOOTAmount"]);
                    r.WDONightDiffAmount = Convert.ToDecimal(oRow["WDONightDiffAmount"]);
                    r.WDONightDiffOTAmount = Convert.ToDecimal(oRow["WDONightDiffOTAmount"]);
                    r.WDORegularHolidayAmount = Convert.ToDecimal(oRow["WDORegularHolidayAmount"]);
                    r.WDORegularHolidayOTAmount = Convert.ToDecimal(oRow["WDORegularHolidayOTAmount"]);
                    r.WDORegularHolidayNightDiffAmount = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffAmount"]);
                    r.WDORegularHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffOTAmount"]);
                    r.WDOSpecialHolidayAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayAmount"]);
                    r.WDOSpecialHolidayOTAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayOTAmount"]);
                    r.WDOSpecialHolidayNightDiffAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffAmount"]);
                    r.WDOSpecialHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffOTAmount"]);
                    r.VLAmount = Convert.ToDecimal(oRow["VLAmount"]);
                    r.SLAmount = Convert.ToDecimal(oRow["SLAmount"]);
                    r.BLAmount = Convert.ToDecimal(oRow["BLAmount"]);
                    r.ELAmount = Convert.ToDecimal(oRow["ELAmount"]);
                    r.Meals = Convert.ToDecimal(oRow["Meals"]);
                    r.TaxableAllowance = Convert.ToDecimal(oRow["TaxableAllowance"]);
                    r.AbsentAmount = Convert.ToDecimal(oRow["AbsentAmount"]);
                    r.TotalTaxableAmount = Convert.ToDecimal(oRow["TotalTaxableAmount"]);
                    r.TaxAmount = Convert.ToDecimal(oRow["TaxAmount"]);
                    r.SSS = Convert.ToDecimal(oRow["SSS"]);
                    r.PHILHEALTH = Convert.ToDecimal(oRow["Philhealth"]);
                    r.PAGIBIG = Convert.ToDecimal(oRow["Pagibig"]);
                    r.ECOLA = Convert.ToDecimal(oRow["ECOLA"]);
                    r.GrossIncome = Convert.ToDecimal(oRow["GrossIncome"]);
                    r.NetIncome = Convert.ToDecimal(oRow["NetIncome"]);
                    r.OverDeductSSSLoan = Convert.ToDecimal(oRow["OverDeductSSSLoan"]);
                    r.OverDeductPagibigLoan = Convert.ToDecimal(oRow["OverDeductPagibigLoan"]);
                    r.VoluntaryContribution = Convert.ToDecimal(oRow["VoluntaryContribution"]);
                    r.Canteen = Convert.ToDecimal(oRow["Canteen"]);
                    r.RCBCLoan = Convert.ToDecimal(oRow["RCBCLoan"]);
                    r.PesonalAccounts = Convert.ToDecimal(oRow["PersonalAccount"]);
                    r.SSSLoan = Convert.ToDecimal(oRow["SSSLoan"]);
                    r.PAGIBIGLoan = Convert.ToDecimal(oRow["PAGIBIGLoan"]);
                }
            }
            return r;
        }
    }
}
