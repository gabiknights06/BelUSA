using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculation.Persistent.Utilities
{
    public class TaxJarErrorResponseDTO
    {
        public string Status { get; set; }
        public string Error { get; set; }
        public string Detail { get; set; }
    }
}
