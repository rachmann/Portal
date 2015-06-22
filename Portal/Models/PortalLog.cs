using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class PortalLog
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
        public string Message { get; set; }
        public string User { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string HttpMethod { get; set; }
        public string ContentType { get; set; }
        public string ContentEncoding { get; set; }
    }
}
