using UnityEngine;
using System.Collections;

public class Rotatior : MonoBehaviour {

    // Use this for initialization
    float Yrot;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Yrot += 6;
        this.transform.localRotation = Quaternion.Euler(Yrot, 0, 0);
	}
}
