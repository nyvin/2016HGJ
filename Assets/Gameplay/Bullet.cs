using UnityEngine;
using System.Collections;
using GamepadInput;
using System;

public class Bullet : MonoBehaviour {

    public MeshRenderer Trzonek;
    // Use this for initialization
    public GameObject AxeExplosion;
    CharacterController CharController;
    bool BlockRotation = false;
    public GamePad.Index PlayerIndex;
    public bool[] leftSides;

    Color PlayerColor;
    void Start() {
        leftSides = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            leftSides[i] = false;
        }
        CharController = GetComponent<CharacterController>();
        transform.FindChild("Right").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Front").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Left").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Back").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        StartCoroutine(EnableColider());
    }

    public void SetPlayerIndex(GamePad.Index Index,Color PlayerColor)
    {
        this.PlayerColor = PlayerColor;
        PlayerIndex = Index;
        Trzonek.GetComponent<MeshRenderer>().materials[0].color = PlayerColor;
    }
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.localPosition += transform.forward * Time.deltaTime;
        }
        
        
        //highRaycast
        RaycastHit Hit;
        Ray TempRay = new Ray(this.transform.position, -this.transform.up);
        
        Debug.DrawRay(this.transform.position, -this.transform.up, Color.blue);
        if (Physics.Raycast(TempRay, out Hit, 1.0f))
        {
            if (Mathf.Abs(Hit.distance - 0.25f) > 0.001f)
            {
                //this.transform.position -= this.transform.up / 30;
                Debug.Log(Hit.point);
                this.transform.position = Hit.point + transform.up * 0.25f;
                Debug.Log(transform.position);
            }
        } 
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
        for (int i = 0; i < 4; i++)
        {
            leftSides[i] = false;
        }
    }

    void OnMissingGround(ERayCasterSide Side)
    {
        //float DistancerDivider = 2.5f;
        leftSides[(int)Side] = true;
        Debug.Log(Side);
        if (!BlockRotation && checkSides())
        {
            Side = getSide();
            switch(Side)
            {
                case ERayCasterSide.Front:
                    Debug.Log(this.transform.forward);
                    this.transform.localPosition += this.transform.forward * this.transform.localScale.x * 0.5f;
                    this.transform.Rotate(90, 0, 0);
                    this.transform.localPosition += this.transform.forward * this.transform.localScale.x * 0.5f * 1.001f;
                    //this.transform.localPosition += this.transform.forward;
                    Debug.Log("front");
                    break;
                //case ERayCasterSide.Back:
                //    this.transform.Rotate(-90, 0, 0);
                //    break;
                //case ERayCasterSide.Left:
                //    this.transform.Rotate(0, 0, 90);
                //    break;
                //case ERayCasterSide.Right:
                //    this.transform.Rotate(0, 0, -90);
                //    break;
                default:
                    break;
            }


            //switch (Side)
            //{
            //    case ERayCasterSide.Front:
            //        this.transform.position += this.transform.forward / DistancerDivider;
            //        this.transform.Rotate(90, 0, 0);
            //        this.transform.position += this.transform.forward / DistancerDivider;
            //        break;
            //    case ERayCasterSide.Back:
            //        this.transform.position -= this.transform.forward / DistancerDivider;
            //        this.transform.Rotate(-90, 0, 0);
            //        this.transform.position -= this.transform.forward / DistancerDivider;
            //        break;
            //    case ERayCasterSide.Left:
            //        this.transform.position -= this.transform.right / DistancerDivider;
            //        this.transform.Rotate(0, 0, 90);
            //        this.transform.position -= this.transform.right / DistancerDivider;
            //        break;
            //    case ERayCasterSide.Right:
            //        this.transform.position += this.transform.right / DistancerDivider;
            //        this.transform.Rotate(0, 0, -90);
            //        this.transform.position += this.transform.right / DistancerDivider;
            //        break;
            //    default:
            //        break;
            //}
            for (int i = 0; i < 4; i++)
            {
                leftSides[i] = false;
            }
            StartCoroutine(RotationBlocker());
        }
    }

    private ERayCasterSide getSide()
    {
        if (leftSides[0] && leftSides[1] && leftSides[2])
            return (ERayCasterSide)1;
        if (leftSides[1] && leftSides[2] && leftSides[3])
            return (ERayCasterSide)2;
        if (leftSides[2] && leftSides[3] && leftSides[0])
            return (ERayCasterSide)3;
        if (leftSides[3] && leftSides[0] && leftSides[1])
            return (ERayCasterSide)0;
        throw new NotImplementedException();
    }

    private bool checkSides()
    {
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += leftSides[i] ? 1 : 0;
        }
        return sum > 2;
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
}
