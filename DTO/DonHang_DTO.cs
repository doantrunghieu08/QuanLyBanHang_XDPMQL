using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }       
        public DateTime OrderDate { get; set; } 
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; }   
        public string Address { get; set; } 
        public string Phone { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
    }
}
