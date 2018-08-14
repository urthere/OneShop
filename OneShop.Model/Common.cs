using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace OneShop.Model
{
    public static class Common<T> where T : class
    {
        /// <summary>
        /// 分页查询 + 条件查询 + 排序
        /// </summary>
        /// <typeparam name="Tkey">泛型</typeparam>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="total">总数量</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>IQueryable 泛型集合</returns>
        public static IQueryable<T> LoadPageItems<Tkey>(
            int pageSize,
            int pageIndex,
            out int total,
            Expression<Func<T, bool>> whereLambda,
            Func<T, Tkey> orderbyLambda,
            bool isAsc,
            DbContext context)
        {
            total = context.Set<T>().Where(whereLambda).Count();
            if (isAsc)
            {
                var temp = context.Set<T>().Where(whereLambda)
                             .OrderBy<T, Tkey>(orderbyLambda)
                             .Skip(pageSize * (pageIndex - 1))
                             .Take(pageSize);
                return temp.AsQueryable();
            }
            else
            {
                var temp = context.Set<T>().Where(whereLambda)
                           .OrderByDescending<T, Tkey>(orderbyLambda)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize);
                return temp.AsQueryable();
            }
        }

        public static IList<OrderDetailsNameModel> QueryJoin(OneShopEntities context, DateTime startDate, DateTime endDate)
        {
            var b = context.OrderDetails.Join(
                context.Stocks,
                od => od.ItemBarcode,
                s => s.ItemBarcode,
                (od, s) => new {
                    od.ItemBarcode,
                    s.ItemName,
                    od.UnitPrice,
                    od.DetailPrice,
                    od.Discount,
                    od.DatailDate,
                    od.ItemCount,
                    od.IsValid
                }).Where(
                od =>  od.IsValid &&
                od.DatailDate > startDate &&
                od.DatailDate < endDate
                ).ToList();

            IList<OrderDetailsNameModel> tmp = new List<OrderDetailsNameModel>();
            b.ForEach(x => tmp.Add(new OrderDetailsNameModel()
            {
                ItemBarcode = x.ItemBarcode,
                ItemCount = x.ItemCount,
                ItemName = x.ItemName,
                UnitPrice = x.UnitPrice,
                Discount = x.Discount,
                DatailDate = x.DatailDate,
                IsValid = x.IsValid,
                DetailPrice = x.DetailPrice
            }));
            return tmp;
        }
    }
}
