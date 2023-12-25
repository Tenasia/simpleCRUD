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
    public class EmployeePortal
    {
        public EmployeePortal()
        {
        }

        //dashboard data
        public Employee Employee { get; set; }
        public EmployeePersonal Personal { get; set; }
        public EmployeeContract Contract { get; set; }

        public static EmployeePortal GetEmployeePortalDetails(Int64 Id)
        {
            var e = new EmployeePortal();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetEmployeeDetails_EmployeePortal",
                    new string[] { "eID" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out a, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    e.Employee.ID = Convert.ToInt64(aRow["ID"]);
                    e.Employee.EmployeeID = aRow["EmployeeID"].ToString();
                }
            }
            return e;
        }
    }

    public class EmployeePortalAttachments
    {
        //Important Notice: There is only two file uploads in the Employee portal pages, one is from OT page and the other one is in OB page.
        // The DirectiveID in the table, sets as the folder and also an OB ID and OT ID in the db tables.
        // There are two types for files, Type 1 = OverTime and Type 2 = OfficialBusiness
        // There is no Editing for the attachments, only saving and deleting of files.
        // -louie 
        public long ID { get; set; }
        public Employee Employee { get; set; }
        public long DirectiveID { get; set; }
        public List<string> Images { get; set; }
        public string Filename { get; set; }
        public int Type { get; set; }
        public string AddedBy { get; set; }


        public static void SaveEmployeePortalAttachments(EmployeePortalAttachments data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                int b = 0;
                //db.ExecuteCommandNonQuery("HRIS_DeleteAllAttachments_EmployeePortal",
                //    new string[] { "oEmployeeID", "oDirectiveID", "oType" },
                //    new DbType[] { DbType.String, DbType.Int64, DbType.Int32 },
                //    new object[] { data.EmployeeID, data.DirectiveID, data.Type }, out a, CommandType.StoredProcedure
                //    );
                int x = 0;
                foreach (var i  in data.Images)
                {
                    data.Filename = "";
                    data.Filename += i;
                    db.ExecuteCommandNonQuery("HRIS_SaveAttachments_EmployeePortal",
                       new string[] { "oEmployeeID","oDirectiveID", "oType", "oFileName", "oAddedBy" },
                       new DbType[] { DbType.String,DbType.Int64, DbType.Int32, DbType.String, DbType.String },
                       new object[] { data.Employee.ID,data.DirectiveID,  data.Type, data.Filename, data. AddedBy }, out a, CommandType.StoredProcedure);
                }
            }
        }
        public static void DeleteEmployeePortalAttachments(EmployeePortalAttachments data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                db.ExecuteCommandNonQuery("HRIS_DeleteAttachments_EmployeePortal",
                   new string[] { "oID", "oEmployeeID", "oType" },
                   new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32 },
                   new object[] { data.ID, data.Employee.ID, data.Type}, out a, CommandType.StoredProcedure);
            }
        }
        public static List<EmployeePortalAttachments> GetEmployeePortalAttachments(string EmpID, int Type, Int64 DirId)
        {
            var list = new List<EmployeePortalAttachments>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int a = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetAttachments_EmployeePortal",
                    new string[] { "oEmployeeID", "oType", "oDirectiveID" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.Int64},
                    new object[] { EmpID, Type, DirId}, out a, ref aTable, CommandType.StoredProcedure
                    );
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeePortalAttachments o = new EmployeePortalAttachments();
                    o.Employee = new Employee();
                    o.ID = Convert.ToInt64(aRow["ID"]);
                    o.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    o.Type = Convert.ToInt32(aRow["Type"]);
                    o.Filename = aRow["FileName"].ToString();
                    o.DirectiveID = Convert.ToInt64(aRow["DirectiveID"]);
                    list.Add(o);
                }
            }
            return list;
        }
    }
}
