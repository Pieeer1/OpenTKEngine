using OpenTKEngine.Entities;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenTKEngine.Scenes;
public static class SceneReader
{
    private readonly static SceneSerializer _serializer = new SceneSerializer();
    public static Scene Load(this string sceneJson)
    {
        Scene scene =  JsonSerializer.Deserialize<Scene>(sceneJson, new JsonSerializerOptions()
        {
            Converters = { _serializer },
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        }) ?? throw new InvalidDataException("Unable to deserialize Scene Object");


        return scene;
    }

    public static string Save(this Scene scene)
    {
        string sceneJson = JsonSerializer.Serialize(scene, new JsonSerializerOptions()
        {
            Converters = { _serializer },
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        });

        return sceneJson;
    }
    private class SceneSerializer : JsonConverter<Scene>
    {
        public override Scene? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var name = reader.GetString() ?? throw new NullReferenceException("Could not get json payload");
            var source = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(name) ?? throw new NullReferenceException("Payload is not valid json");

            source.TryGetValue("Name", out JsonElement sceneName);

            Scene scene = new Scene(sceneName.GetString() ?? throw new NullReferenceException("Could not get name from json payload"));

            source.TryGetValue("Entities", out JsonElement jsonEntities);

            jsonEntities.EnumerateArray().ToList().ForEach(entityJson =>
            {
                Entity entity = entityJson.Deserialize<Entity>() ?? throw new NullReferenceException("Could not deserialize entity.");

                jsonEntities.GetProperty("Components").EnumerateArray().ToList().ForEach(x =>
                {
                    Component component = x.Deserialize<Component>() ?? throw new NullReferenceException("Could not deserialize component.");
                    entity.AddComponent(component);
                });
                RecursivelySearchForChildren(entity, entityJson);
                scene.EntityComponentManager.AddEntity(entity);
            });

            return scene;
        }

        private Entity RecursivelySearchForChildren(Entity parent, JsonElement parentJson)
        {
            parentJson.GetProperty("Children").EnumerateArray().ToList().ForEach(x =>
            {
                Entity childEntity = x.Deserialize<Entity>() ?? throw new NullReferenceException("Could not deserialize child entity.");
                if (x.GetProperty("Children").EnumerateArray().Any())
                {
                    RecursivelySearchForChildren(childEntity, x);
                }

                x.GetProperty("Components").EnumerateArray().ToList().ForEach(x =>
                {
                    Component component = x.Deserialize<Component>() ?? throw new NullReferenceException("Could not deserialize child component.");
                    childEntity.AddComponent(component);
                });
                parent.AddChildEntity(childEntity);
            });

            return parent;
        }

        public override void Write(Utf8JsonWriter writer, Scene value, JsonSerializerOptions options)
        {

            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteStartArray("Entities");
            foreach (Entity entity in value.EntityComponentManager.GetEntities())
            {
                writer.WriteStartObject();
                writer.WriteStartArray("Components");
                foreach (Component component in entity.GetComponents())
                {
                    JsonSerializer.Serialize(writer, component, options);
                }
                writer.WriteEndArray();
                writer.WriteStartArray("Children");
                foreach (Entity childEntity in entity.GetChildEntities())
                {
                    JsonSerializer.Serialize(writer, childEntity, new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                    });
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            
        }
    }

}
