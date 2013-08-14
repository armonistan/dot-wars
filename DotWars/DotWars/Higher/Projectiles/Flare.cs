using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DotWars
{
    public class Flare : Tossable
    {
        private readonly List<NPC> capturedDots; //a list used to see what dots have been attracted once
        protected int flareAttractRadius; //summoning radius
        protected int flareRadius; //flare radius
        private string smokeAsset;

        public Flare()
        {
            capturedDots = new List<NPC>();
            damage = 0;

            drag = 0.05f;

            flareRadius = 32;
            flareAttractRadius = 500;
        }

        public override void Update(ManagerHelper mH)
        {
            //check to see if the flare is moving
            //if (velocity.Length() < 40)
            //{
            //    if (deathCount > 0)
            //    {
            //        flareRotationSpeed = 0;
            //        deathCount -= (float)mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            //    }
            //    else
            //    {
            //        DeathCode(mH);
            //    }
            //}
            //
            //Create a smoke particle every now and then
            //if (existenceTime != 0 && mH.GetRandom().Next(100) < (50 * (existenceTime / 6)))
            //{
            //    SpawnSmoke(mH);
            //}

            base.Update(mH);
        }

        public bool InAttractionRadius(Vector2 p, ManagerHelper mH)
        {
            return (PathHelper.Distance(GetOriginPosition(), p) < flareAttractRadius);
        }

        public int GetFlareRadius()
        {
            return flareRadius;
        }

        protected override void EffectSpawnCode(ManagerHelper mH)
        {
            if (existenceTime != 0 && mH.GetRandom().Next(100) < (20*(existenceTime)))
            {
                mH.GetParticleManager()
                  .AddParticle(smokeAsset, GetOriginPosition(),
                               PathHelper.Direction((float) (mH.GetRandom().NextDouble()*Math.PI*2))*15,
                               Particle.MAX_EXIST_TIME, 0.01f, 1, 0.5f);
            }
        }

        public void AddToList(NPC n)
        {
            capturedDots.Add(n);
        }

        public bool IsInList(NPC n)
        {
            return capturedDots.IndexOf(n) != -1;
        }

        public void Set(NPC n, Vector2 v, ManagerHelper mH)
        {
            String color = "Projectiles/flare_";
            smokeAsset = "Effects/smoke_";

            switch (n.GetAffiliation())
            {
                case NPC.AffliationTypes.red:
                    color += "red";
                    smokeAsset += "red";
                    break;
                case NPC.AffliationTypes.blue:
                    color += "blue";
                    smokeAsset += "blue";
                    break;
                case NPC.AffliationTypes.green:
                    color += "green";
                    smokeAsset += "green";
                    break;
                case NPC.AffliationTypes.yellow:
                    color += "yellow";
                    smokeAsset += "yellow";
                    break;
            }

            base.Set(color, n.GetOriginPosition(), n, v, 0, false, 3, mH);
        }
    }
}