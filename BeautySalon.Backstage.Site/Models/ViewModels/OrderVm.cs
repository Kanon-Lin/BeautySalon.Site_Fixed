using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models.ViewModels
{
    public class OrderVm
    {
        public int OrderID { get; set; }

        public int MemberID { get; set; }

        public int OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalNetAmount { get; set; }

        public decimal DiscountedAmount { get; set; }

        public DateTime? CancelDate { get; set; }

        public int SumRemainingQuantity { get; set; }

        public string Name { get; set; } //會員姓名

        // 明細檔資料
        public List<OrderDetailVm> OrderDetails { get; set; }
    }
}