namespace GravityPong.Game
{
    public struct ScoreData
    {
        public const float MAX_STYLE = 1f;

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