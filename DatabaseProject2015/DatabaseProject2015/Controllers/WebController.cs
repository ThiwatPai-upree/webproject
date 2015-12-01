using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseProject2015.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DatabaseProject2015.Controllers
{
    public class WebController : Controller
    {
        // GET: Web


        public void statusLogin()
        {
            if (Convert.ToBoolean(Session["statusLogin"]) == true)
            {
                ViewData["statusLoginWeb"] = true;
                ViewData["Name"] = Convert.ToString(Session["Name"]);
            }
            else
            {
                ViewData["statusLoginWeb"] = false;
            }

        }
        public ActionResult Index()
        {
            statusLogin();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                BestSeller();
                List<Item> itemList = new List<Item>();

                itemList = (from i in osdb.items
                            join c in osdb.companies on i.companyID equals c.companyID
                            join p in osdb.platforms on i.platformID equals p.platformID
                            join r in osdb.ratings on i.ratingID equals r.ratingID
                            join it in osdb.itemgenres on i.itemID equals it.itemID
                            join g in osdb.genres on it.genreID equals g.genreID
                            where i.quantity != 0
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
                            }).Take(8).ToList<Item>();

                ViewData["NewProduct"] = itemList;
                ViewData["BestSellerProduct"] = BestSeller();

            }
            return View();
        }



        public ActionResult ProductList(int type, string valueQuery)
        {
            statusLogin();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                List<Item> itemList = new List<Item>();
                if (type == 1)
                {
                    itemList = AdminController.SelectByName_Item(valueQuery);
                }
                else if (type == 2)  //  1 -> NameProduct 2 -> Company   3 -> Platform  4 -> Genre 
                {
                    itemList = AdminController.SelectByCompanyName_Item(valueQuery);
                }
                else if (type == 3) // platformID
                {
                    itemList = AdminController.SelectByPlatform_Item(valueQuery);
                }
                else if (type == 4) // genreID
                {
                    itemList = AdminController.SelectByGenre_Item(valueQuery);
                }
                else
                {
                    itemList = (from i in osdb.items
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
                                }).ToList<Item>();
                }


                List<Company> companyList = (from Table in osdb.companies
                                             select new Company()
                                             {
                                                 companyID = Table.companyID,
                                                 companyname = Table.companyname
                                             }).ToList<Company>();

                List<Genre> genreList = (from Table in osdb.genres
                                         select new Genre()
                                         {
                                             genreID = Table.genreID,
                                             genrename = Table.genrename
                                         }).ToList<Genre>();

                List<Platform> platformList = (from Table in osdb.platforms
                                               select new Platform()
                                               {
                                                   platformID = Table.platformID,
                                                   platformname = Table.platformname
                                               }).ToList<Platform>();

                ViewData["CompanyList"] = companyList;
                ViewData["GenreList"] = genreList;
                ViewData["PlatformList"] = platformList;
                ViewData["ProductList"] = itemList;

            }
            return View();
        }
        //public ActionResult ProductList(int type, int valueQuery)
        //{
        //    statusLogin();
        //    using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
        //    {
        //        List<Item> itemList = new List<Item>();

        //        if (type == 1)  // 1 -> Company   2 -> Platform  3 -> Genre  4 -> All
        //        {
        //            itemList = (from i in osdb.items
        //                        join c in osdb.companies on i.companyID equals c.companyID
        //                        join p in osdb.platforms on i.platformID equals p.platformID
        //                        join r in osdb.ratings on i.ratingID equals r.ratingID
        //                        join it in osdb.itemgenres on i.itemID equals it.itemID
        //                        join g in osdb.genres on it.genreID equals g.genreID
        //                        where c.companyID == valueQuery
        //                        orderby i.companyID
        //                        select new Item()
        //                        {
        //                            ItemID = i.itemID,
        //                            ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile),
        //                            Name = i.name,
        //                            Description = i.description,
        //                            CompanyName = c.companyname,
        //                            Genre = g.genrename,
        //                            Platform = p.platformname,
        //                            Cost = i.cost,
        //                            SellingPrice = i.sellingprice,
        //                            Quantity = i.quantity,
        //                            Rating = r.ratingname,
        //                            DateModified = i.datemodified
        //                        }).ToList<Item>();
        //        }
        //        else if (type == 2)
        //        {
        //            itemList = (from i in osdb.items
        //                        join c in osdb.companies on i.companyID equals c.companyID
        //                        join p in osdb.platforms on i.platformID equals p.platformID
        //                        join r in osdb.ratings on i.ratingID equals r.ratingID
        //                        join it in osdb.itemgenres on i.itemID equals it.itemID
        //                        join g in osdb.genres on it.genreID equals g.genreID
        //                        where p.platformID == valueQuery
        //                        orderby i.datemodified
        //                        select new Item()
        //                        {
        //                            ItemID = i.itemID,
        //                            ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile),
        //                            Name = i.name,
        //                            Description = i.description,
        //                            CompanyName = c.companyname,
        //                            Genre = g.genrename,
        //                            Platform = p.platformname,
        //                            Cost = i.cost,
        //                            SellingPrice = i.sellingprice,
        //                            Quantity = i.quantity,
        //                            Rating = r.ratingname,
        //                            DateModified = i.datemodified
        //                        }).ToList<Item>();
        //        }
        //        else if (type == 3)
        //        {
        //            itemList = (from i in osdb.items
        //                        join c in osdb.companies on i.companyID equals c.companyID
        //                        join p in osdb.platforms on i.platformID equals p.platformID
        //                        join r in osdb.ratings on i.ratingID equals r.ratingID
        //                        join it in osdb.itemgenres on i.itemID equals it.itemID
        //                        join g in osdb.genres on it.genreID equals g.genreID
        //                        where g.genreID == valueQuery
        //                        orderby i.datemodified
        //                        select new Item()
        //                        {
        //                            ItemID = i.itemID,
        //                            ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile),
        //                            Name = i.name,
        //                            Description = i.description,
        //                            CompanyName = c.companyname,
        //                            Genre = g.genrename,
        //                            Platform = p.platformname,
        //                            Cost = i.cost,
        //                            SellingPrice = i.sellingprice,
        //                            Quantity = i.quantity,
        //                            Rating = r.ratingname,
        //                            DateModified = i.datemodified
        //                        }).ToList<Item>();
        //        }
        //        else
        //        {
        //            itemList = (from i in osdb.items
        //                        join c in osdb.companies on i.companyID equals c.companyID
        //                        join p in osdb.platforms on i.platformID equals p.platformID
        //                        join r in osdb.ratings on i.ratingID equals r.ratingID
        //                        join it in osdb.itemgenres on i.itemID equals it.itemID
        //                        join g in osdb.genres on it.genreID equals g.genreID
        //                        orderby i.datemodified
        //                        select new Item()
        //                        {
        //                            ItemID = i.itemID,
        //                            ImageProfile = string.Format("data:image/png;base64,{0}", i.imageprofile),
        //                            Name = i.name,
        //                            Description = i.description,
        //                            CompanyName = c.companyname,
        //                            Genre = g.genrename,
        //                            Platform = p.platformname,
        //                            Cost = i.cost,
        //                            SellingPrice = i.sellingprice,
        //                            Quantity = i.quantity,
        //                            Rating = r.ratingname,
        //                            DateModified = i.datemodified
        //                        }).ToList<Item>();
        //        }


        //        List<Company> companyList = (from Table in osdb.companies
        //                                     select new Company()
        //                                     {
        //                                         companyID = Table.companyID,
        //                                         companyname = Table.companyname
        //                                     }).ToList<Company>();

        //        List<Genre> genreList = (from Table in osdb.genres
        //                                 select new Genre()
        //                                 {
        //                                     genreID = Table.genreID,
        //                                     genrename = Table.genrename
        //                                 }).ToList<Genre>();

        //        List<Platform> platformList = (from Table in osdb.platforms
        //                                       select new Platform()
        //                                       {
        //                                           platformID = Table.platformID,
        //                                           platformname = Table.platformname
        //                                       }).ToList<Platform>();

        //        ViewData["CompanyList"] = companyList;
        //        ViewData["GenreList"] = genreList;
        //        ViewData["PlatformList"] = platformList;
        //        ViewData["ProductList"] = itemList;

        //    }
        //    return View();
        //}

        public ActionResult ProductDetail(int id)
        {
            statusLogin();
            List<Item> itemList = new List<Item>();
            itemList = AdminController.SelectAll_Item();
            Item item = new Item();
            item = itemList.Where(x => x.ItemID == id).FirstOrDefault();
            //item.ImageProfile = string.Format("data:image/png;base64,{0}", item.ImageProfile);
            return View("ProductDetail", item);
        }
        //--------------  Cart  -------------------
        public ActionResult ShoppingCartList()
        {
            statusLogin();
            string momery_Cart = Convert.ToString(Session["memoryCart"]); // 2;3;5;7;8
            string[] momery_Cart_Array;
            momery_Cart_Array = momery_Cart.Split(';'); //[2][3][5][7][8]
            int memory_Cart_Size = momery_Cart_Array.Length;

            List<Item> itemList = new List<Item>();
            List<DeliveryMethod> deliverymethodList = new List<DeliveryMethod>();
            double productCost = 0.00;
            double totalCost = 0.00;

            if (momery_Cart != "")
            {
                using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
                {
                    for (int j = 0; j < memory_Cart_Size; j++)
                    {

                        int counterSameTime = 0;
                        for (int k = 0; k < memory_Cart_Size; k++) // count Same item
                        {
                            if (momery_Cart_Array[j] == momery_Cart_Array[k])
                            {
                                counterSameTime++;
                            }
                        }
                        bool repeatItem = false;
                        for (int p = 0; p < j; p++) // find print already
                        {
                            if (momery_Cart_Array[j] == momery_Cart_Array[p])
                            {
                                repeatItem = true;
                                break;
                            }
                        }

                        if (!repeatItem)
                        {
                            Item item = new Item();
                            item = (from i in osdb.items
                                    join c in osdb.companies on i.companyID equals c.companyID
                                    join p in osdb.platforms on i.platformID equals p.platformID
                                    join r in osdb.ratings on i.ratingID equals r.ratingID
                                    join it in osdb.itemgenres on i.itemID equals it.itemID
                                    join g in osdb.genres on it.genreID equals g.genreID
                                    where i.itemID == Convert.ToInt16(momery_Cart_Array[j])

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
                                        Quantity = counterSameTime,
                                        Rating = string.Format("{0:0.00}", (counterSameTime * Convert.ToDouble(i.cost))),
                                        DateModified = i.datemodified
                                    }).FirstOrDefault();

                            productCost += Convert.ToDouble(item.Rating);
                            itemList.Add(item);
                        }



                    }

                }
            }
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                deliverymethodList = (from delivery in osdb.deliverymethods
                                      select new DeliveryMethod()
                                      {
                                          deliverymethodID = delivery.deliverymethodID,
                                          deliverymethodname = delivery.deliverymethodname,
                                          fee = Convert.ToInt16(delivery.fee)
                                      }).ToList<DeliveryMethod>();
            }

            Order TotalCost = new Order();
            TotalCost.FirstName = productCost + "";
            TotalCost.LastName = (productCost + 5) + "";
            ViewData["itemCartList"] = itemList;
            ViewData["deliverymethodList"] = deliverymethodList;
            return View("ShoppingCartList", TotalCost);
        }

        public ActionResult ShoppingCartRegister()
        {
            statusLogin();
            return View();
        }

        //public ActionResult ShoppingCartEnd()
        //{
        //    statusLogin();
        //    return View();
        //}
        //[HttpPost]
        public ActionResult ShoppingCartEnd(int deliveryMethodID, int totalCost)
        {
            statusLogin();
            string userName = Convert.ToString(Session["Username"]);
            //userName = "user6";
            string momery_Cart = Convert.ToString(Session["memoryCart"]);
            string[] momery_Cart_Array;
            momery_Cart_Array = momery_Cart.Split(';'); //[2][3][5][7][8]
            int memory_Cart_Size = momery_Cart_Array.Length;
            order order = new order();
            using (OnlineShoppingDataClassesDataContext OnlineShoppingDB = new OnlineShoppingDataClassesDataContext())
            {
                order = new order()
                {
                    username = userName,
                    orderstatusID = 2,
                    deliverymethodID = deliveryMethodID,
                    date = DateTime.Now,
                    total = totalCost
                };
                OnlineShoppingDB.orders.InsertOnSubmit(order);
                OnlineShoppingDB.SubmitChanges();


                var orderID = (from o in OnlineShoppingDB.orders
                               where o.username == userName
                               orderby o.orderID descending
                               select o.orderID).FirstOrDefault();

                for (int j = 0; j < memory_Cart_Size; j++)
                {

                    int counterSameTime = 0;
                    for (int k = 0; k < memory_Cart_Size; k++) // count Same item
                    {
                        if (momery_Cart_Array[j] == momery_Cart_Array[k])
                        {
                            counterSameTime++;
                        }
                    }
                    bool repeatItem = false;
                    for (int p = 0; p < j; p++) // find print already
                    {
                        if (momery_Cart_Array[j] == momery_Cart_Array[p])
                        {
                            repeatItem = true;
                            break;
                        }
                    }

                    if (!repeatItem)
                    {
                        int currentItemID = Convert.ToInt16(momery_Cart_Array[j]);
                        Item currentItem = AdminController.SelectByID_Item(currentItemID);
                        itemorder itemOrder = new itemorder()
                        {
                            orderID = orderID,
                            itemID = currentItemID,
                            price = currentItem.SellingPrice,
                            amount = counterSameTime,
                            cost = currentItem.Cost
                        };
                        OnlineShoppingDB.itemorders.InsertOnSubmit(itemOrder);
                        OnlineShoppingDB.SubmitChanges();
                    }
                }
                order = new order();
                order.orderID = orderID;

            }
            return View("ShoppingCartEnd", order);
        }
        //--------------  Cart  -------------------
        public ActionResult Login()
        {

            return View();
        }

        public string loginStudentID(string username, string password)
        {
            //Session.Clear();
            var resultLogin = "";
            resultLogin = null;
            using (OnlineShoppingDataClassesDataContext onlineShoppingDB = new OnlineShoppingDataClassesDataContext())
            {
                resultLogin = (from user in onlineShoppingDB.users
                               where user.username == username && user.password == password
                               select user.name).FirstOrDefault();
            }

            if (resultLogin != null)
            {
                Session["statusLogin"] = true;
                Session["userName"] = username;
                Session["password"] = password;
                Session["Name"] = resultLogin;
                return "True";
            }
            else
            {
                Session.Clear();
                return "Not Found";
            }
        }
        public void Logout()
        {
            Session.Clear();
        }
        public ActionResult orderHistory()
        {
            string username = Convert.ToString(Session["userName"]);
            statusLogin();
            ViewData["orderHistory"] = GetOrderList(username);
            return View();
        }
        public ActionResult profile()
        {
            statusLogin();
            User username_check = new User();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                string username = Convert.ToString(Session["userName"]);
                username_check = (from u in osdb.users
                                  where u.username == username
                                  select new User()
                                  {
                                      Username = u.username,
                                      Password = u.password,
                                      Gender = u.gender,
                                      FirstName = u.name,
                                      LastName = u.surname,
                                      Address = u.address,
                                      District = u.district,
                                      Birthday = u.dateofbirth + "",

                                      Email = u.email,
                                      Province = u.province,
                                      SubDistrict = u.subdistrict,
                                      Telephone = u.telephone,
                                      Zipcode = u.zipcode
                                  }).FirstOrDefault();
            }
            return View("profile", username_check);
        }
        [HttpPost]
        public ActionResult profile(FormCollection collection)
        {
            statusLogin();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                string username = collection["username"];
                user user = (from u in osdb.users
                             where u.username == username
                             select u).SingleOrDefault();
                if (user != null)
                {
                    string password = collection["password"];
                    string fname = collection["fname"];
                    string lname = collection["lname"];
                    string gender = collection["gender"];
                    DateTime dob = Convert.ToDateTime(collection["dob"]);
                    string phone = collection["phone"];
                    string email = collection["email"];
                    string address = collection["address"];
                    string province = collection["province"];
                    string district = collection["district"];
                    string sdistrict = collection["subdistrict"];
                    string zcode = collection["zcode"];

                    user.password = (user.password == password) ? user.password : password;
                    user.name = (user.name == fname) ? user.name : fname;
                    user.surname = (user.surname == lname) ? user.surname : lname;
                    user.gender = (user.gender == gender) ? user.gender : gender;
                    user.dateofbirth = (user.dateofbirth == dob) ? user.dateofbirth : dob;
                    user.telephone = (user.telephone == phone) ? user.telephone : phone;
                    user.email = (user.email == email) ? user.email : email;
                    user.address = (user.address == address) ? user.address : address;
                    user.province = (user.province == province) ? user.province : province;
                    user.district = (user.district == district) ? user.district : district;
                    user.subdistrict = (user.subdistrict == sdistrict) ? user.subdistrict : sdistrict;
                    user.zipcode = (user.zipcode == zcode) ? user.zipcode : zcode;

                    osdb.SubmitChanges();
                    return View("Index");
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult Register()
        {
            statusLogin();
            return View("Register");
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            statusLogin();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                string username = collection["username"];
                string username_check = (from u in osdb.users
                                         where u.username == username
                                         select u.username).SingleOrDefault();
                if (username_check == null)
                {
                    string password = collection["password"];
                    string fname = collection["fname"];
                    string lname = collection["lname"];
                    string gender = collection["gender"];
                    DateTime dob = Convert.ToDateTime(collection["dob"]);
                    string phone = collection["phone"];
                    string email = collection["email"];
                    string address = collection["address"];
                    string province = collection["province"];
                    string district = collection["district"];
                    string sdistrict = collection["subdistrict"];
                    string zcode = collection["zcode"];
                    user u = new user()
                    {

                        username = username,
                        password = password,
                        name = fname,
                        surname = lname,
                        gender = gender,
                        dateofbirth = dob,
                        telephone = phone,
                        email = email,
                        address = address,
                        province = province,
                        district = district,
                        subdistrict = sdistrict,
                        zipcode = zcode
                    };
                    osdb.users.InsertOnSubmit(u);
                    osdb.SubmitChanges();
                    return View("Login");
                }
                else
                {
                    return View();
                }
            }


        }

        public List<Item> BestSeller()
        {
            List<Item> items = new List<Item>();
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                List<Int64> obj = (from i in osdb.itemorders.Take(8)
                                   group i by i.itemID into g
                                   orderby g.Sum(x => x.amount) descending
                                   select g.Key).ToList();

                items = (from i in osdb.items
                         join c in osdb.companies on i.companyID equals c.companyID
                         join p in osdb.platforms on i.platformID equals p.platformID
                         join r in osdb.ratings on i.ratingID equals r.ratingID
                         join it in osdb.itemgenres on i.itemID equals it.itemID
                         join g in osdb.genres on it.genreID equals g.genreID
                         where obj.Contains(i.itemID)
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
            }
            return items;
        }

        public List<Order> GetOrderList(string username)
        {
            List<Order> orders;
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                orders = (from o in osdb.orders
                          join u in osdb.users on o.username equals u.username
                          join d in osdb.deliverymethods on o.deliverymethodID equals d.deliverymethodID
                          join s in osdb.orderstatus on o.orderstatusID equals s.orderstatusID
                          where u.username == username
                          select new Order()
                          {
                              OrderID = o.orderID
                              ,
                              OrderStatus = s.orderstatusname
                              ,
                              DeliveryMethod = d.deliverymethodname
                              ,
                              DateAdded = o.date
                              ,
                              Total = o.total
                          }).ToList();
            }
            return orders;
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
                                  ImageProfile = i.imageprofile
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

        public void EditUserDetail(string username, string fname, string lname, DateTime dob, string gender, string address, string sdistrict, string district, string province, string zcode, string phone, string email)
        {
            using (OnlineShoppingDataClassesDataContext osdb = new OnlineShoppingDataClassesDataContext())
            {
                user user = (from u in osdb.users
                             where u.username == username
                             select u).SingleOrDefault();
                user.name = (user.name.Equals(fname)) ? user.name : fname;
                user.surname = (user.surname.Equals(lname)) ? user.surname : lname;
                user.dateofbirth = (user.dateofbirth.Equals(dob)) ? user.dateofbirth : dob;
                user.gender = (user.gender.Equals(gender)) ? user.gender : gender;
                user.address = (user.address.Equals(address)) ? user.address : address;
                user.subdistrict = (user.subdistrict.Equals(sdistrict)) ? user.subdistrict : sdistrict;
                user.district = (user.district.Equals(district)) ? user.district : district;
                user.province = (user.province.Equals(province)) ? user.province : province;
                user.zipcode = (user.zipcode.Equals(zcode)) ? user.zipcode : zcode;
                user.telephone = (user.telephone.Equals(phone)) ? user.telephone : phone;
                user.email = (user.email.Equals(email)) ? user.email : email;
                osdb.SubmitChanges();
            }
        }

        public void add_ItemToCart(string itemID)
        {
            string momery_Cart = Convert.ToString(Session["memoryCart"]);
            if (Session["memoryCart"] == "" || Session["memoryCart"] == null || momery_Cart.Trim() == "")
            {
                momery_Cart = itemID;
            }
            else
            {
                momery_Cart += ";" + itemID;
            }
            Session["memoryCart"] = momery_Cart;
        }

        public void delete_ItemToCart(string itemID)
        {
            string newMomery_Cart = "";
            string momery_Cart = Convert.ToString(Session["memoryCart"]);
            string[] momery_Cart_Array;
            momery_Cart_Array = momery_Cart.Split(';'); //[2][3][5][7][8]
            int memory_Cart_Size = momery_Cart_Array.Length;


            for (int i = 0; i < memory_Cart_Size; i++)
            {
                if (momery_Cart_Array[i] != itemID)
                {
                    if (newMomery_Cart == "")
                    {
                        newMomery_Cart = momery_Cart_Array[i];
                    }
                    else
                    {
                        newMomery_Cart += ";" + momery_Cart_Array[i];
                    }
                }
            }
            Session["memoryCart"] = newMomery_Cart;
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult HowToBuy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUs(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("supanat_toystory@hotmail.com"));  // replace with valid value 
                message.From = new MailAddress(model.FromEmail);  // replace with valid value
                message.Subject = model.Subject;
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "mineo_w@outlook.co.th",  // replace with valid value
                        Password = "sonyvaio1995"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }




    }
}