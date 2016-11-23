using UnityEngine;
using System.Collections;

public class TimerDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterDelay());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }
}
