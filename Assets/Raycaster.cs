using UnityEngine;
using System.Collections;
public enum ERayCasterSide
{
    Front,
    Back,
    Left,
    Right
}
public class Raycaster : MonoBehaviour
{
    public delegate void MissingGround(ERayCasterSide Side);
    public event MissingGround OnMissingGround;
    public ERayCasterSide ThisSide;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        Ray TempRay = new Ray(this.transform.position, -this.transform.up);
        Physics.Raycast(TempRay, out Hit, 1.0f);
        Debug.DrawRay(this.transform.position, -this.transform.up, Color.red);

        if (Hit.collider == null)
        {
            if (OnMissingGround != null) OnMissingGround(ThisSide);

        }

    }
}
