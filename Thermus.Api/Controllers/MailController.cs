using Microsoft.AspNetCore.Mvc;
using Thermus.Api.Services;

namespace Thermus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly IEmailService _email;

    public MailController(IEmailService email)
    {
        // guardo la dependencia para usarla en los endpoints
        _email = email;
    }

    [HttpPost("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        
        await _email.SendAsync("2021redes@gmail.com", "Prueba Brevo", "Si recibís esto, ya quedó andando.");
        return Ok("Enviado");
    }
}
