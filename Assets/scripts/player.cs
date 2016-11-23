using UnityEngine;
using System.Collections;
using GamepadInput;

public class player : MonoBehaviour {
    public int color;
    public GameObject menuObject;
    public bool isOnChangingColor = false;
    public bool isConnected = false;
    public bool isAccepted = false;
    public bool isAPushed = false;
    public GamePad.Index playerIndex;
    public string nameOfPlayer;
    // Use this for initialization
    void Start () {
        menuObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = nameOfPlayer;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
