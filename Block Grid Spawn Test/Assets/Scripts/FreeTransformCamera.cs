using UnityEngine;
using System.Collections;

public class FreeTransformCamera : MonoBehaviour 
{

    #region Global Variable Declaration

    public bool freeTransform = false;
	public float movementSpeed = 0f;

	Transform myTransform;
	Vector3 initialPosition;
	Vector3 scrollPosition;

	float verticalPan = 0f;
	float horizontalPan = 0f;
	float vertPanMin = 10f;
	float vertPanMax = 80f;
	float horPanMin = 10f;
	float horPanMax = 80f;

	float mouseY = 0f;
	float mouseX = 0f;
	float scroll = 0f;
	public float minY = 10f;
	public float maxY = 80f;

	float panBoarderThickness = 150f;

	bool setBackToInitial = false;

    #endregion

    void Awake()
	{
		myTransform = transform;
		panBoarderThickness = Screen.height / 8;
	}
	void Start () 
	{
		initialPosition = myTransform.position;
	}
	void Update () 
	{
		mouseX = Input.mousePosition.x;
		mouseY = Input.mousePosition.y;

        if (Input.GetButtonDown("Chat"))
		{
			freeTransform = !freeTransform;
		}

        //		Debug.Log (panBoarderThickness);

        if (freeTransform) 
		{
			verticalPan = Input.GetAxis ("vertical");
			horizontalPan = Input.GetAxis ("horizontal");

			setBackToInitial = true;

			if (verticalPan > 0 || mouseY >= Screen.height - panBoarderThickness) 
			{
				myTransform.Translate (Vector3.forward * movementSpeed * Time.deltaTime, Space.World);
			}
			if (verticalPan < 0 || mouseY <= panBoarderThickness)
			{
				myTransform.Translate (-Vector3.forward * movementSpeed * Time.deltaTime, Space.World);
			}
			if (horizontalPan > 0 || mouseX >= Screen.width - panBoarderThickness) 
			{
				myTransform.Translate (Vector3.right * movementSpeed * Time.deltaTime, Space.World);
			}
			if (horizontalPan < 0 || mouseX <= panBoarderThickness) 
			{
				myTransform.Translate (-Vector3.right * movementSpeed * Time.deltaTime, Space.World);
			}

			scroll = Input.GetAxis ("Mouse ScrollWheel");
			if(scroll != 0 && freeTransform)
			{
				scrollPosition = myTransform.position;
				scrollPosition.y -= scroll * 350 * movementSpeed * Time.deltaTime;
				scrollPosition.y = Mathf.Clamp (scrollPosition.y, minY, maxY);

				myTransform.position = scrollPosition;
			}
			
		} 
		else if (!freeTransform && setBackToInitial) 
		{
			StartCoroutine(SetBackToInitial ());
		}
	}

	IEnumerator SetBackToInitial()
	{
		myTransform.position = Vector3.Lerp(initialPosition, myTransform.position, 0.4f);
		yield return new WaitForSeconds (1);
		setBackToInitial = false;
	}

}
