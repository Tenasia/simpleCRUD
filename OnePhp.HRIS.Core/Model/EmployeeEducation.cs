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
    public class EmployeeEducation
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string CourseStudy { get; set; }
        public string Name { get; set; }
        public string Honors { get; set; }
        public int StartYearAttended { get; set; }
        public int EndYearAttended { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public EducationType Type { get; set; }
        public SystemLogs Logs { get; set; }
        public static List<EducationType> GetTypes()
        {
            var list = new List<EducationType>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetRef_Education",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EducationType e = new EducationType();
                    e.ID = Convert.ToInt32(aRow["ID"]);
                    e.Name = aRow["Name"].ToString();
                    list.Add(e);
                }
            }
            return list;
        }
        public static void SaveEducation(EmployeeEducation data, string[] Name, int[] Type, string[] SYear, string[] EYear, string[] Honor, string[] Course)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteData_Education",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64},
                    new object[] { data.Employee.ID }, out a, CommandType.StoredProcedure);
                int x = 0;
                foreach (string j in Name)
                {

                    data.Name = "";
                    data.Name = Name[x];
                    data.Type = new EducationType();
                    data.Type.ID = 0;
                    data.Type.ID = Type[x];
                    data.StartYearAttended = 0;
                    if (SYear[x] != "")
                    {
                        data.StartYearAttended +=Convert.ToInt32(SYear[x]);
                    }
                    data.EndYearAttended = 0;
                    if (EYear[x] != "")
                    {
                        data.EndYearAttended += Convert.ToInt32(EYear[x]);
                    }
                    data.Honors = Honor[x];
                    data.CourseStudy = Course[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_Education",
                       new string[] { "eEmployeeID", "eType", "eName", "eYearAttendedStart", "eYearAttendedEnd", "eHonorsReceived", "eCourse", "eAddedBy", "eModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.Int32, DbType.String, DbType.Int32, DbType.Int32, DbType.String, DbType.String, DbType.String, DbType.String, },
                       new object[] { data.Employee.ID, data.Type.ID, data.Name, data.StartYearAttended, data.EndYearAttended, data.Honors, data.CourseStudy, data.AddedBy, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                   data.Logs.After +="[EmployeeID:"+data.Employee.ID+ ", Type:" + data.Type.ID + ", StartYearAttended:" + data.StartYearAttended + ", " +
                        "Honors:" + data.Honors + ",  CourseStudy:" + data.Honors + "], ";
                    x += 1;
                }
                data.Logs.Module = 5;
               
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
        public static void DeleteEducation(EmployeeEducation data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Education",
                    new string[] { "eID", "eEmployeeID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Employee.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<EmployeeEducation> GetByEmployeeID(Int64 Id)
        {
            var list = new List<EmployeeEducation>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Education",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id}, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeEducation e = new EmployeeEducation();
                    e.Employee = new Employee();
                    e.Type = new EducationType();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.Name = aRow["Name"].ToString();
                    e.Type.ID = Convert.ToInt32(aRow["TypeID"]);
                    e.Type.Name = aRow["TypeName"].ToString();
                    e.StartYearAttended = Convert.ToInt32(aRow["StartYearAttended"]);
                    e.EndYearAttended = Convert.ToInt32(aRow["EndYearAttended"]);
                    e.Honors = aRow["HonorsReceived"].ToString();
                    e.CourseStudy = aRow["Course"].ToString();
                    list.Add(e);
                }
            }
            return list;
        }
        public static EmployeeEducation GetByID(Int64 Id)
        {
            var e = new EmployeeEducation();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Education",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Employee = new Employee();
                    e.Type = new EducationType();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.Name = aRow["Name"].ToString();
                    e.Type.ID = Convert.ToInt32(aRow["TypeID"]);
                    e.Type.Name = aRow["TypeName"].ToString();
                    e.StartYearAttended = Convert.ToInt32(aRow["StartYearAttended"]);
                    e.EndYearAttended = Convert.ToInt32(aRow["EndYearAttended"]);
                    e.Honors = aRow["HonorsReceived"].ToString();
                    e.CourseStudy = aRow["Course"].ToString();
                }
            }
            return e;
        }
    }
    public class EducationType
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
    public class EmployeeLicense
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public int Name { get; set; }
        public string Number { get; set; }
        public DateTime DateIssued { get; set; }
        public int Type { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void SaveLicense(EmployeeLicense data,int[]LName, string[] LNumber, DateTime[] LDateIssued, int[] LType)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteData_DriversLicense",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64},
                    new object[] { data.Employee.ID }, out a, CommandType.StoredProcedure);
                int x = 0;
                string _after = "";
                foreach (int i in LType)
                {
                    data.Number = "";
                    data.Number += LNumber[x];
                    data.DateIssued = LDateIssued[x];
                    data.Type = LType[x];
                    data.Name = LName[x];
                    db.ExecuteCommandNonQuery("HRIS_Save_DriversLicense",
                     new string[] { "eEmpID","eName", "eNumber", "eDateIssued", "eLicenseType", "eAddedBy", "eModifiedBy" },
                     new DbType[] { DbType.Int64,DbType.Int32, DbType.String, DbType.Date, DbType.Int32, DbType.String, DbType.String },
                     new object[] { data.Employee.ID, data.Name, data.Number, data.DateIssued, data.Type, data.AddedBy, data.ModifiedBy }, out b, CommandType.StoredProcedure);

                    _after +=   "[ID: " + data.ID.ToString() + ", EmployeeID: " + data.Employee.ID + ", Name: " + data.Name + ", Number: " + data.Number + "," +
                                    " DateIssued: " + data.DateIssued.ToString("MM-dd-yyyy") + ",  Type: " + data.Type.ToString() + "], ";
                    x += 1;
                }
                data.Logs.After = _after;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static void DeleteLicense(EmployeeLicense data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_DriversLicense",
                    new string[] { "eID", "eEmployeeID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Employee.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<EmployeeLicense> GetByEmployeeID(Int64 Id)
        {
            var list = new List<EmployeeLicense>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_DriversLicense",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64},
                    new object[] { Id },out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeLicense e = new EmployeeLicense();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Name = Convert.ToInt32(aRow["Name"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.Number = aRow["Number"].ToString();
                    e.DateIssued = Convert.ToDateTime(aRow["DateIssued"]);
                    e.Type = Convert.ToInt32(aRow["LicenseType"]);
                    list.Add(e);
                }
            }
            return list;
        }
        public static EmployeeLicense GetByID(Int64 Id)
        {
            var e = new EmployeeLicense();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_DriversLicense",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Name = Convert.ToInt32(aRow["Name"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.Number = aRow["Number"].ToString();
                    e.DateIssued = Convert.ToDateTime(aRow["DateIssued"]);
                    e.Type = Convert.ToInt32(aRow["LicenseType"]);
                }
            }
            return e;
        }
    }
    public class EmployeeCertifications
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string File { get; set; }
        public DateTime TrainingBondStart { get; set; }
        public DateTime TrainingBondEnd { get; set; }
        public DateTime CertificationDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public double AmountTraining { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Employee Employee { get; set; }
        public SystemLogs Logs { get; set; }

        public static void SaveCertifications(EmployeeCertifications data, string[] Title, string[] Duration, string[] TrainingBondStart,string[] TrainingBondEnd, string[] CertificationDate, string[] ValidityDate, string[] AmountTraining)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteAll_Certifications",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID }, out a, CommandType.StoredProcedure);
                int x = 0;
                string _after = "";
                foreach (string v in Title)
                {
                    data.Title = Title[x];
                    data.Duration =Convert.ToInt32(Duration[x]) ;
                    data.TrainingBondEnd =Convert.ToDateTime(TrainingBondEnd[x]) ;
                    data.TrainingBondStart = Convert.ToDateTime(TrainingBondStart[x]) ;
                    data.AmountTraining = Convert.ToDouble(AmountTraining[x]);
                    data.CertificationDate = Convert.ToDateTime(CertificationDate[x]);
                    data.ValidityDate = Convert.ToDateTime(ValidityDate[x]);
                    db.ExecuteCommandNonQuery("HRIS_Save_Certifications",
                    new string[] { "eEmpID", "eTitle", "eDuration","eTrainingBondStart", "eTrainingBondEnd","eCertificationDate","eValidityDate", "eAmountTraning", "eAddedBy", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String, DbType.Int32, DbType.Date, DbType.Date, DbType.Date, DbType.Date, DbType.Double, DbType.String, DbType.String },
                    new object[] { data.Employee.ID, data.Title, data.Duration, data.TrainingBondStart, data.TrainingBondEnd, data.CertificationDate,data.ValidityDate, data.AmountTraining, data.AddedBy, data.ModifiedBy }, out b,CommandType.StoredProcedure);

                    _after += "[ ID:" + data.ID.ToString() + ",  EmployeeID:" + data.Employee.ID.ToString() + ", Title:" + data.Title + ", " +
                                    "Duation:" + data.Duration + ", TrainingBondStart:" + data.TrainingBondStart.ToString("MM/dd/yyyy") + ", TrainingBondEnd:" + data.TrainingBondEnd.ToString("MM/dd/yyyy") + ", " +
                                    "CertificationDate:"+data.CertificationDate.ToString("MM/dd/yyyy")+ ", ValidityDate:" + data.ValidityDate.ToString("MM/dd/yyyy") + ",  AmountTraining:" + data.AmountTraining.ToString() + ",  ],";
                    x += 1;
                }
                data.Logs.After = _after;
                data.Logs.Module = 6;
                SystemLogs.SaveSystemLogs(data.Logs);
                var ss = new SystemLogs();
                ss.Type = 4;
                ss.Module = 6;
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
                ss.Before = "EmployeeID: " + data.Employee.ID + ", Status :"+ _setStat+"";
                ss.After = "EmployeeID: "+data.Employee.ID+", Status :Draft";
                ss.UserID = data.Logs.UserID;
                ss.UserName = data.Logs.UserName;
                SystemLogs.SaveSystemLogs(ss);

            }
        }
        public static void DeleteCertifications(EmployeeCertifications data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Certifications",
                    new string[] { "eID", "eEmpID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64,DbType.String },
                    new object[] { data.ID, data.Employee.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                data.Logs.Module = 5;
                data.Logs.Type = 3;
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<EmployeeCertifications> GetCertifications(Int64 Id)
        {
            var list = new List<EmployeeCertifications>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Certifications",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeCertifications e = new EmployeeCertifications();
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.Title = aRow["Title"].ToString();
                    e.Duration = Convert.ToInt32(aRow["Duration"]);
                    e.TrainingBondEnd = Convert.ToDateTime(aRow["TrainingBondEnd"]);
                    e.TrainingBondStart = Convert.ToDateTime(aRow["TrainingBondStart"]);
                    e.CertificationDate = Convert.ToDateTime(aRow["CertificationDate"]);
                    e.ValidityDate = Convert.ToDateTime(aRow["ValidityDate"]);
                    e.AmountTraining = Convert.ToDouble(aRow["AmountTraining"]);
                    list.Add(e);
                }
            }
            return list;
        }
        public static EmployeeCertifications GetByID(Int64 Id)
        {
            var e = new EmployeeCertifications();
            //
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Certifications",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.Title = aRow["Title"].ToString();
                    e.Duration = Convert.ToInt32(aRow["Duration"]);
                    e.TrainingBondEnd = Convert.ToDateTime(aRow["TrainingBondEnd"]);
                    e.TrainingBondStart = Convert.ToDateTime(aRow["TrainingBondStart"]);
                    e.CertificationDate = Convert.ToDateTime(aRow["CertificationDate"]);
                    e.ValidityDate = Convert.ToDateTime(aRow["ValidityDate"]);
                    e.AmountTraining = Convert.ToDouble(aRow["AmountTraining"]);
                }
            }
            return e;
        }

    }
}

