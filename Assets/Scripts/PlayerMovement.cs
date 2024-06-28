using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{   
    public PlayerMovement movement;
    // How much throttle 
    public float throttleIncrement = 0.1f;
    public float throttleDIncrement = 0.04f;
    // Maximum thrust
    public float maxThrust = 200f;
    // How responsive is the plane
    public float responsiveness = 75f;
    public float responsivenessRoll = 35f;
    // How much lift force plane generates as it gains speed
    public float lift = 135f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    Rigidbody rb;
    public static AudioSource engine;
    [SerializeField] TextMeshProUGUI hud;
    [SerializeField] Transform propella;
    public AudioSource source;
    public AudioClip clip;
    
    public float responseModifier {  // Value used to tweak responsiveness to plane's mass
        get{
        return (rb.mass/10f) * responsiveness;
        }
    }

    public float responseModifierRoll {  // Value used to tweak responsiveness roll to plane's mass
        get{
        return (rb.mass/10f) * responsivenessRoll;
        }
    }

    private void Awake() {

        rb =GetComponent<Rigidbody>();
        engine = GetComponent<AudioSource>();

    }

    private void HandleIpnuts() {

        // Rotation values from our axis input
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Yaw");

        // Handle throttle value between 0 and 100
        if(Input.GetKey(KeyCode.Space)){

             throttle += throttleIncrement * (Time.deltaTime * 200);

        }
        else if(Input.GetKey(KeyCode.LeftControl)) {

            throttle -= throttleDIncrement * (Time.deltaTime * 200);

        }

        float terrainHeight = Terrain.activeTerrain.SampleHeight( transform.position );

        // Helps plane with the landing
        if((terrainHeight + 5) > transform.position.y ){

            throttle = Mathf.Clamp(throttle, 0f, 100f);
            throttleDIncrement = 0.02f;

        }
        else{

            throttle = Mathf.Clamp(throttle, 45f, 100f);
            throttleDIncrement = 0.04f;

        }
        

    }

    // Update is called once per frame
    void Update()
    {

        HandleIpnuts();
        UpdateHUD();

        if(!MenuPause.isPaused){
            propella.Rotate(Vector3.right * throttle);
            engine.volume = throttle * 0.01f;
        }

    }

    private void FixedUpdate() {

        // Apply forces to our plane 
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifierRoll);
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
        
        // If the Plane is flying upwards -- speed, if the plane is flying downwards ++ speed
        if(transform.rotation.eulerAngles.x < 340 && transform.rotation.eulerAngles.x > 100 ){
            if(rb.mass > 600){
                rb.mass = 600;
            }
            rb.mass = rb.mass + 1;
        }else if(transform.rotation.eulerAngles.x > 10 && transform.rotation.eulerAngles.x < 200 ){
            if(rb.mass < 250){
                rb.mass = 250;
            }
            rb.mass = rb.mass - 1;
        }else {
            rb.mass = 400;
        }

        if(rb.velocity.magnitude * 3.6f > 200){
            rb.mass += 2;
        }

        //CameraFollow();

    }

    private void UpdateHUD() {

        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Speed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }
    

    /*void CameraFollow() {

        Vector3 updateCam = transform.position - transform.forward * 12.0f + Vector3.up * 5.0f;
        float bias = 0.94f;
        Camera.main.transform.position = Camera.main.transform.position * bias + updateCam * (1.0f-bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);

    }*/
    
     private void OnTriggerEnter(Collider other)
    {
        source.PlayOneShot(clip);
        throttle = 0f;
        engine.volume = 0f;
        movement.enabled = false;

    }
}
