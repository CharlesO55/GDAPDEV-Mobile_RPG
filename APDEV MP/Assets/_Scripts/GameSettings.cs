using System;
using System.Collections;
public static class GameSettings
{


    public static bool IS_REROLL_ENABLED = true;
    public static bool IS_DIEROLL_ALWAYS_WIN = false;
    public static bool IS_DIEROLL_ALWAYS_FAIL = false;

    public static bool IS_GODMODE_ON = false;

    public static bool IS_UNLOCK_ALL_DOORS = false;

    //SCENE TRACKING
    public static Tuple<int, int> PLAYABLE_SCENES_INDEX_RANGE = new(1, 6);
    public static int END_SCENE_INDEX = 7;
}