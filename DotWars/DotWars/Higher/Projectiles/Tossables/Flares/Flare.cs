using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DotWars
{
    public class Flare : Tossable
    {
        protected int flareRadius;//flare radius
        protected int flareAttractRadius;//summoning radius
        List<NPC> capturedDots; //a list used to see what dots have been attracted once

        public Flare()
            : base()
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
            ////Create a smoke particle every now and then
            //if (deathCount != 0 && mH.GetRandom().Next(100) < (50 * (deathCount / 6)))
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

        public virtual void SpawnSmoke(ManagerHelper mH)
        {
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

            switch (n.affiliation)
            {
                case NPC.AffliationTypes.red:
                    color += "red";
                    break;
                case NPC.AffliationTypes.blue:
                    color += "blue";
                    break;
                case NPC.AffliationTypes.green:
                    color += "green";
                    break;
                case NPC.AffliationTypes.yellow:
                    color += "yellow";
                    break;
            }

            base.Set(color, n.GetOriginPosition(), n, v, 0, false, 3, mH);
        }
    }
}
