#region
using System;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCWeapon : Component
    {
        public bool IsOutOfBounds { get; set; }
        public Action<TDCWielder> OnUnEquip { get; set; }
        public Action<TDCWielder> OnEquip { get; set; }

        public override void NextTurn()
        {
            base.NextTurn();
            if (Field.GetComponents(X, Y, typeof (TDCWeapon)).Any(x => x != this)) TDLSounds.Play("SoundSwordClash");
        }
    }
}