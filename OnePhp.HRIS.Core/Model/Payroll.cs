using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OnePhp.HRIS.Core.Data;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace OnePhp.HRIS.Core.Model
{
    public class Payroll
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public Department Department { get; set; }
        public Division Division { get; set; }
        public EmployeeContract Contract { get; set; }
        public bool isProcessing {get;set;}
        public PayPeriod PayPeriod { get; set; }
        public EmployeeClassCode Class { get; set; }
        public Ref_PaySchedule PaySchedule { get; set; }
        public Adjustments Adjustments { get; set; }
        public BatchAdjustment AdjustmentCode { get; set; }
        public static List<Payroll> GetEmployeeForPayrollProcessing(Int64 PayPeriod)
        {
            var _list = new List<Payroll>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("Hris_GetEmployee_PayrollProcessing",
                    new string[] { "ePayperiodID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { PayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    Payroll p = new Payroll();
                    p.Employee = new Employee();
                    p.Department = new Department();
                    p.Division = new Division();
                    p.Contract = new EmployeeContract();
                    p.Employee.ID = Convert.ToInt64(oRow["ID"]);
                    p.Employee.EmployeeID = oRow["EmployeeID"].ToString();
                    p.Employee.Firstname = oRow["Firstname"].ToString();
                    p.Employee.Lastname = oRow["Lastname"].ToString();
                    p.Employee.Middlename = oRow["Middlename"].ToString();
                    p.Employee.Suffix = oRow["Suffix"].ToString();
                    p.Department.Description= oRow["Department"].ToString();
                    p.Division.Div_Desc = oRow["Division"].ToString();
                    p.Contract.NewSalary = Convert.ToDouble(oRow["NewSalary"]);
                    p.Contract.Ecola = Convert.ToDouble(oRow["Ecola"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
        public static Payroll CurrentProcess()
        {
            var p = new Payroll();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = db.Fetch(string.Format("select isProcessing from HRIS_PayrollStatus"));
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    if (Convert.ToInt32(oRow["isProcessing"]) == 1)
                    {
                        p.isProcessing = true;
                    }
                    else
                    {
                        p.isProcessing = false;
                    }
                }
            }
            return p;
        }
        public static void SetToProcessing()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_SetIsProcessing_PayrollStatus",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }
        public static void ProcessingIsDone()
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_ProcessingIsDone_PayrollStatus",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, CommandType.StoredProcedure);
            }
        }
        public static void ProcessingSIL(Int64 pID)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                long pClassId = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Annerversaries_SIL",
                    new string[] { "PayPeriodID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { pID }, out x, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in aTable.Rows)
                {
                    Payroll p = new Payroll();
                    p.Employee = new Employee();
                    p.Contract = new EmployeeContract();
                    p.PaySchedule = new Ref_PaySchedule();
                    p.AdjustmentCode = new BatchAdjustment();
                    p.Class = new EmployeeClassCode();
                    p.Employee.ID = Convert.ToInt64(oRow["ID"]);
                    p.Contract.NewSalary = Convert.ToDouble(oRow["NewSalary"]);
                    p.PayPeriod = new PayPeriod();
                    p.PayPeriod.ID= Convert.ToInt64(oRow["PayperiodID"]);
                    p.Class.ID = Convert.ToInt64(oRow["ClassID"]);
                    p.PaySchedule.ID = Convert.ToInt64(oRow["Payschedule"]);
                    
                    //34
                    //1. No need to check if SIL is added or deducted, because it is a single transaction per employee
                    //2. Need to check if employee is eligible (T/F:Rank & File/Daily Rate)
                    double totalSIL =0;
                    double salary = 0;
                    string adjCode = "";
                    salary = p.Contract.NewSalary;
                    pClassId = p.Class.ID;
                    if (pClassId == 22)
                    {
                        //check if employee is a daily rate
                        if (p.PaySchedule.ID == 1)
                        {
                            totalSIL = salary * 5;
                            DataTable oTable = db.Fetch(string.Format("select id,code from hris_batchadjustment where id = {0}",34)); // 34 = SIL in tables
                            if (oTable.Rows.Count > 0)
                            {
                                try
                                {
                                    DataRow bRow = oTable.Rows[0];
                                    adjCode = bRow["Code"].ToString();
                                    p.Adjustments = new Adjustments();
                                    p.Adjustments.Adjustment = new BatchAdjustment();
                                    p.Adjustments.UploadedBy = new Employee();
                                    p.Adjustments.PayPeriod = new PayPeriod();
                                    p.Adjustments.Employee = new Employee();
                                    p.Adjustments.Adjustment.ID = 34;
                                    p.Adjustments.Adjustment.Code = bRow["Code"].ToString();
                                    p.Adjustments.Amount = totalSIL;
                                    p.Adjustments.Taxable = false;
                                    p.Adjustments.PayPeriod.ID = pID;
                                    p.Adjustments.Employee.ID = p.Employee.ID;
                                    p.Adjustments.UploadedBy.ID = 0;
                                    p.Adjustments.Logs = new SystemLogs();
                                    p.Adjustments.Logs.Type = 1;
                                    p.Adjustments.Logs.Module = 9;
                                    p.Adjustments.Logs.Before = "";
                                    p.Adjustments.Logs.UserID = 0;
                                    p.Adjustments.Logs.UserName = "System Administrator";
                                    Adjustments.SaveNewAdjustment(p.Adjustments);
                                }
                                catch (Exception ex)
                                {
                                    _ = ex.Message;
                                }
                            }
                            
                        }
                        //if false, employee is a monthly rate/semi monthly
                        else
                        {
                            salary = p.Contract.NewSalary * 12 / 340;
                            totalSIL = salary * 5;

                            DataTable oTable = db.Fetch(string.Format("select id,code from hris_batchadjustment where id = {0}", 34)); // 34 = SIL in tables
                            if (oTable.Rows.Count > 0)
                            {
                                try
                                {
                                    DataRow bRow = oTable.Rows[0];
                                    adjCode = bRow["Code"].ToString();
                                    p.Adjustments = new Adjustments();
                                    p.Adjustments.Adjustment = new BatchAdjustment();
                                    p.Adjustments.UploadedBy = new Employee();
                                    p.Adjustments.PayPeriod = new PayPeriod();
                                    p.Adjustments.Employee = new Employee();
                                    p.Adjustments.Adjustment.ID = 34;
                                    p.Adjustments.Adjustment.Code = bRow["Code"].ToString();
                                    p.Adjustments.Amount = totalSIL;
                                    p.Adjustments.Taxable = false;
                                    p.Adjustments.PayPeriod.ID = pID;
                                    p.Adjustments.Employee.ID = p.Employee.ID;
                                    p.Adjustments.UploadedBy.ID = 0;
                                    p.Adjustments.Logs = new SystemLogs();
                                    p.Adjustments.Logs.Type = 1;
                                    p.Adjustments.Logs.Module = 9;
                                    p.Adjustments.Logs.Before = "";
                                    p.Adjustments.Logs.UserID = 0;
                                    p.Adjustments.Logs.UserName = "System Administrator";
                                    Adjustments.SaveNewAdjustment(p.Adjustments);
                                }
                                catch (Exception ex)
                                {
                                    _ = ex.Message;
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        public static void Process(Int64[] EmployeeIDs, Int64 PayPeriodID)
        {
            long PayOutID = 0;
            try
            {
                using (AppDb db = new AppDb())
                {
                    db.Open();
                    DataTable oTable = db.Fetch(string.Format("select ID PayOUTID from hris_payout where PayPeriodID = {0}", PayPeriodID));
                    if (oTable.Rows.Count > 0)
                    {
                        DataRow oRow = oTable.Rows[0];
                        PayOutID = Convert.ToInt64(oRow["PayOutID"]);
                       
                    }
                    else
                    {
                        int x = 0;
                        db.ExecuteCommandReader("HRIS_Save_Payout",
                            new string[] { "PayPeriodID" },
                            new DbType[] { DbType.Int64 },
                            new object[] { PayPeriodID }, out x, ref oTable, CommandType.StoredProcedure);
                        DataRow oRow = oTable.Rows[0];
                        PayOutID = Convert.ToInt64(oRow["PayOutID"]);
                    }
                }
                Payroll.ProcessingSIL(PayPeriodID);
                foreach (Int64 EmpID in EmployeeIDs)
                {
                    ProcessEmployee(EmpID, PayPeriodID, PayOutID);
                }

                //Task.Delay(5000);
                Task.Delay(20000);
                //todo - set IsProcessing  = 0;
                ProcessingIsDone();
            }
            catch (Exception ex)
            {
                ProcessingIsDone();
            }
        }
        public static void ProcessEmployee(Int64 EmployeeID, Int64 PayPeriodID, Int64 PayOutID)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();

                try
                {
                    PayPeriod payPeriod = PayPeriod.GetPayPeriodByID(PayPeriodID);

                    DataTable oTable = db.Fetch(String.Format("select * from hris_payperiod where Year = {0} and Month = {1}", payPeriod.Year, payPeriod.Month));

                    int Cutoff = 0;
                    foreach (DataRow nRow in oTable.Rows)
                    {
                        Cutoff++;
                        if (PayPeriodID == Convert.ToInt64(nRow["ID"])) break;
                    }

                    //get rate
                    decimal Salary = 0;
                    decimal ECola = 0;
                    long ContractID = 0;

                    oTable = db.Fetch(String.Format("select * from hris_employee_contractdetails where EmployeeID = {0} and Status = 4", EmployeeID));

                    if (oTable.Rows.Count == 0) return;

                    DataRow oRow = oTable.Rows[0];
                    Salary = Convert.ToDecimal(oRow["NewSalary"]);
                    ECola = Convert.ToDecimal(oRow["Ecola"]);
                    ContractID = Convert.ToInt64(oRow["ID"]);

                    decimal DailyRate = Salary;
                    decimal SemiMonthlyRate = Salary / 2;
                    Decimal GrossPay = SemiMonthlyRate;
                    Decimal NetPay = SemiMonthlyRate;


                    // 5 is semi monthly
                    if (payPeriod.Frequency.ID == 5)
                    {
                        DailyRate = Salary * 12 / 313;
                    }

                    oTable = db.Fetch(String.Format("select * from HRIS_StatutoryDeduction where FrequencyID = {0} and Status = 1", payPeriod.Frequency.ID));
                    if (oTable.Rows.Count == 0) return;

                    int deductSSS = 0;
                    int deductPhilhealth = 0;
                    int deductPagibig = 0;
                    int deductTax = 0;

                    decimal sAmountSSS = 0;
                    decimal sAmountPhilhealth = 0;
                    decimal sAmountPagibig = 0;
                    decimal sAmountTax = 0;

                    foreach (DataRow nRow in oTable.Rows)
                    {
                        if (nRow["1st"].ToString() == "1" && Cutoff == 1)
                        {
                            switch (nRow["DeductionType"].ToString())
                            {
                                case "1": deductPagibig = 1; break;
                                case "2": deductPhilhealth = 1; break;
                                case "3": deductSSS = 1; break;
                                case "4": deductTax = 1; break;
                            }
                        }

                        else if (nRow["2nd"].ToString() == "1" && Cutoff == 2)
                        {
                            switch (nRow["DeductionType"].ToString())
                            {
                                case "1": deductPagibig = 1; break;
                                case "2": deductPhilhealth = 1; break;
                                case "3": deductSSS = 1; break;
                                case "4": deductTax = 1; break;
                            }
                        }
                        else if (nRow["distribute"].ToString() == "1")
                        {
                            switch (nRow["DeductionType"].ToString())
                            {
                                case "1": deductPagibig = 2; break;
                                case "2": deductPhilhealth = 2; break;
                                case "3": deductSSS = 2; break;
                                case "4": deductTax = 2; break;
                            }
                        }
                    }

                    if (deductSSS == 1)
                    {
                        oTable = db.Fetch(string.Format("select AC_RSS_Employees as SSS from hris_ssscontributiontable where {0} >= RangeOfCompensationFrom and {0} <= RangeOfCompensationTo", Salary));
                        sAmountSSS = Convert.ToDecimal(oTable.Rows[0]["SSS"]);
                        NetPay -= sAmountSSS;
                    }

                    if (deductPhilhealth == 1)
                    {
                        oTable = db.Fetch(string.Format("select (BaseAmount + (({0} - `From`) * cast(replace(Percent, \"%\", \"\") as decimal) / 100)) / 2 as Philhealth from hris_philhealthcontributiontable hp where {0} >= `From` and {0} <= `To` ", Salary));
                        sAmountPhilhealth = Convert.ToDecimal(oTable.Rows[0]["Philhealth"]);
                        NetPay -= sAmountPhilhealth;
                    }

                    if (deductPagibig == 1)
                    {
                        oTable = db.Fetch(string.Format("select {0} * cast(replace(PMC_EmployerShare , \"%\", \"\") as decimal) / 100 as Pagibig from hris_pagibigcontributiontable hp where {0} >= MC_From  and {0} <= MC_To  ", Salary));
                        sAmountPagibig = Convert.ToDecimal(oTable.Rows[0]["Pagibig"]);
                        NetPay -= sAmountPagibig;
                    }

                    if (deductTax == 1)
                    {

                    }
                    else if (deductTax == 2)
                    {
                        oTable = db.Fetch(string.Format("select BaseAmount + (({0} - ExcessFrom) * cast(replace(RateOfExcess, \"%\", \"\") as decimal) / 100) as TaxAmount from hris_taxtable ht where {0} >= `From` and {0} < NotOver and type = 6", SemiMonthlyRate));
                        sAmountTax = Convert.ToDecimal(oTable.Rows[0]["TaxAmount"]);
                        NetPay -= sAmountTax;
                    }



                    PaySlip r = new PaySlip();
                    r.Employee = new Employee();
                    r.PayRegister = new PayRegister();
                    r.PaySchedule = new Ref_PaySchedule();
                    r.Employee.Contract = new EmployeeContract();
                    r.Employee.Department = new Department();
                    r.Employee.Division = new Division();
                    r.Payperiod = new PayPeriod();
                    r.Project = new ProjectCode();
                    r.Division = new Division();
                    r.Department = new Department();
                    r.PayOut = new PayOut();
                    r.PayOut.ID = PayOutID;
                    r.Payperiod = payPeriod;
                    r.Employee = Employee.GetEmployeeDetailsByID(EmployeeID);

                    r.PayRegister.BasicPay = SemiMonthlyRate;
                    r.PayRegister.HoursWorked = 0;
                    r.PayRegister.Overtime = 0;
                    r.PayRegister.OvertimeAmount = 0;
                    r.PayRegister.NightDiff = 0;
                    r.PayRegister.NightDiffAmount = 0;
                    r.PayRegister.NightDiffOT = 0;
                    r.PayRegister.NightDiffOTAmount = 0;
                    r.PayRegister.RegularHoliday = 0;
                    r.PayRegister.RegularHolidayAmount = 0;
                    r.PayRegister.RegularHolidayOT = 0;
                    r.PayRegister.RegularHolidayOTAmount = 0;
                    r.PayRegister.RegularHolidayNightDiff = 0;
                    r.PayRegister.RegularHolidayNightDiffAmount = 0;
                    r.PayRegister.RegularHolidayNightDiffOT = 0;
                    r.PayRegister.RegularHolidayNightDiffOTAmount = 0;
                    r.PayRegister.SpecialHoliday = 0;
                    r.PayRegister.SpecialHolidayAmount = 0;
                    r.PayRegister.SpecialHolidayOT = 0;
                    r.PayRegister.SpecialHolidayOTAmount = 0;
                    r.PayRegister.SpecialHolidayNightDiff = 0;
                    r.PayRegister.SpecialHolidayNightDiffAmount = 0;
                    r.PayRegister.SpecialHolidayNightDiffOT = 0;
                    r.PayRegister.SpecialHolidayNightDiffOTAmount = 0;
                    r.PayRegister.WDO = 0;
                    r.PayRegister.WDOAmount = 0;
                    r.PayRegister.WDOOT = 0;
                    r.PayRegister.WDOOTAmount = 0;
                    r.PayRegister.WDONightDiff = 0;
                    r.PayRegister.WDONightDiffAmount = 0;
                    r.PayRegister.WDONightDiffOT = 0;
                    r.PayRegister.WDONightDiffOTAmount = 0;
                    r.PayRegister.WDORegularHoliday = 0;
                    r.PayRegister.WDORegularHolidayAmount = 0;
                    r.PayRegister.WDORegularHolidayOT = 0;
                    r.PayRegister.WDORegularHolidayOTAmount = 0;
                    r.PayRegister.WDORegularHolidayNightDiff = 0;
                    r.PayRegister.WDORegularHolidayNightDiffAmount = 0;
                    r.PayRegister.WDORegularHolidayNightDiffOT = 0;
                    r.PayRegister.WDORegularHolidayNightDiffOTAmount = 0;
                    r.PayRegister.WDOSpecialHoliday = 0;
                    r.PayRegister.WDOSpecialHolidayAmount = 0;
                    r.PayRegister.WDOSpecialHolidayOT = 0;
                    r.PayRegister.WDOSpecialHolidayOTAmount = 0;
                    r.PayRegister.WDOSpecialHolidayNightDiff = 0;
                    r.PayRegister.WDOSpecialHolidayNightDiffAmount = 0;
                    r.PayRegister.WDOSpecialHolidayNightDiffOT = 0;
                    r.PayRegister.WDOSpecialHolidayNightDiffOTAmount = 0;
                    r.PayRegister.VLDays = 0;
                    r.PayRegister.VLAmount = 0;
                    r.PayRegister.SLDays = 0;
                    r.PayRegister.SLAmount = 0;
                    r.PayRegister.BLDays = 0;
                    r.PayRegister.BLAmount = 0;
                    r.PayRegister.ELDays = 0;
                    r.PayRegister.ELAmount = 0;
                    r.PayRegister.Meals = 0;
                    r.PayRegister.TaxableAllowance = 0;
                    r.PayRegister.AbsentDays = 0;
                    r.PayRegister.AbsentAmount = 0;
                    r.PayRegister.TotalTaxableAmount = SemiMonthlyRate;
                    r.PayRegister.TaxAmount = sAmountTax;
                    r.PayRegister.SSS = sAmountSSS;
                    r.PayRegister.TardyAmount = 0;
                    r.PayRegister.TardyDays = 0;
                    r.PayRegister.PHILHEALTH = sAmountPhilhealth;
                    r.PayRegister.PAGIBIG = sAmountPagibig;
                    r.PayRegister.ECOLA = ECola;
                    r.PayRegister.GrossIncome = GrossPay;
                    r.PayRegister.NetIncome = NetPay;

                    int x = 0;
                    db.ExecuteCommandNonQuery("HRIS_Save_Payroll",
                            new string[] { "pPayoutID", "pEmployeeID", "pContractID", "BasicPay", "HoursWorked", "Overtime", "OvertimeAmount",
                                        "NightDiff", "NightDiffAmount", "NightDiffOT", "NightDiffOTAmount",
                                        "RegularHoliday", "RegularHolidayAmount", "RegularHolidayOT", "RegularHolidayOTAmount",
                                        "RegularHolidayNightDiff", "RegularHolidayNightDiffAmount", "RegularHolidayNightDiffOT", "RegularHolidayNightDiffOTAmount",
                                        "SpecialHoliday", "SpecialHolidayAmount", "SpecialHolidayOT", "SpecialHolidayOTAmount",
                                        "SpecialHolidayNightDiff", "SpecialHolidayNightDiffAmount", "SpecialHolidayNightDiffOT", "SpecialHolidayNightDiffOTAmount",
                                        "WDO", "WDOAmount", "WDOOT", "WDOOTAmount",
                                        "WDONightDiff", "WDONightDiffAmount", "WDONightDiffOT", "WDONightDiffOTAmount",
                                        "WDORegularHoliday", "WDORegularHolidayAmount", "WDORegularHolidayOT", "WDORegularHolidayOTAmount",
                                        "WDORegularHolidayNightDiff", "WDORegularHolidayNightDiffAmount", "WDORegularHolidayNightDiffOT", "WDORegularHolidayNightDiffOTAmount",
                                        "WDOSpecialHoliday", "WDOSpecialHolidayAmount", "WDOSpecialHolidayOT", "WDOSpecialHolidayOTAmount",
                                        "WDOSpecialHolidayNightDiff", "WDOSpecialHolidayNightDiffAmount", "WDOSpecialHolidayNightDiffOT", "WDOSpecialHolidayNightDiffOTAmount",
                                        "VLDays", "VLAmount", "SLDays", "SLAmount",
                                        "BLDays", "BLAmount", "ELDays", "ELAmount",
                                        "Meals", "TaxableAllowance", "AbsentDays", "AbsentAmount",
                                        "TardyDays", "TardyAmount", "TotalTaxableAmount", "TaxAmount",
                                        "SSS", "Philhealth", "PagIbig", "ECOLA", "GrossIncome", "NetIncome" },
                            new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal,
                                        DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.Decimal

                            },
                            new object[] { PayOutID, r.Employee.ID, ContractID, r.PayRegister.BasicPay, r.PayRegister.HoursWorked, r.PayRegister.Overtime, r.PayRegister.OvertimeAmount,
                                        r.PayRegister.NightDiff, r.PayRegister.NightDiffAmount, r.PayRegister.NightDiffOT, r.PayRegister.NightDiffOTAmount,
                                        r.PayRegister.RegularHoliday, r.PayRegister.RegularHolidayAmount, r.PayRegister.RegularHolidayOT, r.PayRegister.RegularHolidayOTAmount,
                                        r.PayRegister.RegularHolidayNightDiff, r.PayRegister.RegularHolidayNightDiffAmount, r.PayRegister.RegularHolidayNightDiffOT, r.PayRegister.RegularHolidayNightDiffOTAmount,
                                        r.PayRegister.SpecialHoliday, r.PayRegister.SpecialHolidayAmount, r.PayRegister.SpecialHolidayOT, r.PayRegister.SpecialHolidayOTAmount, 
                                        r.PayRegister.SpecialHolidayNightDiff, r.PayRegister.SpecialHolidayNightDiffAmount, r.PayRegister.SpecialHolidayNightDiffOT, r.PayRegister.SpecialHolidayNightDiffOTAmount, 
                                        r.PayRegister.WDO, r.PayRegister.WDOAmount, r.PayRegister.WDOOT, r.PayRegister.WDOOTAmount, 
                                        r.PayRegister.WDONightDiff, r.PayRegister.WDONightDiffAmount, r.PayRegister.WDONightDiffOT, r.PayRegister.WDONightDiffOTAmount, 
                                        r.PayRegister.WDORegularHoliday,r.PayRegister.WDORegularHolidayAmount, r.PayRegister.WDORegularHolidayOT, r.PayRegister.WDORegularHolidayOTAmount, 
                                        r.PayRegister.WDORegularHolidayNightDiff, r.PayRegister.WDORegularHolidayNightDiffAmount, r.PayRegister.WDORegularHolidayNightDiffOT, r.PayRegister.WDORegularHolidayNightDiffOTAmount, 
                                        r.PayRegister.WDOSpecialHoliday,r.PayRegister.WDOSpecialHolidayAmount, r.PayRegister.WDOSpecialHolidayOT, r.PayRegister.WDOSpecialHolidayOTAmount, 
                                        r.PayRegister.WDOSpecialHolidayNightDiff, r.PayRegister.WDOSpecialHolidayNightDiffAmount, r.PayRegister.WDOSpecialHolidayNightDiffOT, r.PayRegister.WDOSpecialHolidayNightDiffOTAmount,
                                        r.PayRegister.VLDays, r.PayRegister.VLAmount, r.PayRegister.SLDays, r.PayRegister.SLAmount,
                                        r.PayRegister.BLDays, r.PayRegister.BLAmount, r.PayRegister.ELDays, r.PayRegister.ELAmount,
                                        r.PayRegister.Meals, r.PayRegister.TaxableAllowance, r.PayRegister.AbsentDays, r.PayRegister.AbsentAmount,
                                        r.PayRegister.TardyDays, r.PayRegister.TardyAmount, r.PayRegister.TotalTaxableAmount, r.PayRegister.TaxAmount,
                                        r.PayRegister.SSS, r.PayRegister.PHILHEALTH, r.PayRegister.PAGIBIG, r.PayRegister.ECOLA, r.PayRegister.GrossIncome, r.PayRegister.NetIncome
                            }, out x, CommandType.StoredProcedure); 
                }
                catch (Exception ex)
                {
                    //todo processing error log
                    ProcessingIsDone();
                }
            }
        }
    }

    public class BETATransmittal
    {
        public long ID { get; set; }
        public string Filename { get; set; }
        public BankCode Bank { get; set; }
        public PayOut Payout { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public CompanySetUp Company { get; set; }
        public string DateToSend { get; set; }
        public string DateCreated { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountInWords { get; set; }


        public string TotalRecords { get; set; }
        public static string Save(BETATransmittal details)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable yTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isFilenameInUse_Transmittal",
                    new string[] { "ePayoutID", "eFilename", "eBank", },
                    new DbType[] { DbType.Int64, DbType.String, DbType.Int64 },
                    new object[] { details.Payout.ID, details.Filename, details.Bank.ID }, out y, ref yTable, CommandType.StoredProcedure);
                if (yTable.Rows.Count > 0)
                {
                    res = "Filename is in use with the same pay out and same bank.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_Transmittal",
                       new string[] { "ePayoutID", "eFilename", "eBank", "eAddedBy", "eModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.String, DbType.Int64, DbType.String, DbType.String },
                       new object[] { details.Payout.ID, details.Filename, details.Bank.ID, details.AddedBy, details.ModifiedBy }, out x, CommandType.StoredProcedure);
                    res = "ok";
                }
            }
            return res;
        }
        public static string Edit(BETATransmittal details)
        {
            string res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable yTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isFilenameInUse_Update_Transmittal",
                    new string[] { "eID", "ePayoutID", "eFilename", "eBank", },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64 },
                    new object[] { details.ID, details.Payout.ID, details.Filename, details.Bank.ID }, out y, ref yTable, CommandType.StoredProcedure);
                if (yTable.Rows.Count > 0)
                {
                    res = "Filename is in use with the same pay out and same bank.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_EDIT_Transmittal",
                        new string[] { "eID", "ePayoutID", "eFilename", "eBank", "eModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64, DbType.String },
                        new object[] { details.ID, details.Payout.ID, details.Filename, details.Bank.ID, details.ModifiedBy }, out x, CommandType.StoredProcedure);
                    res = "ok";
                }
            }
            return res;
        }
        public static void Delete(BETATransmittal details)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Transmittal",
                     new string[] { "eID", "eModifiedBy" },
                     new DbType[] { DbType.Int64, DbType.String },
                     new object[] { details.ID, details.ModifiedBy }, out x, CommandType.StoredProcedure);
            }
        }
        public static List<BETATransmittal> Get()
        {
            var _list = new List<BETATransmittal>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Transmittal",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in oTable.Rows)
                {
                    BETATransmittal t = new BETATransmittal();
                    t.Payout = new PayOut();
                    t.Payout.PayPeriod = new PayPeriod();
                    t.Bank = new BankCode();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Payout.ID = Convert.ToInt64(aRow["PayoutID"]);
                    t.Payout.PayPeriod.PayDate = Convert.ToDateTime(aRow["PayDate"]);
                    t.Filename = aRow["FileName"].ToString();
                    t.Bank = BankCode.GetByID(Convert.ToInt64(aRow["bank"]));
                    _list.Add(t);
                }
            }
            return _list;
        }
        public static BETATransmittal GetByID(Int64 Id)
        {
            var t = new BETATransmittal();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_ByID_Transmittal",
                   new string[] { "eId" },
                   new DbType[] { DbType.Int64 },
                   new object[] { Id }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    t.Payout = new PayOut();
                    t.Bank = new BankCode();
                    t.ID = Convert.ToInt64(aRow["ID"]);
                    t.Payout.ID = Convert.ToInt64(aRow["PayoutID"]);
                    t.Filename = aRow["FileName"].ToString();
                    t.Bank = BankCode.GetByID(Convert.ToInt64(aRow["bank"]));
                }
            }
            return t;
        }
        public static BETATransmittal GenerateATDBDO(Int64 eFrequencyID, Int32 eMonth, Int32 eYear, Int64 ePayPeriod)
        {
            var t = new BETATransmittal();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDetailsFor_BDO_Transmittal",
                    new string[] { "eFrequencyID", "eMonth", "eYear", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { eFrequencyID, eMonth, eYear, ePayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    t.TotalAmount = Convert.ToDecimal(oRow["TotalAmount"]);
                    t.Company = new CompanySetUp();
                    t.Company.CompanyName = oRow["CompanyName"].ToString();
                    t.Company.BDOAccountNumber = oRow["BDOAccountNumber"].ToString();
                    t.Payout = new PayOut();
                    t.Payout.PayPeriod = new PayPeriod();
                    t.Payout.PayPeriod.PayDate = Convert.ToDateTime(oRow["PayDate"]);
                    t.DateCreated = oRow["DateCreated"].ToString();
                    t.DateToSend = oRow["DateToSend"].ToString();
                }
            }
            return t;
        }
        public static BETATransmittal GenerateATDMetroBank(Int64 eFrequencyID, Int32 eMonth, Int32 eYear, Int64 ePayPeriod)
        {
            var t = new BETATransmittal();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDetailsFor_MTB_Transmittal",
                    new string[] { "eFrequencyID", "eMonth", "eYear", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { eFrequencyID, eMonth, eYear, ePayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    t.TotalAmount = Convert.ToDecimal(oRow["TotalAmount"]);
                    t.Company = new CompanySetUp();
                    t.Company.CompanyName = oRow["CompanyName"].ToString();
                    t.Company.MTBAccountNumber = oRow["MEtroBankAccountNumber"].ToString();
                    t.Company.MTBCompanyBranchCode = oRow["BankBranchCode"].ToString();
                    t.Company.MTBCompanyCode = oRow["BankCode"].ToString();
                    t.Payout = new PayOut();
                    t.Payout.PayPeriod = new PayPeriod();
                    t.Payout.PayPeriod.PayDate = Convert.ToDateTime(oRow["PayDate"]);
                    t.DateCreated = oRow["DateCreated"].ToString();
                    t.DateToSend = oRow["DateToSend"].ToString();
                    t.TotalRecords = oRow["TotalRecords"].ToString();
                }
            }
            return t;
        }
    }
    public class SAPFile
    {
        public string Header { get; set; }
        public string Details { get; set; }
        public string Trailer { get; set; }
        public string FileTitle { get; set; }

        //used for metrobank excel file
        public string BranchCodeOfCompany { get; set; }
        public string CompanyAccountNumber { get; set; }
        public string BranchCodeOfEmployee { get; set; }
        public string EmployeeAccountNumber { get; set; }
        public Decimal Amount { get; set; }
        public string CompanyCode { get; set; }
        public string AccountName { get; set; }
        public static List<SAPFile> GenerateMTBExcelFileDetails(Int64 eFrequencyID, Int32 eMonth, Int32 eYear, Int64 ePayPeriod)
        {
            var _list = new List<SAPFile>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Generate_MTB_EXE__Transmittal",
                    new string[] { "eFrequencyID", "eMonth", "eYear", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { eFrequencyID, eMonth, eYear, ePayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    SAPFile s = new SAPFile();
                    s.BranchCodeOfCompany = oRow["BranchCodeOfTheCompany"].ToString();
                    s.CompanyAccountNumber = oRow["CompanyAccountNumber"].ToString();
                    s.BranchCodeOfEmployee = oRow["BranchCodeOfTheEmployee"].ToString();
                    s.EmployeeAccountNumber = oRow["EmployeeAccountNumber"].ToString();
                    s.Amount = Convert.ToDecimal(oRow["Amount"]);
                    s.CompanyCode = oRow["CompanyCode"].ToString();
                    s.AccountName = oRow["AccountName"].ToString();
                    _list.Add(s);
                }
            }
            return _list;
        }
        public static SAPFile GenerateHeader(Int64 eFrequencyID, Int32 eMonth, Int32 eYear, Int64 ePayPeriod)
        {
            var s = new SAPFile();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GenerateBDO_PAY_Header_Transmittal",
                    new string[] { "eFrequencyID", "eMonth", "eYear", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { eFrequencyID, eMonth, eYear, ePayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    s.Header = oRow["Header"].ToString();
                    s.FileTitle = oRow["FileTitle"].ToString();
                }
            }
            return s;
        }
        public static List<SAPFile> GenerateDetails(Int64 eFrequencyID, Int32 eMonth, Int32 eYear, Int64 ePayPeriod)
        {
            var _list = new List<SAPFile>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GenerateBDO_PAY_DETAILS_Transmittal",
                    new string[] { "eFrequencyID", "eMonth", "eYear", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { eFrequencyID, eMonth, eYear, ePayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    SAPFile s = new SAPFile();
                    s.Details = oRow["Details"].ToString();
                    _list.Add(s);
                }
            }
            return _list;
        }
        public static SAPFile GenerateTrailerRecord(Int64 eFrequencyID, Int32 eMonth, Int32 eYear, Int64 ePayPeriod)
        {
            var s = new SAPFile();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GenerateBDO_PAY_Trailer_Transmittal",
                    new string[] { "eFrequencyID", "eMonth", "eYear", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int64 },
                    new object[] { eFrequencyID, eMonth, eYear, ePayPeriod }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    s.Trailer = oRow["Trailer"].ToString();
                }
            }
            return s;
        }

    }
    public class PaySlip
    {
        public PayRegister PayRegister { get; set; }
        public Employee Employee { get; set; }
        public PayPeriod Payperiod { get; set; }
        public Ref_PaySchedule PaySchedule { get; set; }
        public decimal YTDSalaryNet { get; set; }
        public decimal YTDSalaryGross { get; set; }
        public decimal YTDSSS { get; set; }
        public decimal YTDTotalTaxableAmount { get; set; }
        public decimal YTDPHILHEALTH { get; set; }
        public decimal YTDWTax { get; set; }
        public decimal YTDTardy { get; set; }
        public decimal YTDAbsent { get; set; }
        public decimal YTDPAGIBIG { get; set; }
        public ProjectCode Project { get; set; }
        public Department Department { get; set; }
        public Division Division { get; set; }
        public PayOut PayOut { get; set; }
        public Loans Loan { get; set; }
        public int LoanNumberOfPayments { get; set; }
        public Adjustments Adjustments{ get; set; }
        public static List<PaySlip> GetEmployeePaySlip(Int64 EmpID)
        {
            var _list = new List<PaySlip>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetBYEmployee_Payslip",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpID }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    PaySlip r = new PaySlip();
                    r.Employee = new Employee();
                    r.PaySchedule = new Ref_PaySchedule();
                    r.Payperiod = new PayPeriod();
                    r.Payperiod.Frequency = new PayFrequencyDetails();
                    r.Payperiod.Frequency.CutOff = new PayrollCutOff();
                    r.PayRegister = new PayRegister();
                    r.PayRegister.ID = Convert.ToInt64(oRow["ID"]);
                    r.Employee.ID =  Convert.ToInt64(oRow["EmployeeID"]);
                    r.Payperiod.Month = Convert.ToInt32(oRow["Month"]);
                    r.Payperiod.Start = Convert.ToDateTime(oRow["Start"]);
                    r.Payperiod.End = Convert.ToDateTime(oRow["End"]);
                    r.Payperiod.Year = Convert.ToInt32(oRow["Year"]);
                    r.PaySchedule.Description = oRow["Description"].ToString();
                    r.Payperiod.Frequency.CutOff.CutoffCount = oRow["CutOffCount"].ToString() ;
                    _list.Add(r);
                }
                
            }
            return _list;
        }
        public static PaySlip GetPaySlipDetails(Int64 ID, Int64 EmpID) 
        {
            var r = new PaySlip();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDetailsBYEmployee_Payslip",
                    new string[] { "eID","eEmployeeID" },
                    new DbType[] { DbType.Int64,DbType.Int64 },
                    new object[] { ID, EmpID }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    r.Employee = new Employee();
                    r.PayRegister = new PayRegister();
                    r.PaySchedule = new Ref_PaySchedule();
                    r.Employee.Contract = new EmployeeContract();
                    r.Employee.Department = new Department();
                    r.Employee.Division = new Division();
                    r.Payperiod = new PayPeriod();
                    r.Project = new ProjectCode();
                    r.Division = new Division();
                    r.Department = new Department();
                    r.PayOut = new PayOut();
                    r.PayOut.ID = Convert.ToInt32(oRow["PayoutID"]);
                    r.Payperiod.Frequency = new PayFrequencyDetails();
                    r.Payperiod.Frequency.CutOff = new PayrollCutOff();
                    r.Payperiod.Month = Convert.ToInt32(oRow["Month"]);
                    r.Payperiod.Start = Convert.ToDateTime(oRow["Start"]);
                    r.Payperiod.End = Convert.ToDateTime(oRow["End"]);
                    r.Payperiod.Year = Convert.ToInt32(oRow["Year"]);
                    r.PaySchedule.Description = oRow["Description"].ToString();
                    r.Payperiod.Frequency.CutOff.CutoffCount = oRow["CutOffCount"].ToString();
                    r.PayRegister.ID = Convert.ToInt64(oRow["ID"]);
                    r.Employee = Employee.GetEmployeeDetailsByID(Convert.ToInt64(oRow["EmployeeID"]));
                    r.Division.Div_Desc = oRow["Division"].ToString();
                    r.Project.ProjectName = oRow["ProjectName"].ToString();
                    r.Department.Description = oRow["Department"].ToString();
                    r.PayRegister.BasicPay = Convert.ToDecimal(oRow["BasicPay"]);
                    r.PayRegister.HoursWorked = Convert.ToDecimal(oRow["HoursWorked"]);
                    r.PayRegister.Overtime = Convert.ToDecimal(oRow["Overtime"]);
                    r.PayRegister.OvertimeAmount = Convert.ToDecimal(oRow["OvertimeAmount"]);
                    r.PayRegister.NightDiff = Convert.ToDecimal(oRow["NightDiff"]);
                    r.PayRegister.NightDiffAmount = Convert.ToDecimal(oRow["NightDiffAmount"]);
                    r.PayRegister.NightDiffOT = Convert.ToDecimal(oRow["NightDiffOT"]);
                    r.PayRegister.NightDiffOTAmount = Convert.ToDecimal(oRow["NightDiffOTAmount"]);
                    r.PayRegister.RegularHoliday = Convert.ToDecimal(oRow["RegularHoliday"]);
                    r.PayRegister.RegularHolidayAmount = Convert.ToDecimal(oRow["RegularHolidayAmount"]);
                    r.PayRegister.RegularHolidayOT = Convert.ToDecimal(oRow["RegularHolidayOT"]);
                    r.PayRegister.RegularHolidayOTAmount = Convert.ToDecimal(oRow["RegularHolidayOTAmount"]);
                    r.PayRegister.RegularHolidayNightDiff = Convert.ToDecimal(oRow["RegularHolidayNightDiff"]);
                    r.PayRegister.RegularHolidayNightDiffAmount = Convert.ToDecimal(oRow["RegularHolidayNightDiffAmount"]);
                    r.PayRegister.RegularHolidayNightDiffOT = Convert.ToDecimal(oRow["RegularHolidayNightDiffOT"]);
                    r.PayRegister.RegularHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["RegularHolidayNightDiffOTAmount"]);
                    r.PayRegister.SpecialHoliday = Convert.ToDecimal(oRow["SpecialHoliday"]);
                    r.PayRegister.SpecialHolidayAmount = Convert.ToDecimal(oRow["SpecialHolidayAmount"]);
                    r.PayRegister.SpecialHolidayOT = Convert.ToDecimal(oRow["SpecialHolidayOT"]);
                    r.PayRegister.SpecialHolidayOTAmount = Convert.ToDecimal(oRow["SpecialHolidayOTAmount"]);
                    r.PayRegister.SpecialHolidayNightDiff = Convert.ToDecimal(oRow["SpecialHolidayNightDiff"]);
                    r.PayRegister.SpecialHolidayNightDiffAmount = Convert.ToDecimal(oRow["SpecialHolidayNightDiffAmount"]);
                    r.PayRegister.SpecialHolidayNightDiffOT = Convert.ToDecimal(oRow["SpecialHolidayNightDiffOT"]);
                    r.PayRegister.SpecialHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["SpecialHolidayNightDiffOTAmount"]);
                    r.PayRegister.WDO = Convert.ToDecimal(oRow["WDO"]);
                    r.PayRegister.WDOAmount = Convert.ToDecimal(oRow["WDOAmount"]);
                    r.PayRegister.WDOOT = Convert.ToDecimal(oRow["WDOOT"]);
                    r.PayRegister.WDOOTAmount = Convert.ToDecimal(oRow["WDOOTAmount"]);
                    r.PayRegister.WDONightDiff = Convert.ToDecimal(oRow["WDONightDiff"]);
                    r.PayRegister.WDONightDiffAmount = Convert.ToDecimal(oRow["WDONightDiffAmount"]);
                    r.PayRegister.WDONightDiffOT = Convert.ToDecimal(oRow["WDONightDiffOT"]);
                    r.PayRegister.WDONightDiffOTAmount = Convert.ToDecimal(oRow["WDONightDiffOTAmount"]);
                    r.PayRegister.WDORegularHoliday = Convert.ToDecimal(oRow["WDORegularHoliday"]);
                    r.PayRegister.WDORegularHolidayAmount = Convert.ToDecimal(oRow["WDORegularHolidayAmount"]);
                    r.PayRegister.WDORegularHolidayOT = Convert.ToDecimal(oRow["WDORegularHolidayOT"]);
                    r.PayRegister.WDORegularHolidayOTAmount = Convert.ToDecimal(oRow["WDORegularHolidayOTAmount"]);
                    r.PayRegister.WDORegularHolidayNightDiff = Convert.ToDecimal(oRow["WDORegularHolidayNightDiff"]);
                    r.PayRegister.WDORegularHolidayNightDiffAmount = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffAmount"]);
                    r.PayRegister.WDORegularHolidayNightDiffOT = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffOT"]);
                    r.PayRegister.WDORegularHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["WDORegularHolidayNightDiffOTAmount"]);
                    r.PayRegister.WDOSpecialHoliday = Convert.ToDecimal(oRow["WDOSpecialHoliday"]);
                    r.PayRegister.WDOSpecialHolidayAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayAmount"]);
                    r.PayRegister.WDOSpecialHolidayOT = Convert.ToDecimal(oRow["WDOSpecialHolidayOT"]);
                    r.PayRegister.WDOSpecialHolidayOTAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayOTAmount"]);
                    r.PayRegister.WDOSpecialHolidayNightDiff = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiff"]);
                    r.PayRegister.WDOSpecialHolidayNightDiffAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffAmount"]);
                    r.PayRegister.WDOSpecialHolidayNightDiffOT = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffOT"]);
                    r.PayRegister.WDOSpecialHolidayNightDiffOTAmount = Convert.ToDecimal(oRow["WDOSpecialHolidayNightDiffOTAmount"]);
                    r.PayRegister.VLDays = Convert.ToDecimal(oRow["VLDays"]);
                    r.PayRegister.VLAmount = Convert.ToDecimal(oRow["VLAmount"]);
                    r.PayRegister.SLDays = Convert.ToDecimal(oRow["SLDays"]);
                    r.PayRegister.SLAmount = Convert.ToDecimal(oRow["SLAmount"]);
                    r.PayRegister.BLDays = Convert.ToDecimal(oRow["BLDays"]);
                    r.PayRegister.BLAmount = Convert.ToDecimal(oRow["BLAmount"]);
                    r.PayRegister.ELDays = Convert.ToDecimal(oRow["ELDays"]);
                    r.PayRegister.ELAmount = Convert.ToDecimal(oRow["ELAmount"]);
                    r.PayRegister.Meals = Convert.ToDecimal(oRow["Meals"]);
                    r.PayRegister.TaxableAllowance = Convert.ToDecimal(oRow["TaxableAllowance"]);
                    r.PayRegister.AbsentDays = Convert.ToDecimal(oRow["AbsentDays"]);
                    r.PayRegister.AbsentAmount = Convert.ToDecimal(oRow["AbsentAmount"]);
                    r.PayRegister.TotalTaxableAmount = Convert.ToDecimal(oRow["TotalTaxableAmount"]);
                    r.PayRegister.TaxAmount = Convert.ToDecimal(oRow["TaxAmount"]);
                    r.PayRegister.SSS = Convert.ToDecimal(oRow["SSS"]);
                    r.PayRegister.TardyAmount = Convert.ToDecimal(oRow["TardyAmount"]);
                    r.PayRegister.TardyDays = Convert.ToDecimal(oRow["TardyDays"]);
                    r.PayRegister.PHILHEALTH = Convert.ToDecimal(oRow["Philhealth"]);
                    r.PayRegister.PAGIBIG = Convert.ToDecimal(oRow["Pagibig"]);
                    r.PayRegister.ECOLA = Convert.ToDecimal(oRow["ECOLA"]);
                    r.PayRegister.GrossIncome = Convert.ToDecimal(oRow["GrossIncome"]);
                    r.PayRegister.NetIncome = Convert.ToDecimal(oRow["NetIncome"]);
                }
            }
            return r;
        }
        public static PaySlip GetPaySlipYTD(Int64 ID)
        {
            var r = new PaySlip();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("hris_GetPayroll_YTD_PaySlip",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { ID }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    r.YTDSalaryNet = Convert.ToDecimal(oRow["YTDSalaryNet"]);
                    r.YTDSalaryGross = Convert.ToDecimal(oRow["YTDSalaryGross"]);
                    r.YTDTotalTaxableAmount = Convert.ToDecimal(oRow["YTDTotalTaxableAmount"]);
                    r.YTDSSS = Convert.ToDecimal(oRow["YTDSSS"]);
                    r.YTDPHILHEALTH = Convert.ToDecimal(oRow["YTDPhilHealth"]);
                    r.YTDWTax = Convert.ToDecimal(oRow["YTDWTax"]);
                    r.YTDTardy = Convert.ToDecimal(oRow["YTDTardy"]);
                    r.YTDAbsent = Convert.ToDecimal(oRow["YTDABSENT"]);
                    r.YTDPAGIBIG = Convert.ToDecimal(oRow["YTDPAGIBIG"]);
                }
            }
            return r;
        }
        public static List<PaySlip> GetLoansForPayslipDetails(Int64 EmpId, Int64 PayoutID)
        {
            var _list = new List<PaySlip>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetLoans_Payslip",
                    new string[] { "eemployeeid", "ePayoutID" },
                    new DbType[] {DbType.Int64, DbType.Int64 },
                    new object[] { EmpId, PayoutID }, out x, ref oTable, CommandType.StoredProcedure);
                foreach(DataRow aRow in oTable.Rows)
                {
                    PaySlip l = new PaySlip();
                    l.Employee = new Employee();
                    l.Loan = new Loans();
                    
                    l.Employee.EmployeeID = aRow["EmpID"].ToString();
                    l.Loan.LoanType = new LoanTypes();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Loan.Balance = Convert.ToDecimal(aRow["Balance"]);
                    l.Loan.LoanType.Code = aRow["Code"].ToString();
                    l.Loan.LoanType.Description = aRow["Description"].ToString();
                    l.Loan.LoanType.ID = Convert.ToInt64(aRow["LoanTypeID"]);
                    l.Loan.Amount = Convert.ToDecimal(aRow["Amount"]);
                    l.Loan.Interest = Convert.ToDecimal(aRow["Interest"]);
                    l.Loan.Amortization = Convert.ToDecimal(aRow["Amortization"]);
                    l.Loan.TotalPayable = Convert.ToDecimal(aRow["TotalPayable"]);
                    l.Loan.LoanDate = Convert.ToDateTime(aRow["LoanDate"]);
                    l.Loan.StartPayment = Convert.ToDateTime(aRow["StartPayment"]);
                    l.Loan.Status = Convert.ToInt32(aRow["Status"]);
                    l.Loan.PaymentStatus = Convert.ToInt32(aRow["PaymentStatus"]);
                    l.Loan.isPaymentActive = Convert.ToBoolean(aRow["isPaymentActive"]);
                    l.LoanNumberOfPayments = Convert.ToInt32(aRow["NumberofPayments"]);
                    _list.Add(l) ;
                }
            }
            return _list;
        }
        public static List<PaySlip> GetAddidionForAdjustment(Int64 EmpId, Int64 PayoutID)
        {
            var _list = new List<PaySlip>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAddition_Payslip",
                      new string[] { "eemployeeid", "ePayoutID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { EmpId, PayoutID }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in oTable.Rows)
                {
                    PaySlip a = new PaySlip();
                    a.Adjustments = new Adjustments();
                    a.Adjustments.Adjustment = new BatchAdjustment();
                    a.Adjustments.Adjustment.Code = resultsRow["AdjustmentCode"].ToString();
                    a.Adjustments.Adjustment.Description = resultsRow["Description"].ToString();
                    a.Adjustments.Amount = Convert.ToDouble(resultsRow["Amount"]);
                    _list.Add(a);
                }
            }
            return _list;
        }
        public static List<PaySlip> GetDeductionForAdjustment(Int64 EmpId, Int64 PayoutID)
        {
            var _list = new List<PaySlip>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetDeductions_Payslip",
                      new string[] { "eemployeeid", "ePayoutID" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { EmpId, PayoutID }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in oTable.Rows)
                {
                    PaySlip a = new PaySlip();
                    a.Adjustments = new Adjustments();
                    a.Adjustments.Adjustment = new BatchAdjustment();
                    a.Adjustments.Adjustment.Code = resultsRow["AdjustmentCode"].ToString();
                    a.Adjustments.Adjustment.Description = resultsRow["AdjustmentDescription"].ToString();
                    a.Adjustments.Amount = Convert.ToDouble(resultsRow["Amount"]);
                    _list.Add(a);
                }
            }
            return _list;
        }

        
    }
    public class Loans
    {
        public long ID { get; set; }
        public LoanTypes LoanType { get; set; }
        public Employee Employee { get; set; }
        public decimal Amount { get; set; }
        public decimal Interest { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal Balance { get; set; }
        public decimal Amortization { get; set; }
        public Boolean isPaymentActive { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime StartPayment { get; set; }
        public int Status { get; set; }
        public int PaymentStatus { get; set; }
        public Employee UploadedBy { get; set; }
        public SystemLogs Logs { get; set; }
        /*
         * Important:
         *  -->PaymentStatus is defined by number 0(defuault, payment false) and 1(payment true).
         *  -->Status is the data status defined by number 0(deleted/replaced) and 1(default,active)
         *  -->TotalPayable, saving and updating in the backend is (loan.interest + loan.Amount)
         *  -->Balance is defined by solution loan.totalpayable - sum(loanPayments.amount)
         */
        public static string SaveLoan(Loans data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                //affected rows
                int a = 0;
                int b = 0;
                int c = 0;
                int d = 0;
                int e = 0;
                //data tables
                DataTable aTable = new DataTable();
                DataTable bTable = new DataTable();
                DataTable cTable = new DataTable();
                DataTable dTable = new DataTable();
                DataTable eTable = new DataTable();
                //get the id of loantype based on code
                //data.LoanType = new LoanTypes();
                db.ExecuteCommandReader("HRIS_GetLoanTypeIDByCode_LoanTypes",
                    new string[] { "eCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.LoanType.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    data.LoanType.ID = Convert.ToInt64(aRow["ID"]);
                }
                /*
                 * check first if payment has been made
                 * This is to prevent double entry or deduction 
                 */
                long _bId = 0;//param for returned ID if payment has been made
                if (data.LoanType.ID != 0)// true and has loan type id
                {
                    db.ExecuteCommandReader("HRIS_isPaymentMade_loans",
                        new string[] { "eEmployeeID", "eLoanType" },
                        new DbType[] { DbType.Int64, DbType.Int64 },
                        new object[] { data.Employee.ID, data.LoanType.ID }, out b, ref bTable, CommandType.StoredProcedure);
                    if (bTable.Rows.Count > 0)//if true
                    {
                        res = "Payment has been made.";
                    }
                    else //if false
                    {
                        //check if the loan exist and has a payment status of zero
                        long _cId = 0;//variable for returned loan id that is exist and had payment status of zero
                        db.ExecuteCommandReader("HRIS_isPaymentStatusIsZero_Loans",
                              new string[] { "eEmployeeId", "eLoanTypeID" },
                              new DbType[] { DbType.Int64, DbType.Int64 },
                              new object[] { data.Employee.ID, data.LoanType.ID }, out c, ref cTable, CommandType.StoredProcedure);
                        if (cTable.Rows.Count > 0)//if data exist
                        {
                            DataRow cRow = cTable.Rows[0];
                            _cId = Convert.ToInt64(cRow["ID"]);
                            db.ExecuteCommandNonQuery("HRIS_setStatusToZero_HRIS",
                                new string[] { "eId", "eEmployeeID", "eLoanTypeId" },
                                new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                                new object[] { _cId, data.Employee.ID, data.LoanType.ID }, out d, CommandType.StoredProcedure);
                            db.ExecuteCommandNonQuery("HRIS_Save_Loan",
                                new string[] { "eLoanTypeID", "eEmployeeId", "eAmount", "eInterest", "eAmortization", "eLoanDate", "eStartPayment", "eUploadedBy" },
                                new DbType[] { DbType.Int64, DbType.Int64, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.DateTime, DbType.DateTime, DbType.Int64 },
                                new object[] { data.LoanType.ID, data.Employee.ID, data.Amount, data.Interest, data.Amortization, data.LoanDate, data.StartPayment, data.UploadedBy.ID }, out e, CommandType.StoredProcedure);
                            data.Logs.After = "ID:"+data.ID.ToString()+ ", LoanType:" + data.LoanType.ID.ToString()+ ", EmployeeID:" + data.Employee.ID.ToString()+ "," +
                                                        " Amount:" + data.Amount.ToString()+ ", Interest:" + data.Interest.ToString()+ ", Amortization:" + data.Amortization.ToString()+ ", " +
                                                        "LoanDate:" + data.LoanDate.ToString("MM-dd-yyyy")+ ", ID:" + data.StartPayment.ToString("MM-dd-yyyy")+ ", UploadedBy:" + data.UploadedBy+ "";
                            SystemLogs.SaveSystemLogs(data.Logs);
                            res = "Saved sucessfully.";
                        }
                        else
                        {
                            db.ExecuteCommandNonQuery("HRIS_Save_Loan",
                               new string[] { "eLoanTypeID", "eEmployeeId", "eAmount", "eInterest", "eAmortization", "eLoanDate", "eStartPayment", "eUploadedBy" },
                               new DbType[] { DbType.Int64, DbType.Int64, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.DateTime, DbType.DateTime, DbType.Int64 },
                               new object[] { data.LoanType.ID, data.Employee.ID, data.Amount, data.Interest, data.Amortization, data.LoanDate, data.StartPayment, data.UploadedBy.ID }, out e, CommandType.StoredProcedure);
                            data.Logs.After = "ID:" + data.ID.ToString() + ", LoanType:" + data.LoanType.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + "," +
                                                        " Amount:" + data.Amount.ToString() + ", Interest:" + data.Interest.ToString() + ", Amortization:" + data.Amortization.ToString() + ", " +
                                                        "LoanDate:" + data.LoanDate.ToString("MM-dd-yyyy") + ", ID:" + data.StartPayment.ToString("MM-dd-yyyy") + ", UploadedBy:" + data.UploadedBy + "";
                            SystemLogs.SaveSystemLogs(data.Logs);
                            res = "Saved sucessfully.";
                        }
                    }
                }
                else
                {
                    res = "Loan type code is not on the list.";
                }
            }
            return res;
        }
        public static string UpdateLoans(Loans data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                //affected rows
                int a = 0;
                int b = 0;
                //datatables
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetLoanTypeIDByCode_LoanTypes",
                   new string[] { "eCode" },
                   new DbType[] { DbType.String },
                   new object[] { data.LoanType.Code }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    data.LoanType.ID = Convert.ToInt64(aRow["ID"]);
                    db.ExecuteCommandNonQuery("HRIS_Edit_Loan",
                              new string[] { "eID", "eLoanTypeID", "eEmployeeId", "eAmount", "eInterest", "eAmortization", "eLoanDate", "eStartPayment", "eUploadedBy", "eIsPaymentActive" },
                              new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Decimal, DbType.Decimal, DbType.Decimal, DbType.DateTime, DbType.DateTime, DbType.Int64, DbType.Boolean },
                              new object[] { data.ID, data.LoanType.ID, data.Employee.ID, data.Amount, data.Interest, data.Amortization, data.LoanDate, data.StartPayment, data.UploadedBy.ID, data.isPaymentActive }, out b, CommandType.StoredProcedure);
                    res = "Updated  sucessfully.";
                    data.Logs.After = "ID:" + data.ID.ToString() + ", LoanType:" + data.LoanType.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + "," +
                                                " Amount:" + data.Amount.ToString() + ", Interest:" + data.Interest.ToString() + ", Amortization:" + data.Amortization.ToString() + ", " +
                                                "LoanDate:" + data.LoanDate.ToString("MM-dd-yyyy") + ", ID:" + data.StartPayment.ToString("MM-dd-yyyy") + ", UploadedBy:" + data.UploadedBy + "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
                else
                {
                    res = "Loan type code is not on the list.";
                }
            }
            return res;
        }
        public static void DeleteLoan(Loans data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Loans",
                    new string[] { "eId", "eEmployeeId", "eUploadedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { data.ID, data.Employee.ID, data.UploadedBy.ID }, out a, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<Loans> GetLoans()
        {
            var _list = new List<Loans>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                //affected rows
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Loans",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Loans l = new Loans();
                    l.Employee = new Employee();
                    l.LoanType = new LoanTypes();
                    l.UploadedBy = new Employee();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.EmployeeID = aRow["EmpID"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Balance = Convert.ToDecimal(aRow["Balance"]);
                    l.LoanType.Code = aRow["Code"].ToString();
                    l.LoanType.Description = aRow["Description"].ToString();
                    l.LoanType.ID = Convert.ToInt64(aRow["LoanTypeID"]);
                    l.Amount = Convert.ToDecimal(aRow["Amount"]);
                    l.Interest = Convert.ToDecimal(aRow["Interest"]);
                    l.Amortization = Convert.ToDecimal(aRow["Amortization"]);
                    l.TotalPayable = Convert.ToDecimal(aRow["TotalPayable"]);
                    l.LoanDate = Convert.ToDateTime(aRow["LoanDate"]);
                    l.StartPayment = Convert.ToDateTime(aRow["StartPayment"]);
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.PaymentStatus = Convert.ToInt32(aRow["PaymentStatus"]);
                    l.isPaymentActive = Convert.ToBoolean(aRow["isPaymentActive"]);
                    _list.Add(l);
                }
            }
            return _list;
        }
        public static List<Loans> GetLoansByEmpAndLoanType(Int64 EmpId, Int64 LoanTypeId)
        {
            var _list = new List<Loans>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                //affected rows
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByEmpIdAndLoanType_Loans",
                    new string[] { "eEmployeeId", "eLoanTypeId" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { EmpId, LoanTypeId }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Loans l = new Loans();
                    l.Employee = new Employee();
                    l.LoanType = new LoanTypes();
                    l.UploadedBy = new Employee();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.EmployeeID = aRow["EmpID"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.Balance = Convert.ToDecimal(aRow["Balance"]);
                    l.LoanType.Code = aRow["Code"].ToString();
                    l.LoanType.ID = Convert.ToInt64(aRow["LoanTypeID"]);
                    l.TotalPayable = Convert.ToDecimal(aRow["TotalPayable"]);
                    l.LoanType.Description = aRow["Description"].ToString();
                    l.Amortization = Convert.ToDecimal(aRow["Amortization"]);
                    l.Amount = Convert.ToDecimal(aRow["Amount"]);
                    l.Interest = Convert.ToDecimal(aRow["Interest"]);
                    l.LoanDate = Convert.ToDateTime(aRow["LoanDate"]);
                    l.StartPayment = Convert.ToDateTime(aRow["StartPayment"]);
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.PaymentStatus = Convert.ToInt32(aRow["PaymentStatus"]);
                    l.isPaymentActive = Convert.ToBoolean(aRow["isPaymentActive"]);
                    _list.Add(l);
                }
            }
            return _list;
        }

        public static Loans GetLoansById(Int64 Id)
        {
            var l = new Loans();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetById_Loans",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    l.Employee = new Employee();
                    l.LoanType = new LoanTypes();
                    l.UploadedBy = new Employee();
                    l.ID = Convert.ToInt64(aRow["ID"]);
                    l.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    l.Employee.EmployeeID = aRow["EmpID"].ToString();
                    l.Employee.Firstname = aRow["FirstName"].ToString();
                    l.Employee.Middlename = aRow["MiddleName"].ToString();
                    l.Employee.Lastname = aRow["LastName"].ToString();
                    l.Employee.Suffix = aRow["Suffix"].ToString();
                    l.TotalPayable = Convert.ToDecimal(aRow["TotalPayable"]);
                    l.Amortization = Convert.ToDecimal(aRow["Amortization"]);
                    //.Balance = Convert.ToDecimal(aRow["Balance"]);
                    l.LoanType.Code = aRow["Code"].ToString();
                    l.LoanType.ID = Convert.ToInt64(aRow["LoanTypeID"]);
                    l.LoanType.Description = aRow["Description"].ToString();
                    l.Amount = Convert.ToDecimal(aRow["Amount"]);
                    l.Interest = Convert.ToDecimal(aRow["Interest"]);
                    l.LoanDate = Convert.ToDateTime(aRow["LoanDate"]);
                    l.StartPayment = Convert.ToDateTime(aRow["StartPayment"]);
                    l.Status = Convert.ToInt32(aRow["Status"]);
                    l.PaymentStatus = Convert.ToInt32(aRow["PaymentStatus"]);
                    l.isPaymentActive = Convert.ToBoolean(aRow["isPaymentActive"]);
                }
            }
            return l;
        }
    }
    public class Adjustments
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public PayPeriod PayPeriod { get; set; }
        public BatchAdjustment Adjustment { get; set; }
        public double Amount { get; set; }
        public Boolean Taxable { get; set; }
        public long PayoutID { get; set; }
        public Employee UploadedBy { get; set; }
        public Employee ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static string SaveNewAdjustment(Adjustments data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                int affectedRows1 = 0;
                int affectedRows2 = 0;
                DataTable resultsTable = new DataTable();
                //get the ID of the batchadjustment code
                int k = 0;
                DataTable _tyxTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAdjustmentIDByCOde_BatcAdjustmentCode",
                    new string[] { "aCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Adjustment.Code }, out k, ref _tyxTable, CommandType.StoredProcedure);
                if (_tyxTable.Rows.Count > 0)
                {
                    DataRow zRow = _tyxTable.Rows[0];
                    //the returned id will be the value of ADJUSTMENT_ID
                    data.Adjustment.ID = Convert.ToInt64(zRow["ID"]);
                }
              
                /*
                 * this first statement is to check
                 * if adjusment has been made
                 * to prevent double entry
                 */
                long _gId = 0;
                int g = 0;
                DataTable gTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetIdOfHasPayoutID_Adjustments",
                    new string[] { "eEmployeeId", "ePayPeriod", "eAdjustmentCode", "eAdjustmentId" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64 },
                    new object[] { data.Employee.ID, data.PayPeriod.ID, data.Adjustment.Code, data.Adjustment.ID }, out g, ref gTable, CommandType.StoredProcedure);
                if (gTable.Rows.Count > 0)
                {
                    DataRow gRow = gTable.Rows[0];
                    _gId = Convert.ToInt64(gRow["ID"]);
                }
                if (_gId == 0)
                {
                    //check if details exist and has no payout ID 
                    db.ExecuteCommandReader("HRIS_GetIdOfNoPayoutID_Adjustments",
                        new string[] { "eEmployeeId", "ePayPeriod", "eAdjustmentCode", "eAdjustmentId" },
                        new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int64 },
                        new object[] { data.Employee.ID, data.PayPeriod.ID, data.Adjustment.Code, data.Adjustment.ID }, out affectedRows2, ref resultsTable, CommandType.StoredProcedure);
                    if (resultsTable.Rows.Count > 0)
                    {
                        //if true, set the status to zero(0) or deleted
                        DataRow resultsRow = resultsTable.Rows[0];
                        long _adjId = Convert.ToInt64(resultsRow["ID"]);
                        db.ExecuteCommandNonQuery("HRIS_UpdateStatusNoPayOutId_Adjustments",
                            new string[] { "eID" },
                            new DbType[] { DbType.Int64 },
                            new object[] { _adjId }, out affectedRows1, CommandType.StoredProcedure);
                        //then save the new details
                        db.ExecuteCommandNonQuery("HRIS_SaveDetails_Adjustments",
                          new string[] {   "ePayPeriod", "eEmployeeId",  "eAmount", "eAdjustmentCode",
                                       "eTaxable","eAdjustmentId","eUploadedBy"},
                          new DbType[] {   DbType.Int64, DbType.Int64, DbType.Double, DbType.String,
                                       DbType.Boolean, DbType.Int64,  DbType.Int64},
                          new object[] {   data.PayPeriod.ID, data.Employee.ID,data.Amount, data.Adjustment.Code,
                                       data.Taxable, data.Adjustment.ID, data.UploadedBy.ID}, out affectedRows, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", PayPeriodID:" + data.PayPeriod.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + ", Amount:" + data.Amount.ToString() + "," +
                            "AdjustmentCode:" + data.Adjustment.ID.ToString() + ",UploadedBy:" + data.UploadedBy.ID.ToString() + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "ok";
                    }
                    else
                    {
                        //if false, save the new details
                        db.ExecuteCommandNonQuery("HRIS_SaveDetails_Adjustments",
                           new string[] {   "ePayPeriod", "eEmployeeId",  "eAmount", "eAdjustmentCode",
                                        "eTaxable","eAdjustmentId","eUploadedBy"},
                           new DbType[] {   DbType.Int64, DbType.Int64, DbType.Double, DbType.String,
                                        DbType.Boolean, DbType.Int64,  DbType.Int64},
                           new object[] {   data.PayPeriod.ID, data.Employee.ID,data.Amount, data.Adjustment.Code,
                                        data.Taxable, data.Adjustment.ID, data.UploadedBy.ID}, out affectedRows, CommandType.StoredProcedure);
                        data.Logs.After = "ID:" + data.ID.ToString() + ", PayPeriodID:" + data.PayPeriod.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + ", Amount:" + data.Amount.ToString() + "," +
                            "AdjustmentCode:" + data.Adjustment.ID.ToString() + ",UploadedBy:" + data.UploadedBy.ID.ToString() + "";
                        SystemLogs.SaveSystemLogs(data.Logs);
                        res = "ok";
                    }

                }
                else
                {
                    res = "Adjustment has been made.";
                }

            }
            return res;
        }
        public static void UpdateAdjustment(Adjustments data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                int k = 0;
                DataTable _tyxTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAdjustmentIDByCOde_BatcAdjustmentCode",
                    new string[] { "aCode" },
                    new DbType[] { DbType.String },
                    new object[] { data.Adjustment.Code }, out k, ref _tyxTable, CommandType.StoredProcedure);
                foreach (DataRow zRow in _tyxTable.Rows)
                {
                    //the returned id will be the vlue of ADJUSTMENT_ID
                    data.Adjustment.ID = Convert.ToInt64(zRow["ID"]);
                }
                db.ExecuteCommandNonQuery("HRIS_Upate_Adjustments",
                   new string[] {   "eId", "ePayPeriod", "eEmployeeId",  "eAmount", "eAdjustmentCode",
                                    "eTaxable","eAdjustmentId","eModifiedBy"},
                   new DbType[] {   DbType.Int64, DbType.Int64, DbType.Int64, DbType.Double, DbType.String,
                                    DbType.Boolean, DbType.Int64,  DbType.Int64},
                   new object[] {   data.ID, data.PayPeriod.ID, data.Employee.ID,data.Amount, data.Adjustment.Code,
                                    data.Taxable, data.Adjustment.ID, data.ModifiedBy.ID}, out affectedRows, CommandType.StoredProcedure);
                data.Logs.After = "ID:" + data.ID.ToString() + ", PayPeriodID:" + data.PayPeriod.ID.ToString() + ", EmployeeID:" + data.Employee.ID.ToString() + ", Amount:" + data.Amount.ToString() + "," +
                             "AdjustmentCode:" + data.Adjustment.ID.ToString() + ";";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeleteAdjustment(Adjustments data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Adjustment",
                    new string[] { "eId", "eEmployeeId", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.Int64 },
                    new object[] { data.ID, data.Employee.ID, data.ModifiedBy.ID }, out a, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<Adjustments> GetAdjustmentsByPayPeriod(Int64 PayPeriodId)
        {
            var _list = new List<Adjustments>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByPayPeriod_Adjustments",
                    new string[] { "ePayPeriod" },
                    new DbType[] { DbType.Int64 },
                    new object[] { PayPeriodId }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in resultsTable.Rows)
                {
                    Adjustments a = new Adjustments();
                    a.Employee = new Employee();
                    a.PayPeriod = new PayPeriod();
                    a.Adjustment = new BatchAdjustment();
                    a.ID = Convert.ToInt64(resultsRow["ID"]);
                    a.Employee.ID = Convert.ToInt64(resultsRow["EmpId"]);
                    a.Employee.EmployeeID = resultsRow["EmployeeID"].ToString();
                    a.Employee.Firstname = resultsRow["FirstName"].ToString();
                    a.Employee.Middlename = resultsRow["MiddleName"].ToString();
                    a.Employee.Lastname = resultsRow["LastName"].ToString();
                    a.Employee.Suffix = resultsRow["Suffix"].ToString();
                    a.Adjustment.Code = resultsRow["AdjustmentCode"].ToString();
                    a.Adjustment.Description = resultsRow["AdjustmentDescription"].ToString();
                    a.Amount = Convert.ToDouble(resultsRow["Amount"]);
                    a.Taxable = Convert.ToBoolean(resultsRow["Taxable"]);
                    a.PayPeriod = PayPeriod.GetPayPeriodByID(Convert.ToInt64(resultsRow["PayPeriod"]));
                    _list.Add(a);
                }
            }
            return _list;
        }
        public static List<Adjustments> GetAdjustments()
        {
            var _list = new List<Adjustments>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Adjustments",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in resultsTable.Rows)
                {
                    Adjustments a = new Adjustments();
                    a.Employee = new Employee();
                    a.PayPeriod = new PayPeriod();
                    a.Adjustment = new BatchAdjustment();
                    a.ID = Convert.ToInt64(resultsRow["ID"]);
                    a.Employee.ID = Convert.ToInt64(resultsRow["EmpId"]);
                    a.Employee.EmployeeID = resultsRow["EmployeeID"].ToString();
                    a.Employee.Firstname = resultsRow["FirstName"].ToString();
                    a.Employee.Middlename = resultsRow["MiddleName"].ToString();
                    a.Employee.Lastname = resultsRow["LastName"].ToString();
                    a.Employee.Suffix = resultsRow["Suffix"].ToString();
                    a.Adjustment.Code = resultsRow["AdjustmentCode"].ToString();
                    a.Adjustment.Description = resultsRow["AdjustmentDescription"].ToString();
                    a.PayoutID = Convert.ToInt64(resultsRow["PayOutID"]);
                    a.Amount = Convert.ToDouble(resultsRow["Amount"]);
                    a.Taxable = Convert.ToBoolean(resultsRow["Taxable"]);
                    a.PayPeriod = PayPeriod.GetPayPeriodByID(Convert.ToInt64(resultsRow["PayPeriod"]));
                    _list.Add(a);
                }
            }
            return _list;
        }
        public static Adjustments GetAdjustmentsByID(Int64 Id)
        {
            var a = new Adjustments();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetById_Adjustments",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                if (resultsTable.Rows.Count > 0)
                {
                    DataRow resultsRow = resultsTable.Rows[0];
                    a.Employee = new Employee();
                    a.PayPeriod = new PayPeriod();
                    a.Adjustment = new BatchAdjustment();
                    a.ID = Convert.ToInt64(resultsRow["ID"]);
                    a.Employee.ID = Convert.ToInt64(resultsRow["EmpId"]);
                    a.Employee.EmployeeID = resultsRow["EmployeeID"].ToString();
                    a.Employee.Firstname = resultsRow["FirstName"].ToString();
                    a.Employee.Middlename = resultsRow["MiddleName"].ToString();
                    a.Employee.Lastname = resultsRow["LastName"].ToString();
                    a.Employee.Suffix = resultsRow["Suffix"].ToString();
                    a.Adjustment.Code = resultsRow["AdjustmentCode"].ToString();
                    a.Adjustment.Description = resultsRow["AdjustmentDescription"].ToString();
                    a.Amount = Convert.ToDouble(resultsRow["Amount"]);
                    a.PayoutID = Convert.ToInt64(resultsRow["PayOutID"]);
                    a.Taxable = Convert.ToBoolean(resultsRow["Taxable"]);
                    a.PayPeriod = PayPeriod.GetPayPeriodByID(Convert.ToInt64(resultsRow["PayPeriod"]));
                }
            }
            return a;
        }
        public static List<Adjustments> GetAdjustmentsByEmpIdAndPayPeriod(Int64 EmpId, Int64 PayPeriod)
        {
            var _list = new List<Adjustments>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByEmpIdAndPayPeriod_Adjustments",
                    new string[] { "eEmployeeId", "ePayPeriod" },
                    new DbType[] { DbType.Int64, DbType.Int64 },
                    new object[] { EmpId, PayPeriod }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                foreach (DataRow resultsRow in resultsTable.Rows)
                {
                    Adjustments a = new Adjustments();
                    a.Employee = new Employee();
                    a.PayPeriod = new PayPeriod();
                    a.Adjustment = new BatchAdjustment();
                    a.ID = Convert.ToInt64(resultsRow["ID"]);
                    a.Employee.ID = Convert.ToInt64(resultsRow["EmpId"]);
                    a.Employee.EmployeeID = resultsRow["EmployeeID"].ToString();
                    a.Employee.Firstname = resultsRow["FirstName"].ToString();
                    a.Employee.Middlename = resultsRow["MiddleName"].ToString();
                    a.Employee.Lastname = resultsRow["LastName"].ToString();
                    a.Employee.Suffix = resultsRow["Suffix"].ToString();
                    a.Adjustment.Code = resultsRow["AdjustmentCode"].ToString();
                    a.Adjustment.Description = resultsRow["AdjustmentDescription"].ToString();
                    a.PayoutID = Convert.ToInt64(resultsRow["PayOutID"]);
                    a.Amount = Convert.ToDouble(resultsRow["Amount"]);
                    a.Taxable = Convert.ToBoolean(resultsRow["Taxable"]);
                    a.PayPeriod = OnePhp.HRIS.Core.Model.PayPeriod.GetPayPeriodByID(Convert.ToInt64(resultsRow["PayPeriod"]));
                    _list.Add(a);
                }
            }
            return _list;
        }
    }
}
