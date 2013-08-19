using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DotWars
{
    public enum MenuSelect
    {
        start,
        characterSelect,
        levelSelect,
        teamSelect
    }

    public class Menu : Level
    {
        #region Declarations

        private const double SECONDS_TO_WAIT_FOR_INPUT = 0.25;
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

        private Sprite[] commanderCards;
        private int[] commanderSlots;
        private int indexOfKing;

        private Sprite gametypeCards;
        private Sprite mapCards;
        private Sprite mapGametypeCards;

        private Sprite[] controllerCards;
        private Sprite[] teamCards;

        private String[] mapNames;
        private String[] gametypeNames;

        private Sprite testBack;

        private Sprite backButton;
        private Sprite startButton;
        private Sprite triggers;
        private Sprite triggersOnly;

        private int map;
        private int gametype;

        private PlayerIndex[] playerIndices;

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

            mapNames = new String[7];
            mapNames[0] = "Archipelago";
            mapNames[1] = "Relic";
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

            sounds.Play("DotWars", 0.9f, 0, 0, true);
        }

        public override void LoadContent(ContentManager cM)
        {
            //Set up sprites
            testBack = new Sprite("Backgrounds/Menu/menuBackground", new Vector2(624, 360));
            testBack.LoadContent(textures);

            backButton = new Sprite("Backgrounds/Menu/backButton", new Vector2(624, 360));
            startButton = new Sprite("Backgrounds/Menu/startButton", new Vector2(624, 360));
            backButton.LoadContent(textures);
            startButton.LoadContent(textures);

            triggers = new Sprite("Backgrounds/Menu/triggers", new Vector2(624, 360));
            triggersOnly = new Sprite("Backgrounds/Menu/triggersOnly", new Vector2(624, 360));
            triggers.LoadContent(textures);
            triggersOnly.LoadContent(textures);

            //StartScreen
            logo = new Sprite("Backgrounds/Menu/LogoScreen", new Vector2(624, 360));
            logo.LoadContent(textures);

            //PlayerScreen
            commanderCards = new Sprite[4];
            for (int i = 0; i < commanderCards.Length; i++)
            {
                commanderCards[i] = new Sprite("Backgrounds/Menu/CommanderCards", new Vector2(GetSizeOfLevel().X / 2 + (-2 + i) * 300 + 16, 153));
                commanderCards[i].LoadContent(textures);
                commanderCards[i].position += commanderCards[i].origin;
            }

            //GameScreen
            mapCards = new Sprite("Backgrounds/Menu/mapGametypeCard", new Vector2(624, 260));
            mapCards.LoadContent(textures);
            gametypeCards = new Sprite("Backgrounds/Menu/mapGametypeCard", new Vector2(624, 520));
            gametypeCards.LoadContent(textures);

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

            //sounds.Play("testMusic", 0.5f, 0, 0);
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
                            if (keyState.IsKeyDown(Keys.Escape) || theStates[c].IsButtonDown(Buttons.Back))
                            {
                                return null;
                            }
                            else if (keyState.IsKeyDown(Keys.Space) || theStates[c].IsButtonDown(Buttons.Start))
                            {
                                sounds.Play("confirm", 3, 0, 0, false);
                                stage = MenuSelect.characterSelect;
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

                    for (int c = 0; c < theStates.Length; c++)
                    {
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
                                else if (keyState.IsKeyDown(Keys.Left) || theStates[c].IsButtonDown(Buttons.DPadLeft))
                                {
                                    commanderCards[c].SetFrameIndex(3);
                                    ResetSingleCounter(c);
                                }
                                else if (keyState.IsKeyDown(Keys.Right) || theStates[c].IsButtonDown(Buttons.DPadRight))
                                {
                                    commanderCards[c].SetFrameIndex(4);
                                    ResetSingleCounter(c);
                                }
                                else if (keyState.IsKeyDown(Keys.Escape) ||
                                         theStates[((indexOfKing != -1) ? indexOfKing : c)].IsButtonDown(Buttons.Back))
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
                            else if ((theStates[c].IsButtonDown(Buttons.B) || keyState.IsKeyDown(Keys.B)) &&
                                     commanderCards[c].GetFrameIndex() != 0)
                            {
                                commanderSlots[commanderCards[c].GetFrameIndex() - 1] = -1;
                                commanderCards[c].SetModeIndex(0);
                                ResetSingleCounter(c);

                                //Find a new king if necessary
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
                                }
                            }
                        }
                        else
                        {
                            stageCounters[c] += gT.ElapsedGameTime.TotalSeconds;
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
                                    gametype = (gametype == 2 ? 3 : 2);
                                    break;
                                case 1:
                                    gametype = (gametype == 1 ? 4 : 1);
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
                        else if (theStates[indexOfKing].IsButtonDown(Buttons.Back))
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

                    //Update frames
                    mapCards.UpdateFrame();
                    gametypeCards.UpdateFrame();
                    break;
                case MenuSelect.teamSelect:
                    //Let players pick teams
                    for (int c = 0; c < theStates.Length; c++)
                    {
                        //Make sure they are playing
                        if (commanderCards[c].GetModeIndex() != 0)
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
                                            if (commanderCards[s].GetModeIndex() == 0 || controllerCards[s].GetFrameIndex() != 0)
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
            testBack.Draw(sB, Vector2.Zero, managers);
            switch (stage)
            {
                case MenuSelect.start:
                    logo.Draw(sB, Vector2.Zero, managers);
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
                    break;
                case MenuSelect.levelSelect:
                    triggers.Draw(sB, Vector2.Zero, managers);
                    startButton.Draw(sB, Vector2.Zero, managers);
                    backButton.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, "Game Select", new Vector2(624, 88), Color.Black, TextureManager.FontSizes.big, true);
                    mapCards.Draw(sB, Vector2.Zero, managers);
                    gametypeCards.Draw(sB, Vector2.Zero, managers);
                    textures.DrawString(sB, mapNames[map], new Vector2(624, 260), Color.White, TextureManager.FontSizes.small, true);
                    textures.DrawString(sB, gametypeNames[gametype], new Vector2(624, 512), Color.White, TextureManager.FontSizes.small, true);
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
                        if (commanderCards[c].GetModeIndex() != 0)
                        {
                            controllerCards[c].Draw(sB, Vector2.Zero, managers);
                        }
                    }
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
                    gametype = 2;
                    break;
                case 1:
                    gametype = 1;
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
                    theGametype = new Survival(theTeams, allCommanders, 8, 2);
                    break;
            }
        }

        private Level SetUpMaps()
        {
            //Set up Maps
            switch (map)
            {
                case 0:
                    return new PreGame(new Archipelago(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
                case 1:
                    return new PreGame(new Relic(theGametype, playerCommanders, textures, sounds), theGametype, playerIndices[indexOfKing], textures, sounds);
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