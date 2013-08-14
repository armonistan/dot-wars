using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class ConquestBase : Environment
    {
        #region Declarations

        //variables for counting the amount of time needed
        //to conqueror a territory
        private readonly Dictionary<NPC.AffliationTypes, Double> conquestCounters;
        private readonly double conquestTime;

        public NPC.AffliationTypes affiliation;

        public List<SpawnPoint> spawns;

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
            List<NPC.AffliationTypes> currentContestors = Controllers(mH);
            //Remove old suitors
            for (int i = 0; i < conquestCounters.Count; i++)
            {
                if (!currentContestors.Contains(conquestCounters.Keys.ElementAt(i)))
                {
                    conquestCounters.Remove(conquestCounters.Keys.ElementAt(i));
                    i--;
                }
            }

            foreach (SpawnPoint sP in spawns)
                if (affiliation != NPC.AffliationTypes.grey)
                    sP.affilation = affiliation;
                else
                    sP.affilation = NPC.AffliationTypes.black;

            //Add new suitors
            foreach (NPC.AffliationTypes t in currentContestors)
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
                                    smokeAsset = "Effects/smoke_red";
                                    break;
                                case NPC.AffliationTypes.blue:
                                    frameIndex = 2;
                                    smokeAsset = "Effects/smoke_blue";
                                    break;
                                case NPC.AffliationTypes.green:
                                    frameIndex = 3;
                                    smokeAsset = "Effects/smoke_green";
                                    break;
                                case NPC.AffliationTypes.yellow:
                                    frameIndex = 4;
                                    smokeAsset = "Effects/smoke_yellow";
                                    break;
                            }

                            //Spawn Explosion
                            mH.GetParticleManager().AddExplosion(GetOriginPosition(), affiliation, 0);

                            for (int i = 0; i < NUM_SMOKES; i++)
                            {
                                mH.GetParticleManager().AddParticle(smokeAsset, GetOriginPosition(),
                                    PathHelper.Direction((float)(mH.GetRandom().NextDouble() * Math.PI * 2)) * 50, 1, 0.01f, 1, 0.05f);
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
                        NPC.AffliationTypes tempConquestor = conquestCounters.Keys.ElementAt(0);
                        Double tempTimer = conquestCounters.Values.ElementAt(0);
                        conquestCounters.Remove(tempConquestor);
                        conquestCounters.Add(tempConquestor, tempTimer + mH.GetGameTime().ElapsedGameTime.TotalSeconds);

                        if (mH.GetRandom().NextDouble() < (tempTimer/conquestTime*0.2f))
                        {
                            mH.GetParticleManager()
                              .AddFire(GetOriginPosition(),
                                       PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2)) * 50, 1, 0.01f,
                                       1, 0.1f);
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

        private List<NPC.AffliationTypes> Controllers(ManagerHelper mH)
        {
            var suitors = new List<NPC.AffliationTypes>();

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

            return suitors;
        }

        public void AddSpawnpoint(Vector2 v, ManagerHelper mH)
        {
            spawns.Add(new SpawnPoint(v, NPC.AffliationTypes.black, mH));
        }
    }
}