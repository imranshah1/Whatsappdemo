using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whatsapp.Models
{
    public class Cookies
    {
        public LoginModel getCookieValue()
        {
            //HttpCookie cookies = HttpContext.Current.Request.Cookies["cookies"];
            var json = HttpContext.Current.Request.Cookies["Cook"].Value;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = serializer.Deserialize<LoginModel>(json);
            return result;
        }
    }
}