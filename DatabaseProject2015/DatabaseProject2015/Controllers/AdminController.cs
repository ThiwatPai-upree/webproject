using DatabaseProject2015.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Linq.SqlClient;
using System.Data.Linq;
using System.IO;
using System.Drawing;

namespace DatabaseProject2015.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult ItemsIndex()
        {
            List<Item> Items = new List<Item>();
            Items = SelectAll_Item().OrderBy(i => i.Name).ToList();

            ViewData["companies"] = SelectAll_Company();
            ViewData["platforms"] = SelectAll_Platform();
            ViewData["genres"] = SelectAll_Genre();
            return View(Items);
        }

        [HttpPost]
        public ActionResult ItemsIndex(FormCollection collection)
        {
            string search = collection["search"];
            int type = int.Parse(collection["filter"]);
            int price = int.Parse(collection["price"]);
            List<Item> Items = SelectAll_Item();

            if (!String.IsNullOrEmpty(search))
            {
                if (type == 1)
                {
                    int id = 0;
                    if(int.TryParse(search, out id))
                    {
                        Items = new List<Item> { SelectByID_Item(id) };
                    }
                }
                else if (type == 2)
                {
                    Items = SelectByName_Item(search);
                }
                else if (type == 3)
                {
                    Items = SelectByCompanyName_Item(search);
                }
                else if (type == 4)
                {
                    Items = SelectByPlatform_Item(search);
                }
                else
                {
                    Items = SelectByGenre_Item(search);
                }
            }

            int start = 0, end = 9999;
            if (price == 1)
            {
                end = 499;
            }
            else if (price == 2)
            {
                start = 500;
                end = 999;
            }
            else if (price == 3)
            {
                start = 1000;
                end = 1499;
            }
            else if (price == 4)
            {
                start = 1500;
            }

            Items = Items.Where(i => i.SellingPrice >= start && i.SellingPrice <= end).OrderBy(i => i.Name).ToList();

            return View("ItemsIndex", Items);
        }

        //public ActionResult ItemsIndexSearch(int type, string search)
        //{
        //    List<Item> Items = new List<Item>();
        //    if (type == 1) // search by name
        //    {
        //        Items = SelectByName_Item(search).OrderBy(i => i.Name).ToList();
        //    }
        //    else if (type == 2) // search by company
        //    {

        //    }
        //    else // search by platform
        //    {

        //    }
        //    //Items = SelectAll_Item().OrderBy(i => i.Name).ToList();

        //    ViewData["companies"] = SelectAll_Company();
        //    ViewData["platforms"] = SelectAll_Platform();
        //    ViewData["genres"] = SelectAll_Genre();
        //    return View("ItemsIndex", Items);
        //}

        // GET: Admin/Details/5

        public ActionResult ItemDetails(int id)
        {
            Item Item = SelectByID_Item(id);
            return View(Item);
        }

        // GET: Admin/Create
        public ActionResult CreateItem()
        {
            ViewData["ratings"] = SelectAll_Rating();
            ViewData["companies"] = SelectAll_Company();
            ViewData["platforms"] = SelectAll_Platform();
            ViewData["genres"] = SelectAll_Genre();

            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult CreateItem(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var Image = Request.Files["imageprofile"];
                string Name = collection["name"].TrimEnd(',');
                string Company = collection["company"].TrimEnd(',');
                string Genre = collection["genre"].TrimEnd(',');
                string Platform = collection["platform"].TrimEnd(',');
                Int64 RatingID = Int64.Parse(collection["rating"]);
                decimal Cost = Decimal.Parse(collection["cost"]);
                decimal SellingPrice = Decimal.Parse(collection["sellingprice"]);
                Int64 Quantity = Int64.Parse(collection["quantity"]);
                string Description = collection["description"];

                string Path = @"C:\DB_Picture\";
                if(Image.ContentLength != 0)
                {
                    Path += Image.FileName;
                }
                else
                {
                    Path += "default-image.jpg";
                }
                Image Img = System.Drawing.Image.FromFile(Path, true);
                string ImageProfile = ImageToBase64(Img, System.Drawing.Imaging.ImageFormat.Jpeg);
                item Item = new item();
                Item.imageprofile = ImageProfile;
                Item.name = Name;
                Item.ratingID = RatingID;
                Item.cost = Cost;
                Item.sellingprice = SellingPrice;
                Item.quantity = Quantity;
                Item.description = Description;
                Item.datemodified = DateTime.Now;

                Int64 CompanyID;
                if (!Int64.TryParse(Company, out CompanyID))
                {
                    CompanyID = AddCompany(Company);
                    
                }
                Item.companyID = CompanyID;

                Int64 PlatformID;
                if(!Int64.TryParse(Platform, out PlatformID))
                {
                    PlatformID = AddPlatform(Platform);
                }
                Item.platformID = PlatformID;

                Int64 ItemID = AddItem(Item);

                Int64 GenreID;
                if (!Int64.TryParse(Genre, out GenreID))
                {
                    GenreID = AddGenre(Genre);
                }
                AddItemGenre(ItemID, GenreID);

                return RedirectToAction("ItemsIndex");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult EditItem(int id)
        {
            Item i = SelectByID_Item(id);
            ViewData["ratings"] = SelectAll_Rating();
            ViewData["companies"] = SelectAll_Company();
            ViewData["platforms"] = SelectAll_Platform();
            ViewData["genres"] = SelectAll_Genre();
            //ViewData["Image"] = Base64ToImage(i.ImageProfile);
            return View(i);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditItem(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var Image = Request.Files["imageprofile"];
                string Name = collection["name"].TrimEnd(',');
                string Company = collection["company"].TrimEnd(',');
                string Genre = collection["genre"].TrimEnd(',');
                string Platform = collection["platform"].TrimEnd(',');
                Int64 RatingID = Int64.Parse(collection["rating"]);
                decimal Cost = Decimal.Parse(collection["cost"]);
                decimal SellingPrice = Decimal.Parse(collection["sellingprice"]);
                Int64 Quantity = Int64.Parse(collection["quantity"]);
                string Description = collection["description"];

                string Path = @"C:\DB_Picture\";
                if (Image.ContentLength != 0)
                {
                    Path += Image.FileName;
            }
                else
            {
                    Path += "default-image.jpg";
            }
                Image Img = System.Drawing.Image.FromFile(Path, true);
                string ImageProfile = ImageToBase64(Img, System.Drawing.Imaging.ImageFormat.Jpeg);
                using(OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
                {
                    item Item = osdb.items.Where(i => i.itemID == id).SingleOrDefault();
                    Item.imageprofile = (Item.imageprofile.Equals(ImageProfile)) ? Item.imageprofile : ImageProfile;
                    Item.name = (Item.name.Equals(Name)) ? Item.name : Name;
                    Item.ratingID = (Item.ratingID == RatingID) ? Item.ratingID : RatingID;
                    Item.cost = (Item.cost == Cost) ? Item.cost : Cost;
                    Item.sellingprice = (Item.sellingprice == SellingPrice) ? Item.sellingprice : SellingPrice;
                    Item.quantity = (Item.quantity == Quantity) ? Item.quantity : Quantity;
                    Item.description = (Item.description.Equals(Description)) ? Item.description : Description;

                    Int64 CompanyID;
                    if (!Int64.TryParse(Company, out CompanyID))
                    {
                        CompanyID = AddCompany(Company);

        }
                    Item.companyID = (Item.companyID == CompanyID) ? Item.companyID : CompanyID;

                    Int64 PlatformID;
                    if (!Int64.TryParse(Platform, out PlatformID))
        {
                        PlatformID = AddPlatform(Platform);
        }
                    Item.platformID = (Item.platformID == PlatformID) ? Item.platformID : PlatformID;

                    osdb.SubmitChanges();

                    Int64 GenreID;
                    if (!Int64.TryParse(Genre, out GenreID))
            {
                        GenreID = AddGenre(Genre);
                    }
                    itemgenre ig = osdb.itemgenres.Where(i => i.itemID == Item.itemID).SingleOrDefault();
                    ig.genreID = (ig.genreID == GenreID) ? ig.genreID : GenreID;
                    osdb.SubmitChanges();

                }
                return RedirectToAction("ItemsIndex");
            }
            catch
            {
                return View();
            }
        }

        //// GET: Admin/Delete/5
        //public ActionResult DeleteItem(int id)
        //{
        //    return View();
        //}

        // POST: Admin/Delete/5

        //[HttpPost]
        //public ActionResult DeleteItem(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        /*------------------------------------------------------------------------------------*/

        public ActionResult OrdersIndex()
        {
            List<Order> Orders = SelectAll_Order();
            return View(Orders);
        }

        [HttpPost]
        public ActionResult OrdersIndex(FormCollection collection)
        {
            List<Order> Orders = SelectAll_Order();
            string search = collection["search"];
            int types = int.Parse(collection["types"]);
            if (!String.IsNullOrEmpty(search))
            {
                if(types == 1)
                {
                    int id = 0;
                    if(int.TryParse(search, out id))
                    {
                        Orders = Orders.Where(o => o.OrderID == id).ToList();
                    }
                }
                else if(types == 2)
                {
                    Orders = Orders.Where(o => o.FirstName.ToLower().Contains(search.ToLower()) 
                                              || o.LastName.ToLower().Contains(search.ToLower())).ToList();
                }
                else
                {
                    Orders = Orders.Where(o => o.OrderStatus.ToLower().Contains(search.ToLower())).ToList();
                }
            }
            return View("OrdersIndex", Orders);
        }

        // GET: Admin/Details/5
        public ActionResult OrderDetails(int id)
        {
            Order order = SelectByID_Order(id);
            User user = SelectByUsername_User(order.Username);
            List<ItemOrder> items = SelectByOrderID_ItemOrder(id);
            ViewBag.orderdetail = order;
            ViewBag.userdetail = user;
            decimal total = 0;
            foreach (ItemOrder io in items)
            {
                total += io.Total;
            }
            ViewBag.Total = total;
            return View(items);
        }

        // GET: Admin/Edit/5
        public ActionResult UpdateOrderStatus(int id)
        {
            ViewBag.orderdetail = SelectByID_Order(id);
            List<orderstatus> orderstatusList = SelectAll_OrderStatus();
            return View(orderstatusList);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult UpdateOrderStatus(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                using(OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
                {
                    order order = osdb.orders.Where(o => o.orderID == id).SingleOrDefault();
                Int64 osid = Int64.Parse(collection["osID"]);
                    order.orderstatusID = osid;
                    osdb.SubmitChanges();
                }
                return RedirectToAction("OrdersIndex");
            }
            catch
            {
                return View();
            }
        }

        /* -----------------------------------product----------------------------------- */

        public void AddItemGenre(Int64 itemID, Int64 genreID)
        {
            using(OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                osdb.itemgenres.InsertOnSubmit(new itemgenre
                {
                    itemID = itemID,
                    genreID = genreID
                });
                osdb.SubmitChanges();
            }
        }

        public Int64 AddItem(item i)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                osdb.items.InsertOnSubmit(i);
                osdb.SubmitChanges();
                return i.itemID;
            }
        }

        public static List<Item> SelectAll_Item()
        {

            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                List<Item> Items = new List<Item>();

                Items = (from i in osdb.items
                         join c in osdb.companies on i.companyID equals c.companyID
                         join p in osdb.platforms on i.platformID equals p.platformID
                         join r in osdb.ratings on i.ratingID equals r.ratingID
                         join it in osdb.itemgenres on i.itemID equals it.itemID
                         join g in osdb.genres on it.genreID equals g.genreID
                         orderby i.datemodified
                         select new Item()
                         {
                             ItemID = i.itemID,
                             ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile),
                             Name = i.name,
                             Description = i.description,
                             CompanyName = c.companyname,
                             Genre = g.genrename,
                             Platform = p.platformname,
                             Cost = i.cost,
                             SellingPrice = i.sellingprice,
                             Quantity = i.quantity,
                             Rating = r.ratingname,
                             DateModified = i.datemodified
                         }).ToList();

                return Items;
            }

        }

        public static Item SelectByID_Item(int id)
        {
            Item Item = SelectAll_Item().Where(i => i.ItemID == id).SingleOrDefault();

            //using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            //{
            //    item = (from i in osdb.items
            //            join c in osdb.companies on i.companyID equals c.companyID
            //            join p in osdb.platforms on i.platformID equals p.platformID
            //            join r in osdb.ratings on i.ratingID equals r.ratingID
            //            join it in osdb.itemgenres on i.itemID equals it.itemID
            //            join g in osdb.genres on it.genreID equals g.genreID
            //            where i.itemID == id
            //            select new Item()
            //            {
            //                itemID = i.itemID,
            //                imageProfile = i.imageprofile,
            //                name = i.name,
            //                description = i.description,
            //                genre = g.genrename,
            //                companyName = c.companyname,
            //                platform = p.platformname,
            //                cost = i.cost,
            //                sellingPrice = i.sellingprice,
            //                quantity = i.quantity,
            //                rating = r.ratingname,
            //                dateModified = i.datemodified
            //            }).SingleOrDefault();
            //}

            return Item;

        }

        public static List<Item> SelectByName_Item(string name)
        {
            List<Item> Items = SelectAll_Item().Where(i => i.Name.ToLower().Contains(name.ToLower())).ToList();
            //using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            //{
            //    items = (from i in osdb.items
            //             join c in osdb.companies on i.companyID equals c.companyID
            //             join p in osdb.platforms on i.platformID equals p.platformID
            //             join r in osdb.ratings on i.ratingID equals r.ratingID
            //             join it in osdb.itemgenres on i.itemID equals it.itemID
            //             join g in osdb.genres on it.genreID equals g.genreID
            //             where SqlMethods.Like(i.name, "%" + name + "%")
            //             select new Item()
            //             {
            //                 itemID = i.itemID,
            //                 imageProfile = i.imageprofile,
            //                 name = i.name,
            //                 description = i.description,
            //                 genre = g.genrename,
            //                 companyName = c.companyname,
            //                 platform = p.platformname,
            //                 cost = i.cost,
            //                 sellingPrice = i.sellingprice,
            //                 quantity = i.quantity,
            //                 rating = r.ratingname,
            //                 dateModified = i.datemodified
            //             }).ToList();
            //}
            return Items;
        }

        public static List<Item> SelectByCompanyName_Item(string name)
        {
            List<Item> Items = SelectAll_Item().Where(i => i.CompanyName.ToLower().Contains(name.ToLower())).ToList();
            return Items;
        }

        public static List<Item> SelectByPlatform_Item(string name)
        {
            List<Item> Items = SelectAll_Item().Where(i => i.Platform.ToLower().Contains(name.ToLower())).ToList();
            return Items;
        }

        public static List<Item> SelectByGenre_Item(string name)
        {
            List<Item> Items = SelectAll_Item().Where(i => i.Genre.ToLower().Contains(name.ToLower())).ToList();
            return Items;
        }

        public static List<Item> SelectByPrice_Item(decimal start, decimal end)
        {
            List<Item> Items = SelectAll_Item().Where(i => i.SellingPrice >= start && i.SellingPrice <= end).ToList();
            //using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            //{
            //    items = (from i in osdb.items
            //             join c in osdb.companies on i.companyID equals c.companyID
            //             join p in osdb.platforms on i.platformID equals p.platformID
            //             join r in osdb.ratings on i.ratingID equals r.ratingID
            //             join it in osdb.itemgenres on i.itemID equals it.itemID
            //             join g in osdb.genres on it.genreID equals g.genreID
            //             where i.sellingprice >= start && i.sellingprice <= end
            //             select new Item()
            //             {
            //                 itemID = i.itemID,
            //                 imageProfile = i.imageprofile,
            //                 name = i.name,
            //                 description = i.description,
            //                 genre = g.genrename,
            //                 companyName = c.companyname,
            //                 platform = p.platformname,
            //                 cost = i.cost,
            //                 sellingPrice = i.sellingprice,
            //                 quantity = i.quantity,
            //                 rating = r.ratingname,
            //                 dateModified = i.datemodified
            //             }).ToList();
            //}
            return Items;
        }

        public static List<Item> SelectByRating_Item(string rate)
        {
            List<Item> Items = SelectAll_Item().Where(i => i.Rating.Equals(rate, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return Items;
        }

        public Int64 AddCompany(string companyname)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                company c = new company
                {
                    companyname = companyname
                };
                osdb.companies.InsertOnSubmit(c);
                osdb.SubmitChanges();
                return c.companyID;
            }
        }

        public Int64 SearchCompany(string companyname)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                company company = (from c in osdb.companies
                                   where c.companyname.Equals(companyname.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                   select c).SingleOrDefault();
                return (company == null) ? -1 : company.companyID;
            }
        }

        public List<company> SelectAll_Company()
        {
            List<company> companies = new List<company>();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                companies = osdb.companies.ToList();
            }
            return companies;
        }

        public Int64 AddPlatform(string platformname)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                platform p = new platform
                {
                    platformname = platformname
                };
                osdb.platforms.InsertOnSubmit(p);
                osdb.SubmitChanges();
                return p.platformID;
            }

        }

        public Int64 SearchPlatform(string platformname)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                platform platform = (from p in osdb.platforms
                                     where p.platformname.Equals(platformname.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                     select p).SingleOrDefault();
                return (platform == null) ? -1 : platform.platformID;
            }
        }

        public List<platform> SelectAll_Platform()
        {
            List<platform> platforms = new List<platform>();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                platforms = osdb.platforms.ToList();
            }
            return platforms;
        }

        public Int64 AddGenre(string genrename)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                genre g = new genre
                {
                    genrename = genrename
                };
                osdb.genres.InsertOnSubmit(g);
                osdb.SubmitChanges();
                return g.genreID;
            }
        }

        public Int64 SearchGenre(string genrename)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                genre genre = (from g in osdb.genres
                               where g.genrename.Equals(genrename.Trim(), StringComparison.InvariantCultureIgnoreCase)
                               select g).SingleOrDefault();
                return (genre == null) ? -1 : genre.genreID;
            }
        }

        public List<genre> SelectAll_Genre()
        {
            List<genre> genres = new List<genre>();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                genres = osdb.genres.ToList();
            }
            return genres;
        }

        public List<rating> SelectAll_Rating()
        {
            List<rating> ratings;
            using(OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                ratings = osdb.ratings.ToList();
            }
            return ratings;
        }

        /* -----------------------------------image----------------------------------- */

        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        /* -----------------------------------order----------------------------------- */

        public List<Order> SelectAll_Order()
        {
            List<Order> orders;

            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                orders = (from o in osdb.orders
                          join u in osdb.users on o.username equals u.username
                          join d in osdb.deliverymethods on o.deliverymethodID equals d.deliverymethodID
                          join s in osdb.orderstatus on o.orderstatusID equals s.orderstatusID
                          select new Order()
                          {
                              OrderID = o.orderID
                              ,
                              Username = o.username
                              ,
                              FirstName = u.name
                              ,
                              LastName = u.surname
                              ,
                              DeliveryMethod = d.deliverymethodname
                              ,
                              Fee = d.fee
                              ,
                              OrderStatus = s.orderstatusname
                              ,
                              DateAdded = o.date
                              ,
                              Total = o.total
                          }).ToList();
            }

            return orders;
        }

        public Order SelectByID_Order(int id)
        {
            Order order = SelectAll_Order().Where(o => o.OrderID == id).SingleOrDefault();
            return order;
        }

        public List<ItemOrder> SelectByOrderID_ItemOrder(int orderID)
        {
            List<ItemOrder> itemorders;

            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                itemorders = (from io in osdb.itemorders
                              join i in osdb.items on io.itemID equals i.itemID
                              where io.orderID == orderID
                              select new ItemOrder()
                              {
                                  ItemID = io.itemID
                                  ,
                                  Name = i.name
                                  ,
                                  ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile)
                                  ,
                                  Amount = io.amount
                                  ,
                                  Price = io.price
                                  ,
                                  Total = (io.price * io.amount)
                              }).ToList();
            }
            return itemorders;
        }

        public List<orderstatus> SelectAll_OrderStatus()
        {
            List<orderstatus> os;
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                os = (from record in osdb.orderstatus
                      select record).ToList();
            }
            return os;
        }

        /* -----------------------------------user----------------------------------- */

        public User SelectByUsername_User(string username)
        {
            User user;
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                user = (from u in osdb.users
                        where u.username.Equals(username)
                        select new User()
                        {
                            FirstName = u.name
                            ,
                            LastName = u.surname
                            ,
                            Address = u.address
                            ,
                            District = u.district
                            ,
                            SubDistrict = u.subdistrict
                            ,
                            Province = u.province
                            ,
                            Zipcode = u.zipcode
                            ,
                            Email = u.email
                            ,
                            Telephone = u.telephone
                        }).SingleOrDefault();
            }
            return user;
        }

        

    }
}
