using UnityEngine;
using System.Collections;

public class AStarNode
{
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public AStarNode parent;


	public AStarNode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;

	}

	public int fCost
	{
		get{return gCost + hCost;}
	}
}
