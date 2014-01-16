#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    /*PURPOSE: This public class is the Texture Manager. Because DW has so many textures, sprites, images, ect. it becomes very difficult
               to keep up with all this data. Thus we create a "box" to hold of this stuff which is this public class. This box will make
               it so that we may easily access whatever visual data we need.
    */

    public class TextureManager
    {
        #region Declarations

        //Stores the Textures
        private readonly List<Texture2D> textures;

        //The assets
        private readonly List<String> assets;
        private readonly List<List<Vector2>[,]> blockers;

        //The Data
        private readonly List<Color[,]> colors;

        //The Frames
        private readonly List<Rectangle> frames;

        //Prevents stupid
        private int assetNumber;
        private bool loadCalled;

        private SpriteFont smallFont;
        private SpriteFont bigFont;
        private SpriteFont tinyFont;

        #endregion

        #region Location Constants

        public const string TEXTURES = "Textures/";

        #endregion

        public enum FontSizes
        {
            tiny,
            small,
            big
        }

        private TextureManager(List<String> aL)
        {
            //Sets up textures
            textures = new List<Texture2D>();

            //Sets up assets
            assets = aL;

            //Sets up colors
            colors = new List<Color[,]>();

            //Sets up frames
            frames = new List<Rectangle>();

            blockers = new List<List<Vector2>[,]>();

            //Sets loadcalled
            assetNumber = 0;
            loadCalled = false;
        }

        public TextureManager()
            : this(new List<string>())
        {
            //Nothing else
        }

        public void Add(String a, Rectangle f)
        {
            if (loadCalled)
            {
                throw new Exception("LoadContent has been called.");
            }
            else
            {
                //Add it
                assets.Add(a);
                frames.Add(f);
            }
        }

        public void GetData(String a, out Texture2D t, out Rectangle f, out Color[,] cD, out List<Vector2>[,] b)
        {
            //Prevents premature call
            if (loadCalled)
            {
                //Stores the index of the asset
                int index = assets.IndexOf(a);

                //Here we test to see if the texture exists in the manager, if not...
                if (index != -1)
                {
                    t = textures.ElementAt(index);
                    f = frames.ElementAt(index);
                    cD = colors.ElementAt(index);
                    b = blockers.ElementAt(index);
                }
                    //...then we have a problem. The texture manager does not have the texture for some reason
                else
                {
                    throw new Exception("Texture not found... probably forgot to import.");
                }
            }
            else
            {
                throw new Exception("LoadContent not called.");
            }
        }

        public void StartLoad(ContentManager cM)
        {
            tinyFont = cM.Load<SpriteFont>("Fonts/tinytext");
            smallFont = cM.Load<SpriteFont>("Fonts/text");
            bigFont = cM.Load<SpriteFont>("Fonts/bigtext");
        }

        public void LoadContent(ContentManager cM)
        {
            int nodeSize = 32;

            textures.Add(cM.Load<Texture2D>("./Textures/" + assets.ElementAt(assetNumber)));
            colors.Add(new Color[textures[assetNumber].Width,textures[assetNumber].Height]);
            Color[] tempColors = new Color[textures[assetNumber].Width*textures[assetNumber].Height];
            textures[assetNumber].GetData(tempColors);

            for (int x = 0; x < textures[assetNumber].Width; x++)
            {
                for (int y = 0; y < textures[assetNumber].Height; y++)
                {
                    colors[assetNumber][x, y] = tempColors[y*textures[assetNumber].Width + x];
                }
            }

            int spriteCols = frames[assetNumber].Width/nodeSize,
                spriteRows = frames[assetNumber].Height/nodeSize;

            List<Vector2>[,] tempBlockers =
                new List<Vector2>[textures[assetNumber].Width/frames[assetNumber].Width,
                    textures[assetNumber].Height/frames[assetNumber].Height];

            for (int j = 0; j < tempBlockers.GetLength(0); j++)
            {
                for (int k = 0; k < tempBlockers.GetLength(1); k++)
                {
                    tempBlockers[j, k] = new List<Vector2>();

                    for (int spriteX = 0; spriteX < spriteCols; spriteX++)
                    {
                        for (int spriteY = 0; spriteY < spriteRows; spriteY++)
                        {
                            for (int blockX = 0; blockX < nodeSize; blockX++)
                            {
                                bool found = false;
                                for (int blockY = 0; blockY < nodeSize; blockY++)
                                {
                                    if (
                                        colors[assetNumber][
                                            spriteX*nodeSize + blockX + j*frames[assetNumber].Width,
                                            spriteY*nodeSize + blockY + k*frames[assetNumber].Height].A > 0)
                                    {
                                        tempBlockers[j, k].Add(new Vector2(spriteX, spriteY));
                                        found = true;
                                        break;
                                    }
                                }

                                if (found)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            blockers.Add(tempBlockers);

            GC.Collect();

            assetNumber++;
        }

        public void FinishLoad()
        {
            loadCalled = true;
        }

        public int GetLoadPercent()
        {
            return (int) ((float) assetNumber/assets.Count*100.0f);
        }

        public void DrawString(SpriteBatch sB, String message, Vector2 loc, Color color, FontSizes fS, bool centered)
        {
            SpriteFont temp;

            switch (fS)
            {
                case FontSizes.tiny:
                    temp = tinyFont;
                    break;
                case FontSizes.small:
                    temp = smallFont;
                    break;
                case FontSizes.big:
                    temp = bigFont;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fS");
            }

            if (centered)
            {
                float offsetX = temp.MeasureString(message).X/2;
                loc.X -= offsetX;

                float offsetY = temp.MeasureString(message).Y/2;
                loc.Y -= offsetY;
            }

            sB.DrawString(temp, message, loc, color);
        }
    }
}