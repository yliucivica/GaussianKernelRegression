using Accord.Math;
using Accord.Math.Optimization;
using Accord.Statistics.Models.Regression.Fitting;
using GuassianKernelRegression.models;
using GuassianKernelRegression.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuassianKernelRegression.Regressions
{
    public class NonlinearRegression_Accord
    {
        /// <summary>
        /// calculate scattergraph indicator using Accord Non-linear regression library
        /// </summary>
        /// <param name="diameter">sewer pipe diameter, m</param>
        /// <param name="slope"></param>
        /// <param name="depthData">all depth sensor data that fall in time range specified by this indicator result's start and end time </param>
        /// <param name="velocityData">all velocity sensor data that fall in time range specified by this indicator result's start and end time </param>
        /// <returns></returns>
        public static void Regression_LevenbergMarquardt(double diameter, double slope,
            List<SensorDataDTO> depthData, List<SensorDataDTO> velocityData)
        {

            // validate and get list of depth-velocity pair with common timestamp
            List<Tuple<double, double>> depth_velocity_list = ValidateAndCombineInputOutputDataToDepthVelocityPairList(depthData, velocityData);

            if (depth_velocity_list.Count <= 10)
            {
                throw new Exception("Error: don't have enough data for regression: your total number of input data, which is " + depth_velocity_list.Count + ", less than 10");
            }



            // transfer depth_velocity pair list to array to improve loop speed
            Tuple<double, double>[] depth_velocity_array = depth_velocity_list.ToArray();

            //get depth and velocity data to array
            int numData = depth_velocity_array.Length;
            double[,] data = new double[2, numData];    //first row is depth as input, second row is velocity as output
            for (int i = 0; i < numData; i++)
            {
                data[0, i] = Convert.ToDouble(depth_velocity_array[i].Item1);
                data[1, i] = Convert.ToDouble(depth_velocity_array[i].Item2);
            }

            // Extract inputs and outputs
            double[][] inputs_depth_array = data.GetRow(0).ToJagged();
           

            double[] outputs = data.GetRow(1);

            // Create a Nonlinear regression using 
            var nls = new NonlinearLeastSquares()
            {
                NumberOfParameters = 1,

                // Initialize to some random values
                StartValues = new[] { 5.5 },

                // use Lanfear-Coll method  :  V_lc = C_lc * R_lc^(2/3)
                // x is list of R_lc^(2/3), w is C_lc             
                Function = (w, x) => w[0] * x[0],

                // Derivative in respect to the weights:
                Gradient = (w, x, r) =>
                {
                    r[0] = x[0];
                    //Console.WriteLine(h + ": " + r[0]);                    
                    //h++;
                },

                Algorithm = new LevenbergMarquardt()
                {
                    MaxIterations = 20,
                    Tolerance = 0
                }
            };


            // regression
            var regression = nls.Learn(inputs_depth_array, outputs);


            // get the roughness
            double C_lc = regression.Coefficients[0];
            double roughness = Math.Sqrt(slope) / C_lc;


      

            //calculate R-Squred
            double[] velocitys_manning_calculated = regression.Transform(inputs_depth_array); 

            
        }







        ///////////////////////////////////////////////////////////////////
        // helpers
        public static List<Tuple<double, double>> ValidateAndCombineInputOutputDataToDepthVelocityPairList(List<SensorDataDTO> depthData, List<SensorDataDTO> velocityData)
        {
            List<Tuple<double, double>> depth_velocity_list = new List<Tuple<double, double>>();

            // round timestamp to nearest number that end with 5 or 0  
            foreach (SensorDataDTO s in depthData)
            {
                s.timestamp = QaqcUitilies.RoundMinutesToNextValueLastDigitIs5Or0(s.timestamp);
            }
            foreach (SensorDataDTO s in velocityData)
            {
                s.timestamp = QaqcUitilies.RoundMinutesToNextValueLastDigitIs5Or0(s.timestamp);
            }


            //transfer data to dictionary to get better performance in the process of getting list of depth-velocity pair with common timestamp
            Dictionary<DateTimeOffset, Double> depth_dict = depthData.ToDictionary();
            HashSet<DateTimeOffset> depthTimestampSet = new HashSet<DateTimeOffset>(depth_dict.Keys);

            Dictionary<DateTimeOffset, Double> velocity_dic = velocityData.ToDictionary();
            HashSet<DateTimeOffset> velocityTimestampSet = new HashSet<DateTimeOffset>(velocity_dic.Keys);

            HashSet<DateTimeOffset> commonTimestampSet = new HashSet<DateTimeOffset>(depthTimestampSet.Intersect(velocityTimestampSet));



            // get list of depth-velocity pair with common timestamp            
            foreach (DateTimeOffset dt in commonTimestampSet)
            {
                depth_velocity_list.Add(new Tuple<double, double>(depth_dict[dt], velocity_dic[dt]));
            }

            return depth_velocity_list;
        }

    }
}
