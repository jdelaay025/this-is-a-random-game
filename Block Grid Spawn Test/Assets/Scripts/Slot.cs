using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Slot : MonoBehaviour, IDropHandler 
{

    #region Global Variable Declaration

    public int id;

	Inventory inv;

    #endregion

    public void Start()
	{
		inv = GameMasterObject.inventory;
	}

	public void OnDrop (PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();

		if(inv.items[id].ID == -1)
		{
			inv.items [droppedItem.slot] = new Item ();
			inv.items [id] = droppedItem.item;
			droppedItem.slot = id;
		}
		else if(inv.items[id].ID >= 0 && droppedItem.slot != id)
		{
			Transform item = this.transform.GetChild (0);
			item.GetComponent<ItemData>().slot = droppedItem.slot;
			item.transform.SetParent (inv.slots[droppedItem.slot].transform);
			item.transform.position = inv.slots [droppedItem.slot].transform.position;

			droppedItem.slot = id;
			droppedItem.transform.SetParent (this.transform);
			droppedItem.transform.position = this.transform.position;

			inv.items [droppedItem.slot] = item.GetComponent<ItemData> ().item;
			inv.items [id] = droppedItem.item;
		}
	}
}
