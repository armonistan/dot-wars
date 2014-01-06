using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class EnvironmentManager
    {
        #region Declarations
        private readonly List<Environment> botObjects;
        private readonly List<Impassable> notPassables;
        private readonly List<Impathable> notPathables;
        private readonly List<Environment> staticBlockers;
        private readonly List<Environment> topObjects;
        private ManagerHelper managers;
        #endregion

        public EnvironmentManager()
        {
            topObjects = new List<Environment>();
            botObjects = new List<Environment>();
            staticBlockers = new List<Environment>();
            notPassables = new List<Impassable>();
            notPathables = new List<Impathable>();
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void AddTopObject(Environment e)
        {
            topObjects.Add(e);
            e.LoadContent(managers.GetTextureManager());
        }

        public void AddBotObject(Environment e)
        {
            botObjects.Add(e);
            e.LoadContent(managers.GetTextureManager());
        }

        public void AddStaticBlocker(Environment b)
        {
            staticBlockers.Add(b);
            b.LoadContent(managers.GetTextureManager());
        }

        public void AddImpassable(Impassable i)
        {
            notPassables.Add(i);
            i.LoadContent(managers.GetTextureManager());
        }

        public void AddImpathable(Impathable i)
        {
            notPathables.Add(i);
            i.LoadContent(managers.GetTextureManager());
        }

        public void RemoveTopObject(Environment e)
        {
            topObjects.Remove(e);
        }

        public void RemoveBotObject(Environment e)
        {
            botObjects.Remove(e);
        }

        public void RemoveStaticBlocker(Environment e)
        {
            staticBlockers.Remove(e);
        }

        public void RemoveImpassable(Impassable i)
        {
            notPassables.Remove(i);
        }

        public void RemoveImpathable(Impathable i)
        {
            notPathables.Remove(i);
        }

        public void Update()
        {
            //Goes through all the objects
            for (int i = 0; i < topObjects.Count; i++)
            {
                //Store the item in case it dies
                Environment e = topObjects[i];

                //Lets it update
                e.Update(managers);

                //If it no longer exists, re-sync the loop
                if (!topObjects.Contains(e))
                {
                    i--;
                }
            }

            //Goes through all the objects
            for (int i = 0; i < botObjects.Count; i++)
            {
                //Store the item in case it dies
                Environment e = botObjects[i];

                //Lets it update
                e.Update(managers);

                //If it no longer exists, re-sync the loop
                if (!botObjects.Contains(e))
                {
                    i--;
                }
            }

            for (int i = 0; i < staticBlockers.Count; i++)
            {
                //Store the item in case it dies
                Environment e = staticBlockers[i];

                //Lets it update
                e.Update(managers);

                //If it no longer exists, re-sync the loop
                if (!staticBlockers.Contains(e))
                {
                    i--;
                }
            }

            for (int i = 0; i < notPassables.Count; i++)
            {
                //Store the item in case it dies
                Impassable e = notPassables[i];

                //Lets it update
                e.Update(managers);

                //If it no longer exists, re-sync the loop
                if (!notPassables.Contains(e))
                {
                    i--;
                }
            }

            for (int i = 0; i < notPathables.Count; i++)
            {
                //Store the item in case it dies
                Impathable e = notPathables[i];

                //Lets it update
                e.Update(managers);

                //If it no longer exists, re-sync the loop
                if (!notPathables.Contains(e))
                {
                    i--;
                }
            }
        }

        public void DrawTop(SpriteBatch sB, Vector2 d)
        {
            foreach (Environment e in staticBlockers)
            {
                e.Draw(sB, d, managers);
            }

            foreach (Environment e in topObjects)
            {
                e.Draw(sB, d, managers);
            }
        }

        public void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (Environment a in botObjects)
            {
                a.Draw(sB, d, managers);
            }

            foreach (Impassable e in notPassables)
            {
                e.Draw(sB, d, managers);
            }

            foreach (Impathable e in notPathables)
            {
                e.Draw(sB, d, managers);
            }
        }

        #region Sets and Gets
        public List<Environment> GetStaticBlockers()
        {
            return staticBlockers;
        }

        public List<Impassable> GetImpassables()
        {
            return notPassables;
        }

        public List<Impathable> GetImpathables()
        {
            return notPathables;
        }
        #endregion
    }
}