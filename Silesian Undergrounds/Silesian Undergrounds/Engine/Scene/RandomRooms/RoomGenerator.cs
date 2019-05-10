using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Scene.RandomRooms
{
    public class RoomGenerator
    {
        public List<GeneratedRoom> result { get; private set; }
        public bool isJobDone;

        // Constant helpers
        private static readonly int minRoomWidth = 6;
        private static readonly int maxRoomWidth = 10;
        private static readonly int minRoomHeight = 6;
        private static readonly int maxRoomHeight = 10;

        public RoomGenerator()
        {
            isJobDone = false;
            result = new List<GeneratedRoom>();
        }

        public void GenerateRooms(Texture2D[][] layer)
        {
            List<Point> positions = FilterTiles(layer);

            if (positions.Count < (minRoomWidth * minRoomHeight))
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
                    if (layer[y][x] != null)
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
                if (group.Value.Count < (minRoomWidth * minRoomHeight))
                    groupsToDelete.Add(group.Key);
            }

            foreach (var key in groupsToDelete)
                groups.Remove(key);
        }

        // Function to build room based on generated groups, generated objects are put
        // into List<GeneratedRoom> result" object 
        private void BuildRoomsFromGroups(Dictionary<int, List<Point>> groups)
        {
            foreach(var group in groups)
            {
                if (group.Value.Count == 0)
                    continue;

                // build matrix to start process of building room from group
                RoomGroupMatrix roomMatrix = PrepareMatrixFromRoom(group.Value);
                Random rng = new Random();
                List<RoomGroupMatrix> rooms = SplitMatrixForFewRooms(ref roomMatrix, group.Value, rng);
                foreach (var room in rooms)
                {
                    RoomGroupMatrix r = room;
                    BuildRoomFromMatrix(ref r);
                    result.Add(new GeneratedRoom(r));
                }
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

        private List<RoomGroupMatrix> SplitMatrixForFewRooms(ref RoomGroupMatrix matrix, List<Point> points, Random rng)
        {
            List<RoomGroupMatrix> list = new List<RoomGroupMatrix>();

            CheckMatrixForWrongCells(ref matrix, points);

            int sizeX = matrix.data.Length;
            int sizeY = matrix.data[0].Length;

            if (sizeX >= (minRoomWidth * 2 + 1) || sizeY >= (minRoomHeight * 2 + 1))
            {
                if (sizeY > sizeX)
                    SplitMatrixByAxisY(ref matrix, list, rng, sizeX, sizeY);
                else
                    SplitMatrixByAxisX(ref matrix, list, rng, sizeX, sizeY);
            }
            else
                list.Add(matrix);

            return list;
        }

        private void CheckMatrixForWrongCells(ref RoomGroupMatrix matrix, List<Point> points)
        {
            // Swap tile type to ROOM_TILE_NONE when tile is proper tile 
            foreach (var point in points)
                matrix.data[point.X - matrix.offset.X][point.Y - matrix.offset.Y] = RoomTileType.ROOM_TILE_NONE;

            int sizeX = matrix.data.Length;
            int sizeY = matrix.data[0].Length;

            int minX = -1;
            int maxX = -1;
            int minY = -1;
            int maxY = -1;

            // Fill coordinates for empty area
            for (int x = 0; x < sizeX; ++x)
            {
                for(int y = 0; y < sizeY; ++y)
                {
                    if (matrix.data[x][y] == RoomTileType.ROOM_TILE_EMPTY)
                    {
                        if (minX == -1)
                            minX = x;

                        if (maxX < x || maxX == -1)
                            maxX = x;

                        if (minY == -1)
                            minY = y;

                        if (maxY < y || maxY == -1)
                            maxY = y;
                    }
                }
            }

            // If there are coordinates for empty area then remove it from matrix
            if (minX > -1 && minY > -1)
                RemoveUnwantedEmptySpace(ref matrix, minX, maxX, minY, maxY, sizeX, sizeY);
        }

        private void RemoveUnwantedEmptySpace(ref RoomGroupMatrix matrix, int minX, int maxX, int minY, int maxY, int sizeX, int sizeY)
        {
            int startX = minX;
            int startY = minY;

            // test code
            startX = sizeX - 1;
            startY = sizeY - 1;
            // 

            if (maxX == sizeX - 1)
                startX = 0;

            if (maxY == sizeY - 1)
                startY = 0;

            if (startX == 0 || startY == 0)
            {
                int newSizeX = sizeX - (maxX - startX + 1);
                int newSizeY = sizeY - (maxY - startY + 1);


            }
        }

        private void SplitMatrixByAxisX(ref RoomGroupMatrix matrix, List<RoomGroupMatrix> list, Random rng, int sizeX, int sizeY)
        {
            int chance = rng.Next(100);
            // Check chance to build one bigg room instead of two sepparate
            if (chance < 45)
            {
                list.Add(matrix);
                return;
            }

            int leftRoomSpace = sizeX;
            int rightRoomSpace = sizeX;
            int splitMode = rng.Next(0, 2);
            switch (splitMode)
            {
                case 0: // split rooms evenly(50/50)
                    leftRoomSpace = sizeX / 2;
                    rightRoomSpace = leftRoomSpace;
                    break;
                case 1: // split rooms in way that left room is bigger
                    leftRoomSpace = sizeX - minRoomWidth;
                    rightRoomSpace = minRoomWidth;
                    break;
                case 2: // split rooms in way that right room is bigger
                    leftRoomSpace = minRoomWidth;
                    rightRoomSpace = sizeX - minRoomWidth;
                    break;
            }

            // Prepare matrix for left room
            RoomGroupMatrix leftRoom = new RoomGroupMatrix();
            leftRoom.offset = new Point(matrix.offset.X, matrix.offset.Y);
            leftRoom.splitSide = RoomSplitSide.SPLIT_SIDE_LEFT;
            leftRoom.data = new RoomTileType[leftRoomSpace][];
            for (int i = 0; i < leftRoomSpace; ++i)
                leftRoom.data[i] = new RoomTileType[sizeY];

            // copy left area from orginal matrix
            for (int x = 0; x < leftRoomSpace; ++x)
            {
                for (int y = 0; y < sizeY; ++y)
                    leftRoom.data[x][y] = matrix.data[x][y];
            }

            // Prepare matrix for right room
            RoomGroupMatrix rightRoom = new RoomGroupMatrix();
            rightRoom.offset = new Point(matrix.offset.X + leftRoomSpace, matrix.offset.Y);
            rightRoom.splitSide = RoomSplitSide.SPLIT_SIDE_RIGHT;
            rightRoom.data = new RoomTileType[rightRoomSpace][];
            for (int i = 0; i < rightRoomSpace; ++i)
                rightRoom.data[i] = new RoomTileType[sizeY];

            // copy right area from orginal matrix
            for (int x = 0; x < rightRoomSpace; ++x)
            {
                for (int y = 0; y < sizeY; ++y)
                    rightRoom.data[x][y] = matrix.data[x + leftRoomSpace][y];
            }

            list.Add(leftRoom);
            list.Add(rightRoom);
        }

        private void SplitMatrixByAxisY(ref RoomGroupMatrix matrix, List<RoomGroupMatrix> list, Random rng, int sizeX, int sizeY)
        {
            int chance = rng.Next(100);
            // Check chance to build one bigg room instead of two sepparate
            if (chance < 45)
            {
                list.Add(matrix);
                return;
            }

            int upperRoomSpace = sizeY;
            int bottomRoomSpace = sizeY;
            int splitMode = rng.Next(0, 2);
            switch (splitMode)
            {
                case 0: // split rooms evenly(50/50)
                    upperRoomSpace = sizeY / 2;
                    bottomRoomSpace = upperRoomSpace;
                    break;
                case 1: // split rooms in way that upper room is bigger
                    upperRoomSpace = sizeY - minRoomHeight;
                    bottomRoomSpace = minRoomHeight;
                    break;
                case 2: // split rooms in way that bottom room is bigger
                    upperRoomSpace = minRoomHeight;
                    bottomRoomSpace = sizeY - minRoomHeight;
                    break;
            }

            // Prepare matrix for upper room
            RoomGroupMatrix upperRoom = new RoomGroupMatrix();
            upperRoom.offset = new Point(matrix.offset.X, matrix.offset.Y);
            upperRoom.splitSide = RoomSplitSide.SPLIT_SIDE_UP;
            upperRoom.data = new RoomTileType[sizeX][];
            for (int i = 0; i < sizeX; ++i)
                upperRoom.data[i] = new RoomTileType[upperRoomSpace];

            // copy upper area from orginal matrix
            for (int x = 0; x < sizeX; ++x)
            {
                for (int y = 0; y < upperRoomSpace; ++y)
                    upperRoom.data[x][y] = matrix.data[x][y];
            }

            // Prepare matrix for bottom room
            RoomGroupMatrix bottomRoom = new RoomGroupMatrix();
            bottomRoom.offset = new Point(matrix.offset.X, matrix.offset.Y + upperRoomSpace);
            bottomRoom.splitSide = RoomSplitSide.SPLIT_SIDE_DOWN;
            bottomRoom.data = new RoomTileType[sizeX][];
            for (int i = 0; i < sizeX; ++i)
                bottomRoom.data[i] = new RoomTileType[bottomRoomSpace];

            // copy bottom area from orginal matrix
            for (int x = 0; x < sizeX; ++x)
            {
                for (int y = 0; y < bottomRoomSpace; ++y)
                    bottomRoom.data[x][y] = matrix.data[x][y + upperRoomSpace];
            }

            list.Add(upperRoom);
            list.Add(bottomRoom);
        }

        private void BuildRoomFromMatrix(ref RoomGroupMatrix matrix)
        {
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

            // Add corners
            matrix.data[0][0] = RoomTileType.ROOM_TILE_CORNER_UL;
            matrix.data[0][sizeY - 1] = RoomTileType.ROOM_TILE_CORNER_BL;
            matrix.data[sizeX - 1][0] = RoomTileType.ROOM_TILE_CORNER_UR;
            matrix.data[sizeX - 1][sizeY - 1] = RoomTileType.ROOM_TILE_CORNER_BR;

            // Create passage between rooms if there is need for that
            if (matrix.splitSide != RoomSplitSide.SPLIT_SIDE_NONE)
            {
                switch(matrix.splitSide)
                {
                    case RoomSplitSide.SPLIT_SIDE_LEFT:
                    {
                        int tileToSwap = sizeY / 2 - 1;
                        matrix.data[sizeX - 1][tileToSwap] = RoomTileType.ROOM_TILE_GROUND;
                        matrix.data[sizeX - 1][tileToSwap + 1] = RoomTileType.ROOM_TILE_GROUND;
                        break;
                    }
                        
                    case RoomSplitSide.SPLIT_SIDE_RIGHT:
                    {
                        int tileToSwap = sizeY / 2 - 1;
                        matrix.data[0][tileToSwap] = RoomTileType.ROOM_TILE_GROUND;
                        matrix.data[0][tileToSwap + 1] = RoomTileType.ROOM_TILE_GROUND;
                        break;
                    }
                    case RoomSplitSide.SPLIT_SIDE_UP:
                    {
                        int tileToSwap = sizeX / 2;
                        matrix.data[tileToSwap][sizeY - 1] = RoomTileType.ROOM_TILE_GROUND;
                        matrix.data[tileToSwap + 1][sizeY - 1] = RoomTileType.ROOM_TILE_GROUND;
                        break;
                    }
                    case RoomSplitSide.SPLIT_SIDE_DOWN:
                    {
                        int tileToSwap = sizeX / 2;
                        matrix.data[tileToSwap][0] = RoomTileType.ROOM_TILE_GROUND;
                        matrix.data[tileToSwap + 1][0] = RoomTileType.ROOM_TILE_GROUND;
                        break;
                    }
                }
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
