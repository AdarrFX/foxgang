using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private float groundLength, groundScrollPosition;
    private GameHandler gameHandler;
    Vector3 originalGroundPosition;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        groundLength = GetComponent<BoxCollider>().size.z * 5 / 3;
        transform.position = new Vector3(transform.position.x, transform.position.y, groundLength);
        originalGroundPosition = transform.position;
        groundScrollPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameIsRunning)
        {
            groundScrollPosition -= 6.0f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, groundScrollPosition);

            if (groundScrollPosition <= -originalGroundPosition.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, originalGroundPosition.z);
                groundScrollPosition = transform.position.z;
            }
        }
    }
}
