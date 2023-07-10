using OpenTK.Mathematics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Configurations
{
    public class SettingsConfiguration
    {
        public bool IsFullScreen { get; set; }
        public string Resolution { get; set; } = DataManipulationService.ParseResolution(new Vector2i(800, 600));
    }
}
