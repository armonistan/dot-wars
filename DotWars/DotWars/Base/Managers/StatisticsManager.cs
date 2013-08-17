using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class StatisticsManager
    {
        protected Dictionary<NPC.AffliationTypes, int> kills;
        protected Dictionary<NPC.AffliationTypes, int> deaths;

        protected Dictionary<NPC.AffliationTypes, int> medicKills;
        protected NPC.AffliationTypes mostKilledMedics;

        protected Dictionary<NPC.AffliationTypes, float> timeAlive;

        protected Dictionary<NPC.AffliationTypes, int> flagsCaptured;
        
        protected Dictionary<NPC.AffliationTypes, int> flagsReturned;

        protected Dictionary<NPC.AffliationTypes, int> casualities;

        private int[,] killsToCommanders;
        private int vendittaKills;
        private NPC.AffliationTypes vendittaKiller;
        private NPC.AffliationTypes vendittaVictim;

        private int rocksCreatedCounter;
        private Dictionary<NPC.AffliationTypes, int> rocksDestroyed;

        private int waterSpilledCounter;
        private int lightningTravelledCounter;
        private int dotsSetOnFireCounter;

        private Dictionary<NPC.AffliationTypes, int> dotsRecruited;
        private Dictionary<NPC.AffliationTypes, float> mostTimeFlagAway;
        private Dictionary<NPC.AffliationTypes, float> quickestFlagCapture;
        //list of teams
        protected List<NPC.AffliationTypes> affilations;
        protected List<NPC.AffliationTypes> teams;

        public StatisticsManager(List<NPC.AffliationTypes> t)
        {
            affilations = new List<NPC.AffliationTypes>();
            affilations.Add(NPC.AffliationTypes.red);
            affilations.Add(NPC.AffliationTypes.blue);
            affilations.Add(NPC.AffliationTypes.green);
            affilations.Add(NPC.AffliationTypes.yellow);
            teams = new List<NPC.AffliationTypes>();

            foreach (NPC.AffliationTypes a in t)
                if (a != NPC.AffliationTypes.black)
                    teams.Add(a);
        }

        public void Intitialize()
        {
            //kd stuff
            kills = new Dictionary<NPC.AffliationTypes, int>();
            deaths = new Dictionary<NPC.AffliationTypes, int>();
            medicKills = new Dictionary<NPC.AffliationTypes, int>();
            timeAlive = new Dictionary<NPC.AffliationTypes, float>();
            flagsCaptured = new Dictionary<NPC.AffliationTypes, int>();
            flagsReturned = new Dictionary<NPC.AffliationTypes, int>();
            rocksDestroyed = new Dictionary<NPC.AffliationTypes, int>();
            dotsRecruited = new Dictionary<NPC.AffliationTypes, int>();
            casualities = new Dictionary<NPC.AffliationTypes, int>();

            killsToCommanders = new int[4, 4];
            vendittaKills = 0;
            vendittaKiller = NPC.AffliationTypes.black;
            vendittaVictim = NPC.AffliationTypes.black;
            rocksCreatedCounter = 0;
            waterSpilledCounter = 0;
            lightningTravelledCounter = 0;
            foreach (NPC.AffliationTypes a in affilations)
            {
                kills.Add(a, 0);
                deaths.Add(a, 0);
                medicKills.Add(a, 0);
                timeAlive.Add(a, 0);
                flagsCaptured.Add(a, 0);
                flagsReturned.Add(a, 0);
                rocksDestroyed.Add(a, 0);
                dotsRecruited.Add(a, 0);
                casualities.Add(a, 0);
            }
        }

        public void Update(ManagerHelper mH) 
        {
            kills = mH.GetNPCManager().GetKills();
            deaths = mH.GetNPCManager().GetDeaths();
            medicKills = mH.GetNPCManager().GetMedicKills();
            timeAlive = mH.GetNPCManager().GetMaxTimeAlive();
            flagsCaptured = mH.GetGametype().GetFlagsCaptured();
            flagsReturned = mH.GetGametype().GetFlagsReturned();
            killsToCommanders = mH.GetNPCManager().GetKillsToCommanders();
            rocksCreatedCounter = (mH.GetNPCManager().GetSpecificCommander(NPC.AffliationTypes.green) != null) ? 
                mH.GetNPCManager().GetSpecificCommander(NPC.AffliationTypes.green).GetPowerStatistic() :
                rocksCreatedCounter;
            rocksDestroyed = mH.GetAbilityManager().GetRocksDestroyedByCommanders();
            waterSpilledCounter = mH.GetAbilityManager().GetWaterSpilledCounter();
            dotsRecruited = mH.GetGametype().GetDotsRecruited();
            lightningTravelledCounter = mH.GetAbilityManager().GetLightningTravelledCounter();
            mostTimeFlagAway = mH.GetGametype().GetMostTimeFlagAway();
            quickestFlagCapture = mH.GetGametype().GetQuickestFlagCapture();
            dotsSetOnFireCounter = mH.GetAbilityManager().GetDotsSetOnFireCounter();
            casualities = mH.GetNPCManager().GetCasualities();
        }

        public Dictionary<NPC.AffliationTypes, int> GetKills()
        {
            return kills;
        }
        
        public Dictionary<NPC.AffliationTypes, int> GetDeaths()
        {
            return deaths;
        }

        private bool CalculateMostMedicsKilled()
        {
            Dictionary<NPC.AffliationTypes, int> mostMedicsKilled = new Dictionary<NPC.AffliationTypes,int>();
            int max = 0;
            mostKilledMedics = NPC.AffliationTypes.black;

            foreach (NPC.AffliationTypes a in affilations) 
            {
                if (max < medicKills[a])
                {
                    mostKilledMedics = a;
                    max = medicKills[a];
                }
            }

            mostMedicsKilled.Add(mostKilledMedics, max);
            
            return (NPC.AffliationTypes.black != mostKilledMedics);
        }

        private NPC.AffliationTypes CalculateMaxTime()
        {
            float max = 0;
            NPC.AffliationTypes team = NPC.AffliationTypes.black;
            bool immortal = false; //flag that indicates someone never died

            foreach (NPC.AffliationTypes a in affilations)
            {
                if (timeAlive[a] == 0)
                {
                    immortal = true;
                    team = a;
                    max = timeAlive[a];
                }
                else if (!immortal && timeAlive[a] > max)
                {
                    team = a;
                    max = timeAlive[a];
                }
            }
            return team;
        }

        private NPC.AffliationTypes CalculateMostFlagsCaputured()
        {
            NPC.AffliationTypes a = NPC.AffliationTypes.black;
            int flagsCapped = 0;

            foreach (NPC.AffliationTypes af in affilations)
            {
                if(flagsCapped < flagsCaptured[af])
                {
                    a = af;
                    flagsCapped = flagsCaptured[af];
                }
            }

            return a;
        }

        private NPC.AffliationTypes CalculateMostFlagsReturned()
        {
            NPC.AffliationTypes a = NPC.AffliationTypes.black;
            int flagsReturn = 0;

            foreach (NPC.AffliationTypes af in affilations)
            {
                if (flagsReturn < flagsReturned[af])
                {
                    a = af;
                    flagsReturn = flagsReturned[af];
                }
            }

            return a;
        }

        private NPC.AffliationTypes CalculateMostRocksDestroyed()
        {
            NPC.AffliationTypes a = NPC.AffliationTypes.black;
            int rocksCrushed = 0;
            
            foreach(NPC.AffliationTypes af in affilations)
                if (rocksCrushed < rocksDestroyed[af])
                {
                    rocksCrushed = rocksDestroyed[af];
                    a = af;
                }

            return a;
        }

        private void CalculateVenditta()
        {
            for (int killer = 0; killer < 4; killer++)
                for (int victim = 0; victim < 4; victim++)
                    if (killsToCommanders[killer, victim] > vendittaKills)
                    {
                        vendittaKills = killsToCommanders[killer, victim];

                        switch (killer)
                        {
                            case 0:
                                {
                                    switch (victim)
                                    {
                                        case 1:
                                            vendittaKiller = NPC.AffliationTypes.red;
                                            vendittaVictim = NPC.AffliationTypes.blue;
                                            break;
                                        case 2:
                                            vendittaKiller = NPC.AffliationTypes.red;
                                            vendittaVictim = NPC.AffliationTypes.green;
                                            break;
                                        case 3:
                                            vendittaKiller = NPC.AffliationTypes.red;
                                            vendittaVictim = NPC.AffliationTypes.yellow;
                                            break;
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    switch (victim)
                                    {
                                        case 0:
                                            vendittaKiller = NPC.AffliationTypes.blue;
                                            vendittaVictim = NPC.AffliationTypes.red;
                                            break;
                                        case 2:
                                            vendittaKiller = NPC.AffliationTypes.blue;
                                            vendittaVictim = NPC.AffliationTypes.green;
                                            break;
                                        case 3:
                                            vendittaKiller = NPC.AffliationTypes.blue;
                                            vendittaVictim = NPC.AffliationTypes.yellow;
                                            break;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    switch (victim)
                                    {
                                        case 1:
                                            vendittaKiller = NPC.AffliationTypes.green;
                                            vendittaVictim = NPC.AffliationTypes.blue;
                                            break;
                                        case 0:
                                            vendittaKiller = NPC.AffliationTypes.green;
                                            vendittaVictim = NPC.AffliationTypes.red;
                                            break;
                                        case 3:
                                            vendittaKiller = NPC.AffliationTypes.green;
                                            vendittaVictim = NPC.AffliationTypes.yellow;
                                            break;
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    switch (victim)
                                    {
                                        case 1:
                                            vendittaKiller = NPC.AffliationTypes.yellow;
                                            vendittaVictim = NPC.AffliationTypes.blue;
                                            break;
                                        case 2:
                                            vendittaKiller = NPC.AffliationTypes.yellow;
                                            vendittaVictim = NPC.AffliationTypes.green;
                                            break;
                                        case 0:
                                            vendittaKiller = NPC.AffliationTypes.yellow;
                                            vendittaVictim = NPC.AffliationTypes.red;
                                            break;
                                    }
                                    break;
                                }

                        }
                    }
        }

        private NPC.AffliationTypes CalculateMostDotsRecruited()
        {
            NPC.AffliationTypes a = NPC.AffliationTypes.black;
            int dotsDeployed = 0;

            foreach (NPC.AffliationTypes af in affilations)
            {
                if (dotsDeployed < dotsRecruited[af])
                {
                    dotsDeployed = dotsRecruited[af];
                    a = af;
                }
            }

            return a;
        }

        public String GetMostMedicsKilledStatistic() 
        {
            String result = "";

            if (CalculateMostMedicsKilled())
            {
                switch (mostKilledMedics)
                {
                    case NPC.AffliationTypes.red:
                        result += "Mustachio ";
                        break;
                    case NPC.AffliationTypes.blue:
                        result += "Aquoes ";
                        break;
                    case NPC.AffliationTypes.green:
                        result += "Terron ";
                        break;
                    case NPC.AffliationTypes.yellow:
                        result += "Dian ";
                        break;
                }
                result += "killed " + medicKills[mostKilledMedics] + " harmless medics";
            }
            else
                return "No medics were harmed!";

            return result;
        }

        public String GetKDStatistics()
        {
            String result = "";

            foreach (NPC.AffliationTypes a in affilations)
            {
                if (a != NPC.AffliationTypes.black)
                {
                   switch (a)
                   {
                       case NPC.AffliationTypes.red:
                           result += "Mustachio:  ";
                           break;
                       case NPC.AffliationTypes.blue:
                           result += "Aquoes:      ";
                           break;
                       case NPC.AffliationTypes.green:
                           result += "Terron:      ";
                           break;
                       case NPC.AffliationTypes.yellow:
                           result += "Dian:         ";
                           break;
                   }

                    result += ("K: " + kills[a] + " D: " + deaths[a] + "\n");
                }
            }
            return result;
        }

        public String GetMaxTimeAliveStatistic()
        {
            String result = "";

            switch (CalculateMaxTime())
            {
                 case NPC.AffliationTypes.red:
                     result += "Mustachio ";
                     break;
                 case NPC.AffliationTypes.blue:
                     result += "Aquoes ";
                     break;
                 case NPC.AffliationTypes.green:
                     result += "Terron ";
                     break;
                 case NPC.AffliationTypes.yellow:
                     result += "Dian ";
                     break;
             }
            if ((int)timeAlive[CalculateMaxTime()] == 0)
                result += "never died!";
            else
                result += " had the longest \nlife of " + (int)timeAlive[CalculateMaxTime()] + " seconds";
            
            return result;
        }

        public String GetMostFlagsCaputuredStatistic()
        {
            String result = "";

            switch (CalculateMostFlagsCaputured())
            {
                case NPC.AffliationTypes.red:
                    result += "Mustachio captured the \nmost flags (";
                    break;
                case NPC.AffliationTypes.blue:
                    result += "Aquoes captured the \nmost flags (";
                    break;
                case NPC.AffliationTypes.green:
                    result += "Terron captured the \nmost flags (";
                    break;
                case NPC.AffliationTypes.yellow:
                    result += "Dian captured the \nmost flags (";
                    break;
                case NPC.AffliationTypes.black:
                    return "No one bothered \ncapturing a flag";
            }
            result += "" + flagsCaptured[CalculateMostFlagsCaputured()] + ")";

            return result;
        }

        public String GetMostFlagsReturnedStatistic()
        {
            String result = "";

            switch (CalculateMostFlagsReturned())
            {
                case NPC.AffliationTypes.red:
                    result += "Mustachio returned \nthe most flags(";
                    break;
                case NPC.AffliationTypes.blue:
                    result += "Aquoes returned the \nmost flags(";
                    break;
                case NPC.AffliationTypes.green:
                    result += "Terron returned the \nmost flags(";
                    break;
                case NPC.AffliationTypes.yellow:
                    result += "Dian returned the \nmost flags(";
                    break;
                case NPC.AffliationTypes.black:
                    return "No one cared to \nreturn their flag";
            }
            result += "" + flagsReturned[CalculateMostFlagsCaputured()] + ")";

            return result;
        }

        public String GetVendittaStatistic()
        {
            String result = "";
            CalculateVenditta();
            switch (vendittaKiller)
            {
                case NPC.AffliationTypes.red:
                    {
                        switch (vendittaVictim)
                        {
                            case NPC.AffliationTypes.blue:
                                result = "Mustachio killed \nAquoes " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.green:
                                result = "Mustachio killed \nTerron " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.yellow:
                                result = "Mustachio killed \nDian " + vendittaKills + " times";
                                break;
                        }
                        break;
                    }
                case NPC.AffliationTypes.blue:
                    {
                        switch (vendittaVictim)
                        {
                            case NPC.AffliationTypes.red:
                                result = "Aquoes killed \nMustachio " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.green:
                                result = "Aquoes killed \nTerron " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.yellow:
                                result = "Aquoes killed \nDian " + vendittaKills + " times";
                                break;
                        }
                        break;
                    }
                case NPC.AffliationTypes.green:
                    {
                        switch (vendittaVictim)
                        {
                            case NPC.AffliationTypes.blue:
                                result = "Terron killed \nAquoes " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.red:
                                result = "Terron killed \nMustachio " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.yellow:
                                result = "Terron killed \nDian " + vendittaKills + " times";
                                break;
                        }
                        break;
                    }
                case NPC.AffliationTypes.yellow:
                    {
                        switch (vendittaVictim)
                        {
                            case NPC.AffliationTypes.blue:
                                result = "Dian killed \nAquoes " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.green:
                                result = "Dian killed \nTerron " + vendittaKills + " times";
                                break;
                            case NPC.AffliationTypes.red:
                                result = "Dian killed \nMustachio " + vendittaKills + " times";
                                break;
                        }
                        break;
                    }
            }

            return result;
        }

        public String GetYellowCommanderPowerStatistic()
        {
            return "Dian left " + lightningTravelledCounter + "\n" + "meters of lightning";
        }

        public String GetGreenCommanderPowerStatistic()
        {
            return "Terron created " + rocksCreatedCounter + "\n" + " rocks";
        }

        public String GetBlueCommanderPowerStatistic()
        {
            return "Aquoes spilled " + waterSpilledCounter + "\n" + "gallons of water";
        }
       
        public String GetRedCommanderPowerStatistic()
        {
            return "Mustachio set " + dotsSetOnFireCounter + " dots" + "\n" + "on fire";
        }
        
        public String GetRocksDestroyedStatistic()
        {
            String result = "";

            switch (CalculateMostRocksDestroyed())
            {
                case NPC.AffliationTypes.red:
                    result += "Mustachio ";
                    break;
                case NPC.AffliationTypes.blue:
                    result += "Aquoes ";
                    break;
                case NPC.AffliationTypes.green:
                    result += "Terron ";
                    break;
                case NPC.AffliationTypes.yellow:
                    result += "Dian ";
                    break;
                case NPC.AffliationTypes.black:
                    return "No rocks were hurt this game.";
            }
            result += "destroyed " + rocksDestroyed[CalculateMostRocksDestroyed()] + " rocks";


            return result;
        }

        public String GetMostDotsRecruitedStatistic()
        {
            String result = "";

            switch (CalculateMostDotsRecruited())
            {
                case NPC.AffliationTypes.red:
                    result += "Mustachio ";
                    break;
                case NPC.AffliationTypes.blue:
                    result += "Aquoes ";
                    break;
                case NPC.AffliationTypes.green:
                    result += "Terron ";
                    break;
                case NPC.AffliationTypes.yellow:
                    result += "Dian ";
                    break;
                case NPC.AffliationTypes.black:
                    return "No dots were sent to their deaths this game.";
            }
            result += "recruited " + dotsRecruited[CalculateMostDotsRecruited()] + " dots";


            return result;
        }

        public String GetMostTimeFlagAwayStatistic()
        {
            float maxTime = 0;
            NPC.AffliationTypes af = NPC.AffliationTypes.black;

            foreach(NPC.AffliationTypes a in teams)
            {
                if (mostTimeFlagAway[a] > maxTime)
                {
                    maxTime = mostTimeFlagAway[a];
                    af = a;
                }
            }

            switch (af)
            {
                case NPC.AffliationTypes.red:
                    return "Red flag " + maxTime + " seconds";
                case NPC.AffliationTypes.blue:
                    return "Blue flag " + maxTime + " seconds";
                case NPC.AffliationTypes.green:
                    return "Green flag " + maxTime + " seconds";
                case NPC.AffliationTypes.yellow:
                    return "Yellow flag " + maxTime + " seconds";
                case NPC.AffliationTypes.black:
                    return "No flag was abducted in this game";
            }

            return "heh";
        }

        public String GetQuickestFlagCaptureStatistic()
        {
            float minTime = 1000;
            NPC.AffliationTypes af = NPC.AffliationTypes.black;

            foreach (NPC.AffliationTypes a in teams)
            {
                if (quickestFlagCapture[a] < minTime)
                {
                    minTime = quickestFlagCapture[a];
                    af = a;
                }
            }

            switch (af)
            {
                case NPC.AffliationTypes.red:
                    return "Quickest flag capture was \nred flag in " + minTime + " seconds";
                case NPC.AffliationTypes.blue:
                    return "Quickest flag capture was \nblue flag in " + minTime + " seconds";
                case NPC.AffliationTypes.green:
                    return "Quickest flag capture was \ngreen flag in " + minTime + " seconds";
                case NPC.AffliationTypes.yellow:
                    return "Quickest flag capture was \nyellow flag in " + minTime + " seconds";
                case NPC.AffliationTypes.black:
                    return "No flag was harmed \nin this game";
            }

            return "heh";
        }

        public String GetCasualitiesStatistic()
        {
            string result = "";
            int maxCasualities = 0;
            NPC.AffliationTypes affilationOfMaxCasualities = NPC.AffliationTypes.black;

            foreach (NPC.AffliationTypes a in teams)
            {
                if (casualities[a] > maxCasualities)
                {
                    maxCasualities = casualities[a];
                    affilationOfMaxCasualities = a;
                }
            }

            switch (affilationOfMaxCasualities)
            {
                case NPC.AffliationTypes.red:
                    result += "Red Team suffered \n" + maxCasualities + " casualities";
                    break;
                case NPC.AffliationTypes.blue:
                    result += "Blue Team suffered \n" + maxCasualities + " casualities";
                    break;
                case NPC.AffliationTypes.green:
                    result += "Green Team suffered \n" + maxCasualities + " casualities";
                    break;
                case NPC.AffliationTypes.yellow:
                    result += "Yellow Team suffered \n" + maxCasualities + " casualities";
                    break;
            }

            return result;
        }
    }
}
