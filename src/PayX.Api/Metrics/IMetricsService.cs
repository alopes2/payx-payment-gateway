using Prometheus;

namespace PayX.Api.Metrics
{
    public interface IMetricsService
    {
        /// <summary>
        /// Total number of exceptions thrown counter.
        /// </summary>
        Counter UnhandledExceptionsCounter { get; }         
    }
}