using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManager.Services;
using SmartLibraryManager.ViewModels;

namespace SmartLibraryManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService emailService;

        public EmailController(EmailService emailService)
        {
            this.emailService = emailService;
        }


        [HttpPost("send")]
        public IActionResult SendEmail(Email email )
        {
            emailService.SendEmailPHPProject(email);
            return Ok();
        }

    }
}
