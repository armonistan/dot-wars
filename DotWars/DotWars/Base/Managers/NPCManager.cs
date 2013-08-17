using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class NPCManager
    {
        #region Declarations

        //Collection of NPCs
        private readonly Dictionary<NPC.AffliationTypes, List<NPC>> agents;
        private readonly List<Commander> commanders;
        private List<NPC.AffliationTypes> teams = new List<NPC.AffliationTypes>();
        private List<NPC> bombers;

        private ManagerHelper managers;
        private Dictionary<NPC.AffliationTypes, int> kills;
        private Dictionary<NPC.AffliationTypes, int> deaths;
        private Dictionary<NPC.AffliationTypes, int> medicKills;
        private Dictionary<NPC.AffliationTypes, float> timeAlive;
        private Dictionary<NPC.AffliationTypes, float> maxTimeAlive;
        private Dictionary<NPC.AffliationTypes, bool> isAlive;
        private Dictionary<NPC.AffliationTypes, int> casualities;
        private int[,] killsToCommanders;
        #endregion

        public NPCManager()
        {
            agents = new Dictionary<NPC.AffliationTypes, List<NPC>>();
            bombers = new List<NPC>();
            commanders = new List<Commander>();
            kills = new Dictionary<NPC.AffliationTypes, int>();
            deaths = new Dictionary<NPC.AffliationTypes, int>();
            medicKills = new Dictionary<NPC.AffliationTypes, int>();
            timeAlive = new Dictionary<NPC.AffliationTypes, float>();
            maxTimeAlive = new Dictionary<NPC.AffliationTypes, float>();
            isAlive = new Dictionary<NPC.AffliationTypes, bool>();
            casualities = new Dictionary<NPC.AffliationTypes, int>();
        }

        public NPCManager(int gruntCount, int gunnerCount, int sniperCount, int medicCount, int specialistCount,
                          int juggernautCount, int bombardierCount)
        {
            agents = new Dictionary<NPC.AffliationTypes, List<NPC>>();
            bombers = new List<NPC>();
            commanders = new List<Commander>();
            kills = new Dictionary<NPC.AffliationTypes, int>();
            deaths = new Dictionary<NPC.AffliationTypes, int>();
            medicKills = new Dictionary<NPC.AffliationTypes, int>();
            timeAlive = new Dictionary<NPC.AffliationTypes, float>();
            maxTimeAlive = new Dictionary<NPC.AffliationTypes, float>();
            isAlive = new Dictionary<NPC.AffliationTypes, bool>();
            casualities = new Dictionary<NPC.AffliationTypes, int>();
        }

        public void Initialize(ManagerHelper mH)
        {
            teams.Add(NPC.AffliationTypes.red);
            teams.Add(NPC.AffliationTypes.blue);
            teams.Add(NPC.AffliationTypes.green);
            teams.Add(NPC.AffliationTypes.yellow);

            foreach (NPC.AffliationTypes a in teams)
            {
                kills.Add(a, 0);
                deaths.Add(a, 0);
                medicKills.Add(a, 0);
                timeAlive.Add(a, 0);
                maxTimeAlive.Add(a, 0);
                isAlive.Add(a, false);
                casualities.Add(a, 0);
            }

            killsToCommanders = new int [4,4];

            managers = mH;
        }

        public void Add(NPC a)
        {
            if (a is Bomber)
            {
                bombers.Add(a);
                a.LoadContent(managers.GetTextureManager());
                return;
            }

            List<NPC> tempList;

            if (agents.TryGetValue(a.GetAffiliation(), out tempList))
            {
                tempList.Add(a);
            }
            else
            {
                tempList = new List<NPC> {a};
                agents.Add(a.GetAffiliation(), tempList);
            }

            if (a is Commander)
            {
                isAlive[a.GetPersonalAffilation()] = true;
                commanders.Add((Commander) a);
            }

            a.LoadContent(managers.GetTextureManager());
        }

        public void Remove(NPC a)
        {
            UpdateCasualities(a.GetAffiliation());

            if (a is Commander && a.GetLastDamager() != null)
            {
                if(a is Commander)
                    this.UpdateDeaths(a.GetPersonalAffilation());
                if (a.GetLastDamager() is Commander)
                {
                    if (a is Medic)
                        this.UpdateMedicKills(a.GetLastDamager().GetPersonalAffilation());

                    this.UpdateKillsToCommanders(a.GetLastDamager().GetPersonalAffilation(), a.GetAffiliation());
                    this.UpdateKills(a.GetLastDamager().GetPersonalAffilation());
                }
            }

            if (a is Bomber)
            {
                bombers.Remove(a);
            }

            List<NPC> tempList;
            if (agents.TryGetValue(a.GetAffiliation(), out tempList))
            {
                tempList.Remove(a);
            }
            else
            {
                throw new Exception("That team does not exist.");
            }

            if (a is Commander)
            {
                commanders.Remove((Commander) a);
            }
        }

        public void Update()
        {
            NPC a;

            foreach (NPC.AffliationTypes af in teams)
                if (isAlive[af])
                    timeAlive[af] += (float)managers.GetGameTime().ElapsedGameTime.TotalSeconds;

            foreach (var team in agents.Values)
            {
                for (int i = 0; i < team.Count; i++)
                {
                    a = team[i];

                    a.Update(managers);
                    if (!team.Contains(a))
                    {
                        i--;
                    }
                }
            }

            for (int i = 0; i < bombers.Count; i++)
            {
                a = bombers[i];

                a.Update(managers);
                if (!bombers.Contains(a))
                {
                    i--;
                }
            }
        }

        public void DrawLowest(SpriteBatch sB, Vector2 d)
        {
            foreach (var team in agents.Values)
            {
                foreach (NPC r in team)
                {
                    r.Draw(sB, d, managers);
                }
            }
        }

        public void DrawHighest(SpriteBatch sB, Vector2 d)
        {
            foreach (NPC b in bombers)
            {
                b.Draw(sB, d, managers);
            }
        }

        #region Gets

        #region Get Groups

        public List<NPC> GetNPCs()
        {
            var tempList = new List<NPC>();

            foreach (var team in agents.Values)
            {
                tempList.AddRange(team);
            }

            return tempList;
        }

        public List<Commander> GetCommanders()
        {
            return commanders;
        }

        public List<NPC> GetAllies(NPC.AffliationTypes af)
        {
            List<NPC> tempList;
            if (agents.TryGetValue(af, out tempList))
            {
                return new List<NPC>(tempList);
            }
            else
            {
                return new List<NPC>();
            }
        }

        public List<NPC> GetAllies(NPC.AffliationTypes af, NPC t)
        {
            List<NPC> them = GetAllies(af);

            for (int i = 0; i < them.Count; i++)
            {
                if (them[i] == t)
                {
                    them.RemoveAt(i);
                    break;
                }
            }

            return them;
        }

        public List<NPC> GetAlliesInSight(NPC.AffliationTypes af, NPC c)
        {
            List<NPC> tempList,
                      finalList = new List<NPC>();

            foreach (NPC.AffliationTypes t in agents.Keys)
            {
                if (t == af)
                {
                    agents.TryGetValue(t, out tempList);
                    finalList.AddRange(GetAllXInDirection(c.GetOriginPosition(), c.GetSight(), c.GetRotation(), c.GetVision(), tempList));
                }
            }

            return finalList;
        }

        public List<NPC> GetAllButAllies(NPC.AffliationTypes af)
        {
            List<NPC> tempList,
                      finalList = new List<NPC>();

            foreach (NPC.AffliationTypes t in agents.Keys)
            {
                if (t != af)
                {
                    agents.TryGetValue(t, out tempList);
                    finalList.AddRange(tempList);
                }
            }

            return finalList;
        }

        public List<NPC> GetAllButAlliesInSight(NPC.AffliationTypes af, NPC c, ManagerHelper mH)
        {
            List<NPC> tempList,
                      finalList = new List<NPC>();

            foreach (NPC.AffliationTypes t in agents.Keys)
            {
                if (t != af)
                {
                    agents.TryGetValue(t, out tempList);
                    finalList.AddRange(GetAllXInDirection(c.GetOriginPosition(), c.GetSight(), c.GetRotation(), c.GetVision(), tempList));
                }
            }

            return finalList;
        }

        public List<NPC> GetAlliesButBombAndSnipers(NPC.AffliationTypes a)
        {
            var allies = new List<NPC>();

            foreach (NPC n in GetAllies(a))
            {
                if (!(n is Bombardier) && !(n is Sniper))
                    allies.Add(n);
            }

            return allies;
        }

        #endregion

        #region Get In Radius

        public List<NPC> GetAllButAlliesInRadius(NPC.AffliationTypes af, Vector2 p, float aw)
        {
            var finalList = new List<NPC>();

            foreach (NPC.AffliationTypes t in agents.Keys)
            {
                if (t != af)
                {
                    finalList.AddRange(GetAlliesInRadius(t, p, aw));
                }
            }

            return finalList;
        }

        public List<NPC> GetAlliesInRadius(NPC.AffliationTypes af, Vector2 p, float aw)
        {
            List<NPC> tempList;

            if (agents.TryGetValue(af, out tempList))
            {
                return GetAllXInRadius(p, aw, tempList);
            }
            else
            {
                foreach (NPC.AffliationTypes a in managers.GetGametype().GetTeams())
                {
                    if (a == af)
                        return new List<NPC>();
                }
                throw new Exception("That teams does not exist.");
            }
        }

        public List<NPC> GetAllXInRadius(Vector2 p, float r, List<NPC> nL)
        {
            var close = new List<NPC>();

            foreach (NPC a in nL)
            {
                float distance = PathHelper.Distance(a.GetOriginPosition(), p);

                if (distance <= r)
                {
                    close.Add(a);
                }
            }

            return close;
        }

        #endregion

        #region Get In Direction

        public List<NPC> GetAllButAlliesInDirection(NPC.AffliationTypes af, Vector2 p, float r, float d, float aL)
        {
            var finalList = new List<NPC>();

            foreach (NPC.AffliationTypes t in agents.Keys)
            {
                if (t != af)
                {
                    finalList.AddRange(GetAlliesInDirection(t, p, r, d, aL));
                }
            }

            return finalList;
        }

        public List<NPC> GetAlliesInDirection(NPC.AffliationTypes af, Vector2 p, float r, float d, float aL)
        {
            List<NPC> tempList;

            if (agents.TryGetValue(af, out tempList))
            {
                return GetAllXInDirection(p, r, d, aL, tempList);
            }
            else
            {
                return new List<NPC>();
            }
        }

        public List<NPC> GetAllXInDirection(Vector2 p, float r, float d, float aL, List<NPC> nL)
        {
            var dirAgents = new List<NPC>();

            foreach (NPC a in nL)
            {
                if (!managers.GetPathHelper().IsVectorObstructed(p, a.GetOriginPosition()))
                {
                    float dir = PathHelper.Direction(p, a.GetOriginPosition());

                    if (Math.Abs(dir - d) < aL)
                    {
                        dirAgents.Add(a);
                    }
                }
            }

            return dirAgents;
        }

        #endregion

        #region Commanders

        public NPC GetCommander(Type c)
        {
            foreach (Commander a in commanders)
            {
                if (c == a.GetType())
                {
                    return a;
                }
            }

            return null;
        }

        public NPC GetCommander(NPC.AffliationTypes a)
        {
            foreach (Commander n in commanders)
            {
                if (n.GetAffiliation() == a)
                {
                    return n;
                }
            }

            return null;
        }

        public NPC GetSecondaryCommander(NPC.AffliationTypes teamAffilation)
        {
            foreach (NPC commander in commanders)
            {
                if (commander.GetAffiliation() == teamAffilation && commander.GetPersonalAffilation() != teamAffilation)
                    return commander;
            }

            return null;
        }
        #endregion

        public NPC GetSpecificCommander(NPC.AffliationTypes personalAffilation)
        {
            foreach (Commander n in commanders)
            {
                if (n.GetPersonalAffilation() == personalAffilation)
                    return n;
            }

            return null;
        }

        public NPC GetClosestInList(List<NPC> aL, Vector2 p)
        {
            if (aL.Count > 0)
            {
                NPC closest = aL.First();
                var minDist =
                    (float)
                    (Math.Sqrt(Math.Pow(p.X - closest.GetOriginPosition().X, 2) +
                               Math.Pow(p.Y - closest.GetOriginPosition().Y, 2)));

                foreach (NPC a in aL)
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

                return closest;
            }

            return null;
        }

        public NPC GetClosestInList(List<NPC> aL, NPC n)
        {
            Vector2 p = n.GetOriginPosition();

            if (aL.Count > 0)
            {
                NPC closest = aL.First();
                var minDist =
                    (float)
                    (Math.Sqrt(Math.Pow(p.X - closest.GetOriginPosition().X, 2) +
                               Math.Pow(p.Y - closest.GetOriginPosition().Y, 2)));

                foreach (NPC a in aL)
                {
                    if (a != n)
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

        public NPC GetRandInList(List<NPC> aL, NPC n)
        {
            Vector2 p = n.GetOriginPosition();

            if (aL.Count > 0)
            {
                NPC closest = aL.First();
                var minDist =
                    (float)
                    (Math.Sqrt(Math.Pow(p.X - closest.GetOriginPosition().X, 2) +
                               Math.Pow(p.Y - closest.GetOriginPosition().Y, 2)));

                foreach (NPC a in aL)
                {
                    if (a != n && managers.GetRandom().Next(10) > 5)
                    {
                        closest = a;
                    }
                }

                return closest;
            }
            return null;
        }


        public int GetNumBombers(NPC.AffliationTypes a)
        {
            int numBombers = 0;

            foreach (Bomber b in bombers)
            {
                if (b.GetAffiliation() == a)
                    numBombers++;
            }

            return numBombers;
        }

        public bool DoesNPCExist(NPC a)
        {
            foreach (var team in agents.Values)
            {
                foreach (NPC n in team)
                {
                    if (a.Equals(n))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public NPC GetSafeAlly(NPC.AffliationTypes af, NPC c)
        {
            List<NPC> allies = GetAlliesButBombAndSnipers(af);
            var safeAllies = new List<NPC>();

            foreach (NPC a in allies)
            {
                if (GetAllButAlliesInRadius(af, a.GetOriginPosition(), 224).Count == 0)
                    safeAllies.Add(a);
            }

            return GetClosestInList(safeAllies, c.GetOriginPosition());
        }

        public Dictionary<NPC.AffliationTypes, int> GetKills()
        {
            return kills;
        }

        public Dictionary<NPC.AffliationTypes, int> GetMedicKills()
        {
            return medicKills;
        }

        public Dictionary<NPC.AffliationTypes, int> GetDeaths()
        {
            return deaths;
        }
        
        public void ResetKills(ManagerHelper mH)
        {
            foreach (NPC.AffliationTypes a in mH.GetGametype().GetTeams())
            {
                kills[a] = 0;
            }
	}

        public Dictionary<NPC.AffliationTypes, float> GetMaxTimeAlive()
        {
            return maxTimeAlive;
        }

        public int[,] GetKillsToCommanders()
        {
            return killsToCommanders;
        }

        public Dictionary<NPC.AffliationTypes, int> GetCasualities()
        {
            return casualities;
        }
        #endregion

        public void UpdateMedicKills(NPC.AffliationTypes a)
        {
            medicKills[a]++;
        }

        public void UpdateKills(NPC.AffliationTypes a)
        {
            kills[a]++;
        }

        public void UpdateDeaths(NPC.AffliationTypes a)
        {
            if (timeAlive[a] > maxTimeAlive[a])
                maxTimeAlive[a] = timeAlive[a];
            timeAlive[a] = 0;
            isAlive[a] = false;

            deaths[a]++;
        }
        
        public void ResetDeaths(ManagerHelper mH)
        {
            foreach (NPC.AffliationTypes a in mH.GetGametype().GetTeams())
            {
                deaths[a] = 0;
            }
        }

        private void UpdateKillsToCommanders(NPC.AffliationTypes killer, NPC.AffliationTypes killed)
        {
            switch (killer)
            {
                case NPC.AffliationTypes.red:
                {
                    switch (killed)
                    {
                        case NPC.AffliationTypes.blue:
                            killsToCommanders[0, 1]++;
                            break;
                        case NPC.AffliationTypes.green:
                            killsToCommanders[0, 2]++;
                            break;
                        case NPC.AffliationTypes.yellow:
                            killsToCommanders[0, 3]++;
                            break;
                    }
                    break;
                }
                case NPC.AffliationTypes.blue:
                {
                    switch (killed)
                    {
                        case NPC.AffliationTypes.red:
                            killsToCommanders[1, 0]++;
                            break;
                        case NPC.AffliationTypes.green:
                            killsToCommanders[1, 2]++;
                            break;
                        case NPC.AffliationTypes.yellow:
                            killsToCommanders[1, 3]++;
                            break;
                    }
                    break;
                }
                case NPC.AffliationTypes.green:
                {
                    switch (killed)
                    {
                        case NPC.AffliationTypes.blue:
                            killsToCommanders[2, 1]++;
                            break;
                        case NPC.AffliationTypes.red:
                            killsToCommanders[2, 0]++;
                            break;
                        case NPC.AffliationTypes.yellow:
                            killsToCommanders[2, 3]++;
                            break;
                    }
                    break;
                }
                case NPC.AffliationTypes.yellow:
                {
                    switch (killed)
                    {
                        case NPC.AffliationTypes.blue:
                            killsToCommanders[3, 1]++;
                            break;
                        case NPC.AffliationTypes.green:
                            killsToCommanders[3, 2]++;
                            break;
                        case NPC.AffliationTypes.red:
                            killsToCommanders[3, 0]++;
                            break;
                    }
                    break;
                }

            }
        }
 
        private void UpdateCasualities(NPC.AffliationTypes killedAffilation)
        {
            if(killedAffilation != NPC.AffliationTypes.black)
            casualities[killedAffilation]++;
        }
    }
}