#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace DotWars
{
    public class Logo : Level
    {
        private enum stage
        {
            drawing,
            loading
        }

        private stage theStage;
        private ContentManager theContent;
        private readonly Sprite logo;
        private Sprite logoFade, controlsFade;
        private readonly Sprite controls;
        private int startLoad;

        public Logo()
            : base(null, new Dictionary<Type, int>(), new Vector2(1248, 720), null, null)
        {
            textures = new TextureManager();
            logo = new Sprite("Textures/Backgrounds/LogoScreen/splashLogoCombined", DEFAUT_SCREEN_SIZE/2);
            controls = new Sprite("Textures/Backgrounds/LogoScreen/controllerSplash", DEFAUT_SCREEN_SIZE/2);
            theStage = stage.drawing;
            sounds = new AudioManager();
        }

        public override void Initialize()
        {
            #region Menu

            textures.Add("Backgrounds/Menu/menuBackground", new Rectangle(0, 0, 1248, 720));

            //StartScreen
            textures.Add("Backgrounds/Menu/LogoScreen", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/LogoScreen/controllerSplash", new Rectangle(0, 0, 1250, 369));
            textures.Add("Backgrounds/Menu/backButton", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/Menu/startButton", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/Menu/triggers", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/Menu/triggersOnly", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/Menu/menuSelector", new Rectangle(0, 0, 625, 139));

            //Dotapedia
            textures.Add("Backgrounds/Menu/dotapedia", new Rectangle(0, 0, 1248, 720));
            textures.Add("Dots/dotAtlas", new Rectangle(0, 0, 128, 32));
            textures.Add("Dots/commanderAtlas", new Rectangle(0, 0, 32, 32));

            //PlayerScreen
            textures.Add("Backgrounds/Menu/CommanderCards", new Rectangle(0, 0, 260, 432));
            textures.Add("Backgrounds/Menu/buttonA", new Rectangle(0, 0, 48, 48));
            textures.Add("Backgrounds/Menu/buttonB", new Rectangle(0, 0, 48, 48));

            //GameScreen
            textures.Add("Backgrounds/Menu/mapGametypeCard", new Rectangle(0, 0, 624, 200));

            //TeamScreen
            textures.Add("Backgrounds/Menu/teamCards", new Rectangle(0, 0, 90, 90));
            textures.Add("Backgrounds/Menu/controllerCards", new Rectangle(0, 0, 260, 432));

            //Results Screen
            textures.Add("Backgrounds/ResultsScreen/winnersBackground", new Rectangle(0, 0, 1028, 288));
            textures.Add("Backgrounds/ResultsScreen/resultsBackground", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/ResultsScreen/startButtonWhite", new Rectangle(0, 0, 1248, 720));

            #endregion

            #region Archipelago

            textures.Add("Backgrounds/Archipelago/archipelago_background", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/Archipelago/archipelago_solid", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/Archipelago/archipelago_trees", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/Archipelago/whirlpool0", new Rectangle(0, 0, 256, 256));
            textures.Add("Backgrounds/Archipelago/whirlpool1", new Rectangle(0, 0, 224, 224));
            textures.Add("Backgrounds/Archipelago/whirlpool2", new Rectangle(0, 0, 160, 160));
            textures.Add("Backgrounds/Archipelago/whirlpool3", new Rectangle(0, 0, 96, 96));
            textures.Add("Backgrounds/Archipelago/whirlpool4", new Rectangle(0, 0, 64, 64));
            textures.Add("Backgrounds/Archipelago/whirlpool5", new Rectangle(0, 0, 32, 32));

            #endregion

            #region Caged

            textures.Add("Backgrounds/Caged/cagedBackground", new Rectangle(0, 0, 864, 544));
            textures.Add("Backgrounds/Caged/cagedBlockers", new Rectangle(0, 0, 864, 544));
            textures.Add("Backgrounds/Caged/cagedImpassable", new Rectangle(0, 0, 864, 544));
            textures.Add("Backgrounds/Caged/lava", new Rectangle(0, 0, 200, 544));

            #endregion

            #region Zen Garden

            textures.Add("Backgrounds/ZenGarden/zenBackground", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/ZenGarden/zenBlockers", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/ZenGarden/zenForeground", new Rectangle(0, 0, 1312, 992));

            #endregion

            #region Faercrag

            textures.Add("Backgrounds/Faercrag/faercragBlockers", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/Faercrag/faercragBackground", new Rectangle(0, 0, 1312, 992));
            textures.Add("Backgrounds/Faercrag/faercragImpassable", new Rectangle(0, 0, 1312, 992));

            #endregion

            #region Relic

            textures.Add("Backgrounds/Relic/relicBackground", new Rectangle(0, 0, 1088, 864));
            textures.Add("Backgrounds/Relic/relicBlockers", new Rectangle(0, 0, 1088, 864));
            textures.Add("Backgrounds/Relic/meteorAnimated2", new Rectangle(0, 0, 128, 128));
            textures.Add("Backgrounds/Relic/pulse", new Rectangle(0, 0, 470, 441));
            textures.Add("Backgrounds/Relic/impathableMeteor", new Rectangle(0, 0, 96, 32));

            #endregion

            #region Switch

            textures.Add("Backgrounds/Switch/switchBackground", new Rectangle(0, 0, 800, 800));
            textures.Add("Backgrounds/Switch/switchForeground", new Rectangle(0, 0, 800, 800));
            textures.Add("Backgrounds/Switch/switchBlockers", new Rectangle(0, 0, 800, 800));
            textures.Add("Backgrounds/Switch/switchCrane", new Rectangle(0, 0, 230, 230));
            textures.Add("Backgrounds/Switch/BL", new Rectangle(0, 0, 166, 27));
            textures.Add("Backgrounds/Switch/BLr", new Rectangle(0, 0, 27, 166));
            textures.Add("Backgrounds/Switch/BR", new Rectangle(0, 0, 200, 27));
            textures.Add("Backgrounds/Switch/BRr", new Rectangle(0, 0, 27, 200));
            textures.Add("Backgrounds/Switch/BT", new Rectangle(0, 0, 116, 27));
            textures.Add("Backgrounds/Switch/BTr", new Rectangle(0, 0, 27, 116));
            textures.Add("Backgrounds/Switch/DBr", new Rectangle(0, 0, 77, 76));
            textures.Add("Backgrounds/Switch/switchBox", new Rectangle(0, 0, 21, 21));

            textures.Add("Backgrounds/Switch/elect1", new Rectangle(0, 0, 96, 32));
            textures.Add("Backgrounds/Switch/elect2", new Rectangle(0, 0, 128, 32));
            textures.Add("Backgrounds/Switch/elect3", new Rectangle(0, 0, 32, 96));
            textures.Add("Backgrounds/Switch/elect4", new Rectangle(0, 0, 32, 64));
            textures.Add("Backgrounds/Switch/elect5", new Rectangle(0, 0, 64, 32));
            textures.Add("Backgrounds/Switch/elect6", new Rectangle(0, 0, 44, 49));
            textures.Add("Backgrounds/Switch/elect7", new Rectangle(0, 0, 32, 71));
            textures.Add("Backgrounds/Switch/elect8", new Rectangle(0, 0, 96, 95));
            textures.Add("Backgrounds/Switch/elect9", new Rectangle(0, 0, 64, 32));
            textures.Add("Backgrounds/Switch/elect10", new Rectangle(0, 0, 96, 32));
            textures.Add("Backgrounds/Switch/elect11", new Rectangle(0, 0, 32, 145));
            textures.Add("Backgrounds/Switch/elect12", new Rectangle(0, 0, 32, 98));

            #endregion

            #region Plaza

            textures.Add("Backgrounds/Plaza/plazaBackground", new Rectangle(0, 0, 1328, 1008));
            textures.Add("Backgrounds/Plaza/plazaBlockers", new Rectangle(0, 0, 1328, 1008));
            textures.Add("Backgrounds/Plaza/plazaLighting", new Rectangle(0, 0, 1328, 1008));
            textures.Add("Backgrounds/Plaza/light", new Rectangle(0, 0, 160, 160));

            #endregion

            #region PreGame

            textures.Add("Backgrounds/PreGame/commanders", new Rectangle(0, 0, 200, 200));
            textures.Add("Backgrounds/PreGame/profileBombardier", new Rectangle(0, 0, 200, 200));
            textures.Add("Backgrounds/PreGame/profileGrunt", new Rectangle(0, 0, 200, 200));
            textures.Add("Backgrounds/PreGame/profileGunner", new Rectangle(0, 0, 300, 200));
            textures.Add("Backgrounds/PreGame/profileJuggernaut", new Rectangle(0, 0, 200, 200));
            textures.Add("Backgrounds/PreGame/profileMedic", new Rectangle(0, 0, 200, 200));
            textures.Add("Backgrounds/PreGame/profileSniper", new Rectangle(0, 0, 400, 200));
            textures.Add("Backgrounds/PreGame/profileSpecialist", new Rectangle(0, 0, 200, 300));
            textures.Add("Backgrounds/PreGame/tipsBases", new Rectangle(0, 0, 200, 200));
            textures.Add("Backgrounds/PreGame/profileMeanie", new Rectangle(0, 0, 300, 300));
            textures.Add("Backgrounds/PreGame/singlesPregameBkg", new Rectangle(0, 0, 1248, 720));
            textures.Add("Backgrounds/PreGame/teamsPregameBkg", new Rectangle(0, 0, 1248, 720));

            #endregion

            #region Default Textures

            textures.Add("Dots/Red/grunt_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/bombardier_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/bombardier_redVFX", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/specialist_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/juggernaut_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/sniper_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/medic_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/gunner_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Red/commander_red", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Blue/grunt_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/bombardier_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/bombardier_blueVFX", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/specialist_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/juggernaut_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/sniper_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/medic_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/gunner_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Blue/commander_blue", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Green/grunt_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/bombardier_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/bombardier_greenVFX", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/specialist_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/juggernaut_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/sniper_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/medic_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/gunner_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Green/commander_green", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Yellow/grunt_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/bombardier_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/bombardier_yellowVFX", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/specialist_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/juggernaut_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/sniper_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/medic_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/gunner_yellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Dots/Yellow/commander_yellow", new Rectangle(0, 0, 32, 32));

            textures.Add("Dots/Red/bomber_red", new Rectangle(0, 0, 96, 64));
            textures.Add("Dots/Blue/bomber_blue", new Rectangle(0, 0, 96, 64));
            textures.Add("Dots/Green/bomber_green", new Rectangle(0, 0, 96, 64));
            textures.Add("Dots/Yellow/bomber_yellow", new Rectangle(0, 0, 96, 64));

            textures.Add("Dots/Red/targetRed", new Rectangle(0, 0, 64, 64));
            textures.Add("Dots/Blue/targetBlue", new Rectangle(0, 0, 64, 64));
            textures.Add("Dots/Green/targetGreen", new Rectangle(0, 0, 64, 64));
            textures.Add("Dots/Yellow/targetYellow", new Rectangle(0, 0, 64, 64));

            textures.Add("Projectiles/bullet_standard", new Rectangle(0, 0, 7, 6));
            textures.Add("Projectiles/bullet_sniper", new Rectangle(0, 0, 7, 6));
            textures.Add("Projectiles/bullet_rocket", new Rectangle(0, 0, 17, 10));
            textures.Add("Projectiles/bullet_shotgun", new Rectangle(0, 0, 7, 6));

            textures.Add("Projectiles/grenade", new Rectangle(0, 0, 16, 16));
            textures.Add("Projectiles/bullet_bombs", new Rectangle(0, 0, 14, 5));

            textures.Add("Effects/particle_smoke", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/smoke_red", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_blue", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_green", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_yellow", new Rectangle(0, 0, 9, 9));
            textures.Add("Effects/smoke_standard", new Rectangle(0, 0, 30, 30));

            textures.Add("Effects/suicide_spawn_sprite", new Rectangle(0, 0, 96, 96));

            textures.Add("Effects/spark_red", new Rectangle(0, 0, 5, 5));
            textures.Add("Effects/spark_blue", new Rectangle(0, 0, 5, 5));
            textures.Add("Effects/spark_green", new Rectangle(0, 0, 5, 5));
            textures.Add("Effects/spark_yellow", new Rectangle(0, 0, 5, 5));

            textures.Add("Effects/particle_blood_red", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_blood_blue", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_blood_green", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_blood_yellow", new Rectangle(0, 0, 8, 8));
            textures.Add("Effects/particle_fire", new Rectangle(0, 0, 14, 14));
            textures.Add("Effects/particle_heal", new Rectangle(0, 0, 16, 16));

            textures.Add("Effects/PI_redCommander", new Rectangle(0, 0, 64, 64));
            textures.Add("Effects/PI_blueCommander", new Rectangle(0, 0, 64, 64));
            textures.Add("Effects/PI_greenCommander", new Rectangle(0, 0, 64, 64));
            textures.Add("Effects/PI_yellowCommander", new Rectangle(0, 0, 64, 64));

            textures.Add("Dots/Grey/grey_claimable", new Rectangle(0, 0, 36, 36));
            textures.Add("Dots/Grey/grey_suicide", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/spr_bolt_strip3", new Rectangle(0, 0, 30, 30));

            textures.Add("Projectiles/mine", new Rectangle(0, 0, 16, 16));

            textures.Add("Effects/explodeTop", new Rectangle(0, 0, 128, 128));
            textures.Add("bases", new Rectangle(0, 0, 64, 64));
            textures.Add("Objectives/flag_red", new Rectangle(0, 0, 32, 32));
            textures.Add("Objectives/flag_blue", new Rectangle(0, 0, 32, 32));
            textures.Add("Objectives/flag_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Objectives/flag_yellow", new Rectangle(0, 0, 32, 32));

            //HUD elements
            textures.Add("HUD/bar_damage", new Rectangle(0, 0, 127, 14));
            textures.Add("HUD/bar_health", new Rectangle(0, 0, 127, 14));
            textures.Add("HUD/bar_special", new Rectangle(0, 0, 84, 14));
            textures.Add("HUD/hud_background", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_blue", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_green", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_gun", new Rectangle(0, 0, 55, 38));
            textures.Add("HUD/hud_red", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/hud_toss", new Rectangle(0, 0, 20, 20));
            textures.Add("HUD/hud_yellow", new Rectangle(0, 0, 240, 60));
            textures.Add("HUD/2playerSplit", cameras.GetFullSize().Bounds);
            textures.Add("HUD/4playerSplit", cameras.GetFullSize().Bounds);
            textures.Add("HUD/pauseOverlay", new Rectangle(0, 0, 1248, 720));
            textures.Add("HUD/hud_teammates", new Rectangle(0, 0, 40, 20));
            textures.Add("HUD/hud_score_frame", new Rectangle(0, 0, 348, 94));
            textures.Add("HUD/hud_time_frame", new Rectangle(0, 0, 154, 94));
            textures.Add("HUD/buttons", new Rectangle(0, 0, 1248, 720));

            //Guts
            textures.Add("Effects/Guts/Red/deadGruntRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadGunnerRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadJuggRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadBombRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadSniperRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadMedicRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadSpecialistRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Red/deadCommanderRed", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadGruntBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadGunnerBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadJuggBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadBombBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadSniperBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadMedicBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadSpecialistBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Blue/deadCommanderBlue", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadGruntGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadGunnerGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadJuggGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadBombGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadSniperGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadMedicGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadSpecialistGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Green/deadCommanderGreen", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadGruntYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadGunnerYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadJuggYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadBombYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadSniperYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadMedicYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadSpecialistYellow", new Rectangle(0, 0, 32, 32));
            textures.Add("Effects/Guts/Yellow/deadCommanderYellow", new Rectangle(0, 0, 32, 32));

            //Abilities
            textures.Add("Abilities/ability_red", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/ability_green", new Rectangle(0, 0, 32, 32));
            textures.Add("Abilities/ability_blue_splash", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/ability_blue_spread2", new Rectangle(0, 0, 64, 64));
            textures.Add("Abilities/yellow_test", new Rectangle(0, 0, 32, 16));
            textures.Add("Abilities/ability_green2", new Rectangle(0, 0, 96, 96));
            textures.Add("Abilities/ability_red2", new Rectangle(0, 0, 32, 32));

            textures.Add("square", new Rectangle(0, 0, 32, 32));

            #endregion
        }

        public override void LoadContent(ContentManager cM)
        {
            theContent = cM;

            logo.LoadContent(cM);
            controls.LoadContent(cM);
            textures.StartLoad(cM);
        }

        public override Level Update(GameTime gT)
        {
            if (theStage == stage.loading)
            {
                if (textures.GetLoadPercent() != 100)
                {
                    textures.LoadContent(theContent);
                }

                if (textures.GetLoadPercent() == 100)
                {
                    textures.FinishLoad();

                    for (int c = 0; c < 4; c++)
                    {
                        if (GamePad.GetState(CameraManager.GetPlayerIndex(c)).IsButtonDown(Buttons.Start) ||
                            Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            return new Menu(textures, sounds);
                        }
                    }
                }
            }

            return this;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sB, GraphicsDeviceManager gM,
                                  bool drawHUD)
        {
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            sB.GraphicsDevice.Clear(Color.White);
            if (textures.GetLoadPercent() > 65)
            {
                controls.Draw(sB, Vector2.Zero, null);
            }
            else
            {
                logo.Draw(sB, Vector2.Zero, null);
            }

            if (textures.GetLoadPercent() != 100)
            {
                textures.DrawString(sB, "Loading " + textures.GetLoadPercent() + "%", new Vector2(624, 624), Color.Gray,
                                    TextureManager.FontSizes.small, true);
            }
            else
            {
                textures.DrawString(sB, "Finished Loading\n   Press Start", new Vector2(624, 624), Color.Gray,
                                    TextureManager.FontSizes.small, true);
            }
            sB.End();

            theStage = stage.loading;
        }
    }
}