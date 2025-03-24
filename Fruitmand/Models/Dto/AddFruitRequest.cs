namespace Fruitmand.Models.Dto
{
    public class AddFruitRequest
    {
        public DateTime AankoopDatum { get; set; }
        public bool? IsBiologisch { get; set; }
        public string? SoortAppel { get; set; }
    }
}
