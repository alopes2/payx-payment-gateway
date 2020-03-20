using Prometheus;

namespace PayX.Api.Metrics
{
    public class MetricsService : IMetricsService
    {
        private const string ApplicationPrefix = "payx";

        /// <summary>
        /// Total number of exceptions.
        /// </summary>
        public Counter ExceptionsCounter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMetrics"/> class.
        /// </summary>
        public MetricsService()
        {
            ExceptionsCounter = Prometheus.Metrics.CreateCounter(
                name: $"{ApplicationPrefix}_exceptions_total",
                help: "Total count of unhandled exceptions");
        }        
    }
}