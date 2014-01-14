using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    internal class SwitchElectricity : Impassable
    {
        #region Declarations

        private readonly bool[] beamOn;
        private readonly Impathable[] beams;
        private readonly double endTimer;
        private readonly Vector2[] startPos;
        private bool on;

        private double timer;

        #endregion

        public SwitchElectricity(Vector2 p)
            : base("", p)
        {
            on = false;
            timer = 0;
            endTimer = 5;

            //Set up beamOn
            beamOn = new bool[12];
            for (int i = 0; i < 12; i++)
            {
                beamOn[i] = false;
            }

            //Set up startpos
            startPos = new Vector2[12];
            startPos[0] = new Vector2(80, 64);
            startPos[1] = new Vector2(96, 304);
            startPos[2] = new Vector2(256, 176);
            startPos[3] = new Vector2(192, 384);
            startPos[4] = new Vector2(544, 96);
            startPos[5] = new Vector2(672, 128);
            startPos[6] = new Vector2(704, 256);
            startPos[7] = new Vector2(224, 576);
            startPos[8] = new Vector2(416, 608);
            startPos[9] = new Vector2(624, 544);
            startPos[10] = new Vector2(496, 704);
            startPos[11] = new Vector2(736, 720);

            //Set up sprites
            beams = new Impathable[12];
            beams[0] = new Impathable("Backgrounds/Switch/elect1", startPos[0], Vector2.Zero);
            beams[1] = new Impathable("Backgrounds/Switch/elect2", startPos[1], Vector2.Zero);
            beams[2] = new Impathable("Backgrounds/Switch/elect3", startPos[2], Vector2.Zero);
            beams[3] = new Impathable("Backgrounds/Switch/elect4", startPos[3], Vector2.Zero);
            beams[4] = new Impathable("Backgrounds/Switch/elect5", startPos[4], Vector2.Zero);
            beams[5] = new Impathable("Backgrounds/Switch/elect6", startPos[5], Vector2.Zero);
            beams[6] = new Impathable("Backgrounds/Switch/elect7", startPos[6], Vector2.Zero);
            beams[7] = new Impathable("Backgrounds/Switch/elect8", startPos[7], Vector2.Zero);
            beams[8] = new Impathable("Backgrounds/Switch/elect9", startPos[8], Vector2.Zero);
            beams[9] = new Impathable("Backgrounds/Switch/elect10", startPos[9], Vector2.Zero);
            beams[10] = new Impathable("Backgrounds/Switch/elect11", startPos[10], Vector2.Zero);
            beams[11] = new Impathable("Backgrounds/Switch/elect12", startPos[11], Vector2.Zero);
        }

        public override void LoadContent(TextureManager tM)
        {
            blockers = new List<Vector2>[0,0];

            foreach (Sprite b in beams)
            {
                b.LoadContent(tM);
            }
        }

        public override void Update(ManagerHelper mH)
        {
            if (timer > endTimer)
            {
                timer = 0;
                on = !on;

                if (on)
                {
                    for (int i = 0; i < beamOn.Length; i++)
                    {
                        if (mH.GetRandom().NextDouble() > 0.5f)
                        {
                            beamOn[i] = true;

                            mH.GetEnvironmentManager().GetImpathables().Add(beams[i]);
                        }
                    }
                }
                else
                {
                    //blockers.Clear();

                    for (int i = 0; i < beamOn.Length; i++)
                    {
                        if (beamOn[i])
                        {
                            beamOn[i] = false;
                            mH.GetEnvironmentManager().RemoveImpathable(beams[i]);
                        }
                    }
                }
            }
            else
            {
                timer += mH.GetGameTime().ElapsedGameTime.TotalSeconds;
            }

            if (on)
            {
                for (int i = 0; i < beams.Length; i++)
                {
                    if (beamOn[i])
                    {
                        foreach (NPC a in mH.GetNPCManager().GetNPCs())
                        {
                            if (!(a is Bomber))
                            {
                                if (CollisionHelper.IntersectPixelsDirectional(a, beams[i]) != -1)
                                {
                                    a.ChangeHealth(-3, a.GetLastDamager());
                                }
                            }
                        }
                    }

                    beams[i].SetFrameIndex(mH.GetRandom().Next(beams[i].totalFrames));
                }
            }

            base.Update(mH);
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
        }

        public override List<Vector2> GetFrameBlockers()
        {
            return new List<Vector2>();
        }
    }
}