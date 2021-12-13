namespace Models
{
    public class Resource
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public double averageRating { get; set; }
        public bool deprecated { get; set; }
    }
}