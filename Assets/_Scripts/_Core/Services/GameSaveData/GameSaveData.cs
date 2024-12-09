using System;

namespace GravityPong
{
    [Serializable]
    public class GameSaveData
    {
        [Serializable]
        public class GameClassicGameplayData
        {
            public int Hits;
            public float Time;

            public GameClassicGameplayData()
            {
                Hits = 0;
                Time = 0;
            }
            public GameClassicGameplayData(int hits, float time)
            {
                Hits = hits;
                Time = time;
            }
        }

        public int ClassicGameHighscore;
        public GameClassicGameplayData PreviousGameTry;

        public int ArcadeGameHighscore;

        public int CurrentGameMode;

        public GameSaveData()
        {
            ClassicGameHighscore = 0;
            ArcadeGameHighscore = 0;
            CurrentGameMode = 0;

            PreviousGameTry = new GameClassicGameplayData();
        }
    }
}
