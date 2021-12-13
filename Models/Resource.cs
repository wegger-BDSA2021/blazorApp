namespace Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double AverageRating { get; set; }
        public bool Deprecated { get; set; }
    }
}