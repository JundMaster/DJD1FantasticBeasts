using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class NewLevelReset : MonoBehaviour
{
    void Awake()
    {
        LevelManager.CreaturesSaved = 0;
        LevelManager.GAMEOVER = false;
        LevelManager.AssistMode = false;
        LevelManager.WONGAME = false;
        LevelManager.ReachedBoss = false;
        LevelManager.BossDefeated = false;
        PauseMenu.gamePaused = false;
    }
}
