using System;
using TimeDRODPOF.TDGame;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCScroll : Component
    {
        [NonSerialized] private readonly TDGGame _game;
        private readonly string _text;
        private bool _isRead;

        public TDCScroll(TDGGame mGame, string mText)
        {
            _game = mGame;
            _text = mText;
        }

        public override void Added()
        {
            base.Added();

            Field.TurnActions[TDLTurnActions.Start] += () => _game.SetScrollText("");
        }

        public override void NextTurn()
        {
            base.NextTurn();

            if (Field.HasEntityByTag(X, Y, TDLTags.Player))
            {
                _game.SetScrollText(_text);
                if (!_isRead)
                {
                    TDLSounds.Play("SoundScroll");
                    _isRead = true;
                }
            }
            else
                _isRead = false;
        }
    }
}