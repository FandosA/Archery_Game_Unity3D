using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_VR : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // Catch variables
    public GameObject arrow = null; // Arrow GameObject
    private GameObject bow; // Bow GameObject
    public float throwForce = 4000.0f; // Throwing force

    private Vector3 arrowInHandsPos = new Vector3(0.0880127f, -0.2420006f, -0.01998901f);
    private Vector3 arrowInHandsRot = new Vector3(3.311f, 353.949f, 0.0f);

    private Vector3 arrowInTablePos = new Vector3(-0.0222f, 0.0092f, 0.03123992f);
    private Vector3 arrowInTableRot = new Vector3(0f, 90f, 90f);

    private Vector3 arrowAimPos = new Vector3(0.0f, -0.043f, 0.15f);
    private Vector3 arrowAimRot = new Vector3(180.155f, 177.086f, 45f);

    private Vector3 bowInHandsPos = new Vector3(-0.06f, -0.37f, 1.59f);
    private Vector3 bowInHandsRot = new Vector3(-73.214f, -123.23f, 214.077f);

    private Vector3 bowInTablePos = new Vector3(-0.002130486f, -0.004807826f, 0.0306052f);
    private Vector3 bowInTableRot = new Vector3(-180.553f, -184.282f, 279.476f);

    private Vector3 bowAimPos = new Vector3(-0.00994873f, 0.08805847f, 2.201263f);
    private Vector3 bowAimRot = new Vector3(4f, -87.538f, 189.321f);
    ////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////
    // Movement
    public float WalkSpeed = 15.0f; // Regular speed
    public float RunSpeed = 40.0f; //  Run speed
    ////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////
    // Buttons of the controller
    private bool RunButtonPressed;
    private bool AimButtonPressed;
    private bool AimButtonGetDown;
    private bool ShootButtonPressed;
    private bool LeaveOnTableButtonDown;
    ////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////
    // Variables for the timer
    private float timerAfterShoot = 3.0f;
    private float waitTime = 2.0f;
    ////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////
    // Audios
    private AudioSource audios;
    public AudioClip carga_arco;
    ////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////
    // Attention redirecting variables
    private GameObject target_item;
    private GameObject target_icon;
    private GameObject target_item2;
    private GameObject target_icon2;
    private GameObject target_item3;
    private GameObject target_icon3;
    private GameObject target_item4;
    private GameObject target_icon4;
    private GameObject target_item5;
    private GameObject target_icon5;
    private GameObject target_item6;
    private GameObject target_icon6;
    private GameObject target_mobile;
    private GameObject arrow_item;
    private GameObject arrow_icon;
    private GameObject bow_item;
    private GameObject bow_icon;
    private const float limit_inf = -438.0f;
    private const float limit_sup = 451.0f;
    private float angle_player_item;
    private float time_not_taking_arrow = 0.0f;
    private float timer_sparkles = 0.0f;
    private GameObject sparkles;
    ////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////
    // Variables to leave the arrow and bow on the table
    private GameObject table;
    private float dist2LeaveArrowBow = 5.0f;
    private bool arrowInTable = false;
    private GameObject pressQpanel;
    private GameObject pressQleaveARROW;
    private GameObject pressQtakeARROW;
    private GameObject TakeArrowToLeave;
    private Vector3 BowScale = new Vector3(2.463147f, 1.658835f, 1.950424f);
    private Vector3 ArrowScale = new Vector3(4f, 4f, 4f);
    ////////////////////////////////////////////////////////////////////////
    
    ///////////////////////////////////////////////////////////////////////////
    // Variables to move the target
    public float targetMobile_speed = 1.0f;
    private bool ascending = true;
    ////////////////////////////////////////////////////////////////////////

    void Start()
    {
        bow = GameObject.Find("Bow");
        bow_item = bow;
        bow_icon = GameObject.Find("bow_icon");
        bow_icon.SetActive(false);
        audios = GetComponent<AudioSource>();
        target_item = GameObject.Find("/Diana/Target");
        target_icon = GameObject.Find("target_icon");
        target_item2 = GameObject.Find("/Diana2/Target");
        target_icon2 = GameObject.Find("target_icon2");
        target_item3 = GameObject.Find("/Diana3/Target");
        target_icon3 = GameObject.Find("target_icon3");
        target_item4 = GameObject.Find("/Diana4/Target");
        target_icon4 = GameObject.Find("target_icon4");
        target_item5 = GameObject.Find("/Diana5/Target");
        target_icon5 = GameObject.Find("target_icon5");
        target_item6 = GameObject.Find("/Diana6/Target");
        target_icon6 = GameObject.Find("target_icon6");
        target_mobile = GameObject.Find("Target_mobile");
        arrow_item = GameObject.Find("/CameraParent/Camera/Arrow");
        arrow_icon = GameObject.Find("arrow_icon");
        table = GameObject.Find("Table");
        sparkles = GameObject.Find("FX_Sparkle");
        sparkles.SetActive(false);

        pressQleaveARROW = GameObject.Find("/PressQtoLeaveArrow/QleaveArrow");
        pressQtakeARROW = GameObject.Find("/PressQtoLeaveArrow/QtakeArrow");
        pressQpanel = GameObject.Find("/PressQtoLeaveArrow/Panel_leaveArow");
        TakeArrowToLeave = GameObject.Find("/PressQtoLeaveArrow/TakeArrowToLeave");
        pressQpanel.SetActive(false);
        pressQtakeARROW.SetActive(false);
        pressQleaveARROW.SetActive(false);
        TakeArrowToLeave.SetActive(false);
    }

    void Update()
    {
        timerAfterShoot += Time.deltaTime;
        if (arrow == null && !arrowInTable)
            time_not_taking_arrow += Time.deltaTime;

        SetIcon(target_item, target_icon);
        SetIcon(target_item2, target_icon2);
        SetIcon(target_item3, target_icon3);
        SetIcon(target_item4, target_icon4);
        SetIcon(target_item5, target_icon5);
        SetIcon(target_item6, target_icon6);
        SetIcon(arrow_item, arrow_icon);
        SetIcon(bow_item, bow_icon);

        if (time_not_taking_arrow > 20.0f)
        {
            timer_sparkles += Time.deltaTime;
            if (timer_sparkles < 10.0f)
            {
                float angle_to_arrow = ComputeAnglesBetween(arrow_item);
                angle_to_arrow = Mathf.Clamp(angle_to_arrow, -50.0f, 50.0f);
                sparkles.transform.localPosition = new Vector3(angle_to_arrow / 5.0f, 0.0f, 10.0f);
                sparkles.SetActive(true);
            }
            else
            {
                sparkles.SetActive(false);
                time_not_taking_arrow = 5.0f;
                timer_sparkles = 0.0f;
            }
        }

        if (ascending)
        {
            target_mobile.transform.localPosition += new Vector3(0.0f, targetMobile_speed * Time.deltaTime, 0.0f);
            if (target_mobile.transform.localPosition.y >= -30f) ascending = false;
        }
        else
        {
            target_mobile.transform.localPosition -= new Vector3(0.0f, targetMobile_speed * Time.deltaTime, 0.0f);
            if (target_mobile.transform.localPosition.y <= -41f) ascending = true;
        }

        GetPressedButtons();

        PlayerMovement();

        ShootArrow();

        LeaveArrowAndBow();
    }

    void GetPressedButtons()
    {
        RunButtonPressed = Input.GetKey(KeyCode.Joystick1Button1); // D
        AimButtonPressed = Input.GetKey(KeyCode.Joystick1Button0); // B
        AimButtonGetDown = Input.GetKeyDown(KeyCode.Joystick1Button0);
        ShootButtonPressed = Input.GetKey(KeyCode.Joystick1Button2); // C
        LeaveOnTableButtonDown = Input.GetKey(KeyCode.Joystick1Button3); // A
    }

    void PlayerMovement()
    {
        Vector3 PlayerTranslation = Vector3.zero;
        if (Input.GetAxis("Vertical") > 0) PlayerTranslation += transform.forward;
        else if (Input.GetAxis("Vertical") < 0) PlayerTranslation -= transform.forward;
        else if (Input.GetAxis("Horizontal") < 0) PlayerTranslation -= transform.right;
        else if (Input.GetAxis("Horizontal") > 0) PlayerTranslation += transform.right;

        PlayerTranslation.z = PlayerTranslation.z / Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.x);

        if (RunButtonPressed) PlayerTranslation *= RunSpeed * Time.deltaTime;
        else PlayerTranslation *= WalkSpeed * Time.deltaTime;

        transform.Translate(PlayerTranslation);
        Vector3 newPosition = transform.position;
        newPosition.y = Terrain.activeTerrain.SampleHeight(transform.position) + 5.0f;
        transform.position = newPosition;
    }

    void OnCollisionEnter(Collision col)
    {
        print("Colision antes del if");
        if (timerAfterShoot >= waitTime)
        {
            if (col.gameObject.tag == "Arrow" && arrow == null && !arrowInTable)
            {
                print("Colision despues del if");
                arrow = col.gameObject;
                arrow.GetComponent<Rigidbody>().isKinematic = true;
                arrow.GetComponent<Rigidbody>().useGravity = false;
                time_not_taking_arrow = 0.0f;
            }
        }
    }

    void ShootArrow()
    {
        if (arrow != null)
        {
            arrow_icon.SetActive(false);
            if (!AimButtonPressed && !ShootButtonPressed)
            {
                arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
                arrow.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                arrow.transform.SetParent(transform.GetChild(0));
                arrow.transform.localPosition = arrowInHandsPos;
                arrow.transform.localEulerAngles = arrowInHandsRot;
                arrow.GetComponent<Rigidbody>().isKinematic = true;
                bow.transform.localPosition = bowInHandsPos;
                bow.transform.localEulerAngles = bowInHandsRot;
            }
            else if (AimButtonPressed)
            {
                // Point
                if (AimButtonGetDown)
                    audios.PlayOneShot(carga_arco, 1.0f);
                bow.transform.localPosition = bowAimPos;
                bow.transform.localEulerAngles = bowAimRot;
                arrow.transform.localPosition = arrowAimPos;
                arrow.transform.localEulerAngles = arrowAimRot;
                if (ShootButtonPressed)
                {
                    // Throw
                    Physics.IgnoreCollision(this.GetComponent<Collider>(), arrow.GetComponent<Collider>());
                    RegainCollisionAfterTime(this.GetComponent<Collider>(), arrow.GetComponent<Collider>(), 0.5f);
                    arrow.GetComponent<Rigidbody>().isKinematic = false;
                    arrow.transform.SetParent(null);
                    arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * throwForce);
                    arrow.GetComponent<Rigidbody>().useGravity = true;
                    arrow = null;
                    timerAfterShoot = 0.0f;
                }
            }
            else if (ShootButtonPressed)
            {
                // Throw
                Physics.IgnoreCollision(this.GetComponent<Collider>(), arrow.GetComponent<Collider>());
                RegainCollisionAfterTime(this.GetComponent<Collider>(), arrow.GetComponent<Collider>(), 0.5f);
                arrow.GetComponent<Rigidbody>().isKinematic = false;
                arrow.transform.SetParent(null);
                arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * throwForce);
                arrow.GetComponent<Rigidbody>().useGravity = true;
                arrow = null;
                timerAfterShoot = 0.0f;
            }
        }
        else
        {
            if (!arrowInTable)
            {
                arrow_icon.SetActive(true);
                if (AimButtonPressed)
                {
                    // Point
                    bow.transform.localPosition = bowAimPos;
                    bow.transform.localEulerAngles = bowAimRot;
                }
                else
                {
                    bow.transform.localPosition = bowInHandsPos;
                    bow.transform.localEulerAngles = bowInHandsRot;
                }
            }
        }
    }

    IEnumerator RegainCollisionAfterTime(Collider a, Collider b, float time)
    {
        print("Ignorando la colision");
        yield return new WaitForSeconds(time);
        Physics.IgnoreCollision(a, b, false);
        print("Colision recuperada");
    }

    private float ComputeAnglesBetween(GameObject item)
    {
        Vector2 vector_parent_object = new Vector2(item.transform.position.x - transform.position.x, item.transform.position.z - transform.position.z);
        Vector2 player_forward = new Vector2(transform.forward.x, transform.forward.z);
        player_forward.Normalize();
        vector_parent_object.Normalize();
        return Vector2.SignedAngle(vector_parent_object, player_forward);
    }

    void SetIcon(GameObject item, GameObject icon)
    {
        float angle = ComputeAnglesBetween(item);
        icon.GetComponent<RectTransform>().localPosition = new Vector3((limit_sup + limit_inf) / 2.0f + angle / 360.0f * (limit_sup - limit_inf),
                                                                        icon.GetComponent<RectTransform>().localPosition.y,
                                                                        icon.GetComponent<RectTransform>().localPosition.z);
    }

    void LeaveArrowAndBow()
    {
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - table.transform.position.x, 2) + Mathf.Pow(transform.position.z - table.transform.position.z, 2)) < dist2LeaveArrowBow)
        {
            pressQpanel.SetActive(true);
            if (arrowInTable == false)
            {
                pressQtakeARROW.SetActive(false);
                pressQleaveARROW.SetActive(true);
                TakeArrowToLeave.SetActive(false);
            }

            if (arrowInTable == true && arrow_item.transform.IsChildOf(table.transform))
            {
                pressQtakeARROW.SetActive(true);
                pressQleaveARROW.SetActive(false);
                TakeArrowToLeave.SetActive(false);
            }

            if (!arrow_item.transform.IsChildOf(Camera.main.transform) && !arrow_item.transform.IsChildOf(table.transform))
            {
                pressQtakeARROW.SetActive(false);
                pressQleaveARROW.SetActive(false);
                TakeArrowToLeave.SetActive(true);
            }

            if (LeaveOnTableButtonDown)
            {
                if (arrowInTable == false)
                {
                    if (arrow_item.transform.IsChildOf(Camera.main.transform))
                    {
                        bow_icon.SetActive(true);
                        arrow_icon.SetActive(true);
                        arrowInTable = true;
                        arrow_item.transform.SetParent(table.transform);
                        arrow_item.transform.localPosition = arrowInTablePos;
                        arrow_item.transform.localEulerAngles = arrowInTableRot;
                        bow.transform.SetParent(table.transform);
                        bow.transform.localPosition = bowInTablePos;
                        bow.transform.localEulerAngles = bowInTableRot;
                        arrow = null;
                    }
                }
                else
                {
                    bow_icon.SetActive(false);
                    arrowInTable = false;
                    arrow = GameObject.Find("/Table/Arrow");
                    arrow.transform.SetParent(Camera.main.transform);
                    arrow.transform.localScale = ArrowScale;
                    arrow.transform.localPosition = arrowInHandsPos;
                    arrow.transform.localEulerAngles = arrowInHandsRot;
                    bow.transform.SetParent(Camera.main.transform);
                    bow.transform.localScale = BowScale;
                    bow.transform.localPosition = bowInHandsPos;
                    bow.transform.localEulerAngles = bowInHandsRot;
                }
            }
        }
        else
        {
            pressQpanel.SetActive(false);
            pressQtakeARROW.SetActive(false);
            TakeArrowToLeave.SetActive(false);
            pressQleaveARROW.SetActive(false);
        }
    }
}