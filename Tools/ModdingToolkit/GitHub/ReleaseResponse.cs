using System;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace ModdingToolkit.GitHub
{
    public class ReleaseResponse
    {
        public string url { get; set; }
        public long id { get; set; }
        
        public string tag_name { get; set; }
        public string target_commitish { get; set; }

        public string name { get; set; }
        public bool draft { get; set; }
        public bool prerelease { get; set; }

        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }

        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
    }
}