using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    [SerializeField] Transform[] povs;

    private int index = 2;
    private Vector3 cam;
    private bool transition = false;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        else if(Input.GetKeyDown(KeyCode.Alpha2)) index = 1;
        else if(Input.GetKeyDown(KeyCode.Alpha3)) index = 2;

        cam = povs[index].position;

    }

    public Transform player;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(index == 2){

            if(!transition)
            {

                transform.LookAt(player);
                transform.position = cam;

            }
            transition = true;

            Vector3 updateCam = player.transform.position - player.transform.forward * 12.0f + Vector3.up * 5.0f;
            float bias = 0.94f;
            transform.position = Camera.main.transform.position * bias + updateCam * (1.0f-bias);
            transform.LookAt(player.transform.position + player.transform.forward * 30.0f);

        }else if(index == 0) {

            transition = false;

            transform.position = player.transform.position - player.transform.forward * 5.0f + player.transform.up * 5.0f;
            transform.rotation = player.rotation;

        }
        else
        {   
            transition = false;
           
            transform.LookAt(player);
            //transform.position = Vector3.MoveTowards(transform.position, cam, Time.deltaTime * 100.0f);
            transform.position = cam;

        }

    }
}
