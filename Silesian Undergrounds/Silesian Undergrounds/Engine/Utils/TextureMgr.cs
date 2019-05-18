using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class TextureMgr
    {
        private static TextureMgr instance = null;

        private TextureMgr()
        {
            textures = new Dictionary<string, Texture2D>();
            animations = new Dictionary<string, List<Texture2D>>();
        }

        public static TextureMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextureMgr();

                return instance;
            }
        }

        private ContentManager contentMgr;
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, List<Texture2D>> animations;

        public void SetCurrentContentMgr(ContentManager mgr) { contentMgr = mgr; }

        public Texture2D GetTexture(string name)
        {
            if (!textures.ContainsKey(name))
            {
                LoadIfNeeded(name);
                if (!textures.ContainsKey(name))
                    return null;
            }
            
            return textures[name];
        }

        public List<Texture2D> GetAnimation(string name)
        {
            if (!animations.ContainsKey(name))
                return null;

            return animations[name];
        }

        public void LoadIfNeeded(string name)
        {
            if (textures.ContainsKey(name))
                return;

            Texture2D texture = contentMgr.Load<Texture2D>(name);
            textures.Add(name, texture);
        }

        public void LoadIfNeeded(Dictionary<int, string>.ValueCollection list)
        {
            foreach (var txt in list)
                LoadIfNeeded(txt);
        }
        //
        // Summary:
        //     Function to create animation's texture array
        //     It loads whole spritesheet and creates Texture2D objects with frames
        //
        // Parameters:
        //   index:
        //     index of row/column, starts from 0
        public bool LoadAnimationFromSpritesheet(string fileName, string animName, int spritesheetRows, int spritesheetColumns, int index, int amount)
        {
            return LoadAnimationFromSpritesheet(fileName, animName, spritesheetRows, spritesheetColumns, index, amount, 0, 0, false);
        }
        //
        // Summary:
        //     Function to create animation's texture array
        //     It loads whole spritesheet and creates Texture2D objects with frames
        //
        // Parameters:
        //   index:
        //     index of row/column, starts from 0
        //   spacingX:
        //     define amount of spacing pixels between two separate textures in row
        //   spacingY:
        //     define amount of spacing pixels between two separate textures in column
        //   loadByColumn:
        //     if true function will load textures present in column instead of row
        public bool LoadAnimationFromSpritesheet(string fileName, string animName, int spritesheetRows, int spritesheetColumns, int index, int amount,
            int spacingX, int spacingY, bool canAddToExisting = false, bool loadByColumn = false)
        {
            bool contain = false;
            if (animations.ContainsKey(animName))
            {
                if (!canAddToExisting)
                    return false;

                contain = true;
            }

            Texture2D spritesheet = contentMgr.Load<Texture2D>(fileName);
            List<Texture2D> list = contain ? animations[animName] : new List<Texture2D>(amount);
            if (!contain)
                animations.Add(animName, list);

            int pixelsPerRow = (spritesheet.Height - ((spacingX * 2) * spritesheetRows)) / spritesheetRows;
            int pixelsPerColumn = (spritesheet.Width - ((spacingY * 2) * spritesheetColumns)) / spritesheetColumns;

            // copy information about pixels from texture containing our spriteshieet
            Color[] colorSourceOrg = new Color[spritesheet.Height * spritesheet.Width];
            spritesheet.GetData(colorSourceOrg);
            Color[][] colorSource = MakeTwoDimColorArray(colorSourceOrg, spritesheet.Width, spritesheet.Height);

            for (int i = 0; i < amount; ++i)
            {
                // Create color data array which will let frame get information about its pixel
                Color[] frameData = CreateFrameColorArray(pixelsPerColumn, pixelsPerRow, colorSource, index, i, spacingX, spacingY, loadByColumn);

                Texture2D frame = new Texture2D(spritesheet.GraphicsDevice, pixelsPerColumn, pixelsPerRow);
                frame.SetData(0, new Rectangle(0, 0, pixelsPerColumn, pixelsPerRow), frameData, 0, pixelsPerColumn * pixelsPerRow);
                frame.Name = animName + "_" + (i + 1);
                list.Add(frame);
            }

            return true;
        }

        public void LoadSingleAnimFrame(string fileName, string animName)
        {
            if (!animations.ContainsKey(animName))
                animations.Add(animName, new List<Texture2D>());

            Texture2D frame = contentMgr.Load<Texture2D>(fileName);
            animations[animName].Add(frame);
        }
        //
        // Summary:
        //     Function to create animation's texture array
        //     It loads whole spritesheet and creates Texture2D objects with frames
        //
        // Parameters:
        //   row:
        //     index of row, starts from 0
        //   column:
        //     index of column, starts from 0
        //   spacingX:
        //     define amount of spacing pixels between two separate textures in row
        //   spacingY:
        //     define amount of spacing pixels between two separate textures in column
        public void LoadSingleTextureFromSpritescheet(string fileName, string name, int spritesheetRows, int spritesheetColumns, int row, int column, int spacingX = 0, int spacingY = 0)
        {
            if (textures.ContainsKey(name))
                return;

            Texture2D spritesheet = contentMgr.Load<Texture2D>(fileName);

            int pixelsPerRow = (spritesheet.Height - ((spacingX * 2) * spritesheetRows)) / spritesheetRows;
            int pixelsPerColumn = (spritesheet.Width - ((spacingY * 2) * spritesheetColumns)) / spritesheetColumns;

            Color[] colorSourceOrg = new Color[spritesheet.Height * spritesheet.Width];
            spritesheet.GetData(colorSourceOrg);
            Color[][] colorSource = MakeTwoDimColorArray(colorSourceOrg, spritesheet.Width, spritesheet.Height);

            // Create color data array which will let frame get information about its pixel
            Color[] frameData = CreateFrameColorArray(pixelsPerColumn, pixelsPerRow, colorSource, row, column, spacingX, spacingY, false);

            Texture2D frame = new Texture2D(spritesheet.GraphicsDevice, pixelsPerColumn, pixelsPerRow);
            frame.SetData(0, new Rectangle(0, 0, pixelsPerColumn, pixelsPerRow), frameData, 0, pixelsPerColumn * pixelsPerRow);
            frame.Name = name;
            textures.Add(name, frame);
        }

        private Color[] CreateFrameColorArray(int w, int h, Color[][] source, int pivot, int current, int spacingX, int spacingY, bool loadByColumn)
        {
            Color[] frameData = new Color[w * h];
            // calculate space of previous elements, spacing from both sides of texture + texture size on proper axis
            int heightWithSpacing = h + spacingX + spacingX;
            int widthWithSpacing = w + spacingY + spacingY;

            // select proper pixels from source array and put them to created data
            int startX = pivot * heightWithSpacing + spacingX;
            int endX = startX + h;
            int startY = current * widthWithSpacing + spacingY;
            int endY = startY + w;
            int index = 0;

            if (loadByColumn)
            {
                startX = current * heightWithSpacing + spacingX;
                endX = startX + h;
                startY = pivot * widthWithSpacing + spacingY;
                endY = startY + w;
            }

            for (int x = startX; x < endX; ++x)
            {
                for (int y = startY; y < endY; ++y)
                    frameData[index++] = source[x][y];
            }

            return frameData;
        }

        private Color[][] MakeTwoDimColorArray(Color[] source, int w, int h)
        {
            Color[][] colors = new Color[h][];
            for (int i = 0; i < h; ++i)
                colors[i] = new Color[w];

            int index = 0;

            for (int x = 0; x < h; ++x)
            {
                for (int y = 0; y < w; ++y)
                    colors[x][y] = source[index++];
            }

            return colors;
        }
    }
}
