using IronBarCode;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BarcodeGenerator.Models
{
    public partial class Barcode
    {
        public Tip Tip { get; set; }
        public Cap1 Cap1 { get; set; }
        public Cap2? Cap2 { get; set; }
        public byte Voltaj { get; set; }
        public float Direnc { get; set; }
        public int Sure { get; set; }
        public SogumaSuresi SogumaSuresi { get; set; }
        public byte Rezistans { get; set; }
        public byte EnerjiDuzeltmeEksi { get; set; }
        public byte EnerjiDuzeltmeArti { get; set; }
    }

    public enum Tip : byte
    {
        [Display(Name = "Raytrans")]
        Raytrans = 91,
        [Display(Name = "JT.D.W.tapping tee")]
        JTDW_Tapping_Tee = 92,
        [Display(Name = "Y Reduction")]
        Y_Reduction = 93,
        [Display(Name = "tapping tee")]
        Tapping_Tee = 94,
        [Display(Name = "I Coupler")]
        Coupler = 95,
        [Display(Name = "[ Single socket")]
        Single_Socket = 96,
        [Display(Name = "T Tees")]
        T_Tees = 97,
        [Display(Name = "Elbow")]
        Elbow = 98
    }
    public enum Cap1
    {
        [Display(Name = "020")]
        Yirmi = 020,
        [Display(Name = "025")]
        YirmiBeş = 025,
        [Display(Name = "032")]
        Otuzİki = 032,
        [Display(Name = "040")]
        Kırk = 040,
        [Display(Name = "050")]
        Elli = 050,
        [Display(Name = "063")]
        AltmışÜç = 063,
        [Display(Name = "075")]
        YetmişBeş = 075,
        [Display(Name = "090")]
        Doksan = 090,
        [Display(Name = "110")]
        YüzOn = 110,
        [Display(Name = "125")]
        YüzYirmiBeş = 125,
        [Display(Name = "140")]
        YüzKırk = 140,
        [Display(Name = "160")]
        YüzAltmış = 160,
        [Display(Name = "180")]
        YüzSeksen = 180,
        [Display(Name = "200")]
        İkiYüz = 200,
        [Display(Name = "225")]
        İkiYüzYirmiBeş = 225,
        [Display(Name = "250")]
        İkiYüzElli = 250,
        [Display(Name = "315")]
        ÜçYüzOnBeş = 315
    }
    public enum Cap2
    {
        [Display(Name = "020")]
        Yirmi = 020,
        [Display(Name = "025")]
        YirmiBeş = 025,
        [Display(Name = "032")]
        Otuzİki = 032,
        [Display(Name = "040")]
        Kırk = 040,
        [Display(Name = "050")]
        Elli = 050,
        [Display(Name = "063")]
        AltmışÜç = 063,
        [Display(Name = "075")]
        YetmişBeş = 075,
        [Display(Name = "090")]
        Doksan = 090,
        [Display(Name = "110")]
        YüzOn = 110,
        [Display(Name = "125")]
        YüzYirmiBeş = 125,
        [Display(Name = "140")]
        YüzKırk = 140,
        [Display(Name = "160")]
        YüzAltmış = 160,
        [Display(Name = "180")]
        YüzSeksen = 180,
        [Display(Name = "200")]
        İkiYüz = 200,
        [Display(Name = "225")]
        İkiYüzYirmiBeş = 225,
        [Display(Name = "250")]
        İkiYüzElli = 250,
        [Display(Name = "315")]
        ÜçYüzOnBeş = 315
    }
    public enum SogumaSuresi : byte
    {
        [Display(Name = "5")]
        Beş,
        [Display(Name = "10")]
        On,
        [Display(Name = "15")]
        OnBeş,
        [Display(Name = "20")]
        Yirmi,
        [Display(Name = "30")]
        Otuz,
        [Display(Name = "45")]
        KırkBeş,
        [Display(Name = "60")]
        Altmış,
        [Display(Name = "75")]
        YetmişBeş,
        [Display(Name = "90")]
        Doksan,
    }
}
