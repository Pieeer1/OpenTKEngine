using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Services;

namespace OpenTKEngine.Scenes
{
    public class SceneManager : SingletonService<SceneManager>
    {
        private SceneManager() { }
        private HashSet<Scene> _scenes { get; set; } = new HashSet<Scene>();


        public void AddScene(Scene scene)
        {
            if (_scenes.Any(x => x.Name == scene.Name))
            {
                throw new InvalidOperationException($"Scene {scene.Name} Already Exists");
            }
            scene.Id = _scenes.Any() ? _scenes.Max(x => x.Id) + 1 : 0;
            _scenes.Add(scene);
        }

        public void RemoveScene(string name) 
        {
            if (!_scenes.Any(x => x.Name == name)) { return; }

            _scenes.RemoveWhere(x => x.Name == name);
        }
        public void SwapScene(int id)
        {
            if (!_scenes.Any(x => x.Id == id))
            {
                throw new InvalidOperationException($"Cannot Find Scene with Id {id}");
            }

            if (_scenes.Any(x => x.IsActive)) { _scenes.First(x => x.IsActive).IsActive = false; }
            _scenes.First(x => x.Id == id).IsActive = true;
            
        }
        public void SwapScene(string name)
        {
            if (!_scenes.Any(x => x.Name == name))
            {
                throw new InvalidOperationException($"Cannot Find Scene with Name {name}");
            }

            if (_scenes.Any(x => x.IsActive)) { _scenes.First(x => x.IsActive).IsActive = false; }
            _scenes.First(x => x.Name == name).IsActive = true;
        }
        public void LoadAllScenes()
        {
            foreach (Scene scene in _scenes)
            {
                scene.OnLoad();
            }
        }
        public void LoadScene(int id)
        {
            if (!_scenes.Any(x => x.Id == id))
            {
                throw new InvalidOperationException($"Cannot Find Scene with Id {id}");
            }
            _scenes.First(x => x.Id == id).OnLoad();
        }
        public void LoadScene(string name)
        {
            if (!_scenes.Any(x => x.Name == name))
            {
                throw new InvalidOperationException($"Cannot Find Scene with Name {name}");
            }
            _scenes.First(x => x.Name == name).OnLoad();
        }
        public void DrawActiveScene()
        {
            if (_scenes.Any(x => x.IsActive))
            {
                _scenes.First(x => x.IsActive).OnDraw(); 
            }
        }
        public void UpdateActiveScene()
        {
            if (_scenes.Any(x => x.IsActive))
            {
                _scenes.First(x => x.IsActive).OnUpdate();
            }
        }
        public void RefreshActiveScene()
        {
            if (_scenes.Any(x => x.IsActive))
            {
                _scenes.First(x => x.IsActive).Refresh();
            }
        }
        public void UpdateActiveInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            if (_scenes.Any(x => x.IsActive))
            {
                _scenes.First(x => x.IsActive).UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            }
        }
        public void SetActiveComponentReferences(Attribute att, object value)
        {
            if (_scenes.Any(x => x.IsActive))
            {
                _scenes.First(x => x.IsActive).SetComponentReferencesWithAttribute(att, value);
            }
        }
    }
}
