#region
using System;
using System.Linq;
using SFMLStart.Data;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCPressurePlate : Component
    {
        #region PressurePlateType enum
        public enum PressurePlateType
        {
            Single,
            Multiple,
            OnOff
        }
        #endregion

        private readonly TDCFloodFiller _floodFillerComponent;
        private readonly TDCIDCaller _idCallerComponent;
        private readonly PressurePlateType _pressurePlateType;
        private readonly TDCRender _renderComponent;
        private readonly TDCSpecialSquare _specialSquareComponent;
        private readonly string _triggeredLabel, _unTriggeredLabel;

        public TDCPressurePlate(PressurePlateType mPressurePlateType, TDCRender mRenderComponent, TDCFloodFiller mFloodFillerComponent,
                                TDCIDCaller mIDCallerComponent, TDCSpecialSquare mSpecialSquareComponent, string mTriggeredLabel, string mUntriggeredLabel)
        {
            _pressurePlateType = mPressurePlateType;
            _renderComponent = mRenderComponent;
            _floodFillerComponent = mFloodFillerComponent;
            _idCallerComponent = mIDCallerComponent;
            _specialSquareComponent = mSpecialSquareComponent;
            _triggeredLabel = mTriggeredLabel;
            _unTriggeredLabel = mUntriggeredLabel;

            _renderComponent.OnDraw += Draw;
            _specialSquareComponent.OnMoveToAllowed += SpecialSquareBehavior;
        }

        public bool Triggered { get; set; }
        public bool CallSent { get; set; }

        public void Trigger()
        {
            if (Triggered) return;
            foreach (var plate in _floodFillerComponent.GetAttachedEntities())
                plate.GetComponent<TDCPressurePlate>().Triggered = true;

            if (HasAnyNeighborSentCall()) return;

            _idCallerComponent.SendCalls();
            TDLSounds.Play("SoundPressurePlate");

            foreach (var plate in _floodFillerComponent.GetAttachedEntities())
                plate.GetComponent<TDCPressurePlate>().CallSent = true;
        }

        public bool IsAnyNeighborOccupied()
        {
            return _floodFillerComponent.GetAttachedEntities().Any
                (x => x.Field.GetEntitiesByTag(x.X, x.Y, TDLTags.Solid).Any(y => !y.HasTag(TDLTags.DoesNotTriggerPlate)));
        }

        private bool HasAnyNeighborSentCall() { return _floodFillerComponent.GetAttachedEntities().Any(x => x.GetComponent<TDCPressurePlate>().CallSent); }

        private bool SpecialSquareBehavior(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;
            Trigger();
            return true;
        }

        private void Draw() { _renderComponent.GetSprite(0).TextureRect = Assets.Tilesets["pressureplatetiles"].GetTextureRect(Triggered ? _triggeredLabel : _unTriggeredLabel); }

        public override void NextTurn()
        {
            base.NextTurn();

            if (_pressurePlateType == PressurePlateType.Single)
            {
                if (Field.GetEntitiesByTag(X, Y, TDLTags.Solid).Any(y => !y.HasTag(TDLTags.DoesNotTriggerPlate)))
                    Trigger();
            }
            else if (_pressurePlateType == PressurePlateType.Multiple)
            {
                if (Field.GetEntitiesByTag(X, Y, TDLTags.Solid).Any(y => !y.HasTag(TDLTags.DoesNotTriggerPlate)))
                    Trigger();

                var occupied = IsAnyNeighborOccupied();
                Triggered = occupied;

                if (!occupied)
                    foreach (var plate in _floodFillerComponent.GetAttachedEntities())
                        plate.GetComponent<TDCPressurePlate>().CallSent = false;
            }
            else if (_pressurePlateType == PressurePlateType.OnOff)
            {
                if (Field.GetEntitiesByTag(X, Y, TDLTags.Solid).Any(y => !y.HasTag(TDLTags.DoesNotTriggerPlate)))
                    Trigger();

                if (IsAnyNeighborOccupied()) return;
                if (CallSent)
                {
                    foreach (var plate in _floodFillerComponent.GetAttachedEntities())
                        plate.GetComponent<TDCPressurePlate>().CallSent = false;

                    _idCallerComponent.SendCalls();
                    TDLSounds.Play("SoundPressurePlate");
                }

                foreach (var plate in _floodFillerComponent.GetAttachedEntities())
                {
                    plate.GetComponent<TDCPressurePlate>().CallSent = false;
                    plate.GetComponent<TDCPressurePlate>().Triggered = false;
                }
            }
        }
    }
}