using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class ProjectileManager
    {
        #region Declarations
        private readonly Queue<Projectile> activeProjectiles;
        private readonly Stack<Projectile> inactiveProjectiles;
        private readonly int projectileCap;

        private readonly Queue<Flare> activeFlares;
        private readonly Stack<Flare> inactiveFlares;
        private readonly int flareCap;

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

        public ProjectileManager(int pC, int tC, int fC)
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

            flareCap = fC;
            inactiveFlares = new Stack<Flare>(flareCap);
            activeFlares = new Queue<Flare>(flareCap);

            for (int i = 0; i < flareCap; i++)
            {
                inactiveFlares.Push(new Flare());
            }
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
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

        public void AddFlare(NPC n, Vector2 v)
        {
            if (inactiveFlares.Count > 0)
            {
                Flare temp = inactiveFlares.Pop();
                temp.Set(n, v, managers);
                activeFlares.Enqueue(temp);
            }
        }

        private void RemoveFlare()
        {
            inactiveFlares.Push(activeFlares.Dequeue());
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

            numDeletes = 0;
            foreach (Flare f in activeFlares)
            {
                f.Update(managers);

                if (f.GetExistenceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveFlare();
            }
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

            foreach (Flare f in activeFlares)
            {
                f.Draw(sB, d, managers);
            }
        }

        #region Sets and Gets

        public Queue<Projectile> GetProjectiles()
        {
            return activeProjectiles;
        }

        public Flare GetFlare(NPC.AffliationTypes aT)
        {
            foreach (Flare f in activeFlares)
            {
                if (f.GetAffiliation() == aT)
                {
                    return f;
                }
            }

            return null;
        }

        #endregion
    }
}