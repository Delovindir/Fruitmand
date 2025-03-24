namespace Fruitmand.Models.Dto 
{ 
    public class MandOverviewDto
    {
        public Dictionary<string, int> AantallenVanFruit { get; set; } = new();
        public bool IsBedorven { get; set; }
    }
}
