using GuassianKernelRegression.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuassianKernelRegression.utilities
{
    /// <summary>
    /// Read data from csv file and save data to SensorDataDTO
    /// </summary>
    public class CsvFile
    {
        /// <summary>
        /// Read data from csv file and save data to list of SensorDataDTO
        /// </summary>
        /// <param name="filePath">The file path of the csv file</param>
        /// <returns>List of SensorDataDTO</returns>
        public static List<SensorDataDTO> Read(string filePath)
        {
            List<SensorDataDTO> data = new List<SensorDataDTO>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                //ignore the first line
                reader.ReadLine();

                //the data line string
                string dataLine = string.Empty;

                //start to read the data
                while((dataLine = reader.ReadLine()) != null)
                {
                    var dataArray = dataLine.Split(',');

                    if (dataArray.Length < 2) continue;

                    data.Add(new SensorDataDTO()
                    {
                        timestamp = DateTime.Parse(dataArray[0]),
                        value = decimal.Parse(dataArray[1])
                    });
                }
            }

            return data;
        }
    }
}
