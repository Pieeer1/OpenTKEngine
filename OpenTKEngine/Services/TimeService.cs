namespace OpenTKEngine.Services
{
    public class TimeService : SingletonService<TimeService>
    {
        private TimeService() { }
        private double _deltaTime;
        public double DeltaTime 
        { 
            get => _deltaTime;
            set
            {
                UpdatedTriggered();
                _deltaTime = value;
            }
        }
        public event EventHandler<TimeArgs>? OnUpdateTriggered;
        public void UpdatedTriggered() // can potentially get expensive might need to refactor
        {
            if (OnUpdateTriggered != null)
            { 
                OnUpdateTriggered(this, new TimeArgs(DeltaTime));
            }
        }
    }
    public class TimeArgs : EventArgs
    {
        public TimeArgs(double deltaTime)
        {
            DeltaTime = deltaTime;
        }

        public double DeltaTime { get; private set; }
    }
}
