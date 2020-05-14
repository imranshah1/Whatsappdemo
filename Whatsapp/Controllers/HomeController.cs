using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Whatsapp.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Reflection;

namespace Whatsapp.Controllers
{


    public class HomeController : Controller
    {
        HttpCookie cookies = new HttpCookie("cook");
        LoginModel val;
        Cookies cls = new Cookies();
        WhatsappDBEntities db = new WhatsappDBEntities();
        public ActionResult Index()
        {
            if (Request.Cookies["cook"] != null)
            {
                val = cls.getCookieValue();
                return View();

            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult LoadImages()
        {
            var images = db.tbl_Images.ToList();
            return Json(images);
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            var file = Request.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            file.SaveAs(path);
            tbl_Images image = new tbl_Images();
            image.Title = fileName;
            image.Type = "background";
            image.Url = "/Images/"+fileName;
            image.CreatedDate = DateTime.Now;
            db.tbl_Images.Add(image);
            db.SaveChanges();


            
            return Json("Image Uploaded Succesfully");
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            if (Request.Files.Count == 0)
            {
                return Json("No File Uploaded");
            }

           else if (Request.Cookies["cook"] != null)
            {
                val = cls.getCookieValue();
                
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                file.SaveAs(path);

                int bookid = Convert.ToInt32(db.tbl_bookinfor.Any() ? db.tbl_bookinfor.Max(q => q.BookID) + 1 : 1);

                List<tbl_book> tblbook = new List<tbl_book>();
                tbl_bookinfor info = new tbl_bookinfor();
                info.BackTitle = fileName;
                info.BookID = bookid;
                info.UserID = val.userid;
                info.CreationDate = DateTime.Now;
                db.tbl_bookinfor.Add(info);
                db.SaveChanges();
                int lastmessageId = Convert.ToInt32(db.tbl_book.Max(x => x.MessageID));
                int msgid = 1;
                int counter = 0;
                DateTime datechecker = new DateTime();
                // int msgidcheck = 0;
                if (lastmessageId != 0)
                {
                    msgid = lastmessageId;
                }
                foreach (string line in System.IO.File.ReadAllLines(path))
                {
                    var date = line.ToString().Split(',')[0];
                    if (date.ToString().Contains("/") && !date.ToString().Contains("//"))
                    {
                        //datechecker = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                        DateTime dateTime = Convert.ToDateTime(date);
                        //string d = f.ToString();
                        //DateTime dateTime = DateTime.ParseExact(d, "dd/MM/yyyy", null);
                        string formatDate = dateTime.ToString("MM/dd/yyyy");
                        int pFrom = line.IndexOf(", ") + ", ".Length;
                        int pTo = line.IndexOf("- ");
                        var result = line.Substring(pFrom, pTo - pFrom);
                        string msg = "";
                        string SenderName = "";

                        int charFrom = line.IndexOf("- ") + "- ".Length;
                        int charTo = line.IndexOf(": ");
                        if (charTo != -1)
                        {
                            SenderName = line.Substring(charFrom, charTo - charFrom);
                            msg = line.ToString().Substring(line.IndexOf(": ") + 1);

                        }
                        else
                        {

                            msg = line.ToString().Substring(line.IndexOf('-') + 1);
                        }
                        DateTime timeValue = Convert.ToDateTime(result);
                        TimeSpan time = TimeSpan.Parse(timeValue.ToString("HH:mm"));
                        /// var time = line.ToString().Split(',')[1];
                        tbl_book book = new tbl_book();
                        book.MessageID = msgid;
                        book.Date = Convert.ToDateTime(formatDate);
                        book.Time = time;
                        book.SenderName = SenderName;
                        book.Message = msg;
                        book.BookID = bookid;
                        book.UserID = val.userid;
                        tblbook.Add(book);

                        counter++;
                        msgid++;
                    }
                    else
                    {
                        var values = tblbook.Where(x => x.BookID == bookid && x.MessageID == msgid - 1).FirstOrDefault();
                        values.Message += " " + line.ToString();
                    }
                }
                db.tbl_book.AddRange(tblbook);
                db.SaveChanges();
                return Json(tblbook);

            }
            else
            {

                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult upload()
        {
            if (Request.Cookies["cook"] != null)
            {
                val = cls.getCookieValue();
                ViewBag.BookId = new SelectList(db.tbl_bookinfor.Where(x=>x.UserID == val.userid).ToList(), "BookID", "BackTitle");
                return View();

            }
            return RedirectToAction("Index", "Login");
            
        }

        [HttpGet]
        public ActionResult UpdateMsg(string MessageID , string Message)
        {
            int msgid = Convert.ToInt32(MessageID);
            var book = db.tbl_book.Where(x => x.MessageID == msgid).FirstOrDefault();
            book.Message = Message;
            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json("Updated SuccessFully",JsonRequestBehavior.AllowGet);

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Upload(string [] data)
        {
            //var date = data[0].Split(',')[0];
            //var date = data[0].Split(',')[0];
            //var date = data[0].Split(',')[0];
            //foreach (var v in data)
            //{
            //    tbl_book msgsbook = new tbl_book();
            //    db.tbl_book.Add(msgsbook);
            //}



            return Json("");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult LoadChatByBook(string idd)
        {
            db.Configuration.ProxyCreationEnabled = false;
            int id = Convert.ToInt32(idd);
            var Book = db.tbl_book.Where(x => x.BookID == id).ToList();
            return Json(Book);
            
        }
        [HttpPost]
        public ActionResult LoadChatByDate(DateTime date , string bookid)
        {
            DateTime dates = Convert.ToDateTime(date);
           // string bookid = null;
            int bookID = 0; 
            if (Request.Cookies["cook"] != null)
            {
                val = cls.getCookieValue();
                if (bookid != null && bookid != ("undefined"))
                {
                    bookID = Convert.ToInt32(bookid);
                    var Book = db.tbl_book.Where(x => x.UserID == val.userid && x.BookID == bookID && x.Date >= dates).ToList();
                    return Json(Book);
                }
                else
                {
                    var result = db.tbl_book.Where(x => x.UserID == val.userid && x.Date >= dates).ToList();
                    //JavaScriptSerializer jsJson = new JavaScriptSerializer();
                    //jsJson.MaxJsonLength = 2147483644;
                    //tbl_book obj = jsJson.Deserialize<tbl_book>(result);
                    //return Json(obj);
                    // MiniProfiler.Se-ttings.MaxJsonResponseSize = 104857600;
                    var jsonResult = Json(result);
                    
                    return jsonResult;

                }

            }
            else
            {
                return RedirectToAction("Index","Login");
            }

            
        }
       

    }
}