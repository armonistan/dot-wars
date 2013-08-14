using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class HealthStation : Environment
    {
        private readonly float endTime;
        private readonly int healRadius;
        private int HEAL_NUM = 25;
        private NPC healTarget;
        private float timer;

        public HealthStation(Vector2 p)
            : base("health_station", p, Vector2.Zero)
        {
            timer = 0;
            healRadius = 64;
            endTime = 0.5f;
        }

        public override void Update(ManagerHelper mH)
        {
            //Animation
            if (timer > endTime)
            {
                if (frameIndex >= 6)
                    frameIndex = 0;
                else
                    frameIndex++;

                timer = 0;
            }
            else
            {
                timer += (float) mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            SelectHealTarget(mH);

            if (healTarget != null)
                Heal(mH);

            base.Update(mH);
        }

        private void SelectHealTarget(ManagerHelper mH)
        {
            //Set up a couple of lists
            List<NPC> agents;

            agents = mH.GetNPCManager()
                       .GetAllButAlliesInRadius(NPC.AffliationTypes.grey, GetOriginPosition(), healRadius);

            //Check to find the most wounded unit
            for (int i = 0; i < agents.Count; i++)
            {
                if (healTarget != null)
                {
                    if (healTarget.GetHealth() > agents[i].GetHealth())
                    {
                        healTarget = agents[i];
                    }
                }
                else
                {
                    healTarget = agents[i];
                }
            }
        }

        private void Heal(ManagerHelper mH) //The actual heal method
        {
            //check if in heal range
            if (PathHelper.Distance(GetOriginPosition(), healTarget.GetOriginPosition()) <= healRadius)
                healTarget.ChangeHealth(HEAL_NUM, NPC.AffliationTypes.grey); //heal the targeted dot
        }
    }
}