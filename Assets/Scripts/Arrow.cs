using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    private Rigidbody arrow_rb;
    Collider arrow_collider;
    private bool stuck = false;
    private bool launched = false;
    private bool in_player = false;
    private Vector3 Player_pos;
    private float pickup_distance;
    private int score;
    private Vector3 rotation;
    private Vector3 target_center;

    public AudioClip Acierto;
    public AudioClip ninos_bien;
    public AudioClip Fallo;
    public AudioClip disparo;
    public AudioClip arrow_coll;
    private AudioSource audios;

    private int TotalScore;
    public Text total_score;
    public Text curr_score;
    private GameObject canvas_currScore, confeti;
    private float timer;
    private float timeFinishCelebration;

    private GameObject target;
    private Vector3 collisionPoint;

    private Transform parent;
    private Vector3 posRelToParent;

    void Start()
    {
        arrow_rb = GetComponent<Rigidbody>();
        arrow_collider = GetComponent<Collider>();
        pickup_distance = GameObject.Find("CameraParent").GetComponent<CapsuleCollider>().radius;
        audios = GetComponent<AudioSource>();
        TotalScore = 0;
        canvas_currScore = GameObject.Find("CurrScore");
        canvas_currScore.SetActive(false);
        confeti = GameObject.Find("Confeti");
        confeti.SetActive(false);
        timeFinishCelebration = 3.0f;
        timer = 3.5f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        rotation = transform.eulerAngles;

        if (transform.parent != null && transform.parent.name == "Camera")
        {
            parent = null;
            in_player = true;
            stuck = false;
        }

        if (in_player && transform.parent == null)
        {
            audios.PlayOneShot(disparo, 1.0f);
            in_player = false;
            launched = true;
        }

        if (Input.GetKey(KeyCode.R))
        {
            TotalScore = 0;
            total_score.text = "Total score: " + TotalScore;
        }

        if (parent != null)
        {
            transform.position = parent.position + posRelToParent;
        }

        ArrowTrajectory();

        AllowToTakeArrow();

        if (timer > timeFinishCelebration)
        {
            canvas_currScore.SetActive(false);
            confeti.SetActive(false);
        }

    }

    void AllowToTakeArrow()
    {
        Player_pos = GameObject.Find("CameraParent").transform.position;
        if (stuck && Mathf.Sqrt(Mathf.Pow(transform.position.x - Player_pos.x, 2) + Mathf.Pow(transform.position.z - Player_pos.z, 2)) < pickup_distance)
        {
            Physics.IgnoreCollision(arrow_collider, GameObject.Find("Pata1").GetComponent<Collider>(), false);
            Physics.IgnoreCollision(arrow_collider, GameObject.Find("Pata2").GetComponent<Collider>(), false);
            Physics.IgnoreCollision(arrow_collider, GameObject.Find("Pata3").GetComponent<Collider>(), false);
            stuck = false;
            GameObject.Find("CameraParent").GetComponent<Move_player>().arrow = this.gameObject;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    void ArrowTrajectory()
    {
        if (launched && !stuck && arrow_rb.velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(arrow_rb.velocity);
    }

    void OnCollisionEnter(Collision collide)
    {
        transform.eulerAngles = rotation;
        arrow_rb.isKinematic = true;
        arrow_rb.useGravity = false;
        launched = false;
        stuck = true;
        if (collide.gameObject.tag == "Target")
        {
            ComputeScore(collide);
        }
        else if (collide.gameObject.name != "CameraParent")
        {
            audios.PlayOneShot(arrow_coll, 1.0f);
            audios.PlayOneShot(Fallo, 1.0f);
        }
    }

    void ComputeScore(Collision collider)
    {
        //float distance_to_target_centre = Mathf.Sqrt(Mathf.Pow(collider.transform.position.x - collider.contacts[0].point.x, 2) + Mathf.Pow(collider.transform.position.y - collider.contacts[0].point.y, 2));
        //float target_radius = collider.gameObject.GetComponent<Renderer>().bounds.size.y / 2.0f;

        target = collider.gameObject;

        if (collider.gameObject.name == "Target_mobile")
        {
            parent = target.transform;
            posRelToParent = transform.position - parent.position;
        }
        collisionPoint = GameObject.Find("/Arrow/arrowhead").transform.position + new Vector3(0.0f, 0.0f, 0.030f);
        Vector3 collision_point_local = target.transform.InverseTransformPoint(collisionPoint);
        //Vector3 target_center_local = target.transform.InverseTransformPoint(target.transform.position);
        float distance_to_target_centre = Mathf.Sqrt(Mathf.Pow(collision_point_local.x, 2) + Mathf.Pow(collision_point_local.z, 2));
        float target_radius = target_radius = collider.transform.localScale.x / 2.0f;
        print("Distance to center:" + distance_to_target_centre);

        if (distance_to_target_centre < target_radius)
        {
            confeti.SetActive(true);
            audios.PlayOneShot(arrow_coll, 1.0f);
            audios.PlayOneShot(Acierto, 1.0f);
            audios.PlayOneShot(ninos_bien, 0.25f);
            score = (int)Mathf.Ceil(10.0f * (1.0f - distance_to_target_centre / target_radius));
            TotalScore += score;
            total_score.text = "Total score: " + TotalScore;
            curr_score.text = "+" + score + " pts";
            canvas_currScore.SetActive(true);
            timer = 0;
        }

    }
}