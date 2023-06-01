using BarcodeGenerator.Models;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace BarcodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private readonly IWebHostEnvironment _environment;
        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult CreateBarcode()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateBarcode(Barcode barcode, string radioButton)
        {
            long first12Digit = 0, last11Digit = 0;
            string barcodeNum, all23Digits = "";
            int digit1_2 = Convert.ToByte(barcode.Tip);
            int digit3_4 = 06, digit5_6 = 16, digit7 = 3;
            int digit8 = Convert.ToByte(barcode.SogumaSuresi);
            int digit9_10_11_calc = barcode.CalculateDiameter(Convert.ToInt16(barcode.Cap1), Convert.ToInt16(barcode.Cap2));
            int digit9_10_11 = Convert.ToInt16(barcode.Cap1);
            int digit12 = 8;
            int digit13_14 = barcode.Voltaj;
            float digit15_16_17 = barcode.Direnc;
            int digit18 = barcode.Rezistans;
            int digit19_20_21_calc = barcode.ModifyTime(barcode.Sure);
            int digit19_20_21 = barcode.Sure;
            int digit22 = barcode.EnerjiDuzeltmeEksi;
            int digit23 = barcode.EnerjiDuzeltmeArti;
            int digit24 = 0;
            try
            {
                if (radioButton == "Second")
                {
                    if (barcode.Cap2 != null)
                    {
                        all23Digits = digit1_2 + digit3_4.ToString("D2") + digit5_6 + digit7 + digit8 + digit9_10_11_calc.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21.ToString("D3") + digit22 + digit23;
                    }
                    else
                    {
                        all23Digits = digit1_2 + digit3_4.ToString("D2") + digit5_6 + digit7 + digit8 + digit9_10_11.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21.ToString("D3") + digit22 + digit23;
                    }
                }
                else if (radioButton == "Minute")
                {
                    if (barcode.Cap2 != null)
                    {
                        all23Digits =  digit1_2 + digit3_4.ToString("D2") + digit5_6 + digit7 + digit8 + digit9_10_11_calc.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21_calc.ToString("D3") + digit22 + digit23;
                    }
                    else
                    {
                        all23Digits = digit1_2 + digit3_4.ToString("D2") + digit5_6 + digit7 + digit8 + digit9_10_11.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21_calc.ToString("D3") + digit22 + digit23;
                    }
                }

                first12Digit = Convert.ToInt64(all23Digits.Substring(0, 12));
                last11Digit = Convert.ToInt64(all23Digits.Substring(12));
                digit24 = Convert.ToInt32(barcode.GenerateDigit24(first12Digit, last11Digit));

                barcodeNum = first12Digit.ToString() + last11Digit.ToString() + digit24.ToString();

                GeneratedBarcode Barcode = IronBarCode.BarcodeWriter.CreateBarcode(barcodeNum, BarcodeWriterEncoding.ITF);
                Barcode.ResizeTo(500, 120);
                Barcode.AddBarcodeValueTextBelowBarcode();
                Barcode.ChangeBarCodeColor(System.Drawing.Color.Black);
                Barcode.SetMargins(10);
                string path = Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                Barcode.SaveAsPng(filePath);
                string fileName = Path.GetFileName(filePath);
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                ViewBag.QrCodeUri = imageUrl;
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        public ActionResult Print()
        {
            string path = Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
            string filePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
            string fileName = Path.GetFileName(filePath);
            string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
            if (!System.IO.File.Exists(filePath))
            {
                ViewBag.Message = "Dosya Bulunamadı.";
                return View();
            }
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (sender, args) =>
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(filePath);
                args.Graphics.DrawImage(image, args.MarginBounds);
                args.HasMorePages = false;
                image.Dispose();
            };
            pd.Print();
            ViewBag.Message = "Yazdırma işlemi tamamlandı.";
            return View();
        }
    }
}