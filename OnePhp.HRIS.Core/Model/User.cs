using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using OnePhp.HRIS.Core.Data;

namespace OnePhp.HRIS.Core.Model
{
    public class HRISUser : Employee
    {
        public string Email { get; set; }
        public string UserID { get; set; }
        public string Role { get; set; }
        public string RoleID { get; set; }
        public int Status { get; set; }
        public bool EmailConfirmed { get; set; }
        public SystemLogs Logs { get; set; }

        public static List<HRISUser> GetList(Int64 eID)
        {
            var res = new List<HRISUser>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetUsers_ByEmployeeID", 
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { eID }, out a, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    HRISUser e = new HRISUser();
                    e.Department = new Department();
                    e.Division = new Division();
                    e.Position = new PositionCode();
                    e.Type = new EmployeeTypeCode();
                    e.Class = new EmployeeClassCode();
                    e.ActiveStatus = new RecordStatus();
                    e.Personal = new EmployeePersonal();
                    if (aRow["ActiveStatus"] != DBNull.Value)
                    {
                        e.ActiveStatus = RecordStatus.GetByID(Convert.ToInt64(aRow["ActiveStatus"]));
                    }
                    else
                    {
                        e.ActiveStatus.ID = 0;
                    }
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.EmployeeID = aRow["EmployeeID"].ToString();
                    e.Firstname = aRow["FirstName"].ToString();
                    e.Lastname = aRow["LastName"].ToString();
                    e.Middlename = aRow["MiddleName"].ToString();
                    e.Suffix = aRow["Suffix"].ToString();
                    e.Department.Description = aRow["Department"].ToString();
                    e.Division.Div_Desc = aRow["Division"].ToString();
                    e.Position.Position = aRow["Position"].ToString();
                    e.Class.Description = aRow["Class"].ToString();
                    e.Type.Description = aRow["Type"].ToString();
                    e.DateHired = Convert.ToDateTime(aRow["DateHired"]);
                    e.DateResigned = Convert.ToDateTime(aRow["DateResigned"]);
                    e.ProbationDate = Convert.ToDateTime(aRow["ProbationDate"]);
                    e.DateRegularized = Convert.ToDateTime(aRow["DateRegularized"]);
                    e.DateOfSeparation = Convert.ToDateTime(aRow["DateOfSeparation"]);
                    e.Personal.Gender = aRow["Gender"].ToString();
                    e.Personal.DateOfBirth = Convert.ToDateTime(aRow["DateOfBirth"]);
                    e.ReasonForSeparation = aRow["ReasonForSeparation"].ToString();
                    e.Role = aRow["Role"].ToString();
                    e.Email = aRow["Email"].ToString();
                    e.UserID = aRow["UserID"].ToString();
                    res.Add(e);
                }
            }
            return res;
        }
        public static void SaveUser(string UserID, long EmployeeID, SystemLogs data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                db.Execute(string.Format("Insert into HRIS_UserEmployee select \'{0}\', {1}, 1;", UserID, EmployeeID));
                db.Execute(string.Format("Update aspnetusers set EmailConfirmed = 1 where Id = \'{0}\';", UserID));
                data.After += "EmployeeUserID:" + UserID + ",EmployeeID" + EmployeeID + ", EmailConfirmed:1";
                SystemLogs.SaveSystemLogs(data);
            }

        }
        public static void DeleteUserEmail(string UserEmail, string UserID, long EmployeeID, SystemLogs data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                db.Execute(string.Format("Delete From aspnetusers  where email = \'{0}\';", UserEmail));
                db.Execute(string.Format("Delete From HRIS_UserEmployee  where userid=  \'{0}\' and EmployeeId= \'{1}\';", UserID, EmployeeID));
                data.Before += "EmployeeUserID:" + UserID + ",EmployeeID" + EmployeeID + "";
                SystemLogs.SaveSystemLogs(data);
            }
        }
        public static Employee GetEmployee(string UserID)
        {
            Employee e = new Employee();
            using (AppDb db = new AppDb())
            {
                db.Open();

                long _id = 0;
                DataTable oTable = db.Fetch(string.Format("select EmployeeID from hris_useremployee where UserID = \'{0}\'", UserID));
                if (oTable.Rows.Count> 0)
                {
                    DataRow oRow = oTable.Rows[0];
                    _id = Convert.ToInt64(oRow["EmployeeID"]);
                    
                    e = Employee.GetEmployeeDetailsByID(_id);
                }
            }
            return e;
        }
        
        public static bool PasswordHasChanged(string UserID)
        {
            bool hasChanged = false;
            using (AppDb db = new AppDb())
            {
                db.Open();
                long _id = 0;
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_MustChangePasswordFalse_aspnetusers",
                    new string[] { "userId" },
                    new DbType[] { DbType.String },
                    new object[] { UserID }, out x, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    hasChanged = true;
                }
            }
            return hasChanged;
        }
        public static UserInfo isANewUser(string UserID)
        {
            var u = new UserInfo();
            using (AppDb db = new AppDb())
            {
                db.Open();
                long _id = 0;
                DataTable oTable = db.Fetch(string.Format("select MustChangePassword from aspnetusers  where ID = \'{0}\'", UserID));
                if (oTable.Rows.Count > 0)
                {
                    DataRow aRow = oTable.Rows[0];
                    if (Convert.ToInt32(aRow["MustChangePassword"]) == 0)
                    {
                        u.MustChangePassword = true;
                    }
                    else
                    {
                        u.MustChangePassword = false;
                    }
                }
            }
            return u;
        }
    }

    public class UserInfo
    {
        public long EmployeeID { get; set; }
        public string Email { get; set; }
        public string UserID { get; set; }
        public string Role { get; set; }
        public string RoleID { get; set; }
        public int Status { get; set; }
        public bool EmailConfirmed { get; set; }
        public string OldEmail { get; set; }
        public bool MustChangePassword { get; set; }
    }
    //public class ResetPasswordModel
    //{
    //    [Required]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    public string Email { get; set; }
    //    public string Token { get; set; }
    //}
}
