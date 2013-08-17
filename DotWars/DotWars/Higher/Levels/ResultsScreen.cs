using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    public class ResultsScreen : Level
    {
        private readonly Level result;
        private String leftHandStats = "";
        private String centerStats = "";
        private String rightHandStats = "";
        private String winnerString = "fuck you";
        private String winsString = "Wins!";
        private Sprite winnerSpriteLeft;
        private Sprite winnerSpriteRight;
        private int winnerFrameLeft = -1;
        private int winnerFrameRight = -1;

        private const int RED_INDEX = 0;
        private const int BLUE_INDEX = 1;
        private const int GREEN_INDEX = 2;
        private const int YELLOW_INDEX = 3;

        public ResultsScreen(Level l, TextureManager tM, AudioManager audio)
            : base(
                new ResultScreenGametype(), new Dictionary<Type, int>(),
                new Vector2(1248, 720), tM, audio)
        {
            result = l;
            winnerSpriteLeft = new Sprite("Backgrounds/PreGame/commanders", new Vector2(210, 270));
            winnerSpriteRight = new Sprite("Backgrounds/PreGame/commanders", new Vector2(1020, 270));
        }

        public override void Initialize()
        {
            base.Initialize();
            backgrounds.AddBackground(new Sprite("bases", Vector2.Zero));
            backgrounds.AddBackground(new Sprite("Backgrounds/ResultsScreen/winnersBackground", new Vector2(624, 240)));
            backgrounds.AddBackground(new Sprite("Backgrounds/ResultsScreen/resultsBackground", new Vector2(624, 360)));
            GenerateLeftHandStatistic();
            GenerateCenterStatistic();
            GenerateRightHandStatistic();
            GenerateWinnerString();
        }

        public override void LoadContent(ContentManager cM)
        {
            if (winnerSpriteLeft != null && winnerSpriteRight != null)
            {
                winnerSpriteLeft.LoadContent(textures);
                winnerSpriteRight.LoadContent(textures);
            }
            base.LoadContent(cM);
        }

        public override Level Update(GameTime gT)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start))
            {
                return new Menu(textures, sounds);
            }

            return base.Update(gT);
        }

        public override void Draw(SpriteBatch sB, GraphicsDeviceManager gM)
        {
            result.Draw(sB, gM);

            sB.Begin();
            backgrounds.DrawBackgrounds(sB, Vector2.Zero);
            objects.DrawBottom(sB, Vector2.Zero);
            typeOfGame.DrawBottom(sB, Vector2.Zero);
            projectiles.Draw(sB, Vector2.Zero);
            agents.DrawLowest(sB, Vector2.Zero);
            objects.DrawTop(sB, Vector2.Zero);
            agents.DrawHighest(sB, Vector2.Zero);
            backgrounds.Drawforegrounds(sB, Vector2.Zero);
            managers.GetTextureManager().DrawString(sB, leftHandStats, new Vector2(195, 570), Color.White, TextureManager.FontSizes.small);
            managers.GetTextureManager().DrawString(sB, centerStats, new Vector2(590, 605), Color.White, TextureManager.FontSizes.small);
            managers.GetTextureManager().DrawString(sB, rightHandStats, new Vector2(990, 590), Color.White, TextureManager.FontSizes.small);
            managers.GetTextureManager().DrawString(sB, winnerString, new Vector2(620, 150), Color.White, TextureManager.FontSizes.big);

            if(winnerString.CompareTo("Tie Game...") != 0)
                managers.GetTextureManager().DrawString(sB, winsString, new Vector2(620, 260), Color.White, TextureManager.FontSizes.big);

            if (winnerSpriteLeft != null)
            {
                winnerSpriteLeft.SetFrameIndex(winnerFrameLeft);
                winnerSpriteLeft.UpdateFrame();
                winnerSpriteLeft.Draw(sB, Vector2.Zero, managers);
            }
            if (winnerSpriteRight != null)
            {
                winnerSpriteRight.SetFrameIndex(winnerFrameRight);
                winnerSpriteRight.UpdateFrame();
                winnerSpriteRight.Draw(sB, Vector2.Zero, managers);
            }
            sB.End();
        }

        public void GenerateLeftHandStatistic()
        {
            leftHandStats = result.ToString();
            leftHandStats += "\n";
            leftHandStats += result.GetManagerHelper().GetGametype().GetWinnerString();
            leftHandStats += "\n";
            leftHandStats += result.GetManagerHelper().GetGametype().GetEndScores();
            leftHandStats += "\n";
        }

        public void GenerateCenterStatistic()
        {
            centerStats += result.GetManagerHelper().GetStatisticsManager().GetKDStatistics();
            centerStats += "\n";
        }

        public void GenerateRightHandStatistic()
        { 
            //players
            switch(managers.GetRandom().Next(3))
            {
                case 0:
                    rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetRedCommanderPowerStatistic();
                    break;
                case 1:
                    rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetBlueCommanderPowerStatistic();
                    break;
                case 2:
                    rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetGreenCommanderPowerStatistic();
                    break;
                case 3:
                    rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetYellowCommanderPowerStatistic();
                    break;
            }

            rightHandStats += "\n";

            //gametype specific
            if (result.GetManagerHelper().GetGametype() is Assasssins)
            {
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetVendittaStatistic();
                rightHandStats += "\n";
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMaxTimeAliveStatistic();
                rightHandStats += "\n";
            }
            else if (result.GetManagerHelper().GetGametype() is Deathmatch)
            {
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMostMedicsKilledStatistic();
                rightHandStats += "\n";
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMostDotsRecruitedStatistic();
                rightHandStats += "\n";
            }
            else if (result.GetManagerHelper().GetGametype() is CaptureTheFlag || result.GetManagerHelper().GetGametype() is Assault)
            {
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetCasualitiesStatistic();
                rightHandStats += "\n";
                switch(managers.GetRandom().Next(3))
                {
                    case 0:
                        rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMostFlagsCaputuredStatistic();
                        break;
                    case 1:
                        rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMostFlagsReturnedStatistic();
                        break;
                    case 2:
                        rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetQuickestFlagCaptureStatistic();
                        break;
                }
            }
            else if (result.GetManagerHelper().GetGametype() is Conquest)
            {
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetCasualitiesStatistic();
                rightHandStats += "\n";
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMostMedicsKilledStatistic();
                rightHandStats += "\n";
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetCasualitiesStatistic();
                rightHandStats += "\n";
            }
            //survival
            else
            {
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetCasualitiesStatistic();
                rightHandStats += "\n";
                rightHandStats += result.GetManagerHelper().GetStatisticsManager().GetMostDotsRecruitedStatistic();
                rightHandStats += "\n";
            }
        }

        public void GenerateWinnerString()
        {
            NPC secondaryWinner = null;

            if (result.GetManagerHelper().GetGametype() is Assasssins)
            {
                Assasssins gametypeWinner = (Assasssins)result.GetManagerHelper().GetGametype();
                NPC.AffliationTypes winner = gametypeWinner.GetWinner();
                secondaryWinner = result.GetManagerHelper().GetNPCManager().GetSecondaryCommander(winner);

                switch (winner)
                {
                    case NPC.AffliationTypes.red:
                        winnerString = "Mustachio ";
                        winnerFrameLeft = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerString = "Aqueos ";
                        winnerFrameLeft = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerString = "Terran ";
                        winnerFrameLeft = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerString = "Dian ";
                        winnerFrameLeft = YELLOW_INDEX;
                        break;
                    case NPC.AffliationTypes.same:
                        winnerString = "Tie Game...";
                        winnerSpriteRight = null;
                        winnerSpriteLeft = null;
                        break;
                }
            }
            else if (result.GetManagerHelper().GetGametype() is Assault)
            {
                Assault gametypeWinner = (Assault)result.GetManagerHelper().GetGametype();
                NPC.AffliationTypes winner = gametypeWinner.GetWinner();
                secondaryWinner = result.GetManagerHelper().GetNPCManager().GetSecondaryCommander(winner);

                switch (winner)
                {
                    case NPC.AffliationTypes.red:
                        winnerString = "Red Team ";
                        winnerFrameLeft = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerString = "Blue Team ";
                        winnerFrameLeft = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerString = "Green Team ";
                        winnerFrameLeft = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerString = "Yellow Team ";
                        winnerFrameLeft = YELLOW_INDEX;
                        break;
                    case NPC.AffliationTypes.same:
                        winnerString = "Tie Game...";
                        winnerSpriteRight = null;
                        winnerSpriteLeft = null;
                        break;
                }
            }
            else if (result.GetManagerHelper().GetGametype() is CaptureTheFlag)
            {
                CaptureTheFlag gametypeWinner = (CaptureTheFlag)result.GetManagerHelper().GetGametype();
                NPC.AffliationTypes winner = gametypeWinner.GetWinner();
                secondaryWinner = result.GetManagerHelper().GetNPCManager().GetSecondaryCommander(winner);

                switch (winner)
                {
                    case NPC.AffliationTypes.red:
                        winnerString = "Red Team ";
                        winnerFrameLeft = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerString = "Blue Team ";
                        winnerFrameLeft = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerString = "Green Team ";
                        winnerFrameLeft = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerString = "Yellow Team ";
                        winnerFrameLeft = YELLOW_INDEX;
                        break;
                    case NPC.AffliationTypes.same:
                        winnerString = "Tie Game...";
                        winnerSpriteRight = null;
                        winnerSpriteLeft = null;
                        break;
                }
            }
            else if (result.GetManagerHelper().GetGametype() is Conquest)
            {
                Conquest gametypeWinner = (Conquest)result.GetManagerHelper().GetGametype();
                NPC.AffliationTypes winner = gametypeWinner.GetWinner();
                secondaryWinner = result.GetManagerHelper().GetNPCManager().GetSecondaryCommander(winner);

                switch (winner)
                {
                    case NPC.AffliationTypes.red:
                        winnerString = "Red Team ";
                        winnerFrameLeft = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerString = "Blue Team ";
                        winnerFrameLeft = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerString = "Green Team ";
                        winnerFrameLeft = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerString = "Yellow Team ";
                        winnerFrameLeft = YELLOW_INDEX;
                        break;
                    case NPC.AffliationTypes.same:
                        winnerString = "Tie Game...";
                        winnerSpriteRight = null;
                        winnerSpriteLeft = null;
                        break;
                }
            }
            else if (result.GetManagerHelper().GetGametype() is Deathmatch)
            {
                Deathmatch gametypeWinner = (Deathmatch)result.GetManagerHelper().GetGametype();
                NPC.AffliationTypes winner = gametypeWinner.GetWinner();
                secondaryWinner = result.GetManagerHelper().GetNPCManager().GetSecondaryCommander(winner);

                switch (winner)
                {
                    case NPC.AffliationTypes.red:
                        winnerString = "Red Team ";
                        winnerFrameLeft = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerString = "Blue Team ";
                        winnerFrameLeft = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerString = "Green Team ";
                        winnerFrameLeft = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerString = "Yellow Team ";
                        winnerFrameLeft = YELLOW_INDEX;
                        break;
                    case NPC.AffliationTypes.same:
                        winnerString = "Tie Game...";
                        winnerSpriteRight = null;
                        winnerSpriteLeft = null;
                        break;
                }
            }
            else if (result.GetManagerHelper().GetGametype() is Survival)
            {
                Survival gametypeWinner = (Survival)result.GetManagerHelper().GetGametype();
                NPC.AffliationTypes winner = gametypeWinner.GetWinner();
                secondaryWinner = result.GetManagerHelper().GetNPCManager().GetSecondaryCommander(winner);

                switch (winner)
                {
                    case NPC.AffliationTypes.red:
                        winnerString = "Mustachio ";
                        winnerFrameLeft = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerString = "Aqueos ";
                        winnerFrameLeft = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerString = "Terran ";
                        winnerFrameLeft = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerString = "Dian ";
                        winnerFrameLeft = YELLOW_INDEX;
                        break;
                    case NPC.AffliationTypes.same:
                        winnerString = "Tie Game...";
                        winnerSpriteRight = null;
                        winnerSpriteLeft = null;
                        break;
                }
            }

            if(secondaryWinner != null)
                switch (secondaryWinner.GetPersonalAffilation())
                {
                    case NPC.AffliationTypes.red:
                        winnerFrameRight = RED_INDEX;
                        break;
                    case NPC.AffliationTypes.blue:
                        winnerFrameRight = BLUE_INDEX;
                        break;
                    case NPC.AffliationTypes.green:
                        winnerFrameRight = GREEN_INDEX;
                        break;
                    case NPC.AffliationTypes.yellow:
                        winnerFrameRight = YELLOW_INDEX;
                        break;
                }
        }
    }
}