using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OneShopCharts.Models;

namespace OneShopCharts.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly OneShopOrderDetailsChartsContext _context;

        public OrderDetailsController(OneShopOrderDetailsChartsContext context)
        {
            _context = context;
        }

        public JsonResult JsonIndex(int id)
        {
            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            var list = _context.OrderDetail
                .GroupBy(x => x.DatailDate.Date)
                .Select(f => new { SumDate = f.Key, Rows = f.Count(), DailySum = f.Sum(w => w.DetailPrice) }).ToList();
            //.Where(w => w.SumDate > firstDay && w.SumDate < DateTime.Now);

            //var res = list.GroupBy(x => x.DatailDate.Date)
            //    .Select(f => new { SumDate = f.Key, Rows = f.Count(), DailySum = f.Sum(w => w.DetailPrice) })
            //    .Where(w => w.SumDate > firstDay && w.SumDate < DateTime.Now).ToList();
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new DefaultContractResolver();
            JArray ja = new JArray();
            JObject obj = new JObject();
            var days = string.Empty;
            var sales = string.Empty;
            list.ForEach(x => {
                days += x.SumDate.Day.ToString() + ",";
                sales += x.DailySum.ToString() + ",";
            });
            obj.Add("Days", "[" + days.Substring(0, days.Length - 1) + "]");
            obj.Add("DailySales", "[" + sales.Substring(0, sales.Length - 1) + "]");
            return Json(obj, settings);
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderDetail.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .SingleOrDefaultAsync(m => m.DetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetailID,OrderID,ItemBarcode,UnitPrice,ItemCount,Discount,DetailPrice,IsValid,DatailDate,Remarks")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.SingleOrDefaultAsync(m => m.DetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetailID,OrderID,ItemBarcode,UnitPrice,ItemCount,Discount,DetailPrice,IsValid,DatailDate,Remarks")] OrderDetail orderDetail)
        {
            if (id != orderDetail.DetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.DetailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .SingleOrDefaultAsync(m => m.DetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetail.SingleOrDefaultAsync(m => m.DetailID == id);
            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.DetailID == id);
        }
    }
}
