#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class SwitchBeltNetwork : InDestructable
    {
        #region Declarations

        private readonly double endTime;
        private readonly Crane theCrane;
        public Sprite[] belts;
        public Node[] boxNodes;
        public Vector2[] directions;

        private double timer;

        #endregion

        public SwitchBeltNetwork(Crane c)
            : base("", Vector2.Zero)
        {
            belts = new Sprite[8];
            directions = new Vector2[8];
            boxNodes = new Node[14];
            theCrane = c;

            belts[0] = new Sprite("Backgrounds/Switch/BL", new Vector2(364, 288));
            belts[1] = new Sprite("Backgrounds/Switch/BLr", new Vector2(512.5f, 435));
            belts[1].Turn(MathHelper.Pi);
            belts[2] = new Sprite("Backgrounds/Switch/BRr", new Vector2(416, 221));
            belts[3] = new Sprite("Backgrounds/Switch/BR", new Vector2(579, 384));
            belts[3].Turn(MathHelper.Pi);
            belts[4] = new Sprite("Backgrounds/Switch/BT", new Vector2(487, 192.5f));
            belts[5] = new Sprite("Backgrounds/Switch/BTr", new Vector2(607, 313));
            belts[5].Turn(MathHelper.Pi);
            belts[6] = new Sprite("Backgrounds/Switch/DBr", new Vector2(582, 217));
            belts[6].Turn(MathHelper.Pi);
            belts[7] = new Sprite("Backgrounds/Switch/DBr", new Vector2(441, 359));

            directions[0] = new Vector2(-1f, 0f);
            directions[1] = new Vector2(0f, -1f);
            directions[2] = new Vector2(0f, 1f);
            directions[3] = new Vector2(1f, 0f);
            directions[4] = new Vector2(-1f, 0f);
            directions[5] = new Vector2(0f, -1f);
            directions[6] = new Vector2(-(float) Math.Sqrt(2)/2, -(float) Math.Sqrt(2)/2);
            directions[7] = new Vector2((float) Math.Sqrt(2)/2, (float) Math.Sqrt(2)/2);

            #region Set up nodes

            var tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(0, 1));
            boxNodes[0] = new Node(new Vector2(416, 112), tempDirs);

            boxNodes[1] = new Node(new Vector2(416, 192), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(-1, 0));
            boxNodes[2] = new Node(new Vector2(544, 192), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(-(float) Math.Sqrt(2)/2, -(float) Math.Sqrt(2)/2));
            boxNodes[3] = new Node(new Vector2(608, 256), tempDirs);

            tempDirs = new List<Vector2>();
            boxNodes[4] = new Node(new Vector2(256, 288), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(-1, 0));
            tempDirs.Add(new Vector2(0, 1));
            boxNodes[5] = new Node(new Vector2(416, 288), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2((float) Math.Sqrt(2)/2, (float) Math.Sqrt(2)/2));
            boxNodes[6] = new Node(new Vector2(416, 320), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(1, 0));
            boxNodes[7] = new Node(new Vector2(480, 384), tempDirs);

            boxNodes[8] = new Node(new Vector2(512, 384), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(0, -1));
            tempDirs.Add(new Vector2(1, 0));
            boxNodes[9] = new Node(new Vector2(608, 384), tempDirs);

            tempDirs = new List<Vector2>();
            boxNodes[10] = new Node(new Vector2(704, 384), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(0, -1));
            boxNodes[11] = new Node(new Vector2(512, 544), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(0, -1));
            boxNodes[12] = new Node(new Vector2(620, 284), tempDirs);

            tempDirs = new List<Vector2>();
            tempDirs.Add(new Vector2(-1, 0));
            boxNodes[13] = new Node(new Vector2(512, 180), tempDirs);

            #endregion

            timer = 0.0;
            endTime = .05;
        }

        public override void LoadContent(TextureManager tM)
        {
            foreach (Sprite b in belts)
            {
                b.LoadContent(tM);
            }
        }

        public override void Update(ManagerHelper mH)
        {
            if (timer > endTime)
            {
                timer = 0.0;

                for (int i = 0; i < belts.Length; i++)
                {
                    belts[i].SetFrameIndex(belts[i].GetFrameIndex() + 1);
                    belts[i].SetModeIndex(belts[i].GetModeIndex() + 1);

                    if (belts[i].GetFrameIndex() == belts[i].totalFrames)
                    {
                        belts[i].SetFrameIndex(0);
                    }

                    if (belts[i].GetModeIndex() == belts[i].totalModes)
                    {
                        belts[i].SetModeIndex(0);
                    }

                    foreach (NPC a in mH.GetNPCManager().GetNPCs())
                    {
                        int tempCollide = CollisionHelper.IntersectPixelsDirectional(a, belts[i]);

                        if (tempCollide != -1)
                        {
                            a.AddAcceleration(directions[i]*2);
                        }
                    }

                    belts[i].Update(mH);
                }

                if (mH.GetRandom().NextDouble() > 0.993)
                {
                    mH.GetEnvironmentManager()
                      .AddStaticBlocker(new SwitchBox(boxNodes[(mH.GetRandom().Next(2) == 0) ? 0 : 11].pos, this,
                                                      theCrane));
                }
            }
            else
            {
                timer += (mH.GetGameTime().ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            foreach (Sprite b in belts)
            {
                b.Draw(sB, displacement, mH);
            }
        }

        public class Node
        {
            public List<Vector2> dirs;
            public Vector2 pos;

            public Node(Vector2 p, List<Vector2> ds)
            {
                pos = p;
                dirs = ds;
            }

            public Vector2 GetRandomDir(ManagerHelper mH)
            {
                if (dirs.Count == 0)
                {
                    return Vector2.Zero;
                }

                return dirs[mH.GetRandom().Next(dirs.Count)];
            }
        }
    }
}