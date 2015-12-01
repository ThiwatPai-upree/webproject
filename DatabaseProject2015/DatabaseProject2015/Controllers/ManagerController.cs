using DatabaseProject2015.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseProject2015.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {

            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                List<Item> itemList = new List<Item>();
                itemList = (from i in osdb.items
                            join io in osdb.itemorders on i.itemID equals io.itemID into g
                            orderby g.Sum(x => x.amount) descending
                            select new Item()
                            {
                                ItemID = i.itemID,
                                ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile),
                                Name = i.name
                            }).Take(6).ToList<Item>();
                ViewData["NewProduct"] = itemList;


            }
                return View();
        }

        // GET: Manager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Manager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Manager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Manager/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
