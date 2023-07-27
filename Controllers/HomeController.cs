using BarcodeGenerator.Models;
using IronBarCode;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using Zen.Barcode;
using SixLabors.ImageSharp.Drawing.Processing;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        //[AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        //[Authorize]
        public IActionResult CreateBarcode()
        {
            return View();
        }
        //[Authorize]
        public IActionResult Index()
        {
            string pdfFilePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/GeneratedBarcode/barcode.pdf");
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
            //string digit15_16_17 = barcode.Direnc.ToString("F2").Replace(".", "").Replace(",", "");
            string digit15_16_17 = barcode.Direnc.Replace(",", "").Replace(".", "").PadLeft(3, '0');
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
                    all23Digits = digit1_2 + digit_3_4_5_6 + digit7 + digit8 + digit9_10_11_calc.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21_calc.ToString("D3") + digit22 + digit23;
                }
                else
                {
                    all23Digits = digit1_2 + digit_3_4_5_6 + digit7 + digit8 + digit9_10_11.ToString("D3") + digit12 + digit13_14 + digit15_16_17 + digit18 + digit19_20_21_calc.ToString("D3") + digit22 + digit23;
                }

                first12Digit = Convert.ToInt64(all23Digits.Substring(0, 12));
                last11Digit = Convert.ToInt64(all23Digits.Substring(12));
                digit24 = Convert.ToInt32(barcode.GenerateDigit24(first12Digit, last11Digit));

                barcodeNum = first12Digit.ToString() + last11Digit.ToString() + digit24.ToString();

                string path = System.IO.Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                ConvertToInterleaved2of5Barcode(barcodeNum, barcode.Voltaj.ToString(), barcode.Sure.ToString());

                //GeneratedBarcode Barcode = IronBarCode.BarcodeWriter.CreateBarcode(barcodeNum, BarcodeWriterEncoding.ITF);
                //Barcode.ResizeTo(500, 120);
                //Barcode.AddBarcodeValueTextBelowBarcode();
                //Barcode.ChangeBarCodeColor(System.Drawing.Color.Black);
                //Barcode.SetMargins(10);
                string filePath = System.IO.Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                //Barcode.SaveAsPng(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                string pngFilePath = System.IO.Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                string pdfFilePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/GeneratedBarcode/barcode.pdf");

                using (var document = new Document())
                {
                    using (var fs = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        var writer = PdfWriter.GetInstance(document, fs);

                        document.Open();
                        document.NewPage();

                        var image = iTextSharp.text.Image.GetInstance(pngFilePath);
                        image.ScaleToFit(300f, 300f);
                        image.SetDpi(800, 800);
                        document.Add(image);
                        document.Close();
                    }
                }
                ViewBag.BarcodeUri = imageUrl;
                ViewBag.Barcode = barcodeNum;
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }


        private void ConvertToInterleaved2of5Barcode(string num, string v, string time)
        {
            BarcodeDraw barcodeDraw = BarcodeDrawFactory.Code25InterleavedWithoutChecksum;

            int targetWidth = 500;
            int targetHeight = 120;

            string path = System.IO.Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
            string filePath = System.IO.Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");

            using (System.Drawing.Image barcodeImage = barcodeDraw.Draw(num, targetWidth, targetHeight))
            {
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(targetWidth, targetHeight + 40))
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(System.Drawing.Color.White);
                        graphics.DrawImage(barcodeImage, new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight));
                        using (System.Drawing.Font font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 12))
                        {
                            using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
                            {
                                float scale = GetTextScale(graphics, num, font, targetWidth);
                                float scaledFontSize = font.Size * scale;
                                using (System.Drawing.Font scaledFont = new System.Drawing.Font(font.FontFamily, scaledFontSize))
                                {
                                    float textY = targetHeight;
                                    float textY2 = textY + 20;
                                    float textX = (targetWidth - graphics.MeasureString(num, scaledFont).Width) / 2;
                                    float textX2 = (targetWidth - graphics.MeasureString(v + " V / " + time + " sn", scaledFont).Width) / 2;

                                    graphics.DrawString(num, scaledFont, brush, textX, textY);
                                    graphics.DrawString(v + " V / " + time + " sn", scaledFont, brush, textX2, textY2);
                                }
                            }
                        }
                        bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }
        private float GetTextScale(System.Drawing.Graphics graphics, string text, System.Drawing.Font font, int targetWidth)
        {
            float scale = 1f;
            while (graphics.MeasureString(text, font).Width * scale > targetWidth)
            {
                scale -= 0.1f;
                if (scale <= 0)
                {
                    scale = 0.1f;
                    break;
                }
            }
            return scale;
        }
        //private string ConvertToInterleaved2of5Barcode(string input)
        //{
        //    BarcodeDraw barcodeDraw = BarcodeDrawFactory.Code25InterleavedWithoutChecksum;
        //    BarcodeMetrics metrics = barcodeDraw.GetDefaultMetrics(100);

        //    using (System.Drawing.Image image = barcodeDraw.Draw(input, metrics))
        //    {
        //        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);
        //        string base64Image = ConvertBitmapToBase64(bitmap);
        //        return base64Image;
        //    }
        //}
        //private string ConvertBitmapToBase64(System.Drawing.Bitmap bitmap)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        //        byte[] byteImage = memoryStream.ToArray();
        //        return Convert.ToBase64String(byteImage);
        //    }
        //}
        public FileResult PrintFile()
        {
            try
            {
                string pdfFilePath = System.IO.Path.Combine(_environment.ContentRootPath, "wwwroot/GeneratedBarcode/barcode.pdf");
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