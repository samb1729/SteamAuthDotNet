using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SteamAuth.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            long? steamid = null;


            var authenticateResult =
                await HttpContext.GetOwinContext().Authentication.AuthenticateAsync("ExternalCookie");

            if (authenticateResult != null)
            {
                var idString = authenticateResult.Identity.Claims.First().Value;

                if (idString.StartsWith("http://steamcommunity.com/openid/id/"))
                {
                    steamid = long.Parse(idString.Replace("http://steamcommunity.com/openid/id/", ""));
                }
            }

            return View(steamid);
        }

        public ActionResult AuthoriseSteam()
        {
            return new ChallengeResult("Steam", Url.Action("Index"));
        }
    }
}