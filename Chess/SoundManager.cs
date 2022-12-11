using SFML.Audio;

using static ChessEngine.Enum;

namespace Chess
{
    public static class SoundManager
    {
        public static void PlaySound(ChessEvents? chessEvent){
            SoundBuffer buffer;

            switch (chessEvent)
            {
                case ChessEvents.CHECK:
                    buffer = new SoundBuffer("sounds/check.wav");
                    break;
                case ChessEvents.CHECKMATE:
                    buffer = new SoundBuffer("sounds/checkmate.wav");
                    break;
                case ChessEvents.STALEMATE:
                    buffer = new SoundBuffer("sounds/stalemate.wav");
                    break;
                case ChessEvents.TAKE:
                    buffer = new SoundBuffer("sounds/take.wav");
                    break;
                case ChessEvents.MOVE:
                    buffer = new SoundBuffer("sounds/move.wav");
                    break;
                default:
                    buffer = new SoundBuffer("sounds/move.wav");
                    break;
            }
            Sound sound = new Sound(buffer);
            sound.Play();
        }
    }
}
