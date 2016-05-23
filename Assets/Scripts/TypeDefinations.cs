public class TypeDefinations {

	public class LevelData
    {
        public int levelNumber;
        public int levelTime;

        public LevelData(int levelNumber, int levelTime)
        {
            this.levelNumber = levelNumber;
            this.levelTime = levelTime;
        }
    }

    public class LevelClassicModeData : LevelData
    {
        public LevelClassicModeData(int levelNumber,int levelTime) : base(levelNumber,levelTime) { }
    }

    public class LevelSurpriseModeData : LevelData
    {
        public int maxWaves;
        public int roundsPerWave;
        public int pipeResetCount;
        public int maxPipesOnEntry;

        public LevelSurpriseModeData(int levelNumber,int levelTime,int maxWaves,int roundsPerWave,int pipeResetCount,int maxPipesOnEntry) : base(levelNumber,levelTime)
        {
            this.maxWaves = maxWaves;
            this.roundsPerWave = roundsPerWave;
            this.pipeResetCount = pipeResetCount;
            this.maxPipesOnEntry = maxPipesOnEntry;
        }
    }
}
