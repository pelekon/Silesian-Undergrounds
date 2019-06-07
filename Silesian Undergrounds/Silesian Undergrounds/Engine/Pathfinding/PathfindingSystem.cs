using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RoyT.AStar;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Pathfinding
{
    // source of algorithm: http://community.monogame.net/t/a-pathfinding-examples/8220
    public sealed class PathfindingSystem
    {
        // singleton definitions
        private static PathfindingSystem instance;

        private PathfindingSystem() { }

        public static PathfindingSystem GetInstance()
        {
            if (instance == null)
                instance = new PathfindingSystem();

            return instance;
        }

        private Grid grid;

        public void SetupSystemData(SceneGridDimension dimension, List<Tile> tiles, List<List<GameObject>> randomRoomsGameobjects)
        {
            GenerateGrid(dimension);
            PopulateGridWithObstacles(tiles, randomRoomsGameobjects);
        }

        private void GenerateGrid(SceneGridDimension dimension)
        {
            int sizeX = dimension.maxX + 1;
            int sizeY = dimension.maxY + 1;

            // multiply with 2 as we will split each tile into 4 movement nodes
            sizeX *= 2;
            sizeY *= 2;

            grid = new Grid(sizeY, sizeX);
        }

        private void PopulateGridWithObstacles(List<Tile> tiles, List<List<GameObject>> randomRoomsGameobjects)
        {
            int wallLayer = (int)LayerEnum.Walls;

            foreach (var tile in tiles)
            {
                if (tile.layer == wallLayer)
                    MarkObstacle(tile);
            }

            foreach (var room in randomRoomsGameobjects)
            {
                foreach (var tile in tiles)
                {
                    if (tile.layer == wallLayer)
                        MarkObstacle(tile);
                }
            }
        }

        private void MarkObstacle(GameObject go)
        {
            int posX = (int)go.position.X / ResolutionMgr.TileSize;
            int posY = (int)go.position.Y / ResolutionMgr.TileSize;

            posX *= 2;
            posY *= 2;

            grid.BlockCell(new Position(posY, posX));
            grid.BlockCell(new Position(posY + 1, posX));
            grid.BlockCell(new Position(posY, posX + 1));
            grid.BlockCell(new Position(posY + 1, posX + 1));
        }

        // Calculates and returns path which will be used to move to destinated point
        public void GetPathWithCallback(Vector2 sourcePos, Vector2 destination, Action<List<Vector2>> callBack)
        {
            //DebugPrint();
            Position source = GetGridPosFromGlobalPos(sourcePos);
            Position dest = GetGridPosFromGlobalPos(destination);

            GetPath(source, dest, callBack);
        }

        private void GetPath(Position source, Position dest, Action<List<Vector2>> callBack)
        {
            Task task = new Task(() =>
            {
                Position[] path = grid.GetPath(source, dest);
                List<Vector2> list = new List<Vector2>(path.Length);
                
                foreach (var node in path)
                {
                    float x = ((float)node.X) / 2;
                    float y = ((float)node.Y) / 2;

                    x *= ResolutionMgr.TileSize;
                    y *= ResolutionMgr.TileSize;

                    list.Add(new Vector2(x, y));
                }

                callBack(list);
            });

            task.Start();
        }

        private Position GetGridPosFromGlobalPos(Vector2 pos)
        {
            float posXF = pos.X / ResolutionMgr.TileSize;
            float posYF = pos.Y / ResolutionMgr.TileSize;
            posXF *= 2;
            posXF *= 2;

            int posX = Convert.ToInt32(posXF);
            int posY = Convert.ToInt32(posYF);

            return new Position(posX, posY);
        }
        
        private void DebugPrint()
        {
            for (int x = 0; x < grid.DimX; ++x)
            {
                for (int y = 0; y < grid.DimY; ++y)
                {
                    float display = 1;
                    if (float.IsInfinity(grid.GetCellCost(new Position(x, y))))
                        display = 0;

                    if (y + 1 == grid.DimY)
                        Console.WriteLine(display);
                    else
                    {
                        Console.Write(display);
                        Console.Write(" ");
                    }  
                }
            }
        }
    }
}
