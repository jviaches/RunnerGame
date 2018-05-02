using Assets.scripts.DialogModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

<<<<<<< HEAD
public class LevelManagerScript : MonoBehaviour
{
    public GameObject RoadManager;
    public float roadTileCreationSpeed = 1;     // tile per second
    public int directionChangingFrequency = 3;  // Random 0 =  change direction

    public float droneRedrawSpeed = 60f;
    public GameObject Drone;

    public PlayerControllerScript playerScript;
    
    public int Level = WorldManager.Instance.Level;
    public int timeToSurvive = 4; //in seconds

    public GameObject MainModalPanel;               // ..
    public GameObject SuccessModalPanel;            // Modal Dialog when user successfully completed level
    public Text SuccessModalTitle;                  // Caption of modal dialog
    public Button SuccessModalOkButton;             // OK button in Success modal dialog
    public Button SuccessModalNextLvlButton;        // Next level button in Success modal dialog

    public GameObject FailModalPanel;               // Modal Dialog when user Fail to complete level
    public Text FailModalTitle;                     // Caption of modal dialog
    public Button FailModalOkButton;                // OK button in Fail modal dialog
    public Button FailModalRepeatLvlButton;         // repeat level button in Fail modal dialog

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

        AdvancingRoad();
        DroneMovement();
    }

    private void initTimer()
    {
        Debug.Log("initTimer");
        timeLeft = timeToSurvive;

        RestartTimer();
        Run();
    }

    private void Run()
    {
        if (timeLeft != 0)
        {
            timeLeft = timeLeft - 1;
            LevelTimeText.text = "Time: " + timeLeft + " sec";

            Invoke("Run", 1);
        }
        else
        {
            FinishLevel(false);
        }
    }

    private void RestartTimer()
    {
        timeLeft = timeToSurvive;
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
        if (!GameOver)
        {
            roadScript.ForceStart();
            roadScript.AdvanceRoad();

            Invoke("AdvancingRoad", 1 / roadTileCreationSpeed);
        }
    }

    public void FinishLevel(bool playerLost)
    {
        GameOver = true;
        playerScript.GameOver = true;

        if (playerLost)
            showModalFailLevelDialog();
        else
            showModalSuccessLevelCompletedDialog();      
    }

    /*
=======
public class LevelManagerScript : MonoBehaviour {

	public GameObject RoadManager;
	public int directionChangingFrequency =3; // Random 0 =  change direction
	public float droneRedrawSpeed = 60f;
	public GameObject Drone;
	private bool GameOver = false;
	public PlayerControllerScript playerScript;
	public GameObject GameOverPanel;
	public Text EndLevelMessageText;

	private Timer timerScript;

	public void FinishLevel(bool playerLost){
		if (playerLost) {
			
		

			Debug.Log ("Player has lost. GameOver.");
		} else {
			

			EndLevelMessageText.text = "Level Completed !";

			Debug.Log ("Player has Won. GameOver.");
		}
		roadScript.frontWall.GetComponent<FrontWallAnimation> ().StopAnimation ();
		playerScript.GameOver = true;
		GameOverPanel.SetActive (true);
		GameOver = true;
	}


	public float roadTileCreationSpeed=1;//tile per second


	private RoadGenerationScript roadScript;


	void Start () {
		GameOverPanel.SetActive (false);
		timerScript =this.GetComponent<Timer>();
		roadScript = (RoadGenerationScript)RoadManager.GetComponent<RoadGenerationScript> ();
		if(roadScript==null){
			Debug.Log ("Failed to load roads script. Exiting");
			return;
		}
		timerScript.StartTimer ();
		AdvancingRoad ();
		DroneMovement ();

	}
	private void DroneMovement (){
		Vector3 currentLocation = Drone.transform.position;
		Drone.transform.position = currentLocation + (roadScript.frontWall.transform.position+ new Vector3(0,5,0) - currentLocation)*roadTileCreationSpeed  / droneRedrawSpeed ;
		Drone.transform.Rotate (roadScript.newTileRotation*roadTileCreationSpeed  / droneRedrawSpeed );
		Invoke ("DroneMovement", 1 / droneRedrawSpeed);
	}

	private void AdvancingRoad(){
		if (!GameOver) {
			roadScript.ForceStart ();
			roadScript.AdvanceRoad ();
			Invoke ("AdvancingRoad", 1 / roadTileCreationSpeed);
		}
	}

	/*
>>>>>>> RoadRework
		calculating if next tile should be an obsticle
		for now we are using simple Random, but could use complex function 
		to determine complexity level
	*/
    private bool IsNextTileAnObsticle()
    {
        return UnityEngine.Random.Range(0, directionChangingFrequency + 1) == 0;
    }

    private void InitAllPanelsAndDialogs()
    {
        dialogManager = new DialogManager();

        Dictionary<Button, UnityAction> successModalDictionary = new Dictionary<Button, UnityAction>();
        successModalDictionary.Add(SuccessModalNextLvlButton, loadNextLevelModalDialog);
        successModalDictionary.Add(SuccessModalOkButton, loadMainMenuModalDialog);

        ModalDialog successModalDialog = new ModalDialog(SuccessModalTitle, SuccessModalPanel, successModalDictionary, MainModalPanel);
        dialogManager.AddDialog(successModalDialog);

        Dictionary<Button, UnityAction> failModalDictionary = new Dictionary<Button, UnityAction>();
        failModalDictionary.Add(FailModalRepeatLvlButton, repeatLevelFailModalDialog);
        failModalDictionary.Add(FailModalOkButton, loadMainMenuModalDialog);

        ModalDialog failModalDialog = new ModalDialog(FailModalTitle, FailModalPanel, failModalDictionary, MainModalPanel);
        dialogManager.AddDialog(failModalDialog);

        dialogManager.CloseAllOpenedModalDialogs();
    }

    private void loadNextLevelModalDialog()
    {
        dialogManager.CloseAllOpenedModalDialogs();

        //startLevel();
    }

    private void repeatLevelFailModalDialog()
    {
        dialogManager.CloseAllOpenedModalDialogs();

        //startLevel();
    }

    private void loadMainMenuModalDialog()
    {
        SceneManager.LoadScene(0);
    }

    private void showModalSuccessLevelCompletedDialog()
    {
        GameOver = false;
        CancelInvoke("spawnPlatform");

        //playerScript.SetSpeed(0);

        SuccessModalPanel.transform.position = MainModalPanel.transform.position;
        SuccessModalPanel.SetActive(true);
        SuccessModalTitle.text = "Level " + Level + " completed !";

        WorldManager.Instance.Level++;
    }

    private void showModalFailLevelDialog()
    {
        GameOver = false;
        CancelInvoke("spawnPlatform");

        //playerScript.SetSpeed(0);
        //playerScript.SetMovingDirection(true);

        dialogManager.ShowModalDialog(FailModalPanel, MainModalPanel);
        FailModalTitle.text = "Level " + Level + " Failed !";
    }
}