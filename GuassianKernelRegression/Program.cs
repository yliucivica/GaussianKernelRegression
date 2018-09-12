using Accord.Math;
using Accord.Math.Optimization;
using Accord.Statistics.Models.Regression.Fitting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuassianKernelRegression
{
    class Program
    {
        static void Main(string[] args)
        {

            // FM1 three month data
            //string depthFilePath = @"~/../../../QAQC/data/BAIF_DEPTH_FM1_STATION_ID_1278_PERIOD_JULY012017_OCT012017.csv";
            //string velocityFilePath = @"~/../../../QAQC/data/BAIF_VELOCITY_FM1_STATION_ID_1278_PERIOD_JULY012017_OCT012017.csv";

            // FM2 three month data
            //string depthFilePath = @"~/../../../QAQC/data/BAIF_DEPTH_FM2_STATION_ID_1279_PERIOD_JULY012017_OCT012017.csv";
            //string velocityFilePath = @"~/../../../QAQC/data/BAIF_VELOCITY_FM2_STATION_ID_1279_PERIOD_JULY012017_OCT012017.csv";


            // FM3 three month data
            //string depthFilePath = @"~/../../../QAQC/data/BAIF_DEPTH_FM3_STATION_ID_1280_PERIOD_JULY012017_OCT012017.csv";
            //string velocityFilePath = @"~/../../../QAQC/data/BAIF_VELOCITY_FM3_STATION_ID_1280_PERIOD_JULY012017_OCT012017.csv";


            // FM2 three month data 2018-JAN  TO APRIL
            //string depthFilePath = @"~/../../../QAQC/data/BAIF_DEPTH_FM2_STATION_ID_1279_PERIOD_JAN012018_APR012018.csv";
            //string velocityFilePath = @"~/../../../QAQC/data/BAIF_VELOCITY_FM2_STATION_ID_1279_PERIOD_JAN012018_APR012018.csv";



            // FM2 three month data 2018-JAN  TO APRIL fROM India team
            string depthFilePath = @"~/data/BAIF_FM2_STATION_1279_DEPTH_01012018_31032018_FROM_IND.csv";
            string velocityFilePath = @"~/data/BAIF_FM2_STATION_1279_VELOCITY_01012018_31032018_FROM_IND.csv";



            // FM1 one day's data
            //string depthFilePath = @"~/../../../QAQC/data/BAIF_DEPTH_FM1_less_data.csv";
            //string velocityFilePath = @"~/../../../QAQC/data/BAIF_VELOCITY_FM1_less_data.csv";












            return;




            String fileName = @"C:\Users\yliu\Desktop\Tasks\QAQC_Research\Baif_Dev_FM1_Pattern_for_Gaussian_Test\Baif_Dev_FM1_Pattern_for_Gaussian_Test_CSV.csv";


            String[] lines = System.IO.File.ReadAllLines(fileName);


            SortedList<double, double> points = new SortedList<double, double>();
            for (int i = 1; i < lines.Length; i++)
            {
                double xi = double.Parse(lines[i].Split(',')[1]);
                double yi = double.Parse(lines[i].Split(',')[3]);

                points.Add(xi, yi);
            }

            List<double> x_queryLocations = new List<double>();

            for (int k = 0; k < points.Count; k++)
            {
                var p = points.ElementAt(k);
                if (k == 0)
                {
                    x_queryLocations.Add(p.Key);
                }
                else
                {
                    if ((k + 1) % 12 == 0)
                    {
                        x_queryLocations.Add(p.Key);
                    }
                }
            }

            double sigma = 5;
            double kernelWindowSize = 6;
            SortedList<double, double> regressionValues = new GaussianKernelRegression(sigma, kernelWindowSize).GetRegressionValues(x_queryLocations, points);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var p in regressionValues)
            {
                stringBuilder.AppendLine(p.Key + "," + p.Value);
            }

            System.IO.File.WriteAllText(@"C: \Users\yliu\Desktop\Tasks\QAQC_Research\Baif_Dev_FM1_Pattern_for_Gaussian_Test\Baif_Dev_FM1_Pattern_for_Gaussian_Regression_result.csv", stringBuilder.ToString());



            // using regression values to evaluate other points value
            x_queryLocations = new List<double>();
            for (int k = 1; k <= 288; k++)
            {

                x_queryLocations.Add(k);

            }

            sigma = 6;
            kernelWindowSize = 25;

            SortedList<double, double> predictedValues = new GaussianKernelRegression(sigma, kernelWindowSize).GetRegressionValues(x_queryLocations, regressionValues);

            stringBuilder = new StringBuilder();

            foreach (var p in predictedValues)
            {
                stringBuilder.AppendLine(p.Key + "," + p.Value);
            }

            System.IO.File.WriteAllText(@"C: \Users\yliu\Desktop\Tasks\QAQC_Research\Baif_Dev_FM1_Pattern_for_Gaussian_Test\Baif_Dev_FM1_Pattern_for_Gaussian_Regression_predictedValues.csv", stringBuilder.ToString());























            // Suppose we would like to map the continuous values in the
            // second column to the integer values in the first column.
            double[,] data =
            {
                { -40,    -21142.1111111111 },
                { -30,    -21330.1111111111 },
                { -20,    -12036.1111111111 },
                { -10,      7255.3888888889 },
                {   0,     32474.8888888889 },
                {  10,     32474.8888888889 },
                {  20,      9060.8888888889 },
                {  30,    -11628.1111111111 },
                {  40,    -15129.6111111111 },
            };


            // Extract inputs and outputs
            double[][] inputs = data.GetColumn(0).ToJagged();
            double[] outputs = data.GetColumn(1);

            // Create a Nonlinear regression using 
            var nls = new NonlinearLeastSquares()
            {
                NumberOfParameters = 3,

                // Initialize to some random values
                StartValues = new[] { 4.2, 0.3, 1 },

                // Let's assume a quadratic model function: ax² + bx + c
                Function = (w, x) => w[0] * x[0] * x[0] + w[1] * x[0] + w[2],

                // Derivative in respect to the weights:
                Gradient = (w, x, r) =>
                {
                    r[0] = w[0] * w[0]; // w.r.t a: a²  // https://www.wolframalpha.com/input/?i=diff+ax²+%2B+bx+%2B+c+w.r.t.+a
                    r[1] = w[0];       // w.r.t b: b   // https://www.wolframalpha.com/input/?i=diff+ax²+%2B+bx+%2B+c+w.r.t.+b
                    r[2] = 1;          // w.r.t c: 1   // https://www.wolframalpha.com/input/?i=diff+ax²+%2B+bx+%2B+c+w.r.t.+c
                },

                Algorithm = new LevenbergMarquardt()
                {
                    MaxIterations = 100,
                    Tolerance = 0
                }
            };


            var regression = nls.Learn(inputs, outputs);

            // Use the function to compute the input values
            double[] predict = regression.Transform(inputs);







            // Baif development fm1 regression values from sunday's mean pattern
            double[,] data_Baif_FM1_Gaussian_Regression_values =
            {
                {1,0.843583238859984},
                {12,0.808214893088327},
                {24,0.769726299319266},
                {36,0.740846246915708},
                {48,0.725768880553313},
                {60,0.725449048010978},
                {72,0.751216253011904},
                {84,0.819776029981002},
                {96,0.874824753578552},
                {108,0.904100592020644},
                {120,0.918876958470868},
                {132,0.918613100338434},
                {144,0.912382838858771},
                {156,0.904891186350628},
                {168,0.896757292426472},
                {180,0.892574620228816},
                {192,0.888423797146568},
                {204,0.892518295131734},
                {216,0.899512739057531},
                {228,0.903684396998739},
                {240,0.905212224625666},
                {252,0.906419205656247},
                {264,0.893860862478626},
                {276,0.87675216096711},
                {288,0.846998769532342}
            };






            //################## non linear ###########################//

            // Extract inputs and outputs
            double[][] inputs_Baif = data_Baif_FM1_Gaussian_Regression_values.GetColumn(0).ToJagged();
            double[] outputs_Baif = data_Baif_FM1_Gaussian_Regression_values.GetColumn(1);

            double delta = 0.0000000001;

            double pi = Math.PI;
            double flowDayCycle = 288;

            double modelCycle = 2 * pi / 288;


            // Create a Nonlinear regression using 
            // with 5 parameters , fixed cycle:2*pi/288
            // accept solution: ok but not perfect
            var nls_Baif = new NonlinearLeastSquares()
            {
                NumberOfParameters = 5,

                // Initialize to some random values
                //StartValues = new[] { 1.3, 0.258, -1, 1.1, 3.8, 2 },
                //StartValues = new[] { 0.2, -1, 0.2, 3.8, 0.8 },
                StartValues = new[] { 0.0, 0.0, 0.0, 0.0, 0.0 },
                //StartValues = new[] { 5, 0.022119618, -6, 0.1, 1, 4 },

                // Let's assume a quadratic model function: w0*(sin(0.022119618*x +w1))^2 + w2*sin(0.022119618*x + w3) +w4
                Function = (w, x) => w[0] * Math.Pow(Math.Sin(modelCycle * x[0] + w[1]), 2) + w[2] * Math.Sin(modelCycle * x[0] + w[3]) + w[4],

                // Derivative in respect to the weights:
                // accept solution: ok but not perfect
                Gradient = (w, x, r) =>
                {

                    // function: w0*(sin(w1x+w2))^2 + w3*sin(w1*x+w4)+w5
                    // w[0] derivative: sin(c*x+w_1)^2
                    r[0] = Math.Pow(Math.Sin(modelCycle * x[0] + w[1]), 2);



                    //w[1] derivative: -b^2*(2*a*sin(b*x+c)^2-2*a*cos(b*x+c)^2+d*sin(b*x+e))
                    //r[1] =  w[1];
                    //r[1] = 0.022119618;


                    // w[1] derivative: 2*w_0*cos(w_1+c*x)*sin(w_1+c*x)
                    r[1] = 2 * w[0] * Math.Cos(w[1] + modelCycle * x[0]) * Math.Sin(w[1] + modelCycle * x[0]);


                    // w2 derivative: sin(c*x+w_3)
                    r[2] = Math.Sin(modelCycle * x[0] + w[3]);


                    // w3 derivative: w_2*cos(w_3+c*x)
                    r[3] = w[2] * Math.Cos(w[3] + modelCycle * x[0]);

                    r[4] = 1;
                },

                Algorithm = new LevenbergMarquardt()
                {
                    MaxIterations = 10000,
                    Tolerance = 0
                }

                //Algorithm = new GaussNewton()
                //{
                //    MaxIterations = 1000,
                //    Tolerance = 0
                //}
            };


            // another soloution : 7 parameter sin wave
            // Create a Nonlinear regression using            
            //var nls_Baif = new NonlinearLeastSquares()
            //{
            //    NumberOfParameters = 7,

            //    // Initialize to some random values
            //    //StartValues = new[] { 1.3, 0.258, -1, 1.1, 3.8, 2 },
            //    //StartValues = new[] { 0.2, -1, 0.2, 3.8, 0.8 },
            //    StartValues = new[] { 0.1, 0.02, -0.01, 0.0, 0.0, 0.0, 0.1 },
            //    //StartValues = new[] { 5, 0.022119618, -6, 0.1, 1, 4 },

            //    // Let's assume a quadratic model function: w0*(sin(w1*x +w2))^2 + w3*sin(w4*x + w5) +w6
            //    Function = (w, x) => w[0] * Math.Pow(Math.Sin(w[1] * x[0] + w[2]), 2) + w[3] * Math.Sin(w[4] * x[0] + w[5]) + w[6],

            //    // Derivative in respect to the weights:               
            //    Gradient = (w, x, r) =>
            //    {

            //        // function: w0*(sin(w1x+w2))^2 + w3*sin(w1*x+w4)+w5
            //        // w[0] derivative: sin(w_1*x+w_2)^2
            //        r[0] = Math.Pow(Math.Sin(w[1] * x[0] + w[2]), 2);

            //        // w[1] derivative: 2*w_0*x*cos(x*w_1+w_2)*sin(x*w_1+w_2)
            //        r[1] = 2 * w[0] * x[0] * Math.Cos(w[1] * x[0] + w[2]) * Math.Sin(w[1] * x[0] + w[2]);


            //        // w2 derivative: 2*w_0*cos(w_2+w_1*x)*sin(w_2+w_1*x)
            //        r[2] = 2 * w[0] * Math.Cos(w[1] * x[0] + w[2]) * Math.Sin(w[1] * x[0] + w[2]);


            //        // w3 derivative: sin(w_4*x+w_5)
            //        r[3] = Math.Sin(w[4] * x[0] + w[5]);

            //        // w4 derivative: w_3*x*cos(x*w_4+w_5)
            //        r[4] = w[3] * x[0] * Math.Cos(w[4] * x[0] + w[5]);


            //        // w5 derivative: w_3*cos(w_5+w_4*x)
            //        r[5] = w[3] * Math.Cos(w[4] * x[0] + w[5]);

            //        // w6 derivative: 1
            //        r[6] = 1;


            //    },

            //    Algorithm = new LevenbergMarquardt()
            //    {
            //        MaxIterations = 100000,
            //        Tolerance = 0
            //    }

            //    //Algorithm = new GaussNewton()
            //    //{
            //    //    MaxIterations = 100000,
            //    //    Tolerance = 0
            //    //}
            //};




            // another soloution :  poly
            // Create a Nonlinear regression using  
            // result : not acceptable
            //var nls_Baif = new NonlinearLeastSquares()
            //{
            //    NumberOfParameters = 6,


            //    StartValues = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },

            //    // Let's assume a quadratic model function: w0*x^5 +w1*x^4 + w2*x^3 + w3*x^2 +w4*x[0] +w5
            //    Function = (w, x) => w[0] * Math.Pow(x[0], 5) + w[1] * Math.Pow(x[0], 4) + w[2] * Math.Pow(x[0], 3) + w[3] * Math.Pow(x[0], 2) + w[4] * x[0] + w[5],

            //    // Derivative in respect to the weights:               
            //    Gradient = (w, x, r) =>
            //    {


            //        r[0] = Math.Pow(x[0], 5);


            //        r[1] = Math.Pow(x[0], 4);


            //        r[2] = Math.Pow(x[0], 3);

            //        r[3] = Math.Pow(x[0], 2);

            //        r[4] = x[0];

            //        r[5] = 1;


            //    },

            //    Algorithm = new LevenbergMarquardt()
            //    {
            //        MaxIterations = 1000000,
            //        Tolerance = 0
            //    }

            //    //Algorithm = new GaussNewton()
            //    //{
            //    //    MaxIterations = 100000,
            //    //    Tolerance = 0
            //    //}
            //};


            var regression_Baif = nls_Baif.Learn(inputs_Baif, outputs_Baif);

            // Use the function to compute the input values
            double[] predict_Baif = regression_Baif.Transform(inputs_Baif);


            Console.WriteLine(string.Join(",", regression_Baif.Coefficients));

            /*
            //############### linear #####################///
            // Let's retrieve the input and output data:
            double[]inputs_Baif = data_Baif_FM1_Gaussian_Regression_values.GetColumn(0);
            double[] outputs_Baif = data_Baif_FM1_Gaussian_Regression_values.GetColumn(1);

            // We can create a learning algorithm
            var ls = new PolynomialLeastSquares()
            {
                Degree = 5
            };

            // Now, we can use the algorithm to learn a polynomial
            PolynomialRegression poly = ls.Learn(inputs_Baif, outputs_Baif);

            // The learned polynomial will be given by
            string str = poly.ToString("N1"); // "y(x) = 1.0x^2 + 0.0x^1 + 0.0"

            // Where its weights can be accessed using
            double[] weights = poly.Weights;   // { 1.0000000000000024, -1.2407665029287351E-13 }
            double intercept = poly.Intercept; // 1.5652369518855253E-12

            // Finally, we can use this polynomial
            // to predict values for the input data
            double[] predict_Baif = poly.Transform(inputs_Baif);
            */





            stringBuilder = new StringBuilder();

            for (int i = 0; i < inputs_Baif.Length; i++)
            {
                stringBuilder.AppendLine(inputs_Baif[i][0] + "," + predict_Baif[i]);
            }

            System.IO.File.WriteAllText(@"C: \Users\yliu\Desktop\Tasks\QAQC_Research\Baif_Dev_FM1_Pattern_for_Gaussian_Test\Baif_Dev_FM1_Pattern_NonlinearRegression_Predict_values.csv", stringBuilder.ToString());

            Console.ReadKey();
        }
    }
}
