namespace OpenTKEngine.Services
{
    public class TimeService : SingletonService<TimeService>
    {
        private TimeService() { }
        public double DeltaTime { get; set; }
    }
}
