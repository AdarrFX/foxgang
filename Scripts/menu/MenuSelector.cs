using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuSelector : MonoBehaviour
{
    private float spinSpeed = 700.0f;
    private int menuSelected;
    public TextMeshProUGUI[] menuSelection = new TextMeshProUGUI[3];
    public GameObject optionsMenu, mainMenu;
    public float[] menuAdjustment = new float[3];
    private bool stickPushVertical, stickPushHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        menuSelected = 1;
        menuAdjustment[0] = 110.0f;
        menuAdjustment[1] = 145.0f;
        menuAdjustment[2] = 95.0f;
        stickPushVertical = true;
        stickPushHorizontal = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkIfStickZero(1);
        checkIfStickZero(2);

        if (Input.GetKeyDown(KeyCode.UpArrow) || singleStickPush("up"))
        {
            if (menuSelected <= 1)
            {
                menuSelected = 3;
            }
            else
            {
                menuSelected -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || singleStickPush("down"))
        {
            if (menuSelected >= 3)
            {
                menuSelected = 1;
            }
            else
            {
                menuSelected += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || (Input.GetButtonDown("Submit")))
        {
            handleEnter();
        }
        handleMenuSelection();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + spinSpeed * Time.deltaTime);
    }

    void handleMenuSelection()
    {
        switch (menuSelected)
        {
            case 1:
                GetComponent<RectTransform>().localPosition = new Vector3(menuSelection[0].rectTransform.localPosition.x - menuAdjustment[0], menuSelection[0].rectTransform.localPosition.y + 15.0f, transform.position.z);
                break;
            case 2:
                GetComponent<RectTransform>().localPosition = new Vector3(menuSelection[1].rectTransform.localPosition.x - menuAdjustment[1], menuSelection[1].rectTransform.localPosition.y + 15.0f, transform.position.z);
                break;
            case 3:
                GetComponent<RectTransform>().localPosition = new Vector3(menuSelection[2].rectTransform.localPosition.x - menuAdjustment[2], menuSelection[2].rectTransform.localPosition.y + 15.0f, transform.position.z);
                break;
        }
    }

    bool singleStickPush(string pushDirection)
    {

        switch (pushDirection)
        {
            case "up":
                if (Input.GetAxisRaw("Vertical") >= 0.50 && stickPushVertical)
                {
                    stickPushVertical = false;
                    return true;
                }
                else return false;
            case "down":
                if (Input.GetAxisRaw("Vertical") <= -0.50 && stickPushVertical)
                {
                    stickPushVertical = false;
                    return true;
                }
                else return false;
            case "left":
                if (Input.GetAxisRaw("Horizontal") >= 0.50 && stickPushHorizontal)
                {
                    stickPushHorizontal = false;
                    return true;
                }
                else return false;
            case "right":
                if (Input.GetAxisRaw("Horizontal") <= -0.50 && stickPushHorizontal)
                {
                    stickPushHorizontal = false;
                    return true;
                }
                else return false;
            default:
                return false;
        }
    }

    void checkIfStickZero(int direction)
    {
        switch (direction)
        {
            case 1:
                if (Input.GetAxis("Vertical") >= -0.001f && Input.GetAxis("Vertical") <= 0.001f)
                {
                    stickPushVertical = true;
                }
                break;
            case 2:
                if (Input.GetAxis("Horizontal") >= -0.001f && Input.GetAxis("Horizontal") <= 0.001f)
                {
                    stickPushHorizontal = true;
                }
                break;
        }
    }

    void handleEnter()
    {
        switch (menuSelected)
        {
            case 1:
                SceneManager.LoadScene("My Game");
                break;
            case 2:
                optionsMenu.SetActive(true);
                mainMenu.SetActive(false);
                break;
            case 3:
                //UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
                break;
        }
    }
}
