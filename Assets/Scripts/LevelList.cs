using System.Collections.Generic;

public class LevelList {

    // Stores the level entries packed with the game and also includes the downloaded contents.
    public static List<TypeDefinations.LevelData> LevelDataList = new List<TypeDefinations.LevelData>(new[]
    {
        (TypeDefinations.LevelData) new TypeDefinations.LevelClassicModeData(1,90),
        new TypeDefinations.LevelSurpriseModeData(1,10,2,3,5,10),
        new TypeDefinations.LevelSurpriseModeData(2,12,3,3,5,12),
        new TypeDefinations.LevelSurpriseModeData(3,10,3,5,5,9),
        new TypeDefinations.LevelSurpriseModeData(4,10,4,2,5,10)
    });

    public static TypeDefinations.LevelData FindLevelWithId(int gameMode,int levelNumber)
    {
        foreach(TypeDefinations.LevelData curData in LevelDataList)
            if (((gameMode == GameController.GAME_MODE_CLASSIC && (curData is TypeDefinations.LevelClassicModeData)) || (gameMode == GameController.GAME_MODE_SURPRISE && (curData is TypeDefinations.LevelSurpriseModeData))) && curData.levelNumber == levelNumber) return curData;

        return null;
    }
}
