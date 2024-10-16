using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class OrderDetailVm
    {
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int ServiceID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }

        public decimal DiscountApplied { get; set; }

        public decimal NetAmount { get; set; }

        public int UsedQuantity { get; set; }

        public int RemainingQuantity { get; set; }



        public string ServiceName { get; set; } // 項目名稱

        public List<int> AppointmentIDs { get; set; } = new List<int>();// 添加預約id
        public List<DateTime> AppointmentDates { get; set; } = new List<DateTime>(); // 添加預約日期

        public List<int> AppointmentStatus { get; set; } = new List<int>(); // 添加預約狀態
    }
}