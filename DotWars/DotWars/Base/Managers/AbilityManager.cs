#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class AbilityManager
    {
        #region Declarations

        private readonly Queue<Fireball> activeFireballs;
        private readonly List<LargeRock> activeLargeRocks;
        private readonly Queue<LightningTrail> activeLightning;
        private readonly Queue<WaterPool> activeWaterpools;
        private readonly int fireballCap;
        private readonly Stack<Fireball> inactiveFireballs;
        private readonly Stack<LargeRock> inactiveLargeRocks;
        private readonly Stack<LightningTrail> inactiveLightning;

        private readonly Stack<WaterPool> inactiveWaterpools;
        private readonly int largeRockCap;
        private readonly int lightningCap;
        private readonly List<LargeRock> removeTheseRocks;
        private readonly int waterpoolCap;

        private ManagerHelper managers;

        private readonly Dictionary<NPC.AffliationTypes, int> rocksDestroyedByCommanders;
        private int waterSpilledCounter;
        private int lightningTravelledCounter;
        private int dotsSetOnFireCounter;

        #endregion

        public AbilityManager(int lRC, int lC, int fC, int wC, ManagerHelper mH)
        {
            managers = mH;

            rocksDestroyedByCommanders = new Dictionary<NPC.AffliationTypes, int>();
            rocksDestroyedByCommanders.Add(NPC.AffliationTypes.red, 0);
            rocksDestroyedByCommanders.Add(NPC.AffliationTypes.blue, 0);
            rocksDestroyedByCommanders.Add(NPC.AffliationTypes.green, 0);
            rocksDestroyedByCommanders.Add(NPC.AffliationTypes.yellow, 0);

            waterSpilledCounter = 0;
            lightningTravelledCounter = 0;
            dotsSetOnFireCounter = 0;
            largeRockCap = lRC;
            inactiveLargeRocks = new Stack<LargeRock>(largeRockCap);
            activeLargeRocks = new List<LargeRock>(largeRockCap);
            removeTheseRocks = new List<LargeRock>(largeRockCap);

            for (int i = 0; i < largeRockCap; i++)
            {
                inactiveLargeRocks.Push(new LargeRock(managers));
            }

            lightningCap = lC;
            inactiveLightning = new Stack<LightningTrail>(lightningCap);
            activeLightning = new Queue<LightningTrail>(lightningCap);

            for (int i = 0; i < lightningCap; i++)
            {
                inactiveLightning.Push(new LightningTrail(managers));
            }

            fireballCap = fC;
            inactiveFireballs = new Stack<Fireball>(fireballCap);
            activeFireballs = new Queue<Fireball>(fireballCap);

            for (int i = 0; i < fireballCap; i++)
            {
                inactiveFireballs.Push(new Fireball(managers));
            }

            waterpoolCap = wC;
            inactiveWaterpools = new Stack<WaterPool>(waterpoolCap);
            activeWaterpools = new Queue<WaterPool>(waterpoolCap);

            for (int i = 0; i < waterpoolCap; i++)
            {
                inactiveWaterpools.Push(new WaterPool());
            }
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void LoadContent(ManagerHelper mH)
        {
            foreach (LightningTrail l in inactiveLightning)
            {
                l.LoadContent(mH.GetTextureManager());
            }

            foreach (Fireball f in inactiveFireballs)
            {
                f.LoadContent(mH.GetTextureManager());
            }

            foreach (WaterPool w in inactiveWaterpools)
            {
                w.LoadContent(mH.GetTextureManager());
            }
        }

        public void AddLargeRock(Vector2 p, NPC.AffliationTypes aT)
        {
            if (inactiveLargeRocks.Count > 0)
            {
                LargeRock temp = inactiveLargeRocks.Pop();
                temp.Set(p, aT, managers);
                activeLargeRocks.Add(temp);
            }
            else
            {
                LargeRock temp = activeLargeRocks[0];
                temp.Set(p, aT, managers);
                activeLargeRocks.RemoveAt(0);
                activeLargeRocks.Add(temp);
            }
        }

        public void AddLightning(Vector2 p, float r, NPC.AffliationTypes aT)
        {
            if (inactiveLightning.Count > 0)
            {
                LightningTrail temp = inactiveLightning.Pop();
                temp.Set(p, r, aT, managers);
                activeLightning.Enqueue(temp);
            }
        }

        public void AddFireball(Vector2 p, Vector2 direction, NPC.AffliationTypes aT)
        {
            if (inactiveFireballs.Count > 0)
            {
                Fireball temp = inactiveFireballs.Pop();
                temp.Set(p, direction, aT, managers);
                activeFireballs.Enqueue(temp);
            }
        }

        public void AddWaterpool(Vector2 p, NPC.AffliationTypes aT)
        {
            if (inactiveWaterpools.Count > 0)
            {
                WaterPool temp = inactiveWaterpools.Pop();
                temp.Set(p, aT, managers);
                activeWaterpools.Enqueue(temp);
            }
        }

        public void Update()
        {
            //Largerocks
            foreach (LargeRock r in activeLargeRocks)
            {
                r.Update(managers);

                if (r.GetHealth() <= 0)
                {
                    if (r.GetLastDamager() is Commander)
                        UpdateRocksDestroyedByCommanders(r.GetLastDamager().GetPersonalAffilation());
                    removeTheseRocks.Add(r);
                }
            }

            foreach (LargeRock e in removeTheseRocks)
            {
                int temp = activeLargeRocks.IndexOf(e);
                inactiveLargeRocks.Push(activeLargeRocks[temp]);
                activeLargeRocks.Remove(e);
            }

            removeTheseRocks.Clear();

            //Lightning
            int numDeletes = 0;
            foreach (LightningTrail l in activeLightning)
            {
                l.Update(managers);

                if (l.GetLifeTime() <= 0)
                {
                    lightningTravelledCounter++;
                    numDeletes++;
                }
            }

            for (; numDeletes > 0; numDeletes--)
            {
                inactiveLightning.Push(activeLightning.Dequeue());
            }

            //Fireballs
            numDeletes = 0;
            foreach (Fireball f in activeFireballs)
            {
                f.Update(managers);

                if (f.IsDone())
                {
                    numDeletes++;
                }
            }

            for (; numDeletes > 0; numDeletes--)
            {
                dotsSetOnFireCounter += activeFireballs.Peek().GetDotsSetOnFire().Count;
                activeFireballs.Peek().ResetFireStatus();
                inactiveFireballs.Push(activeFireballs.Dequeue());
            }

            //Waterpools
            numDeletes = 0;
            foreach (WaterPool w in activeWaterpools)
            {
                w.Update(managers);

                if (w.IsDone())
                {
                    waterSpilledCounter += managers.GetRandom().Next(4);
                    numDeletes++;
                }
            }

            for (; numDeletes > 0; numDeletes--)
            {
                inactiveWaterpools.Push(activeWaterpools.Dequeue());
            }
        }

        public void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (WaterPool w in activeWaterpools)
            {
                w.Draw(sB, d, managers);
            }
        }

        public void DrawTop(SpriteBatch sB, Vector2 d)
        {
            foreach (LargeRock r in activeLargeRocks)
            {
                r.DrawPulse(sB, d, managers);
                r.Draw(sB, d, managers);
            }

            foreach (LightningTrail l in activeLightning)
            {
                l.Draw(sB, d, managers);
            }

            foreach (Fireball f in activeFireballs)
            {
                f.Draw(sB, d, managers);
            }
        }

        public List<LargeRock> GetLargeRocks()
        {
            return activeLargeRocks;
        }

        public Queue<LightningTrail> GetLightning()
        {
            return activeLightning;
        }

        public bool HasReachedLargeRockCap()
        {
            return inactiveLargeRocks.Count == 0;
        }

        private void UpdateRocksDestroyedByCommanders(NPC.AffliationTypes a)
        {
            rocksDestroyedByCommanders[a]++;
        }

        public Dictionary<NPC.AffliationTypes, int> GetRocksDestroyedByCommanders()
        {
            return rocksDestroyedByCommanders;
        }

        public int GetWaterSpilledCounter()
        {
            return waterSpilledCounter;
        }

        public int GetLightningTravelledCounter()
        {
            return lightningTravelledCounter;
        }

        public int GetDotsSetOnFireCounter()
        {
            return dotsSetOnFireCounter;
        }
    }
}