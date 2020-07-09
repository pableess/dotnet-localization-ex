using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Owin.Localization;

namespace Mvc5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult French()
        {
            this.Response.Cookies.Set(new HttpCookie(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("fr-ca"))));

            return RedirectToAction("Index");
        }

        public ActionResult German()
        {
            this.Response.Cookies.Set(new HttpCookie(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("de"))));

            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            if (this.Response.Cookies[CookieRequestCultureProvider.DefaultCookieName] != null)
                this.Response.Cookies[CookieRequestCultureProvider.DefaultCookieName].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Index");
        }
    }
}
