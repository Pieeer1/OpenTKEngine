using System.Reflection;

namespace OpenTKEngine.Services
{
    public class SingletonService<T> where T : class 
    {
        private static T? _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    ConstructorInfo? c = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }/*empty ctor*/, null);
                    _instance = (T?)c?.Invoke(new object[] { });
                }
                return _instance ?? throw new InvalidOperationException($"Cannot create instance of {typeof(T).Name} as no unparameterized contructor was found.");
            }
        }
    }
}
