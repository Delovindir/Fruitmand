namespace Fruitmand.Models
{
    public class FruitItem
    {
        public string SoortFruit { get; set; }
        public DateTime AankoopDatum { get; set; }
        public bool? IsBiologisch { get; set; }
        public string? SoortAppel { get; set; }

        public bool IsBedorven
        {
            get
            {
                var houdbaarheid = FruitShelfLife.GetHoudbaarheidInDagen(SoortFruit, SoortAppel);
                return AankoopDatum.AddDays(houdbaarheid) < DateTime.UtcNow;
            }
        }
    }

    public static class FruitShelfLife
    {
        public static int GetHoudbaarheidInDagen(string soort, string? soortAppel)
        {
            return soort.ToLower() switch
            {
                "banaan" => 4,
                "appel" => 8,
                "aardbei" => 2,
                "kiwi" => 5,
                _ => 3
            };
        }
    }
}
