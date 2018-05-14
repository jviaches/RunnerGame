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
    public GameObject RoadManager;              // responsible for dynamic road creation
    public float roadTileCreationSpeed = 1;     // tile per second
    public int directionChangingFrequency = 3;  // Random 0 =  change direction

    public float droneRedrawSpeed = 60f;
    public GameObject Drone;                    // flying drone simulating road creation

    //public PlayerControllerScript playerScript; // Script attached to buggy

    public int Level = 1;//WorldManager.Instance.Level;
    public int timeToSurviveSec = 4;                //Time ot survive to next level

    public GameObject MainModalPanel;               // Canvas
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
   // private RoadGenerationScript script_RoadScript;
   //private EventManager script_eventManager;
    private int timeLeft;

    //private ObsticalManager obsticalsScript;
    private Button[] canvasButtons;


    private Dictionary<string, MonoBehaviour> scriptsDictionary;

    private void OnEnable()
    {
        EventManager.LevelFailed += LevelEndedWithFail;
    }

    private void OnDisable()
    {
        EventManager.LevelFailed -= LevelEndedWithFail;
    }

      public void Start()
    {
        Debug.Log("Start");
        OrganizeScripts();

        

        InitAllPanelsAndDialogs();
        canvasButtons = MainModalPanel.GetComponentsInChildren<Button>(); // left and rightdirection buttons

        initTimer();


        RoadGenerationScript rs = (RoadGenerationScript)scriptsDictionary["RoadGenerationScript"];
        if (rs == null)
        {
            Debug.Log("Failed to load roads script. Exiting");
            return;
        }
        rs.ForceStart();

        SetVisualCanvasItems(true);
        AdvancingRoad();
        ((EventManager)scriptsDictionary["EventManager"]).FireRestartLevelEvent();

        DroneMovement();
    }

    void OrganizeScripts()
    {
        if (scriptsDictionary==null) scriptsDictionary = new Dictionary<string, MonoBehaviour>();
        scriptsDictionary.Add("ObsticalManager", (MonoBehaviour)GameObject.Find("ObsticalManagerObject").GetComponent<ObsticalManager>());
        scriptsDictionary.Add("EventManager", GameObject.Find("EventManager").GetComponent<EventManager>());
        scriptsDictionary.Add("RoadGenerationScript",  RoadManager.GetComponent<RoadGenerationScript>());
    }


    public MonoBehaviour GetScript(string scriptName)
    {
        return scriptsDictionary[scriptName];
    }

    private void initTimer()
    {
        Debug.Log("initTimer");
        timeLeft = timeToSurviveSec;

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
            ((EventManager) scriptsDictionary["EventManager"]).FireLevelWonEvent();
            FinishLevel(false);
        }
    }

    void LevelEndedWithFail()
    {
        FinishLevel(true);
    }

    private void DroneMovement()
    {
        Vector3 currentLocation = Drone.transform.position;
        RoadGenerationScript rs = (RoadGenerationScript)scriptsDictionary["RoadGenerationScript"];
        Drone.transform.position = currentLocation + 
                                   (rs.frontWall.transform.position + new Vector3(0, 5, 0) - currentLocation) * 
                                   roadTileCreationSpeed / droneRedrawSpeed;

        Drone.transform.Rotate(rs.newTileRotation * roadTileCreationSpeed / droneRedrawSpeed);

        Invoke("DroneMovement", 1 / droneRedrawSpeed);
    }

    private void AdvancingRoad()
    {
        if (!this.GameOver)
        {
            ((RoadGenerationScript)scriptsDictionary["RoadGenerationScript"]).AdvanceRoad();

            Invoke("AdvancingRoad", 1 / roadTileCreationSpeed);
        }
    }

    public void FinishLevel(bool playerLost)
    {
        GameOver = true;
      
        SetVisualCanvasItems(false);

        if (playerLost)
            showModalFailLevelDialog();
        else
            showModalSuccessLevelCompletedDialog();
    }

    private void SetVisualCanvasItems(bool status)
    {
        if (canvasButtons != null && canvasButtons.Length != 0)
        {
            foreach (Button btn in canvasButtons)
                btn.gameObject.SetActive(status);
        }
        Debug.Log("canvasButtons amount=" + canvasButtons.Length);

        Image[] canvasImages = MainModalPanel.GetComponentsInChildren<Image>();
        if (canvasImages != null)
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
        successModalDictionary.Add(SuccessModalOkButton, okAreYouSureDialog);

        ModalDialog successModalDialog = new ModalDialog(SuccessModalPanel, successModalDictionary, MainModalPanel);
        dialogManager.AddDialog(successModalDialog);

        Dictionary<Button, UnityAction> failModalDictionary = new Dictionary<Button, UnityAction>();
        failModalDictionary.Add(FailModalRepeatLvlButton, repeatLevelFailModalDialog);
        failModalDictionary.Add(FailModalOkButton, okAreYouSureDialog);

        ModalDialog failModalDialog = new ModalDialog(FailModalPanel, failModalDictionary, MainModalPanel);
        dialogManager.AddDialog(failModalDialog);

        //Dictionary<Button, UnityAction> areYouSureModalDictionary = new Dictionary<Button, UnityAction>();
        //areYouSureModalDictionary.Add(AreYouSureOkButton, okAreYouSureDialog);
        //areYouSureModalDictionary.Add(AreYouSureCancelButton, cancelAreYouSureDialog);

        //ModalDialog areYouSureModalDialog = new ModalDialog(AreYouSureModalPanel, areYouSureModalDictionary, MainModalPanel);
        //dialogManager.AddDialog(areYouSureModalDialog);

        //Dictionary<Button, UnityAction> menuModalDictionary = new Dictionary<Button, UnityAction>();
        //menuModalDictionary.Add(MenuButton, menuButtonInvocation);

        //ModalDialog menuButtonModalDialog = new ModalDialog(MainModalPanel, menuModalDictionary, MainModalPanel);
        //dialogManager.AddDialog(menuButtonModalDialog);

        dialogManager.CloseAllOpenedModalDialogs();
    }

    private void loadNextLevelModalDialog()
    {
        GameOver = false;
        ((EventManager)scriptsDictionary["EventManager"]).FireRestartLevelEvent();

        initTimer();

        dialogManager.CloseAllOpenedModalDialogs();

        SetVisualCanvasItems(true);

        AdvancingRoad();
        DroneMovement();
    }

    private void repeatLevelFailModalDialog()
    {
        //temporary
        loadNextLevelModalDialog();
    }

    //private void cancelAreYouSureDialog()
    //{
    //    dialogManager.CloseAllOpenedModalDialogs();
    //}

    private void okAreYouSureDialog()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void showModalSuccessLevelCompletedDialog()
    {
        CancelInvoke("spawnPlatform");
        dialogManager.ShowModalDialog(dialogManager.ModalsDialogList[0]);
    }

    private void showModalFailLevelDialog()
    {
        CancelInvoke("spawnPlatform");
        dialogManager.ShowModalDialog(dialogManager.ModalsDialogList[1]);
    }
}