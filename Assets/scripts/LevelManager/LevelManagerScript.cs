using Assets.scripts.DialogModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
	
public class LevelManagerScript : MonoBehaviour
{
    public GameObject RoadManager;
    public float roadTileCreationSpeed = 1;     // tile per second
    public int directionChangingFrequency = 3;  // Random 0 =  change direction

    public float droneRedrawSpeed = 60f;
    public GameObject Drone;

    public PlayerControllerScript playerScript;

    public int Level = 1;//WorldManager.Instance.Level;
    public int timeToSurvive = 4;                   //in seconds

    public GameObject MainModalPanel;               // ..
    public GameObject SuccessModalPanel;            // Modal Dialog when user successfully completed level
    public Button SuccessModalOkButton;             // OK button in Success modal dialog
    public Button SuccessModalNextLvlButton;        // Next level button in Success modal dialog

    public GameObject FailModalPanel;               // Modal Dialog when user Fail to complete level
    public Button FailModalOkButton;                // OK button in Fail modal dialog
    public Button FailModalRepeatLvlButton;         // repeat level button in Fail modal dialog

    public GameObject AreYouSureModalPanel;         
    public Button AreYouSureOkButton;          
    public Button AreYouSureCancelButton;   

    public Button MenuButton;   

    public Text LevelTimeText;

    private DialogManager dialogManager;

    private bool GameOver = false;
    private RoadGenerationScript roadScript;
    private int timeLeft;

    public void Start()
    {
        Debug.Log("Start");
        InitAllPanelsAndDialogs();

        initTimer();

        roadScript = RoadManager.GetComponent<RoadGenerationScript>();

        if (roadScript == null)
        {
            Debug.Log("Failed to load roads script. Exiting");
            return;
        }
        roadScript.ForceStart();

        SetVisualCanvasItems(true);
        AdvancingRoad();
        DroneMovement();
    }

    private void initTimer()
    {
        Debug.Log("initTimer");
        timeLeft = timeToSurvive;

        InvokeRepeating("Run", 1, 1);
    }

    private void Run()
    {
        if (timeLeft != 0)
        {
            timeLeft -= 1;
            LevelTimeText.text = timeLeft + " sec";
        }
        else
        {
            CancelInvoke("Run");
            FinishLevel(false);
        }
    }

    private void DroneMovement()
    {
        Vector3 currentLocation = Drone.transform.position;

        Drone.transform.position = currentLocation + 
                                   (roadScript.frontWall.transform.position + new Vector3(0, 5, 0) - currentLocation) * 
                                   roadTileCreationSpeed / droneRedrawSpeed;

        Drone.transform.Rotate(roadScript.newTileRotation * roadTileCreationSpeed / droneRedrawSpeed);

        Invoke("DroneMovement", 1 / droneRedrawSpeed);
    }

    private void AdvancingRoad()
    {
        if (!this.GameOver)
        {
            roadScript.AdvanceRoad();

            Invoke("AdvancingRoad", 1 / roadTileCreationSpeed);
        }
    }

    public void FinishLevel(bool playerLost)
    {
        this.GameOver = true;
        playerScript.GameOver = true;

        SetVisualCanvasItems(false);

        if (playerLost)
            showModalFailLevelDialog();
        else
            showModalSuccessLevelCompletedDialog();
    }

    private void SetVisualCanvasItems(bool status)
    {
        Button[] canvasButtons = MainModalPanel.GetComponentsInChildren<Button>();
        if (canvasButtons != null && canvasButtons.Length != 0)
        {
            foreach (Button btn in canvasButtons)
                btn.gameObject.SetActive(status);
        }

        Image[] canvasImages = MainModalPanel.GetComponentsInChildren<Image>();
        if (canvasImages != null && canvasImages.Length != 0)
        {
            foreach (Image img in canvasImages)
            {
                if (img.name == "TimerImage")
                    img.gameObject.SetActive(status);
            }
        }
    }

    private bool IsNextTileAnObsticle()
    {
        return UnityEngine.Random.Range(0, directionChangingFrequency + 1) == 0;
    }

    private void InitAllPanelsAndDialogs()
    {
        dialogManager = new DialogManager();

        Dictionary<Button, UnityAction> successModalDictionary = new Dictionary<Button, UnityAction>();
        successModalDictionary.Add(SuccessModalNextLvlButton, loadNextLevelModalDialog);
        successModalDictionary.Add(SuccessModalOkButton, cancelAreYouSureDialog);

        ModalDialog successModalDialog = new ModalDialog(SuccessModalPanel, successModalDictionary, MainModalPanel);
        dialogManager.AddDialog(successModalDialog);

        Dictionary<Button, UnityAction> failModalDictionary = new Dictionary<Button, UnityAction>();
        failModalDictionary.Add(FailModalRepeatLvlButton, repeatLevelFailModalDialog);
        failModalDictionary.Add(FailModalOkButton, cancelAreYouSureDialog);

        ModalDialog failModalDialog = new ModalDialog(FailModalPanel, failModalDictionary, MainModalPanel);
        dialogManager.AddDialog(failModalDialog);

        Dictionary<Button, UnityAction> areYouSureModalDictionary = new Dictionary<Button, UnityAction>();
        areYouSureModalDictionary.Add(AreYouSureOkButton, okAreYouSureDialog);
        areYouSureModalDictionary.Add(AreYouSureCancelButton, cancelAreYouSureDialog);

        ModalDialog areYouSureModalDialog = new ModalDialog(AreYouSureModalPanel, areYouSureModalDictionary, MainModalPanel);
        dialogManager.AddDialog(areYouSureModalDialog);

        //Dictionary<Button, UnityAction> menuModalDictionary = new Dictionary<Button, UnityAction>();
        //menuModalDictionary.Add(MenuButton, menuButtonInvocation);

        //ModalDialog menuButtonModalDialog = new ModalDialog(MainModalPanel, menuModalDictionary, MainModalPanel);
        //dialogManager.AddDialog(menuButtonModalDialog);

        dialogManager.CloseAllOpenedModalDialogs();
    }

    //private void menuButtonInvocation()
    //{
    //    dialogManager.ShowModalDialog(AreYouSureModalPanel, MainModalPanel);
    //}

    private void loadNextLevelModalDialog()
    {
        GameOver = false;
        playerScript.RestartPlayer();
        //roadScript.InitRoad();

        initTimer();

        roadScript.ForceStart();
        dialogManager.CloseAllOpenedModalDialogs();

        SetVisualCanvasItems(true);
        AdvancingRoad();
        DroneMovement();
    }

    private void repeatLevelFailModalDialog()
    {
        dialogManager.CloseAllOpenedModalDialogs();
    }

    private void cancelAreYouSureDialog()
    {
        dialogManager.CloseAllOpenedModalDialogs();
    }

    private void okAreYouSureDialog()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void showModalSuccessLevelCompletedDialog()
    {
        CancelInvoke("spawnPlatform");
        dialogManager.ShowModalDialog(SuccessModalPanel, MainModalPanel);

        //WorldManager.Instance.Level++;
    }

    private void showModalFailLevelDialog()
    {
        CancelInvoke("spawnPlatform");
        dialogManager.ShowModalDialog(FailModalPanel, MainModalPanel);
        //playerScript.SetSpeed(0);
        //playerScript.SetMovingDirection(true);


    }
}