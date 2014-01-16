#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class ProjectileManager
    {
        #region Declarations

        private readonly Queue<Projectile> activeProjectiles;
        private readonly Stack<Projectile> inactiveProjectiles;
        private readonly int projectileCap;

        private readonly List<Mine> activeMines;
        private readonly List<Mine> removeTheseMines;
        private readonly Stack<Mine> inactiveMines;
        private readonly int mineCap;

        private readonly Queue<Tossable> activeTossables;
        private readonly Stack<Tossable> inactiveTossables;
        private readonly int tossableCap;

        private ManagerHelper managers;

        public static string SHOTGUN = "Projectiles/bullet_shotgun";
        public static string STANDARD = "Projectiles/bullet_standard";
        public static string ROCKET = "Projectiles/bullet_rocket";
        public static string BOMB = "Projectiles/bullet_bombs";
        public static string GRENADE = "Projectiles/grenade";

        #endregion

        public ProjectileManager(int pC, int tC, int mC)
        {
            projectileCap = pC;
            inactiveProjectiles = new Stack<Projectile>(projectileCap);
            activeProjectiles = new Queue<Projectile>(projectileCap);

            for (int i = 0; i < projectileCap; i++)
            {
                inactiveProjectiles.Push(new Projectile(new Grunt("Dots/Red/grunt_red", Vector2.Zero)));
            }

            tossableCap = tC;
            inactiveTossables = new Stack<Tossable>(tossableCap);
            activeTossables = new Queue<Tossable>(tossableCap);

            for (int i = 0; i < tossableCap; i++)
            {
                inactiveTossables.Push(new Tossable());
            }

            mineCap = mC;
            inactiveMines = new Stack<Mine>(mineCap);
            activeMines = new List<Mine>(mineCap);
            removeTheseMines = new List<Mine>(mineCap);

            for (int i = 0; i < mineCap; i++)
            {
                inactiveMines.Push(new Mine());
            }
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void LoadContent()
        {
            foreach (Mine mine in inactiveMines)
            {
                mine.LoadContent(managers.GetTextureManager());
            }
        }

        public void AddProjectile(String a, Vector2 p, NPC n, Vector2 v, int d, bool iE, bool collide, float dT)
        {
            if (inactiveProjectiles.Count > 0)
            {
                Projectile temp = inactiveProjectiles.Pop();
                temp.Set(a, p, n, v, d, iE, collide, dT, managers);
                activeProjectiles.Enqueue(temp);
            }
            else
            {
                //throw new Exception("Ran out of bullets.");
            }
        }

        private void RemoveProjectile()
        {
            inactiveProjectiles.Push(activeProjectiles.Dequeue());
        }

        public void AddTossable(String a, Vector2 p, NPC n, Vector2 v, int d, bool iE, float dT)
        {
            if (inactiveTossables.Count > 0)
            {
                Tossable temp = inactiveTossables.Pop();
                temp.Set(a, p, n, v, d, iE, true, dT, managers);
                activeTossables.Enqueue(temp);
            }
        }

        private void RemoveTossable()
        {
            inactiveTossables.Push(activeTossables.Dequeue());
        }

        public void AddMine(NPC n)
        {
            if (inactiveMines.Count > 0)
            {
                Mine temp = inactiveMines.Pop();
                temp.Set(n, n.GetOriginPosition());
                activeMines.Add(temp);
            }
        }

        private void RemoveMine(Mine m)
        {
            int tempIndex = activeMines.IndexOf(m);
            inactiveMines.Push(activeMines[tempIndex]);
            activeMines.RemoveAt(tempIndex);
        }

        public void Update()
        {
            int numDeletes = 0;
            foreach (Projectile p in activeProjectiles)
            {
                p.Update(managers);

                if (p.GetExistenceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveProjectile();
            }

            numDeletes = 0;
            foreach (Tossable t in activeTossables)
            {
                t.Update(managers);

                if (t.GetExistenceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveTossable();
            }

            foreach (Mine mine in activeMines)
            {
                mine.Update(managers);

                if (!mine.IsDrawing())
                {
                    removeTheseMines.Add(mine);
                }
            }

            foreach (Mine mine in removeTheseMines)
            {
                RemoveMine(mine);
            }

            removeTheseMines.Clear();
        }

        public void Draw(SpriteBatch sB, Vector2 d)
        {
            foreach (Projectile p in activeProjectiles)
            {
                p.Draw(sB, d, managers);
            }

            foreach (Tossable t in activeTossables)
            {
                t.Draw(sB, d, managers);
            }

            foreach (Mine mine in activeMines)
            {
                mine.Draw(sB, d, managers);
            }
        }

        #region Sets and Gets

        public Queue<Projectile> GetProjectiles()
        {
            return activeProjectiles;
        }

        #endregion
    }
}