using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BeautySalon.FrontEnd.Site.Models;
using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Services;
using Microsoft.AspNet.Identity; // 確保您有對應的模型類

namespace BeautySalon.FrontEnd.Site.Controllers.APIs
{

	[RoutePrefix("api/OrdersApi")]
	public class OrdersApiController : ApiController
	{
		private readonly OrderService _orderService;

		public OrdersApiController(OrderService orderService)
		{
			_orderService = orderService;
		}


		

		[HttpPost]
		[Route("")]
		public IHttpActionResult CreateOrder(CreateOrderDto orderDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var userId = int.Parse(User.Identity.GetUserId());
			var orderId = _orderService.CreateOrder(userId, orderDto);

			return Ok(new { success = true, orderId });
		}

		private decimal CalculateDiscountRate(int totalQuantity)
		{
			if (totalQuantity >= 15) return 0.3m;
			if (totalQuantity >= 10) return 0.2m;
			if (totalQuantity >= 5) return 0.1m;
			return 0;
		}

		private string GetDiscountDescription(int totalQuantity)
		{
			if (totalQuantity >= 15) return "滿15堂打7折";
			if (totalQuantity >= 10) return "滿10堂打8折";
			if (totalQuantity >= 5) return "滿5堂打9折";
			return "無折扣";
		}

		[HttpGet]
		[Route("")]
		public IHttpActionResult GetOrders()
		{
			var userId = User.Identity.GetUserId();
			var orders = _orderService.GetOrdersForUser(userId);
			return Ok(orders);
		}
		[HttpGet]
		[Route("{id:int}")]
		public IHttpActionResult GetOrderDetails(int id)
		{
			var userId = User.Identity.GetUserId(); // 確認用戶ID

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized(); // 若未登入則回傳未授權

			var order = _orderService.GetOrderDetails(id, userId);

			if (order == null)
				return NotFound(); // 找不到訂單則回傳404

			return Ok(order); // 成功回傳訂單資料
		}

		[HttpPost]
		[Route("{id:int}/cancel")]
		public IHttpActionResult CancelOrder(int id)
		{
			var userId = User.Identity.GetUserId(); // 取得目前用戶ID

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized(); // 驗證失敗則回傳未授權

			var result = _orderService.CancelOrder(id, int.Parse(userId));
			if (result.Success)
			{
				return Ok(new
				{
					success = true,
					message = result.Message,
					newStatus = result.NewStatus
				});
			}
			else
			{
				return BadRequest(result.Message);
			}
		}
	}
}