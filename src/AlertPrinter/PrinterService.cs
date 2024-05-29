using System.Drawing;
using Zebra.Sdk.Graphics;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace AlertPrinter;

public class PrinterService
{
    public IEnumerable<Printer> Discover() {
        return UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter())
            .Select(x => new Printer(x.Address));
    }

    public void Print() {
        var printers = UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter());
        var printer = printers.FirstOrDefault();
        if (printer is null) {
            throw new Exception("No printer found");
        }

        var connection = printer.GetConnection();
        connection.Open();
        var instance = ZebraPrinterFactory.GetInstance(connection);
        const int xResolution = 864;
        const int xLengthMm = 108;
        const int labelWidth = 80;
        const int labelHeight = 40;
        var labelWidthResolution = labelWidth * xResolution / xLengthMm;
        var labelHeightResolution = labelHeight * xResolution / xLengthMm;
        using Image image = new Bitmap(labelWidthResolution, labelHeightResolution);
        using var graphics = Graphics.FromImage(image);
        var rect = new RectangleF(0, 0, labelWidthResolution, 50);
        using var stringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        
        graphics.DrawString("Hello", new Font("Tahoma",40), Brushes.Black, rect, stringFormat);
        var zebraImage = ZebraImageFactory.GetImage(image);
        instance.PrintImage(zebraImage, 0, 0, image.Width, image.Height, false);
    }
}