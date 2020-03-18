using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class ThrowableItem : MonoBehaviour {

    [SerializeField] private int noiseLevel;
    [SerializeField] private GameObject soundSource;
    [SerializeField] private float soundDuration;
    [SerializeField] private GameObject infinitePool;
    [Space]
    [SerializeField] GameObject meshArcReference;
    [SerializeField] GameObject meshPivot;
    [SerializeField] private float meshWidth;
    [SerializeField] private int renderingResolution;

    private Vector3 direction;
    private float height;
    private float velocity;

    private float distance;

    private SoundManager sound_Manager;

    private Rigidbody rb;

    private bool wasInHand = false;

    //Linerendering variables
    private Mesh arcMesh;
    private float gravity;
    private float radianAngle;
    private float maxDistance;
    private float v = 10f;

    //SoundVariables
    [Space]
    [SerializeField] AudioClip[] possibleSounds;
    private AudioSource m_audioSource;

    private void Start()
    {
        sound_Manager = GameObject.FindWithTag("GameController").GetComponent<SoundManager>();
        rb = this.GetComponent<Rigidbody>();

        m_audioSource = this.GetComponent<AudioSource>();

        gravity = Mathf.Abs(Physics.gravity.y);

        arcMesh = meshArcReference.GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        //Maybe make the object rotate around its pivot to make it look better
        //or don't
        //don't f* know
    }

    public void ThrowItem (Vector3 _target)
    {
        //look at the target position before thowing the item
        this.transform.LookAt(_target);
        //throw the item at the target
        rb.velocity = BallisticVel(_target);

        wasInHand = true;
    }

    //method for getting the needed force to hit the target
    Vector3 BallisticVel(Vector3 _target)
    {
        direction = _target - transform.position; // get target direction
        height = direction.y;  // get height difference
        direction.y = 0;  // retain only the horizontal direction
        distance = direction.magnitude;  // get horizontal distance
        direction.y = distance;  // set elevation to 45 degrees
        distance += height;  // correct for different heights
        velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude);

        return velocity * direction.normalized;  // returns Vector3 velocity
    }

    //Methods that turn on/off the RigidBody
    public void TurnRbOn()
    {
        rb.isKinematic = false;
        StartCoroutine(MakeColliderApear());
    }

    public void TurnRbOff()
    {
        rb.isKinematic = true;
        this.GetComponent<CapsuleCollider>().enabled = false;
    }

    //make noise when colliding with something
    private void OnCollisionEnter(Collision _col)
    {
        if (wasInHand)
        {
            StartCoroutine(sound_Manager.MakeNoise(soundSource, noiseLevel, soundDuration));
            rb.isKinematic = true;
            this.GetComponent<MeshRenderer>().enabled = false;

            m_audioSource.clip = possibleSounds[(int)Random.Range(0, possibleSounds.Length)];
            m_audioSource.Play();

            if(infinitePool == null) //if there is no pool from wich the player can get, it will destroy the obj
            {
                Destroy(this, soundDuration);
            }
            else // if there is a pool the player can get from, it will make the obj re apear after a time
            {
                StartCoroutine(WaitToReApear());
            }
        }
        
    }

    //Reapear the object in the obj pool
    IEnumerator WaitToReApear()
    {
        MeshRenderer mRenderer;
        mRenderer = this.GetComponent<MeshRenderer>();
        mRenderer.enabled = false; //turn off the mesh renderer

        yield return new WaitForSecondsRealtime(1.5f);

        this.transform.position = infinitePool.transform.position;
        mRenderer.enabled = true; //Turn on the mesh Renderer
    }

    //Make the collider come back
    IEnumerator MakeColliderApear()
    {
        yield return new WaitForSeconds(0.8f);

        this.GetComponent<CapsuleCollider>().enabled = true;
        StartCoroutine(CheckIfTookTooLong());
    }

    IEnumerator CheckIfTookTooLong()
    {
        yield return new WaitForSeconds(5);

        StartCoroutine(WaitToReApear());
    }

    #region Line rendereing functions

    public void DoTheArc(Vector3 _target)
    {
        MakeArcMesh(CalculateArcArray(_target));
    }

    private void MakeArcMesh(Vector3[] arcVerts)
    {
        arcMesh.Clear();
        Vector3[] vertices = new Vector3[(renderingResolution + 1) * 2];
        int[] triangles = new int [renderingResolution * 12];

        for (int i = 0; i <= renderingResolution; i++)
        {
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x);
            vertices[i * 2 +1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);

            //set triangles
            if(i != renderingResolution)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }

            arcMesh.vertices = vertices;
            arcMesh.triangles = triangles;
        }
    }

    private Vector3[] CalculateArcArray(Vector3 _target)
    {
        Vector3[] arcArray = new Vector3[renderingResolution + 1];

        radianAngle = Mathf.Deg2Rad * 45;
        maxDistance = Vector3.Distance(this.transform.position, _target);

        for (int i = 0; i <= renderingResolution; i++)
        {
            float t = (float)i / (float)renderingResolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    private Vector3 CalculateArcPoint(float _t, float _maxDistance)
    {
        float x = _t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * v * v * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(x, y);
    }

    #endregion

}
