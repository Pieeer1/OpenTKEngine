using OpenTKEngine.Scenes;

namespace OpenTKEngine.Services
{
    public class SceneScopedService<T, X> where T : SingletonService<T> where X : Scene
    {
        private static Dictionary<Type, T> _scopedServicePairs = new Dictionary<Type, T>();

        public static T Instance 
        {
            get
            {
                if (!_scopedServicePairs.TryGetValue(typeof(X), out _))
                {
                    _scopedServicePairs.Add(typeof(X), SingletonService<T>.Instance);
                }
                return _scopedServicePairs[typeof(X)];
            }
        }

    }
}
