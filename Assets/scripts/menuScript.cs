using UnityEngine;
using System.Collections;
using GamepadInput;

public class menuScript : MonoBehaviour {

    public static menuScript Singleton;
    public Material defaultMaterial;
    public static int numberOfColors = 8;
    public Material[] colors = new Material[numberOfColors];
    public bool[] reserverColors = new bool[numberOfColors];
    public player[] players = new player[4];
    public Player[] GameplayPlayer = new Player[4];
    private int numberOfConnectedPlayers = 0;
    private int numberOfAcceptedPlayers = 0;
    public GameObject UIContener;
    public Color textColor;
    float fadeOfColor = 0;
    public GameObject title;
    // Use this for initialization
    void Awake()
    {
        if (Singleton)
        {
            Destroy(Singleton.gameObject);
        }
        Singleton = this;
    }
    void Start () {
        QualitySettings.antiAliasing = 8;
        for(int i = 0; i < 4; i++)
        {
            players[i].menuObject.GetComponent<Renderer>().material = defaultMaterial;
        }
        for (int i = 0; i < numberOfColors; i++)
        {
            reserverColors[i] = false;
        }
    }
    // Update is called once per frame
    void Update () {
        for(int i = 0; i < 4; i++)
        {
            //Connecting
            if (players[i].isConnected == false)
            {
                if (GamePad.GetButtonDown(GamePad.Button.A, players[i].playerIndex))
                {
                    Connect(ref players[i]);
                    players[i].isOnChangingColor = true;
                    players[i].isAPushed = true;
                }
            }
            if(players[i].isAPushed == true)
            {
                if (GamePad.GetButtonUp(GamePad.Button.A, players[i].playerIndex))
                {
                    players[i].isAPushed = false;
                }
            }
            //Choosing color
            if (players[i].isOnChangingColor == true)
            {
                if (GamePad.GetAxis(GamePad.Axis.LeftStick, players[i].playerIndex).x > 0.9)
                {
                    ChangeColorNext(ref players[i]);
                    players[i].menuObject.GetComponent<Renderer>().material = colors[players[i].color];
                    players[i].isOnChangingColor = false;

                }
                else if(GamePad.GetAxis(GamePad.Axis.LeftStick, players[i].playerIndex).x < -0.9)
                {
                    ChangeColorPrevious(ref players[i]);
                    players[i].menuObject.GetComponent<Renderer>().material = colors[players[i].color];
                    players[i].isOnChangingColor = false;
                }  
            }
            else if (GamePad.GetAxis(GamePad.Axis.LeftStick, players[i].playerIndex).x == 0.0 && players[i].isAccepted == false && players[i].isConnected == true)
            {
                players[i].isOnChangingColor = true;
            }

            //Accepting
            if(players[i].isAccepted == false && players[i].isAPushed == false)
            {
                if (GamePad.GetButtonDown(GamePad.Button.A, players[i].playerIndex))
                {
                    if(reserverColors[players[i].color] == false)
                    {
                        reserverColors[players[i].color] = true;
                        players[i].menuObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>().color = new Color(textColor.r, textColor.g, textColor.b, 1.0f);
                        AcceptColor(ref players[i]);
                        players[i].isOnChangingColor = false;
                        GameplayPlayer[i].gameObject.SetActive(true);
                        GameplayPlayer[i].SetColor(colors[players[i].color].color);
                        //MenuManager.Singleton.Panels[i].color = colors[players[i].color].color;
                        MenuManager.Singleton.Panels[i].material = colors[players[i].color];
                    }
                }
            }

            //changing fade of text
            if(players[i].isAccepted == false)
            {
                fadeOfColor += 0.03f;
                players[i].menuObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>().color = new Color(textColor.r, textColor.g, textColor.b,Mathf.Cos(fadeOfColor));
            }
        }

        if (numberOfAcceptedPlayers == 4)
        {
            Debug.Log("END OF MENU");
            foreach (Player pl in GameplayPlayer)
            {
                pl.HasGameStarted = true;
            }
            UIContener.gameObject.SetActive(false);
            title.SetActive(false);
        }
    }

    void Connect(ref player p)
    {
        p.isConnected = true;
        p.color = 0;
        p.menuObject.GetComponent<Renderer>().material = colors[p.color];
        p.menuObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "'A' to accept";
        numberOfConnectedPlayers++;
    }

    void ChangeColorNext(ref player p)
    {
        p.color++;
        if (p.color >= numberOfColors)
        {
            p.color = 0;
        }
    }

    void ChangeColorPrevious(ref player p)
    {
        p.color--;
        if(p.color < 0)
        {
            p.color = numberOfColors - 1;
        }
    }

    void AcceptColor(ref player p)
    {
        p.isAccepted = true;
        numberOfAcceptedPlayers++;
        p.menuObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = "Accepted!";
    }
}
