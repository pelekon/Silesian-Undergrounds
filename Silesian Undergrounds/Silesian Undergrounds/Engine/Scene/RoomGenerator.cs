using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class RoomGenerator
    {
        private List<GameObject> result;
        public bool isJobDone;

        // Constant helpers
        private static readonly int minRoomWidht = 5;
        private static readonly int maxRoomWidht = 10;
        private static readonly int minRoomHeight = 5;
        private static readonly int maxRoomHeight = 10;

        public RoomGenerator()
        {
            isJobDone = false;
            result = new List<GameObject>();
        }

        public void GenerateRooms(Texture2D[][] layer)
        {
            List<Point> positions = FilterTiles(layer);

            if (positions.Count < (minRoomWidht * minRoomHeight))
            {
                isJobDone = true;
                return;
            }



            isJobDone = true;
        }

        private List<Point> FilterTiles(Texture2D[][] layer)
        {
            List<Point> list = new List<Point>();

            for(int x = 0; x < layer.Length; ++x)
            {
                for(int y = 0; y < layer[x].Length; ++y)
                {
                    if (layer[x][y] != null)
                        list.Add(new Point(x, y));
                }
            }

            return list;
        }
    }
}
