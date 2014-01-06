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
        private double counter, spawnSecs;
        public float suicideSpawnModifier;
        private int survivalPointModifier;
        private bool redCommanderHasSpawned;
        private bool blueCommanderHasSpawned;
        private bool greenCommanderHasSpawned;
        private bool yellowCommanderHasSpawned;

        private bool hasAppliedModifier;

        private const int NUM_SUICIDES = 15;
        #endregion

        public Survival(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC, float sT)
            : base(tL, pL, -1, pC, sT)
        {
            this.typeOfGame = GT.SURVIVAL;
            suicideSpawnModifier = 0;
            survivalPointModifier = -1;
            gameEndTimer = 120;
            counter = 0;
            spawnSecs = 2;
            redCommanderHasSpawned = false;
            blueCommanderHasSpawned = false;
            greenCommanderHasSpawned = false;
            yellowCommanderHasSpawned = false;

            hasAppliedModifier = false;
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
            if (GetIfAllPlayersAreDead(mH) && redCommanderHasSpawned)
                gameEndTimer = -1;

            if ((int)gameEndTimer % 30 == 0)
            {
                if (!hasAppliedModifier)
                {   
                    survivalPointModifier++;
                    suicideSpawnModifier++;
                    UpdateScoreSurvival(mH);
                        
                    hasAppliedModifier = true;
                }
            }
            else
            {
                hasAppliedModifier = false;
            }

            #region Spawns

            if (mH.GetNPCManager().GetAllies(NPC.AffliationTypes.black).Count <= NUM_SUICIDES && counter > spawnSecs)
            {
                mH.GetNPCManager().Add(new Suicide(mH.GetSpawnHelper().SpawnSucideDots(), mH));

                counter = 0;
                spawnSecs -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            //Update spent time
            counter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            #endregion

            foreach (Claimable c in claimables)
            {
                c.Update(mH);
            }

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
                scores[0]+=10 * survivalPointModifier;
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.blue) != null)
                scores[1] += 10 * survivalPointModifier;
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.green) != null)
                scores[2] += 10 * survivalPointModifier;
            if (mH.GetNPCManager().GetCommander(NPC.AffliationTypes.yellow) != null)
                scores[3] += 10 * survivalPointModifier;
        }

        public bool GetIfAllCommandersAreDead(ManagerHelper mH)
        {
            return (redCommanderHasSpawned && blueCommanderHasSpawned && greenCommanderHasSpawned && yellowCommanderHasSpawned && mH.GetNPCManager().GetCommanders().Count == 0);
        }

        public bool GetIfAllPlayersAreDead(ManagerHelper mH)
        {
            foreach (Commander commander in mH.GetNPCManager().GetCommanders())
            {
                if (commander is RedPlayerCommander)
                {
                    return false;
                }
                else if (commander is BluePlayerCommander)
                {
                    return false;
                }
                else if (commander is GreenPlayerCommander)
                {
                    return false;
                }
                else if (commander is YellowPlayerCommander)
                {
                    return false;
                }
            }

            return true;
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
                float minDist = float.PositiveInfinity;

                foreach (Claimable a in claimables)
                {
                    if (!a.taken)
                    {
                        float aDist = PathHelper.DistanceSquared(p, a.GetOriginPosition());

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

        public override void SpawnCommander(ManagerHelper mH, Type commanderType, NPC.AffliationTypes team, Vector2 point)
        {
            if (!redCommanderHasSpawned && commanderType == typeof(RedCommander))
            {
                mH.GetNPCManager().Add(new RedCommander(point, team));
                redCommanderHasSpawned = true;
            }
            else if (!blueCommanderHasSpawned && commanderType == typeof(BlueCommander))
            {
                mH.GetNPCManager().Add(new BlueCommander(point, team));
                blueCommanderHasSpawned = true;
            }
            else if (!greenCommanderHasSpawned && commanderType == typeof(GreenCommander))
            {
                mH.GetNPCManager().Add(new GreenCommander(point, team));
                greenCommanderHasSpawned = true;
            }
            else if (!yellowCommanderHasSpawned && commanderType == typeof(YellowCommander))
            {
                mH.GetNPCManager().Add(new YellowCommander(point, team));
                yellowCommanderHasSpawned = true;
            }
            else if (!redCommanderHasSpawned && commanderType == typeof(RedPlayerCommander))
            {
                mH.GetNPCManager().Add(new RedPlayerCommander(point, team, mH));
                redCommanderHasSpawned = true;
            }
            else if (!blueCommanderHasSpawned && commanderType == typeof(BluePlayerCommander))
            {
                mH.GetNPCManager().Add(new BluePlayerCommander(point, team, mH));
                blueCommanderHasSpawned = true;
            }
            else if (!greenCommanderHasSpawned && commanderType == typeof(GreenPlayerCommander))
            {
                mH.GetNPCManager().Add(new GreenPlayerCommander(point, team, mH));
                greenCommanderHasSpawned = true;
            }
            else if (!yellowCommanderHasSpawned && commanderType == typeof(YellowPlayerCommander))
            {
                mH.GetNPCManager().Add(new YellowPlayerCommander(point, team, mH));
                yellowCommanderHasSpawned = true;
            }
        }

        public override NPC.AffliationTypes GetWinner()
        {
            int maxScore = 0;
            int winningTeam = 0;

            //Go through each team and see who got highest score

            for (int i = 0; i < teams.Count; i++)
            {
                if (scores[i] >= maxScore)
                {
                    winningTeam = i;
                    maxScore = scores[i];
                }
            }

            if (maxScore > 0)
                return teams[winningTeam];

            return NPC.AffliationTypes.same;
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