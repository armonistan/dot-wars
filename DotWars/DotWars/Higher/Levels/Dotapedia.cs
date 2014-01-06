using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DotWars.Higher.Levels
{
    public class Info
    {
        public string damage { get; set; }
        public string health { get; set; }
        public string reload { get; set; }
        public string vision { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Sprite image;
        public Sprite[] inGame;

        public Info() 
        {}
    }

    class Dotapedia
    {
        private Vector2 picturePos;
        private Vector2 ingamePos;

        private Sprite commander;
        private Sprite juggernaut;
        private Sprite gunner;
        private Sprite bombardier;
        private Sprite medic;
        private Sprite specialist;
        private Sprite sniper;
        private Sprite grunt;
        private Sprite suicide;

        private Sprite commanderInGame;
        private Sprite soldierInGame;

        public String[] unitTypes { get; set; }
        public Info[] soldierInfo { get; set; }
        public Info[] commanderInfo { get; set; }

        private int unitMax, soldierMax, commanderMax;

        private int currentUnitType;
        private int currentSoldier;
        private int currentCommander;

        public Dotapedia() 
        {
            picturePos = new Vector2(280, 280);
            ingamePos = new Vector2(280, 520);

            commander = new Sprite("Backgrounds/PreGame/commanders", picturePos);
            juggernaut = new Sprite("Backgrounds/PreGame/profileJuggernaut", picturePos);
            gunner = new Sprite("Backgrounds/PreGame/profileGunner", picturePos);
            medic = new Sprite("Backgrounds/PreGame/profileMedic", picturePos);
            specialist = new Sprite("Backgrounds/PreGame/profileSpecialist", picturePos);
            sniper = new Sprite("Backgrounds/PreGame/profileSniper", picturePos);
            bombardier = new Sprite("Backgrounds/PreGame/profileBombardier", picturePos);
            grunt = new Sprite("Backgrounds/PreGame/profileGrunt", picturePos);
            suicide = new Sprite("Backgrounds/PreGame/profileMeanie", picturePos);

            soldierInGame = new Sprite("Dots/dotAtlas", ingamePos);
            commanderInGame = new Sprite("Dots/commanderAtlas", ingamePos);

            unitMax = 2;
            soldierMax = 8;
            commanderMax = 4;

            currentUnitType = 0;
            currentSoldier = 0;
            currentCommander = 0;

            unitTypes = new String[unitMax];
            unitTypes[0] = "Soldiers";
            unitTypes[1] = "Commanders";

            soldierInfo = new Info[soldierMax];
            commanderInfo = new Info[commanderMax];

            for (int i = 0; i < soldierMax; i++)
            {
                soldierInfo[i] = new Info();
            }

            for (int i = 0; i < commanderMax; i++)
            {
                commanderInfo[i] = new Info();
            }

            #region Grunt
            soldierInfo[0].name = "Grunt";
            soldierInfo[0].image = grunt;
            soldierInfo[0].damage = "2";
            soldierInfo[0].health = "4";
            soldierInfo[0].vision = "3";
            soldierInfo[0].reload = "3";
            soldierInfo[0].description = "Grunts are the base unit of any dot\narmy and are well-rounded fighters.\nThey are the only dots on your team\nthat throw grenades and can help you\ncapture objectives. Strength, health,\nreload speed, and vision for these\nunits are all about average.";
#endregion

            #region Gunner
            soldierInfo[1].name = "Gunner";
            soldierInfo[1].image = gunner;
            soldierInfo[1].damage = "1";
            soldierInfo[1].health = "5";
            soldierInfo[1].vision = "3";
            soldierInfo[1].reload = "10";
            soldierInfo[1].description = "Gunners possess the ability to\ndevastate an enemy's ground forces\nwith a wave of their hand. They wield\na machine gun capable of high rates\nof fire. Although each bullet is much\nweaker, the Gunner is able to sustain\nfire and keep enemies suppressed.";
            #endregion

            #region Specialist
            soldierInfo[2].name = "Specialist";
            soldierInfo[2].image = specialist;
            soldierInfo[2].damage = "7";
            soldierInfo[2].health = "5";
            soldierInfo[2].vision = "3";
            soldierInfo[2].reload = "1";
            soldierInfo[2].description = "The Specialist is one of the most\ndangerous dots on the battlefield.\nTheir rocket launchers are sure to\nmake quick work of the opposition,\nas long as they have enough team\nsupport while they reload.";
            #endregion

            #region Bombardier
            soldierInfo[3].name = "Bombardier";
            soldierInfo[3].image = bombardier;
            soldierInfo[3].damage = "10";
            soldierInfo[3].health = "4";
            soldierInfo[3].vision = "7";
            soldierInfo[3].reload = "0";
            soldierInfo[3].description = "Bombardiers are small dots who\nhide away in corners and call in\nairstrikes. While some may think\nthey're cowardly, their teams are\nalways grateful for the added\nfirepower. They have no weapons to\ndefend themselves with.";
            #endregion

            #region Sniper
            soldierInfo[4].name = "Sniper";
            soldierInfo[4].image = sniper;
            soldierInfo[4].damage = "6";
            soldierInfo[4].health = "4";
            soldierInfo[4].vision = "10";
            soldierInfo[4].reload = "1";
            soldierInfo[4].description = "Methodical in their targeting,\nSnipers are able to kill high\npriority enemies from across\nthe map. Their huge vision range\nand high damage are assets while\nthey're safe. If they're put under\npressure, they'll run.";
            #endregion

            #region Juggernaut
            soldierInfo[5].name = "Juggernaut";
            soldierInfo[5].image = juggernaut;
            soldierInfo[5].damage = "0";
            soldierInfo[5].health = "10";
            soldierInfo[5].vision = "4";
            soldierInfo[5].reload = "0";
            soldierInfo[5].description = "Juggernauts are the most sultry of\nunits. Their riot shields are used\nto block incoming enemy bullets.\nKilling them is a difficult task,\nbut they won't fight back. They'll\nsimply deflect your shots. And\nlaugh at you.";
            #endregion

            #region Medic
            soldierInfo[6].name = "Medic";
            soldierInfo[6].image = medic;
            soldierInfo[6].damage = "5";
            soldierInfo[6].health = "4";
            soldierInfo[6].vision = "4";
            soldierInfo[6].reload = "3";
            soldierInfo[6].description = "The Medic is a gentle soul. Rather\nthan kill the enemy dots, Medics heal\ntheir allies. They are easily\ndistinguished on the battlefield by\ntheir cross capes. Medics aren't\narmed, so protect them if you want\nthem to live and sustain you.";
            #endregion

            #region Meanie
            soldierInfo[7].name = "???";
            soldierInfo[7].image = suicide;
            soldierInfo[7].damage = "10";
            soldierInfo[7].health = "???";
            soldierInfo[7].vision = "4";
            soldierInfo[7].reload = "0";
            soldierInfo[7].description = "Nobody is quite sure what these are\nor where they came from. These\nmenacing looking dots get stronger\nover time and will surely blow you\naway if you accidentally get too\nclose. Approach with caution.";
            #endregion

            #region Mustachio
            commanderInfo[0].name = "Mustachio";
            commanderInfo[0].image = commander;
            commanderInfo[0].damage = "10";
            commanderInfo[0].health = "9";
            commanderInfo[0].vision = "10";
            commanderInfo[0].reload = "9";
            commanderInfo[0].description = "Mustachio is the commander of the Red\nArmies. His special ability is to\nsummon a giant fireball that travels\na short distance in front of him,\ndestroying all enemy units in the way.\nFor dealing tons of damage, there is\nno better choice than him.";
            #endregion

            #region Aquoes
            commanderInfo[1].name = "Aquoes";
            commanderInfo[1].image = commander;
            commanderInfo[1].damage = "10";
            commanderInfo[1].health = "9";
            commanderInfo[1].vision = "10";
            commanderInfo[1].reload = "9";
            commanderInfo[1].description = "Aquoes commands the Blue Legions.\nHis specialty involves creating a\nlarge puddle directly in front of him.\nThis may not sound menacing, but\nall enemies inside of the puddle are\nslowed, and all allies healed. He can\ncontrol the tides of battle with it.";
            #endregion

            #region Terron
            commanderInfo[2].name = "Terron";
            commanderInfo[2].image = commander;
            commanderInfo[2].damage = "10";
            commanderInfo[2].health = "9";
            commanderInfo[2].vision = "10";
            commanderInfo[2].reload = "9";
            commanderInfo[2].description = "Terron is the commander of the Green\nPlatoon. His powerful grasp over the\nearth allows him to build boulders\non the battlefield. These are tough\nto destroy and many can exist at once.\nCreating a boulder on enemies squashes,\nhurts, and pushes them away.";
            #endregion

            #region Dian
            commanderInfo[3].name = "Dian";
            commanderInfo[3].image = commander;
            commanderInfo[3].damage = "10";
            commanderInfo[3].health = "9";
            commanderInfo[3].vision = "10";
            commanderInfo[3].reload = "9";
            commanderInfo[3].description = "Dian commands the Yellow Regiment. Her\nability lets her lay down a trail of\nlightning that speeds her up and hurts\nenemies who walk over it. Holding the\nability button allows Dian to lay a\nlonger trail. Her speed is great when\nyou are running out of time.";
            #endregion
        }

        public void init(TextureManager textures)
        {
            commander.LoadContent(textures);
            juggernaut.LoadContent(textures);
            gunner.LoadContent(textures);
            medic.LoadContent(textures);
            specialist.LoadContent(textures);
            sniper.LoadContent(textures);
            bombardier.LoadContent(textures);
            grunt.LoadContent(textures);
            suicide.LoadContent(textures);
            soldierInGame.LoadContent(textures);
            commanderInGame.LoadContent(textures);
        }

        public void changeUnitType(int amount)
        {
            if (currentUnitType == 0)
            {
                currentUnitType = 1;
            }
            else
            {
                currentUnitType = 0;
            }
        }

        public void changeUnit(int amount)
        {
            if (currentUnitType == 0)
            {
                changeSoldier(amount);
            }
            else
            {
                changeCommander(amount);
            }
        }

        private void changeSoldier(int amount)
        {
            int newIndex = currentSoldier + amount;

            if (newIndex > soldierMax - 1)
            {
                currentSoldier = 0;
            }
            else if (newIndex < 0)
            {
                currentSoldier = soldierMax - 1;
            }
            else
            {
                currentSoldier = newIndex;
            }
            soldierInGame.SetModeIndex(currentSoldier);
            soldierInGame.UpdateFrame();
        }

        private void changeCommander(int amount)
        {
            int newIndex = currentCommander + amount;

            if (newIndex > commanderMax - 1)
            {
                currentCommander = 0;
            }
            else if (newIndex < 0)
            {
                currentCommander = commanderMax - 1;
            }
            else
            {
                currentCommander = newIndex;
            }
            commander.SetFrameIndex(currentCommander);
            commander.UpdateFrame();
            commanderInGame.SetFrameIndex(currentCommander);
            commanderInGame.UpdateFrame();
        }

        public String getCurrentUnitType()
        {
            return unitTypes[currentUnitType];
        }

        public String getCurrentName()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].name;
            }
            return commanderInfo[currentCommander].name;
        }

        public String getCurrentDescription()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].description;
            }
            return commanderInfo[currentCommander].description;
        }

        public String getCurrentDamage()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].damage;
            }
            return commanderInfo[currentCommander].damage;
        }

        public String getCurrentVision()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].vision;
            }
            return commanderInfo[currentCommander].vision;
        }

        public String getCurrentHealth()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].health;
            }
            return commanderInfo[currentCommander].health;
        }

        public String getCurrentReload()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].reload;
            }
            return commanderInfo[currentCommander].reload;
        }

        public Sprite getCurrentImage()
        {
            if (currentUnitType == 0)
            {
                return soldierInfo[currentSoldier].image;
            }
            return commanderInfo[currentCommander].image;
        }

        public Sprite getCurrentIngameImage()
        {
            if (currentUnitType == 0)
            {
                return soldierInGame;
            }
            return commanderInGame;
        }
    }
}
