using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISuicide : MonoBehaviour {

    bool ScaleUp = false;
    bool ScaleDown = false;
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ScaleUp) this.transform.localScale *= 1.005f;
        if (ScaleDown) this.transform.localScale *= 0.97f;
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, Random.Range(0, 1.0f), GetComponent<Image>().color.b);
    }
    IEnumerator Scale()
    { 
        ScaleUp = true;
        yield return new WaitForSeconds(0.5f);
        ScaleUp = false;
        ScaleDown = true;
        yield return new WaitForSeconds(0.5f);
        ScaleDown = false;
        gameObject.SetActive(false);
    }
    public void StartAnimation()
    {
        this.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        gameObject.SetActive(true);
        StartCoroutine(Scale());
    }
}
