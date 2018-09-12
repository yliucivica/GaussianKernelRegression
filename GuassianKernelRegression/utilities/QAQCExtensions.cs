using GuassianKernelRegression.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GuassianKernelRegression.utilities
{
    public static class QAQCExtensions
    {      


        public static bool IsNullOrEmpty(this IEnumerable<Object> list)
        {
            return ((list == null) || (list.Count() == 0));
        }

        public static List<SensorDataDTO> Combine(this List<SensorDataDTO> sensorDataDTOs, IEnumerable<SensorDataDTO> inputSensorDataDTOs)
        {
            HashSet<DateTimeOffset> thisDateTimeOffsets = new HashSet<DateTimeOffset>( sensorDataDTOs.Select(x => x.timestamp).ToList());

            foreach(SensorDataDTO sdd in inputSensorDataDTOs)
            {
                if( !thisDateTimeOffsets.Contains(sdd.timestamp))
                {
                    sensorDataDTOs.Add(sdd);
                }
            }

            return sensorDataDTOs;
        }


        /// <summary>
        /// Convert  IEnumerable<SensorDataDTO> to Dictionary , timestamp as key , sensor value as value.
        /// </summary>
        /// <param name="sensorDataDTOs"></param>
        /// <returns></returns>
        public static Dictionary<DateTimeOffset, Double> ToDictionary(this IEnumerable<SensorDataDTO> sensorDataDTOs)
        {
            Dictionary<DateTimeOffset, Double> result = new Dictionary<DateTimeOffset, double>();
            foreach (SensorDataDTO sdd in sensorDataDTOs)
            {
                if (result.ContainsKey(sdd.timestamp))                {
                    
                    if (result[sdd.timestamp] > 0)
                    {
                        result[sdd.timestamp] = (result[sdd.timestamp] + (Double)sdd.value) / 2.0;
                    }
                }
                else
                {
                    result.Add(sdd.timestamp, (Double)sdd.value);
                }

            }

            return result;
        }


        /// <summary>
        /// Convert IEnumerable<SensorDataDTO> to sorted dictionary , timestamp as key , sensor value as value.
        /// </summary>
        /// <param name="sensorDataDTOs"></param>
        /// <returns></returns>
        public static SortedDictionary<DateTimeOffset, Double> ToSortedDictionary(this IEnumerable<SensorDataDTO> sensorDataDTOs)
        {
            SortedDictionary<DateTimeOffset, Double> result = new SortedDictionary<DateTimeOffset, Double>();
            //SortedDictionary<DateTimeOffset, List<Double>> temp = new SortedDictionary<DateTimeOffset, List<Double>>();
            foreach (SensorDataDTO sdd in sensorDataDTOs)
            {
                if (result.ContainsKey(sdd.timestamp))
                {
                    //if (temp.ContainsKey(sdd.timestamp))
                    //{
                    //    temp[sdd.timestamp].Add((Double)sdd.value);                        
                    //}
                    //else
                    //{
                    //    temp.Add(sdd.timestamp, new List<double>() { (Double)sdd.value });
                    //    temp[sdd.timestamp].Add(result[sdd.timestamp]);
                    //}
                    if (result[sdd.timestamp] > 0)
                    {
                        result[sdd.timestamp] = (result[sdd.timestamp] + (Double)sdd.value) / 2.0;
                    }
                }
                else
                {
                    result.Add(sdd.timestamp, (Double)sdd.value);
                }
                
            }

            return result;
        }


        /// <summary>
        /// Group data according their date, put all data in a same day into a dictionary.
        /// Return a dictionary with date as key and the date's corresponding dictionary as value
        /// </summary>
        /// <param name="sortedDict"></param>
        /// <returns></returns>
        public static SortedDictionary<DateTimeOffset, SortedDictionary<DateTimeOffset, Double>> ToSortedDictionaryOfDailyDictionary(this SortedDictionary<DateTimeOffset, Double> sortedDict)
        {
            SortedDictionary<DateTimeOffset, SortedDictionary<DateTimeOffset, Double>> DailyDictionaries = new SortedDictionary<DateTimeOffset, SortedDictionary<DateTimeOffset, Double>>();

            // Get first timestamps in the sorted dictionary
            DateTimeOffset firstTimestampInCurrentDay = sortedDict.Keys.First();

            // create first date's dictionary
            SortedDictionary<DateTimeOffset, Double> firstDateDictionary = new SortedDictionary<DateTimeOffset, Double>();
            firstDateDictionary.Add(firstTimestampInCurrentDay, sortedDict[firstTimestampInCurrentDay]);
            DailyDictionaries.Add(firstTimestampInCurrentDay.Date, firstDateDictionary);
            // Since the sortedDict is ordered list
            // , we are sure once the current date is different with previous date
            // there will not be any previous date's data in the rest of the list.

            foreach (DateTimeOffset key in sortedDict.Keys)
            {
                if (key.Date == firstTimestampInCurrentDay.Date)
                {
                    if (!DailyDictionaries[key.Date].ContainsKey(key))
                    {
                        DailyDictionaries[key.Date].Add(key, sortedDict[key]);
                    }
                }
                else
                {                    
                    firstTimestampInCurrentDay = key;
                    SortedDictionary<DateTimeOffset, Double> currentDateDictionary = new SortedDictionary<DateTimeOffset, Double>();
                    currentDateDictionary.Add(key, sortedDict[key]);
                    DailyDictionaries.Add(firstTimestampInCurrentDay.Date, currentDateDictionary);
                }
            }
            return DailyDictionaries;
        }

    }
}
