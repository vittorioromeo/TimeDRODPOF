#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFMLStart;
using SFMLStart.Data;
using TimeDRODPOF.TDGame;
using TimeDRODPOF.TDStructure;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public class TDEEditor : Game
    {
        private readonly TDCommon _common;
        private readonly List<TDEEntity> _drawEntities;
        private readonly List<Sprite> _elementSprites;
        private readonly int _previewSpriteCount;
        private TDEEntity _copiedEntity;
        private List<TDEEntityParameter> _copiedParameters;
        private TDEOutline _copiedParametersOutline;
        private TDEEntity _currentEntity;
        private TDEOutline _currentOutline;
        private TDETileManager _currentTileManager;
        private bool _drawIDs = true, _drawSortRequired;
        private bool _rectangleDeleting;
        private TDVector2? _rectangleEnd;
        private bool _rectangleMode, _rectanglePainting;
        private TDVector2? _rectangleStart;

        public TDEEditor(GameWindow mGameWindow)
        {
            TDGameWindow = mGameWindow;
            _drawEntities = new List<TDEEntity>();
            _elementSprites = new List<Sprite>();
            _common = new TDCommon(mGameWindow);

            _currentOutline = TDEOutlines.GetOutlines().First();
            _rectangleMode = true;

            OnUpdate += mFrameTime =>
                        {
                            UpdateSelector();
                            UpdateInfoText();
                        };
            AddDrawAction(DrawEntities);

            _previewSpriteCount = 14;
            for (var i = 0; i < _previewSpriteCount; i++) _elementSprites.Add(new Sprite());

            OnDrawAfterDefault += DrawGUI;
            OnDrawBeforeCamera += _common.GUI.DrawBackground;

            InitializeInputs();
        }

        private GameWindow TDGameWindow { get; set; }
        public TDGGame Game { private get; set; }
        private TDSControl Control { get; set; }
        private TDETile CurrentTile { get { return _currentTileManager.Tiles[_common.TilePosition.X, _common.TilePosition.Y]; } }

        private void InitializeInputs()
        {
            const float panspeed = 9;
            const float zoominspeed = 1.05f;
            const float zoomoutspeed = 0.95f;

            Bind("createnewhold", 50, CreateNewHold, null, new KeyCombination(Keyboard.Key.F1));
            Bind("createnewlevel", 50, CreateNewLevel, null, new KeyCombination(Keyboard.Key.F2));
            Bind("createnewroom", 50, () => CreateNewRoom(false), null, new KeyCombination(Keyboard.Key.F3));
            Bind("selectlevel", 50, SelectLevel, null, new KeyCombination(Keyboard.Key.F4));
            Bind("selectroom", 50, SelectRoom, null, new KeyCombination(Keyboard.Key.F5));

            Bind("pan_n", 0, () => GameWindow.Camera.Move(new Vector2f(0, -panspeed)), null, new KeyCombination(Keyboard.Key.I));
            Bind("pan_s", 0, () => GameWindow.Camera.Move(new Vector2f(0, panspeed)), null, new KeyCombination(Keyboard.Key.K));
            Bind("pan_e", 0, () => GameWindow.Camera.Move(new Vector2f(panspeed, 0)), null, new KeyCombination(Keyboard.Key.L));
            Bind("pan_w", 0, () => GameWindow.Camera.Move(new Vector2f(-panspeed, 0)), null, new KeyCombination(Keyboard.Key.J));

            Bind("zoom_in", 0, () => GameWindow.Camera.Zoom(zoominspeed), null, new KeyCombination(Keyboard.Key.N));
            Bind("zoom_out", 0, () => GameWindow.Camera.Zoom(zoomoutspeed), null, new KeyCombination(Keyboard.Key.M));

            Bind("reset", 0, () =>
                             {
                                 if (_currentTileManager == null) return;
                                 _drawEntities.Clear();
                                 _currentTileManager.Clear(33, 24);
                                 _common.RefreshTexture(TDUtils.TileSize*_currentTileManager.Width, TDUtils.TileSize*_currentTileManager.Height);
                             }, null, new KeyCombination(Keyboard.Key.R));
            Bind("exit", 0, () => Environment.Exit(0), null, new KeyCombination(Keyboard.Key.Escape));

            Bind("switchtogame", 0, () => GameWindow.SetGame(Game), null, new KeyCombination(Keyboard.Key.F6));

            Bind("paint", 0, Paint, StopPainting, new KeyCombination(Mouse.Button.Left));
            Bind("paintnochecks", 0, PaintNoChecks, null, new KeyCombination(Keyboard.Key.LControl, Mouse.Button.Left));
            Bind("delete", 0, Delete, StopDeleting, new KeyCombination(Mouse.Button.Right));
            Bind("deleteall", 0, DeleteAll, null, new KeyCombination(Keyboard.Key.LControl, Mouse.Button.Right));
            Bind("pick", 20, Pick, null, new KeyCombination(Keyboard.Key.Q));
            Bind("rotatecw", 15, () => RotateEntity(1), null, new KeyCombination(Keyboard.Key.H));
            Bind("rotateccw", 15, () => RotateEntity(-1), null, new KeyCombination(Keyboard.Key.G));

            Bind("browseparameters", 50, BrowseParameters, null, new KeyCombination(Mouse.Button.Middle));
            Bind("copyparameters", 5, CopyParameters, null, new KeyCombination(Keyboard.Key.C));
            Bind("pasteparameters", 0, PasteParameters, null, new KeyCombination(Keyboard.Key.V));

            Bind("copyentity", 5, CopyEntity, null, new KeyCombination(Keyboard.Key.LControl, Keyboard.Key.C));
            Bind("pasteentity", 0, PasteEntity, null, new KeyCombination(Keyboard.Key.LControl, Keyboard.Key.V));

            Bind("outlinesprevious", 20, PreviousOutline, null, new KeyCombination(Keyboard.Key.A));
            Bind("outlinesnext", 20, NextOutline, null, new KeyCombination(Keyboard.Key.S));

            Bind("savetofile", 50, SaveToFile, null, new KeyCombination(Keyboard.Key.Comma));
            Bind("loadfromfile", 50, LoadFromFile, null, new KeyCombination(Keyboard.Key.Period));

            Bind("toggledrawtexts", 20, () => _drawIDs = !_drawIDs, null, new KeyCombination(Keyboard.Key.T));

            Bind("switchroom_n", 20, () => SwitchRoom(0, -1), null, new KeyCombination(Keyboard.Key.Numpad8));
            Bind("switchroom_s", 20, () => SwitchRoom(0, 1), null, new KeyCombination(Keyboard.Key.Numpad2));
            Bind("switchroom_w", 20, () => SwitchRoom(-1, 0), null, new KeyCombination(Keyboard.Key.Numpad4));
            Bind("switchroom_e", 20, () => SwitchRoom(1, 0), null, new KeyCombination(Keyboard.Key.Numpad6));

            Bind("switchpaintingmode", 20, () => _rectangleMode = !_rectangleMode, null, new KeyCombination(Keyboard.Key.D));
        }

        private void RefreshTileManager()
        {
            _drawEntities.Clear();

            if (_currentTileManager != null) _currentTileManager.OnEntityCreated -= AddDrawEntity;

            _currentTileManager = Control.CurrentRoom.TileManager;
            _currentTileManager.OnEntityCreated += AddDrawEntity;

            foreach (var entity in _currentTileManager.Entities) AddDrawEntity(entity);

            _common.RefreshTexture(TDUtils.TileSize*_currentTileManager.Width, TDUtils.TileSize*_currentTileManager.Height);
            _common.CenterCamera();
        }

        private void CreateNewHold()
        {
            var holdTitle = TDEUtils.InputStringBox("Insert hold name", "", "UnnamedHold");
            Control = new TDSControl();
            Control.CurrentHold = TDSControl.CreateHold(Control, holdTitle);

            CreateNewLevel();
        }
        private void CreateNewLevel()
        {
            if (Control == null || Control.CurrentHold == null) return;

            var levelName = TDEUtils.InputStringBox("Insert level name", "", "UnnamedLevel");
            var roomWidth = int.Parse(TDEUtils.InputStringBox("Insert room width", TDUtils.DefaultRoomWidth.ToString(), TDUtils.DefaultRoomWidth.ToString()));
            var roomHeight = int.Parse(TDEUtils.InputStringBox("Insert room height", TDUtils.DefaultRoomHeight.ToString(), TDUtils.DefaultRoomHeight.ToString()));

            Control.CurrentLevel = TDSControl.CreateLevel(Control.CurrentHold, levelName, roomWidth, roomHeight);

            CreateNewRoom(true);
        }
        private void CreateNewRoom(bool mIsEntrance)
        {
            if (Control == null || Control.CurrentLevel == null) return;

            if (mIsEntrance) Control.CurrentRoom = TDSControl.CreateRoom(Control.CurrentLevel, 0, 0, true, false);
            else
            {
                var room = TDEUtils.InputCreateRoomBox(Control.CurrentRoom);
                if (room == null) return;
                Control.CurrentLevel.AddRoom(room);
                Control.CurrentRoom = room;
            }
            RefreshTileManager();
        }

        private void SelectLevel()
        {
            if (Control == null || Control.CurrentHold == null) return;

            var index = TDEUtils.InputListBox("Select level", Control.CurrentHold.Levels.Select((level, i) => string.Format("{0}: {1}", i, level.Name)).ToArray());
            if (index == -1) return;

            Control.CurrentLevel = Control.CurrentHold.Levels[index];
            Control.CurrentRoom = Control.CurrentLevel.GetRoom(0, 0);

            RefreshTileManager();
        }
        private void SelectRoom()
        {
            if (Control == null || Control.CurrentLevel == null) return;

            var roomNames = Control.CurrentLevel.Rooms.Select(room => string.Format("{0},{1}", room.Value.X, room.Value.Y)).ToList();

            var index = TDEUtils.InputListBox("Select room", roomNames.ToArray());
            if (index == -1) return;
            var roomSelectionSplit = roomNames[index].Split(',');
            var roomX = int.Parse(roomSelectionSplit[0]);
            var roomY = int.Parse(roomSelectionSplit[1]);

            Control.CurrentRoom = Control.CurrentLevel.GetRoom(roomX, roomY);

            RefreshTileManager();
        }

        private void Paint()
        {
            if (IsInvalid()) return;

            if (_rectangleMode)
            {
                _rectanglePainting = true;
                _rectangleDeleting = false;

                if (_rectangleStart == null)
                    _rectangleStart = _common.TilePosition;

                _rectangleEnd = _common.TilePosition;
                return;
            }

            if (CurrentTile.OrderedEntities.Find(x => x.EditorGroup == _currentOutline.EditorGroup) != null) return;
            _currentTileManager.CreateEntity(_common.TilePosition.X, _common.TilePosition.Y, new TDEEntity(_currentOutline));
        }
        private void PaintNoChecks()
        {
            if (IsInvalid()) return;

            if (CurrentTile.OrderedEntities.Find(x => x.Outline == _currentOutline) != null) return;
            _currentTileManager.CreateEntity(_common.TilePosition.X, _common.TilePosition.Y, new TDEEntity(_currentOutline));
        }
        private void StopPainting()
        {
            _rectanglePainting = false;
            if (!_rectangleMode || _rectangleDeleting) return;
            if (IsInvalid() || _rectangleStart == null || _rectangleEnd == null) return;

            var startX = Math.Min(_rectangleStart.Value.X, _rectangleEnd.Value.X);
            var startY = Math.Min(_rectangleStart.Value.Y, _rectangleEnd.Value.Y);
            var width = Math.Abs(_rectangleStart.Value.X - _rectangleEnd.Value.X);
            var height = Math.Abs(_rectangleStart.Value.Y - _rectangleEnd.Value.Y);

            for (var iY = startY; iY <= startY + height; iY++)
                for (var iX = startX; iX <= startX + width; iX++)
                {
                    if (!_currentTileManager.IsValid(iX, iY)) continue;
                    if (_currentTileManager.Tiles[iX, iY].OrderedEntities.Find(x => x.EditorGroup == _currentOutline.EditorGroup) != null) continue;
                    _currentTileManager.CreateEntity(iX, iY, new TDEEntity(_currentOutline));
                }

            _rectangleStart = null;
            _rectangleEnd = null;
        }

        private void Delete()
        {
            if (IsInvalid()) return;

            if (_rectangleMode)
            {
                _rectanglePainting = false;
                _rectangleDeleting = true;

                if (_rectangleStart == null)
                    _rectangleStart = _common.TilePosition;

                _rectangleEnd = _common.TilePosition;
                return;
            }

            var entity = CurrentTile.OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
            if (entity == null) return;

            _currentTileManager.DeleteEntity(entity);
            _drawEntities.Remove(entity);
        }
        private void DeleteAll()
        {
            if (IsInvalid()) return;

            var entity = CurrentTile.OrderedEntities.LastOrDefault();
            if (entity == null) return;

            _currentTileManager.DeleteEntity(entity);
            _drawEntities.Remove(entity);
        }
        private void StopDeleting()
        {
            _rectangleDeleting = false;
            if (!_rectangleMode || _rectanglePainting) return;
            if (IsInvalid() || _rectangleStart == null || _rectangleEnd == null) return;

            var startX = Math.Min(_rectangleStart.Value.X, _rectangleEnd.Value.X);
            var startY = Math.Min(_rectangleStart.Value.Y, _rectangleEnd.Value.Y);
            var width = Math.Abs(_rectangleStart.Value.X - _rectangleEnd.Value.X);
            var height = Math.Abs(_rectangleStart.Value.Y - _rectangleEnd.Value.Y);

            for (var iY = startY; iY <= startY + height; iY++)
                for (var iX = startX; iX <= startX + width; iX++)
                {
                    var entity = _currentTileManager.Tiles[iX, iY].OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
                    if (entity == null) continue;

                    _currentTileManager.DeleteEntity(entity);
                    _drawEntities.Remove(entity);
                }

            _rectangleStart = null;
            _rectangleEnd = null;
        }

        private void Pick()
        {
            if (IsInvalid()) return;

            foreach (var entity in CurrentTile.OrderedEntities)
                if (_currentOutline != entity.Outline)
                {
                    _currentOutline = entity.Outline;
                    break;
                }
        }

        private void BrowseParameters()
        {
            if (IsInvalid()) return;

            var entity = CurrentTile.OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
            if (entity == null) return;

            TDEUtils.InputParametersBox(string.Format("{0}'s parameters", entity.Name), entity);
        }
        private void CopyParameters()
        {
            if (IsInvalid()) return;

            var entity = CurrentTile.OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
            if (entity == null) return;

            _copiedParametersOutline = entity.Outline;
            _copiedParameters = entity.CloneParameters();
        }
        private void PasteParameters()
        {
            if (IsInvalid()) return;

            if (_copiedParametersOutline == null || _copiedParameters == null) return;
            var entity = CurrentTile.OrderedEntities.Find(x => x.Outline == _copiedParametersOutline);
            if (entity == null) return;

            var newParameters = _copiedParameters.Select(parameter => parameter.Clone()).ToList();
            entity.Parameters = new List<TDEEntityParameter>(newParameters);
        }

        private void CopyEntity()
        {
            if (IsInvalid()) return;

            var entity = CurrentTile.OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
            if (entity == null) return;

            _copiedEntity = entity.Clone();
        }
        private void PasteEntity()
        {
            if (IsInvalid()) return;
            if (_copiedEntity == null) return;

            var entity = CurrentTile.OrderedEntities.Find(x => x.Outline == _currentOutline);
            if (entity != null)
            {
                _currentTileManager.DeleteEntity(entity);
                _drawEntities.Remove(entity);
            }

            _currentTileManager.CreateEntity(_common.TilePosition.X, _common.TilePosition.Y, _copiedEntity.Clone());
        }
        private void RotateEntity(int mAmount)
        {
            if (IsInvalid()) return;

            var entity = CurrentTile.OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
            if (entity == null) return;

            var directionParameter = entity.Parameters.Find(x => x.Name == "mDirection");
            if (directionParameter == null) return;

            directionParameter.Value = (int) directionParameter.Value + mAmount;
            if ((int) directionParameter.Value > 7) directionParameter.Value = 0;
            if ((int) directionParameter.Value < 0) directionParameter.Value = 7;
        }

        private void NextOutline()
        {
            var outlines = TDEOutlines.GetOutlines();

            var index = outlines.IndexOf(_currentOutline);
            _currentOutline = outlines.Count > index + 1 ? outlines[index + 1] : outlines[0];
        }
        private void PreviousOutline()
        {
            var outlines = TDEOutlines.GetOutlines();

            var index = outlines.IndexOf(_currentOutline);
            _currentOutline = index - 1 > -1 ? outlines[index - 1] : outlines[TDEOutlines.GetOutlines().Count - 1];
        }

        private void SwitchRoom(int mDirectionX, int mDirectionY)
        {
            if (Control.CurrentRoom == null) return;

            var roomX = Control.CurrentRoom.X + mDirectionX;
            var roomY = Control.CurrentRoom.Y + mDirectionY;

            var room = Control.CurrentLevel.GetRoom(roomX, roomY);
            if (room == null) return;

            Control.CurrentRoom = room;
            RefreshTileManager();
        }

        private void SaveToFile()
        {
            if (Control == null) return;

            var holdName = TDEUtils.InputStringBox("Save hold", Control.CurrentHold.Name);
            if (string.IsNullOrEmpty(holdName)) return;

            TDELevelIO.SaveToFile(Control, string.Format(@"{0}.hold", holdName));
        }
        private void LoadFromFile()
        {
            var holdNames = new DirectoryInfo(Environment.CurrentDirectory).GetFiles().Where(x => x.Extension.EndsWith("hold")).Select(fileInfo => fileInfo.Name).ToList();
            var holdIndex = TDEUtils.InputListBox("Load hold", holdNames.ToArray());
            if (holdIndex == -1) return;
            var holdName = holdNames[holdIndex];

            Control = TDELevelIO.LoadFromFile(holdName);
            Control.CurrentHold = Control.CurrentHold;
            Control.CurrentLevel = Control.CurrentHold.Levels.First();
            Control.CurrentRoom = Control.CurrentLevel.GetRoom(0, 0);
            RefreshTileManager();
        }

        private void AddDrawEntity(TDEEntity mEntity)
        {
            _drawEntities.Add(mEntity);
            mEntity.Sprite.Position = new Vector2f(mEntity.Tile.X, mEntity.Tile.Y)*TDUtils.TileSize + new Vector2f(TDUtils.TileSize/2f, TDUtils.TileSize/2f);
            mEntity.Sprite.Origin = new Vector2f(mEntity.Sprite.TextureRect.Width/2f, mEntity.Sprite.TextureRect.Height/2f);
            _drawSortRequired = true;
        }

        private void DrawEntities()
        {
            if (_currentTileManager == null) return;

            _common.ClearTexture();

            if (_drawSortRequired) _drawEntities.Sort((a, b) => a.Layer.CompareTo(b.Layer));
            _drawSortRequired = false;

            foreach (var entity in new List<TDEEntity>(_drawEntities))
            {
                var sprite = entity.Sprite;

                var directionParameter = entity.Parameters.Find(x => x.Name == "mDirection");
                if (directionParameter != null) sprite.Rotation = 45*(int) directionParameter.Value;

                _common.GameTexture.Draw(sprite);

                if (_drawIDs) DrawEntitiesSpecialIDs(sprite, entity);
                DrawEntitiesSpecialOnOff(sprite, entity);
                DrawEntitySpecialBroken(sprite, entity);
            }

            _common.GameTexture.Draw(_common.SelectionSprite);

            _common.GameTexture.Display();
            GameWindow.RenderWindow.Draw(_common.GameSprite);
        }
        private void DrawEntitySpecialBroken(Sprite mSprite, TDEEntity mEntity)
        {
            var isBrokenParameter = mEntity.Parameters.Find(x => x.Name == "mIsBroken");
            if (isBrokenParameter == null || (!((bool) isBrokenParameter.Value))) return;

            var brokenOverlay = Assets.Tilesets["brokenoverlaytiles"].GetSprite(1, 1, Assets.GetTexture(@"environment\brokenoverlay"));
            brokenOverlay.Position = mSprite.Position;
            _common.GameTexture.Draw(brokenOverlay);
        }
        private void DrawEntitiesSpecialOnOff(Sprite mSprite, TDEEntity mEntity)
        {
            var offParameter = mEntity.Parameters.Find(x => x.Name == "mIsOff");
            var directionParameter = mEntity.Parameters.Find(x => x.Name == "mDirection");

            if (offParameter == null) return;

            if (mEntity.Name.Contains("Door")) return;

            if (directionParameter == null)
                mSprite.TextureRect = (bool) offParameter.Value ? Assets.Tilesets["onofftiles"].GetTextureRect("off") : Assets.Tilesets["onofftiles"].GetTextureRect("on");
            else
                mSprite.TextureRect = (bool) offParameter.Value ? Assets.Tilesets["onoffdirtiles"].GetTextureRect("off_n") : Assets.Tilesets["onoffdirtiles"].GetTextureRect("on_n");
        }
        private void DrawEntitiesSpecialIDs(Sprite mSprite, TDEEntity mEntity)
        {
            Text text = null;

            var idsParameter = mEntity.Parameters.Find(x => x.Name == "mIDs");
            if (idsParameter != null)
            {
                var idsString = idsParameter.ToString();
                if (idsString == "-1") return;
                text = new Text(idsString, Font.DefaultFont, 10) {Position = mSprite.Position - mSprite.Origin, Style = Text.Styles.Bold, Color = Color.White};
            }

            var idsTargetParameter = mEntity.Parameters.Find(x => x.Name == "mTargetIDs");
            if (idsTargetParameter != null)
            {
                var idsTargetIDs = idsTargetParameter.ToString();
                if (idsTargetIDs == "-1") return;
                text = new Text(idsTargetIDs, Font.DefaultFont, 10) {Position = mSprite.Position - mSprite.Origin, Style = Text.Styles.Bold, Color = Color.Blue};
            }

            if (text == null) return;

            var textShadow1 = new Text(text) {Position = mSprite.Position - mSprite.Origin + new Vector2f(1, 1), Color = Color.Black};
            var textShadow2 = new Text(text) {Position = mSprite.Position - mSprite.Origin + new Vector2f(-1, 1), Color = Color.Black};
            var textShadow3 = new Text(text) {Position = mSprite.Position - mSprite.Origin + new Vector2f(1, -1), Color = Color.Black};
            var textShadow4 = new Text(text) {Position = mSprite.Position - mSprite.Origin + new Vector2f(-1, -1), Color = Color.Black};
            _common.GameTexture.Draw(textShadow1);
            _common.GameTexture.Draw(textShadow2);
            _common.GameTexture.Draw(textShadow3);
            _common.GameTexture.Draw(textShadow4);
            _common.GameTexture.Draw(text);
        }
        private void DrawGUI()
        {
            if (Control != null)
            {
                if (Control.CurrentHold != null) _common.GUI.HoldName = Control.CurrentHold.Name;
                if (Control.CurrentLevel != null) _common.GUI.LevelName = Control.CurrentLevel.Name;
                if (Control.CurrentRoom != null)
                {
                    _common.GUI.RoomX = Control.CurrentRoom.X;
                    _common.GUI.RoomY = Control.CurrentRoom.Y;
                }
            }
            _common.GUI.DrawGUI();

            if (_currentOutline == null) return;

            for (var i = 0; i < _previewSpriteCount; i++)
            {
                var sprite = _elementSprites[i];

                sprite.Color = i != _previewSpriteCount/2 ? new Color(255, 255, 255, 100) : new Color(255, 255, 255, 255);
                sprite.Scale = i != _previewSpriteCount/2 ? new Vector2f(1.4f - (Math.Abs(i - _previewSpriteCount/2)*0.1f), 1.4f - (Math.Abs(i - _previewSpriteCount/2)*0.1f)) : new Vector2f(2, 2);

                var outlines = TDEOutlines.GetOutlines();
                var outlineIndex = outlines.IndexOf(_currentOutline) - (_previewSpriteCount/2);

                outlineIndex += i;

                while (outlineIndex >= outlines.Count)
                    outlineIndex -= outlines.Count;

                while (outlineIndex < 0)
                    outlineIndex += outlines.Count;

                sprite.Texture = outlines[outlineIndex].Texture;
                sprite.TextureRect = outlines[outlineIndex].TextureRect;

                sprite.Origin = new Vector2f(sprite.TextureRect.Width/2f, sprite.TextureRect.Height/2f);
                sprite.Position = new Vector2f(288 + 50*i, 71);

                GameWindow.RenderWindow.Draw(sprite);
            }
        }

        private void UpdateSelector()
        {
            _common.UpdatePositions();
            _common.SelectionSprite.Texture = _currentOutline.Texture;
            _common.SelectionSprite.TextureRect = _currentOutline.TextureRect;
            _common.SelectionSprite.Color = new Color(255, 255, 255, 100);

            if (_currentTileManager == null) return;
            if (!_currentTileManager.IsValid(_common.TilePosition.X, _common.TilePosition.Y)) return;
            var entity = CurrentTile.OrderedEntities.Find(x => x.Layer == _currentOutline.Layer);
            _currentEntity = entity;

            UpdateRectanglePreview();
        }
        private void UpdateRectanglePreview()
        {
            if (!_rectanglePainting && !_rectangleDeleting) return;
            if (_rectangleStart == null || _rectangleEnd == null) return;

            var xInverse = (_rectangleStart.Value.X <= _rectangleEnd.Value.X) ? 1 : 0;
            var yInverse = (_rectangleStart.Value.Y <= _rectangleEnd.Value.Y) ? 1 : 0;
            var xStart = _rectangleStart.Value.X;
            var yStart = _rectangleStart.Value.Y;
            var xEnd = _rectangleEnd.Value.X;
            var yEnd = _rectangleEnd.Value.Y;
            var xDist = xEnd - xStart + xInverse;
            var yDist = yEnd - yStart + yInverse;

            _common.SelectionSprite.Texture = new Texture(_common.SelectionSprite.Texture.CopyToImage(), new IntRect(0, 0, TDUtils.TextureSize, TDUtils.TextureSize)) {Repeated = true};
            _common.SelectionSprite.Position = new Vector2f((xStart - (xInverse - 1))*TDUtils.TileSize, (yStart - (yInverse - 1))*TDUtils.TileSize);
            _common.SelectionSprite.TextureRect = new IntRect(0, 0, (xDist + (xInverse - 1))*TDUtils.TileSize, (yDist + (yInverse - 1))*TDUtils.TileSize);
        }
        private void UpdateInfoText()
        {
            if (_currentTileManager == null)
            {
                _common.GUI.SetScrollText("Press F1 to create a new hold\nPress F2 to create a new level\nPress F3 to create a new room");
                return;
            }

            var infoString = new StringBuilder();
            infoString.Append(_rectangleMode ? "Painting mode: rectangle" : "Painting mode: point");
            infoString.Append("\n");
            infoString.Append("\n");
            if (Control.CurrentRoom != null)
            {
                infoString.Append(Control.CurrentRoom.IsRequired ? "This room is required" : "This room is not required");
                infoString.Append("\n");
                infoString.Append(Control.CurrentRoom.IsSecret ? "This room is secret" : "This room is not secret");
                infoString.Append("\n");
            }
            infoString.Append("\n");
            infoString.Append(string.Format("Tile [{0}, {1}]", _common.TilePosition.X, _common.TilePosition.Y));
            infoString.Append("\n\n");
            infoString.Append(string.Format("Current tile: {0}", _currentOutline.Name));
            infoString.Append("\n");
            infoString.Append(string.Format("Current layer: {0}", _currentOutline.Layer));
            infoString.Append("\n");
            infoString.Append(string.Format("Current editor group: {0}", _currentOutline.EditorGroup));

            if (_currentEntity != null)
            {
                infoString.Append("\n\n");
                infoString.Append(string.Format("Current mEntity parameters:"));
                infoString.Append("\n");
                foreach (var parameter in _currentEntity.Parameters)
                {
                    infoString.Append(string.Format("- {0}: {1}", parameter.Name, parameter));
                    infoString.Append("\n");
                }
            }

            if (_copiedParameters != null)
            {
                if (_currentEntity == null) infoString.Append("\n");
                infoString.Append("\n");
                infoString.Append(string.Format("Current copied parameters:"));
                infoString.Append("\n");
                foreach (var parameter in _copiedParameters)
                {
                    infoString.Append(string.Format("- {0}: {1}", parameter.Name, parameter));
                    infoString.Append("\n");
                }
            }

            _common.GUI.SetScrollText(infoString.ToString());
        }

        private bool IsInvalid()
        {
            if (_currentTileManager == null || !_currentTileManager.IsValid(_common.TilePosition.X, _common.TilePosition.Y))
            {
                _rectangleStart = null;
                _rectangleEnd = null;
                return true;
            }
            return false;
        }
    }
}