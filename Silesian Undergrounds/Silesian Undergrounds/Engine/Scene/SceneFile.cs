using System.Collections.Generic;
using Newtonsoft.Json;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class SceneFile
    {
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }
        [JsonProperty("infinite", Required = Required.Always)]
        public bool Infinite { get; set; }
        [JsonProperty("nextlayerid")]
        public int NextLayerId { get; set; }
        [JsonProperty("nextobjectid")]
        public int NextObjectId { get; set; }
        [JsonProperty("orientation")]
        public string Orientation { get; set; }
        [JsonProperty("renderorder")]
        public string RenderOrder { get; set; }
        [JsonProperty("tiledversion")]
        public string TiledVersion { get; set; }
        [JsonProperty("tileheight", Required = Required.Always)]
        public int TileHeight { get; set; }
        [JsonProperty("tilewidth")]
        public int TileWidth { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("version")]
        public double Version { get; set; }
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }
        [JsonProperty("layers", Required = Required.Always)]
        public IReadOnlyList<Layer> Layers { get; set; }
        [JsonProperty("tilesets", Required = Required.Always)]
        public IReadOnlyList<TileSet> TileSets { get; set; }
    }

    public class Layer
    {
        [JsonProperty("data", Required = Required.Always)]
        public IReadOnlyList<int> Data { get; set; }
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("opacity")]
        public double Opacity { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }
        [JsonProperty("visible")]
        public bool Visible { get; set; }
    }

    public class TileSet
    {
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("firstgid")]
        public int FirstGId { get; set; }
    }
}
