using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zSprite.Json;

namespace zSprite
{
    internal static class Serializers
    {
        #region Scene
        internal static void deserializeScene(string file)
        {
            using (var sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var joRoot = JsonReader.ParseObject(json);

                loadGameObjects(Root.instance.RootObject, joRoot);
            }
        }

        internal static void loadGameObjects(GameObject go, JsonObject parent)
        {
            var gameObjects = parent.get<JsonObject>("gameObjects");
            if (gameObjects != null)
            {
                var items = gameObjects.get<JsonArray>("items");
                if (items != null)
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        var item = (JsonObject)items[i];
                        var name = (string)item["name"];
                        var goParent = go.createChild(name);

                        var properties = item.get<JsonObject>("properties");
                        foreach (var key in properties.Keys)
                        {
                            var value = properties[key];
                            goParent.add(key, value);
                        }

                        //lets clear to much sure the memory is gc'ed
                        properties.Clear();
                        loadGameObjects(goParent, gameObjects);
                    }
                }
            }
        }
        #endregion
    }
}
