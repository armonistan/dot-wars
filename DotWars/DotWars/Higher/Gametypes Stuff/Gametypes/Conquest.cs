using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Conquest : Gametype
    {
        #region Declarations

        //Determines when to spawn
        private readonly float spawnSecs;

        //list of bases
        public List<ConquestBase> bases;
        private float counter;

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

            if (teams.Contains(NPC.AffliationTypes.red))
            {
                ChangeScoreAbsolute(NPC.AffliationTypes.red, GetAlliedBases(NPC.AffliationTypes.red).Count);
            }
            if (teams.Contains(NPC.AffliationTypes.blue))
            {
                ChangeScoreAbsolute(NPC.AffliationTypes.blue, GetAlliedBases(NPC.AffliationTypes.blue).Count);
            }
            if (teams.Contains(NPC.AffliationTypes.green))
            {
                ChangeScoreAbsolute(NPC.AffliationTypes.green, GetAlliedBases(NPC.AffliationTypes.green).Count);
            }
            if (teams.Contains(NPC.AffliationTypes.yellow))
            {
                ChangeScoreAbsolute(NPC.AffliationTypes.yellow, GetAlliedBases(NPC.AffliationTypes.yellow).Count);
            }

            //Spawn Section
            if (counter > spawnSecs)
            {
                counter = 0;

                foreach (ConquestBase b in bases)
                {
                    if (b.affiliation != NPC.AffliationTypes.grey)
                    {
                        Spawn(mH, b.affiliation,
                              mH.GetSpawnHelper().Spawn(b.affiliation, mH, (GetAlliedBases(b.affiliation).Count == 0)),
                              mH.GetRandom().Next(3));
                    }
                }
            }
            else
            {
                counter += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            #region base code slightly modified

            #region Spawning Commanders

            if (!(mH.GetLevel() is Menu))
            {
                for (int i = 0; i < commanders.Count; i++)
                {
                    if (mH.GetNPCManager().GetCommander(commanders.Keys.ElementAt(i)) == null)
                    {
                        if (spawnsCounters[i] > spawnTime && (!HasSomeoneWon() || GetWinner() == commanders.Values.ElementAt(i)))
                        {
                            if (commanders.Keys.ElementAt(i) == typeof (RedCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new RedCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i)));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (BlueCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new BlueCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i)));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (GreenCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new GreenCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i)));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (YellowCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new YellowCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i)));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (RedPlayerCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new RedPlayerCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i), mH));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (BluePlayerCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new BluePlayerCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i), mH));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (GreenPlayerCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new GreenPlayerCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
                                          commanders.Values.ElementAt(i), mH));
                                spawnsCounters[i] = 0;
                            }
                            else if (commanders.Keys.ElementAt(i) == typeof (YellowPlayerCommander))
                            {
                                mH.GetNPCManager()
                                  .Add(
                                      new YellowPlayerCommander(
                                          mH.GetSpawnHelper()
                                            .Spawn(commanders.Values.ElementAt(i), mH,
                                                   (GetAlliedBases(commanders.Values.ElementAt(i)).Count == 0)),
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

            gameEndTimer -= (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            return false;

            #endregion
        }

        protected void ChangeScoreAbsolute(NPC.AffliationTypes t, int s)
        {
            //If the team exists, update it's score
            if (teams.Contains(t))
            {
                scores[teams.IndexOf(t)] = s;
            }

                //Otherwise, throw an exception
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

        public List<ConquestBase> GetEnemyBases(NPC.AffliationTypes a)
        {
            //Holds the temporary list of greys
            var bL = new List<ConquestBase>();

            //Cycles through all agents
            foreach (ConquestBase b in bases)
            {
                if (b.affiliation != a)
                    bL.Add(b);
            }

            //Returns the completed list
            return bL;
        }

        #region Gets

        public List<ConquestBase> GetAlliedBases(NPC.AffliationTypes a)
        {
            //Holds the temporary list of greys
            var bL = new List<ConquestBase>();

            //Cycles through all agents
            foreach (ConquestBase b in bases)
            {
                if (b.affiliation == a)
                    bL.Add(b);
            }

            //Returns the completed list
            return bL;
        }

        public ConquestBase GetClosestInList(List<ConquestBase> bL, Vector2 p)
        {
            if (bL.Count > 0)
            {
                ConquestBase closest = bL.First();
                var minDist =
                    (float)
                    (Math.Sqrt(Math.Pow(p.X - closest.GetOriginPosition().X, 2) +
                               Math.Pow(p.Y - closest.GetOriginPosition().Y, 2)));

                foreach (ConquestBase b in bL)
                {
                    var aDist =
                        (float)
                        (Math.Sqrt(Math.Pow(p.X - b.GetOriginPosition().X, 2) +
                                   Math.Pow(p.Y - b.GetOriginPosition().Y, 2)));
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

        public List<ConquestBase> GetBases()
        {
            return bases;
        }

        public ConquestBase GetForwardBase(NPC.AffliationTypes a, ManagerHelper mH)
        {
            ConquestBase fb = null;

            if (GetAlliedBases(a).Count != 0 && GetAlliedBases(a).Count != winScore)
            {
                foreach (ConquestBase b in GetAlliedBases(a))
                {
                    if (fb == null)
                        fb = b;
                    else if (
                        PathHelper.Distance(b.GetOriginPosition(),
                                            GetClosestInList(GetEnemyBases(a), b.GetOriginPosition())
                                                .GetOriginPosition()) <
                        PathHelper.Distance(fb.GetOriginPosition(),
                                            GetClosestInList(GetEnemyBases(a), fb.GetOriginPosition())
                                                .GetOriginPosition()))
                    {
                        fb = b;
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

        #endregion

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
            return "First team to capture\nallthe bases is the winner!";
        }
    }
}