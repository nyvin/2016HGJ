using UnityEngine;
using System.Collections;
using GamepadInput;

public class Bullet : MonoBehaviour {

    public MeshRenderer Trzonek;
    // Use this for initialization
    public GameObject AxeExplosion;
    CharacterController CharController;
    bool BlockRotation = false;
    public GamePad.Index PlayerIndex;

    Color PlayerColor;
    void Start() {
        CharController = GetComponent<CharacterController>();
        transform.FindChild("Right").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Front").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Left").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Back").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        StartCoroutine(EnableColider());
        StartCoroutine(Destroy());
    }

    public void SetPlayerIndex(GamePad.Index Index,Color PlayerColor)
    {
        this.PlayerColor = PlayerColor;
        PlayerIndex = Index;
        Trzonek.GetComponent<MeshRenderer>().materials[0].color = PlayerColor;
    }
	// Update is called once per frame
	void Update () {
        this.transform.position += this.transform.forward / 5;   
        //highRaycast
        RaycastHit Hit;
        Ray TempRay = new Ray(this.transform.position, -this.transform.up);
        Physics.Raycast(TempRay, out Hit, 1.0f);
        Debug.DrawRay(this.transform.position, -this.transform.up, Color.red);
        if (Hit.distance > 0.5f)
        {
            this.transform.position -= this.transform.up / 30;
        }
    }
    void OnMissingGround(ERayCasterSide Side)
    {
        float DistancerDivider = 2.5f;
        if (!BlockRotation)
        {
            switch (Side)
            {
                case ERayCasterSide.Front:
                    this.transform.position += this.transform.forward / DistancerDivider;
                    this.transform.Rotate(90, 0, 0);
                    this.transform.position += this.transform.forward / DistancerDivider;
                    break;
                case ERayCasterSide.Back:
                    this.transform.position -= this.transform.forward / DistancerDivider;
                    this.transform.Rotate(-90, 0, 0);
                    this.transform.position -= this.transform.forward / DistancerDivider;
                    break;
                case ERayCasterSide.Left:
                    this.transform.position -= this.transform.right / DistancerDivider;
                    this.transform.Rotate(0, 0, 90);
                    this.transform.position -= this.transform.right / DistancerDivider;
                    break;
                case ERayCasterSide.Right:
                    this.transform.position += this.transform.right / DistancerDivider;
                    this.transform.Rotate(0, 0, -90);
                    this.transform.position += this.transform.right / DistancerDivider;
                    break;
                default:
                    break;
            }
            StartCoroutine(RotationBlocker());
        }
    }
    IEnumerator EnableColider()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<SphereCollider>().enabled = true;
    }
    IEnumerator RotationBlocker()
    {
        BlockRotation = true;
        yield return new WaitForSeconds(0.05f);
        BlockRotation = false;
    }
    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.GetComponent<Bullet>())
        {
            //sound throw
            SoundController.Singleton.CreateSound(ESoundType.Clash);
            GameObject Particle = Instantiate(AxeExplosion, this.transform.position, this.transform.rotation) as GameObject;
            Particle.GetComponent<ParticleSystem>().startColor = PlayerColor;
            Destroy(this.gameObject);
            Debug.Log(PlayerIndex+" hit by axe of" +coll.gameObject.GetComponent<Bullet>().PlayerIndex);
        }
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }
}
