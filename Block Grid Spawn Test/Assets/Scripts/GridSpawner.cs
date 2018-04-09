using UnityEngine;
using System.Collections;

public class GridSpawner : MonoBehaviour 
{

    #region Global Variable Declaration

    public GameObject block1;

	[SerializeField] int worldWidth = 10;
	[SerializeField] int worldHeight = 10;

	public float spawnSpeed = 0f;
	public int positionalOffset = 2;
	public float xzOffset;

	public bool generate = false;

    #endregion

    void Start () 
	{

		if(generate)
		{
			StartCoroutine (CreateWorld());
		}

		xzOffset = worldWidth / 2;

	}

	IEnumerator CreateWorld()
	{

		for (int x = 0; x < worldWidth; x += positionalOffset) 
		{
			yield return new WaitForSeconds (spawnSpeed);

			for (int z = 0; z < worldHeight; z += positionalOffset) 
			{
				yield return new WaitForSeconds (spawnSpeed);

				GameObject block = Instantiate (block1, transform.position, transform.rotation) as GameObject;

				block.transform.parent = transform;
				block.transform.position = new Vector3 (x + -xzOffset, 0f ,z + -xzOffset);
			}
		}

	}

}
