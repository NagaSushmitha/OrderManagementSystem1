using Dapper;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OrderManagementSystem.Services
{
    public class OrderService : IOrderService
    {

        #region Private Members

        private readonly IConnectionFactory _connectionFactory;

        #endregion

        #region Constructor
        public OrderService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        #endregion
        public async Task<List<Order>> GetOrders(long userId)
        {
            using (var connection = this._connectionFactory.GetConnection)
            {
                string getOrderDetailsQuery = null;
             string   getUserRoleQuery = string.Format(QueryHelper.GetUserRoleQuery, userId);               
                int userRole = await connection.QuerySingleOrDefaultAsync<int>(getUserRoleQuery);

                getOrderDetailsQuery = string.Format(QueryHelper.GetOrderDetailsQuery, (short)Status.Cancelled);
                     

                if (userRole == (int)Roles.Admin)
                {
                    getOrderDetailsQuery = getOrderDetailsQuery + string.Format(QueryHelper.GetBuyerOrderDetailsQuery, userId);
                     
                }
                List<Order> orders = await connection.QueryAsync<Order>(getOrderDetailsQuery) as List<Order>;
                List<Item> itemsList = await connection.QueryAsync<Item>(getOrderDetailsQuery) as List<Item>;
                orders.ForEach(w =>
                {
                    w.ItemsList = itemsList.Where(y => y.OrderId == w.OrderId).ToList();
                });
                return orders;


            }
        }


        public async Task<OrderResponse> SaveOrder(Order order)
        {
            using (var connection = this._connectionFactory.GetConnection)
            {
                OrderResponse response = new OrderResponse();
                
                if (order.IsDirty)
                {

                    OracleDynamicParameters parameters = new OracleDynamicParameters();
                    parameters.Add("BuyerId", order.BuyerId, dbType: OracleDbType.Int64, direction: ParameterDirection.Input);
                    parameters.Add("OrderStatusCode", order.OrderStatusCode, dbType: OracleDbType.Int16, direction: ParameterDirection.Input);
                    parameters.Add("ShippingAddress", order.ShippingAddress, dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input);
                    
                    parameters.Add("OrderId", order.OrderId, dbType: OracleDbType.Int64, direction: ParameterDirection.InputOutput);
                    parameters.Add("EffectiveDate", order.EffectiveDate, dbType: OracleDbType.Date, direction: ParameterDirection.Input);
                    parameters.Add("ExpiryDate", order.ExpiryDate, dbType: OracleDbType.Date, direction: ParameterDirection.Input);

                    await connection.QueryAsync<string>(CommonConstants.SaveOrder, param: parameters, commandType: CommandType.StoredProcedure);
                    response.OrderId = Convert.ToInt32(parameters.Get("OrderId"));
                    response.Message = CommonConstants.SuccessMessage;
                }
                else
                {
                    response.OrderId = order.OrderId;
                    response.Message = CommonConstants.SuccessMessage;
                }
                

                order.ItemsList.ForEach (e =>
                {
                    e.OrderId = order.OrderId;
                    e.StatusCode = order.OrderStatusCode;
                    e.EffectiveDate = order.EffectiveDate;
                }) ;
                    SaveItems(order.ItemsList.Where(item => item.IsDirty).ToList(), connection);

                return response;
            }
        }

        private void SaveItems(List<Item> items, IDbConnection connection = null)
        {
            bool isConOpened = false;
            if (connection == null)
            {
                connection = _connectionFactory.GetConnection;
                isConOpened = true;
            }
            foreach (var item in items)
            {
                if (item.IsDelete)
                {
                    item.StatusCode = (short)Status.Cancelled;
                }
                if (item.IsDirty)
                {
                    string updatePreviousRecord = string.Format(QueryHelper.UpdatePreviousQuery, item.EffectiveDate, item.OrderId, CommonConstants.ExpiryDate);
                    connection.Execute(updatePreviousRecord);
                }
                string insertItemQuery = string.Format(QueryHelper.InsertItemsQuery, item.ItemId, item.OrderId, item.Quantity);                
                connection.Execute(insertItemQuery);


            }
            if (isConOpened)
                connection.Close();
        }

    }
}