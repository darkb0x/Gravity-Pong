namespace GravityPong.Game.Singleplayer
{
    public struct ScoreData
    {
        public int Score { get; }
        public float Style { get; }
        public string StyleMessage { get; }

        public ScoreData(int score, float style, string styleMessage)
        {
            Score = score;
            Style = style;
            StyleMessage = styleMessage;
        }
    }
}