using OpenTK.Mathematics;

namespace OpenTKEngine.Services
{
    public static class DataManipulationService
    {
        public static IEnumerable<float[]> GetTextureShadedArrayFromVectors(IEnumerable<Vector3> positions, IEnumerable<Vector3> normals, IEnumerable<Vector2> textures)
        {
            if (positions.Count() != normals.Count() || normals.Count() != textures.Count() || positions.Count() != textures.Count())
            {
                throw new InvalidOperationException("Cannot create float array of different sized Enumerables");
            }
            for (int i = 0; i < positions.Count(); i++)
            {
                yield return new float[] {
                positions.ElementAt(i).X,
                positions.ElementAt(i).Y,
                positions.ElementAt(i).Z,

                normals.ElementAt(i).X,
                normals.ElementAt(i).Y,
                normals.ElementAt(i).Z,

                textures.ElementAt(i).X,
                textures.ElementAt(i).Y };
            }
        }
        public static IEnumerable<T> Get1DFrom2D<T>(this IEnumerable<IEnumerable<T>> values)
        { 
            foreach (IEnumerable<T> enu in values)
            {
                foreach (T t in enu)
                {
                    yield return t;
                }
            }
        }
    }
}
