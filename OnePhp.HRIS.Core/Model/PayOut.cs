using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OnePhp.HRIS.Core.Data;

namespace OnePhp.HRIS.Core.Model
{
    public class PayOut
    {
        public long ID { get; set; }
        public PayPeriod PayPeriod { get; set; }
        public int Status { get; set; }
        public bool IsLocked { get; set; }
        public string CheckedBy { get; set; }
        public DateTime CheckedByDate { get; set; }
        public DateTime ProcessedTime { get; set; }
        public string  ProcessedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }

        public static List<PayOut> GetPayoutForReferencens()
        {
            var _list = new List<PayOut>();
            using (AppDb db = new AppDb())
            {
                db.Open();
                int x = 0;
                DataTable oTable = new DataTable();
                db.ExecuteCommandReader("HRIS_GetPayOut_References_Payroll",
                    new string[] { },
                    new DbType[] { },
                    new object[] { }, out x,ref oTable, CommandType.StoredProcedure);
                foreach (DataRow oRow in oTable.Rows)
                {
                    PayOut p = new PayOut();
                    p.PayPeriod = new PayPeriod();
                    p.ID = Convert.ToInt64(oRow["ID"]);
                    p.PayPeriod.PayDate = Convert.ToDateTime(oRow["PayDate"]);
                    _list.Add(p);
                }
            }
            return _list;
        }
    }
}
