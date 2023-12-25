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
    public class BrandName
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public SystemLogs Logs { get; set; }

        public static string Save(BrandName data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("hris_isExist_brandname",
                    new string[] { "eBrandname" },
                    new DbType[] { DbType.String },
                    new object[] { data.Name }, out y, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    res = "Brandname Exist.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("hris_save_brandname",
                       new string[] { "eBrandName", "eAddedBy", "eModifiedBy" },
                       new DbType[] { DbType.String, DbType.String, DbType.String },
                       new object[] { data.Name, data.AddedBy, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                    data.Logs.Before = "";
                    data.Logs.After = " ID:" + data.ID.ToString() + ", BrandName: " + data.Name + ",  AddedBy: " + data.AddedBy + ", ModifriedBy: " + data.ModifiedBy + ". ";
                    data.Logs.Type = 1;
                    data.Logs.TypeName = "Add";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    res = "ok";
                }

            }
            return res;
        }
        public static string Edit(BrandName data)
        {
            var res = "";
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                int y = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("hris_isExist_Update_brandname",
                    new string[] { "eId", "eBrandName" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.Name }, out y, ref oTable, CommandType.StoredProcedure);
                if (oTable.Rows.Count > 0)
                {
                    res = "Brandname is in use.";
                }
                else
                {
                    db.ExecuteCommandNonQuery("hris_edit_brandname",
                        new string[] { "eId", "eBrandName", "eModifiedBy" },
                        new DbType[] { DbType.Int64, DbType.String, DbType.String },
                        new object[] { data.ID, data.Name, data.ModifiedBy }, out x, CommandType.StoredProcedure);
                    data.Logs.Before = "";
                    data.Logs.After = " ID:" + data.ID.ToString() + ", BrandName: " + data.Name + ",  AddedBy: " + data.AddedBy + ", ModifriedBy: " + data.ModifiedBy + ". ";
                    data.Logs.Type = 2;
                    data.Logs.TypeName = "Update";
                    SystemLogs.SaveSystemLogs(data.Logs);
                    res = "ok";
                }
            }
            return res;
        }
        public static void Delete(BrandName data)
        {
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                db.ExecuteCommandNonQuery("hris_Delete_brandname",
                    new string[] { "eId", "eModifiedBy" },
                    new DbType[] { DbType.Int64, DbType.String },
                    new object[] { data.ID, data.ModifiedBy }, out x, CommandType.StoredProcedure);

                data.Logs.After = "";
                data.Logs.Type = 3;
                data.Logs.TypeName = "Delete";
                SystemLogs.SaveSystemLogs(data.Logs);
            }
        }
        public static List<BrandName> Get()
        {
            var _list = new List<BrandName>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable xTable = new DataTable();
                db.ExecuteCommandReader("hris_get_brandname",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x, ref xTable, CommandType.StoredProcedure);
                foreach (DataRow aRow in xTable.Rows)
                {
                    BrandName b = new BrandName();
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.Name = aRow["BrandName"].ToString();
                    _list.Add(b);
                }
            }
            return _list;
        }
        public static BrandName GetById(Int64 Id)
        {
            var b = new BrandName();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable xTable = new DataTable();
                db.ExecuteCommandReader("hris_getbyid_brandname",
                    new string[] { "eId" },
                    new DbType[] { DbType.Int64 },
                    new object[] { Id }, out x, ref xTable, CommandType.StoredProcedure);
                if (xTable.Rows.Count > 0)
                {
                    DataRow aRow = xTable.Rows[0];
                    b.ID = Convert.ToInt64(aRow["ID"]);
                    b.Name = aRow["BrandName"].ToString();
                }
            }
            return b;
        }
    }
}
