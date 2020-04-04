using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Whatsapp.Models;

namespace Whatsapp.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        HttpCookie cookies = new HttpCookie("cook");
        LoginModel val;
        Cookies cls = new Cookies();
        WhatsappDBEntities db = new WhatsappDBEntities();
        public ActionResult Index()
        {
            if (Request.Cookies["cook"] != null)
            {
                val = cls.getCookieValue();
                return RedirectToAction("upload", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            LoginModel lg = new LoginModel();
            try
            {
                var checkuser = db.tbl_Users.Where(x => x.Username == username && x.Password == password ).FirstOrDefault();
                if (checkuser != null)
                {
                    lg.userid = checkuser.ID;
                    lg.Username = checkuser.Username;
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(lg);
                    HttpCookie cook = new HttpCookie("cook", json);
                    cook.Expires = DateTime.Now.AddHours(12);
                    Response.SetCookie(cook);
                    return RedirectToAction("upload", "Home");
                }
                else
                {
                    ViewBag.Error = "Invalid Username or Password";
                    return View();
                }
            }
            catch (Exception e)
            {

                return Json( "We are facing connection issue kindly try after 5 minutes");
            }
        }

    }
}