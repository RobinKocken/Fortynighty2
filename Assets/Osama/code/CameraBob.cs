using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public float walkingBobbingSpeed = 80f;
    public float bobbingAmount = 0.001f;
    public PlayerController controller;

    float defaultPosY = 0;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("player").GetComponent<PlayerController>();
        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            if (Mathf.Abs(controller.moveDirection.x) > 0.1f || Mathf.Abs(controller.moveDirection.z) > 0.1f)
            {
                //Player is moving
                timer += Time.deltaTime * walkingBobbingSpeed;
                transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
            }
            else
            {
                //Idle
                timer = 0;
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);
            }
        }
    }
}
