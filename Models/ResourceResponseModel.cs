using System;
using System.Collections.Generic;

namespace Models
{
    public class ResourceResponseModel
    {
        public string TitleFromUser { get; set; }
        public string TitleFromSource { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime TimeOfReference { get; set; }
        public string Url { get; set; }
        public string HostBaseUrl { get; set; }
        public ICollection<string> TagsFoundInSource { get; set; } = new List<string>();
        public int InitialRating { get; set; }
        public Boolean Deprecated { get; set; }
        public DateTime LastCheckedForDeprecation { get; set; }
        public bool IsVideo { get; set; }
        public bool IsOfficialDocumentation { get; set; }
    }
}