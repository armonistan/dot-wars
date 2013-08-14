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

        public ResultsScreen(Level l, TextureManager tM, AudioManager audio)
            : base(
                new ResultScreenGametype(), new Dictionary<Type, int>(),
                new Vector2(1248, 720), tM, audio)
        {
            result = l;

            if (result.GetManagerHelper().GetGametype() is Assasssins || result.GetManagerHelper().GetGametype() is Deathmatch ||
                result.GetManagerHelper().GetGametype() is CaptureTheFlag)
            {
                
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetMostMedicsKilledStatistic();
                //tempResult += "\n";
               // tempResult += result.GetManagerHelper().GetStatisticsManager().GetMaxTimeAliveStatistic();
               // tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetMostFlagsCaputuredStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetMostFlagsReturnedStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetVendittaStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetYellowCommanderPowerStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetRocksDestroyedStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetBlueCommanderPowerStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetMostDotsRecruitedStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetQuickestFlagCaptureStatistic();
                //tempResult += "\n";
                //tempResult += result.GetManagerHelper().GetStatisticsManager().GetRedCommanderPowerStatistic();
                //tempResult += "\n";
                // += result.GetManagerHelper().GetStatisticsManager().GetCasualitiesStatistic();
            }
            else if (result.GetManagerHelper().GetGametype() is Assault)
            {
                NPC.AffliationTypes tempWinner;
                var tempAss = (Assault)result.GetManagerHelper().GetGametype();
                if (result.GetManagerHelper().GetGametype().GetGameEndTimer() <= 0)
                {
                    tempWinner = tempAss.GetDefender();
                }
                else
                {
                    tempWinner = tempAss.GetAttacker();
                }
            }
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
        }

        public override void LoadContent(ContentManager cM)
        {
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
            managers.GetTextureManager().DrawString(sB, leftHandStats, new Vector2(100, 500), Color.White, TextureManager.FontSizes.small);
            managers.GetTextureManager().DrawString(sB, centerStats, new Vector2(400, 500), Color.White, TextureManager.FontSizes.small);
            managers.GetTextureManager().DrawString(sB, rightHandStats, new Vector2(900, 500), Color.White, TextureManager.FontSizes.small);
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
    }
}