using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuassianKernelRegression.models
{
    public class SensorDataDTO 
    {
        public int Id { get; set; }
        public decimal? value { get; set; }
        public DateTimeOffset timestamp { get; set; }
        public short Processed { get; set; }
        public bool IsRaw { get; set; }
        public int MonitoringSensorId { get; set; }
    }

}
