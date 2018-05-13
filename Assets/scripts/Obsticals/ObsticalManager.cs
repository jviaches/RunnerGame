using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalManager : MonoBehaviour {

    public int AmountOfObsticals = 100;

    private ArrayList q_obsticalsOnTheRoad;
    private ArrayList pool_obsticalsPool;
    private bool isItitialized = false;

    private string obsticlePath = "Obsticals/";
    private Vector3 offSceneLocation = new Vector3(1000, 1000, 1000);
    private float maxAnimationStartingHeight = 35f;

    private int landingForceAmlification = 5000;
    private float tileSize;
    
	private float delayInSeconds;

	private bool GameOver = false;


    private RoadGenerationScript roadScript;



    private void OnEnable()
    {
        EventManager.LevelFailed += FinishLevel;
        EventManager.LevelWon += FinishLevel;
    }

    private void OnDisable()
    {
        EventManager.LevelFailed -= FinishLevel;
        EventManager.LevelWon -= FinishLevel;
    }
    void Start () {
       
	}

	public void ResetAllObsticals(){
		int amount = q_obsticalsOnTheRoad.Count;
		for (int i = 0; i < amount; i++) {
			GameObject m = (GameObject) q_obsticalsOnTheRoad [0];
			q_obsticalsOnTheRoad.RemoveAt (0);
			ResetMetheor (m);
			pool_obsticalsPool.Add (m);
		}

	}

	 void FinishLevel(){
		GameOver = true;
		ResetAllObsticals ();
	}
	public void SendMetheorToARandomLocation(bool repeat , float _delayInSeconds)
    {
		GameOver = false;
		delayInSeconds = _delayInSeconds;
  

        if (roadScript!=null && roadScript.q_tileLocations!=null && roadScript.q_tileLocations.Count > 0)
        {
            int tileId = UnityEngine.Random.Range(0, roadScript.q_tileLocations.Count - 1);
			Vector3 landingPoint = (Vector3)roadScript.q_tileLocations[tileId] + new Vector3((float)Random.Range(-1*tileSize,tileSize) ,0,(float)Random.Range(-1*tileSize,tileSize));
            SendMetheor(landingPoint);
        }
		if(repeat)
        Invoke("SendMetheorToRandomLocation", delayInSeconds);
    }


	private void SendMetheorToRandomLocation(){

	if (roadScript!=null && roadScript.q_tileLocations!=null && roadScript.q_tileLocations.Count > 0)
	{
		int tileId = UnityEngine.Random.Range(0, roadScript.q_tileLocations.Count - 1);
		Vector3 landingPoint = (Vector3)roadScript.q_tileLocations[tileId] + new Vector3((float)Random.Range(-1*tileSize,tileSize) ,0,(float)Random.Range(-1*tileSize,tileSize));
		SendMetheor(landingPoint);
	}
		if(!GameOver)
		Invoke("SendMetheorToRandomLocation", delayInSeconds);
}

    public void Init()
    {


        if (isItitialized) return;

        pool_obsticalsPool = new ArrayList();
        q_obsticalsOnTheRoad = new ArrayList();

        for(int i=0; i<AmountOfObsticals+2;i++)
        {
            GameObject temp = InstanciateObject(obsticlePath+"Metheorite",offSceneLocation,Vector3.zero,"metheorite");
            Transform rock = temp.transform.Find("Rock");
            rock.GetComponent<Rigidbody>().useGravity = false;
            rock.GetComponent<Rigidbody>().isKinematic = true;
            ParticleSystem ps = null;
            ps = (ParticleSystem)(temp.transform.Find("DebriePS").GetComponent<ParticleSystem>());
            rock.GetComponent<MetheorCollisionDetection>().debrie = ps;
            ps = (ParticleSystem)(temp.transform.Find("SmokePS").GetComponent<ParticleSystem>());
            rock.GetComponent<MetheorCollisionDetection>().smoke = ps;
            ps = (ParticleSystem)(rock.GetComponent<ParticleSystem>());
            rock.GetComponent<MetheorCollisionDetection>().exaust = ps;
			rock.GetComponent<MetheorCollisionDetection> ().target = (temp.transform.Find ("target").gameObject);

            temp.transform.position = offSceneLocation;
            StopMetheoriteAnimation(temp);
            pool_obsticalsPool.Add(temp);
        }

        roadScript = GameObject.Find("RoadManager").GetComponent<RoadGenerationScript>();
        tileSize = roadScript.tileSize;

        isItitialized = true;
    }


    public void ClearLastObsticle()
    {
        GameObject temp =(GameObject) q_obsticalsOnTheRoad[0];
        q_obsticalsOnTheRoad.RemoveAt(0);
        ResetMetheor(temp);
        pool_obsticalsPool.Add(temp);
    }

    private void ResetMetheor(GameObject m)
    {
        m.transform.position = offSceneLocation;
		m.transform.Find("Rock").position = offSceneLocation;
		m.transform.Find("target").position = offSceneLocation;
		m.transform.Find ("target").gameObject.SetActive (true);
		m.transform.Find("DebriePS").position = offSceneLocation;
		m.transform.Find("SmokePS").position = offSceneLocation;
        StopMetheoriteAnimation(m);

    }

    public void SendMetheor(Vector3 landingLocation)
    {
        if (q_obsticalsOnTheRoad.Count == AmountOfObsticals) ClearLastObsticle();
        GameObject m = (GameObject)pool_obsticalsPool[0];
        pool_obsticalsPool.RemoveAt(0);
		q_obsticalsOnTheRoad.Add(m);
        Vector3 startPosition = landingLocation + new Vector3(Random.Range(-1 * tileSize, tileSize),
                                                           Random.Range(tileSize, maxAnimationStartingHeight + tileSize),
                                                           Random.Range(-1 * tileSize, tileSize));
		m.transform.position = startPosition;
		m.transform.Find("Rock").position = startPosition;
       

		StartCoroutine(StartMetheoriteAnimation(m,landingLocation + new Vector3(0,0.01f,0)));
        
    }

    private void StopMetheoriteAnimation(GameObject metheorite) {
        ParticleSystem ps;
        ps = (ParticleSystem) (metheorite.transform.Find("DebriePS").GetComponent<ParticleSystem>());
        if (ps != null)
            ps.Stop();
        ps = (ParticleSystem)(metheorite.transform.Find("SmokePS").GetComponent<ParticleSystem>());
        if (ps != null)
            ps.Stop();
        ps = (ParticleSystem)(metheorite.transform.Find("Rock").GetComponent<ParticleSystem>());
        if (ps != null)
            ps.Stop();

        metheorite.transform.Find("Rock").GetComponent<Rigidbody>().isKinematic = true;
        metheorite.transform.Find("Rock").GetComponent<Rigidbody>().useGravity = false; ;
    }

	private IEnumerator StartMetheoriteAnimation(GameObject m, Vector3 landingLocation)
    {

		yield return new WaitForSeconds (0);
        Rigidbody rb = m.transform.Find("Rock").GetComponent<Rigidbody>();
        m.transform.Find("target").position = landingLocation;
        m.transform.Find("DebriePS").position = landingLocation;
        m.transform.Find("SmokePS").position = landingLocation;
        rb.isKinematic = false;
        rb.useGravity = false;
        ParticleSystem ps = (ParticleSystem)(m.transform.Find("Rock").GetComponent<ParticleSystem>());
        if (ps != null)
            ps.Play();
        rb.AddForce((landingLocation- m.transform.Find("Rock").transform.position)* landingForceAmlification, ForceMode.Force);
    }

   

    private GameObject InstanciateObject(string resourcePath, Vector3 location, Vector3 rotation, string tag)
    {
        GameObject result = null;

        try
        {
            result = Instantiate((GameObject)Resources.Load(resourcePath), location, Quaternion.LookRotation(rotation));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

       return result;
    }

    private bool AnimationWasTrigered(GameObject metheor)
    {
        return metheor.transform.Find("DebriePS").GetComponent<ParticleSystem>().isPlaying;
    }
    private bool HasLanded(GameObject m)
    {
        return m.transform.Find("Rock").transform.position.y == 0;
    }

    public void Update()
    {
     if (!isItitialized) return;

     foreach(GameObject m in q_obsticalsOnTheRoad)
        {

        }
    }

}
