#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Window;
using SFMLStart;
using SFMLStart.Data;
using TimeDRODPOF.TDComponents;
using TimeDRODPOF.TDEditor;
using TimeDRODPOF.TDLib;
using TimeDRODPOF.TDStructure;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDGame
{
    public class TDGGame : Game
    {
        private readonly TDCommon _common;
        private readonly TDGUndoValues _undoValues = new TDGUndoValues();
        private int _battleFirstMove, _battleSecondMove;
        private TDSControl _control;
        private int _currentTurn;
        private TDGInstance _instance;
        private bool _isLastBattleInputTwo;
        private bool _levelSoundPlayed, _playerAlive;

        public TDGGame(GameWindow mGameWindow)
        {
            _common = new TDCommon(mGameWindow);

            OnUpdate += mFrameTime => _common.UpdatePositions();
            OnDrawBeforeCamera += _common.GUI.DrawBackground;
            OnDrawAfterCamera += DrawEntities;
            OnDrawAfterDefault += DrawGUI;
            InitializeInputs();
        }

        public TDEEditor Editor { private get; set; }
        public bool IsLevelClear { get; private set; }
        public int LastInput { get; private set; }

        private void InitializeInputs()
        {
            const int delay = 8;
            const float panspeed = 9;
            const float zoominspeed = 1.05f;
            const float zoomoutspeed = 0.95f;

            Bind("n", delay, () => NextTurn(0), null, new KeyCombination(Keyboard.Key.W));
            Bind("ne", delay, () => NextTurn(1), null, new KeyCombination(Keyboard.Key.E));
            Bind("e", delay, () => NextTurn(2), null, new KeyCombination(Keyboard.Key.D));
            Bind("se", delay, () => NextTurn(3), null, new KeyCombination(Keyboard.Key.C));
            Bind("s", delay, () => NextTurn(4), null, new KeyCombination(Keyboard.Key.S));
            Bind("sw", delay, () => NextTurn(5), null, new KeyCombination(Keyboard.Key.Z));
            Bind("w", delay, () => NextTurn(6), null, new KeyCombination(Keyboard.Key.A));
            Bind("nw", delay, () => NextTurn(7), null, new KeyCombination(Keyboard.Key.Q));
            Bind("swingcw", delay + 4, () => NextTurn(8), null, new KeyCombination(Keyboard.Key.Left));
            Bind("swingccw", delay + 4, () => NextTurn(9), null, new KeyCombination(Keyboard.Key.Right));
            Bind("wait", delay, () => NextTurn(-1), null, new KeyCombination(Keyboard.Key.LShift));

            Bind("debugbattlekey", 0, () =>
                                      {
                                          var move = _battleFirstMove;
                                          if (_isLastBattleInputTwo) move = _battleSecondMove;
                                          NextTurn(move);
                                      }, null, new KeyCombination(Keyboard.Key.LControl));

            Bind("debugundokey", 15, Undo, null, new KeyCombination(Keyboard.Key.Back));

            Bind("pan_n", 0, () => GameWindow.Camera.Move(new Vector2f(0, -panspeed)), null, new KeyCombination(Keyboard.Key.I));
            Bind("pan_s", 0, () => GameWindow.Camera.Move(new Vector2f(0, panspeed)), null, new KeyCombination(Keyboard.Key.K));
            Bind("pan_e", 0, () => GameWindow.Camera.Move(new Vector2f(panspeed, 0)), null, new KeyCombination(Keyboard.Key.L));
            Bind("pan_w", 0, () => GameWindow.Camera.Move(new Vector2f(-panspeed, 0)), null, new KeyCombination(Keyboard.Key.J));

            Bind("zoom_in", 0, () => GameWindow.Camera.Zoom(zoominspeed), null, new KeyCombination(Keyboard.Key.N));
            Bind("zoom_out", 0, () => GameWindow.Camera.Zoom(zoomoutspeed), null, new KeyCombination(Keyboard.Key.M));

            Bind("reset", 20, () =>
                              {
                                  _undoValues.Inputs.Clear();
                                  Reset();
                              }
                 , null, new KeyCombination(Keyboard.Key.R));
            Bind("exit", 0, () => Environment.Exit(0), null, new KeyCombination(Keyboard.Key.Escape));
            Bind("switchtoeditor", 0, () => GameWindow.SetGame(Editor), null, new KeyCombination(Keyboard.Key.F5));
            Bind("loadfromfiledebug", 60, LoadFromFile, null, new KeyCombination(Keyboard.Key.Period));
        }

        private void LoadFromFile()
        {
            var holdNames = new DirectoryInfo(Environment.CurrentDirectory).GetFiles().Where(x => x.Extension.EndsWith("hold")).Select(fileInfo => fileInfo.Name).ToList();
            var holdIndex = TDEUtils.InputListBox("Load hold", holdNames.ToArray());
            if (holdIndex == -1) return;
            var holdName = holdNames[holdIndex];
            _control = TDELevelIO.LoadFromFile(holdName);

            _control.CurrentLevel = _control.CurrentHold.Levels.First();

            StartLevel();
        }

        private void StartLevel()
        {
            _levelSoundPlayed = false;
            CheckLevelClear(); // Check if level is clear and play sound if needed

            TDEEntity firstStartPoint = null;

            // Set rooms clear or not clear (check if there are required enemies)
            foreach (var keyValuePair in _control.CurrentLevel.Rooms)
                if (CreateInstance(keyValuePair.Value, true).IsClear()) keyValuePair.Value.IsClear = true;

            // Find a start point in every room (get the first and ignore the others)
            foreach (var keyValuePair in _control.CurrentLevel.Rooms)
            {
                var room = keyValuePair.Value;
                var startPoints = room.TileManager.Entities.Where(x => x.Outline.UID == -1);
                firstStartPoint = startPoints.FirstOrDefault();

                if (firstStartPoint == null) continue;

                _control.CurrentRoom = room;
                _undoValues.WasRoomClear = _control.CurrentRoom.IsClear;
                _undoValues.ClearTurns = 0;
                break;
            }

            // If no start point was found, return and set structurecontrol to null (you must load an hold again)
            if (firstStartPoint == null)
            {
                _control = null;
                return;
            }

            // Set field and create a player at the start point
            SetInstance(CreateInstance(_control.CurrentRoom), firstStartPoint.Tile.X, firstStartPoint.Tile.Y, (int) firstStartPoint.Parameters[0].Value);

            RefreshUndoValues();
        }

        private void RefreshUndoValues()
        {
            // fix broken undo
            _undoValues.WasRoomClear = _control.CurrentRoom.IsClear;
            _undoValues.ClearTurns = 0;
            _undoValues.Inputs = new List<int>(); // an extra undo is added after this, fix 
        }

        private void SetInstance(TDGInstance mInstance, int mPlayerX, int mPlayerY, int mPlayerDirection)
        {
            _instance = mInstance;
            _instance.CreatePlayer(mPlayerX, mPlayerY, mPlayerDirection);
            _instance.RunLoadChecks();

            // Starting variables
            _currentTurn = 0;
            _playerAlive = true;


            // Reset and set undo stuff           
            _undoValues.PlayerStartX = mPlayerX;
            _undoValues.PlayerStartY = mPlayerY;
            _undoValues.PlayerStartDirection = mPlayerDirection;
        }
        private void RefreshGFX(int mWidth, int mHeight)
        {
            _common.RefreshTexture(mWidth, mHeight);
            _common.CenterCamera();
        }

        public void RoomSwitchMovement(Entity mPlayer, int mX, int mY)
        {
            int roomX = _control.CurrentRoom.X, roomY = _control.CurrentRoom.Y;
            int targetRoomX = roomX, targetRoomY = roomY;
            int spawnX = mX, spawnY = mY;

            if (mX < 0)
            {
                targetRoomX--;
                spawnX = _control.CurrentLevel.RoomWidth - 1;
            }
            else if (mX > _control.CurrentLevel.RoomWidth - 1)
            {
                targetRoomX++;
                spawnX = 0;
            }

            if (mY < 0)
            {
                targetRoomY--;
                spawnY = _control.CurrentLevel.RoomHeight - 1;
            }
            else if (mY > _control.CurrentLevel.RoomHeight - 1)
            {
                targetRoomY++;
                spawnY = 0;
            }

            var targetRoom = _control.CurrentLevel.GetRoom(targetRoomX, targetRoomY);
            if (targetRoom == null) return;

            var movementComponent = mPlayer.GetComponent<TDCMovement>();
            var oldRoom = _control.CurrentRoom;
            _control.CurrentRoom = targetRoom;

            var fieldInstance = CreateInstance(targetRoom, true);

            if (!fieldInstance.IsRoomSwitchAllowed(spawnX, spawnY, movementComponent))
            {
                _control.CurrentRoom = oldRoom;
                return;
            }

            // checks if level is clear only on room switching
            CheckLevelClear();

            // fix weapon sheating and changing!
            SetInstance(CreateInstance(targetRoom), spawnX, spawnY, mPlayer.GetComponent<TDCDirection>().Direction);

            RefreshUndoValues();
            //// fix broken undo
            //_undoValues.WasRoomClear = _control.CurrentRoom.IsClear;
            //_undoValues.ClearTurns = 0;
            //_undoValues.Inputs = new List<int>(); // an extra undo is added after this, fix 
        }

        private void CheckLevelClear()
        {
            IsLevelClear = _control.CurrentLevel.IsClear();
            if (!IsLevelClear || _levelSoundPlayed) return;
            TDLSounds.Play("SoundLevelComplete");
            _levelSoundPlayed = true;
        }

        private void Undo()
        {
            if (_undoValues.Inputs == null || _undoValues.Inputs.Count < 1) return;
            _undoValues.Inputs.RemoveAt(_undoValues.Inputs.Count - 1);

            TDLSounds.SoundsEnabled = false;

            Reset();

            _undoValues.ClearTurns = 0;
            var clearTurns = 0;

            foreach (var move in _undoValues.Inputs)
            {
                NextTurn(move, false);
                clearTurns++;
                if (!_undoValues.WasRoomClear && _undoValues.ClearTurns > 0 && clearTurns >= _undoValues.ClearTurns)
                    _control.CurrentRoom.IsClear = true;
            }

            TDLSounds.SoundsEnabled = true;
        }
        private void Reset()
        {
            if (_control.CurrentRoom == null) return;
            _control.CurrentRoom.IsClear = _undoValues.WasRoomClear; // this is wrong somehow (maybe hurr durr) 
            SetInstance(CreateInstance(_control.CurrentRoom), _undoValues.PlayerStartX, _undoValues.PlayerStartY, _undoValues.PlayerStartDirection);
        }

        private void DrawEntities()
        {
            if (_instance == null) return;

            _common.ClearTexture();

            _instance.Draw(_common.GameTexture);
            _common.GameTexture.Draw(_common.SelectionSprite);
            _common.GameTexture.Display();

            GameWindow.RenderWindow.Draw(_common.GameSprite);
        }
        private void DrawGUI()
        {
            var gameGUI = _common.GUI;

            if (_control != null)
            {
                if (_control.CurrentHold != null) gameGUI.HoldName = _control.CurrentHold.Name;
                if (_control.CurrentLevel != null) gameGUI.LevelName = _control.CurrentLevel.Name;
                if (_control.CurrentRoom != null)
                {
                    gameGUI.Turn = _currentTurn;
                    gameGUI.RoomX = _control.CurrentRoom.X;
                    gameGUI.RoomY = _control.CurrentRoom.Y;
                }
            }
            else
                gameGUI.HoldName = "Press '.' to load an hold";

            gameGUI.DrawGUI();
        }

        private void NextTurn(int mMove, bool mSaveMove = true)
        {
            if (_instance == null) return;
            _currentTurn++;

            TurnSetBattleMove(mMove);
            LastInput = mMove;
            if (_playerAlive && mSaveMove) _undoValues.Inputs.Add(mMove);

            _instance.NextTurn();
        }
        private void TurnSetBattleMove(int mMove)
        {
            if (_isLastBattleInputTwo) _battleSecondMove = mMove;
            else _battleFirstMove = mMove;
            _isLastBattleInputTwo = !_isLastBattleInputTwo;
        }
        public void TurnCheckPlayerKilled()
        {
            if (!_playerAlive || _instance.IsPlayerAlive()) return;
            TDLSounds.Play("SoundDeath");
            _playerAlive = false;
        }
        public void TurnCheckRoomClear()
        {
            if (!_playerAlive) return;
            if (_control.CurrentRoom.IsClear) _undoValues.ClearTurns++;
            if (_control.CurrentRoom.IsClear || !_instance.IsClear()) return;
            _control.CurrentRoom.IsClear = true;
            _undoValues.ClearTurns++;
            TDLSounds.Play("SoundLaugh1");
        }

        public bool IsRoomClear() { return _control.CurrentRoom != null && _control.CurrentRoom.IsClear; }

        public void SetScrollText(string mText) { _common.GUI.SetScrollText(mText); }
        private TDGInstance CreateInstance(TDSRoom mRoom, bool mTemporary = false)
        {
            var result = new TDGInstance(this, mTemporary);
            TDLFactory.Instance = result;

            // Initialize field and pathfinder
            result.Initialize(mRoom.Level.RoomWidth, mRoom.Level.RoomHeight);

            // Refresh texture size, if instance isn't temporary
            if (!mTemporary) RefreshGFX(TDUtils.TileSize*result.Width, TDUtils.TileSize*result.Height);

            // Fill the instance with floor
            for (var iY = 0; iY < result.Height; iY++)
                for (var iX = 0; iX < result.Width; iX++)
                {
                    TDLFactory.Tile = result.GetTile(iX, iY);
                    result.AddEntity(TDLFactory.Floor());
                }

            // Get all tiles from the editor tilemanager and create corrispondent entities
            foreach (var tileManagerEntity in mRoom.TileManager.Entities)
            {
                var methodInfo = tileManagerEntity.Outline.MethodInfo;
                var parameterValues = new object[tileManagerEntity.Parameters.Count];

                for (var i = 0; i < tileManagerEntity.Parameters.Count; i++)
                    parameterValues[i] = tileManagerEntity.Parameters[i].Value;

                TDLFactory.Tile = result.GetTile(tileManagerEntity.Tile.X, tileManagerEntity.Tile.Y);
                var invokedEntity = (Entity) methodInfo.Invoke(null, parameterValues);
                result.AddEntity(invokedEntity);

                // If the room was cleared previously and the entity is required, destroy it instantly
                // This means that if the player returns to the room after it was cleared, there are no mobs
                if (mRoom.IsClear && invokedEntity.HasTag(TDLTags.RequiredKill))
                    invokedEntity.Destroy();
            }

            result.CalculatePathmaps();

            return result;
        }
    }
}