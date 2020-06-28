using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class NewLevelReset : MonoBehaviour
{
    void Awake()
    {
        // Only assist mode (infinite lives) will remain the same

        LevelManager.CreaturesSaved = 0;
        LevelManager.GAMEOVER = false;
        LevelManager.WONGAME = false;
        LevelManager.ReachedBoss = false;
        LevelManager.BossDefeated = false;
        PauseMenu.gamePaused = false;
    }
}
