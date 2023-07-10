using OpenTKEngine.Enums;

namespace OpenTKEngine.Services
{
    public class InputFlagService : SingletonService<InputFlagService>
    {
        private InputFlagService() { }

        public InputFlags ActiveInputFlags { get; set; } = InputFlags.Reset;
    }
}
