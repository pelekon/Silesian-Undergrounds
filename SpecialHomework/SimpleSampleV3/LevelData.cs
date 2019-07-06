using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace SimpleSampleV3
{
    public class LevelData
    {
        [XmlElement("Player", Type = typeof(Player)) ]
        [XmlElement("Enemy", Type = typeof(Enemy)) ]
        public List<GameObject> gameObjects { get; set; }
        public List<Wall> walls { get; set; }
        public List<Decoration> decorations { get; set; }
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }



    }
}
