using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnitySampleAssets.Characters.FirstPerson;

public class ContainerInbox : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
	Inventory inventory;
	public GameObject[] droppedItems; //list of all items
	public List <GameObject> nearUs = new List<GameObject>(); //list of close items that will show on dropbox
	public GameObject itemBox;
	public List<GameObject> listItembox = new List<GameObject>();
	
	private int boxOffset = 48; // where the box starts
	private int boxHeight = 20; // where the next one is made
	
	public Container container; // used to find the items in the container u look at
	public FirstPersonController player;
	private Interact interactScript;
	
	private Text number;
	
	void Awake()
	{
		interactScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Interact> ();
	}
	void Start () 
	{
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory> (); // find the inventory script
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController>();
		number = transform.GetChild(1).GetComponent<Text>();
	}
	public void updateIndex()
	{
		for (int i = 0; i < listItembox.Count; i++)
		{
			listItembox[i].GetComponent<ContainerItem>().index = i;
		}
	}
	public void createItemInBox()
	{
		Vector3 posi = new Vector3 (0, (-boxHeight * listItembox.Count) + boxOffset, 0); // set location of box
		GameObject item = Instantiate (itemBox) as GameObject; // create box
		
		ContainerItem boxWithItem = item.GetComponent<ContainerItem>(); // access script
		boxWithItem.index = listItembox.Count; // 
		boxWithItem.item = nearUs [listItembox.Count].GetComponent<DroppedItem>().item; // get the item information
		
		listItembox.Add (item); //add box to list
		item.transform.SetParent (this.gameObject.transform, true); // new set parent
		item.GetComponent<RectTransform> ().localPosition = posi; // change the location of box
		item.transform.GetChild (0).GetComponent<Image>().sprite = nearUs [listItembox.Count - 1].GetComponent<DroppedItem> ().item.itemIcon; // gets and sets icon
		item.transform.GetChild (1).GetComponent<Text>().text = nearUs [listItembox.Count - 1].GetComponent<DroppedItem> ().item.itemName; // gets and sets name
	}
	public void updateItemboxPosition()
	{
		for (int i = 0; i < listItembox.Count; i++)
		{
			Vector3 posi = new Vector3(0, (-boxHeight * i) + boxOffset, 0);
			listItembox[i].GetComponent<RectTransform>().localPosition = posi;
		}
	}
	public void GetItemsInContainer() // check the items in the container and then creates the UI
	{
		container = interactScript.container;
        print(container.name);
		
		//int test = container.transform.childCount; // find the total number of items in a container
		int test = 0;

        print(container.transform.childCount);
		for(int i = 0; i < container.transform.childCount; ++i)
		{
			if(container.transform.GetChild(i).gameObject.tag == "ContainerItem")
			++ test;
		}
		droppedItems = new GameObject[test]; // set the array to the number of items in the container
		
		for(int i = 0; i < droppedItems.Length; i++)
		{
			droppedItems[i] = container.transform.GetChild(i).gameObject;
			//print(droppedItems[i]);
			
			ItemClass item = droppedItems[i].GetComponent<DroppedItem>().item;
			
			if(nearUs.Count == 0) // if list is empty instantly add the item
			{
				nearUs.Add(droppedItems[i]);
				createItemInBox();
			}
			else
			{
				bool temp = false;
				
				for (int k = 0; k < nearUs.Count; ++k)
				{
					if(nearUs[k] != null)
					{
						if(nearUs[k].GetComponent<DroppedItem>().item.Equals(item))
						{
							temp = true;
						}
						else if (!temp && k == nearUs.Count - 1)
						{
							nearUs.Add(droppedItems[i]);
							createItemInBox();
						}
					}
				}
			}
		}
	}
	public void closeContainer() // takes item out of list when out of range
	{
		for(int i = 0; i < nearUs.Count; i++)
		{
			Destroy(listItembox[i]);
			updateItemboxPosition();
			updateIndex();
		}
		listItembox.Clear();
		nearUs.Clear();
	}
	public void OnPointerDown(PointerEventData data)
	{
		/*
		if (interactScript.container.usedSlots < interactScript.container.getTotalSlots && Inventory.draggingItem) 
		{
			interactScript.container.usedSlots += 1;
			dropItem(Inventory.DraggedItem);
			inventory.closeDraggedItem();
			GetItemsInContainer();
			UpdateText();
			inventory.TurnOnItem();
		} */
		//else if (interactScript.container.usedSlots == 0)
		//{
		//	print ("Too many items");
		//}
	}
	public void OnPointerEnter(PointerEventData data)   //for mouse over item
	{
		//inventory.closeTooltip();
	}
	public void dropper()
	{
		interactScript.container.usedSlots += 1;
		GetItemsInContainer(); // recreates the list

		UpdateText();
		inventory.closeTooltip();
	}
	public void dropItem(ItemClass item)
	{
		//-- inventory.slotsFilled; // taken out so that when you take items out of inventory it does it faster
		GameObject itemAsGameobject = Instantiate (item.itemModel, interactScript.container.transform.position, Quaternion.identity) as GameObject;
		itemAsGameobject.GetComponent<Collider>().enabled = false; // makes it so the item can be in the container
		itemAsGameobject.GetComponent<Rigidbody>().isKinematic = true; // makes it so the item will not fall through the world
		itemAsGameobject.transform.parent = interactScript.container.transform; // parents the item to the container
		itemAsGameobject.transform.SetSiblingIndex (0);
		itemAsGameobject.GetComponent<MeshRenderer> ().enabled = false; // turn the renderer off the mesh s oit does'nt poke out
		itemAsGameobject.tag = "ContainerItem"; // changes the name to be accessed by contianer
		itemAsGameobject.GetComponent<DroppedItem>().item = item;
		inventory.TurnOnItem();
	}
	public void OpenSlots()
	{
		interactScript.container.usedSlots -= 1;
	}
	public void UpdateText()
	{
		number.text = interactScript.container.usedSlots.ToString() + " / " + interactScript.container.getTotalSlots.ToString();
	}
	public void NewDropUnClick()
    {
		//if(Inventory.draggingItem)
		//{
			dropItem(Inventory.DraggedItem); // drop item on floor
			inventory.closeDraggedItem(); //
			GetItemsInContainer();
			interactScript.container.usedSlots += 1; // change amount of items in container
			UpdateText();
			inventory.TurnOnItem();
		//}
    }
}