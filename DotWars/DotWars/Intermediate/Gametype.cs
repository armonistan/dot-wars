#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class Gametype
    {
        #region Declarations

        private int NUMBOMBARDIER;
        private int NUMGRUNT;
        private int NUMGUNNER;
        private int NUMJUGGERNAUT;
        private int NUMMEDIC;
        private int NUMSNIPER;
        private int NUMSPECIALIST;
        private readonly int TEAMCAP;

        protected Dictionary<Type, NPC.AffliationTypes> commanders;
        protected List<NPC.AffliationTypes> teams;

        protected double gameEndTimer;
        protected int[] scores;
        protected int winScore;
        public GT typeOfGame;

        protected double spawnTime;
        protected double[] spawnsCounters;

        protected ManagerHelper managers;

        protected int[] flagsCaptured;
        protected int[] flagsReturned;
        protected int[] dotsRecruited;
        protected double[] timeFlagAway;
        protected double[] mostTimeFlagAway;
        protected double[] quickestFlagCapture;

        #endregion

        public enum GT
        {
            ASSASSINS,
            CONQUEST,
            DEATHMATCH,
            SURVIVAL,
            ASSAULT,
            CTF
        }

        public Gametype(List<NPC.AffliationTypes> tL, Dictionary<Type, NPC.AffliationTypes> pL, int wS, int pC, float sT)
        {
            //Higher level gametype sets up teams
            teams = tL;
            scores = new int[teams.Count];
            TEAMCAP = pC;

            //Set up players
            commanders = pL;

            //Same with winScore
            winScore = wS;

            //spawning stuff
            spawnsCounters = new double[commanders.Count];

            spawnTime = sT;

            gameEndTimer = 300;
        }

        public void Initialize(ManagerHelper mH)
        {
            //constant hard caps (assuming the team numbers are 8 or higher for all deathmatchs)
            NUMJUGGERNAUT = 2; //hard cap of two juggernauts and that is it
            NUMSNIPER = 2;
            NUMBOMBARDIER = 2;

            //caps that depend on size of teams
            NUMGUNNER = TEAMCAP/3; //three represents grunts, on average for every three grunts there should be a gunner
            NUMMEDIC = TEAMCAP/4; //see above 
            NUMSPECIALIST = TEAMCAP/4; //see above the above
            NUMGRUNT = ((this is Survival) ? TEAMCAP : TEAMCAP/2); //half the team can be grunts

            managers = mH;

            flagsCaptured = new int[4];
            flagsReturned = new int[4];
            dotsRecruited = new int[4];
            timeFlagAway = new double[4];
            mostTimeFlagAway = new double[4];
            quickestFlagCapture = new double[4];

            for (int t = 0; t < 4; t++)
            {
                flagsCaptured[t] = 0;
                flagsReturned[t] = 0;
                dotsRecruited[t] = 0;
                timeFlagAway[t] = 0;
                mostTimeFlagAway[t] = 0;
                quickestFlagCapture[t] = double.MaxValue;
            }
        }

        public virtual bool Update(ManagerHelper mH)
        {
            #region Spawning Commanders

            int counter = 0;
            foreach (var commander in commanders)
            {
                if (mH.GetNPCManager().GetCommander(commander.Key) == null)
                {
                    if (spawnsCounters[counter] > spawnTime)
                    {
                        SpawnCommander(mH, commander.Key, commander.Value, mH.GetSpawnHelper().Spawn(commander.Value));

                        spawnsCounters[counter] = 0;
                    }
                    else
                    {
                        spawnsCounters[counter] += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
                    }
                }

                counter++;
            }

            #endregion

            if (winScore != -1)
            {
                if (GetWinner() != NPC.AffliationTypes.same)
                {
                    return true;
                }
            }

            if (gameEndTimer < 0)
            {
                return true;
            }

            gameEndTimer -= mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            return false;
        }

        public virtual void SpawnCommander(ManagerHelper mH, Type commanderType, NPC.AffliationTypes team, Vector2 point)
        {
            if (commanderType == typeof (RedCommander))
            {
                mH.GetNPCManager().Add(new RedCommander(point, team));
            }
            else if (commanderType == typeof (BlueCommander))
            {
                mH.GetNPCManager().Add(new BlueCommander(point, team));
            }
            else if (commanderType == typeof (GreenCommander))
            {
                mH.GetNPCManager().Add(new GreenCommander(point, team));
            }
            else if (commanderType == typeof (YellowCommander))
            {
                mH.GetNPCManager().Add(new YellowCommander(point, team));
            }
            else if (commanderType == typeof (RedPlayerCommander))
            {
                mH.GetNPCManager().Add(new RedPlayerCommander(point, team, mH));
            }
            else if (commanderType == typeof (BluePlayerCommander))
            {
                mH.GetNPCManager().Add(new BluePlayerCommander(point, team, mH));
            }
            else if (commanderType == typeof (GreenPlayerCommander))
            {
                mH.GetNPCManager().Add(new GreenPlayerCommander(point, team, mH));
            }
            else if (commanderType == typeof (YellowPlayerCommander))
            {
                mH.GetNPCManager().Add(new YellowPlayerCommander(point, team, mH));
            }
        }

        public virtual void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            //Nothing by default
        }

        public virtual void DrawTop(SpriteBatch sB, Vector2 d)
        {
        }

        public virtual void ChangeScore(NPC agent, int s)
        {
            NPC.AffliationTypes t = agent.GetAffiliation();

            //If the team exists, update it's score
            if (teams.Contains(t))
            {
                scores[teams.IndexOf(t)] += s;
            }

                //Otherwise, throw an exception
            else if (t != NPC.AffliationTypes.grey)
            {
                throw new Exception("That team doesn't exist");
            }
        }

        protected virtual void ChangeScoreAbsolute(NPC agent, int s)
        {
            NPC.AffliationTypes t = agent.GetAffiliation();

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

        public bool Spawn(ManagerHelper mH, NPC.AffliationTypes aT, Vector2 p, int aiType)
        {
            int unitType;

            //Spawn a new unit based on color of toucher
            if (mH.GetGametype() is Survival)
                unitType = 0;
            else
                unitType = mH.GetRandom().Next(7);

            //unitType = 0;//temp temp temp

            switch (aT)
            {
                    #region Red Team Spawns

                case NPC.AffliationTypes.red:
                    switch (unitType)
                    {
                        case 0:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gr in mH.GetNPCManager().GetAllies(aT))
                                    if (gr is Grunt)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGRUNT)
                                {
                                    switch (aiType)
                                    {
                                        case 0:
                                        case 3:
                                        case 6:
                                            mH.GetNPCManager().Add(new RedGrunt(p));
                                            break;

                                        case 1:
                                            mH.GetNPCManager().Add(new RedDefensiveConquestGrunt(p));
                                            break;

                                        case 2:
                                            mH.GetNPCManager().Add(new RedOffensiveConquestGrunt(p));
                                            break;

                                        case 4:
                                            mH.GetNPCManager().Add(new RedDefensiveAssaultGrunt(p));
                                            break;

                                        case 5:
                                            mH.GetNPCManager().Add(new RedOffensiveAssaultGrunt(p));
                                            break;

                                        case 7:
                                            mH.GetNPCManager().Add(new RedDefensiveCTFGrunt(p));
                                            break;

                                        case 8:
                                            mH.GetNPCManager().Add(new RedOffensiveCTFGrunt(p));
                                            break;
                                    }

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 1:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gn in mH.GetNPCManager().GetAllies(aT))
                                    if (gn is Gunner)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGUNNER)
                                {
                                    mH.GetNPCManager().Add(new RedGunner(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 3:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sn in mH.GetNPCManager().GetAllies(aT))
                                    if (sn is Sniper)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSNIPER)
                                {
                                    mH.GetNPCManager().Add(new RedSniper(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 2:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sp in mH.GetNPCManager().GetAllies(aT))
                                    if (sp is Specialist)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSPECIALIST)
                                {
                                    mH.GetNPCManager().Add(new RedSpecialist(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 4:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC b in mH.GetNPCManager().GetAllies(aT))
                                    if (b is Bombardier)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMBOMBARDIER)
                                {
                                    mH.GetNPCManager().Add(new RedBombardier(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 5:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC j in mH.GetNPCManager().GetAllies(aT))
                                    if (j is Juggernaut)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMJUGGERNAUT)
                                {
                                    mH.GetNPCManager().Add(new RedJuggernaut(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 6:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC m in mH.GetNPCManager().GetAllies(aT))
                                    if (m is Medic)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMMEDIC)
                                {
                                    mH.GetNPCManager().Add(new RedMedic(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                    }
                    break;

                    #endregion

                    #region Blue Team Spawns

                case NPC.AffliationTypes.blue:
                    switch (unitType)
                    {
                        case 0:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gr in mH.GetNPCManager().GetAllies(aT))
                                    if (gr is Grunt)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGRUNT)
                                {
                                    switch (aiType)
                                    {
                                        case 0:
                                        case 3:
                                        case 6:
                                            mH.GetNPCManager().Add(new BlueGrunt(p));
                                            break;

                                        case 1:
                                            mH.GetNPCManager().Add(new BlueDefensiveConquestGrunt(p));
                                            break;

                                        case 2:
                                            mH.GetNPCManager().Add(new BlueOffensiveConquestGrunt(p));
                                            break;

                                        case 4:
                                            mH.GetNPCManager().Add(new BlueDefensiveAssaultGrunt(p));
                                            break;

                                        case 5:
                                            mH.GetNPCManager().Add(new BlueOffensiveAssaultGrunt(p));
                                            break;

                                        case 7:
                                            mH.GetNPCManager().Add(new BlueDefensiveCTFGrunt(p));
                                            break;

                                        case 8:
                                            mH.GetNPCManager().Add(new BlueOffensiveCTFGrunt(p));
                                            break;
                                    }

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 1:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gn in mH.GetNPCManager().GetAllies(aT))
                                    if (gn is Gunner)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGUNNER)
                                {
                                    mH.GetNPCManager().Add(new BlueGunner(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 3:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sn in mH.GetNPCManager().GetAllies(aT))
                                    if (sn is Sniper)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSNIPER)
                                {
                                    mH.GetNPCManager().Add(new BlueSniper(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 2:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sp in mH.GetNPCManager().GetAllies(aT))
                                    if (sp is Specialist)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSPECIALIST)
                                {
                                    mH.GetNPCManager().Add(new BlueSpecialist(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 4:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC b in mH.GetNPCManager().GetAllies(aT))
                                    if (b is Bombardier)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMBOMBARDIER)
                                {
                                    mH.GetNPCManager().Add(new BlueBombardier(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 5:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC j in mH.GetNPCManager().GetAllies(aT))
                                    if (j is Juggernaut)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMJUGGERNAUT)
                                {
                                    mH.GetNPCManager().Add(new BlueJuggernaut(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 6:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC m in mH.GetNPCManager().GetAllies(aT))
                                    if (m is Medic)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMMEDIC)
                                {
                                    mH.GetNPCManager().Add(new BlueMedic(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                    }
                    break;

                    #endregion

                    #region Green Team Spawns

                case NPC.AffliationTypes.green:
                    switch (unitType)
                    {
                        case 0:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gr in mH.GetNPCManager().GetAllies(aT))
                                    if (gr is Grunt)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGRUNT)
                                {
                                    switch (aiType)
                                    {
                                        case 0:
                                        case 3:
                                        case 6:
                                            mH.GetNPCManager().Add(new GreenGrunt(p));
                                            break;

                                        case 1:
                                            mH.GetNPCManager().Add(new GreenDefensiveConquestGrunt(p));
                                            break;

                                        case 2:
                                            mH.GetNPCManager().Add(new GreenOffensiveConquestGrunt(p));
                                            break;

                                        case 4:
                                            mH.GetNPCManager().Add(new GreenDefensiveAssaultGrunt(p));
                                            break;

                                        case 5:
                                            mH.GetNPCManager().Add(new GreenOffensiveAssaultGrunt(p));
                                            break;

                                        case 7:
                                            mH.GetNPCManager().Add(new GreenDefensiveCTFGrunt(p));
                                            break;

                                        case 8:
                                            mH.GetNPCManager().Add(new GreenOffensiveCTFGrunt(p));
                                            break;
                                    }

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 1:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gn in mH.GetNPCManager().GetAllies(aT))
                                    if (gn is Gunner)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGUNNER)
                                {
                                    mH.GetNPCManager().Add(new GreenGunner(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 3:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sn in mH.GetNPCManager().GetAllies(aT))
                                    if (sn is Sniper)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSNIPER)
                                {
                                    mH.GetNPCManager().Add(new GreenSniper(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 2:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sp in mH.GetNPCManager().GetAllies(aT))
                                    if (sp is Specialist)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSPECIALIST)
                                {
                                    mH.GetNPCManager().Add(new GreenSpecialist(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 4:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC b in mH.GetNPCManager().GetAllies(aT))
                                    if (b is Bombardier)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMBOMBARDIER)
                                {
                                    mH.GetNPCManager().Add(new GreenBombardier(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 5:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC j in mH.GetNPCManager().GetAllies(aT))
                                    if (j is Juggernaut)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMJUGGERNAUT)
                                {
                                    mH.GetNPCManager().Add(new GreenJuggernaut(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 6:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC m in mH.GetNPCManager().GetAllies(aT))
                                    if (m is Medic)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMMEDIC)
                                {
                                    mH.GetNPCManager().Add(new GreenMedic(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                    }
                    break;

                    #endregion

                    #region Yellow Team Spawns

                case NPC.AffliationTypes.yellow:
                    switch (unitType)
                    {
                        case 0:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gr in mH.GetNPCManager().GetAllies(aT))
                                    if (gr is Grunt)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGRUNT)
                                {
                                    switch (aiType)
                                    {
                                        case 0:
                                        case 3:
                                        case 6:
                                            mH.GetNPCManager().Add(new YellowGrunt(p));
                                            break;

                                        case 1:
                                            mH.GetNPCManager().Add(new YellowDefensiveConquestGrunt(p));
                                            break;

                                        case 2:
                                            mH.GetNPCManager().Add(new YellowOffensiveConquestGrunt(p));
                                            break;

                                        case 4:
                                            mH.GetNPCManager().Add(new YellowDefensiveAssaultGrunt(p));
                                            break;

                                        case 5:
                                            mH.GetNPCManager().Add(new YellowOffensiveAssaultGrunt(p));
                                            break;

                                        case 7:
                                            mH.GetNPCManager().Add(new YellowDefensiveCTFGrunt(p));
                                            break;

                                        case 8:
                                            mH.GetNPCManager().Add(new YellowOffensiveCTFGrunt(p));
                                            break;
                                    }

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 1:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC gn in mH.GetNPCManager().GetAllies(aT))
                                    if (gn is Gunner)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMGUNNER)
                                {
                                    mH.GetNPCManager().Add(new YellowGunner(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 3:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sn in mH.GetNPCManager().GetAllies(aT))
                                    if (sn is Sniper)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSNIPER)
                                {
                                    mH.GetNPCManager().Add(new YellowSniper(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 2:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC sp in mH.GetNPCManager().GetAllies(aT))
                                    if (sp is Specialist)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMSPECIALIST)
                                {
                                    mH.GetNPCManager().Add(new YellowSpecialist(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 4:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC b in mH.GetNPCManager().GetAllies(aT))
                                    if (b is Bombardier)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMBOMBARDIER)
                                {
                                    mH.GetNPCManager().Add(new YellowBombardier(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 5:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC j in mH.GetNPCManager().GetAllies(aT))
                                    if (j is Juggernaut)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMJUGGERNAUT)
                                {
                                    mH.GetNPCManager().Add(new YellowJuggernaut(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case 6:
                            {
                                int numOfUnit = 0; //number of that unit on the team

                                //count how many there are
                                foreach (NPC m in mH.GetNPCManager().GetAllies(aT))
                                    if (m is Medic)
                                        numOfUnit++;
                                //if we haven't reached the pop cap, they may spawn
                                if (numOfUnit < NUMMEDIC)
                                {
                                    mH.GetNPCManager().Add(new YellowMedic(p));
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                    }
                    break;

                    #endregion
            }

            return false;
        }

        public bool Spawn(ManagerHelper mH, NPC.AffliationTypes aT, Vector2 p)
        {
            return Spawn(mH, aT, p, 0);
        }

        #region Gets

        public List<NPC.AffliationTypes> GetTeams()
        {
            return teams;
        }

        public int[] GetScores()
        {
            return scores;
        }

        public NPC.AffliationTypes GetTeammaterColor(Type commander)
        {
            NPC.AffliationTypes yourColor;

            if (commanders.TryGetValue(commander, out yourColor))
            {
                for (int i = 0; i < commanders.Count; i++)
                {
                    if (commanders.Keys.ElementAt(i) != commander && commanders.Values.ElementAt(i) == yourColor)
                    {
                        return NPC.CommanderColor(commanders.Keys.ElementAt(i));
                    }
                }
            }

            return NPC.AffliationTypes.grey;
        }

        public virtual NPC.AffliationTypes GetWinner()
        {
            int highestScoreIndex = -1;
            int highestScore = -1;
            bool isTie = true;

            //Go through each team and see if they won
            for (int i = 0; i < teams.Count; i++)
            {
                if (gameEndTimer > 0.0 && scores[i] >= winScore)
                {
                    return teams[i];
                }
                else if (gameEndTimer <= 0.0)
                {
                    if (scores[i] > highestScore)
                    {
                        highestScoreIndex = i;
                        highestScore = scores[i];
                        isTie = false;
                    }
                    else if (scores[i] == highestScore)
                        isTie = true;
                }
            }

            if (isTie)
                return NPC.AffliationTypes.same;
            else
                return teams[highestScoreIndex];
        }

        public List<NPC.AffliationTypes> GetHighestScores()
        {
            int highestScore = scores.Max();
            var tempHighestScores = new List<NPC.AffliationTypes>();

            //Go through each team and see who had the highest score
            for (int i = 0; i < teams.Count; i++)
            {
                if (scores[i] == highestScore)
                {
                    tempHighestScores.Add(teams[i]);
                }
            }

            return tempHighestScores;
        }

        public double GetGameEndTimer()
        {
            return gameEndTimer;
        }

        public double GetSpawnTime()
        {
            return spawnTime;
        }

        public int GetPopCap()
        {
            return TEAMCAP;
        }

        public String GetWinnerString()
        {
            switch (this.GetWinner())
            {
                case NPC.AffliationTypes.red:
                    return "Red Team Won!";
                case NPC.AffliationTypes.blue:
                    return "Blue Team Won!";
                case NPC.AffliationTypes.green:
                    return "Green Team Won!";
                case NPC.AffliationTypes.yellow:
                    return "Yellow Team Won!";
                case NPC.AffliationTypes.same:
                    return "Tie Game";
            }
            return "error";
        }

        public String[] GetEndScores()
        {
            String[] score = new String[4];

            foreach (NPC.AffliationTypes teamColor in teams)
            {
                if (teamColor == NPC.AffliationTypes.red)
                {
                    score[0] = scores[teams.IndexOf(teamColor)] + "";
                }
                else if (teamColor == NPC.AffliationTypes.blue)
                {
                    score[1] = scores[teams.IndexOf(teamColor)] + "";
                }
                else if (teamColor == NPC.AffliationTypes.green)
                {
                    score[2] = scores[teams.IndexOf(teamColor)] + "";
                }
                else if (teamColor == NPC.AffliationTypes.yellow)
                {
                    score[3] = scores[teams.IndexOf(teamColor)] + "";
                }
            }

            return score;
        }

        public virtual String GetGametypeStatistics()
        {
            return "";
        }

        public int[] GetFlagsCaptured()
        {
            return flagsCaptured;
        }

        public Dictionary<Type, NPC.AffliationTypes> GetPlayers()
        {
            return commanders;
        }

        public NPC.AffliationTypes GetSecondaryWinner(NPC.AffliationTypes winner)
        {
            foreach (KeyValuePair<Type, NPC.AffliationTypes> commander in commanders)
            {
                if (NPC.CommanderColor(commander.Key) != winner && commander.Value == winner)
                {
                    return NPC.CommanderColor(commander.Key);
                }
            }

            return NPC.AffliationTypes.red;
        }

        public int[] GetFlagsReturned()
        {
            return flagsReturned;
        }

        public double[] GetMostTimeFlagAway()
        {
            return mostTimeFlagAway;
        }

        public int[] GetDotsRecruited()
        {
            return dotsRecruited;
        }

        public double[] GetQuickestFlagCapture()
        {
            return quickestFlagCapture;
        }

        public void UpdateFlagsReturned(NPC.AffliationTypes a)
        {
            flagsReturned[NPC.GetTeam(a)]++;
        }

        public void UpdateDotsRecruited(NPC.AffliationTypes a)
        {
            dotsRecruited[NPC.GetTeam(a)]++;
        }

        public void UpdateTimeFlagAway(NPC.AffliationTypes a)
        {
            timeFlagAway[NPC.GetTeam(a)] += managers.GetGameTime().ElapsedGameTime.TotalSeconds;
        }

        public void UpdateMostTimeFlagAway(NPC.AffliationTypes a)
        {
            if (timeFlagAway[NPC.GetTeam(a)] > mostTimeFlagAway[NPC.GetTeam(a)])
                mostTimeFlagAway[NPC.GetTeam(a)] = timeFlagAway[NPC.GetTeam(a)];
        }

        public void UpdateQuickestSuccessfulCapture(NPC.AffliationTypes a)
        {
            if (timeFlagAway[NPC.GetTeam(a)] < quickestFlagCapture[NPC.GetTeam(a)])
                quickestFlagCapture[NPC.GetTeam(a)] = timeFlagAway[NPC.GetTeam(a)];
        }

        #endregion

        public virtual String GetName()
        {
            return "Gametype";
        }

        public virtual String GetSummary()
        {
            return "The Gametype 3, \nThe Return of 3";
        }
    }
}