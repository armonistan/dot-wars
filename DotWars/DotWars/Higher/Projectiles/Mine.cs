using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Mine : Sprite
    {
        private double existanceTime;
        private double existanceTimer;

        private double armedTime;

        private double pulseTime;
        private double pulseBeginTime;
        private double pulseTimer;

        private bool draw;

        private bool armed;

        private NPC creator;

        private float explodeRadius;
        private int damage;

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

        public void Set(NPC c, Vector2 pos)
        {
            existanceTimer = 0.0;

            draw = true;

            armed = false;
            modeIndex = 0;

            creator = c;

            position = pos - origin;
        }

        public override void Update(ManagerHelper mH)
        {
            bool shouldExplode = false;

            foreach (Explosion explosion in mH.GetParticleManager().GetExplosions())
            {
                if (CollisionHelper.IntersectPixelsRadius(this, explosion, origin.X, explosion.GetRadius()) !=
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
                frameIndex = (int)(totalFrames*(pulseTimer - pulseBeginTime)/(pulseTime - pulseBeginTime));
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

        public bool IsDrawing()
        {
            return draw;
        }

        public bool IsArmed()
        {
            return armed;
        }
    }
}
