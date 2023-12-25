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
    public class EmployeeRequirements
    {
        public long ID { get; set; }
        public Int32 Type { get; set; }
        public string Filename { get; set; }
        public Employee Employee { get; set; }
        public Employee AddedBy { get; set; }

        public static long SaveRequirements(EmployeeRequirements data) 
        {
            long _id = 0;
            using (AppDb db = new AppDb())
            {
                db.Open();
               
                int x = 0;  
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_save_employeeRequirements",
                    new string[] { "eEmployeeId", "eType", "eFilename", "eAddedBy" },
                    new DbType[] { DbType.Int64, DbType.Int32, DbType.String, DbType.Int64 },
                    new object[] {data.Employee.ID, data.Type, data.Filename, data.AddedBy.ID }, out x, ref aTable, CommandType.StoredProcedure);
                if (aTable.Rows.Count > 0)
                {
                    DataRow aRow = aTable.Rows[0];
                    _id = Convert.ToInt64(aRow["ID"]);
                }
            }
            return _id;
        }
        public static void DeleteRequirements(EmployeeRequirements data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("HRIS_Delete_EmployeeRequirements",
                    new string[] { "eId", "eEmployeeId", "eType" },
                    new DbType[] { DbType.Int32, DbType.Int64, DbType.Int32 },
                    new object[] { data.ID, data.Employee.ID, data.Type }, out x, CommandType.StoredProcedure);
            }
        }
        public static List<EmployeeRequirements> GetEmployeeRequirements(Int64 EmpId)
        {
            var _list = new List<EmployeeRequirements>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable aTable = new DataTable();
                db.ExecuteCommandReader("HRIS_get_EmployeeRequirements",
                    new string[] {"eEmployeeId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { EmpId }, out x, ref aTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in aTable.Rows)
                {
                    EmployeeRequirements e = new EmployeeRequirements();
                    e.ID = Convert.ToInt64(aRow["ID"]);
                    e.Type = Convert.ToInt32(aRow["Type"]);
                    e.Employee = new Employee();
                    e.Filename = aRow["Filename"].ToString();
                    e.Employee.ID = Convert.ToInt64(aRow["EmployeeID"]);
                    _list.Add(e);
                }
            }
            return _list;
        }
    }
}
