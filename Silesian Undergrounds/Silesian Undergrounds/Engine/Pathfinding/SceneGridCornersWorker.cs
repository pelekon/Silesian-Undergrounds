using System.Collections.Generic;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Pathfinding
{
    public class SceneGridCornersWorker
    {
        public SceneGridDimension dim { get; private set; }

        public SceneGridCornersWorker()
        {
            dim = new SceneGridDimension();
        }

        public void DoInitialWork(List<Ground> grounds, List<Tile> tiles)
        {
            foreach (var node in grounds)
                CheckPosition(node);

            foreach (var node in tiles)
                CheckPosition(node);
        }

        public void CheckCornersWithRandRoomData(List<List<GameObject>> randomRoomsGameobjects)
        {
            foreach (var room in randomRoomsGameobjects)
            {
                foreach (var node in room)
                    CheckPosition(node);
            }
        }

        private void CheckPosition(GameObject go)
        {
            int x = (int)go.position.X / ResolutionMgr.TileSize;
            int y = (int)go.position.Y / ResolutionMgr.TileSize;

            if (x > dim.maxX)
                dim.maxX = x;

            if (y > dim.maxY)
                dim.maxY = y;
        }
    }
}
