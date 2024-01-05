namespace GravityPong.Game
{
    public interface IGameHUDWithAdditionalDataView : IGameHUD
    {
        void UpdateHitsText(int hits, int previousHits);
        void UpdatePreviousGameDataText(int previousRoundHits, float previoudRoundTime);
        void UpdateTimeText(float currentTime, float previousTime);
    }
}
