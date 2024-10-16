using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class OrdersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        public OrdersController() : this(new AppDbContext()) { }

        [HttpGet]
        //Index顯示訂單清單
        public ActionResult Index(string searchTerm)
        {
            var filteredOrders = string.IsNullOrEmpty(searchTerm)
                ? _db.Orders.OrderByDescending(o => o.OrderID).ToList() // 直接這裡排序即可
                : _db.Orders
                .Where(o => o.OrderID.ToString().Contains(searchTerm) || o.MemberID.ToString().Contains(searchTerm))
                .OrderByDescending(o => o.OrderID)
                .ToList();

            var viewModel = filteredOrders.Select(o => new OrderVm
            {
                OrderID = o.OrderID,
                MemberID = o.MemberID,
                OrderStatus = o.OrderStatus,
                OrderDate = o.OrderDate,
                TotalQuantity = o.TotalQuantity,
                TotalAmount = o.TotalAmount,
                TotalNetAmount = o.TotalNetAmount,
                DiscountedAmount = o.DiscountedAmount,
                CancelDate = o.CancelDate,
                SumRemainingQuantity = o.SumRemainingQuantity,
                Name = o.Member.Name//來自會員的Name
            });

            //var orders = _db.Orders.ToList();
            return View(viewModel);
        }

        public ActionResult ViewOrder(int orderId)
        {
            // 查詢訂單及其明細，同時包含預約ID和預約日期
            var order = _db.Orders
                .Where(o => o.OrderID == orderId)
                .Select(o => new OrderVm
                {
                    OrderID = o.OrderID,
                    MemberID = o.MemberID,
                    OrderStatus = o.OrderStatus,
                    OrderDate = o.OrderDate,
                    TotalQuantity = o.TotalQuantity,
                    TotalAmount = o.TotalAmount,
                    TotalNetAmount = o.TotalNetAmount,
                    DiscountedAmount = o.DiscountedAmount,
                    CancelDate = o.CancelDate,
                    SumRemainingQuantity = o.SumRemainingQuantity,
                    Name = o.Member.Name, // 會員姓名
                    OrderDetails = o.OrderDetails
                        .Select(od => new OrderDetailVm
                        {
                            OrderDetailID = od.OrderDetailID,
                            OrderID = od.OrderID,
                            ServiceID = od.ServiceID,
                            ServiceName = od.Service.ServiceName,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice,
                            Amount = od.Amount,
                            DiscountApplied = od.DiscountApplied,
                            NetAmount = od.NetAmount,
                            UsedQuantity = od.UsedQuantity,
                            RemainingQuantity = od.RemainingQuantity,
                            AppointmentIDs = od.Appointments
                                .Where(a => a.OrderDetailID == od.OrderDetailID)
                                .Select(a => a.AppointmentID)
                                .ToList(),
                            AppointmentDates = od.Appointments
                                .Where(a => a.OrderDetailID == od.OrderDetailID)
                                .Select(a => a.AppointmentStart)
                                .ToList(),
                            AppointmentStatus = od.Appointments
                                .Where(a => a.OrderDetailID == od.OrderDetailID)
                                .Select(a => a.AppointmentStatus)
                                .ToList(),
                        }).ToList()
                })
                .FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }


        [HttpPost]
        public ActionResult MarkOrderPaid(int orderId)
        {
            // 查找訂單
            var order = db.Orders.Find(orderId);
            if (order != null && order.OrderStatus == 1) // 檢查訂單狀態
            {
                // 更新訂單狀態
                order.OrderStatus = 2;

                // 查找對應的訂單詳情
                var orderDetails = db.OrderDetails.Where(od => od.OrderID == orderId).ToList();

                // 更新每個 OrderDetails 的 RemainingQuantity 並計算 SumRemainingQuantity
                int sumRemainingQuantity = 0;
                foreach (var detail in orderDetails)
                {
                    // 確保 Quantity 不會被修改
                    db.Entry(detail).Property(d => d.Quantity).IsModified = false;

                    // 將 RemainingQuantity 更新為 Quantity
                    detail.RemainingQuantity = detail.Quantity;

                    // 累加 RemainingQuantity
                    sumRemainingQuantity += detail.RemainingQuantity;
                }

                // 更新 Orders 中的 SumRemainingQuantity
                order.SumRemainingQuantity = sumRemainingQuantity;

                // 儲存變更
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }



        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            var order = db.Orders.Find(orderId);
            if (order != null && order.OrderStatus == 1) // 檢查是否是 OrderStatus 1
            {
                order.OrderStatus = 5;
                order.CancelDate = DateTime.Now; // 設置取消日期
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult MarkOrderRefunded(int orderId)
        {
            var order = db.Orders.Find(orderId);
            if (order != null && order.OrderStatus == 3) // 檢查是否是 OrderStatus 3
            {
                order.OrderStatus = 4;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


    }
}