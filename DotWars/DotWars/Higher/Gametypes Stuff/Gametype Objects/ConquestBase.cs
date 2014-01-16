#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class ConquestBase : Environment
    {
        #region Declarations

        //variables for counting the amount of time needed
        //to conqueror a territory
        private readonly Dictionary<NPC.AffliationTypes, Double> conquestCounters;
        private readonly List<KeyValuePair<NPC.AffliationTypes, Double>> countersToRemove;
        private readonly double conquestTime;

        private readonly List<NPC.AffliationTypes> suitors;

        public NPC.AffliationTypes affiliation;

        public List<SpawnPoint> spawns;

        private string red = "Effects/smoke_red";
        private string blue = "Effects/smoke_blue";
        private string green = "Effects/smoke_green";
        private string yellow = "Effects/smoke_yellow";

        //Constants
        private const int NUM_SMOKES = 10;

        #endregion

        public ConquestBase(Vector2 p)
            : base("bases", p, Vector2.Zero)
        {
            affiliation = NPC.AffliationTypes.grey;
            conquestCounters = new Dictionary<NPC.AffliationTypes, Double>();
            spawns = new List<SpawnPoint>();
            conquestTime = 3; //three seconds

            suitors = new List<NPC.AffliationTypes>();
            countersToRemove = new List<KeyValuePair<NPC.AffliationTypes, double>>();
        }

        public ConquestBase(Vector2 p, NPC.AffliationTypes a)
            : this(p)
        {
            affiliation = a;
        }

        public override void LoadContent(TextureManager tM)
        {
            base.LoadContent(tM);

            //Set up image
            switch (affiliation)
            {
                case NPC.AffliationTypes.red:
                    frameIndex = 1;
                    break;
                case NPC.AffliationTypes.blue:
                    frameIndex = 2;
                    break;
                case NPC.AffliationTypes.green:
                    frameIndex = 3;
                    break;
                case NPC.AffliationTypes.yellow:
                    frameIndex = 4;
                    break;
            }
        }

        public override void Update(ManagerHelper mH)
        {
            UpdateControllers(mH);

            foreach (KeyValuePair<NPC.AffliationTypes, double> counter in conquestCounters)
            {
                if (!suitors.Contains(counter.Key))
                {
                    countersToRemove.Add(counter);
                }
            }

            foreach (KeyValuePair<NPC.AffliationTypes, double> counter in countersToRemove)
            {
                conquestCounters.Remove(counter.Key);
            }

            countersToRemove.Clear();

            foreach (SpawnPoint sP in spawns)
                if (affiliation != NPC.AffliationTypes.grey)
                    sP.affilation = affiliation;
                else
                    sP.affilation = NPC.AffliationTypes.black;

            //Add new suitors
            foreach (NPC.AffliationTypes t in suitors)
            {
                if (!conquestCounters.ContainsKey(t))
                {
                    conquestCounters.Add(t, 0);
                }
            }

            if (conquestCounters.Count == 1)
            {
                if (!conquestCounters.Keys.Contains(affiliation))
                {
                    if (conquestCounters.Values.ElementAt(0) > conquestTime)
                    {
                        if (affiliation == NPC.AffliationTypes.grey)
                        {
                            affiliation = conquestCounters.Keys.First();

                            string smokeAsset = "";

                            //Set up image
                            switch (affiliation)
                            {
                                case NPC.AffliationTypes.red:
                                    frameIndex = 1;
                                    smokeAsset = red;
                                    break;
                                case NPC.AffliationTypes.blue:
                                    frameIndex = 2;
                                    smokeAsset = blue;
                                    break;
                                case NPC.AffliationTypes.green:
                                    frameIndex = 3;
                                    smokeAsset = green;
                                    break;
                                case NPC.AffliationTypes.yellow:
                                    frameIndex = 4;
                                    smokeAsset = yellow;
                                    break;
                            }

                            //Spawn Explosion
                            mH.GetParticleManager().AddExplosion(GetOriginPosition(), affiliation, 0);

                            for (int i = 0; i < NUM_SMOKES; i++)
                            {
                                mH.GetParticleManager().AddParticle(smokeAsset, GetOriginPosition(),
                                                                    PathHelper.Direction(
                                                                        (float) (mH.GetRandom().NextDouble()*Math.PI*2))*
                                                                    50, 1, 0.01f, 1, 0.05f);
                            }
                        }
                        else
                        {
                            affiliation = NPC.AffliationTypes.grey;
                            frameIndex = 0;
                            conquestCounters.Clear();

                            //Spawn Explosion
                            mH.GetParticleManager().AddExplosion(GetOriginPosition(), affiliation, 0);
                        }
                    }
                    else
                    {
                        Double tempTimer = conquestCounters.First().Value;

                        conquestCounters[conquestCounters.First().Key] = tempTimer +
                                                                         mH.GetGameTime().ElapsedGameTime.TotalSeconds;

                        if (mH.GetRandom().NextDouble() < (tempTimer/conquestTime*0.2))
                        {
                            mH.GetParticleManager()
                              .AddFire(GetOriginPosition(),
                                       PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*50, 1f,
                                       0.01f,
                                       1f, 0.1f);
                        }
                    }
                }
            }

            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 d, ManagerHelper mH)
        {
            base.Draw(sB, d, mH);
        }

        private void UpdateControllers(ManagerHelper mH)
        {
            suitors.Clear();

            //Find suitors
            foreach (NPC a in mH.GetNPCManager().GetNPCs())
            {
                if ((a is Commander || a is OffensiveConquestGrunt) &&
                    CollisionHelper.IntersectPixelsRadius(a, this, 16, 32) != new Vector2(-1))
                {
                    if (!suitors.Contains(a.GetAffiliation()))
                    {
                        suitors.Add(a.GetAffiliation());
                    }
                }
            }
        }

        public void AddSpawnpoint(Vector2 v, ManagerHelper mH)
        {
            spawns.Add(new SpawnPoint(v, NPC.AffliationTypes.black, mH));
        }
    }
}