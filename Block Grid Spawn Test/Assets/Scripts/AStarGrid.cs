using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarGrid : MonoBehaviour 
{
	public Transform player;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	AStarNode[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		CreateGrid ();

	}

	void CreateGrid()
	{
		grid = new AStarNode[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				grid[x,y] = new AStarNode(walkable, worldPoint, x, y);				
			}
		}
	}

	public List<AStarNode> getNeighbors(AStarNode node)
	{
		List<AStarNode> neighbors = new List<AStarNode> ();

		for(int x = -1; x<= 1; x++)
		{
			for(int y = -1; y<= 1; y++)
			{
				if(x == 0 && y == 0)
				{
					continue;
				}

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbors.Add (grid[checkX, checkY]);
				}
			}
		}
		return neighbors;
	}

	public AStarNode NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid [x,y];
	}

	public List<AStarNode> path;

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
	
		if(grid != null)
		{
			AStarNode playerNode = NodeFromWorldPoint(player.position);
			foreach(AStarNode n in grid)
			{
				Gizmos.color = (n.walkable)?Color.white:Color.blue;
				if(playerNode == n)
				{
					Gizmos.color = Color.blue;
				}

				/*if(path != null)
				{
					if(path.Contains(n))
					{
						Gizmos.color = Color.black;
					}
				}*/

				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}
}
