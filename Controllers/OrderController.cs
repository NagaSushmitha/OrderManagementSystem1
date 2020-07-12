using OrderManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OrderManagementSystem.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        #region Private Members
        private readonly IOrderService _orderService;
        #endregion

        #region Constructor
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        public async Task<IHttpActionResult> Get(long userId)
        {

            var result = await this._orderService.GetOrders(userId);
            return result;
        }

        public async Task<IHttpActionResult> Post(Order order)
        {

            var result = await this._orderService.SaveOrder(order);
            return result;
        }

    }
}