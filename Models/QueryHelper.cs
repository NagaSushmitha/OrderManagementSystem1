using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagementSystem.Models
{
    public static class QueryHelper
    {
        public const string GetUserRoleQuery = @"select user_role from t_mas_user where user_id={0}";
        public const string GetOrderDetailsQuery = @"select OR_ORDER_ID OrderId,OR_BUYER_ID BuyerId,OR_ORDER_STATUS OrderStatusCode ,
                                                  (select STA_DESC from T_MAS_STATUS where STA_CODE=OR_ORDER_STATUS ) OrderStatusDesc,
                                                 OR_SHIPPING_ADDRESS ShippingAddress,IT_ITEM_ID ItemId,IT_ITEM_QUANTITY Quantity,
                                                 PRO_NAME Name,PRO_WEIGHT Weight,PRO_HEIGHT Height,PRO_IMAGE ImageUrl,
                                                 PRO_SKU SKUId,PRO_AVAILABLE_QUANTITY AvailableQuantity from T_TRN_ORDER join T_TRN_ITEM on IT_ORDER_ID=OR_ORDER_ID AND
                                                IT_EXPIRY_DATE=OR_EXPIRY_DATE AND IT_STATUS=OR_ORDER_STATUS join T_MAS_PRODUCT on 
                                                PRO_PRODUCT_ID=IT_ID where OR_EXPIRY_DATE='31/12/3049' and OR_ORDER_STATUS!={0}";

        public const string GetBuyerOrderDetailsQuery = @"and OR_BUYER_ID={0}";
        public const string UpdatePreviousQuery = @"update T_TRN_ITEM set IT_EXPIRY_DATE ={0}  where IT_ITEM_ID = {1} 
                                                       and IT_EXPIRY_DATE = {2 }";
        public const string InsertItemsQuery = @"insert into T_TRN_ITEM(IT_ITEM_ID, IT_ORDER_ID, IT_ITEM_QUANTITY,
                                                 IT_EFFECTIVE_DATE, IT_EXPIRY_DATE) values({ 0}, { 1}, { 2});";
    }
}