#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class Conquest : Gametype
    {
        #region Declarations

        //Determines when to spawn
        private double counter;
        private readonly double spawnSecs;

        private double scoreTimer;
        private readonly double scoreTime;
        private const int SCORE_CHANGE = 1;

        //list of bases
        public List<ConquestBase> bases;

        #endregion

        public Conquest(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC, float sT)
            : base(tL, pL, 200, pC, sT)
        {
            spawnSecs = 4;
            counter = 0;
            scoreTimer = 0.0;
            scoreTime = 1.0;
            this.typeOfGame = GT.CONQUEST;
        }

        public void Initialize(ManagerHelper mH, List<ConquestBase> bL)
        {
            bases = bL;
            //winScore = bL.Count;

            foreach (ConquestBase b in bases)
            {
                b.LoadContent(mH.GetTextureManager());
            }

            base.Initialize(mH);
        }

        public override bool Update(ManagerHelper mH)
        {
            foreach (ConquestBase b in bases)
            {
                b.Update(mH);
            }

            //TODO: Trying this out.
            //Fucking stupid counter
            //int teamCounter = 0;
            //foreach (NPC.AffliationTypes team in teams)
            //{
            //    ChangeScoreAbsolute(teamCounter, GetNumAlliedBases(team));
            //    teamCounter++;
            //}

            //Spawn Section
            if (counter > spawnSecs)
            {
                counter = 0.0;

                foreach (ConquestBase b in bases)
                {
                    if (b.affiliation != NPC.AffliationTypes.grey)
                    {
                        if (mH.GetNPCManager().GetAllies(b.affiliation).Count < mH.GetGametype().GetPopCap())
                        {
                            for (int i = 0; i < 50 &&
                                            !Spawn(mH, b.affiliation,
                                                mH.GetSpawnHelper().Spawn(b.affiliation, (GetNumAlliedBases(b.affiliation) == 0)),
                                                mH.GetRandom().Next(3)); i++)
                            {
                                //Ain't this just a gem?
                            }
                        }
                    }
                }
            }
            else
            {
                counter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            if (scoreTimer > scoreTime)
            {
                scoreTimer = 0.0;

                foreach (var conquestBase in bases)
                {
                    if (conquestBase.affiliation != NPC.AffliationTypes.grey)
                    {
                        ChangeScore(conquestBase.affiliation, SCORE_CHANGE);
                    }
                }
            }
            else
            {
                scoreTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            #region base code slightly modified

            #region Spawning Commanders

            int commanderCounter = 0;
            foreach (var commander in commanders)
            {
                if (mH.GetNPCManager().GetCommander(commander.Key) == null)
                {
                    if (spawnsCounters[commanderCounter] > spawnTime/* &&
                        (!HasSomeoneWon() || GetWinner() == commander.Value)*/)
                    {
                        SpawnCommander(mH, commander.Key, commander.Value, mH.GetSpawnHelper().Spawn(commander.Value, GetNumAlliedBases(commander.Value) == 0));

                        spawnsCounters[commanderCounter] = 0;
                    }
                    else
                    {
                        spawnsCounters[commanderCounter] += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                    }
                }

                commanderCounter++;
            }

            #endregion

            if (gameEndTimer < 0.0)
            {
                return true;
            }

            var tryGetWinner = GetWinner();

            if (tryGetWinner != NPC.AffliationTypes.same)
            {
                /*foreach (KeyValuePair<Type, NPC.AffliationTypes> commander in commanders)
                {
                    if (commander.Value != tryGetWinner)
                    {
                        if (mH.GetNPCManager().GetCommander(commander.Key) != null)
                        {
                            return false;
                        }
                    }
                }*/

                return true;
            }

            gameEndTimer -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            return false;

            #endregion
        }

        private void ChangeScoreAbsolute(int index, int s)
        {
            scores[index] = s;
        }

        public override void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (ConquestBase b in bases)
            {
                b.Draw(sB, d, managers);
            }

            base.DrawBottom(sB, d);
        }

        public ConquestBase GetClosestInList(List<ConquestBase> bL, Vector2 p)
        {
            if (bL.Count > 0)
            {
                ConquestBase closest = null;
                float minDist = float.PositiveInfinity;

                foreach (ConquestBase b in bL)
                {
                    float aDist = PathHelper.DistanceSquared(p, b.GetOriginPosition());

                    if (aDist < minDist)
                    {
                        closest = b;
                        minDist = aDist;
                    }
                }

                return closest;
            }

            return null;
        }

        private int GetNumAlliedBases(NPC.AffliationTypes team)
        {
            int numBases = 0;

            foreach (ConquestBase conquestBase in bases)
            {
                if (conquestBase.affiliation == team)
                {
                    numBases++;
                }
            }

            return numBases;
        }

        public List<ConquestBase> GetBases()
        {
            return bases;
        }

        public ConquestBase GetForwardBase(NPC.AffliationTypes a, ManagerHelper mH)
        {
            ConquestBase fb = null;

            if (GetNumAlliedBases(a) != 0 && GetNumAlliedBases(a) != winScore)
            {
                ConquestBase closestToFB = null;
                float distanceToClosestToFB = float.PositiveInfinity;

                foreach (ConquestBase b in GetBases())
                {
                    if (b.affiliation == a)
                    {
                        ConquestBase closestToB = null;
                        float distanceToClosestToB = float.PositiveInfinity;

                        foreach (ConquestBase conquestBase in GetBases())
                        {
                            if (conquestBase.affiliation != b.affiliation)
                            {
                                float distanceToNewEnemy = PathHelper.DistanceSquared(conquestBase.GetOriginPosition(),
                                                                                      b.GetOriginPosition());

                                if (distanceToNewEnemy < distanceToClosestToB)
                                {
                                    distanceToClosestToB = distanceToNewEnemy;
                                    closestToB = conquestBase;
                                }
                            }
                        }

                        if (distanceToClosestToB < distanceToClosestToFB)
                        {
                            distanceToClosestToFB = distanceToClosestToB;
                            closestToFB = closestToB;
                            fb = b;
                        }
                    }
                }
            }

            return fb;
        }

        private bool HasSomeoneWon()
        {
            foreach (int score in scores)
            {
                if (score == winScore)
                {
                    return true;
                }
            }

            return false;
        }

        public override string GetGametypeStatistics()
        {
            return "Daniel is Silly XD";
        }

        public override string GetName()
        {
            return "Conquest";
        }

        public override string GetSummary()
        {
            return "Capture all bases and\nkill the enemy commanders!";
        }
    }
}