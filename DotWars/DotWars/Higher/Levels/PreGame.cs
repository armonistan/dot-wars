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
        private Sprite commander;
        private Sprite juggernaut;
        private Sprite gunner;
        private Sprite bombardier;
        private Sprite medic;
        private Sprite specialist;
        private Sprite sniper;
        private Sprite bases;
        private Sprite grunt;
        private Sprite tipPic;

        private int frameInt;
        private int tipInt;
        private String tipText;
        private const int NUM_COMMANDERS = 4;
        private Dictionary<Gametype.GT, Tip[]> DictTip;

        private enum DOTS
        {
            MUSTACHIO, AQUOES, DIAN, TERRON, GRUNT, JUGGERNAUT,
            MEDIC, GUNNER, BOMBARDIER, SPECIALIST, SNIPER,
            REDBASE, BLUEBASE, GREENBASE, YELLOWBASE
        }

        private class Tip
        {
            private String text;
            private DOTS[] images;

            public Tip(String text, DOTS[] images)
            {
                this.text = text;
                this.images = images;
            }

            public DOTS getRandomImage()
            {
                Random temp = new Random();
                return this.images[temp.Next(this.images.Length)];
            }

            public String getText()
            {
                return this.text;
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

            Vector2 picturePos = new Vector2(1030, 276);

            #region TIP DECLARATIONS
            background = new Sprite("Backgrounds/PreGame/splashTemplate", DEFAUT_SCREEN_SIZE / 2);
            commander = new Sprite("Backgrounds/PreGame/commanders", picturePos);
            juggernaut = new Sprite("Backgrounds/PreGame/profileJuggernaut", picturePos);
            gunner = new Sprite("Backgrounds/PreGame/profileGunner", picturePos);
            medic = new Sprite("Backgrounds/PreGame/profileMedic", picturePos);
            specialist = new Sprite("Backgrounds/PreGame/profileSpecialist", picturePos);
            sniper = new Sprite("Backgrounds/PreGame/profileSniper", picturePos);
            bombardier = new Sprite("Backgrounds/PreGame/profileBombardier", picturePos);
            grunt = new Sprite("Backgrounds/PreGame/profileGrunt", picturePos);
            bases = new Sprite("bases", picturePos);

            this.DictTip = new Dictionary<Gametype.GT, Tip[]>();

            Tip[] assassinsTips = new Tip[4];
            assassinsTips[0] = new Tip("Mustachio's ability does damage over time.", new DOTS[] { DOTS.MUSTACHIO });
            assassinsTips[1] = new Tip("Not only does Aquoes' ability slow enemies,\nbut it also heals himself and his allies.", new DOTS[] { DOTS.AQUOES });
            assassinsTips[2] = new Tip("Terron's ability deals damage if it hits an enemy.", new DOTS[] { DOTS.TERRON });
            assassinsTips[3] = new Tip("Dian's ability does damage to any enemy Dot\nthat walks through the lightning.", new DOTS[] { DOTS.DIAN });
            this.DictTip.Add(Gametype.GT.ASSASSINS, assassinsTips);

            Tip[] assaultTips = new Tip[2];
            assaultTips[0] = new Tip("Assault is a game of offense and defense.\nBe wary of other commanders!",
                new DOTS[] { DOTS.AQUOES, DOTS.BOMBARDIER, DOTS.DIAN, DOTS.GRUNT, DOTS.GUNNER, DOTS.JUGGERNAUT, DOTS.MEDIC, DOTS.MUSTACHIO, DOTS.SPECIALIST, DOTS.TERRON });
            assaultTips[1] = new Tip("Flares can be especially useful in Assault.\nUse them to defend key spots or to send troops into battle\nwhile you flank the enemy's front line.",
                new DOTS[] { DOTS.AQUOES, DOTS.BOMBARDIER, DOTS.DIAN, DOTS.GRUNT, DOTS.GUNNER, DOTS.JUGGERNAUT, DOTS.MEDIC, DOTS.MUSTACHIO, DOTS.SPECIALIST, DOTS.TERRON });
            this.DictTip.Add(Gametype.GT.ASSAULT, assaultTips);

            Tip[] conquestTips = new Tip[7];
            conquestTips[0] = new Tip("Mustachio's ability deals so much concentrated\ndamage that it is a great tool for base control.", new DOTS[] { DOTS.MUSTACHIO });
            conquestTips[1] = new Tip("Aquoes's ability has many uses in Conquest.\nIt can be used to defend bases or heal troops\nto siege an enemy's base.", new DOTS[] { DOTS.AQUOES });
            conquestTips[2] = new Tip("Use Terron's ability to keep enemies away\nfrom your bases or to give yourself cover while\nyou take theirs.", new DOTS[] { DOTS.TERRON });
            conquestTips[3] = new Tip("Dian's ability can be used to shred through\nenemies that try to go on the offensive.", new DOTS[] { DOTS.DIAN });
            conquestTips[4] = new Tip("When capturing a base, the base will change\nto the color of the team, not necessarily the\ncolor of the individual unit that captured it.", new DOTS[] { DOTS.BLUEBASE, DOTS.GREENBASE, DOTS.MUSTACHIO, DOTS.YELLOWBASE });
            conquestTips[5] = new Tip("Be wary of airstrikes while attempting to\ncapture a base.", new DOTS[] { DOTS.BOMBARDIER });
            conquestTips[6] = new Tip("Only Commanders and Grunts can capture bases.", new DOTS[] { DOTS.MUSTACHIO, DOTS.AQUOES, DOTS.TERRON, DOTS.DIAN, DOTS.GRUNT, DOTS.BLUEBASE, DOTS.REDBASE, DOTS.YELLOWBASE, DOTS.GREENBASE });
            this.DictTip.Add(Gametype.GT.CONQUEST, conquestTips);

            Tip[] ctfTips = new Tip[5];
            ctfTips[0] = new Tip("Mustachio's ability can be used to easily\nshut down an enemy flag runner or mow through\na crowd while trying to make a capture.", new DOTS[] { DOTS.MUSTACHIO });
            ctfTips[1] = new Tip("Aquoes can keep enemies off his trail by using\nhis ability to slow anyone coming after him.", new DOTS[] { DOTS.AQUOES });
            ctfTips[2] = new Tip("Terron's boulders are great for blocking\noff paths and stopping persuing enemies.", new DOTS[] { DOTS.TERRON });
            ctfTips[3] = new Tip("Dian has amazing mobility thanks to her ability,\nmaking her very effective at capturing flags.", new DOTS[] { DOTS.DIAN });
            ctfTips[4] = new Tip("Only Grunts and Commanders can capture flags.", new DOTS[] { DOTS.AQUOES, DOTS.DIAN, DOTS.GRUNT, DOTS.MUSTACHIO, DOTS.TERRON });
            this.DictTip.Add(Gametype.GT.CTF, ctfTips);

            Tip[] deathmatchTips = new Tip[7];
            deathmatchTips[0] = new Tip("Coming face-to-face with a Gunner\nis almost certain death. Try to take a Gunner\nout from a distance by using grenades.", new DOTS[] { DOTS.GUNNER });
            deathmatchTips[1] = new Tip("Rockets explode, and can take out\nmany allied units at once.\nBeware the Specialist.", new DOTS[] { DOTS.SPECIALIST });
            deathmatchTips[2] = new Tip("The Medic heals nearby allies.", new DOTS[] { DOTS.MEDIC });
            deathmatchTips[3] = new Tip("The Bombardier calls in devastating\nair strikes. When you hear the static, raise\nyour eyes to the skies.", new DOTS[] { DOTS.BOMBARDIER });
            deathmatchTips[4] = new Tip("Bombardiers will try to stay\naway from the heat of battle. If airstrikes have got you down,\nhunt these guys.", new DOTS[] { DOTS.BOMBARDIER });
            deathmatchTips[5] = new Tip("Keep an eye out for Snipers,\nwho can kill you from afar.", new DOTS[] { DOTS.SNIPER });
            deathmatchTips[6] = new Tip("The Juggernaut will try to act as a shield\nfor his allies. Dispatch them by\ntossing grenades behind them.", new DOTS[] { DOTS.JUGGERNAUT });
            this.DictTip.Add(Gametype.GT.DEATHMATCH, deathmatchTips);

            Tip[] survivalTips = new Tip[2];
            survivalTips[0] = new Tip("Gray dots will become faster over time.", new DOTS[] { DOTS.MUSTACHIO, DOTS.TERRON, DOTS.DIAN, DOTS.AQUOES });
            survivalTips[1] = new Tip("You cannot kill one another in Survival.", new DOTS[] { DOTS.MUSTACHIO, DOTS.TERRON, DOTS.DIAN, DOTS.AQUOES });
            this.DictTip.Add(Gametype.GT.SURVIVAL, survivalTips);
            #endregion
        }

        public override void Initialize()
        {

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager cM)
        {
            background.LoadContent(textures);

            commander.LoadContent(textures);
            juggernaut.LoadContent(textures);
            gunner.LoadContent(textures);
            medic.LoadContent(textures);
            specialist.LoadContent(textures);
            sniper.LoadContent(textures);
            bombardier.LoadContent(textures);
            bases.LoadContent(textures);
            grunt.LoadContent(textures);


            Random tipSelect = new Random();
            Tip[] tipList = this.DictTip[nextGame.typeOfGame];
            Tip displayTip = tipList[tipSelect.Next(tipList.Length)];
            this.tipText = displayTip.getText();

            DOTS tipImg = displayTip.getRandomImage();

            #region PARSETOIMAGE
            switch (tipImg)
            {
                case DOTS.AQUOES:
                    this.tipPic = commander;
                    this.tipPic.SetFrameIndex(1);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.MUSTACHIO:
                    this.tipPic = commander;
                    this.tipPic.SetFrameIndex(0);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.DIAN:
                    this.tipPic = commander;
                    this.tipPic.SetFrameIndex(3);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.TERRON:
                    this.tipPic = commander;
                    this.tipPic.SetFrameIndex(2);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.JUGGERNAUT:
                    this.tipPic = juggernaut;
                    break;
                case DOTS.SNIPER:
                    this.tipPic = sniper;
                    break;
                case DOTS.MEDIC:
                    this.tipPic = medic;
                    break;
                case DOTS.BOMBARDIER:
                    this.tipPic = bombardier;
                    break;
                case DOTS.SPECIALIST:
                    this.tipPic = specialist;
                    break;
                case DOTS.GREENBASE:
                    this.tipPic = bases;
                    this.tipPic.SetFrameIndex(3);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.YELLOWBASE:
                    this.tipPic = bases;
                    this.tipPic.SetFrameIndex(4);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.REDBASE:
                    this.tipPic = bases;
                    this.tipPic.SetFrameIndex(1);
                    this.tipPic.UpdateFrame();
                    break;
                case DOTS.BLUEBASE:
                    this.tipPic = bases;
                    this.tipPic.SetFrameIndex(2);
                    this.tipPic.UpdateFrame();
                    break;
                default:
                    this.tipPic = grunt;
                    break;
            }
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

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, GraphicsDeviceManager gM)
        {
            sB.Begin();
            background.Draw(sB, Vector2.Zero, managers);
            textures.DrawString(sB, nextGame.GetName(), new Vector2(600, 64), Color.White, TextureManager.FontSizes.big);
            textures.DrawString(sB, nextGame.GetSummary(), new Vector2(452, 240), Color.White, TextureManager.FontSizes.small);
            textures.DrawString(sB, ((timer <= beginTime) ? "Press Start To Begin!" : (int)timer + ""), new Vector2(1037, 600), Color.White, TextureManager.FontSizes.small);
            textures.DrawString(sB, tipText, new Vector2(420, 560), Color.White, TextureManager.FontSizes.small);
            tipPic.Draw(sB, Vector2.Zero, managers);
            sB.End();
        }
    }
}
