using BarcodeGenerator.Models;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Diagnostics;
using System.Drawing;

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
            long first12Digit, last11Digit;
            string barcodeNum;
            int digit1_2 = Convert.ToByte(barcode.Tip);
            int digit3_4 = 06, digit5_6 = 16, digit7 = 3;
            int digit8 = Convert.ToByte(barcode.SogumaSuresi);
            int digit12 = 8;
            int digit13_14 = barcode.Voltaj;
            float digit15_16_17 = barcode.Direnc;
            int digit18 = barcode.Rezistans;
            int digit19_20_21;
            int digit22 = barcode.EnerjiDuzeltmeEksi;
            int digit23 = barcode.EnerjiDuzeltmeArti;
            try
            {
                if (radioButton == "Second")
                {
                    digit19_20_21 = barcode.Sure;
                    if (barcode.Cap2 != null)
                    {
                        int digit9_10_11_calc = barcode.CalculateDiameter(Convert.ToInt16(barcode.Cap1), Convert.ToInt16(barcode.Cap2));
                        first12Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}{6}", digit1_2, digit3_4.ToString("D2"), digit5_6, digit7, digit8, digit9_10_11_calc.ToString("D3"), digit12));
                        last11Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}", digit13_14, digit15_16_17, digit18, digit19_20_21.ToString("D3"), digit22, digit23));
                        int digit24 = Convert.ToInt32(barcode.GenerateDigit24(first12Digit, last11Digit));
                        barcodeNum = first12Digit.ToString() + last11Digit.ToString() + digit24.ToString();
                    }
                    else
                    {
                        int digit9_10_11 = Convert.ToInt16(barcode.Cap1);
                        first12Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}{6}", digit1_2, digit3_4.ToString("D2"), digit5_6, digit7, digit8, digit9_10_11.ToString("D3"), digit12));
                        last11Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}", digit13_14, digit15_16_17, digit18, digit19_20_21.ToString("D3"), digit22, digit23));
                        int digit24 = Convert.ToInt32(barcode.GenerateDigit24(first12Digit, last11Digit));
                        barcodeNum = first12Digit.ToString() + last11Digit.ToString() + digit24.ToString();
                    }
                    GeneratedBarcode finalBarcode = IronBarCode.BarcodeWriter.CreateBarcode(barcodeNum, BarcodeWriterEncoding.ITF);
                    finalBarcode.ResizeTo(500, 120);
                    finalBarcode.AddBarcodeValueTextBelowBarcode();
                    finalBarcode.ChangeBarCodeColor(System.Drawing.Color.Black);
                    finalBarcode.SetMargins(10);
                    string path = Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                    finalBarcode.SaveAsPng(filePath);
                    string fileName = Path.GetFileName(filePath);
                    string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                    ViewBag.QrCodeUri = imageUrl;
                }
                else if (radioButton == "Minute")
                {
                    digit19_20_21 = barcode.ModifyNumber(barcode.Sure);
                    if (barcode.Cap2 != null)
                    {
                        int digit9_10_11_calc = barcode.CalculateDiameter(Convert.ToInt16(barcode.Cap1), Convert.ToInt16(barcode.Cap2));
                        first12Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}{6}", digit1_2, digit3_4.ToString("D2"), digit5_6, digit7, digit8, digit9_10_11_calc.ToString("D3"), digit12));
                        last11Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}", digit13_14, digit15_16_17, digit18, digit19_20_21.ToString("D3"), digit22, digit23));
                        int digit24 = Convert.ToInt32(barcode.GenerateDigit24(first12Digit, last11Digit));
                        barcodeNum = first12Digit.ToString() + last11Digit.ToString() + digit24.ToString();
                    }
                    else
                    {
                        int digit9_10_11 = Convert.ToInt16(barcode.Cap1);
                        first12Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}{6}", digit1_2, digit3_4.ToString("D2"), digit5_6, digit7, digit8, digit9_10_11.ToString("D3"), digit12));
                        last11Digit = Convert.ToInt64(string.Format("{0}{1}{2}{3}{4}{5}", digit13_14, digit15_16_17, digit18, digit19_20_21.ToString("D3"), digit22, digit23));
                        int digit24 = Convert.ToInt32(barcode.GenerateDigit24(first12Digit, last11Digit));
                        barcodeNum = first12Digit.ToString() + last11Digit.ToString() + digit24.ToString();
                    }
                    GeneratedBarcode finalBarcode = IronBarCode.BarcodeWriter.CreateBarcode(barcodeNum, BarcodeWriterEncoding.ITF);
                    finalBarcode.ResizeTo(500, 120);
                    finalBarcode.AddBarcodeValueTextBelowBarcode();
                    finalBarcode.ChangeBarCodeColor(System.Drawing.Color.Black);
                    finalBarcode.SetMargins(10);
                    string path = Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                    finalBarcode.SaveAsPng(filePath);
                    string fileName = Path.GetFileName(filePath);
                    string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                    ViewBag.QrCodeUri = imageUrl;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
    }
}