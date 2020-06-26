using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

sealed public class WinGame : MonoBehaviour
{
    // IN ORDER
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject spawnerSmokePrefab;
    [SerializeField] private GameObject niffler;
    [HideInInspector] private GameObject soundManager;
    [SerializeField] private Transform nifflerRespawnPos;
    [SerializeField] private GameObject text1;
    [SerializeField] private Transform text1Pos;
    [SerializeField] private GameObject goblin;
    [SerializeField] private Transform goblinRespawnPos;
    [SerializeField] private GameObject text2;
    [SerializeField] private Transform text2Pos;
    [SerializeField] private GameObject firstBlackScreen;
    [SerializeField] private GameObject canvesToActive;
    [SerializeField] private GameObject nifflerHead;
    [SerializeField] private TextMeshProUGUI nifflerScore;
    [SerializeField] private GameObject lastBlackScreen;
    [SerializeField] private GameObject creditsText;
    [SerializeField] private Transform bottomCreditsPos;

    // LastMenu related stuff
    private bool canPlayCoRoutine = true;
    private bool canMovePlayer = true;
    private bool nifflerPrintScore = false;
    private bool canPlayCredits = false;
    private float creditsRoll = 0f;

    private PlayerMovement p1;

    void Start()
    {
        p1 = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.WONGAME)
        {
            if (UI.activeSelf) UI.SetActive(false);
            // Blocks controls
            PauseMenu.gamePaused = true;

            // Moves player to the right on the end
            if (canMovePlayer)
                p1.Rb.velocity = new Vector2(p1.RunSpeed, 0f);
            else
            {   // disables sound after a while
                soundManager = GameObject.FindGameObjectWithTag("soundManager");
                if (soundManager != null)
                    soundManager.SetActive(false);
            }

            // Plays CoRoutine
            if (canPlayCoRoutine)
            {
                StartCoroutine(WinMenu());
                canPlayCoRoutine = false;
            }

            // LastMenu related stuff
            if (nifflerPrintScore == false)
                nifflerScore.text = "";
            else nifflerScore.text = $"You saved niffler {LevelManager.CreaturesSaved} / 10 times !!";

            // Refresh credits position
            if (canPlayCredits)
            {
                creditsRoll += 0.003f * Time.deltaTime;
            }
            creditsText.transform.position = new Vector3(creditsText.transform.position.x, creditsText.transform.position.y + creditsRoll, creditsText.transform.position.z);
            if (bottomCreditsPos.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane)).y)
                LoadMenu();

            // Stops coroutine
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopAllCoroutines();
                LoadMenu();
            }
        }
    }


    IEnumerator WinMenu()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Instantiate(spawnerSmokePrefab, nifflerRespawnPos.position, transform.rotation);
            GameObject niff = Instantiate(niffler, nifflerRespawnPos.position , transform.rotation);
            yield return new WaitForSeconds(1f);
            canMovePlayer = false;
            GameObject txt1 = Instantiate(text1, text1Pos.position, transform.rotation);
            yield return new WaitForSeconds(4f);
            Destroy(txt1);
            yield return new WaitForSeconds(1f);
            GameObject gob = Instantiate(goblin, goblinRespawnPos.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
            GameObject gob2 = Instantiate(goblin, goblinRespawnPos.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
            GameObject gob3 = Instantiate(goblin, goblinRespawnPos.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
            GameObject gob4 = Instantiate(goblin, goblinRespawnPos.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
            GameObject txt2 = Instantiate(text2, text2Pos.position, transform.rotation);
            yield return new WaitForSeconds(1.5f);
            Instantiate(spawnerSmokePrefab, nifflerRespawnPos.position, transform.rotation);
            Destroy(niff);
            yield return new WaitForSeconds(2.5f);
            Destroy(txt2);
            yield return new WaitForSeconds(1.5f);
            canvesToActive.SetActive(true);
            firstBlackScreen.SetActive(true);
            yield return new WaitForSeconds(1f);
            nifflerHead.SetActive(true);
            yield return new WaitForSeconds(1f);
            nifflerPrintScore = true;
            yield return new WaitForSeconds(3f);
            nifflerHead.SetActive(false);
            nifflerPrintScore = false;
            yield return new WaitForSeconds(1f);
            firstBlackScreen.SetActive(false);
            lastBlackScreen.SetActive(true);
            yield return new WaitForSeconds(1f);
            canPlayCredits = true;;
            break;
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        PauseMenu.gamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
