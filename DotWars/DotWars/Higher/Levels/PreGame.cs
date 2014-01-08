using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    public class PreGame : Level
    {
        private Level nextLevel;
        private Gametype nextGame;
        private PlayerIndex hostIndex;
        private double timer;
        private double beginTime;

        private Sprite background;
        private Sprite mustachio;
        private Sprite aqueos;
        private Sprite dian;
        private Sprite terron;
        private Sprite bases;

        private String red;
        private String blue;
        private String green;
        private String yellow;

        private Vector2 redPos;
        private Vector2 bluePos;
        private Vector2 greenPos;
        private Vector2 yellowPos;

        private String on;
        private String mapName;

        private String tipText;
        private const int NUM_COMMANDERS = 4;
        private Dictionary<Gametype.GT, Tip[]> DictTip;

        private enum DOTS
        {
            MUSTACHIO, AQUOES, DIAN, TERRON
        }

        private class Tip
        {
            private String tip;

            public Tip(String tip)
            {
                this.tip = tip;
            }

            public String getTip()
            {
                return this.tip;
            }
        }

        public PreGame(Level level, Gametype game, PlayerIndex indexOfHost, TextureManager loadedTextures, AudioManager audio)
            : base(null, new Dictionary<Type, int>(), Vector2.Zero, loadedTextures, audio)
        {
            nextLevel = level;
            nextGame = game;
            hostIndex = indexOfHost;

            timer = 5;
            beginTime = 0;

            redPos = new Vector2(-1000, 0);
            bluePos = new Vector2(-1000, 0);
            greenPos = new Vector2(-1000, 0);
            yellowPos = new Vector2(-1000, 0);

            on = "on";
            mapName = nextLevel.ToString();

            initalizeTeamImages();
            initalizeBackground();

        

            //bases = new Sprite("Backgrounds/PreGame/tipsBases", picturePos);

            #region TIP DECLARATIONS


            this.DictTip = new Dictionary<Gametype.GT, Tip[]>();

            Tip[] assassinsTips = new Tip[4];
            assassinsTips[0] = new Tip( "Mustachio's ability does damage over time.");
            assassinsTips[1] = new Tip( "Not only does Aquoes' ability slow enemies" + "\n"
                                     +  "but it also heals himself and his allies.");
            assassinsTips[2] = new Tip( "Terron's ability deals damage if it hits an" + "\n"
                                     +  "enemy.");
            assassinsTips[3] = new Tip( "Dian's ability does damage to any enemy Dot" + "\n"
                                     +  "that walks through the lightning.");
            this.DictTip.Add(Gametype.GT.ASSASSINS, assassinsTips);

            Tip[] assaultTips = new Tip[2];
            assaultTips[0] = new Tip(   "Assault is a game of offense and defense. " + "\n"
                                   +    "Be wary of other commanders!");
            assaultTips[1] = new Tip(   "Only Commanders and Grunts can take the flag.");
            this.DictTip.Add(Gametype.GT.ASSAULT, assaultTips);

            Tip[] conquestTips = new Tip[7];
            conquestTips[0] = new Tip(  "Mustachio's ability to deal large amounts" + "\n"
                                    +   "of damage makes it a great for base control.");
            conquestTips[1] = new Tip(  "Aquoes's ability has many uses in Conquest." + "\n"
                                    +   "It can be used to defend bases or heal " + "\n"
                                    +   "troops to siege an enemy's base.");
            conquestTips[2] = new Tip(  "Use Terron's ability to keep enemies away " + "\n"
                                    +   "from your bases or to give yourself cover " + "\n"
                                    +   "while you take theirs.");
            conquestTips[3] = new Tip(  "Dian's ability can be used to shred through " + "\n"
                                    +   "enemies that try to go on the offensive.");
            conquestTips[4] = new Tip(  "When capturing a base, the base will change " + "\n"
                                    +   "to the color of the team, not necessarily the" + "\n"
                                    +   "color of the individual unit that captured it.");
            conquestTips[5] = new Tip(  "Be wary of airstrikes while attempting to" + "\n"
                                    +   "capture a base.");
            conquestTips[6] = new Tip(  "Only Commanders and Grunts can capture bases.");
            this.DictTip.Add(Gametype.GT.CONQUEST, conquestTips);

            Tip[] ctfTips = new Tip[5];
            ctfTips[0] = new Tip(       "Mustachio's ability can be used to easily" + "\n"
                               +        "shut down an enemy flag runner or mow through" + "\n"
                               +        "a crowd while trying to make a capture.");
            ctfTips[1] = new Tip(       "Aquoes can keep enemies off his trail by" + "\n"
                               +        "using his ability to slow anyone coming " + "\n"
                               +        "after him.");
            ctfTips[2] = new Tip(       "Terron's boulders are great for blocking" + "\n"
                               +        "off paths and stopping persuing enemies.");
            ctfTips[3] = new Tip(       "Dian has amazing mobility thanks to her" + "\n"
                               +        "ability, making her very effective at" + "\n"
                               +        "capturing flags.");
            ctfTips[4] = new Tip(       "Only Grunts and Commanders can capture flags.");
            this.DictTip.Add(Gametype.GT.CTF, ctfTips);

            Tip[] deathmatchTips = new Tip[6];
            deathmatchTips[0] = new Tip("Coming face-to-face with a Gunner is almost " + "\n"
                               +        "certain death. Try to take a Gunner out from" + "\n" 
                                    +   "a distance by using grenades.");
            deathmatchTips[1] = new Tip("Rockets explode, and can take out many allied" + "\n"
                                      + "units at once. Beware the Specialist.");
            deathmatchTips[2] = new Tip("The Medic heals nearby allies.");
            deathmatchTips[3] = new Tip("The Bombardier calls in devastating airstrikes." + "\n"
                                      + "A large reticle will appear to indicate their " + "\n"
                                      + "target.");
            deathmatchTips[4] = new Tip("Keep an eye out for Snipers, who can kill" + "\n"
                                      + "you from afar.");
            deathmatchTips[5] = new Tip("The Juggernaut will try to act as a shield for" + "\n"
                                      + "his allies. Dispatch them by tossing grenades" + "\n"
                                      +  "behind them.");
            this.DictTip.Add(Gametype.GT.DEATHMATCH, deathmatchTips);

            Tip[] survivalTips = new Tip[9];
            survivalTips[0] = new Tip(  "Gray dots will become faster over time.");
            survivalTips[1] = new Tip(  "You cannot kill one another in Survival.");
            survivalTips[2] = new Tip(  "Gray dots will become faster over time.");
            survivalTips[3] = new Tip(  "You cannot kill other commanders in Survival.");
            survivalTips[4] = new Tip(  "Gray dots will become faster over time.");
            survivalTips[5] = new Tip(  "You cannot kill other commanders in Survival.");
            survivalTips[6] = new Tip(  "Gray dots will become faster over time.");
            survivalTips[7] = new Tip(  "You cannot kill other commanders in Survival.");
            survivalTips[8] = new Tip(  "SIE SIND DAS ESSEN UND WIR SIND DIE JAGER!");
            this.DictTip.Add(Gametype.GT.SURVIVAL, survivalTips);
            #endregion
        }

        private void initalizeTeamImages()
        {
            if (nextGame is Survival || nextGame is Assasssins)
            {
                initalizeCommanderImagesAssassinsSurvival();
                initalizeCommanderTextAssassinsSurvival();
            }
            else
            {
                initalizeCommanderImagesTeams();
                initalizeCommanderTextTeams();
            }
        }

        private void initalizeCommanderTextAssassinsSurvival()
        {
            red = "Mustachio";
            blue = "Aqueos";
            green = "Terron";
            yellow = "Dian";

            redPos = new Vector2(DEFAUT_SCREEN_SIZE.X/11, DEFAUT_SCREEN_SIZE.Y/10);
            bluePos = new Vector2(DEFAUT_SCREEN_SIZE.X/11 * 10, DEFAUT_SCREEN_SIZE.Y/10);
            greenPos = new Vector2(DEFAUT_SCREEN_SIZE.X/11, DEFAUT_SCREEN_SIZE.Y/10 * 6);
            yellowPos = new Vector2(DEFAUT_SCREEN_SIZE.X/11 * 10, DEFAUT_SCREEN_SIZE.Y/10 * 6);
        }

        private void initalizeCommanderImagesAssassinsSurvival()
        {
            Vector2 mustachioPos = new Vector2();
            Vector2 aqueosPos = new Vector2();
            Vector2 terronPos = new Vector2();
            Vector2 dianPos = new Vector2();

            mustachioPos    =new Vector2(DEFAUT_SCREEN_SIZE.X/11, DEFAUT_SCREEN_SIZE.Y/10 + 120);
            aqueosPos        =new Vector2(DEFAUT_SCREEN_SIZE.X/11 * 10, DEFAUT_SCREEN_SIZE.Y/10 + 120);
            terronPos       = new Vector2(DEFAUT_SCREEN_SIZE.X/11, DEFAUT_SCREEN_SIZE.Y/10 * 6 + 120);
            dianPos          = new Vector2(DEFAUT_SCREEN_SIZE.X/11 * 10, DEFAUT_SCREEN_SIZE.Y/10 * 6 + 120);

            mustachio = new Sprite("Backgrounds/PreGame/commanders", mustachioPos);
            aqueos = new Sprite("Backgrounds/PreGame/commanders", aqueosPos);
            terron = new Sprite("Backgrounds/PreGame/commanders", terronPos);
            dian = new Sprite("Backgrounds/PreGame/commanders", dianPos);
        }

        private void initalizeCommanderTextTeams()
        {
            red = "Red Team";
            blue = "Blue Team";
            green = "Green Team";
            yellow = "Yellow Team";
        }

        private void initalizeCommanderImagesTeams() 
        {
            Vector2 mustachioPos = new Vector2();
            Vector2 aqueosPos = new Vector2();
            Vector2 terronPos = new Vector2();
            Vector2 dianPos = new Vector2();

            float x1 = DEFAUT_SCREEN_SIZE.X / 11,
                    x2 = DEFAUT_SCREEN_SIZE.X / 11 * 10,
                    y1 = DEFAUT_SCREEN_SIZE.Y / 8 + 120,
                    y2 = DEFAUT_SCREEN_SIZE.Y / 8 * 4 + 120,
                    y3 = DEFAUT_SCREEN_SIZE.Y / 8; //text

            NPC.AffliationTypes team = nextGame.GetTeams()[0];
            NPC.AffliationTypes secondary = NPC.AffliationTypes.grey;
            NPC.AffliationTypes primary = NPC.AffliationTypes.grey;
            Dictionary<Type, NPC.AffliationTypes> commanders = nextGame.GetPlayers();
            foreach (KeyValuePair<Type, NPC.AffliationTypes> commander in commanders)
            {
                if (NPC.CommanderColor(commander.Key) == team && commander.Value == team)
                {
                    primary = NPC.CommanderColor(commander.Key);
                }

                if (NPC.CommanderColor(commander.Key) != team && commander.Value == team)
                {
                    secondary = NPC.CommanderColor(commander.Key);
                }
            }
            if (team == NPC.AffliationTypes.red)
            {
                redPos = new Vector2(x1, y3);
                mustachioPos = new Vector2(x1, y1);
            }
            else if (team == NPC.AffliationTypes.blue)
            {
                bluePos = new Vector2(x1, y3);
                aqueosPos = new Vector2(x1, y1);
            }
            else if (team == NPC.AffliationTypes.green)
            {
                greenPos = new Vector2(x1, y3);
                terronPos = new Vector2(x1, y1);
            }
            else if (team == NPC.AffliationTypes.yellow)
            {
                yellowPos = new Vector2(x1, y3);
                dianPos = new Vector2(x1, y1);
            }
            if (secondary == NPC.AffliationTypes.red)
                mustachioPos = new Vector2(x1, y2);
            else if (secondary == NPC.AffliationTypes.blue)
                aqueosPos = new Vector2(x1, y2);
            else if (secondary == NPC.AffliationTypes.green)
                terronPos = new Vector2(x1, y2);
            else if (secondary == NPC.AffliationTypes.yellow)
                dianPos = new Vector2(x1, y2);
            ////
            team = nextGame.GetTeams()[1];
            foreach (KeyValuePair<Type, NPC.AffliationTypes> commander in commanders)
            {
                if (NPC.CommanderColor(commander.Key) == team && commander.Value == team)
                {
                    primary = NPC.CommanderColor(commander.Key);
                }

                if (NPC.CommanderColor(commander.Key) != team && commander.Value == team)
                {
                    secondary = NPC.CommanderColor(commander.Key);
                }
            }
            if (team == NPC.AffliationTypes.red)
            {
                redPos = new Vector2(x2, y3);
                mustachioPos = new Vector2(x2, y1);
            }
            else if (team == NPC.AffliationTypes.blue)
            {
                bluePos = new Vector2(x2, y3);
                aqueosPos = new Vector2(x2, y1);
            }
            else if (team == NPC.AffliationTypes.green)
            {
                greenPos = new Vector2(x2, y3);
                terronPos = new Vector2(x2, y1);
            }
            else if (team == NPC.AffliationTypes.yellow)
            {
                yellowPos = new Vector2(x2, y3);
                dianPos = new Vector2(x2, y1);
            } 
            if (secondary == NPC.AffliationTypes.red)
                mustachioPos = new Vector2(x2, y2);
            else if (secondary == NPC.AffliationTypes.blue)
                aqueosPos = new Vector2(x2, y2);
            else if (secondary == NPC.AffliationTypes.green)
                terronPos = new Vector2(x2, y2);
            else if (secondary == NPC.AffliationTypes.yellow)
                dianPos = new Vector2(x2, y2);


            mustachio = new Sprite("Backgrounds/PreGame/commanders", mustachioPos);
            aqueos = new Sprite("Backgrounds/PreGame/commanders", aqueosPos);
            terron = new Sprite("Backgrounds/PreGame/commanders", terronPos);
            dian = new Sprite("Backgrounds/PreGame/commanders", dianPos);
        }

        private void initalizeBackground()
        {
            backgrounds = new BackgroundManager();

            if (nextLevel.ToString() == "Relic")
            {
                backgrounds.AddBackground(new Sprite("Backgrounds/Relic/relicBackground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Relic/relicBlockers", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Relic/meteorAnimated2", DEFAUT_SCREEN_SIZE / 2));
            }
            else if (nextLevel.ToString() == "Archipelago")
            {
                backgrounds.AddBackground(new Sprite("Backgrounds/Archipelago/archipelago_background", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Archipelago/archipelago_trees", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Archipelago/archipelago_solid", DEFAUT_SCREEN_SIZE / 2));
            }
            else if (nextLevel.ToString() == "Faercrag")
            {
                backgrounds.AddBackground(new Sprite("Backgrounds/Faercrag/faercragBackground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Faercrag/faercragBlockers", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Faercrag/faercragImpassable", DEFAUT_SCREEN_SIZE / 2));
            }
            else if (nextLevel.ToString() == "Plaza")
            {
                backgrounds.AddForeground(new Sprite("Backgrounds/Plaza/plazaLighting", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddBackground(new Sprite("Backgrounds/Plaza/plazaBackground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddBackground(new Sprite("Backgrounds/Plaza/plazaBlockers", DEFAUT_SCREEN_SIZE / 2));
                Sprite light = new Sprite("Backgrounds/Plaza/light", new Vector2(476, 184));
                backgrounds.AddBackground(light);
                light = new Sprite("Backgrounds/Plaza/light", new Vector2(832, 182));
                light.Turn((float)(Math.PI) * .25f);
                backgrounds.AddBackground(light);
                light = new Sprite("Backgrounds/Plaza/light", new Vector2(832, 512));
                light.Turn((float)(Math.PI) * .5f);
                backgrounds.AddBackground(light);
            }
            else if (nextLevel.ToString() == "Switch")
            {
                float x = 25;
                float y = -40;

                backgrounds.AddBackground(new Sprite("Backgrounds/Switch/switchBackground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Switch/switchForeground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Switch/switchBlockers", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Switch/switchCrane", new Vector2(600+x, 600+y)));

                Sprite belts = new Sprite("Backgrounds/Switch/BL", new Vector2(564+x, 288+y));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/BLr", new Vector2(712.5f+x, 435+y));
                belts.Turn((float)(Math.PI));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/BRr", new Vector2(616+x, 221+y));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/BR", new Vector2(779+x, 384+y));
                belts.Turn((float)(Math.PI));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/BT", new Vector2(687+x, 192.5f+y));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/BTr", new Vector2(807+x, 313+y));
                belts.Turn((float)(Math.PI));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/DBr", new Vector2(782+x, 217+y));
                belts.Turn((float)(Math.PI));
                backgrounds.AddForeground(belts);
                belts = new Sprite("Backgrounds/Switch/DBr", new Vector2(641+x, 359+y));
                backgrounds.AddForeground(belts);
            }
            else if (nextLevel.ToString() == "Zen Garden")
            {
                backgrounds.AddBackground(new Sprite("Backgrounds/ZenGarden/zenBackground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddForeground(new Sprite("Backgrounds/ZenGarden/zenForeground", DEFAUT_SCREEN_SIZE/2));
                backgrounds.AddForeground(new Sprite("Backgrounds/ZenGarden/zenBlockers", DEFAUT_SCREEN_SIZE / 2));
            }
            else if (nextLevel.ToString() == "Caged")
            {
                backgrounds.AddForeground(new Sprite("Backgrounds/Caged/cagedBackground", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddForeground(new Sprite("Backgrounds/Caged/cagedBlockers", DEFAUT_SCREEN_SIZE / 2));
                backgrounds.AddBackground(new Sprite("Backgrounds/Caged/lava",DEFAUT_SCREEN_SIZE / 2));
            }
        }

        public override void Initialize()
        {

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager cM)
        {
            backgrounds.LoadContent(textures);

            mustachio.LoadContent(textures);
            aqueos.LoadContent(textures);
            terron.LoadContent(textures);
            dian.LoadContent(textures);
            //bases.LoadContent(textures);

            Random tipSelect = new Random();
            Tip[] tipList = this.DictTip[nextGame.typeOfGame];
            Tip displayTip = tipList[tipSelect.Next(tipList.Length)];
            this.tipText = displayTip.getTip();

            #region set up commmanders images
            this.mustachio.SetFrameIndex(0);
            this.mustachio.UpdateFrame();
            
            this.aqueos.SetFrameIndex(1);
            this.aqueos.UpdateFrame(); 

            this.terron.SetFrameIndex(2);
            this.terron.UpdateFrame();
            
            this.dian.SetFrameIndex(3);
            this.dian.UpdateFrame();
            #endregion

        }

        public override Level Update(GameTime gT)
        {
            if (timer <= beginTime)
            {
                if (GamePad.GetState(hostIndex).IsButtonDown(Buttons.Start) || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    sounds.KillSounds();

                    return nextLevel;
                }
            }
            else
            {
                timer -= gT.ElapsedGameTime.TotalSeconds;
            }

            if (GamePad.GetState(hostIndex).IsButtonDown(Buttons.Back))
            {
                return new Menu(textures, sounds);
            }

            return this;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, GraphicsDeviceManager gM, bool drawHUD)
        {
            sB.Begin();
            //background.Draw(sB, Vector2.Zero, managers);
            backgrounds.DrawBackgrounds(sB, Vector2.Zero);
            backgrounds.Drawforegrounds(sB, Vector2.Zero);
            
            textures.DrawString(sB, red, redPos, Color.Red, TextureManager.FontSizes.small, true);
            mustachio.Draw(sB, new Vector2(0, 0), managers);
            textures.DrawString(sB, blue, bluePos, Color.Blue, TextureManager.FontSizes.small, true);
            aqueos.Draw(sB, new Vector2(0, 0), managers);
            textures.DrawString(sB, green, greenPos, Color.Green, TextureManager.FontSizes.small, true);
            terron.Draw(sB, new Vector2(0, 0), managers);
            textures.DrawString(sB, yellow, yellowPos, Color.Yellow, TextureManager.FontSizes.small, true);
            dian.Draw(sB, new Vector2(0, 0), managers);

            textures.DrawString(sB, nextGame.GetName(), new Vector2(DEFAUT_SCREEN_SIZE.X/2, DEFAUT_SCREEN_SIZE.Y/8), Color.White, TextureManager.FontSizes.small, true);
            textures.DrawString(sB, on, new Vector2(DEFAUT_SCREEN_SIZE.X / 2, DEFAUT_SCREEN_SIZE.Y / 8+32), Color.White, TextureManager.FontSizes.small, true);
            textures.DrawString(sB, mapName, new Vector2(DEFAUT_SCREEN_SIZE.X / 2, DEFAUT_SCREEN_SIZE.Y / 8+64), Color.White, TextureManager.FontSizes.small, true);

            textures.DrawString(sB, nextGame.GetSummary(), new Vector2(DEFAUT_SCREEN_SIZE.X / 2, DEFAUT_SCREEN_SIZE.Y / 8 + 192), Color.White, TextureManager.FontSizes.small, true);

            textures.DrawString(sB, ((timer <= beginTime) ? "" : (int)Math.Ceiling(timer) + ""), new Vector2(DEFAUT_SCREEN_SIZE.X / 2, DEFAUT_SCREEN_SIZE.Y / 8 + 320), Color.White, TextureManager.FontSizes.big, true);

            textures.DrawString(sB, tipText, new Vector2(DEFAUT_SCREEN_SIZE.X / 2, DEFAUT_SCREEN_SIZE.Y / 2 + 230), Color.White, TextureManager.FontSizes.small, true);
            sB.End();
        }
    }
}
