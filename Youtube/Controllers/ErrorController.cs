using Microsoft.AspNetCore.Mvc;

namespace Youtube.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
        [Route("error/400")]
        public IActionResult Error400()
        {
            return View();
        }
        [Route("error/401")]
        public IActionResult Error401()
        {
            return View();
        }
        [Route("error/415")]
        public IActionResult Error415()
        {
            return View();
        }
    }
}
