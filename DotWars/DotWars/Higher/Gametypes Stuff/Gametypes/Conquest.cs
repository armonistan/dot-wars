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
        private readonly double spawnSecs;

        //list of bases
        public List<ConquestBase> bases;
        private double counter;

        #endregion

        public Conquest(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int pC, float sT)
            : base(tL, pL, 0, pC, sT)
        {
            spawnSecs = 4;
            counter = 0;
            this.typeOfGame = GT.CONQUEST;
        }

        public void Initialize(ManagerHelper mH, List<ConquestBase> bL)
        {
            bases = bL;
            winScore = bL.Count;

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

            foreach (NPC.AffliationTypes team in teams)
            {
                ChangeScoreAbsolute(team, GetNumAlliedBases(team));
            }

            //Spawn Section
            if (counter > spawnSecs)
            {
                counter = 0.0;

                foreach (ConquestBase b in bases)
                {
                    if (b.affiliation != NPC.AffliationTypes.grey)
                    {
                        Spawn(mH, b.affiliation,
                              mH.GetSpawnHelper().Spawn(b.affiliation, (GetNumAlliedBases(b.affiliation) == 0)),
                              mH.GetRandom().Next(3));
                    }
                }
            }
            else
            {
                counter += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            #region base code slightly modified

            #region Spawning Commanders

            for (int i = 0; i < commanders.Count; i++)
            {
                if (mH.GetNPCManager().GetCommander(commanders.Keys.ElementAt(i)) == null)
                {
                    if (spawnsCounters[i] > spawnTime &&
                        (!HasSomeoneWon() || GetWinner() == commanders.Values.ElementAt(i)))
                    {
                        if (commanders.Keys.ElementAt(i) == typeof (RedCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new RedCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i)));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (BlueCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new BlueCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i)));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (GreenCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new GreenCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i)));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (YellowCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new YellowCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i)));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (RedPlayerCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new RedPlayerCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i), mH));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (BluePlayerCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new BluePlayerCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i), mH));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (GreenPlayerCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new GreenPlayerCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i), mH));
                            spawnsCounters[i] = 0;
                        }
                        else if (commanders.Keys.ElementAt(i) == typeof (YellowPlayerCommander))
                        {
                            mH.GetNPCManager()
                              .Add(
                                  new YellowPlayerCommander(
                                      mH.GetSpawnHelper()
                                        .Spawn(commanders.Values.ElementAt(i),
                                               (GetNumAlliedBases(commanders.Values.ElementAt(i)) == 0)),
                                      commanders.Values.ElementAt(i), mH));
                            spawnsCounters[i] = 0;
                        }
                    }
                    else
                    {
                        spawnsCounters[i] += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                    }
                }
            }

            #endregion

            if (gameEndTimer < 0)
            {
                return true;
            }

            var tryGetWinner = GetWinner();

            if (tryGetWinner != NPC.AffliationTypes.same)
            {
                foreach (KeyValuePair<Type, NPC.AffliationTypes> commander in commanders)
                {
                    if (commander.Value != tryGetWinner)
                    {
                        if (mH.GetNPCManager().GetCommander(commander.Key) != null)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            gameEndTimer -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            return false;

            #endregion
        }

        protected void ChangeScoreAbsolute(NPC.AffliationTypes t, int s)
        {
            int index = teams.IndexOf(t);

            //If the team exists, update it's score
            if (index != -1)
            {
                scores[index] = s;
            }
            else if (t != NPC.AffliationTypes.grey)
            {
                throw new Exception("That team doesn't exist");
            }
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