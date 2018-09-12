using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuassianKernelRegression.utilities
{
    public static class QaqcUitilies
    {
        /// <summary>
        /// Example: 13:06 --> round to 13:10
        ///          13:56 --> round to 14:00
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static DateTimeOffset RoundMinutesToNextValueLastDigitIs5Or0(DateTimeOffset timestamp)
        {
            //TODO: implement later
            int minuets = timestamp.Minute;
            int roundedMinute = (((minuets % 5) == 0) ? minuets : ((minuets - (minuets) % 5) + 5));
            return timestamp.AddMinutes(-minuets).AddMinutes(roundedMinute);
        }
    }
}
