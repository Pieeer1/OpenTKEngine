using OpenTK.Mathematics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Tests.Tests.Services
{
    public class DataManipulationServiceTests
    {
        [Fact]
        public void TestGetFloatEnumerableFromVectors()
        {
            List<Vector3> positions = new List<Vector3>()
            {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
            };
            List<Vector3> normals = new List<Vector3>()
            {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
            };
            List<Vector2> textures = new List<Vector2>()
            {
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(-1.0f, -1.0f),
            };

            IEnumerable<float[]> floats = DataManipulationService.GetTextureShadedArrayFromVectors(positions, normals, textures);

            Assert.Equal(3, floats.Count());
            Assert.Equal(8, floats.First().Count());
            Assert.Equal(8, floats.ElementAt(1).Count());
            Assert.Equal(8, floats.Last().Count());
        }
        [Fact]
        public void Test2dTo1d()
        {
            float[][] floats = new float[][]
            {
                new float[]
                {
                    0.0f, 0.0f, 0.0f, 0.0f,
                },
                new float[]
                {
                    1.0f, 1.0f, 1.0f, 1.0f,
                },
            };

            IEnumerable<float> OneDFloats = DataManipulationService.Get1DFrom2D(floats);

            Assert.Equal(8, OneDFloats.Count());
            Assert.Equal(4, OneDFloats.Count(x => x == 0.0f));
            Assert.Equal(4, OneDFloats.Count(x => x == 1.0f));
        }
        [Fact]
        public void GenerateStreamFromStringTests()
        {
            string testString = "abcd";
            MemoryStream stream = (MemoryStream)DataManipulationService.GenerateStreamFromString(testString);
            Assert.Equal(new byte[] { 0x61, 0x62, 0x63, 0x64 }, stream.ToArray());
        }
        [Fact]
        public void TestParseResolutionString()
        {
            string resolutionString = "15x15";
            Vector2i resolutionVector = new Vector2i(15, 15);

            Assert.Equal(DataManipulationService.ParseResolution(resolutionString), resolutionVector);
        }
        [Fact]
        public void TestParseResolutionVector()
        {
            string resolutionString = "15x15";
            Vector2i resolutionVector = new Vector2i(15, 15);

            Assert.Equal(resolutionString, DataManipulationService.ParseResolution(resolutionVector));
        }
    }
}