#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

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

        private readonly List<Environment> blockersToRemove;
        private readonly List<Impathable> impathablesToRemove; 

        #endregion

        public EnvironmentManager()
        {
            topObjects = new List<Environment>();
            botObjects = new List<Environment>();
            staticBlockers = new List<Environment>();
            notPassables = new List<Impassable>();
            notPathables = new List<Impathable>();

            blockersToRemove = new List<Environment>();
            impathablesToRemove = new List<Impathable>();
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void AddTopObject(Environment e)
        {
            topObjects.Add(e);
            e.LoadContent(managers.GetTextureManager());
            e.SetShouldRemove(false);
        }

        public void AddBotObject(Environment e)
        {
            botObjects.Add(e);
            e.LoadContent(managers.GetTextureManager());
            e.SetShouldRemove(false);
        }

        public void AddStaticBlocker(Environment b)
        {
            staticBlockers.Add(b);
            b.LoadContent(managers.GetTextureManager());
            b.SetShouldRemove(false);
        }

        public void AddImpassable(Impassable i)
        {
            notPassables.Add(i);
            i.LoadContent(managers.GetTextureManager());
            i.SetShouldRemove(false);
        }

        public void AddImpathable(Impathable i)
        {
            notPathables.Add(i);
            i.LoadContent(managers.GetTextureManager());
            i.SetShouldRemove(false);
        }

        private void RemoveStaticBlocker(Environment e)
        {
            staticBlockers.Remove(e);
        }

        private void RemoveImpathable(Impathable i)
        {
            notPathables.Remove(i);
        }

        public void Update()
        {
            foreach (var environment in topObjects)
            {
                environment.Update(managers);
            }

            foreach (var environment in botObjects)
            {
                environment.Update(managers);
            }

            foreach (var blocker in staticBlockers)
            {
                blocker.Update(managers);

                if (blocker.IsShouldRemove())
                {
                    blockersToRemove.Add(blocker);
                }
            }

            foreach (var environment in blockersToRemove)
            {
                RemoveStaticBlocker(environment);
            }

            blockersToRemove.Clear();

            foreach (var notPassable in notPassables)
            {
                notPassable.Update(managers);
            }

            foreach (var notPathable in notPathables)
            {
                notPathable.Update(managers);

                if (notPathable.IsShouldRemove())
                {
                    impathablesToRemove.Add(notPathable);
                }
            }

            foreach (var impathable in impathablesToRemove)
            {
                RemoveImpathable(impathable);
            }

            impathablesToRemove.Clear();
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