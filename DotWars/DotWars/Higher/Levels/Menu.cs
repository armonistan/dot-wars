using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DotWars.Higher.Levels;

namespace DotWars
{
    public enum MenuSelect
    {
        start,
        characterSelect,
        levelSelect,
        teamSelect,
        dotopedia,
        controls,
        credits,
        quit
    }

    public class Menu : Level
    {
        #region Declarations

        private Sprite background;

        private const double SECONDS_TO_WAIT_FOR_INPUT = 0.5;
        private readonly double[] stageCounters;

        private readonly GamePadState[] theStates;
        private readonly GamePadState[] oldStates;
        private KeyboardState keyState;
        private KeyboardState pastState;
        private MenuSelect stage;

        private Dictionary<Type, NPC.AffliationTypes> allCommanders;
        private Dictionary<Type, int> playerCommanders;
        private List<NPC.AffliationTypes> theTeams;
        private Gametype theGametype;

        private Sprite logo;
        private Sprite startOptionsBackground;
        private Sprite startTriggers;
        private string[] startOptions;
        private int startOptionInt;
        private float version;

        private Sprite[] commanderCards;
        private Sprite commanderButtonA;
        private Sprite commanderButtonB;
        private int[] commanderSlots;
        private int indexOfKing;

        private Sprite gametypeBackground;
        private Sprite mapBackground;
        private String[] mapNames;
        private String[] gametypeNames;

        private Sprite[] controllerCards;
        private Sprite[] teamCards;

        private Sprite backButton;
        private Sprite startButton;
        private Sprite triggers;
        private Sprite triggersOnly;

        private Sprite controller;

        private int map;
        private int gametype;

        private PlayerIndex[] playerIndices;

        private Sprite exitButtonA;
        private Sprite exitButtonB;

        private Sprite dotapediaBackground;
        private Dotapedia dotpedia;

        #endregion

        public Menu(TextureManager tM, AudioManager audio) :
            base(null, new Dictionary<Type, int>(), new Vector2(1248, 720), tM, audio)
        {
            stage = MenuSelect.start;
            stageCounters = new double[4];
            theStates = new GamePadState[4];
            oldStates = new GamePadState[4];

            playerIndices = new PlayerIndex[4];
            playerIndices[0] = PlayerIndex.One;
            playerIndices[1] = PlayerIndex.Two;
            playerIndices[2] = PlayerIndex.Three;
            playerIndices[3] = PlayerIndex.Four;

            for (int index = 0; index < playerIndices.Length; index++)
            {
                oldStates[index] = GamePad.GetState(playerIndices[index]);
            }
        }

        public override void Initialize()
        {
            //Set up variables for game setup
            playerCommanders = new Dictionary<Type, int>(4);
            allCommanders = new Dictionary<Type, NPC.AffliationTypes>(4);
            indexOfKing = -1;
            commanderSlots = new int[4];
            for (int i = 0; i < commanderSlots.Length; i++)
            {
                commanderSlots[i] = -1;
            }
            theTeams = new List<NPC.AffliationTypes>(5);

            //create dotapedia
            dotpedia = new Dotapedia();

            startOptions = new string[5];
            startOptions[0] = "Set-up Game";
            startOptions[1] = "Dot-o-pedia";
            startOptions[2] = "Controls";
            startOptions[3] = "Credits";
            startOptions[4] = "Exit";

            startOptionInt = 0;

            mapNames = new String[7];
            mapNames[0] = "Relic";
            mapNames[1] = "Archipelago";
            mapNames[2] = "Switch";
            mapNames[3] = "Faercrag";
            mapNames[4] = "Caged";
            mapNames[5] = "Zen Garden";
            mapNames[6] = "Plaza";

            gametypeNames = new String[6];
            gametypeNames[0] = "Assassins";
            gametypeNames[1] = "Deathmatch";
            gametypeNames[2] = "Conquest";
            gametypeNames[3] = "Assault";
            gametypeNames[4] = "Flag Capture";
            gametypeNames[5] = "Survival";

            map = 0;
            gametype = 0;

            version = 1.1f;

            sounds.Play("DotWars", 0.9f, 0, 0, true);
        }

        public override void LoadContent(ContentManager cM)
        {
            //Set up sprites
            background = new Sprite("Backgrounds/Menu/menuBackground", new Vector2(624, 360));
            background.LoadContent(textures);

            backButton = new Sprite("Backgrounds/Menu/backButton", new Vector2(624, 360));
            startButton = new Sprite("Backgrounds/Menu/startButton", new Vector2(624, 360));
            backButton.LoadContent(textures);
            startButton.LoadContent(textures);

            triggers = new Sprite("Backgrounds/Menu/triggers", new Vector2(624, 360));
            triggersOnly = new Sprite("Backgrounds/Menu/triggersOnly", new Vector2(624, 360));
            triggers.LoadContent(textures);
            triggersOnly.LoadContent(textures);

            //StartScreen
            logo = new Sprite("Backgrounds/Menu/LogoScreen", new Vector2(624, 200));
            startOptionsBackground = new Sprite("Backgrounds/Menu/menuSelector", new Vector2(624, 590));
            startTriggers = new Sprite("Backgrounds/Menu/triggers", new Vector2(624, 680));
            logo.LoadContent(textures);
            startOptionsBackground.LoadContent(textures);startTriggers.LoadContent(textures);

            //dotapedia
            dotapediaBackground = new Sprite("Backgrounds/Menu/dotapedia", new Vector2(624, 360));
            dotapediaBackground.LoadContent(textures);
            dotpedia.init(textures);

            //PlayerScreen
            commanderCards = new Sprite[4];
            for (int i = 0; i < commanderCards.Length; i++)
            {
                commanderCards[i] = new Sprite("Backgrounds/Menu/CommanderCards", new Vector2(GetSizeOfLevel().X / 2 + (-2 + i) * 300 + 16, 153));
                commanderCards[i].LoadContent(textures);
                commanderCards[i].position += commanderCards[i].origin;
            }
            commanderButtonA = new Sprite("Backgrounds/Menu/buttonA", new Vector2(310, 660));
            commanderButtonB = new Sprite("Backgrounds/Menu/buttonB", new Vector2(940, 660));
            commanderButtonA.LoadContent(textures);
            commanderButtonB.LoadContent(textures);

            //GameScreen
            mapBackground = new Sprite("Backgrounds/Menu/mapGametypeCard", new Vector2(624, 260));
            mapBackground.LoadContent(textures);
            gametypeBackground = new Sprite("Backgrounds/Menu/mapGametypeCard", new Vector2(624, 520));
            gametypeBackground.LoadContent(textures);

            //TeamScreen
            teamCards = new Sprite[2];
            for (int t = 0; t < teamCards.Length; t++)
            {
                teamCards[t] = new Sprite("Backgrounds/Menu/teamCards", new Vector2(187 + 874*t, 366));
                teamCards[t].LoadContent(textures);
            }
            controllerCards = new Sprite[4];
            for (int c = 0; c < controllerCards.Length; c++)
            {
                controllerCards[c] = new Sprite("Backgrounds/Menu/controllerCards", new Vector2(624, 90*c + 240));
                controllerCards[c].LoadContent(textures);
                controllerCards[c].SetModeIndex(c);
            }

            //Controls screen
            controller = new Sprite("HUD/pauseOverlay", DEFAUT_SCREEN_SIZE / 2);
            controller.LoadContent(textures);

            //Exit screen
            exitButtonA = new Sprite("Backgrounds/Menu/buttonA", new Vector2(100, 660));
            exitButtonB = new Sprite("Backgrounds/Menu/buttonB", new Vector2(1150, 660));
            exitButtonA.LoadContent(textures);
            exitButtonB.LoadContent(textures);
        }

        public override Level Update(GameTime gT)
        {
            sounds.Update();

            //Get keyboardstate for the pc version
            keyState = Keyboard.GetState();

            //Get the gamepadstates for each controller
            for (int index = 0; index < playerIndices.Length; index++)
            {
                theStates[index] = GamePad.GetState(playerIndices[index]);
            }

            switch (stage)
            {
                case MenuSelect.start:

                    #region Start Code

                    for (int c = 0; c < theStates.Length; c++)
                    {
                        if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                        {
                            if (keyState.IsKeyDown(Keys.Left) || theStates[c].IsButtonDown(Buttons.LeftTrigger))
                            {
                                startOptionInt--;

                                if (startOptionInt < 0)
                                {
                                    startOptionInt = startOptions.Length - 1;
                                }

                                ResetAllCounters();
                            }
                            else if (keyState.IsKeyDown(Keys.Right) || theStates[c].IsButtonDown(Buttons.RightTrigger))
                            {
                                startOptionInt++;

                                if (startOptionInt >= startOptions.Length)
                                {
                                    startOptionInt = 0;
                                }

                                ResetAllCounters();
                            }
                            else if (keyState.IsKeyDown(Keys.Space) || theStates[c].IsButtonDown(Buttons.Start))
                            {
                                sounds.Play("confirm", 3, 0, 0, false);

                                //TODO: Connect to other screens
                                switch (startOptionInt)
                                {
                                    case 0:
                                        stage = MenuSelect.characterSelect;
                                        break;
                                    case 1:
                                        stage = MenuSelect.dotopedia;
                                        break;
                                    case 2:
                                        stage = MenuSelect.controls;
                                        break;
                                    case 3:
                                        stage = MenuSelect.credits;
                                        break;
                                    case 4:
                                        stage = MenuSelect.quit;
                                        break;
                                }

                                ResetAllCounters();
                                break;
                            }
                        }
                        else
                        {
                            stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                        }
                    }

                    #endregion

                    break;
                case MenuSelect.characterSelect:

                    #region Player Choices

                    #region DEBUG: Controller override
                    if (keyState.IsKeyDown(Keys.Up))
                    {
                        commanderCards[0].SetFrameIndex(1);
                        commanderCards[0].SetModeIndex(2);
                        commanderSlots[0] = 0;
                        indexOfKing = 0;
                        sounds.Play("confirm", 3, 0, 0, false);
                        stage = MenuSelect.levelSelect;
                        ResetGametypeCards();
                        ResetAllCounters();
                        break;
                    }
                    else if (keyState.IsKeyDown(Keys.Down))
                    {
                        commanderCards[0].SetFrameIndex(2);
                        commanderCards[0].SetModeIndex(2);
                        commanderSlots[1] = 0;
                        indexOfKing = 0;
                        sounds.Play("confirm", 3, 0, 0, false);
                        stage = MenuSelect.levelSelect;
                        ResetGametypeCards();
                        ResetAllCounters();
                        break;
                    }
                    else if (keyState.IsKeyDown(Keys.Left))
                    {
                        commanderCards[0].SetFrameIndex(3);
                        commanderCards[0].SetModeIndex(2);
                        commanderSlots[2] = 0;
                        indexOfKing = 0;
                        sounds.Play("confirm", 3, 0, 0, false);
                        stage = MenuSelect.levelSelect;
                        ResetGametypeCards();
                        ResetAllCounters();
                        break;
                    }
                    else if (keyState.IsKeyDown(Keys.Right))
                    {
                        commanderCards[0].SetFrameIndex(4);
                        commanderCards[0].SetModeIndex(2);
                        commanderSlots[3] = 0;
                        indexOfKing = 0;
                        sounds.Play("confirm", 3, 0, 0, false);
                        stage = MenuSelect.levelSelect;
                        ResetGametypeCards();
                        ResetAllCounters();
                        break;
                    }
                    #endregion

                    for (int c = 0; c < theStates.Length; c++)
                    {
                        //Prevents controllers that aren't connected from playing
                        if (!theStates[c].IsConnected)
                        {
                            if (commanderCards[c].GetFrameIndex() > 0)
                            {
                                commanderSlots[commanderCards[c].GetFrameIndex() - 1] = -1;
                            }

                            commanderCards[c].SetModeIndex(1);
                            commanderCards[c].SetFrameIndex(0);

                            //Find a new king if necessary
                            FindNewKing(c);
                        }
                        else
                        {
                            if (commanderCards[c].GetModeIndex() == 1 && commanderCards[c].GetFrameIndex() == 0)
                            {
                                commanderCards[c].SetModeIndex(0);
                                commanderCards[c].SetFrameIndex(0);
                            }

                            if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                            {
                                if (commanderCards[c].GetModeIndex() == 0)
                                {
                                    if ((theStates[c].IsButtonDown(Buttons.A) || keyState.IsKeyDown(Keys.A)) &&
                                        commanderCards[c].GetFrameIndex() != 0)
                                    {
                                        if (commanderSlots[commanderCards[c].GetFrameIndex() - 1] == -1)
                                        {
                                            commanderSlots[commanderCards[c].GetFrameIndex() - 1] = c;

                                            sounds.Play("confirm", 3, 0, 0, false);
                                            if (indexOfKing == -1)
                                            {
                                                indexOfKing = c;
                                                commanderCards[c].SetModeIndex(2);
                                            }
                                            else
                                            {
                                                commanderCards[c].SetModeIndex(1);
                                            }
                                        }

                                        ResetSingleCounter(c);
                                    }
                                    else if (keyState.IsKeyDown(Keys.Up) || theStates[c].IsButtonDown(Buttons.DPadUp))
                                    {
                                        commanderCards[c].SetFrameIndex(1);
                                        ResetSingleCounter(c);
                                    }
                                    else if (keyState.IsKeyDown(Keys.Down) || theStates[c].IsButtonDown(Buttons.DPadDown))
                                    {
                                        commanderCards[c].SetFrameIndex(2);
                                        ResetSingleCounter(c);
                                    }
                                    else if (keyState.IsKeyDown(Keys.Left) ||
                                             theStates[c].IsButtonDown(Buttons.DPadLeft))
                                    {
                                        commanderCards[c].SetFrameIndex(3);
                                        ResetSingleCounter(c);
                                    }
                                    else if (keyState.IsKeyDown(Keys.Right) ||
                                             theStates[c].IsButtonDown(Buttons.DPadRight))
                                    {
                                        commanderCards[c].SetFrameIndex(4);
                                        ResetSingleCounter(c);
                                    }
                                    else if (keyState.IsKeyDown(Keys.Escape) ||
                                             theStates[((indexOfKing != -1) ? indexOfKing : c)].IsButtonDown
                                                 (Buttons.Back))
                                    {
                                        sounds.Play("return", 3, 0, 0, false);
                                        stage = MenuSelect.start;
                                        ResetAllCounters();
                                        for (int i = 0; i < commanderCards.Length; i++)
                                        {
                                            commanderCards[i].SetFrameIndex(0);
                                            commanderCards[i].SetModeIndex(0);
                                            commanderSlots[i] = -1;
                                            indexOfKing = -1;
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                    if ((theStates[c].IsButtonDown(Buttons.B) || keyState.IsKeyDown(Keys.B)) &&
                                        commanderCards[c].GetFrameIndex() != 0)
                                    {
                                        commanderSlots[commanderCards[c].GetFrameIndex() - 1] = -1;
                                        commanderCards[c].SetModeIndex(0);
                                        ResetSingleCounter(c);

                                        //Find a new king if necessary
                                        FindNewKing(c);
                                    }
                                    else if (keyState.IsKeyDown(Keys.Space) ||
                                             (indexOfKing != -1 && theStates[indexOfKing].IsButtonDown(Buttons.Start)))
                                    {
                                        sounds.Play("confirm", 3, 0, 0, false);
                                        stage = MenuSelect.levelSelect;
                                        ResetGametypeCards();
                                        ResetAllCounters();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                            }
                        }
                    }

                    #endregion

                    //Update Cards
                    foreach (Sprite cC in commanderCards)
                    {
                        cC.UpdateFrame();
                    }
                    break;
                case MenuSelect.levelSelect:
                    #region Level Selection

                    if (stageCounters[indexOfKing] > SECONDS_TO_WAIT_FOR_INPUT)
                    {
                        #region Map and Gametype Selection

                        if ((keyState.IsKeyDown(Keys.Left) && pastState.IsKeyUp(Keys.Left)) ||
                            theStates[indexOfKing].IsButtonDown(Buttons.LeftTrigger) && oldStates[indexOfKing].IsButtonUp(Buttons.LeftTrigger))
                        {
                            if (map > 0)
                            {
                                map--;
                            }
                            else
                            {
                                map = mapNames.Length - 1;
                            }
                            ResetGametypeCards();
                        }
                        else if ((keyState.IsKeyDown(Keys.Right) && pastState.IsKeyUp(Keys.Right)) ||
                                 theStates[indexOfKing].IsButtonDown(Buttons.RightTrigger) && oldStates[indexOfKing].IsButtonUp(Buttons.RightTrigger))
                        {
                            if (map < mapNames.Length - 1)
                            {
                                map++;
                            }
                            else
                            {
                                map = 0;
                            }
                            ResetGametypeCards();
                        }
                        else if ((keyState.IsKeyDown(Keys.Up) && pastState.IsKeyUp(Keys.Up)) ||
                                 (keyState.IsKeyDown(Keys.Down) && pastState.IsKeyUp(Keys.Down)) ||
                                 theStates[indexOfKing].IsButtonDown(Buttons.LeftShoulder) && oldStates[indexOfKing].IsButtonUp(Buttons.LeftShoulder) ||
                                 theStates[indexOfKing].IsButtonDown(Buttons.RightShoulder) && oldStates[indexOfKing].IsButtonUp(Buttons.RightShoulder))
                        {
                            switch (map)
                            {
                                case 0:
                                    gametype = (gametype == 1 ? 4 : 1);
                                    break;
                                case 1:
                                    gametype = (gametype == 2 ? 3 : 2);
                                    break;
                                case 2:
                                    gametype = (gametype == 0 ? 4 : 0);
                                    break;
                                case 3:
                                    gametype = (gametype == 2 ? 4 : 2);
                                    break;
                                case 4:
                                    gametype = (gametype == 0 ? 5 : 0);
                                    break;
                                case 5:
                                    gametype = (gametype == 1 ? 5 : 1);
                                    break;
                                case 6:
                                    gametype = (gametype == 3 ? 5 : 3);
                                    break;   
                            }

                            ResetSingleCounter(indexOfKing);
                        }
                            #endregion

                        else if (keyState.IsKeyDown(Keys.Space) ||
                                 theStates[indexOfKing].IsButtonDown(Buttons.Start))
                        {
                            if (map == 0 || !Guide.IsTrialMode)
                            {
                                sounds.Play("confirm", 3, 0, 0, false);
                                if (gametype == 0 || gametype == 5)
                                {
                                    //Set up teams
                                    SetUpFourTeams();
                                    SetUpGametypes();
                                    return SetUpMaps();
                                }
                                else
                                {
                                    stage = MenuSelect.teamSelect;
                                    ResetAllCounters();

                                    //Set up king so they automatically get their team
                                    controllerCards[indexOfKing].SetFrameIndex(1);
                                    teamCards[0].SetFrameIndex(commanderCards[indexOfKing].GetFrameIndex());
                                }
                            }
                        }
                        else if (theStates[indexOfKing].IsButtonDown(Buttons.Back) || keyState.IsKeyDown(Keys.Escape))
                        {
                            sounds.Play("return", 3, 0, 0, false);
                            stage = MenuSelect.characterSelect;
                            gametype = 0;
                            map = 0;
                            ResetAllCounters();
                            break;
                        }
                    }
                    else
                    {
                        stageCounters[indexOfKing] += gT.ElapsedGameTime.TotalSeconds;
                    }

                    #endregion

                    //Update frames
                    mapBackground.UpdateFrame();
                    gametypeBackground.UpdateFrame();
                    for (int c = 0; c < theStates.Length; c++)
                    {
                        controllerCards[c].SetModeIndex(commanderCards[c].GetFrameIndex() - 1);
                        Console.WriteLine(commanderCards[c].GetFrameIndex() - 1);
                    }
                    break;

                case MenuSelect.teamSelect:
                    #region Team Selection

                    for (int c = 0; c < theStates.Length; c++)
                    {
                        //Make sure they are playing
                        if (commanderCards[c].GetModeIndex() != 0 &&
                            commanderCards[c].GetFrameIndex() != 0)
                        {
                            if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                            {
                                #region Team Choosing

                                if (theStates[c].IsButtonDown(Buttons.LeftTrigger) || keyState.IsKeyDown(Keys.Left))
                                {
                                    if (controllerCards[c].GetFrameIndex() == 2)
                                    {
                                        controllerCards[c].SetFrameIndex(0);

                                        //Check for empty team
                                        if (NumControllersWithFrame(2) == 0)
                                        {
                                            teamCards[1].SetFrameIndex(0);
                                        }
                                        else if (NumControllersWithFrame(2) == 1)
                                        {
                                            //Find new color for team
                                            for (int k = 0; k < controllerCards.Length; k++)
                                            {
                                                if (commanderCards[k].GetModeIndex() != 0 &&
                                                    controllerCards[k].GetFrameIndex() == 2)
                                                {
                                                    teamCards[1].SetFrameIndex(commanderCards[k].GetFrameIndex());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Check to see if team is already set up
                                        if (teamCards[0].GetFrameIndex() == 0)
                                        {
                                            //If not, set it up
                                            teamCards[0].SetFrameIndex(commanderCards[c].GetFrameIndex());
                                            controllerCards[c].SetFrameIndex(1);
                                        }
                                        else
                                        {
                                            //Check to see if team is full
                                            //If not, move over
                                            if (NumControllersWithFrame(1) == 1)
                                            {
                                                controllerCards[c].SetFrameIndex(1);
                                            }
                                        }
                                    }

                                    ResetSingleCounter(c);
                                }
                                else if (theStates[c].IsButtonDown(Buttons.RightTrigger) || keyState.IsKeyDown(Keys.Right))
                                {
                                    if (controllerCards[c].GetFrameIndex() == 1)
                                    {
                                        controllerCards[c].SetFrameIndex(0);

                                        //Check for empty team
                                        if (NumControllersWithFrame(1) == 0)
                                        {
                                            teamCards[0].SetFrameIndex(0);
                                        }
                                        else if (NumControllersWithFrame(1) == 1)
                                        {
                                            //Find new color for team
                                            for (int k = 0; k < controllerCards.Length; k++)
                                            {
                                                if (commanderCards[k].GetModeIndex() != 0 &&
                                                    controllerCards[k].GetFrameIndex() == 1)
                                                {
                                                    teamCards[0].SetFrameIndex(commanderCards[k].GetFrameIndex());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Check to see if team is already set up
                                        if (teamCards[1].GetFrameIndex() == 0)
                                        {
                                            //If not, set it up
                                            teamCards[1].SetFrameIndex(commanderCards[c].GetFrameIndex());
                                            controllerCards[c].SetFrameIndex(2);
                                        }
                                        else
                                        {
                                            //Check to see if team is full
                                            //If not, move over
                                            if (NumControllersWithFrame(2) == 1)
                                            {
                                                controllerCards[c].SetFrameIndex(2);
                                            }
                                        }
                                    }

                                    ResetSingleCounter(c);
                                }

                                #endregion

                                //Check to see if king is ready to start game
                                if (c == indexOfKing)
                                {
                                    if (keyState.IsKeyDown(Keys.Space) || theStates[c].IsButtonDown(Buttons.Start))
                                    {
                                        //Check to see if teams are ready
                                        int numReady = 0;
                                        for (int s = 0; s < controllerCards.Length; s++)
                                        {
                                            if (commanderCards[s].GetModeIndex() == 1 && commanderCards[s].GetFrameIndex() == 0 ||
                                                commanderCards[s].GetModeIndex() == 0 ||
                                                controllerCards[s].GetFrameIndex() != 0)
                                            {
                                                numReady++;
                                            }
                                        }

                                        //If so, start game
                                        if (numReady == 4)
                                        {
                                            sounds.Play("confirm", 3, 0, 0, false);
                                            SetUpTwoTeams();
                                            SetUpGametypes();
                                            return SetUpMaps();
                                        }
                                    }
                                    else if (keyState.IsKeyDown(Keys.Escape) || theStates[c].IsButtonDown(Buttons.Back))
                                    {
                                        sounds.Play("return", 3, 0, 0, false);
                                        //Reset Team choice variables
                                        for (int k = 0; k < controllerCards.Length; k++)
                                        {
                                            controllerCards[k].SetFrameIndex(0);
                                        }
                                        for (int t = 0; t < teamCards.Length; t++)
                                        {
                                            teamCards[t].SetFrameIndex(0);
                                        }

                                        ResetAllCounters();
                                        stage = MenuSelect.levelSelect;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                            }
                        }
                    }
                    #endregion

                    #region Sprite Updates

                    //Update controller cards and team cards
                    foreach (Sprite cC in controllerCards)
                    {
                        cC.UpdateFrame();

                        //Make position appropriate for frame
                        switch (cC.GetFrameIndex())
                        {
                            case 0:
                                cC.position.X = DEFAUT_SCREEN_SIZE.X / 2 - cC.GetFrame().Width / 2;
                                break;
                            case 1:
                                cC.position.X = 100;
                                break;
                            case 2:
                                cC.position.X = 1000;
                                break;
                        }
                    }
                    foreach (Sprite tC in teamCards)
                    {
                        tC.UpdateFrame();
                    }

                    #endregion

                    break;

                case MenuSelect.dotopedia:
                    #region Dot-o-pedia functionality
                    if (keyState.IsKeyDown(Keys.Left) && pastState.IsKeyUp(Keys.Left))
                    {
                        dotpedia.changeUnitType(-1);
                    }
                    else if (keyState.IsKeyDown(Keys.Right) && pastState.IsKeyUp(Keys.Right))
                    {
                        dotpedia.changeUnitType(1);
                    }
                    else if (keyState.IsKeyDown(Keys.Down) && pastState.IsKeyUp(Keys.Down))
                    {
                        dotpedia.changeUnit(1);
                    }
                    else if (keyState.IsKeyDown(Keys.Up) && pastState.IsKeyUp(Keys.Up))
                    {
                        dotpedia.changeUnit(-1);
                    }

                    for (int c = 0; c < theStates.Length; c++)
                    {
                        if (theStates[c].IsButtonDown(Buttons.LeftTrigger) && oldStates[c].IsButtonUp(Buttons.LeftTrigger))
                        {
                            dotpedia.changeUnitType(-1);
                        }
                        else if (theStates[c].IsButtonDown(Buttons.RightTrigger) && oldStates[c].IsButtonUp(Buttons.RightTrigger))
                        {
                            dotpedia.changeUnitType(1);
                        }
                        else if (theStates[c].IsButtonDown(Buttons.RightShoulder) && oldStates[c].IsButtonUp(Buttons.RightShoulder))
                        {
                            dotpedia.changeUnit(1);
                        }
                        else if (theStates[c].IsButtonDown(Buttons.LeftShoulder) && oldStates[c].IsButtonUp(Buttons.LeftShoulder))
                        {
                            dotpedia.changeUnit(-1);
                        }


                        if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                        {
                            if (keyState.IsKeyDown(Keys.Escape) || theStates[c].IsButtonDown(Buttons.Back))
                            {
                                sounds.Play("return", 3, 0, 0, false);
                                stage = MenuSelect.start;
                                ResetAllCounters();
                            }
                        }
                        else
                        {
                            stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                        }
                    }
                    #endregion
                    break;

                case MenuSelect.controls:
                    #region Controls functionality
                    for (int c = 0; c < theStates.Length; c++)
                    {
                        if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                        {
                            if (keyState.IsKeyDown(Keys.Escape) || theStates[c].IsButtonDown(Buttons.Back))
                            {
                                sounds.Play("return", 3, 0, 0, false);
                                stage = MenuSelect.start;
                                ResetAllCounters();
                            }
                        }
                        else
                        {
                            stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                        }
                    }
                    #endregion
                    break;

                case MenuSelect.credits:
                    #region Credits functionality
                    for (int c = 0; c < theStates.Length; c++)
                    {
                        if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                        {
                            if (keyState.IsKeyDown(Keys.Escape) || theStates[c].IsButtonDown(Buttons.Back))
                            {
                                sounds.Play("return", 3, 0, 0, false);
                                stage = MenuSelect.start;
                                ResetAllCounters();
                            }
                        }
                        else
                        {
                            stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                        }
                    }
                    #endregion
                    break;

                case MenuSelect.quit:
                    #region Quit Functionality
                    for (int c = 0; c < theStates.Length; c++)
                    {
                        if (stageCounters[c] > SECONDS_TO_WAIT_FOR_INPUT)
                        {
                            if (keyState.IsKeyDown(Keys.A) || theStates[c].IsButtonDown(Buttons.A))
                            {
                                return null;
                            }
                            else if (keyState.IsKeyDown(Keys.B) || theStates[c].IsButtonDown(Buttons.B))
                            {
                                sounds.Play("return", 3, 0, 0, false);
                                stage = MenuSelect.start;
                                ResetAllCounters();
                            }
                        }
                        else
                        {
                            stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
                        }
                    }
                    #endregion
                    break;
            }
            pastState = keyState;

            for (int i = 0; i < theStates.Length; i++)
            {
                oldStates[i] = theStates[i];
            }

            return this;
        }


        public override void Draw(SpriteBatch sB, GraphicsDeviceManager gM, bool drawHUD)
        {
            sB.Begin();
            background.Draw(sB, Vector2.Zero, managers);
            switch (stage)
            {
                case MenuSelect.start:
                    logo.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, version + "", new Vector2(950, 150), Color.White, TextureManager.FontSizes.small, false);
                    startOptionsBackground.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, startOptions[startOptionInt], startOptionsBackground.GetOriginPosition(), Color.White, TextureManager.FontSizes.small, true);
                    startTriggers.Draw(sB, Vector2.Zero, managers);
                    startButton.Draw(sB, Vector2.Zero, managers);
                    break;

                case MenuSelect.characterSelect:
                    textures.DrawString(sB, "Commander Select", new Vector2(624, 88), Color.Black, TextureManager.FontSizes.big, true);//TODO: Make better
                    startButton.Draw(sB, Vector2.Zero, managers);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    foreach (Sprite cC in commanderCards)
                    {
                        cC.Draw(sB, Vector2.Zero, managers);
                    }

                    DrawCommanderSelectHelp(sB);
                    break;

                case MenuSelect.levelSelect:
                    triggers.Draw(sB, Vector2.Zero, managers);
                    startButton.Draw(sB, Vector2.Zero, managers);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, "Game Select", new Vector2(624, 88), Color.Black, TextureManager.FontSizes.big, true);
                    mapBackground.Draw(sB, Vector2.Zero, managers);
                    gametypeBackground.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, mapNames[map], new Vector2(624, 260), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, gametypeNames[gametype], new Vector2(624, 512), Color.White, TextureManager.FontSizes.small, true);

                    //If it is trial mode and the map is not Relic
                    if (Guide.IsTrialMode && map != 0)
                    {
                        textures.DrawString(sB, "Map not available in trial version", new Vector2(624, 390), Color.White, TextureManager.FontSizes.small, true);
                    }
                    break;

                case MenuSelect.teamSelect:
                    triggersOnly.Draw(sB, Vector2.Zero, managers);
                    startButton.Draw(sB, Vector2.Zero, managers);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, "Team Select", new Vector2(624, 88), Color.Black, TextureManager.FontSizes.big, true);

                    foreach (Sprite tC in teamCards)
                    {
                        tC.Draw(sB, Vector2.Zero, managers);
                    }

                    for (int c = 0; c < controllerCards.Length; c++)
                    {
                        if (commanderCards[c].GetModeIndex() != 0 && commanderCards[c].GetFrameIndex() != 0)
                        {
                            controllerCards[c].Draw(sB, Vector2.Zero, managers);
                            textures.DrawString(sB, (c+1) + "", controllerCards[c].position + new Vector2(controllerCards[c].GetFrame().Width / 2,
                                (controllerCards[c].GetFrame().Height / 2) - 8), Color.Black, TextureManager.FontSizes.tiny, true);
                        }
                    }
                    break;

                case MenuSelect.dotopedia:
                    dotapediaBackground.Draw(sB, Vector2.Zero, managers);
                    dotpedia.getCurrentImage().Draw(sB, Vector2.Zero, managers);
                    dotpedia.getCurrentIngameImage().Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, "In-Game", new Vector2(280, 460), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentUnitType(), new Vector2(274, 94), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentName(), new Vector2(834, 94), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentDescription(), new Vector2(850, 383), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentDamage(), new Vector2(694, 184), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentHealth(), new Vector2(834, 184), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentReload(), new Vector2(960, 184), Color.Black, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, dotpedia.getCurrentVision(), new Vector2(1090, 184), Color.Black, TextureManager.FontSizes.small, true);
                    //textures.DrawString(sB, "There are no dots in this game.", DEFAUT_SCREEN_SIZE / 2, Color.White, TextureManager.FontSizes.small, true);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    break;

                case MenuSelect.controls:
                    controller.Draw(sB, Vector2.Zero, managers);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    break;

                case MenuSelect.credits:
                    textures.DrawString(sB, "Credits", new Vector2(624, 88), Color.Black, TextureManager.FontSizes.big, true);

                    Vector2 creditsStartPos = DEFAUT_SCREEN_SIZE/2 + new Vector2(0, -100);

                    textures.DrawString(sB, "Emberware Team:", creditsStartPos, Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "Armon Nayeraini", creditsStartPos + new Vector2(0, 48), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "Daniel Pumford", creditsStartPos + new Vector2(0, 96), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "David \"Steak\" Campbell", creditsStartPos + new Vector2(0, 144), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "Riley Turben", creditsStartPos + new Vector2(0, 192), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "Sound Credits:", creditsStartPos + new Vector2(0, 288), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "www.emberware.com/index.php?page=attributions.html", creditsStartPos + new Vector2(0, 336), Color.White, TextureManager.FontSizes.small, true);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    break;

                case MenuSelect.quit:
                    textures.DrawString(sB, "Are you sure you want to quit?", DEFAUT_SCREEN_SIZE / 2, Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "to confirm", new Vector2(230, 680), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, "to cancel", new Vector2(1040, 680), Color.White, TextureManager.FontSizes.small, true);
                    exitButtonA.Draw(sB, Vector2.Zero, managers);
                    exitButtonB.Draw(sB, Vector2.Zero, managers);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            sB.End();
        }

        private int NumControllersWithFrame(int i)
        {
            int tempNum = 0;
            for (int c = 0; c < controllerCards.Length; c++)
            {
                if (controllerCards[c].GetFrameIndex() == i)
                {
                    tempNum++;
                }
            }

            return tempNum;
        }

        private void DrawCommanderSelectHelp(SpriteBatch sB)
        {
            bool pressA = false;
            bool pressB = false;

            for (int c = 0; c < commanderCards.Length; c++)
            {
                switch (commanderCards[c].GetModeIndex())
                {
                    case 0:
                        pressA = pressA || commanderCards[c].GetFrameIndex() != 0;
                        break;
                    case 1:
                    case 2:
                        pressB = pressB || commanderCards[c].GetFrameIndex() != 0;
                        break;
                    default:
                        break;
                }
            }

            Vector2 middlePosition = new Vector2(DEFAUT_SCREEN_SIZE.X/2, 670);

            if (pressA)
            {   
                commanderButtonA.Draw(sB, Vector2.Zero, managers);
                textures.DrawString(sB, "lock in", middlePosition + new Vector2(-220, 0), Color.White, TextureManager.FontSizes.small, true);
            }

            if (pressB)
            {   
                commanderButtonB.Draw(sB, Vector2.Zero, managers);
                textures.DrawString(sB, "back out", middlePosition + new Vector2(210, 0), Color.White, TextureManager.FontSizes.small, true);
            }
        }

        private void FindNewKing(int c)
        {
            if (indexOfKing == c)
            {
                indexOfKing = -1;

                for (int k = 0; k < commanderSlots.Length; k++)
                {
                    if (commanderSlots[k] != -1)
                    {
                        indexOfKing = commanderSlots[k];
                    }
                }

                if (indexOfKing != -1)
                {
                    commanderCards[indexOfKing].SetModeIndex(2);
                }
            }
        }

        private void ResetSingleCounter(int i)
        {
            stageCounters[i] = 0;
        }

        private void ResetAllCounters()
        {
            for (int c = 0; c < stageCounters.Length; c++)
            {
                ResetSingleCounter(c);
            }
        }

        private void ResetGametypeCards()
        {
            switch (map)
            {
                case 0:
                    gametype = 1;
                    break;
                case 1:
                    gametype = 2;
                    break;
                case 2:
                    gametype = 0;
                    break;
                case 3:
                    gametype = 4;
                    break;
                case 4:
                    gametype = 5;
                    break;
                case 5:
                    gametype = 1;
                    break;
                case 6:
                    gametype = 3;
                    break;
            }
            
        }

        private void SetUpFourTeams()
        {
            for (int c = 0; c < commanderSlots.Length; c++)
            {
                int tempIndex = -1;

                for (int k = 0; k < commanderSlots.Length; k++)
                {
                    if (commanderSlots[k] == c)
                    {
                        tempIndex = k;
                    }
                }

                if (tempIndex != -1)
                {
                    switch (tempIndex)
                    {
                        case 0:
                            playerCommanders.Add(typeof (RedPlayerCommander), c);
                            allCommanders.Add(typeof (RedPlayerCommander),
                                              NPC.AffliationTypes.red);
                            break;
                        case 1:
                            playerCommanders.Add(typeof (BluePlayerCommander), c);
                            allCommanders.Add(typeof (BluePlayerCommander),
                                              NPC.AffliationTypes.blue);
                            break;
                        case 2:
                            playerCommanders.Add(typeof (GreenPlayerCommander), c);
                            allCommanders.Add(typeof (GreenPlayerCommander),
                                              NPC.AffliationTypes.green);
                            break;
                        case 3:
                            playerCommanders.Add(typeof (YellowPlayerCommander), c);
                            allCommanders.Add(typeof (YellowPlayerCommander),
                                              NPC.AffliationTypes.yellow);
                            break;
                    }
                }
            }

            //Set up non-players
            for (int c = 0; c < commanderSlots.Length; c++)
            {
                if (commanderSlots[c] == -1)
                {
                    switch (c)
                    {
                        case 0:
                            allCommanders.Add(typeof (RedCommander), NPC.AffliationTypes.red);
                            break;
                        case 1:
                            allCommanders.Add(typeof (BlueCommander), NPC.AffliationTypes.blue);
                            break;
                        case 2:
                            allCommanders.Add(typeof (GreenCommander), NPC.AffliationTypes.green);
                            break;
                        case 3:
                            allCommanders.Add(typeof (YellowCommander), NPC.AffliationTypes.yellow);
                            break;
                    }
                }
            }

            theTeams.Add(NPC.AffliationTypes.red);
            theTeams.Add(NPC.AffliationTypes.blue);
            theTeams.Add(NPC.AffliationTypes.green);
            theTeams.Add(NPC.AffliationTypes.yellow);
            theTeams.Add(NPC.AffliationTypes.black);
        }

        private void SetUpTwoTeams()
        {
            //Used for making the NPCs
            var numTeam = new int[2];
            numTeam[0] = 0;
            numTeam[1] = 0;

            
            //TODO: Fix team creation problems
            for (int t = 0; t < teamCards.Length; t++)
            {
                //If team is not chosen, pick one
                if (teamCards[t].GetFrameIndex() == 0)
                {
                    var reallyTempRandom = new Random();
                    List<Int32> possibleTeams = new List<Int32>(4);
                    List<Int32> illegalTeams = new List<int>(4);

                    for (int c = 0; c < controllerCards.Length; c++)
                    {
                        if (controllerCards[c].GetFrameIndex() != 0)
                        {
                            illegalTeams.Add(commanderCards[c].GetFrameIndex());
                        }
                    }

                    for (int n = 1; n <= commanderCards.Length; n++)
                    {
                        if (!illegalTeams.Contains(n))
                        {
                            possibleTeams.Add(n);
                        }
                    }

                        theTeams.Add(GetAffiliation(possibleTeams[reallyTempRandom.Next(possibleTeams.Count)]));
                }
                else
                {
                    theTeams.Add(GetAffiliation(teamCards[t].GetFrameIndex()));
                }
            }

            //Add players
            for (int c = 0; c < commanderSlots.Length; c++)
            {
                int tempIndex = -1;

                for (int k = 0; k < commanderSlots.Length; k++)
                {
                    if (commanderSlots[k] == c)
                    {
                        tempIndex = k;
                    }
                }

                if (tempIndex != -1)
                {
                    playerCommanders.Add(GetCommanderType(true, tempIndex), c);
                    allCommanders.Add(GetCommanderType(true, tempIndex),
                                      theTeams[controllerCards[c].GetFrameIndex() - 1]);

                    //Update team count
                    numTeam[controllerCards[c].GetFrameIndex() - 1]++;
                }
            }

            //Set up non-players
            //First, add NPC that is on other team in case it is empty
            for (int t = 0; t < teamCards.Length; t++)
            {
                if (teamCards[t].GetFrameIndex() == 0)
                {
                    allCommanders.Add(GetCommanderType(false, NPC.GetTeam(theTeams[t])), theTeams[t]);
                    numTeam[t]++;
                }
            }

            for (int c = 0; c < commanderSlots.Length; c++)
            {
                if (commanderSlots[c] == -1 && GetAffiliation(c + 1) != theTeams[0] &&
                    GetAffiliation(c + 1) != theTeams[1])
                {
                    if (numTeam[0] >= 2)
                    {
                        allCommanders.Add(GetCommanderType(false, c), theTeams[1]);
                        numTeam[1]++;
                    }
                    else if (numTeam[1] >= 2)
                    {
                        allCommanders.Add(GetCommanderType(false, c), theTeams[0]);
                        numTeam[0]++;
                    }
                    else
                    {
                        var reallyTempRandom = new Random();
                        int tempTeam = reallyTempRandom.Next(2);
                        allCommanders.Add(GetCommanderType(false, c), theTeams[tempTeam]);
                        numTeam[tempTeam]++;
                    }
                }
            }
        }

        private void SetUpGametypes()
        {
            //Set up gametype
            switch (gametype)
            {
                case 0:
                    theGametype = new Assasssins(theTeams, allCommanders, 2);
                    break;
                case 1:
                    theGametype = new Deathmatch(theTeams, allCommanders, 8, 2);
                    break;
                case 2:
                    theGametype = new Conquest(theTeams, allCommanders, 8, 2);
                    break;
                case 3:
                    theGametype = new Assault(theTeams, allCommanders, 8, theTeams[0], theTeams[1], 2);
                    break;
                case 4:
                    theGametype = new CaptureTheFlag(theTeams, allCommanders, 8, 2);
                    break;
                case 5:
                    theGametype = new Survival(theTeams, allCommanders, 4, 2);
                    break;
            }
        }

        private Level SetUpMaps()
        {
            //Set up Maps
            switch (map)
            {
                case 0:
                    return new PreGame(new Relic(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                case 1:
                    return new PreGame(new Archipelago(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                    case 2:
                    return new PreGame(new Switch(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                case 3:
                    return new PreGame(new Faercrag(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                case 4:
                    return new PreGame(new Caged(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                case 5:
                    return new PreGame(new ZenGarden(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                case 6:
                    return new PreGame(new Plaza(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                default:
                    return this;
            }
        }

        private NPC.AffliationTypes GetAffiliation(int i)
        {
            switch (i)
            {
                case 1:
                    return NPC.AffliationTypes.red;
                case 2:
                    return NPC.AffliationTypes.blue;
                case 3:
                    return NPC.AffliationTypes.green;
                case 4:
                    return NPC.AffliationTypes.yellow;
                default:
                    return NPC.AffliationTypes.black;
            }
        }

        private Type GetCommanderType(bool p, int i)
        {
            switch (i)
            {
                case 0:
                    return (p) ? typeof (RedPlayerCommander) : typeof (RedCommander);
                case 1:
                    return (p) ? typeof (BluePlayerCommander) : typeof (BlueCommander);
                case 2:
                    return (p) ? typeof (GreenPlayerCommander) : typeof (GreenCommander);
                case 3:
                    return (p) ? typeof (YellowPlayerCommander) : typeof (YellowCommander);
                default:
                    return typeof (Commander);
            }
        }
    }
}