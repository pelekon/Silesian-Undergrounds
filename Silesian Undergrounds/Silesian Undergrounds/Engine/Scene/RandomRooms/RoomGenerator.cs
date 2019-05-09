using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene.RandomRooms
{
    public class RoomGenerator
    {
        private List<GameObject> result;
        public bool isJobDone;

        // Constant helpers
        private static readonly int minRoomWidht = 6;
        private static readonly int maxRoomWidht = 10;
        private static readonly int minRoomHeight = 6;
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

            Dictionary<int, List<Point>> groups = GroupBlocks(positions);
            #if DEBUG
            DebugPrintOfGroups(groups, "Before validation");
            #endif
            ValidateGroups(groups);
            #if DEBUG
            DebugPrintOfGroups(groups, "After validation");
            #endif
            BuildRoomsFromGroups(groups);

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

        // Function to split points between groups, group define area which can be used to
        // generate room, layer can have multiple areas at different sides of map
        private Dictionary<int, List<Point>> GroupBlocks(List<Point> positions)
        {
            Dictionary<int, List<Point>> groups = new Dictionary<int, List<Point>>();
            int group = 0;
            Dictionary<Point, int> pointsToGroupsHelper = new Dictionary<Point, int>();

            // look for close points, if u find point which already belong to group,
            // then merge its group if its size is less or equal to current point
            foreach (var point in positions)
            {
                if (pointsToGroupsHelper.Count == 0)
                {
                    pointsToGroupsHelper.Add(point, group);
                    var temp = new List<Point>();
                    temp.Add(point);
                    groups.Add(group, temp);
                    ++group;
                }

                for (int i = 0; i < positions.Count; ++i)
                {
                    // do not compare same tile with itself
                    if (point == positions[i])
                        continue;

                    Point tileToCheck = positions[i];

                    int absX = Math.Abs(tileToCheck.X - point.X);
                    int absY = Math.Abs(tileToCheck.Y - point.Y);

                    // check if node is in direct contanct with current tile
                    if ((absX == 1 || absX == 0) && (absY == 1 || absY == 0))
                    {
                        // check groups of current tile and checked tile
                        int currentTileGroup = -1;
                        int checkingTileGroup = -1;

                        if (pointsToGroupsHelper.ContainsKey(point))
                            currentTileGroup = pointsToGroupsHelper[point];

                        if (pointsToGroupsHelper.ContainsKey(tileToCheck))
                            checkingTileGroup = pointsToGroupsHelper[tileToCheck];

                        // Skip grouping if tile has group and is in same group as tileToCheck
                        if (currentTileGroup == checkingTileGroup && currentTileGroup != -1)
                            continue;

                        // assign group to tiles
                        if (currentTileGroup >= 0)
                        {
                            if (checkingTileGroup >= 0)
                            {
                                if (groups[currentTileGroup].Count >= groups[checkingTileGroup].Count)
                                    SwapGroups(groups[checkingTileGroup], groups[currentTileGroup], pointsToGroupsHelper, currentTileGroup);
                                else
                                    SwapGroups(groups[currentTileGroup], groups[checkingTileGroup], pointsToGroupsHelper, checkingTileGroup);
                            }
                            else
                            {
                                pointsToGroupsHelper.Add(tileToCheck, currentTileGroup);
                                // current point has group assigned to group has to exist in dict
                                groups[currentTileGroup].Add(tileToCheck);
                            }
                        }
                        else
                        {
                            if (checkingTileGroup >= 0)
                            {
                                groups[checkingTileGroup].Add(point);
                                pointsToGroupsHelper.Add(point, checkingTileGroup);
                            }
                            else
                            {
                                if (!groups.ContainsKey(group))
                                    groups.Add(group, new List<Point>());

                                groups[group].Add(point);
                                groups[group].Add(tileToCheck);
                                pointsToGroupsHelper.Add(point, group);
                                pointsToGroupsHelper.Add(tileToCheck, group);
                                ++group;
                            }
                        }
                    }
                }
            }

            return groups;
        }

        // Helper function to swap points from one group to another
        private void SwapGroups(List<Point> source, List<Point> dest, Dictionary<Point, int> dict, int group)
        {
            foreach (var point in source)
            {
                dest.Add(point);
                dict[point] = group;
            }

            source.Clear();
        }

        // Remove groups with too low amount of tiles( less than minHeight * minWidth)
        private void ValidateGroups(Dictionary<int, List<Point>> groups)
        {
            List<int> groupsToDelete = new List<int>();

            foreach (var group in groups)
            {
                if (group.Value.Count < (minRoomWidht * minRoomHeight))
                    groupsToDelete.Add(group.Key);
            }

            foreach (var key in groupsToDelete)
                groups.Remove(key);
        }
        
        // Function to build room based on generated groups, generated objects are put
        // into "List<GameObject> result" object 
        private void BuildRoomsFromGroups(Dictionary<int, List<Point>> groups)
        {
            foreach(var group in groups)
            {
                if (group.Value.Count == 0)
                    continue;

                // build matrix to start process of building room from group
                RoomGroupMatrix roomMatrix = PrepareMatrixFromRoom(group.Value);
                BuildRoomFromMatrix(ref roomMatrix);
            }
        }

        private RoomGroupMatrix PrepareMatrixFromRoom(List<Point> points)
        {
            RoomGroupMatrix matrix = new RoomGroupMatrix();
            int minX = points[0].X;
            int minY = points[0].Y;
            int maxX = 0;
            int maxY = 0;

            // find min and max X and Y to get offset and size for matrix
            foreach(var point in points)
            {
                if (point.X < minX)
                    minX = point.X;

                if (point.Y < minY)
                    minY = point.Y;

                if (point.X > maxX)
                    maxX = point.X;

                if (point.Y > maxY)
                    maxY = point.Y;
            }

            // Calculate size of matrix and allocate memory for data
            int sizeX = (maxX - minX) + 1;
            int sizeY = (maxY - minY) + 1;
            matrix.data = new RoomTileType[sizeX][];
            for (int i = 0; i < sizeX; ++i)
                matrix.data[i] = new RoomTileType[sizeY];

            matrix.offset = new Point(minX, minY);

            return matrix;
        }

        private void BuildRoomFromMatrix(ref RoomGroupMatrix matrix)
        {
            // Temp test code, just generate one room for now
            int sizeX = matrix.data.Length;
            int sizeY = matrix.data[0].Length;

            for (int x = 0; x < sizeX; ++x)
            {
                matrix.data[x][0] = RoomTileType.ROOM_TILE_WALL_UP;
                matrix.data[x][sizeY - 1] = RoomTileType.ROOM_TILE_WALL_BOTTOM;
            }

            for(int y = 0; y < sizeY; ++y)
            {
                matrix.data[0][y] = RoomTileType.ROOM_TILE_WALL_LEFT;
                matrix.data[sizeX - 1][y] = RoomTileType.ROOM_TILE_WALL_RIGHT;
            }

            for(int x = 1; x < sizeX - 1; ++x)
            {
                for (int y = 1; y < sizeY - 1; ++y)
                    matrix.data[x][y] = RoomTileType.ROOM_TILE_GROUND;
            }
        }

        // Debug print which shows current elements for dictonary with groups
        private void DebugPrintOfGroups(Dictionary<int, List<Point>> groups, string text)
        {
            Console.WriteLine(text);
            foreach(var group in groups)
            {
                Console.WriteLine("");
                Console.WriteLine("Grupa: " + group.Key);

                foreach (var point in group.Value)
                    Console.Write("(" + point.X + ", " + point.Y + "), ");
            }

            Console.WriteLine("");
        }
    }
}
