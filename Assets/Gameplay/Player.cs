using UnityEngine;
using System.Collections;
using GamepadInput;
public class Player : MonoBehaviour
{
    Vector3 SpawnPoint;
    Quaternion SpawnRotation;
    public GameObject Suicider;
    public GameObject RebornParticle;
    public MeshRenderer[] TransparencyRenderers;
    public MeshRenderer Kubraczek;
    public GameObject DeathParticle;
    public GameObject BulletPrefab;
    public GamePad.Index PlayerIndex;
    CharacterController CharController;
    float JoyCalibrate = 10;
    bool BlockRotation = false;
    Transform Head;
    Transform Lufa;
    Color PlayerColor;
    bool IsDead = false;
    int TotalAxes = 12;
    int CurrentAxes = 10;
    public bool HasGameStarted = false;
    bool[] Grounded;
    int killstreak = 0;

    // Use this for initialization
    void Start()
    {
        Grounded = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            Grounded[i] = true;
        }
        SpawnPoint = this.transform.position;
        SpawnRotation = this.transform.rotation;
        CharController = GetComponent<CharacterController>();
        transform.FindChild("Right").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Front").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Left").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        transform.FindChild("Back").GetComponent<Raycaster>().OnMissingGround += OnMissingGround;
        Head = transform.FindChild("Head");
        Lufa = Head.transform.Find("Lufa");
        StartCoroutine(RespAxe());
    }

    // Update is called once per frame
    void Update()
    {
        if (!MenuManager.IsGameFinished)
        {
            Vector2 PadMovement = GamePad.GetAxis(GamePad.Axis.LeftStick, PlayerIndex);
            float MovementX = 0;
            float MovementY = 0;
            if (PadMovement.x != 0)
            {
                MovementX = PadMovement.x / JoyCalibrate;
            }
            if (PadMovement.y != 0)
            {
                MovementY = PadMovement.y / JoyCalibrate;
            }

            CharController.Move(-this.transform.forward * MovementY);
            CharController.Move(-this.transform.right * MovementX);
            //highRaycast
            RaycastHit Hit;
            Ray TempRay = new Ray(this.transform.position, -this.transform.up);
            Physics.Raycast(TempRay, out Hit, 1.0f);
            Debug.DrawRay(this.transform.position, -this.transform.up, Color.red);
            if (Hit.distance > 0.4f)
            {
                this.transform.position -= this.transform.up / 30;
            }
            if (GamePad.GetButtonDown(GamePad.Button.A, PlayerIndex))
            {
                if (!IsDead)
                {
                    if (CurrentAxes > 0)
                    {
                        if (HasGameStarted)
                        {
                            // bullet sound
                            SoundController.Singleton.CreateSound(ESoundType.Throw);
                            CurrentAxes--;
                            GameObject Bullet = Instantiate(BulletPrefab, Lufa.transform.position + Lufa.transform.forward * 0.5f, Lufa.transform.rotation) as GameObject;
                            Bullet.GetComponent<MeshRenderer>().material.color = PlayerColor;
                            Bullet.GetComponent<Bullet>().SetPlayerIndex(PlayerIndex, PlayerColor);
                            switch (PlayerIndex)
                            {
                                case GamePad.Index.Any:
                                    break;
                                case GamePad.Index.One:
                                    MenuManager.Singleton.Player1Stats[0] = CurrentAxes;
                                    break;
                                case GamePad.Index.Two:

                                    MenuManager.Singleton.Player2Stats[0] = CurrentAxes;
                                    break;
                                case GamePad.Index.Three:

                                    MenuManager.Singleton.Player3Stats[0] = CurrentAxes;
                                    break;
                                case GamePad.Index.Four:

                                    MenuManager.Singleton.Player4Stats[0] = CurrentAxes;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                MenuManager.Singleton.RefreshStats();
            }
            RotateHead(PadMovement);
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (!IsDead)
        {
            if (coll.gameObject.GetComponent<Bullet>())
            {
                Destroy(coll.gameObject);
                GetComponent<SphereCollider>().isTrigger = true;
                bool suicide = false;
                if (coll.gameObject.GetComponent<Bullet>().PlayerIndex == PlayerIndex)
                {
                    suicide = true;
                }


                if (suicide)
                {
                    //sound
                    SoundController.Singleton.CreateSound(ESoundType.Suicide);
                    switch (coll.gameObject.GetComponent<Bullet>().PlayerIndex)
                    {
                        case GamePad.Index.Any:
                            break;
                        case GamePad.Index.One:
                            MenuManager.Singleton.Player1Stats[1]--;
                            break;
                        case GamePad.Index.Two:

                            MenuManager.Singleton.Player2Stats[1]--;
                            break;
                        case GamePad.Index.Three:

                            MenuManager.Singleton.Player3Stats[1]--;
                            break;
                        case GamePad.Index.Four:
                            MenuManager.Singleton.Player4Stats[1]--;
                            break;
                        default:
                            break;
                    }

                    Suicider.GetComponent<UISuicide>().StartAnimation();
                }
                else
                {
                    //sound
                    SoundController.Singleton.CreateSound(ESoundType.Death);
                    int killerIndex = (int)coll.gameObject.GetComponent<Bullet>().PlayerIndex;
                    menuScript.Singleton.GameplayPlayer[killerIndex - 1].killstreak++;
                    int OpKillstreak = menuScript.Singleton.GameplayPlayer[killerIndex - 1].killstreak;
                    if (OpKillstreak > 3)
                    {
                        // sound
                        //killing spree
                       
                        SoundController.Singleton.CreateSound(ESoundType.Multikill);

                    }
                    else if (OpKillstreak > 2)
                    {
                        //mmmmmulti kill

                        SoundController.Singleton.CreateSound(ESoundType.KillingSpree);
                    }
                    switch (coll.gameObject.GetComponent<Bullet>().PlayerIndex)
                    {
                        case GamePad.Index.Any:
                            break;
                        case GamePad.Index.One:
                            MenuManager.Singleton.Player1Stats[1]++;
                            break;
                        case GamePad.Index.Two:

                            MenuManager.Singleton.Player2Stats[1]++;
                            break;
                        case GamePad.Index.Three:

                            MenuManager.Singleton.Player3Stats[1]++;
                            break;
                        case GamePad.Index.Four:
                            MenuManager.Singleton.Player4Stats[1]++;
                            break;
                        default:
                            break;
                    }
                }
                switch (PlayerIndex)
                {
                    case GamePad.Index.Any:
                        break;
                    case GamePad.Index.One:
                        MenuManager.Singleton.Player1Stats[2]++;
                        break;
                    case GamePad.Index.Two:

                        MenuManager.Singleton.Player2Stats[2]++;
                        break;
                    case GamePad.Index.Three:

                        MenuManager.Singleton.Player3Stats[2]++;
                        break;
                    case GamePad.Index.Four:

                        MenuManager.Singleton.Player4Stats[2]++;
                        break;
                    default:
                        break;
                }
                StartCoroutine(Respawn());
            }
        }
    }
    void RotateHead(Vector2 dirVector)
    {
        if (Mathf.Abs(dirVector.x) > Mathf.Abs(dirVector.y))
        {
            if (dirVector.x > 0)
            {
                Head.transform.localRotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else if (dirVector.x < 0)
            {
                Head.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
        }
        else
        {
            if (dirVector.y > 0)
            {
                Head.transform.localRotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
            }
            else if (dirVector.y < 0)
            {

                Head.transform.localRotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
            }
        }
    }

    void OnMissingGround(ERayCasterSide Side)
    {
        float DistancerDivider = 1.0f;
        if (!BlockRotation)
        {
            switch (Side)
            {
                case ERayCasterSide.Front:
                    this.transform.position += this.transform.forward / DistancerDivider;
                    this.transform.Rotate(90, 0, 0);
                    this.transform.position += this.transform.forward / DistancerDivider;
                    Grounded[0] = false;
                    break;
                case ERayCasterSide.Back:
                    this.transform.position -= this.transform.forward / DistancerDivider;
                    this.transform.Rotate(-90, 0, 0);
                    this.transform.position -= this.transform.forward / DistancerDivider;
                    Grounded[1] = false;
                    break;
                case ERayCasterSide.Left:
                    this.transform.position -= this.transform.right / DistancerDivider;
                    this.transform.Rotate(0, 0, 90);
                    this.transform.position -= this.transform.right / DistancerDivider;
                    Grounded[2] = false;
                    break;
                case ERayCasterSide.Right:
                    this.transform.position += this.transform.right / DistancerDivider;
                    this.transform.Rotate(0, 0, -90);
                    this.transform.position += this.transform.right / DistancerDivider;
                    Grounded[3] = false;
                    break;
                default:
                    break;
            }
            if (Vector3.Distance(this.transform.position, new Vector3(1.8f, -4.15f, 3.82f)) > 23)
            {
                this.transform.position = SpawnPoint;
                this.transform.rotation = SpawnRotation;
                Debug.Log("test " + Vector3.Distance(this.transform.position, new Vector3(1.8f, -4.15f, 3.82f)));
            }/* int UngroundedNum = 0;
            if (!Grounded[0]) UngroundedNum++;
            if (!Grounded[1]) UngroundedNum++;
            if (!Grounded[2]) UngroundedNum++;
            if (!Grounded[3]) UngroundedNum++;
            if (UngroundedNum > 3)
            {             //return to spawn position
                this.transform.position = SpawnPoint;
                this.transform.rotation = SpawnRotation;
                for (int i = 0; i < 4; i++)
                {
                    Grounded[i] = true;
                }
            }
            */

            StartCoroutine(RotationBlocker());
        }
    }
    IEnumerator RotationBlocker()
    {
        BlockRotation = true;
        yield return new WaitForSeconds(0.15f);
        BlockRotation = false;
    }
    IEnumerator Respawn()
    {
        killstreak = 0;

        GameObject Particle = Instantiate(DeathParticle, this.transform.position, this.transform.rotation) as GameObject;
        //Particle.GetComponent<ParticleSystem>().startColor = PlayerColor;
        Particle.transform.Rotate(-90, 0, 0);
        IsDead = true;
        foreach (MeshRenderer Rend in TransparencyRenderers)
        {
            Rend.material.color = new Color(Rend.material.color.r, Rend.material.color.g, Rend.material.color.b, 0.4f);
        }
        yield return new WaitForSeconds(5.0f);
        SoundController.Singleton.CreateSound(ESoundType.Respawn);
        foreach (MeshRenderer Rend in TransparencyRenderers)
        {
            Rend.material.color = new Color(Rend.material.color.r, Rend.material.color.g, Rend.material.color.b, 1);
        }
        Instantiate(RebornParticle, this.transform.position, this.transform.rotation);
        IsDead = false;
    }
    public void SetColor(Color colorToSet)
    {
        PlayerColor = colorToSet;
        Kubraczek.GetComponent<MeshRenderer>().material.color = colorToSet;
        GetComponent<MeshRenderer>().material.color = colorToSet;
    }
    IEnumerator RespAxe()
    {
        yield return new WaitForSeconds(2);
        //add axe 
        CurrentAxes++;
        if (CurrentAxes > TotalAxes)
        {
            CurrentAxes = TotalAxes;
        }
        switch (PlayerIndex)
        {
            case GamePad.Index.Any:
                break;
            case GamePad.Index.One:
                MenuManager.Singleton.Player1Stats[0] = CurrentAxes;
                break;
            case GamePad.Index.Two:

                MenuManager.Singleton.Player2Stats[0] = CurrentAxes;
                break;
            case GamePad.Index.Three:

                MenuManager.Singleton.Player3Stats[0] = CurrentAxes;
                break;
            case GamePad.Index.Four:

                MenuManager.Singleton.Player4Stats[0] = CurrentAxes;
                break;
            default:
                break;
        }
        StartCoroutine(RespAxe());
    }
}
