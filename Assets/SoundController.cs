using UnityEngine;
using System.Collections;

public enum ESoundType
{
    Death,
    Suicide,
    KillingSpree,
    Throw,
    Multikill,
    Clash,
    Respawn,
}
public class SoundController : MonoBehaviour {

    public static SoundController Singleton;
    public AudioClip Death;
    public AudioClip Suicide;
    public AudioClip KillingSpree;
    public AudioClip Throw;
    public AudioClip Clash;
    public AudioClip MultiKill;
    public AudioClip Resp;
    void Awake()
    {
        if (Singleton)
        {
            Destroy(Singleton.gameObject);
        }
        Singleton = this;

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateSound(ESoundType TypeToCreate)
    {
        GameObject TempSound = new GameObject();
        AudioSource AS=  TempSound.AddComponent<AudioSource>();
        switch (TypeToCreate)
        {
            case ESoundType.Death:
                AS.clip = Death;
                break;
            case ESoundType.Suicide:
                AS.clip = Suicide;
                break;
            case ESoundType.KillingSpree:
                AS.clip = KillingSpree;
                break;
            case ESoundType.Throw:
                AS.clip = Throw;
                break;
            case ESoundType.Clash:
                AS.clip = Clash;
                break;
            case ESoundType.Multikill:
                AS.clip = MultiKill;
                break;
            case ESoundType.Respawn:
                AS.clip = Resp;
                break;
            default:
                break;
        }
        AS.Play();
        TempSound.AddComponent<TimerDestroyer>();

    }
}
