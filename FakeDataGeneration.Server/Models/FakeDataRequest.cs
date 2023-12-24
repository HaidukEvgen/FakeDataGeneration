namespace FakeDataGeneration.Server.Models
{
    public class FakeDataRequest
    {
        public Region Region { get; set; }
        public int Seed { get; set; }
        public double ErrorsAmount { get; set; }
        public int PageNumber { get; set; }
        public int PerPage { get; set; }
    }
}