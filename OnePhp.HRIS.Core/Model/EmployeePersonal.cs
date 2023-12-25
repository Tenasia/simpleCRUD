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
    public class EmployeePersonal
    {
        
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Relatives { get; set; }
        public string Religion { get; set; }
        public string Relation { get; set; }
        public RefMaritalStatus MaritalStatus { get; set; }
        public string MobileNumber { get; set; }
        public string SpouseName { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public string MotherMiddleName { get; set; }
        public string MotherOccupation { get; set; }
        public string FatherFirstName { get; set; }
        public string FatherLastName { get; set; }
        public string FatherMiddleName { get; set; }
        public string FatherOccupation { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public BloodType BloodType { get; set; }
        public DateTime DateOfMedical { get; set; }
        public string MedicalClassification { get; set; }
        public string BuildingNumber { get; set; }
        public BarangayAutocompleteJSON Barangay { get; set; }
        public string StreetName { get; set; }
        public string Area { get; set; }
        public City City { get; set; }
        public string PostalCode { get; set; }
        public Province Province { get; set; }
        public int YearsOfResidency { get; set; }
        public string PBuildingNumber { get; set; }
        public BarangayAutocompleteJSON PBarangay { get; set; }
        public string PStreetName { get; set; }
        public string PArea { get; set; }
        public City PCity { get; set; }
        public string PPostalCode { get; set; }
        public Province PProvince { get; set; }
        public int PYearsOfResidency { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        

        public static void Save(EmployeePersonal data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                //Check if user has personal data
                db.ExecuteCommandReader("HRIS_isEmployeeHasPersonalInfo_Employee",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    //if yes, then update
                    db.ExecuteCommandNonQuery("HRIS_Edit_Personal",
                        new string[] { "eID", "eEmployeeID", "eGender", "eMaritalStatus", "eEmail","eMobileNumber","eRelation", "eSpouseName",
                                        "eReligion", "eDateOfBirth", "eBirthPlace", "eBloodType", "eDateOfMedical", "eMedClassification", "eBuildingNumber",
                                       "eStreetName", "eBarangay", "eCity", "ePostalCode", "eProvince", "eYearsOfResidency", "ePBuildingNumber", "ePStreetName", "ePBarangay", "ePCity",
                                       "ePPostalCode", "ePProvince", "ePYearsOfResidency", "eModifiedBy"},
                        new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Int32, DbType.String, DbType.String,   DbType.String,DbType.String,
                                        DbType.String,DbType.Date,DbType.String,DbType.Int32,DbType.Date,DbType.String,DbType.String,
                                        DbType.String,DbType.String,DbType.Int32,DbType.String,DbType.Int32,DbType.Int32,DbType.String,DbType.String,DbType.Int64,DbType.Int64,
                                        DbType.String,DbType.Int64,DbType.Int64,DbType.String},
                        new object[] {  data.ID, data.Employee.ID,data.Gender, data.MaritalStatus.ID, data.Email,data.MobileNumber, data.Relation,data.SpouseName,
                                        data.Religion, data.DateOfBirth,data.BirthPlace,data.BloodType.ID,data.DateOfMedical,data.MedicalClassification,data.BuildingNumber,
                                        data.StreetName, data.Barangay.Id,data.City.ID,data.PostalCode,data.Province.ID,data.YearsOfResidency,data.PBuildingNumber,data.PStreetName,data.PBarangay.Id,data.PCity.ID,
                                        data.PPostalCode,data.PProvince.ID,data.PYearsOfResidency,data.ModifiedBy}, out b, CommandType.StoredProcedure);
                    data.Logs.Type = 2;
                   
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
                else
                {
                    //if no, then save new
                    db.ExecuteCommandNonQuery("HRIS_Save_EmployeePersonal",
                       new string[] { "eEmployeeID", "eGender", "eMaritalStatus","eEmail","eMobileNumber",  "eRelation", "eSpouseName",
                                        "eMFirstName","eMLastName", "eMMiddleName", "eMOccupation",
                                       "eFFirstName", "eFLastName", "eFMiddleName", "eFOccupation",
                                        "eReligion", "eDateOfBirth", "eBirthPlace", "eBloodType", "eDateOfMedical", "eMedClassification", "eBuildingNumber",
                                       "eStreetName", "eBarangay", "eCity", "ePostalCode", "eProvince", "eYearsOfResidency", "ePBuildingNumber", "ePStreetName", "ePBarangay", "ePCity",
                                       "ePPostalCode", "ePProvince", "ePYearsOfResidency", "eAddedBy", "eModifiedBy"},
                       new DbType[] {   DbType.Int64,DbType.String, DbType.Int32, DbType.String, DbType.String,  DbType.String, DbType.String,
                                        DbType.String,DbType.String,DbType.String,DbType.String,
                                         DbType.String,DbType.String,DbType.String,DbType.String,
                                        DbType.String,DbType.Date,DbType.String,DbType.Int32,DbType.Date,DbType.String,DbType.String,
                                        DbType.String,DbType.Int64,DbType.Int64,DbType.String,DbType.Int32,DbType.Int32,DbType.String,DbType.String,DbType.Int64,DbType.Int64,
                                        DbType.String,DbType.Int32,DbType.Int32,DbType.String,DbType.String },
                       new object[] {   data.Employee.ID,data.Gender, data.MaritalStatus.ID, data.Email,data.MobileNumber,data.Relation,data.SpouseName,
                                        data.MotherFirstName,data.MotherLastName,data.MotherMiddleName,data.MotherOccupation,
                                        data.FatherFirstName,data.FatherLastName,data.FatherMiddleName,data.FatherOccupation,
                                        data.Religion,data.DateOfBirth,data.BirthPlace,data.BloodType.ID,data.DateOfMedical,data.MedicalClassification,data.BuildingNumber,
                                        data.StreetName, data.Barangay.Id,data.City.ID,data.PostalCode,data.Province.ID,data.YearsOfResidency,data.PBuildingNumber,data.PStreetName,data.PBarangay.Id,data.PCity.ID,
                                        data.PPostalCode,data.PProvince.ID,data.PYearsOfResidency,data.AddedBy,data.ModifiedBy}, out c, CommandType.StoredProcedure);
                    data.Logs.Type = 1;
                    data.Logs.Before = "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
            }
        }
        //Use this in file upload
        public static void SaveEmployeePersonal(EmployeePersonal data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int c = 0;
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isEmployeeHasPersonalInfo_Checker",
                new string[] { "eID" },
                new DbType[] { DbType.String },
                new object[] { data.Employee.EmployeeID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    //Details will not saved.
                }
                else
                {
                    
                    db.ExecuteCommandNonQuery("HRIS_Save_EmployeePersonal",
                                    new string[] {    "eEmployeeID", "eGender", "eMaritalStatus", "eEmail","eMobileNumber", "eRelative", "eRelation", "eSpouseName", "eMFirstName", "eMLastName", "eMMiddleName", "eMOccupation",
                                                       "eFFirstName", "eFLastName", "eFMiddleName", "eFOccupation","eReligion", "eDateOfBirth", "eBirthPlace", "eBloodType", "eDateOfMedical", "eMedClassification", "eBuildingNumber",
                                                       "eStreetName", "eBarangay", "eCity", "ePostalCode", "eProvince", "eYearsOfResidency", "ePBuildingNumber", "ePStreetName", "ePBarangay", "ePCity",
                                                       "ePPostalCode", "ePProvince", "ePYearsOfResidency", "eAddedBy", "eModifiedBy"},
                                    new DbType[] {     DbType.Int64,DbType.String, DbType.Int32, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String,
                                                        DbType.String,DbType.String,DbType.String,DbType.String,DbType.String,DbType.Date,DbType.String,DbType.Int32,DbType.Date,DbType.String,DbType.String,
                                                        DbType.String,DbType.Int64,DbType.Int64,DbType.String,DbType.Int64,DbType.Int32,DbType.String,DbType.String,DbType.Int64,DbType.Int64,
                                                        DbType.String,DbType.Int64,DbType.Int32,DbType.String,DbType.String },
                                    new object[] {     data.Employee.EmployeeID,data.Gender,data.MaritalStatus.ID, data.Email,data.MobileNumber,data.Relatives, data.Relation,data.SpouseName,data.MotherFirstName,data.MotherLastName,data.MotherMiddleName,data.MotherOccupation,
                                                        data.FatherFirstName,data.FatherLastName,data.FatherMiddleName,data.FatherOccupation,data.Religion,data.DateOfBirth,data.BirthPlace,data.BloodType.ID,data.DateOfMedical,data.MedicalClassification,data.BuildingNumber,
                                                        data.StreetName, data.Barangay.Id,data.City.ID,data.PostalCode,data.Province.ID,data.YearsOfResidency,data.PBuildingNumber,data.PStreetName,data.PBarangay.Id,data.PCity.ID,
                                                        data.PPostalCode,data.PProvince.ID,data.PYearsOfResidency,data.AddedBy,data.ModifiedBy}, out c, CommandType.StoredProcedure);
                    
                    data.Logs.Type = 1;
                    data.Logs.Before = "";
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
            }
        }
        public static void Delete(EmployeePersonal data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Personal",
                    new string[] { "eID", "eEmployeeID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Employee.ID, data.ModifiedBy }, out a, CommandType.StoredProcedure);
            }
        }
        public static List<RefMaritalStatus> GetMaritalStatus()
        {
            var list = new List<RefMaritalStatus>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Marital",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    RefMaritalStatus r = new RefMaritalStatus();
                    r.ID = Convert.ToInt32(aRow["ID"]);
                    r.Name = aRow["Name"].ToString();
                    list.Add(r);
                }
            }
            return list;
        }
        public static List<ProvinceAutocompleteJSON> GetProvinces(string query)
        {
            var res = new List<ProvinceAutocompleteJSON>();
            using (AppDb db = new AppDb())
            {

                string q = query;
                if (query == null || query.Length < 3)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();

                db.ExecuteCommandReader("HRIS_GET_PROVINCES",
                    new string[] { "q" },
                    new DbType[] { DbType.String },
                    new object[] { q }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    ProvinceAutocompleteJSON pc = new ProvinceAutocompleteJSON();
                    pc.Id = Convert.ToInt64(oRow["ID"]);
                    pc.Text = oRow["Name"].ToString();
                    pc.Code = oRow["Code"].ToString();
                    res.Add(pc);
                }
            }
            return res;
        }
        public static List<Province> GetProvincesListByRegCode(string RegCode)
        {
            var list = new List<Province>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_APIREGCODE_PROVINCE",
                    new string[] { "eRegCode" },
                    new DbType[] { DbType.String },
                    new object[] { RegCode }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Province c = new Province();
                    c.ID = Convert.ToInt32(aRow["ID"]);
                    c.Name = aRow["provDesc"].ToString();
                    c.Code = aRow["provCode"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public static List<Province> GetProvincesList()
        {
            var list = new List<Province>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_API_PROVINCES",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Province c = new Province();
                    c.ID = Convert.ToInt32(aRow["ID"]);
                    c.Name = aRow["provDesc"].ToString();
                    c.Code = aRow["provCode"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public static List<Province> GetRegionList()
        {
            var list = new List<Province>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_API_REGIONS",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    Province c = new Province();
                    c.ID = Convert.ToInt32(aRow["ID"]);
                    c.Name = aRow["regDesc"].ToString();
                    c.Code = aRow["regCode"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public static List<City> GetCity(string provCode)
        {
            var list = new List<City>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_City",
                    new string[] { "provCodes" },
                    new DbType[] { DbType.String },
                    new object[] { provCode }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    City c = new City();
                    c.ID = Convert.ToInt32(aRow["ID"]);
                    c.Name = aRow["Name"].ToString();
                   c.Code = aRow["Code"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public static List<CityAutocompleteJSON> AutoCompleteCityMun(string query)
        {
            var res = new List<CityAutocompleteJSON>();
            using (AppDb db = new AppDb())
            {
                string q = query;
                if (query == null || query.Length < 1)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();

                db.ExecuteCommandReader("HRIS_AutoComplete_Ref_City",
                    new string[] { "q" },
                    new DbType[] { DbType.String },
                    new object[] { q }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    CityAutocompleteJSON pc = new CityAutocompleteJSON();
                    pc.Id = Convert.ToInt64(oRow["ID"]);
                    pc.Text = oRow["Name"].ToString();
                    res.Add(pc);
                }
            }
            return res;
        }
        public static List<BARANGAY> GetBarangay(string cityCode)
        {
            var list = new List<BARANGAY>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_BRNGY",
                    new string[] { "cityCode" },
                    new DbType[] { DbType.String },
                    new object[] { cityCode }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BARANGAY c = new BARANGAY();
                    c.ID = Convert.ToInt32(aRow["ID"]);
                    c.Name = aRow["Name"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
        public static List<BarangayAutocompleteJSON> BarangayAutoComplete(string query)
        {
            var res = new List<BarangayAutocompleteJSON>();
            using (AppDb db = new AppDb())
            {
                string q = query;
                if (query == null || query.Length < 3)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();

                db.ExecuteCommandReader("HRIS_GET_Barangay",
                    new string[] { "q" },
                    new DbType[] { DbType.String, DbType.String },
                    new object[] { q }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    BarangayAutocompleteJSON pc = new BarangayAutocompleteJSON();
                    pc.Id = Convert.ToInt64(oRow["ID"]);
                    pc.Text = oRow["Name"].ToString();

                    res.Add(pc);
                }
            }
            return res;
        }
        public static List<BloodType> GetBloodType()
        {
            var list = new List<BloodType>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_BloodType",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    BloodType b = new BloodType();
                    b.ID = Convert.ToInt32(aRow["ID"]);
                    b.Name = aRow["Name"].ToString();
                    list.Add(b);
                }
            }
            return list;
        }
        public static List<CityAutocompleteJSON> CityAutoComplete(string query,string ProvCode)
        {
            var res = new List<CityAutocompleteJSON>();
            using (AppDb db = new AppDb())
            {
                string q = query;
                if (query == null || query.Length < 3)
                {
                    return res;
                }
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();

                db.ExecuteCommandReader("HRIS_AutoComplete_City",
                    new string[] { "q","ProvCode" },
                    new DbType[] { DbType.String, DbType.String },
                    new object[] { q, ProvCode }, out x, ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    CityAutocompleteJSON pc = new CityAutocompleteJSON();
                    pc.Id = Convert.ToInt64(oRow["ID"]);
                    pc.Text = oRow["Name"].ToString();
                    pc.Code= oRow["Code"].ToString();
                    res.Add(pc);
                }
            }
            return res;
        }
    }
    public class RefMaritalStatus
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
    public class City
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class BARANGAY
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
    public class CityAutocompleteJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
    }
    public class CityAutocompleteObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<CityAutocompleteJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class Province
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class ProvinceAutocompleteJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
    }
    public class ProvinceAutocompleteObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<ProvinceAutocompleteJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class Region
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class RegionAutocompleteJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
    }
    public class RegionAutocompleteObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<ProvinceAutocompleteJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class BarangayAutocompleteJSON
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }
    public class BarangayAutocompleteObject
    {
        public string Success { get; set; }
        public string Status { get; set; }
        public List<BarangayAutocompleteJSON> Results { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class BloodType
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
    public class EmployeeDependents
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void Save(EmployeeDependents data, string[] Name, DateTime[] BirthDay)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                string[] iName = Name;
                DateTime[] iBirthday = BirthDay;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isEmployeeHasDependents_Employee",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID  },out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    int w = 0;
                    foreach (string x in iName)
                    {
                        data.Name = "";
                        data.Name += x;
                        foreach (DateTime y in iBirthday)
                        {
                            DateTime? DOB = null ;
                            DOB = iBirthday[w];
                            db.ExecuteCommandNonQuery("HRIS_Edit_Dependents",
                                new string[] { "eID", "eEmployeeID", "eName", "eBirthDay", "eModifiedBy" },
                                new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Date, DbType.String, },
                                new object[] { data.ID, data.Employee.ID, data.Name, DOB, data.ModifiedBy }, out b, CommandType.StoredProcedure);

                            break;
                        }
                        w += 1;
                    }
                }
                else
                {
                    int w = 0;
                    foreach (string x in iName)
                    {
                        data.Name = "";
                        data.Name += x;
                        foreach (DateTime y in iBirthday)
                        {
                            // DateTime? DOB = null;
                            DateTime DOB = iBirthday[w];
                            db.ExecuteCommandNonQuery("HRIS_Save_Dependents",
                                new string[] { "eEmployeeID", "eName", "eBirthDay", "eAddedBy", "eModifiedBy" },
                                new DbType[] { DbType.Int64, DbType.String, DbType.Date, DbType.String, DbType.String },
                                new object[] { data.Employee.ID, data.Name, DOB, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
                            break;
                        }
                        w += 1;
                    }
                    
                }
            }
        }
        public static void SaveDependents(EmployeeDependents data, string[] Name, DateTime[] BirthDay)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int b = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteData_Dependents",
                    new string[] { "eEmployeeID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID}, out b, CommandType.StoredProcedure);
                int a = 0;
                string[] iName = Name;
                DateTime[] iBirthday = BirthDay;
                int w = 0;
                string _after = "";
                foreach (string x in iName)
                {
                    data.Name = "";
                    data.Name += x;
                    foreach (DateTime y in iBirthday)
                    {
                        // DateTime? DOB = null;
                        DateTime DOB = iBirthday[w];
                        db.ExecuteCommandNonQuery("HRIS_Save_Dependents",
                            new string[] { "eID", "eEmployeeID", "eName", "eBirthDay", "eAddedBy", "eModifiedBy" },
                            new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.Date, DbType.String, DbType.String },
                            new object[] { data.ID,data.Employee.ID, data.Name, DOB, data.AddedBy, data.ModifiedBy }, out a, CommandType.StoredProcedure);
                        _after += "[ID:" + data.ID + ", EmployeeID:" + data.Employee.ID + "Name:" + data.Name + ", DOB:" + DOB + "],"; 
                        
                        break;
                    }
                    w += 1;
                    data.Logs.After = _after;
                    data.Logs.Module = 53;
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
            }
        }
        public static void Delete(EmployeeDependents data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_Dependents",
                    new string[] { "eID", "eEmployeeID", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.Int64,DbType.String },
                    new object[] { data.ID,data.Employee.ID,data.ModifiedBy}, out a, CommandType.StoredProcedure);
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<EmployeeDependents> Get(Int64 Id)
        {
            var list = new List<EmployeeDependents>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_Get_Dependents",
                    new string[] { "eEmpID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeDependents e = new EmployeeDependents();
                    e.Employee = new Employee();
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Name = aRow["Name"].ToString();
                    e.BirthDay = Convert.ToDateTime(aRow["BirthDay"]);
                    list.Add(e);
                }
            }
            return list;
        }
        public static EmployeeDependents GetByID(Int64 Id)
        {
            var e = new EmployeeDependents();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_Dependents",
                   new string[] { "eID" },
                   new DbType[] { DbType.Int64 },
                   new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Employee = new Employee();
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Name = aRow["Name"].ToString();
                    e.BirthDay = Convert.ToDateTime(aRow["BirthDay"]);
                }
            }
            return e;
        }
    }
    public class EmployeeEmergencyDetails
    {
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string ContactNumber { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }
        public static void Save(EmployeeEmergencyDetails data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                int c = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_isEmployeeHas_EmergencyDetails",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { data.Employee.ID }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count == 1)
                {
                    var _ec = EmployeeEmergencyDetails.GetByEmpID(data.Employee.ID);
                    data.Logs.Before = "ID:" + _ec.ID + ", EmployeeID:" + _ec.Employee.ID + ",Name:" + _ec.Name + ", Relationship:" + _ec.Relationship + ", ContactNumber:" + _ec.ContactNumber + ",";
                    db.ExecuteCommandNonQuery("HRIS_Edit_EmerGencyDetails",
                        new string[] { "eID", "eEmployeeID", "eName", "eRelationShip", "eCOntactNumber", "eModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { data.ID, data.Employee.ID, data.Name, data.Relationship, data.ContactNumber, data.ModifiedBy }, out b, CommandType.StoredProcedure);
                    data.Logs.Type = 2;
                    data.Logs.Module = 54;
                    data.Logs.After = "ID:" + data.ID + ", EmployeeID:" + data.Employee.ID + ",Name:" + data.Name + ", Relationship:" + data.Relationship + ", ContactNumber:" + data.ContactNumber + ",";
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
                else
                {
                    db.ExecuteCommandNonQuery("HRIS_Save_EmerGencyDetails",
                       new string[] { "eEmployeeID", "eName", "eRelationShip", "eCOntactNumber", "eAddedBy", "eModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                       new object[] { data.Employee.ID, data.Name, data.Relationship, data.ContactNumber, data.AddedBy, data.ModifiedBy }, out c, CommandType.StoredProcedure);
                    data.Logs.Type = 1;
                    data.Logs.Module = 54;
                    data.Logs.Before = "";
                    data.Logs.After = "ID:" + data.ID + ", EmployeeID:" + data.Employee.ID + ",Name:" + data.Name + ", Relationship:" + data.Relationship + ", ContactNumber:" + data.ContactNumber + ",";
                    SystemLogs.SaveSystemLogs(data.Logs);
                }
            }
        }
        public static EmployeeEmergencyDetails GetByEmpID(Int64 EmpId)
        {
            var e = new EmployeeEmergencyDetails();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                DataTable resultsTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetByID_EmerGencyDetails",
                    new string[] { "eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out affectedRows, ref resultsTable, CommandType.StoredProcedure);
                if (resultsTable.Rows.Count > 0)
                {
                    DataRow resultsRow = resultsTable.Rows[0];
                    e.Employee = new Employee();
                    e.ID = Convert.ToInt64(resultsRow["ID"]);
                    e.Employee.ID = Convert.ToInt64(resultsRow["EmployeeID"]);
                    e.Name = resultsRow["Name"].ToString();
                    e.Relationship = resultsRow["Relationship"].ToString();
                    e.ContactNumber = resultsRow["ContactNumber"].ToString();
                }
            }
            return e;
        }
        public static void UpdateEmergencyContactDetails(EmployeeEmergencyDetails data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int affectedRows = 0;
                db.ExecuteCommandNonQuery("HRIS_Edit_EmerGencyDetails",
                       new string[] { "eID", "eEmployeeID", "eName", "eRelationShip", "eCOntactNumber", "eModifiedBy" },
                       new DbType[] { DbType.Int64, DbType.Int64, DbType.String, DbType.String, DbType.String, DbType.String },
                       new object[] { data.ID, data.Employee.ID, data.Name, data.Relationship, data.ContactNumber, data.ModifiedBy }, out affectedRows, CommandType.StoredProcedure);
            }
        }
    }
}

