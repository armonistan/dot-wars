#region

using Microsoft.Xna.Framework;

#endregion

namespace DotWars
{
    public class Mine : Sprite
    {
        private readonly double existanceTime;
        private double existanceTimer;

        private readonly double armedTime;

        private readonly double pulseTime;
        private readonly double pulseBeginTime;
        private double pulseTimer;

        private bool draw;

        private bool armed;

        private NPC creator;

        private readonly float explodeRadius;
        private readonly int damage;

        private bool isSurvial;

        public Mine()
            : base("Projectiles/mine", Vector2.Zero)
        {
            existanceTime = 30.0;

            armedTime = 3.0;

            pulseTime = 2.0;
            pulseBeginTime = 1.5;

            explodeRadius = 32f;
            damage = 50;
        }

        public void Set(NPC c, Vector2 pos, ManagerHelper mH)
        {
            existanceTimer = 0.0;

            draw = true;

            armed = false;
            modeIndex = 0;

            creator = c;

            position = pos - origin;

            isSurvial = mH.GetGametype() is Survival;
        }

        public override void Update(ManagerHelper mH)
        {
            bool shouldExplode = false;

            foreach (Explosion explosion in mH.GetParticleManager().GetExplosions())
            {
                if (explosion.GetAffiliation() != creator.GetAffiliation() &&
                    CollisionHelper.IntersectPixelsRadius(this, explosion, origin.X, explosion.GetRadius()) !=
                    new Vector2(-1))
                {
                    shouldExplode = true;
                    break;
                }
            }

            if (shouldExplode)
            {
                draw = false;
                mH.GetParticleManager().AddExplosion(GetOriginPosition(), creator, damage);
            }

            if (armed)
            {
                if (existanceTimer > existanceTime)
                {
                    draw = false;
                }
                else
                {
                    if (isSurvial)
                        shouldSurvialExplode(mH);
                    else
                        shouldRegularExplode(mH);
                }
            }
            else
            {
                if (existanceTimer > armedTime)
                {
                    armed = true;

                    modeIndex = NPC.GetTeam(creator.GetAffiliation()) + 1;
                }
            }

            if (pulseTimer > pulseBeginTime)
            {
                frameIndex = (int) (totalFrames*(pulseTimer - pulseBeginTime)/(pulseTime - pulseBeginTime));
            }
            else
            {
                frameIndex = 0;
            }

            if (pulseTimer > pulseTime)
            {
                pulseTimer = 0;
            }

            pulseTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            existanceTimer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;

            base.Update(mH);
        }

        private void shouldRegularExplode(ManagerHelper mH)
        {
            foreach (NPC agent in mH.GetNPCManager().GetNPCs())
            {
                if (agent.GetAffiliation() != creator.GetAffiliation() &&
                    NPCManager.IsNPCInRadius(agent, GetOriginPosition(), explodeRadius))
                {
                    draw = false;
                    mH.GetParticleManager().AddExplosion(GetOriginPosition(), creator, damage);
                }
            }
        }

        private void shouldSurvialExplode(ManagerHelper mH)
        {
            foreach (NPC agent in mH.GetNPCManager().GetNPCs())
            {
                if (agent.GetAffiliation() == NPC.AffliationTypes.black &&
                    NPCManager.IsNPCInRadius(agent, GetOriginPosition(), explodeRadius))
                {
                    draw = false;
                    mH.GetParticleManager().AddExplosion(GetOriginPosition(), creator, damage);
                }
            }
        }

        public bool IsDrawing()
        {
            return draw;
        }

        public bool IsArmed()
        {
            return armed;
        }

        public NPC GetCreator()
        {
            return creator;
        }
    }
}