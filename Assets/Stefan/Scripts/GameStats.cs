public static class GameStats
{
    public static float survivedTime { get; set; }
    public static int rounds { get; set; }
    public static int towersPlaced { get; set; }
    public static int scrapUsed { get; set; }
    public static int ufosShotDown { get; set; }

    public static bool playing { get; set; } = true;

    public static void Reset()
    {
        survivedTime = 0;
        rounds = 0;
        towersPlaced = 0;
        scrapUsed = 0;
        ufosShotDown = 0;
        playing = true;
    }
}
