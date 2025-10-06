using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class GenericResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public int Code { get; set; }     
        public Template template { get; set; }
        public string Status { get; set; }
        public string RequestId { get; set; }
        public List<string> Errors { get; set; }
        public string token { get; set; }

    }
}
