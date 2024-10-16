using System;
using System.Collections.Generic;

namespace BeautySalon.FrontEnd.Site.Models.DTO
{
	public class OrderListItemDto
	{
		public int OrderID { get; set; }
		public DateTime OrderDate { get; set; }
		public int OrderStatus { get; set; }
		public int TotalQuantity { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalNetAmount { get; set; }
		public int SumRemainingQuantity { get; set; }
	}



	public class OrderDetailsDto
	{
		public int OrderID { get; set; }
		public DateTime OrderDate { get; set; }
		public int OrderStatus { get; set; }
		public DateTime? CancelDate { get; set; } // 訂單取消日期
		public int TotalQuantity { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal DiscountedAmount { get; set; }
		public decimal TotalNetAmount { get; set; }
		public int SumRemainingQuantity { get; set; }
		public List<OrderServiceItemDto> OrderDetails { get; set; }
		public OrderAmountCalculationDto AmountCalculation { get; set; }
	}



	public class OrderServiceItemDto
	{
		public int OrderDetailID { get; set; }
		public int ServiceID { get; set; }
		public string ServiceName { get; set; }
		public int Quantity { get; set; }
		public int UsedQuantity { get; set; }
		public int RemainingQuantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Amount { get; set; }
		public decimal DiscountApplied { get; set; }
		public decimal NetAmount { get; set; }
	}



	public class OrderAmountCalculationDto
	{
		public List<OrderItemDetailDto> Items { get; set; }
		public int TotalQuantity { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal DiscountRate { get; set; }
		public decimal DiscountedAmount { get; set; }
		public decimal TotalNetAmount { get; set; }
		public string DiscountDescription { get; set; }
	}
	public class OrderItemDetailDto
	{
		public string ServiceName { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Subtotal { get; set; }
	}

	public class CreateOrderDto
	{
		public List<CreateOrderDetailDto> OrderDetails { get; set; }
	}

	public class CreateOrderDetailDto
	{
		public int ServiceID { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
	}
}