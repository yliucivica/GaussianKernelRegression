using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuassianKernelRegression.utilities
{
    public static class QaqcConstants
    {
        /// <summary>
        /// Set rain tail to 24 hours
        /// </summary>
        public const int RAIN_TAIL_HOURS = 24;

        /// <summary>
        /// unit: meter. If station does not contain valid diameter, use this constants as default
        /// </summary>
        public const double MONITORING_STATION_DEFAULT_DIAMETER = 0.375;

        /// <summary>
        /// If station does not contain valid SLOPE, use this constants as default
        /// </summary>
        public const double MONITORING_STATION_DEFAULT_SLOPE = 0.005;


        /// <summary>
        /// Actual standard devication = averageOfVelocityValues * VELOCITY_STANDARD_DEVIATION_NORMALIZED
        /// </summary>
        public const double VELOCITY_STANDARD_DEVIATION_NORMALIZED = 0.10; //TODO: more research needed


        /// <summary>
        /// Actual FLOW standard devication = averageOfFlowValues * FLOW_STANDARD_DEVIATION_NORMALIZED
        /// </summary>
        public const double FLOW_STANDARD_DEVIATION_NORMALIZED = 0.15; //TODO: more research needed


        /// <summary>
        /// Actual depth standard devication = averageOfDepthValues * DEPTH_STANDARD_DEVIATION_NORMALIZED
        /// </summary>
        public const double DEPTH_STANDARD_DEVIATION_NORMALIZED = 0.10; //TODO: more research needed


        /// <summary>
        /// This value is concluded by research on "good" data
        /// </summary>
        public const double VELOCITY_STANDARD_DEVIATION_UPPER = 0.09; //TODO: more research needed

        /// <summary>
        /// This value is concluded by research on "good" data
        /// </summary>
        public const double VELOCITY_STANDARD_DEVIATION_LOWER = 0.03;//TODO: more research needed

        /// <summary>
        /// This value is concluded by research on "good" data
        /// </summary>
        public const double DEPTH_STANDARD_DEVIATION_UPPER = 0.0057;//TODO: more research needed

        /// <summary>
        /// This value is concluded by research on "good" data
        /// </summary>
        public const double DEPTH_STANDARD_DEVIATION_LOWER = 0.0041;//TODO: more research needed

        /// <summary>
        /// This vale is given in the Indicator Calculation document. Unit m
        /// </summary>
        public const double DEPTH_SPIKE_CONSTANT_COEFFICIENT = 0.12;//TODO: more research needed

        /// <summary>
        /// This vale is given in the Indicator Calculation document. Unit m
        /// </summary>
        public const double DEPTH_SPIKE_MIN_SENSOR_VALUE = 0.1;//TODO: more research needed


        // <summary>
        /// This vale is given in the Indicator Calculation document. Unit m
        /// </summary>
        public const double VELOCITY_SPIKE_CONSTANT_COEFFICIENT = 0.8;//TODO: more research needed

        /// <summary>
        /// This vale is given in the Indicator Calculation document. Unit m
        /// </summary>
        public const double VELOCITY_SPIKE_MIN_SENSOR_VALUE = 0.1;//TODO: more research needed
    }
}
