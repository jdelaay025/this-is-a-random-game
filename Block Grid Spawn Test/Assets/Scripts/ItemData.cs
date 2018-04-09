using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler
{
	public Item item;
	public int amount;
	public int slot;

	Inventory inv;
	ToolTip toolTip;
	Vector2 offset;

	public void Start()
	{
		inv = GameMasterObject.inventory;
		toolTip = inv.GetComponent<ToolTip> ();
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if(item != null)
		{
			#region commented out and put into OnPointerDown function
//			offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
			#endregion

			this.transform.SetParent (this.transform.parent.parent);
			this.transform.position = eventData.position - offset;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}
	public void OnDrag (PointerEventData eventData)
	{
		if(item != null)
		{
			this.transform.position = eventData.position - offset;
		}
	}
	public void OnEndDrag (PointerEventData eventData)
	{
		this.transform.SetParent (inv.slots[slot].transform);
		this.transform.position = inv.slots[slot].transform.position;

		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if(item != null)
		{
			offset = eventData.position - new Vector2 (this.transform.position.x, this.transform.position.y);
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		toolTip.Activate (item);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		toolTip.Deactivate ();
	}
}
