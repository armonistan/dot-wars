using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Survival : Gametype
    {
        #region Declarations

        public List<Claimable> claimables;
        private float counter, spawnSecs;
        public float suicideSpawnModifier;
        private int survivalPointModifier;
        private bool redCommanderHasSpawned;
        private bool blueCommanderHasSpawned;
        private bool greenCommanderHasSpawned;
        private bool yellowCommanderHasSpawned;
        #endregion

        public Survival(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC, float sT)
            : base(tL, pL, 100, pC, sT)
        {
            this.typeOfGame = GT.SURVIVAL;
            suicideSpawnModifier = 1;
            survivalPointModifier = 1;
            gameEndTimer = 120;
            counter = 0;
            spawnSecs = 2;
            winScore = -1;
            redCommanderHasSpawned = false;
            blueCommanderHasSpawned = false;
            greenCommanderHasSpawned = false;
            yellowCommanderHasSpawned = false;
        }

        public void Initialize(ManagerHelper mH, List<Claimable> cL)
        {
            claimables = cL;

            foreach (Claimable c in claimables)
            {
                c.LoadContent(mH.GetTextureManager());
            }
        }

        public override bool Update(ManagerHelper mH)
        {
            if (GetIfAllCommandersAreDead(mH))
                gameEndTimer = -1;

            if (gameEndTimer % 30 == 0)
            {
                UpdateScoreSurvival(mH);
                survivalPointModifier++;
            }

            #region Spawns

            if (mH.GetNPCManager().GetAllies(NPC.AffliationTypes.black).Count <= 15 && counter > spawnSecs)
            {
                mH.GetNPCManager().Add(new Suicide(mH.GetSpawnHelper().SpawnSucideDots(), mH));
                //mH.GetNPCManager().Add(new Suicide(new Vector2(mH.GetRandom().Next(500), mH.GetRandom().Next(500))));
                //mH.GetNPCManager().Add(new Suicide(new Vector2(mH.GetRandom().Next(500), mH.GetRandom().Next(500))));
                //mH.GetNPCManager().Add(new Suicide(new Vector2(mH.GetRandom().Next(500), mH.GetRandom().Next(500))));
                //mH.GetNPCManager().Add(new Suicide(new Vector2(mH.GetRandom().Next(500), mH.GetRandom().Next(500))));
                //mH.GetNPCManager().Add(new Suicide(new Vector2(mH.GetRandom().Next(500), mH.GetRandom().Next(500))));

                counter = 0;
                spawnSecs -= (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            //Update spent time
            counter += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            #endregion

            foreach (Claimable c in claimables)
            {
                c.Update(mH);
            }

            if (gameEndTimer > 30)
                suicideSpawnModifier = (120/gameEndTimer);

            return base.Update(mH);
        }

        public override void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (Claimable c in claimables)
            {
                c.Draw(sB, d, managers);
            }

            base.DrawBottom(sB, d);
        }

        private void UpdateScoreSurvival(ManagerHelper mH)
        {
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.red) != null)
                scores[0]+=10;
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.blue) != null)
                scores[1] += 10;
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.green) != null)
                scores[2] += 10;
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.yellow) != null)
                scores[3] += 10;
        }

        public bool GetIfAllCommandersAreDead(ManagerHelper mH)
        {
            return (redCommanderHasSpawned && blueCommanderHasSpawned && greenCommanderHasSpawned && yellowCommanderHasSpawned && mH.GetNPCManager().GetCommanders().Count == 0);
        }

        public override void ChangeScore(NPC agent, int s)
        {
            base.ChangeScore(agent, s+survivalPointModifier);
        }

        public List<Claimable> GetClaimables()
        {
            return claimables;
        }

        public Claimable GetClosestClaimable(Vector2 p)
        {
            if (claimables.Count > 0)
            {
                Claimable closest = null;
                float minDist = 10000;

                foreach (Claimable a in claimables)
                {
                    if (!a.taken)
                    {
                        var aDist =
                            (float)
                            (Math.Sqrt(Math.Pow(p.X - a.GetOriginPosition().X, 2) +
                                       Math.Pow(p.Y - a.GetOriginPosition().Y, 2)));
                        if (aDist < minDist)
                        {
                            closest = a;
                            minDist = aDist;
                        }
                    }
                }

                return closest;
            }

            return null;
        }

        public virtual void SpawnCommander(ManagerHelper mH, Type commanderType, NPC.AffliationTypes team, Vector2 point)
        {
            if (redCommanderHasSpawned && commanderType == typeof(RedCommander))
            {
                mH.GetNPCManager().Add(new RedCommander(point, team));
                redCommanderHasSpawned = true;
            }
            else if (blueCommanderHasSpawned && commanderType == typeof(BlueCommander))
            {
                mH.GetNPCManager().Add(new BlueCommander(point, team));
                blueCommanderHasSpawned = true;
            }
            else if (greenCommanderHasSpawned && commanderType == typeof(GreenCommander))
            {
                mH.GetNPCManager().Add(new GreenCommander(point, team));
                greenCommanderHasSpawned = true;
            }
            else if (yellowCommanderHasSpawned && commanderType == typeof(YellowCommander))
            {
                mH.GetNPCManager().Add(new YellowCommander(point, team));
                yellowCommanderHasSpawned = true;
            }
            else if (redCommanderHasSpawned && commanderType == typeof(RedPlayerCommander))
            {
                mH.GetNPCManager().Add(new RedPlayerCommander(point, team, mH));
                redCommanderHasSpawned = true;
            }
            else if (blueCommanderHasSpawned && commanderType == typeof(BluePlayerCommander))
            {
                mH.GetNPCManager().Add(new BluePlayerCommander(point, team, mH));
                blueCommanderHasSpawned = true;
            }
            else if (greenCommanderHasSpawned && commanderType == typeof(GreenPlayerCommander))
            {
                mH.GetNPCManager().Add(new GreenPlayerCommander(point, team, mH));
                greenCommanderHasSpawned = true;
            }
            else if (yellowCommanderHasSpawned && commanderType == typeof(YellowPlayerCommander))
            {
                mH.GetNPCManager().Add(new YellowPlayerCommander(point, team, mH));
                yellowCommanderHasSpawned = true;
            }
        }


        public override string GetName()
        {
            return "Survival";
        }

        public override string GetSummary()
        {
            return "Survive.";
        }
    }
}