using Microsoft.AspNetCore.Mvc;

namespace AlertPrinter;

public class AlertController : Controller
{
    private readonly PrinterService _printer;

    public AlertController(PrinterService printer) {
        _printer = printer;
    }
    [HttpPost("print")]
    public IActionResult PrintAlert() {
        _printer.Print();
        return Ok();
    }
    [HttpGet("printers")]
    public IActionResult GetPrinters() {
        
        return Ok(_printer.Discover());
    }
}