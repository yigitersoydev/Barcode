using BarcodeGenerator.Models;
using IronBarCode;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

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
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        [Authorize]
        public IActionResult CreateBarcode()
        {
            return View();
        }
        [Authorize]
        public IActionResult Index()
        {
            string pdfFilePath = Path.Combine(_environment.ContentRootPath, "wwwroot/GeneratedBarcode/barcode.pdf");
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            string base64String = System.Convert.ToBase64String(pdfBytes);
            ViewBag.PdfBase64String = base64String;

            return View();
        }
        [HttpPost]
        public IActionResult CreateBarcode(BarcodeGenerator.Models.Barcode barcode)
        {
            long first12Digit, last11Digit;
            string barcodeNum, all23Digits = "";
            int digit1_2 = Convert.ToByte(barcode.Tip);
            int digit7 = 3;
            string digit_3_4_5_6 = barcode.CalcManufacturer(barcode.Manufacturer);
            int digit8 = Convert.ToByte(barcode.SogumaSuresi);
            int digit9_10_11_calc = barcode.CalculateDiameter(Convert.ToInt16(barcode.Cap1), Convert.ToInt16(barcode.Cap2));
            int digit9_10_11 = Convert.ToInt16(barcode.Cap1);
            int digit12 = 8;
            int digit13_14 = barcode.Voltaj;
            string digit15_16_17 = (barcode.Direnc.ToString("F2").Replace(".", "").Replace(",", ""));
            int digit18 = barcode.Rezistans;
            //int digit19_20_21_calc = barcode.ModifyTime(barcode.Sure);
            int digit19_20_21_calc = barcode.ModifyTimeInSec(barcode.Sure);
            //int digit19_20_21 = barcode.Sure;
            int digit22 = barcode.EnerjiDuzeltmeEksi;
            int digit23 = barcode.EnerjiDuzeltmeArti;
            int digit24;
            try
            {
                    if (barcode.Cap2 != null)
                    {
                        all23Digits =  digit1_2 + digit_3_4_5_6 + digit7 + digit8 + digit9_10_11_calc.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21_calc.ToString("D3") + digit22 + digit23;
                    }
                    else
                    {
                        all23Digits = digit1_2 + digit_3_4_5_6 + digit7 + digit8 + digit9_10_11.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21_calc.ToString("D3") + digit22 + digit23;
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

                string pngFilePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                string pdfFilePath = Path.Combine(_environment.ContentRootPath, "wwwroot/GeneratedBarcode/barcode.pdf");

                using (var document = new Document())
                {
                    using (var fs = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        var writer = PdfWriter.GetInstance(document, fs);

                        document.Open();
                        document.NewPage();

                        var image = iTextSharp.text.Image.GetInstance(pngFilePath);
                        image.ScaleToFit(300f, 300f);
                        image.SetDpi(600, 600);
                        document.Add(image);
                        document.Close();
                    }
                }

                ViewBag.QrCodeUri = imageUrl;
                ViewBag.Barcode = barcodeNum;
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        public FileResult PrintFile()
        {
            try
            {
                string pdfFilePath = Path.Combine(_environment.ContentRootPath, "wwwroot/GeneratedBarcode/barcode.pdf");
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                Response.Headers.Add("Content-Disposition", "inline; filename=barcode.pdf");
                Response.Headers.Add("Content-Type", "application/pdf");
                Response.Headers.Add("Content-Length", fileBytes.Length.ToString());

                return File(fileBytes, "application/pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}