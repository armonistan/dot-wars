using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    public class CameraManager
    {
        #region Declarations

        private readonly List<Camera> cameras;
        private readonly int height;
        private readonly List<HUD> huds;
        private readonly int[] rumbleAmount;
        private readonly int width;
        private Viewport fullSize;
        private Rectangle safeArea;
        private Sprite SplitScreen;
        private Sprite pauseScreen;
        private Type pauser;

        #endregion

        public CameraManager(Dictionary<Type, int> pL, Gametype gT, Vector2 sS)
        {
            fullSize = new Viewport(new Rectangle(0, 0, (int) sS.X, (int) sS.Y));
            safeArea = fullSize.TitleSafeArea;
            rumbleAmount = new int[4];
            for (int i = 0; i < rumbleAmount.Length; i++)
            {
                rumbleAmount[i] = 0;
            }

            //Get sizes
            if (pL.Count == 1)
            {
                width = (int) sS.X;
                height = (int) sS.Y;
            }
            else if (pL.Count == 2)
            {
                width = (int) sS.X;
                height = (int) sS.Y/2;

                SplitScreen = new Sprite("HUD/2playerSplit", sS / 2);
            }
            else
            {
                width = (int) sS.X/2;
                height = (int) sS.Y/2;

                SplitScreen = new Sprite("HUD/4playerSplit", sS / 2);
            }

            cameras = new List<Camera>();
            huds = new List<HUD>();
            for (int i = 0; i < pL.Count; i++)
            {
                cameras.Add(new Camera(pL.Keys.ElementAt(i), new Rectangle((i/2)*width, (i%2)*height, width, height),
                                       pL.Values.ElementAt(i), gT.GetTeammaterColor(pL.Keys.ElementAt(i))));
                huds.Add(new HUD(cameras[i], safeArea, i));
            }

            pauseScreen = new Sprite("HUD/pauseOverlay", sS / 2);
        }

        public void LoadContent(ManagerHelper mH)
        {
            foreach (Camera c in cameras)
            {
                c.dimensions.X = (int) mH.GetLevelSize().X/2;
                c.dimensions.Y = (int) mH.GetLevelSize().Y/2;
            }

            foreach (HUD h in huds)
            {
                h.LoadContent(mH);
            }

            if (SplitScreen != null)
            {
                SplitScreen.LoadContent(mH.GetTextureManager());
            }

            pauseScreen.LoadContent(mH.GetTextureManager());
        }

        public void Update(ManagerHelper mH)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (pauser == null)
                {
                    var commander = (Commander) mH.GetNPCManager().GetCommander(cameras[i].commanderType);
                    if (commander != null)
                    {
                        Vector2 newPlace = Vector2.Lerp(new Vector2(cameras[i].dimensions.X, cameras[i].dimensions.Y),
                                                        commander.GetOriginPosition() +
                                                        ((rumbleAmount[i] != 0)
                                                             ? new Vector2(
                                                                   ((float) mH.GetRandom().NextDouble()*rumbleAmount[i]) -
                                                                   (rumbleAmount[i]/2),
                                                                   ((float) mH.GetRandom().NextDouble()*rumbleAmount[i]) -
                                                                   (rumbleAmount[i]/2))
                                                             : Vector2.Zero), 0.05f);

                        cameras[i].dimensions.X = (int)newPlace.X;
                        cameras[i].dimensions.Y = (int)newPlace.Y;

                        huds[i].Update(mH, commander);
                    }
                }

                if (cameras[i].GetState().IsButtonDown(Buttons.Start) && !cameras[i].GetOldState().IsButtonDown(Buttons.Start) ||
                    cameras[i].GetState().IsButtonDown(Buttons.BigButton) && !cameras[i].GetState().IsButtonDown(Buttons.BigButton) && pauser == null)
                {
                    SetPause(cameras[i].GetCommanderType(), mH);
                }
            }

            //Reset rumble amounts
            ResetAllRumble();
        }

        public void UpdateStateBefore()
        {
            foreach (Camera camera in cameras)
            {
                camera.UpdateBefore();
            }
        }

        public void UpdateStateAfter()
        {
            foreach (Camera camera in cameras)
            {
                camera.UpdateAfter();
            }
        }

        public Vector2 GetDisplacement(Type cT)
        {
            foreach (Camera c in cameras)
            {
                if (c.commanderType == cT)
                {
                    return new Vector2(c.dimensions.X, c.dimensions.Y) - c.focus/c.GetZoom();
                }
            }

            return Vector2.Zero;
        }

        public List<Camera> GetCameras()
        {
            return cameras;
        }

        public HUD GetHud(Camera c)
        {
            foreach (HUD h in huds)
            {
                if (h.GetCamera() == c)
                    return h;
            }

            return null;
        }

        public void DrawSplit(SpriteBatch sB, ManagerHelper mH)
        {
            if (SplitScreen != null)
            {
                SplitScreen.Draw(sB, Vector2.Zero, mH);
            }
        }

        public Viewport GetFullSize()
        {
            return fullSize;
        }

        public void DrawPause(SpriteBatch sB, ManagerHelper mH)
        {
            if (pauser != null)
            {
                pauseScreen.Draw(sB, Vector2.Zero, mH);
            }
        }

        public void SetPause(Type c, ManagerHelper mH)
        {
            if (pauser == null)
            {
                pauser = c;

                pauseScreen.SetModeIndex(GetCommanderInt(c));
                pauseScreen.Update(mH);
            }
            else if (c == pauser)
            {
                pauser = null;
            }
        }

        public Type GetPauser()
        {
            return pauser;
        }

        public static Vector2 Transform(Vector2 p, Vector2 o)
        {
            return p - o;
        }

        public PlayerIndex GetPlayerIndex(Type cT)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].commanderType == cT)
                {
                    return cameras[i].gamePadIndex;
                }
            }

            return PlayerIndex.One;
        }

        public int GetPlayerInt(Type cT)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].commanderType == cT)
                {
                    return i;
                }
            }

            return 0;
        }

        public int GetPlayerIndex(Commander a)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].commanderType == a.GetType())
                {
                    return i;
                }
            }

            return 0;
        }

        public int GetCommanderInt(Type cT)
        {
            if (cT == typeof (RedPlayerCommander))
            {
                return 0;
            }
            else if (cT == typeof(BluePlayerCommander))
            {
                return 1;
            }
            else if (cT == typeof(GreenPlayerCommander))
            {
                return 2;
            }
            else if (cT == typeof(YellowPlayerCommander))
            {
                return 3;
            }

            return 0;
        }

        public void SetAllRumble(int a)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                SetRumble(i, a);
            }
        }

        private void ResetAllRumble()
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                rumbleAmount[i] = 0;
            }
        }

        public void SetRumble(int i, int a)
        {
            rumbleAmount[i] += a;
        }

        public Rectangle GetSafeArea()
        {
            return safeArea;
        }

        public class Camera
        {
            #region Declarations

            public Type commanderType;
            private NPC.AffliationTypes teammateColor;
            public Rectangle dimensions;
            public Vector2 focus;
            public PlayerIndex gamePadIndex;
            public int index;
            public Viewport port;
            private float zoom;
            private GamePadState theState;
            private GamePadState oldState;

            #endregion

            public Camera(Type cT, Rectangle d, int i, NPC.AffliationTypes color)
            {
                commanderType = cT;
                teammateColor = color;
                focus = new Vector2(d.Width/2, d.Height/2);
                dimensions = d;
                port = new Viewport(d);
                zoom = 1;
                index = i;

                switch (i)
                {
                    case 0:
                        gamePadIndex = PlayerIndex.One;
                        break;
                    case 1:
                        gamePadIndex = PlayerIndex.Two;
                        break;
                    case 2:
                        gamePadIndex = PlayerIndex.Three;
                        break;
                    case 3:
                        gamePadIndex = PlayerIndex.Four;
                        break;
                }

                oldState = GamePad.GetState(PlayerIndex.One);
            }

            public void UpdateBefore()
            {
                theState = GamePad.GetState(gamePadIndex);
            }

            public void UpdateAfter()
            {
                oldState = theState;
            }

            public GamePadState GetState()
            {
                return theState;
            }

            public GamePadState GetOldState()
            {
                return oldState;
            }

            public Vector2 GetFocus()
            {
                return new Vector2(port.X, port.Y) - focus;
            }

            public Viewport GetPort()
            {
                return port;
            }

            public void IncrementZoom(float z)
            {
                zoom += z;
            }

            public void SetZoom(float z)
            {
                zoom = z;
            }

            public float GetZoom()
            {
                return zoom;
            }

            public Type GetCommanderType()
            {
                return commanderType;
            }

            public NPC.AffliationTypes GetTeammateColor()
            {
                return teammateColor;
            }
        }

        public class HUD
        {
            #region Declarations

            private readonly Sprite back;
            private readonly Sprite front;
            private readonly Sprite gun;
            private readonly Sprite toss;
            private readonly Sprite teammate;
            private readonly Sprite scoreboard;
            private readonly Sprite timer;

            private readonly StatBar damage;
            private readonly StatBar health;
            private readonly StatBar power;

            private readonly Camera myCamera;

            private int healthWidth,
                        powerWidth;

            private float damageWidth,
                healthCalcWidth;

            private Vector2 leftPos, rightPos;
            #endregion

            public HUD(Camera c, Rectangle sA, int i)
            {
                myCamera = c;

                Vector2 safeAreaStart = new Vector2((i / 2 == 0) ? c.GetPort().TitleSafeArea.Left : 0, (i % 2 == 0) ? c.GetPort().TitleSafeArea.Top : 0);
                Vector2 safeAreaEnd = new Vector2(((i / 2 == 0) ? c.GetPort().TitleSafeArea.Left : 0) + c.GetPort().TitleSafeArea.Width, (i % 2 == 0) ? c.GetPort().TitleSafeArea.Top : 0);

                leftPos = safeAreaStart + new Vector2(150, 50);
                rightPos = safeAreaEnd + new Vector2(-24, 50);

                if (c.commanderType == typeof (RedCommander) || c.commanderType == typeof (RedPlayerCommander))
                {
                    front = new Sprite("HUD/hud_red", leftPos);
                }
                else if (c.commanderType == typeof (BlueCommander) || c.commanderType == typeof (BluePlayerCommander))
                {
                    front = new Sprite("HUD/hud_blue", leftPos);
                }
                else if (c.commanderType == typeof (GreenCommander) || c.commanderType == typeof (GreenPlayerCommander))
                {
                    front = new Sprite("HUD/hud_green", leftPos);
                }
                else if (c.commanderType == typeof (YellowCommander) || c.commanderType == typeof (YellowPlayerCommander))
                {
                    front = new Sprite("HUD/hud_yellow", leftPos);
                }

                //Set up other sprites
                back = new Sprite("HUD/hud_background", leftPos);
                damage = new StatBar("HUD/bar_damage", leftPos + new Vector2(39, -13));
                health = new StatBar("HUD/bar_health", leftPos + new Vector2(39, -13));
                power = new StatBar("HUD/bar_special", leftPos + new Vector2(18, 11));
                gun = new Sprite("HUD/hud_gun", leftPos + new Vector2(-82, 0));
                toss = new Sprite("HUD/hud_toss", leftPos + new Vector2(-47, -8));
                teammate = new Sprite("HUD/hud_teammates", leftPos + new Vector2(86, 11));
                scoreboard = new Sprite("HUD/hud_score_frame", rightPos - new Vector2(180, -10));
                timer = new Sprite("HUD/hud_time_frame", rightPos - new Vector2(440, -10));
            }

            public void LoadContent(ManagerHelper mH)
            {
                front.LoadContent(mH.GetTextureManager());
                back.LoadContent(mH.GetTextureManager());
                damage.LoadContent(mH.GetTextureManager());
                health.LoadContent(mH.GetTextureManager());
                power.LoadContent(mH.GetTextureManager());
                gun.LoadContent(mH.GetTextureManager());
                toss.LoadContent(mH.GetTextureManager());

                teammate.LoadContent(mH.GetTextureManager());
                teammate.SetFrameIndex(NPC.GetTeam(myCamera.GetTeammateColor()));
                teammate.UpdateFrame();

                scoreboard.LoadContent(mH.GetTextureManager());
                scoreboard.SetModeIndex(Math.Min(3, mH.GetGametype().GetTeams().Count - 1));
                scoreboard.UpdateFrame();
                timer.LoadContent(mH.GetTextureManager());

                damageWidth = health.GetFrame().Width;
                healthWidth = health.GetFrame().Width;
                powerWidth = power.GetFrame().Width;
            }

            public void Update(ManagerHelper mH, Commander c)
            {
                if (c != null)
                {
                    healthCalcWidth = c.GetHealth()/(float)c.GetMaxHealth()*healthWidth;
                    damageWidth = ((damageWidth < healthCalcWidth) ? healthCalcWidth : damageWidth-0.3f);

                    gun.SetFrameIndex(c.GetModeIndex());
                    toss.SetFrameIndex(c.grenadeType);
                    gun.Update(mH);
                    toss.Update(mH);
                    damage.Update(mH, (int)damageWidth);
                    health.Update(mH, (int)healthCalcWidth);
                    power.Update(mH, (int)((float)c.CurrentPower() / (float)c.MaxPower() * powerWidth));
                    teammate.Update(mH);
                }

                scoreboard.Update(mH);
                timer.Update(mH);
            }

            public void Draw(SpriteBatch sB, ManagerHelper mH)
            {
                //Draw time left
                int timeLeft = (int)(mH.GetGametype().GetGameEndTimer()) + 1,
                    mins = timeLeft / 60,
                    secs = timeLeft % 60;

                timer.Draw(sB, Vector2.Zero, mH);

                mH.GetTextureManager().DrawString(sB, mins + ":" + ((secs < 10) ? "0" : "") + secs, timer.GetOriginPosition(),
                                                  Color.White, TextureManager.FontSizes.small);

                scoreboard.Draw(sB, Vector2.Zero, mH);

                for (int x = 0; x < mH.GetGametype().GetTeams().Count; x++)
                {
                    Color teamColor = Color.White;
                    switch (mH.GetGametype().GetTeams()[x])
                    {
                        case NPC.AffliationTypes.red:
                            teamColor = Color.Red;
                            break;
                        case NPC.AffliationTypes.blue:
                            teamColor = Color.Blue;
                            break;
                        case NPC.AffliationTypes.green:
                            teamColor = Color.Green;
                            break;
                        case NPC.AffliationTypes.yellow:
                            teamColor = Color.Yellow;
                            break;
                    }

                    if (mH.GetGametype().GetTeams()[x] != NPC.AffliationTypes.black)
                    {
                        mH.GetTextureManager().DrawString(sB, mH.GetGametype().GetScores()[x] + "", rightPos - new Vector2((4 - x) * 82 - 24, -10), teamColor, TextureManager.FontSizes.small);
                    }
                }


                NPC commander = mH.GetNPCManager().GetCommander(myCamera.commanderType);
                if (commander != null)
                {
                    back.Draw(sB, Vector2.Zero, null);
                    power.Draw(sB, Vector2.Zero, null);
                    damage.Draw(sB, Vector2.Zero, null);
                    health.Draw(sB, Vector2.Zero, null);
                    front.Draw(sB, Vector2.Zero, null);
                    gun.Draw(sB, Vector2.Zero, null);
                    toss.Draw(sB, Vector2.Zero, null);

                    if (myCamera.GetTeammateColor() != NPC.AffliationTypes.grey)
                    {
                        teammate.Draw(sB, Vector2.Zero, null);
                    }
                }
            }

            public Camera GetCamera()
            {
                return myCamera;
            }
        }

        private class StatBar : Sprite
        {
            public StatBar(String a, Vector2 p)
                : base(a, p)
            {
                
            }

            public void Update(ManagerHelper mH, int widthVal)
            {
                frame.Width = widthVal;

                base.Update(mH);
            }
        }
    }
}