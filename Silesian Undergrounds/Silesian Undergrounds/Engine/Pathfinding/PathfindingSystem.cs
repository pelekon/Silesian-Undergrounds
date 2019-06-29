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

            grid = new Grid(sizeX, sizeY);
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
                foreach (var tile in room)
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

            grid.BlockCell(new Position(posX, posY));
            grid.BlockCell(new Position(posX + 1, posY));
            grid.BlockCell(new Position(posX, posY + 1));
            grid.BlockCell(new Position(posX + 1, posY + 1));
        }

        // Calculates and returns path which will be used to move to destinated point
        public void GetPathWithCallback(Vector2 sourcePos, Vector2 destination, Action<List<Vector2>> callBack)
        {
            //DebugPrint();
            Position source = GetGridPosFromGlobalPos(sourcePos);
            Position dest = GetGridPosFromGlobalPos(destination);
            //DebugPrintPositions(sourcePos, destination, source, dest);
            //PrintDebugFragment(source, dest);
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

                    //TEST CODE
                    x += 5;
                    y += 5;

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
            posYF *= 2;

            int posX = Convert.ToInt32(posXF);
            int posY = Convert.ToInt32(posYF);

            return new Position(posX, posY);
        }
        
        private void DebugPrint()
        {
            for (int y = 0; y < grid.DimY; ++y)
            {
                for (int x = 0; x < grid.DimX; ++x)
                {
                    float display = 1;
                    if (float.IsInfinity(grid.GetCellCost(new Position(x, y))))
                        display = 0;

                    if (x + 1 == grid.DimX)
                        Console.WriteLine(display);
                    else
                    {
                        Console.Write(display);
                        Console.Write(" ");
                    }  
                }
            }
        }

        private void DebugPrintPositions(Vector2 s, Vector2 d, Position so, Position de)
        {
            Console.WriteLine("Source");
            Console.WriteLine("X: " + s.X + " Y: " + s.Y);
            Console.WriteLine("X: " + so.X + " Y: " + so.Y);
            Console.WriteLine("Destination");
            Console.WriteLine("X: " + d.X + " Y: " + d.Y);
            Console.WriteLine("X: " + de.X + " Y: " + de.Y);
        }

        private void PrintDebugFragment(Position source, Position dest)
        {
            int startX = source.X - 10;
            if (startX < 0)
                startX = 0;

            int startY = source.Y - 10;
            if (startY < 0)
                startY = 0;

            for (int x = startX; x <= startX + 20; ++x)
            {
                for (int y = startY; y <= startY + 20; ++y)
                {
                    int cost = 1;
                    if (float.IsInfinity(grid.GetCellCost(new Position(x, y))))
                        cost = 0;

                    Console.Write(cost + " ");
                }

                Console.WriteLine("");
            }
        }
    }
}
