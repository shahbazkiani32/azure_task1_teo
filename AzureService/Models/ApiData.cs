using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureService.Models
{

    public class ApiPayLoad
    {
        public List<ApiData>? PayLoadData { get; set; } 
    }
    public class ApiData
    {
        public int? Count { get; set; }
        public string? Logkey { get; set; }
        public List<Entries>? Entries { get; set; }
    }
    public class Entries
    {
        public string? API { get; set; }
        public string? Description { get; set; }
        public string? Auth { get; set; }
        public string? HTTPS { get; set; }
        public string? Cors { get; set; }
        public string? Link { get; set; }
        public string? Category { get; set; }
    }
}
