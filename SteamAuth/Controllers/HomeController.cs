using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SteamAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly Regex _accountIdRegex = new Regex(@"^http://steamcommunity\.com/openid/id/(7[0-9]{15,25})$", RegexOptions.Compiled);

        public async Task<ActionResult> Index()
        {
            long? steamid = null;


            var authenticateResult =
                await HttpContext.GetOwinContext().Authentication.AuthenticateAsync("ExternalCookie");

            var firstOrDefault = authenticateResult?.Identity.Claims.FirstOrDefault(claim => claim.Issuer == "Steam" && claim.Type.Contains("nameidentifier"));

            var idString = firstOrDefault?.Value;
            var match = _accountIdRegex.Match(idString ?? "");

            if (match.Success)
            {
                var accountId = match.Groups[1].Value;
                steamid = long.Parse(accountId);
            }

            return View(steamid);
        }

        public ActionResult AuthoriseSteam()
        {
            return new ChallengeResult("Steam", Url.Action("Index"));
        }
    }
}