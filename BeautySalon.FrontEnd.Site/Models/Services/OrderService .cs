using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;

namespace BeautySalon.FrontEnd.Site.Services
{
	public class OrderService
	{
		private readonly AppDbContext _context;

		public OrderService(AppDbContext context)
		{
			_context = context;
		}

		public List<OrderListItemDto> GetOrdersForUser(string userId)
		{
			return _context.Orders
				.Where(o => o.MemberID.ToString() == userId)
				.OrderByDescending(o => o.OrderDate)
				.Select(o => new OrderListItemDto
				{
					OrderID = o.OrderID,
					OrderDate = o.OrderDate,
					OrderStatus = o.OrderStatus,
					TotalQuantity = o.TotalQuantity,
					TotalAmount = o.TotalAmount,
					TotalNetAmount = o.TotalNetAmount,
					SumRemainingQuantity = o.SumRemainingQuantity
				})
				.ToList();
		}


		public int CreateOrder(int memberId, CreateOrderDto orderDto)
		{
			var totalQuantity = orderDto.OrderDetails.Sum(od => od.Quantity);
			var discountRate = CalculateDiscountRate(totalQuantity);
			var totalAmount = orderDto.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);
			var discountedAmount = totalAmount * discountRate;
			var totalNetAmount = totalAmount - discountedAmount;

			var order = new Order
			{
				MemberID = memberId,
				OrderStatus = 1, // 假設 1 表示 "未付款"
				OrderDate = DateTime.Now,
				TotalQuantity = totalQuantity,
				TotalAmount = totalAmount,
				DiscountedAmount = discountedAmount,
				TotalNetAmount = totalNetAmount,
				SumRemainingQuantity = totalQuantity,
				OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
				{
					ServiceID = od.ServiceID,
					Quantity = od.Quantity,
					UnitPrice = od.UnitPrice,
					Amount = od.Quantity * od.UnitPrice,
					DiscountApplied = od.Quantity * od.UnitPrice * discountRate,
					NetAmount = od.Quantity * od.UnitPrice * (1 - discountRate),
					UsedQuantity = 0,
					RemainingQuantity = od.Quantity
				}).ToList()
			};

			_context.Orders.Add(order);
			_context.SaveChanges();

			return order.OrderID;
		}

		public OrderDetailsDto GetOrderDetails(int orderId, string userId)
		{
			var order = _context.Orders
				.Include(o => o.OrderDetails.Select(od => od.Service))
				.FirstOrDefault(o => o.OrderID == orderId && o.MemberID.ToString() == userId);

			if (order == null)
				return null;

			var orderDetailsDto = new OrderDetailsDto
			{
				OrderID = order.OrderID,
				OrderDate = order.OrderDate,
				OrderStatus = order.OrderStatus,
				CancelDate = order.CancelDate,
				TotalQuantity = order.TotalQuantity,
				TotalAmount = order.TotalAmount,
				DiscountedAmount = order.DiscountedAmount,
				TotalNetAmount = order.TotalNetAmount,
				SumRemainingQuantity = order.SumRemainingQuantity,
				OrderDetails = order.OrderDetails.Select(od => new OrderServiceItemDto
				{
					OrderDetailID = od.OrderDetailID,
					ServiceID = od.ServiceID,
					ServiceName = od.Service.ServiceName,
					Quantity = od.Quantity,
					UsedQuantity = od.UsedQuantity,
					RemainingQuantity = od.RemainingQuantity,
					UnitPrice = od.UnitPrice,
					Amount = od.Amount,
					DiscountApplied = od.DiscountApplied,
					NetAmount = od.NetAmount
				}).ToList(),
				AmountCalculation = new OrderAmountCalculationDto
				{
					Items = order.OrderDetails.Select(od => new OrderItemDetailDto
					{
						ServiceName = od.Service.ServiceName,
						Quantity = od.Quantity,
						UnitPrice = od.UnitPrice,
						Subtotal = od.Amount
					}).ToList(),
					TotalQuantity = order.TotalQuantity,
					TotalAmount = order.TotalAmount,
					DiscountRate = CalculateDiscountRate(order.TotalQuantity), // 您需要實現此方法
					DiscountedAmount = order.DiscountedAmount,
					TotalNetAmount = order.TotalNetAmount,
					DiscountDescription = GetDiscountDescription(order.TotalQuantity) // 您需要實現此方法
				}
			};

			return orderDetailsDto;
		}

		// 您需要實現這些方法
		private decimal CalculateDiscountRate(int totalQuantity)
		{
			if (totalQuantity >= 15) return 0.3m;
			if (totalQuantity >= 10) return 0.2m;
			if (totalQuantity >= 5) return 0.1m;
			return 0m;
		}

		private string GetDiscountDescription(int totalQuantity)
		{
			if (totalQuantity >= 15) return "滿15堂打7折";
			if (totalQuantity >= 10) return "滿10堂打8折";
			if (totalQuantity >= 5) return "滿5堂打9折";
			return "無折扣";
		}
		public class CancelOrderResult
		{
			public bool Success { get; set; }
			public string Message { get; set; }
			public int NewStatus { get; set; }
		}

		public CancelOrderResult CancelOrder(int orderId, int userId)
		{
			var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId && o.MemberID == userId);

			if (order == null)
			{
				return new CancelOrderResult
				{
					Success = false,
					Message = "訂單不存在或無權限取消。",
					NewStatus = order?.OrderStatus ?? 0
				};
			}

			switch (order.OrderStatus)
			{
				case 1: // 未付款
					order.OrderStatus = 5; // 已取消
					order.CancelDate = DateTime.Now;
					_context.SaveChanges();
					return new CancelOrderResult
					{
						Success = true,
						Message = "未付款訂單已成功取消。",
						NewStatus = order.OrderStatus
					};

				case 2: // 已付款
				case 4: // 已退款（假設這是您系統中的一個狀態）
					order.OrderStatus = 3; // 待退款
					order.CancelDate = DateTime.Now;
					_context.SaveChanges();
					return new CancelOrderResult
					{
						Success = true,
						Message = "訂單已更新為待退款狀態。我們將在七個工作天內處理您的退款。",
						NewStatus = order.OrderStatus
					};

				default:
					return new CancelOrderResult
					{
						Success = false,
						Message = "無法取消當前狀態的訂單。",
						NewStatus = order.OrderStatus
					};
			}

		}
	}
}
