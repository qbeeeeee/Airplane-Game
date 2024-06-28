using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    
    public GameObject explosiveEffect;
    public Transform brokenPlane;
    public Transform planePivot;
    public GameObject normalPlane;
    public GameObject deadMenu;
    
    public float radius = 10f;
    public float force = 1500f;
    public static bool exploded = false;

    public float slowMotionTimeScale;

    private float startTimeScale;
    private float startFixedDeltaTime;

    public Rigidbody FreezeRb;

    void Start()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        exploded = false;
        //Ingame Time
        startTimeScale = Time.timeScale;
        //Ingame physics Time
        startFixedDeltaTime = Time.fixedDeltaTime;

    }

    private void OnTriggerEnter(Collider other)
    { 
        if(!exploded)
        {
            explode();
            SlowMotion();
            //Freeze position of our player 
            FreezeRb.constraints = RigidbodyConstraints.FreezePosition;
            RenderSettings.ambientIntensity = 0;
        }
    }

    void explode()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (Vector3.Distance(obj.transform.position, transform.position) <= radius)
            {
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                    Instantiate(explosiveEffect, obj.transform.position, obj.transform.rotation);
                }
            }
        }
        //Creates the brokenPlane model into the position and rotation we are
        Instantiate(brokenPlane, planePivot.transform.position, planePivot.transform.rotation);
        //Creates the explion effect into the position and rotation we are
        Instantiate(explosiveEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null) 
            {

                rb.AddExplosionForce(force, transform.position, radius);

            }

        }
        
        exploded = true;
        
        Destroy(normalPlane);

    }

    void SlowMotion()
    {
            //Slows time and FixedDelta time
            Time.timeScale = slowMotionTimeScale;
            Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;

    }

}
