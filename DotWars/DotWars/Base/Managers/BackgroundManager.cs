using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class BackgroundManager
    {
        #region Declarations

        private readonly List<Sprite> foregrounds;
        private readonly List<Sprite> tiles;

        private ManagerHelper managers;

        #endregion

        public BackgroundManager()
        {
            tiles = new List<Sprite>();
            foregrounds = new List<Sprite>();
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void AddBackground(Sprite b)
        {
            tiles.Add(b);
        }

        public void AddForeground(Sprite f)
        {
            foregrounds.Add(f);
        }

        public void LoadContent(TextureManager tM)
        {
            foreach (Sprite b in tiles.Concat(foregrounds))
            {
                b.LoadContent(tM);
                //b.position = b.position + b.origin;
            }
        }

        public void DrawBackgrounds(SpriteBatch sB, Vector2 d)
        {
            foreach (Sprite b in tiles)
            {
                b.Draw(sB, d, managers);
            }
        }

        public void Drawforegrounds(SpriteBatch sB, Vector2 d)
        {
            foreach (Sprite f in foregrounds)
            {
                f.Draw(sB, d, managers);
            }
        }

        public void Update()
        {
            foreach (Sprite s in tiles)
                s.Update(managers);
        }
    }
}