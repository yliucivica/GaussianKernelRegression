using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GuassianKernelRegression
{
    public class GaussianKernelRegression
    {
        /// <summary>
        /// used in gaussian function
        /// </summary>
        public double Sigma { get; }

        public double KernelWindowSize { get; }

        public GaussianKernelRegression(double sigma, double kernelWindowSize)
        {
            Sigma = sigma;
            KernelWindowSize = kernelWindowSize;
        }


        /// <summary>
        /// Gaussian funtion used to computes the weight to apply for data point xi based on its distance from our query point x.
        /// </summary>
        /// <param name="x_queryLocation"> the x value of query point</param>
        /// <param name="xi">the x value that around query point</param>
        /// <returns></returns>
        public double GaussianFuntion(double x_queryLocation, double xi)
        {
            return Math.Pow(Math.E, -(Math.Pow((xi - x_queryLocation), 2)) / (2 * Math.Pow(Sigma, 2)));
        }

        /// <summary>
        /// Given the list of points winthin the kernel window, Calculate the query point's regression value(calculated y value)
        /// </summary>
        /// <param name="x_queryLocation"></param>
        /// <param name="kernelPoints">is all points that in the kernel "window", use Tuple<double, double> repenst a point , x = item1, y=item2 </dobule></param>
        /// <returns></returns>
        public double GetRegressionValue(double x_queryLocation, SortedList<double, double> kernelPoints)
        {
            // default: sorted dictionary is ordered by key in ascending order
            // we will use this characters in later iteration 

            if (kernelPoints.Count == 0)
            {
                throw new Exception("Kernel windows for " + "The query location x: " + x_queryLocation + " does't contain any points");
            }
            if (x_queryLocation < kernelPoints.First().Key || x_queryLocation > kernelPoints.Last().Key)
            {
                throw new Exception("The query location x: " + x_queryLocation + " does't fall in kernel window");
            }

            // regression value is the sum of weighted y devided by sum of weights
            double sumOfWeightedYValue = 0.0;
            double sumOfWeights = 0.0;
            foreach (var tp in kernelPoints)
            {
                double x_i = tp.Key;
                double y_i = tp.Value;

                //calculate weight and weighted y value
                double w_i = GaussianFuntion(x_queryLocation, x_i);
                double y_i_weighted = y_i * w_i;

                //added up
                sumOfWeights = sumOfWeights + w_i;
                sumOfWeightedYValue = sumOfWeightedYValue + y_i_weighted;
            }

            // calculate regression value 
            double y_RegressionValue = sumOfWeightedYValue / sumOfWeights;

            return y_RegressionValue;
        }


        public SortedList<double, double> GetRegressionValues(List<double> x_queryLocations, SortedList<double, double> points)
        {
            // By default, sorted dictionary is ordered by key in ascending order
            // we will use this characters in later iteration 

            SortedList<double, double> regressionValues = new SortedList<double, double>();

            double firstX = points.First().Key;
            double firstY = points.First().Value;
            foreach (double curr_x_queryLocation in x_queryLocations)
            {
                if(Math.Abs(curr_x_queryLocation-firstX) <=0.001)
                {
                    regressionValues.Add(curr_x_queryLocation, firstY);
                }
                else
                {
                    SortedList<double, double> currentKernelPoints = GetAllPointsThatFallInKenerWindow(curr_x_queryLocation, points);
                    double regressionValue = GetRegressionValue(curr_x_queryLocation, currentKernelPoints);
                    regressionValues.Add(curr_x_queryLocation, regressionValue);
                }
            }
                

            return regressionValues;
        }


        public SortedList<double, double> GetAllPointsThatFallInKenerWindow(double x_queryLocation, SortedList<double, double> points)
        {
            //set up kernel window (range)
            double kernelWindowStart = x_queryLocation - KernelWindowSize;
            double kernelWindowEnd = x_queryLocation + KernelWindowSize;

            SortedList<double, double> kernelPoints = new SortedList<double, double>(); 
            
            for (int i = 0; i < points.Count; i++)
            {
                var p = points.ElementAt(i);
                if (p.Key >= kernelWindowStart && p.Key < kernelWindowEnd)
                {
                    kernelPoints.Add(p.Key, p.Value);
                    i = i + 1;
                    while (i < points.Count && points.ElementAt(i).Key < kernelWindowEnd)
                    {
                        var curr_p = points.ElementAt(i);
                        kernelPoints.Add(curr_p.Key, curr_p.Value);
                        i++;
                    }

                    break;
                }
            }

            return kernelPoints;
        }

    }//end of class
}//end of namespace

