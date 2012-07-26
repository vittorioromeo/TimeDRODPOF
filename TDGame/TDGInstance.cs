using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SFML.Graphics;
using SFMLStart.Utilities;
using TimeDRODPOF.TDComponents;
using TimeDRODPOF.TDLib;
using TimeDRODPOF.TDPathfinding;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDGame
{
    public class TDGInstance
    {
        private readonly Field _field;
        private readonly TDGGame _game;
        private readonly bool _isTemporary;
        private readonly string[] _pathmapNames = new[] {TDLPathmaps.SideGood, TDLPathmaps.SideBad};
        private readonly List<TDCRender> _renderComponents;
        private bool _isDrawSortRequired;
        private TDPPathfinder _pathfinder;

        public TDGInstance(TDGGame mGame, bool mIsTemporary)
        {
            _game = mGame;
            _isTemporary = mIsTemporary;
            _field = new Field();
            _renderComponents = new List<TDCRender>();
        }

        public int Width { get { return _field.Width; } }
        public int Height { get { return _field.Height; } }

        public void Initialize(int mWidth, int mHeight)
        {
            _field.Clear(mWidth, mHeight);

            foreach (var turnActionPriority in TDLTurnActions.Priorities)
                _field.TurnActions.Add(turnActionPriority, null);

            if (!_isTemporary) _field.OnEntityAdded += AddRenderEntity;
            _field.TurnActions[TDLTurnActions.EndChecks] += _game.TurnCheckPlayerKilled;
            _field.TurnActions[TDLTurnActions.EndChecks] += _game.TurnCheckRoomClear;

            CreatePathmaps();
        }
        public void RunLoadChecks()
        {
            _field.OnLoad.SafeInvoke();
            _field.OnLoadChecks.SafeInvoke();

            //Serialize();
        }

        private void CreatePathmaps()
        {
            _pathfinder = new TDPPathfinder(_field);
            foreach (var pathmapName in _pathmapNames)
                _pathfinder.CreatePathmap(pathmapName);
        }
        public void CalculatePathmaps()
        {
            foreach (var pathmapName in _pathmapNames)
                _pathfinder.GetPathmap(pathmapName).CalculatePathmap();
        }

        public void CreatePlayer(int mX, int mY, int mDirection, bool mSheated = false)
        {
            TDLFactory.Tile = _field.GetTile(mX, mY);
            var result = TDLFactory.Player(mDirection);
            result.GetComponent<TDCWielder>().IsSheated = mSheated;
            _field.AddEntity(result);
        }

        public void AddEntity(Entity mEntity) { _field.AddEntity(mEntity); }
        private void AddRenderEntity(Entity mEntity)
        {
            var renderComponent = mEntity.GetComponent<TDCRender>();
            if (renderComponent == null) return;

            _renderComponents.Add(renderComponent);
            _isDrawSortRequired = true;
        }

        public Tile GetTile(int mX, int mY) { return _field.GetTile(mX, mY); }
        public TDPPathmap GetPathmap(string mPathmapName) { return _pathfinder.GetPathmap(mPathmapName); }
        public TDPNode JumpPointSearch(int mStartX, int mStartY, int mTargetX, int mTargetY) { return _pathfinder.JumpPointSearch(mStartX, mStartY, mTargetX, mTargetY); }
        public bool IsRoomSwitchAllowed(int mX, int mY, TDCMovement mMovementComponent)
        {
            return TDCMovement.IsNextAllowed(_field, mX, mY, mMovementComponent.AllowedTags) &&
                   !TDCMovement.IsNextObstacle(_field, mX, mY, mMovementComponent.ObstacleTags, mMovementComponent.IgnoreEntities, mMovementComponent.ExceptionTags);
        }
        public bool IsClear() { return !_field.GetEntitiesByTag(TDLTags.RequiredKill).Any(); }
        public bool IsPlayerAlive() { return _field.GetEntitiesByTag(TDLTags.Player).Any(); }

        public void Serialize()
        {
            var bf = new BinaryFormatter();
            var ents = new List<Entity>(_field.GetEntities());

            for (var i = 0; i < ents.Count; i++)
            {
                var entity = ents[i];
                var stream = File.Create(@"D:\WIP\SerializationTests\" + i + @"entity.ptf");
                bf.Serialize(stream, entity);
                stream.Close();
            }

            LoadFromSerializationTests();
        }
        public void LoadFromSerializationTests()
        {
            Initialize(Width, Height);

            var bf = new BinaryFormatter();
            var dir = new DirectoryInfo(@"D:\WIP\SerializationTests\");
            foreach (var file in dir.GetFiles())
            {
                var deserialize = (Entity) bf.Deserialize(File.Open(file.FullName, FileMode.Open));
                _field.AddEntity(deserialize);
            }
        }

        public void NextTurn() { _field.NextTurn(); }
        public void Draw(RenderTarget mRenderTarget)
        {
            if (_isDrawSortRequired) _renderComponents.Sort((a, b) => a.Entity.Layer.CompareTo(b.Entity.Layer));
            _isDrawSortRequired = false;

            foreach (var component in new List<TDCRender>(_renderComponents))
                if (component.Entity.IsAlive) component.Draw(mRenderTarget);
                else _renderComponents.Remove(component);
        }
    }
}