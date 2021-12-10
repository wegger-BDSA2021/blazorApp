namespace Models
{
    public class ResourceRequestModel
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int InitialRating { get; set; }
    }
}