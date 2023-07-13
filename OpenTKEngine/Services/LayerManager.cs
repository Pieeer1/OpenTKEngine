using BulletSharp;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Enums;
using OpenTKEngine.Scenes;

namespace OpenTKEngine.Services
{
    public class LayerManager : SingletonService<LayerManager>
    {
        private LayerManager() { }
        public IEnumerable<Entity> GetEntitiesAtLayer(Layer layer)
        { 
            return SceneManager.Instance.ActiveScene.EntityComponentManager.GetEntities().Where(x => x.Layer == layer); //TODO MAY CHANGE TO GENERIC COLLISION COMPONENT
        }

    }
}
