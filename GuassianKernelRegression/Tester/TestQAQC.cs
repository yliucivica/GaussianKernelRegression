
using GuassianKernelRegression.Regressions;
using GuassianKernelRegression.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BLL.Test.QAQC
{
    public class TestQAQC
    {
        public static void Test_Regression_LevenbergMarquardt(double diameter, double slope, string depthDataFile, string velocityDataFile)
        {
            //read depth and velocity data from csv files
            var depthData = CsvFile.Read(depthDataFile);
            var velocityData = CsvFile.Read(velocityDataFile);

            NonlinearRegression_Accord.Regression_LevenbergMarquardt(
                QaqcConstants.MONITORING_STATION_DEFAULT_DIAMETER, 
                QaqcConstants.MONITORING_STATION_DEFAULT_SLOPE, 
                depthData, 
                velocityData);
            //do the manning roughness regression
        }


        //public static void Test_CalculateIndicator_Accord(double[][] inputs, double[] outputs, double diameter, double roughness, double slope)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.AppendLine("depth,velocity,manningeqution with roughness = " + roughness);
        //    int row = inputs.GetLength(0);
        //    for (int i = 0; i < row; i++)
        //    {
        //        double depth = inputs[i][0];
        //        stringBuilder.AppendLine(depth + "," + outputs[i] + "," + HydraulicAnalysis.ManningVelocity(depth, diameter, roughness, slope).ToString());
        //    }
        //    System.IO.File.WriteAllText(@"C:\Users\yliu\Desktop\Tasks\ManningEquationRegressionAnalysis_Excel\calculatedVelocity.csv", stringBuilder.ToString());
        //}

    }
}
