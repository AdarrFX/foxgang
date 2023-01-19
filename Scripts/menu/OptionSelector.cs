using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour
{
    // Start is called before the first frame update
    private float spinSpeed = 700.0f;
    [SerializeField]
    private int optionMenuSelected;
    public GameObject optionsMenuContainer, mainMenuContainer;
    public TextMeshProUGUI[] optionMenuSelection = new TextMeshProUGUI[3];
    public float[] menuAdjustment = new float[3];
    public Slider musicSlider;
    private bool windowSelect;
    public GameObject graphicsCheckboxWindow, graphicsCheckboxFull;
    public Texture2D checkbox_empty_image, checkbox_full_image;
    private Sprite chk_mpt, chk_full;
    [SerializeField]
    private bool stickPushVertical, stickPushHorizontal;
    [SerializeField]
    private float horiAxis;
    void Start()
    {
        chk_mpt = Sprite.Create(checkbox_empty_image, new Rect(0, 0, checkbox_empty_image.width, checkbox_empty_image.height), new Vector2(0.5f, 0.5f));
        chk_full = Sprite.Create(checkbox_full_image, new Rect(0, 0, checkbox_full_image.width, checkbox_full_image.height), new Vector2(0.5f, 0.5f));
        graphicsCheckboxWindow.GetComponent<Image>().sprite = chk_full;
        optionMenuSelected = 1;
        windowSelect = true;
        menuAdjustment[0] = 70.0f;
        menuAdjustment[1] = 205.0f;
        menuAdjustment[2] = 120.0f;
        stickPushHorizontal = true;
        stickPushVertical = true;

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        } else
        {
            musicSlider.value = 0.25f;
        }
        if (PlayerPrefs.HasKey("fullscreen_borderless"))
        {
            if (PlayerPrefs.GetInt("fullscreen_borderless") == 1)
            {
                setFullscreen();
            } else
            {
                setWindowed();
            }
        }
        else
        {
            musicSlider.value = 0.25f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        horiAxis = Input.GetAxisRaw("Horizontal");
        checkIfStickZero(1);
        checkIfStickZero(2);

        if (Input.GetKeyDown(KeyCode.UpArrow) || singleStickPush("up"))
        {
            if (optionMenuSelected <= 1)
            {
                optionMenuSelected = 3;
            }
            else
            {
                optionMenuSelected -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || singleStickPush("down"))
        {
            if (optionMenuSelected >= 3)
            {
                optionMenuSelected = 1;
            }
            else
            {
                optionMenuSelected += 1;
            }
        }
        if (optionMenuSelected == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || singleStickPush("left") || singleStickPush("right"))
            {
                if (windowSelect)
                {
                    windowSelect = false;
                }
                else
                {
                    windowSelect = true;
                }
            }
        }
        if (/*Input.GetKeyDown(KeyCode.Return) || */Input.GetButtonDown("Submit") && optionMenuSelected == 1)
        {
            if (windowSelect)
            {
                // Set Windowed mode @ 720p
                setWindowed();
                PlayerPrefs.SetInt("fullscreen_borderless", 0);
            } else
            {
                // Set borderless fullscreen window
                setFullscreen();
                PlayerPrefs.SetInt("fullscreen_borderless", 1);
            }
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || singleStickPush("left")) && optionMenuSelected == 2)
        {
            musicSlider.value -= 0.05f;
            PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || singleStickPush("right")) && optionMenuSelected == 2)
        {
            musicSlider.value += 0.05f;
            PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        }
        if (/*(Input.GetKeyDown(KeyCode.Return) || */Input.GetButtonDown("Submit") && optionMenuSelected == 3)
        {
            optionsMenuContainer.SetActive(false);
            mainMenuContainer.SetActive(true);
            PlayerPrefs.Save();
        }
        handleMenuSelection();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + spinSpeed * Time.deltaTime);
    }

    bool singleStickPush(string pushDirection)
    {

        switch (pushDirection)
        {
            case "up":
                if (Input.GetAxisRaw("Vertical") >= 0.45f && stickPushVertical)
                {
                    stickPushVertical = false;
                    return true;
                }
                else return false;
            case "down":
                if (Input.GetAxisRaw("Vertical") <= -0.45f && stickPushVertical)
                {
                    stickPushVertical = false;
                    return true;
                }
                else return false;
            case "left":
                if (Input.GetAxisRaw("Horizontal") <= -0.50f && stickPushHorizontal)
                {
                    stickPushHorizontal = false;
                    return true;
                }
                else return false;
            case "right":
                if (Input.GetAxisRaw("Horizontal") >= 0.50f && stickPushHorizontal)
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
                if (Input.GetAxis("Vertical") >= -0.01f && Input.GetAxis("Vertical") <= 0.01f)
                {
                    stickPushVertical = true;
                }
                break;
            case 2:
                if (Input.GetAxis("Horizontal") >= -0.01f && Input.GetAxis("Horizontal") <= 0.01f)
                {
                    stickPushHorizontal = true;
                }
                break;
        }
    }

    void handleMenuSelection()
    {
        switch (optionMenuSelected)
        {
            case 1:
                if (windowSelect)
                {
                    GetComponent<RectTransform>().localPosition = new Vector3(optionMenuSelection[0].rectTransform.localPosition.x - menuAdjustment[0], optionMenuSelection[0].rectTransform.localPosition.y - 40.0f, transform.position.z);
                } else
                {
                    GetComponent<RectTransform>().localPosition = new Vector3(optionMenuSelection[0].rectTransform.localPosition.x + 180.0f, optionMenuSelection[0].rectTransform.localPosition.y - 40.0f, transform.position.z);
                }
                break;
            case 2:
                GetComponent<RectTransform>().localPosition = new Vector3(optionMenuSelection[1].rectTransform.localPosition.x - menuAdjustment[1], optionMenuSelection[1].rectTransform.localPosition.y + 15.0f, transform.position.z);
                break;
            case 3:
                GetComponent<RectTransform>().localPosition = new Vector3(optionMenuSelection[2].rectTransform.localPosition.x - menuAdjustment[2], optionMenuSelection[2].rectTransform.localPosition.y + 15.0f, transform.position.z);
                break;
        }
    }

    void setFullscreen()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        graphicsCheckboxFull.GetComponent<Image>().sprite = chk_full;
        graphicsCheckboxWindow.GetComponent<Image>().sprite = chk_mpt;
    }
    void setWindowed()
    {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        graphicsCheckboxFull.GetComponent<Image>().sprite = chk_mpt;
        graphicsCheckboxWindow.GetComponent<Image>().sprite = chk_full;
    }
}
