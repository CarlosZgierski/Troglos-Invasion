//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;


[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(AudioSource))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private Control_Anim m_ControlAnim;

    //Player RigidBody
    private Rigidbody rb;

    //Body collider & triggers reference
    [SerializeField] private GameObject feetCollider;
    private Feet feetScript;

    //Feet bool's of trigger detection
    private bool feetOnCover = false;
    private bool feetOnVault = false;
    private bool feetOnHide = false;
    private bool feetOnPush = false;

    //Variables that limit or allow player to do certain movements
    private bool onCover = false;
    private bool onPush = false;
    private float currentCoverRotationY = 0;
    private Quaternion currentPushRotationY;
    private Vector3 currentTargetVault;
    private GameObject currentPushedObject;
    private GameObject currentPushedTrigger;
    private bool playerCanMove = true;
    bool crouch;
    private bool lockMovement = false;
    private bool allowToHide = true;
    [Range(1,2)][SerializeField] private float speedSprintMult = 1.3f;

    //reference to the sound manager
    private SoundManager m_soundManager;

    //reference to the sound source
    [SerializeField] private GameObject soundSource;

    //Variables that allow the player to throw stuff
    private GameObject currentThowableObject;
    private ThrowableItem thowScript;

    //Variables from the throw mechanic
    [SerializeField] private GameObject handReference; //hand reference to put the picked object
    [SerializeField] private GameObject playerInteractionZone; //Interaction Zone from the player, it is a children of the player
    private GameObject pickedObject;
    private ThrowableItem pickedObjectScript;
    private FinalInteractiveObject pickedFinalScript;
    private InteractionZone interactionScript;
    private bool canInteract = false;
    private RaycastHit targetThrow; //variable to store the target hit 
    private bool isAiming = false;

    [Space]
    [SerializeField] private float maxThrowRange = 10f;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject playerBackHead;
    public bool itemInHand = false;
    private Camera m_Camera;

    [Space]
    [SerializeField] private GameObject m_throwArc;
    [SerializeField] private LayerMask throwHitMask;
    [SerializeField] private GameObject m_Crosshair;
    //********************************************************************************//

    [Space]
    //Varibles for the final objective
    [SerializeField] private GameObject backCarrySpot;
    private bool itemPicked;
    private bool finalPicked;
    private GameObject pickedObjRef;

    [Space]
    //Variables for the over the shoulder camera
    [SerializeField] private GameObject oS_CameraGo;
    [SerializeField] private GameObject oS_DestPosition;
    private Camera oS_Camera;
    

    [Space]
    //Sound related variables
    [SerializeField]private AudioSource m_AudioSource;
    private int currentFootStep;
    [SerializeField] private AudioClip[] m_FootstepSoundsCarpet;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] m_FootstepSoundsWood;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] m_FootstepSoundsConcrete;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] m_FootstepSoundsGlass;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private float walkSoundSpeed = 0.5f;
    [SerializeField] private float crouchSoundSpeed = 0.3f;
    private bool alreadyGoing;
    private Coroutine stepCoroutine;
    private bool holdingMove;

    [Space]
    //Camera Related variables
    private float cameraBackSafeSpace = 1f;
    [SerializeField] private GameObject hideCameraRef;
    private bool hidden = false;

    //animation
    private Control_Anim anim;

    //audio
    ScriptSound audioPlay;
    bool playsom;

    // texto para objetos
    public Text tObjetos;

    //audios e texto de sacos
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private string[] legendas;
    [SerializeField]
    private AudioSource sacoAudio;

    private void Start()
    {
        sacoAudio.volume *= Globals.SOUND_GENERAL_SLIDER;

        this.transform.rotation.SetLookRotation(new Vector3(0, 0, 1));

        //gets de script sound
        audioPlay = GetComponent<ScriptSound>();

        m_throwArc.SetActive(false);
        m_Crosshair.SetActive(false);

        m_Camera = Camera.main;
        oS_Camera = oS_CameraGo.GetComponent<Camera>();
        oS_CameraGo.SetActive(false);

        //animation
        anim = GetComponent<Control_Anim>();

        //Gets the Feet Script from the feet child
        feetScript = feetCollider.GetComponent<Feet>();

        //Making sure the secondary camera is off
        hideCameraRef.SetActive(false);

        //player rigidBody
        rb = this. GetComponent<Rigidbody>();

        if(playerCamera == null)
        {
            playerCamera = Camera.main.gameObject;
        }

        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();

        m_soundManager = GameObject.FindWithTag("GameController").GetComponent<SoundManager>();

        interactionScript = playerInteractionZone.GetComponent<InteractionZone>();

        if(m_AudioSource == null)
        {
            m_AudioSource = this.GetComponent<AudioSource>();
            
        }
        m_AudioSource.volume *= Globals.SOUND_FX_SLIDER;
    }

    private void Update()
    {
        currentFootStep = feetScript.CurrentTypeOfGround();

        if(!GameManager.IS_WORLD_PAUSED)
        {
            PlayerSpecialInteractions();
            PlayerThrow();
        }
    }

    private void FixedUpdate()
    {         
        PlayerFeetColision();

        if(!lockMovement)
            PlayerMovement();
    }

    #region Player Movement

    void PlayerMovement()
    {
        if (playerCanMove)
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool running = false;

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                crouch = !crouch;
            }

            if (h != 0 || v != 0)
            {
                holdingMove = true;
            }
            else
            {
                holdingMove = false;
            }

            if(onPush)
            {
                if(!playsom)
                {
                    audioPlay.PlayMesa(true);
                    playsom = true;
                }

                if (h != 0)
                {
                    audioPlay.VolumeMesa(1);
                }
                else
                {
                    audioPlay.VolumeMesa(0);
                }

            }
            else
            {
                audioPlay.PlayMesa(false);
                playsom = false;
            }

            if (!onCover && !onPush) // Normal movement if the player is not on cover in any way, allowing normal movement
            {
                // calculate move direction to pass to character
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;

                // walk speed multiplier
                if (isAiming) m_Move *= 0.4f;

                else if (Input.GetKey(KeyCode.LeftShift) && !finalPicked)
                {
                    running = true;
                }

                // pass all parameters to the character control script
                m_Character.Move(m_Move, crouch, false, running, speedSprintMult); //Last False statement is for the jumping
            }
            else if (!onPush && onCover) // Cover Movement, allowing the player to onli move side to side and backwards to leave the cover
            {
                this.transform.rotation = new Quaternion(this.transform.rotation.x, currentCoverRotationY, this.transform.rotation.z, this.transform.rotation.w);

                m_Character.MoveOnCover(h);
            }
            else if (onPush && !onCover)
            {
                m_Character.MoveOnPush(h, v);
            }

            //Finding out if the player is moving so it makes noise
            if (h != 0 || v != 0)
            {
                if (!crouch)
                {
                    if (!running) soundSource.GetComponent<SphereCollider>().radius = 1.5f;
                    else soundSource.GetComponent<SphereCollider>().radius = 3f;

                    if (!alreadyGoing)
                    {
                        stepCoroutine = StartCoroutine(MakeFootStepSound(walkSoundSpeed));
                        alreadyGoing = true;
                    }
                }
                else
                {
                    soundSource.GetComponent<SphereCollider>().radius = 0.75f;
                    if (!alreadyGoing)
                    {
                        stepCoroutine = StartCoroutine(MakeFootStepSound(crouchSoundSpeed));
                        alreadyGoing = true;
                    }
                }
            }
            else
            {
                soundSource.GetComponent<SphereCollider>().radius = 0.3f;

                if(stepCoroutine !=null)
                    StopCoroutine(stepCoroutine);

                alreadyGoing = false;
            }
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false, false, speedSprintMult);
        }
    }

    void PlayerFeetColision()
    {
        feetOnCover = feetScript.FeetCoverBool();
        feetOnVault = feetScript.FeetVaultBool();
        feetOnHide = feetScript.FeetHideBool();

        if(!onPush)
        feetOnPush = feetScript.FeetPushBool();
    }

    #endregion

    #region Player interaction

    void PlayerSpecialInteractions()
    {
        currentCoverRotationY = feetScript.CoverRotation();
        currentPushRotationY = feetScript.PushRotation();
        currentPushedObject = feetScript.PushedObject();
        currentPushedTrigger = feetScript.PushedObjTrigger();
        canInteract = interactionScript.InteractionCollisionBool();

        if (feetScript.VaultTarget() != null)
            currentTargetVault = feetScript.VaultTarget();

        if (feetOnCover) //Gets In and Out of cover
        {
            if (Input.GetKeyDown(KeyCode.E) && !onPush)
            {
                if (!Globals.SACO_NA_MAO)
                {
                    if (onCover == false)
                        onCover = true;
                    else
                        onCover = false;
                }
            }
        }
        else
        {
            onCover = false;
        }

        if (feetOnHide)
        {
            if (Input.GetKeyDown(KeyCode.E) && allowToHide)
            {
                if (!Globals.SACO_NA_MAO)
                {
                    if (hidden)
                    {
                        //anim.UnHide();
                        allowToHide = false;
                        StartCoroutine("WaitForUnHide");
                        playerCanMove = true;
                    }
                    else
                    {
                        this.transform.rotation = currentPushRotationY;

                        anim.Hide();
                        allowToHide = false;
                        StartCoroutine("WaitForHide");
                        playerCanMove = false;
                        StopCoroutine(stepCoroutine);
                    }
                    StartCoroutine(WaitForHideAnimation());
                }
            }
        }

        if (feetOnVault) //look if the feets are on the vault trigget, so the player can vault
        {
            if (Input.GetKeyDown(KeyCode.Space) && !onPush)
            {
                if (!Globals.SACO_NA_MAO)
                {
                    audioPlay.PlayJump();
                    this.transform.position = currentTargetVault;
                }
            }
        }

        if (feetOnPush) //make the player push a box or object
        {
            if (!Globals.SACO_NA_MAO)
            {
                if (Input.GetKeyDown(KeyCode.E) && !onPush)
                {
                    anim.Push(true);
                    onPush = true;

                    this.transform.rotation = currentPushRotationY;
                    this.transform.position = currentPushedTrigger.transform.position;

                    currentPushedObject.transform.parent = this.gameObject.transform;

                    currentPushedObject.GetComponent<PushInteraction>().WallsInteraction(true); // Make the invisble walls appear
                }
                else if (Input.GetKeyDown(KeyCode.E) && onPush)
                {
                    anim.Push(false);
                    onPush = false;
                    currentPushedObject.transform.parent = null;

                    currentPushedObject.GetComponent<PushInteraction>().WallsInteraction(false); //Make the invisible walls disappear
                }
            }
            else
            {
                onPush = false;
                if (currentPushedObject != null)
                    currentPushedObject.transform.parent = null;
            }
        }

        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E)) //Interaction With Objects
            {

                pickedObject = interactionScript.InteractionCollisionGameObject();
                
                if (pickedObject.CompareTag(Globals.TAG_INTERACTABLE_OBJECT) && !itemInHand)
                {
                    if (!Globals.SACO_NA_MAO)
                    {
                        //m_ControlAnim.PickUp();
                        StartCoroutine("WaitForPickAnim");

                        anim.PickUp();

                        itemInHand = true;
                    }
                }
                else if(pickedObject.CompareTag(Globals.TAG_MAIN_OBJECT) && !itemInHand)
                {
                    Globals.SACO_NA_MAO = true;
                    anim.Olhar();
                    playerCanMove = false;
                    StartCoroutine(WaitForPickAnimation());
                    finalPicked = true;
                    pickedObjRef = pickedObject.gameObject;
                }
                else if (pickedObject.CompareTag(Globals.TAG_SACO_FALSO))
                {
                    if (!Globals.SACO_NA_MAO)
                    {
                        anim.Olhar();
                        playerCanMove = false;
                        StartCoroutine(WaitForPickAnimationFalse());
                        pickedObject.GetComponent<CapsuleCollider>().enabled = false;
                        pickedObject.GetComponent<GlowItens>().enabled = false;
                        interactionScript.DeleteReferences();
                    }
                }
                else if(itemInHand && pickedObjRef != null)
                {
                        Globals.SACO_NA_MAO = false;

                        pickedObjRef.transform.parent = null;
                        pickedObjRef.GetComponent<FinalInteractiveObject>().TurnRbOn();
                        itemInHand = false;
                        pickedObjRef.GetComponent<Rigidbody>().useGravity = true;
                        pickedObjRef = null;
                }
                else print("Out");
            }
        }

    }

    void PlayerThrow()
    {
        if(Input.GetMouseButton(1) && itemInHand)
        {
            isAiming = true;
            Ray throwCast = new Ray(oS_CameraGo.transform.position, oS_CameraGo.transform.forward);
            Physics.Raycast(throwCast, out targetThrow, maxThrowRange, throwHitMask);

            m_throwArc.SetActive(true);
            m_Crosshair.SetActive(true);

            m_Camera.enabled = false;
            playerCamera.GetComponent<CameraScript>().enabled = false;

            oS_CameraGo.transform.parent = oS_DestPosition.transform;

            //playerCanMove = false;

            oS_CameraGo.transform.position = Vector3.Lerp(oS_CameraGo.transform.position, oS_DestPosition.transform.position, 1*Time.deltaTime);
            oS_CameraGo.SetActive(true);

            pickedObjectScript.DoTheArc(targetThrow.point);

            if (Input.GetMouseButtonDown(0)) //Throw the item in the hand of the player
            {
                //this.transform.LookAt(targetThrow.point);
                anim.Throw();

                StartCoroutine(WaitForThrowAnimation(1));
            }
        }
        else
        {
            m_throwArc.SetActive(false);
            m_Crosshair.SetActive(false);
            playerCamera.GetComponent<CameraScript>().enabled = true;

            m_Camera.enabled = true;
            oS_CameraGo.SetActive(false);
            oS_CameraGo.transform.parent = playerCamera.transform;
            //playerCanMove = true;
            isAiming = false;
        }
    }

    #endregion

    #region Player Coroutines

    private IEnumerator WaitForHide()
    {
        feetScript.HidePorta().GetComponent<HideTrigger>().Abrir2();
        yield return new WaitForSecondsRealtime(1.5f);
        hideCameraRef.transform.position = feetScript.HideCameraPosition();
        hideCameraRef.transform.parent = feetScript.HideCameraParent().transform;
        hideCameraRef.transform.rotation = new Quaternion(0, 0, 0, 0);

        this.transform.position = feetScript.HideTargetReturn();

        playerCamera.SetActive(false);
        hideCameraRef.SetActive(true);
        allowToHide = true;

        hidden = true;
        lockMovement = true;
        audioPlay.PlayHide();
    }

    private IEnumerator WaitForUnHide()
    {
        feetScript.HidePorta().GetComponent<HideTrigger>().Abrir2();
        yield return new WaitForSecondsRealtime(2);
        hideCameraRef.transform.parent = this.transform;
        this.transform.position = feetScript.HideTargetReturn();
        allowToHide = true;

        playerCamera.SetActive(true);
        hideCameraRef.SetActive(false);

        hidden = false;
        lockMovement = false;
        audioPlay.PlayHide();
    }

    private IEnumerator WaitForPickAnimationFalse()
    {
        int n = Random.Range(0, clips.Length);
        yield return new WaitForSecondsRealtime(4);
        playerCanMove = true;
        sacoAudio.clip = clips[n];
        sacoAudio.Play();
        tObjetos.text = legendas[n];
        yield return new WaitForSecondsRealtime(clips[n].length);
        tObjetos.text = "";
    }

    private IEnumerator WaitForPickAnimation()
    {
        yield return new WaitForSecondsRealtime(3);

        pickedFinalScript = pickedObject.GetComponent<FinalInteractiveObject>();
        pickedFinalScript.TurnRbOff();
        yield return new WaitForSecondsRealtime(2);
        anim.PickMainObj();
        yield return new WaitForSecondsRealtime(1);
        pickedObject.transform.parent = backCarrySpot.transform;
        pickedObject.transform.localPosition = Vector3.zero;
        itemInHand = pickedObject.GetComponent<FinalInteractiveObject>().TurnRbOff();
        audioPlay.PlaySaco();
        playerCanMove = true;
    }

    private IEnumerator WaitForThrowAnimation(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        //m_ControlAnim.Throw();
        audioPlay.PlayJump();
        pickedObjectScript.TurnRbOn();
        pickedObject.transform.parent = null;

        //m_Character.ThrowAnimation();

        pickedObjectScript.ThrowItem(targetThrow.point);

        itemInHand = false;

        interactionScript.DeleteReferences();
    }

    private IEnumerator WaitForHideAnimation()
    {
        yield return new WaitForSeconds(2);

        allowToHide = true;
    }

    #endregion

    #region Player Sound

    private void PlayFootStepAudio()
    {
        if (currentFootStep == 0) //Carpet Sounds
        {
            // excluding sound at index 0
            int n = UnityEngine.Random.Range(1, m_FootstepSoundsCarpet.Length);
            m_AudioSource.clip = m_FootstepSoundsCarpet[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSoundsCarpet[n] = m_FootstepSoundsCarpet[0];
            m_FootstepSoundsCarpet[0] = m_AudioSource.clip;
        }
        else if (currentFootStep == 1) //Wood Sounds
        {
            if (!hidden)
            {
                // excluding sound at index 0
                int n = UnityEngine.Random.Range(1, m_FootstepSoundsWood.Length);
                m_AudioSource.clip = m_FootstepSoundsWood[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSoundsWood[n] = m_FootstepSoundsWood[0];
                m_FootstepSoundsWood[0] = m_AudioSource.clip;
            }
        }
        else if (currentFootStep == 2) //Concrete Sounds
        {
            // excluding sound at index 0
            int n = UnityEngine.Random.Range(1, m_FootstepSoundsConcrete.Length);
            m_AudioSource.clip = m_FootstepSoundsConcrete[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSoundsConcrete[n] = m_FootstepSoundsConcrete[0];
            m_FootstepSoundsConcrete[0] = m_AudioSource.clip;
        }
        else if (currentFootStep == 3) //Glass Sounds
        {
            // excluding sound at index 0
            m_AudioSource.volume *= Globals.SOUND_FX_SLIDER;
            int n = UnityEngine.Random.Range(1, m_FootstepSoundsGlass.Length);
            m_AudioSource.clip = m_FootstepSoundsGlass[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSoundsGlass[n] = m_FootstepSoundsGlass[0];
            m_FootstepSoundsGlass[0] = m_AudioSource.clip;
        }
            
    }

    private IEnumerator MakeFootStepSound(float _speed)
    {
        PlayFootStepAudio();

        yield return new WaitForSeconds(_speed);

        if(holdingMove)
        {
            StopCoroutine(MakeFootStepSound(_speed));
            StartCoroutine(MakeFootStepSound(_speed));
        }
       
    }
    private IEnumerator WaitForPickAnim()
    {
        yield return new WaitForSeconds(1.21f);
        pickedObjectScript = pickedObject.GetComponent<ThrowableItem>();
        pickedObjectScript.TurnRbOff();

        pickedObject.transform.parent = handReference.transform;
        pickedObject.transform.localPosition = Vector3.zero;
        pickedObject.transform.rotation = new Quaternion(90f, 0f, 0f, 0f);

        audioPlay.PlayObjeto();

    }
    #endregion

    #region Public Player Functions

    public void AllowPlayerMovement(bool _MovBool)
    {
        playerCanMove = _MovBool;
    }

    public bool PlayerIsCrouched()
    {
        return crouch;
    }

    public void RotateAxis(float _HowMuch)
    {
        Quaternion rotate = Quaternion.Euler(this.transform.rotation.x, _HowMuch, this.transform.rotation.z);

        this.transform.rotation = rotate;
    }

    #endregion

    #region Private Player Functions

    private void SearchMainObj(bool _obj)
    {
        if(_obj)
        {

        }
        else
        {

        }
    }

    #endregion
}

