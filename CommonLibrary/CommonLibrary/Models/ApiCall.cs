using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Models
{
    public partial class ApiCall
    {
        public string CorrelationId { get; set; }
        public int? ClientId { get; set; }
        public string Url { get; set; }
        public int ResourceId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public float? ElapsedTime { get; set; }
        public int? CompanyId { get; set; }
        public string IpAddress { get; set; }
        public bool? IsTesting { get; set; }
        public string Type { get; set; }
    }
}
