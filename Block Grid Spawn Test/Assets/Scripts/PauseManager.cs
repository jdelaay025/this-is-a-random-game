using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour 
{
    #region Global Variable Declaration

    public static PauseManager Instance;

    [SerializeField]
    Transform gameplayTarget;
    [SerializeField]
    Transform pauseTarget;
    [SerializeField]
    float turnSpeed = 5f;
    [SerializeField]
    bool paused = false;
    [SerializeField]
    GameObject pauseCanvas;
    [SerializeField]
    ChangeButtonDisplay settingsButton;

    Transform myTransform;
    Transform camTrans;
        
	#endregion

	void Awake () 
	{
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        myTransform = transform;
        camTrans = Camera.main.transform;
	}

	void Start () 
	{
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(paused);
        }
    }

    private void Update()
    {
        if (paused)
        {
            RotateTowardsTarget(pauseTarget, turnSpeed);
        }
        else
        {
            RotateTowardsTarget(gameplayTarget, turnSpeed);
        }
    }

    public void SetPause()
    {
        paused = !paused;
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(paused);
        }
    }

    public void ClosePauseMenu()
    {
        paused = false;
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(paused);
        }

        if(settingsButton != null)
        {
            settingsButton.SetDisplayToSettings();
        }
    }

    void RotateTowardsTarget (Transform target, float speed) 
	{
        if (camTrans != null)
        {
            Vector3 targetDir = target.position - transform.position;

            // The step size is equal to speed times frame time.
            float step = speed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(camTrans.forward, targetDir, step, 0.0f);
            // Debug.DrawRay(camTrans.position, newDir, Color.red);

            // Move our position a step closer to the target.
            camTrans.rotation = Quaternion.LookRotation(newDir);
        }
    }

}
