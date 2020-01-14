using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ContainerItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public ItemClass item;
	public int index; // 
	Inventory inventory;
	ContainerInbox containerBox;
	
	void Start () 
	{
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		containerBox = GameObject.Find ("ContainerDropBox").GetComponent<ContainerInbox> ();
	}
	public void OnPointerDown(PointerEventData data)
	{	}
	public void OnPointerUp(PointerEventData data)
	{ 
		if(data.button == PointerEventData.InputButton.Left && inventory.slotsFilled < 6 && !Inventory.draggingItem)
		{
			PickUpItem();
		}
		/*
		if(data.button == PointerEventData.InputButton.Left && inventory.slotsFilled < 6 && !Inventory.draggingItem)
		{
			print ("trying to drop2");
			/*	if(item.itemType == ItemClass.ItemType.HandsOnly)
			{
				if(inventory.Items [0].itemName == null || inventory.Items [1].itemName == null)
				{
					PickUpItem();
				}
			} // ends here
			if(item.itemType == ItemClass.ItemType.SmallItem) // if its small item
			{
				//if(inventory.Slots[5].GetComponent<Slot>().name == null;
				if(inventory.Items[5].itemName == null)
				{
					//PickUpItem();
				}
			}
			else if(item.itemType != ItemClass.ItemType.SmallItem && inventory.Items [0].itemName != null && inventory.Items[1].itemName != null && inventory.Items[2].itemName != null
			        && inventory.Items[3].itemName != null && inventory.Items[4].itemName != null) // if not a small item and other slots are filled
			{	}
			else
			{
				PickUpItem();
			}
		} */
	}
	public void OnDrag(PointerEventData data) //when you drag the item off
	{
		if(data.button == PointerEventData.InputButton.Left && inventory.slotsFilled < 6 && !Inventory.draggingItem)
		{
			NewDrag();

			/*Destroy(containerBox.listItembox[index]);
			Destroy(containerBox.nearUs[index]); // destroy the item you choose
			inventory.DragNonSlot(item);
			containerBox.listItembox.RemoveAt(index);
			containerBox.nearUs.RemoveAt(index); // remove the item index from list that you chose
			containerBox.updateIndex();  // update the index so ones below the ones you chose -1
			containerBox.updateItemboxPosition(); // reposition the items if you take one in the middle
			containerBox.OpenSlots(); // if an item is taken, add to open slots
			containerBox.UpdateText(); */
		}
	}
	public void OnPointerEnter(PointerEventData data)   //for mouse over item
	{
		if(Inventory.draggingItem == false)
		{
			inventory.showTooltip(this.gameObject.transform.position, item);
		}
	}
	public void OnPointerExit(PointerEventData data)   //for mouse exit item
	{
		inventory.closeTooltip();
	}
	void PickUpItem()
	{
		Destroy(containerBox.listItembox[index]); // destroys the item from the list
		Destroy(containerBox.nearUs[index]); // destroy the item from the container list
		containerBox.listItembox.RemoveAt(index);
		containerBox.nearUs.RemoveAt(index); // remove the item index from list that you chose
		containerBox.updateIndex();  // update the index so ones below the ones you chose -1 

		containerBox.updateItemboxPosition(); // reposition the items if you take one in the middle
		containerBox.OpenSlots(); // if an item is taken, add to open slots
		inventory.addExistingItem(item); //add item to inventory
		containerBox.UpdateText();
		inventory.closeTooltip();
	}
	public void NewDrag() // new for use when you drag container item
	{
		//Destroy(containerBox.listItembox[index]);
		inventory.DragNonSlot(item);

		//containerBox.listItembox[index].transform.parent = GameObject.Find("UI_Temp_Holder").transform; // new testing for drop
		containerBox.listItembox[index].transform.SetParent(GameObject.Find("UI_Temp_Holder").transform, false);
		containerBox.listItembox.RemoveAt(index);
		Destroy(containerBox.nearUs[index]); // destroy the item you choose, from near us array
		containerBox.nearUs.RemoveAt(index); // remove the item index from list that you chose

		containerBox.updateIndex();  // update the index so ones below the ones you chose -1
		containerBox.updateItemboxPosition(); // reposition the items if you take one in the middle
		containerBox.OpenSlots(); // if an item is taken, add to open slots
		containerBox.UpdateText(); // change text of open spots
	}
}