using Microsoft.AspNetCore.Mvc;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            if (statusCode == 404)
                return View("PaginaNoEncontrada");

            return View("Error");
        }
    }
}
