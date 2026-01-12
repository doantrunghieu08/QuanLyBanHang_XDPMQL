using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ThanhToan_DTO
    {
        public string PaymentID { get; set; }

        public string OrderID { get; set; }

        public int Amount { get; set; }

        public string MethodID { get; set; }

        public string StatusID { get; set; }

        public DateTime PaymentDate { get; set; }

        public string MethodName { get; set; }

        public string StatusName { get; set; }
    }
}
