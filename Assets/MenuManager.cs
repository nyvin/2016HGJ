using UnityEngine;
using UnityEngine.UI;
using GamepadInput;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{

    // Use this for initialization
    public static MenuManager Singleton;

    public static bool IsGameFinished = false;
    public Image[] Panels;
    public Text[] Player1Texts;
    public Text[] Player2Texts;
    public Text[] Player3Texts;
    public Text[] Player4Texts;

    public int[] Player1Stats;
    public int[] Player2Stats;
    public int[] Player3Stats;
    public int[] Player4Stats;

    Image WinninPanel;
    void Awake()
    {
        if (Singleton)
        {
            Destroy(Singleton.gameObject);
        }
        Singleton = this;
        
        Player1Stats = new int[3];
        Player2Stats = new int[3];
        Player3Stats = new int[3];
        Player4Stats = new int[3];

        for (int i = 0; i < 3; i++)
        {
            Player1Stats[i] = 0;
            Player2Stats[i] = 0;
            Player3Stats[i] = 0;
            Player4Stats[i] = 0;
        }

    }
    void Start()
    {
        IsGameFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        int winningAmount =10;
        if (!IsGameFinished)
        {

            for (int i = 0; i < 4; i++)
            {
                if (Player1Stats[1] >= winningAmount)
                {
                    WinninPanel = Panels[0];
                    IsGameFinished = true;
                }
                if (Player2Stats[1] >= winningAmount)
                {
                    WinninPanel = Panels[1];
                    IsGameFinished = true;
                }
                if (Player3Stats[1] >= winningAmount)
                {
                    WinninPanel = Panels[2];
                    IsGameFinished = true;
                }
                if (Player4Stats[1] >= winningAmount)
                {
                    WinninPanel = Panels[3];
                    IsGameFinished = true;
                }
            }
        }
        if (IsGameFinished)
        {
            if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
            {
                SceneManager.LoadScene(0);
            }
            WinnerImageAnimatin(WinninPanel);
            
        }

        RefreshStats();
    }
    void WinnerImageAnimatin(Image Panel)
    {
        Panel.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
    public void RefreshStats()
    {
        Player1Texts[0].text = "AXE: " + Player1Stats[0];
        Player1Texts[1].text = "Kills: " + Player1Stats[1];
        Player1Texts[2].text = "Deaths: " + Player1Stats[2];

        Player2Texts[0].text = "AXE: " + Player2Stats[0];
        Player2Texts[1].text = "Kills: " + Player2Stats[1];
        Player2Texts[2].text = "Deaths: " + Player2Stats[2];

        Player3Texts[0].text = "AXE: " + Player3Stats[0];
        Player3Texts[1].text = "Kills: " + Player3Stats[1];
        Player3Texts[2].text = "Deaths: " + Player3Stats[2];

        Player4Texts[0].text = "AXE: " + Player4Stats[0];
        Player4Texts[1].text = "Kills: " + Player4Stats[1];
        Player4Texts[2].text = "Deaths: " + Player4Stats[2];
    }
}
