namespace Fruitmand.Models.Dto
{
    public class MandDetailDto
    {
        public List<FruitItem> Fruits { get; set; } = new();
        public int TotaalAantal { get; set; }
        public int AantalBedorven { get; set; }
        public int SpoilagePercentage { get; set; }
        public bool IsBedorven => SpoilagePercentage > 20;
    }
}
