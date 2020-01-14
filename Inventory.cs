using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
	public List<ItemClass> Items = new List<ItemClass>();
	public List<GameObject> Slots = new List<GameObject>(); // inventory slots
	ItemDatabase database; //access to ItemDatabase
	public GameObject toolTip; // the item description
	public GameObject craftTip;
	public GameObject droppedIcon; // the dragged item

	public GameObject craftIcons;

	public static bool draggingItem = false; // if you are draggin an item
	public static ItemClass DraggedItem; // testing static
	public static int tempSlot;

	public int indexOfDraggedItem; // when you drag an item, this is the previous item to be replaced
	Jail_Brain jailBrain;
	Player_Information playerInformation;
	DropBox dropBox;
	public ItemClass resultItem;
	//put craft items back in inventory
	public GameObject resultSlot;
	//[HideInInspector]
	public int slotsFilled = 0; // int used to know when all the slots are filled

	private GameObject handPos; // position hand item will go to
	[SerializeField]
	private GameObject[] usableItems = new GameObject[7];

	//keys
	private GameObject[] Keys = new GameObject[3];
	// for new crafting 
	BlueprintDatabase bluedataBase;
	public List<int> itemID = new List<int>();
	public List<ItemClass> possibleItems = new List<ItemClass>();
	public CraftIcon[] icons;
    // quick craft
    //public List<ItemClass> qCraft = new List<ItemClass>(); // take 1 out when 1 is put in q-Craft
    public GameObject[] craftSlots;

	public GameObject[] prisoners;
	public List<string> prisonerNames = new List<string>();

	void Awake()
	{
		database = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase> ();
		dropBox = GameObject.FindGameObjectWithTag("DropBox").GetComponent<DropBox>();
		jailBrain = GameObject.Find ("Jail Brain").transform.GetComponent<Jail_Brain> ();
		bluedataBase = GameObject.FindGameObjectWithTag("BlueprintDatabase").GetComponent<BlueprintDatabase>();

		playerInformation = GameObject.FindGameObjectWithTag ("Player").transform.GetComponent<Player_Information> ();
	}
	void Start()
	{
		handPos = playerInformation.rightHandAttachPoint.gameObject;
		toolTip = GameObject.Find ("ToolTip");
		toolTip.SetActive (false);
		droppedIcon = GameObject.Find ("DroppedIcon");
		droppedIcon.SetActive (false);
		craftTip = GameObject.Find("CraftTip");
		//craftTip.SetActive(false); //it's parent is turned off at start
		craftIcons = GameObject.Find("Sprites Parent").gameObject;

		//print (playerInformation.rightHandAttachPoint);

		usableItems [0] = playerInformation.rightHandAttachPoint.transform.Find ("Kitchen Knife").gameObject;
		usableItems [1] = playerInformation.rightHandAttachPoint.transform.Find ("Screwdriver (sharp)").gameObject; 
		usableItems [2] = playerInformation.rightHandAttachPoint.transform.Find ("Pistol").gameObject; 
		usableItems [3] = playerInformation.rightHandAttachPoint.transform.Find ("Screwdriver").gameObject; 
		usableItems [4] = playerInformation.rightHandAttachPoint.transform.Find ("Shiv").gameObject; 
		usableItems [5] = playerInformation.rightHandAttachPoint.transform.Find ("Makeshift Wire Cutters").gameObject; 
		usableItems [6] = playerInformation.rightHandAttachPoint.transform.Find ("Makeshift Hammer").gameObject; 
		usableItems [7] = playerInformation.rightHandAttachPoint.transform.Find ("Light Emitter").gameObject;
		usableItems [8] = playerInformation.rightHandAttachPoint.transform.Find ("Explosive").gameObject; 
		usableItems [9] = playerInformation.rightHandAttachPoint.transform.Find ("Lock Pick").gameObject;
		usableItems [10] = playerInformation.rightHandAttachPoint.transform.Find ("Fortified Hammer").gameObject;
		usableItems [11] = playerInformation.rightHandAttachPoint.transform.Find ("Fortified Wire Cutters").gameObject;
		usableItems [12] = playerInformation.rightHandAttachPoint.transform.Find ("Syringe").gameObject;

		usableItems[0].SetActive (false);
		usableItems[1].SetActive (false);
		usableItems[2].SetActive (false);
		usableItems[3].SetActive (false);
		usableItems[4].SetActive (false);
		usableItems[5].SetActive (false);
		usableItems[6].SetActive (false);
		usableItems[7].SetActive (false);
		usableItems[8].SetActive (false);
		usableItems[9].SetActive (false);
		usableItems[10].SetActive (false);
		usableItems[11].SetActive (false);
		usableItems[12].SetActive (false);

		Keys[0] = transform.GetChild(7).gameObject; // utility
		Keys[1] = transform.GetChild(8).gameObject; // security
		Keys[2] = transform.GetChild(9).gameObject; // warden
	}
	void Update () 
	{
		//if(Input.GetKeyDown("o"))
		//Keys[1].transform.GetChild(0).GetComponent<Image>().enabled = true;

		if(Input.GetKeyUp("c"))
		{
			prisonerNames.Clear();

			prisoners = GameObject.FindGameObjectsWithTag("Prisoner");

			for (int i = 0; i < prisoners.Length; ++i)
			{
				prisonerNames.Add(prisoners[i].GetComponent<Prisoner_Information>().fullName);
			}
		}
		if (draggingItem) // makes drgging icon
		{
			droppedIcon.GetComponent<RectTransform>().position = new Vector2(Input.mousePosition.x + 20, Input.mousePosition.y - 20); // moves icon below and to the right by a bit
			//Vector2 center = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2); // gets mouse position by subtracting screen size and width
			//droppedIcon.GetComponent<RectTransform>().localPosition = new Vector2(center.x + 0, center.y - 0); // moves icon below and to the right by a bit
			//itemName = DraggedItem.itemName;
		}
		if (jailBrain.hacksOn)
		{
			if (Input.GetKeyDown("g")) 
			{
				addItem(Random.Range(0,15));
			}
		}
		if (Input.GetKeyDown("q")) 
		{
			SwitchHands();
		}
		if (draggingItem && this.transform.parent.GetComponent<Canvas>().enabled == false) // drops dragged item if "inven" closes
		{
			dropDragged();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
		{
			dog();
		}
		if(Input.GetKeyUp("z"))
		{
			//GameData._gameData.SaveData();
		}
		if(Input.GetKeyUp("x"))
		{
			//GameData._gameData.LoadData();
		}
	}
	/*
	public bool ItemCheck(int id)
	{
		bool foundItem = false;

		for(int i = 0; i < Slots.Count; ++i)
		{
			if(Items[i].itemID == id)
			{
				Items[i].itemID = id
			}
		}
		if (foundItem)
			return true;
		else
			return false;
	}*/
	public bool MetalCheck ()
	{
		bool foundMetal = false;

		for (int i = 0; i < Slots.Count - 1; ++i)
		{
			foundMetal = Slots[i].GetComponent<Slot>().MetalCheck();

			if (foundMetal)
			{
				break;
			}
		}
		if (foundMetal)
			return true;
		else
			return false;
	}
	public void addItem(int id) // add item to inventory by number
	{
		for (int i = 0; i < database.items.Count; i++)
		{
			//if(database.items[i].itemID == id && Slots[0].GetComponentInChildren<Image>().enabled == false)
			/* if(database.items[i].itemType == ItemClass.ItemType.HandsOnly && Slots[0].GetComponent<Slot>().childImage.enabled == false)
			{
				Debug.Log("HO");
			} */
			if(database.items[i].itemID == id)
			{
				ItemClass item = database.items[i];
				addItemInEmpty(item);
				break;
			}
		}
	}
	void addItemInEmpty(ItemClass item) // number designates which item to place
	{
		for (int i = 0; i < Items.Count; i++) 
		{
			//if(Items[i].itemID == 39)
			//{
			//	TurnOnKeys(1);
			//	break;
			//}
			if(Items[i].itemName == null)
			{
				Items[i] = item;
				++slotsFilled;
				TurnOnItem();
				NewCraft(); // NEW CRAFT TEST
				break;
			}
		}
	}
	public void addExistingItem(ItemClass item)
	{
		//if(Slots[0]) // if the hand slot
		//{
		//print ("hand slots too");
		//}
		if(item.itemID == 39)
			TurnOnKeys(0);
		else if(item.itemID == 40)
			TurnOnKeys(1);
		else if(item.itemID == 41)
			TurnOnKeys(2);
		else
			addItemInEmpty (item);
	}
	public void TurnOnItem() // turns off items then turns on items in hand
	{
		for (int i = 0; i < usableItems.Length; ++i) 
		{
			if (usableItems [i].activeSelf == true)
			{
				usableItems[i].SetActive(false);
			}
		}
		if (Items [4].itemName != null) 
		{
			if (Items [4].itemID == 4) // knife
			{
				usableItems[0].SetActive (true);
			}
			else if (Items [4].itemID == 1) //screwdriver
			{
				usableItems[3].SetActive (true);
			}	
			else if (Items [4].itemID == 0) //pistol
			{
				usableItems[2].SetActive (true);
			}	
			else if (Items [4].itemID == 21) //screw sharp
			{
				usableItems[1].SetActive (true);
			}
			else if (Items [4].itemID == 20) //shiv
			{
				usableItems[4].SetActive (true);
			}
			else if (Items [4].itemID == 5) //MakeShift Wire Cutters
			{
				usableItems[5].SetActive (true);
			}
			else if (Items [4].itemID == 14) //Crafted Hammer
			{
				usableItems[6].SetActive (true);
				usableItems[6].SendMessage("OffCooldown", this.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			else if (Items [4].itemID == 37) //Crafted Lantern 
			{
				usableItems[7].SetActive (true);
				//usableItems[7].SendMessage("TurnOff", this.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			else if (Items [4].itemID == 24) //Explosive
			{
				usableItems[8].SetActive (true);
				usableItems[8].SendMessage("StartFresh", this.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			else if (Items [4].itemID == 46) //Lock Pick
			{
				usableItems[9].SetActive (true);
			}
			else if (Items [4].itemID == 44) //Fortified Hammer
			{
				usableItems[10].SetActive (true);
			}
			else if (Items [4].itemID == 43) //Fortified Wire Cutters
			{
				usableItems[11].SetActive (true);
			}
			else if (Items [4].itemID == 8) //Syringe
			{
				usableItems[12].SetActive (true);
			}
		}
	}
	public void showDraggedItem(ItemClass item, int slotnumber) // is accessed in the slot script
	{
		indexOfDraggedItem = slotnumber;
		closeTooltip ();
		droppedIcon.SetActive (true);
		draggingItem = true;
		DraggedItem = item;
		droppedIcon.GetComponent<Image>().sprite = item.itemIcon;

		slotsFilled --;
	}
	public void DragNonSlot(ItemClass item) //
	{
		closeTooltip (); // turn off tool tip
		droppedIcon.SetActive (true);
		draggingItem = true; // you are now dragging
		DraggedItem = item;
		droppedIcon.GetComponent<Image>().sprite = item.itemIcon;
	}
	public void closeDraggedItem()
	{
		draggingItem = false;
		droppedIcon.SetActive (false);
	}
	public void showTooltip(Vector3 toolPosition, ItemClass item) //turns on and off the tool tip
	{
		toolTip.SetActive(true); // show tool tip
		toolTip.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x - 75, Input.mousePosition.y - 80, toolPosition.z); // NEW
		//Vector2 center = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2); // gets mouse position by subtracting screen size and width
		//toolTip.GetComponent<RectTransform>().position = new Vector3(center.x + 550, center.y + 250, toolPosition.z); // NEW
		//toolTip.GetComponent<RectTransform>().localPosition = new Vector3 (center.x - 50, center.y - 90, toolPosition.z); // move tool tip left and down
		// diplays the item information
		toolTip.transform.GetChild (0).GetComponent<Text> ().text = item.itemName; 
		toolTip.transform.GetChild (1).GetComponent<Text> ().text = "Contraband Level: " + item.contrabandLevel.ToString();
		toolTip.transform.GetChild (2).GetComponent<Text> ().text = item.description;

		if(item.activeItem == true) // if it has condition then show it
		{
			toolTip.transform.GetChild (3).GetComponent<Text> ().text = "Condition = " + item.durability.ToString() + "%";
		}
		else
			toolTip.transform.GetChild (3).GetComponent<Text> ().text = " ";
	}
	public void CraftToolTip(Vector3 toolPosition, ItemClass item)
	{
		craftTip.SetActive(true);
		Vector2 center = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2); // gets mouse position by subtracting screen size and width
		craftTip.GetComponent<RectTransform>().localPosition = new Vector3 (center.x + 115, center.y - 80, toolPosition.z);
		// diplays the item information
		craftTip.transform.GetChild (0).GetComponent<Text> ().text = item.itemName; 
		craftTip.transform.GetChild (1).GetComponent<Text> ().text = " ";
	}
	public void closeTooltip()
	{
		toolTip.SetActive(false);
		//craftTip.SetActive(false);
	}
	void dropDragged()
	{
		if(draggingItem && this.transform.parent.GetComponent<Canvas>().enabled == false) // drops dragged item if "inven" closes
		{
			dropBox.dropItem(DraggedItem);
			closeDraggedItem();
			playerInformation.DroppedItem (DraggedItem.contrabandLevel);
			TurnOnItem();
		}
	}
	void dog() // switch active hand item and slot# item
	{
		if(Input.GetKeyDown(KeyCode.Alpha2)) // switch hand items
		{
			ItemClass leftHand = Items[4]; // saves the item in slot 0
			ItemClass rightHand =Items[5];
			Items[4] = rightHand; // assigns nw slot 0
			Items[5] = leftHand; // assigns new slot 1
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3)) // switch hands to 1st pocket
		{
			ItemClass leftHand = Items[4]; // saves the item in slot 0
			ItemClass rightHand =Items[0];
			Items[4] = rightHand; // assigns nw slot 0
			Items[0] = leftHand; // assigns new slot 1
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4)) // 2nd pocket
		{
			ItemClass leftHand = Items[4]; // saves the item in slot 0
			ItemClass rightHand =Items[1];
			Items[4] = rightHand; // assigns nw slot 0
			Items[1] = leftHand; // assigns new slot 1
		}
		else if(Input.GetKeyDown(KeyCode.Alpha5)) // 3rd pocket
		{
			ItemClass leftHand = Items[4]; // saves the item in slot 0
			ItemClass rightHand =Items[2];
			Items[4] = rightHand; // assigns nw slot 0
			Items[2] = leftHand; // assigns new slot 1
		}
		TurnOnItem(); // turns on new item
	}
	public void SwitchHands()
	{
		List<ItemClass> tempItems = new List<ItemClass>(); // create list to add all active items
		List<int> tempSlots = new List<int>();

		for(int i = 0; i < 5; ++i)
		{
			if(Items[i].activeItem == true)
			{
				tempItems.Add(Items[i]);
				tempSlots.Add(i);
				//tempItems.addItemInEmpty(Items[i]);
			}
		}
		for(int k = 0; k < tempSlots.Count; ++k)
		{
			if (k < (tempSlots.Count - 1))
			{
				Items[tempSlots[k]] = tempItems[(k + 1)];
			}
			else if(k == (tempSlots.Count - 1))
			{
				Items[tempSlots[k]] = tempItems[4];
			}
		}
		TurnOnItem();
		tempItems.Clear();
		tempSlots.Clear();

		/* ItemClass leftHand = Items[0]; // saves the item in slot 0
		ItemClass rightHand =Items[1]; // saves item in slot 1
		Items[0] = rightHand; // assigns nw slot 0
		Items[1] = leftHand; // assigns new slot 1
		leftHand = new ItemClass(); // reset temp variable to nothing
		rightHand = new ItemClass();
		TurnOnItem(); // turns on new item */
	}
	public void SetInventory(int slot, int id)
	{
		ItemClass item = database.items[id];

		Items[slot] = item;
		++slotsFilled;
		TurnOnItem();
	}
	public void NewCraft()
	{
		itemID.Clear(); //clears the list before adding them again so there arent duplicates
		possibleItems.Clear();
        ClearQuickCraft();
        QuickCraft();

		for(int i = 0; i < Slots.Count; ++i) // check the items in the six slots
		{
			if(Items[i].itemType != ItemClass.ItemType.None) // if the slot is not empty
			{
				itemID.Add(Items[i].itemID); // add items to list for ingredients
			}
		}
		for(int k = 0; k < bluedataBase.blueprints.Count; ++k)
		{
			int amountOfTrue = 0; //amount of items that can be made

			for(int z = 0; z < bluedataBase.blueprints[k].ingredients.Count; ++z) // go thru ingredients of blueprint
			{
				for(int d = 0; d < itemID.Count; ++d) // go thru itemID
				{
					if(bluedataBase.blueprints[k].ingredients[z] == itemID[d]) //when ingredients ID and item ID are equal
					{
						amountOfTrue++;
						break;
					}
				}
				if(amountOfTrue == bluedataBase.blueprints[k].ingredients.Count) //create item
				{
					possibleItems.Add(bluedataBase.blueprints[k].finalItem); // add end result to list
                    QuickCraft();
				}
			}
		}
		// turn on all shadows
		for(int i = 0; i < 54; ++i) //53
			//for (int i = 0; i < 48; ++i) //old one
		{
			//print(craftIcons.transform.GetChild(19).name); //72
			craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = true;
		}
		// where shadow will be for icons // 18 is up to DIVIDER
		for(int i = 0; i < 54; ++i)
		{
			if(craftIcons.transform.GetChild(i+19).name == Items[0].itemName)
			{
				craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
			else if(craftIcons.transform.GetChild(i+19).name == Items[1].itemName)
			{
				craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
			else if(craftIcons.transform.GetChild(i+19).name == Items[2].itemName)
			{
				craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
			else if(craftIcons.transform.GetChild(i+19).name == Items[3].itemName)
			{
				craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
			else if(craftIcons.transform.GetChild(i+19).name == Items[4].itemName)
			{
				craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
			else if(craftIcons.transform.GetChild(i+19).name == Items[5].itemName)
			{
				craftIcons.transform.GetChild(i+19).transform.GetChild(0).GetComponent<Image>().enabled = false;
			}
		}
	}
    public void QuickCraft() // 13 child start, key text isn't being used so it might be 12
    {
        if (possibleItems.Count > 0)
        {
            for (int i = 0; i < possibleItems.Count; ++i)
            {
                craftSlots[i].GetComponent<QCraft>().item = possibleItems[i];
                craftSlots[i].GetComponent<QCraft>().ChangeIcon();
                //craftSlots[i].GetComponent<QCraft>().item = qCraft[i];
                //print(craftSlots[i].name);
                //print("bad change");
            }
        }
        else
        {
            for (int i = 0; i < craftSlots.Length; ++i)
            {
                //print("good change");
                craftSlots[i].GetComponent<QCraft>().ChangeIcon();
            }
        }
    }
    void ClearQuickCraft() // clear the quick craft
    {
        for(int i = 0; i < craftSlots.Length; ++i)
        {
            craftSlots[i].GetComponent<QCraft>().item = new ItemClass();
        }
    }
	public void DeleteItemByID(int id) // used to give prisoners items
	{
		for(int i = 0; i < Slots.Count; ++i) // might need to do slots.count - 1
		{
			if(Items[i].itemID == id)
			{
				Items[i] = new ItemClass();
                // TODO add
                --slotsFilled;
				NewCraft();						                                     
				break;
			}
		}
	}
	public void DeleteItem(ItemClass item)
	{
		for(int i = 0; i < bluedataBase.blueprints.Count; i++)
		{
			if(bluedataBase.blueprints[i].finalItem.Equals(item))
			{
				for(int k = 0; k < bluedataBase.blueprints[i].ingredients.Count; ++k) // check where they are
				{
					for(int z = 0; z < Slots.Count; ++z)
					{
						if(Items[z].itemID == bluedataBase.blueprints[i].ingredients[k])
						{
							Items[z] = new ItemClass();
                            slotsFilled--; // added 9/11/18 to fix slots being overfilled when crafting
							NewCraft();						                                     
							break;
						}
					}
				}
			}
		}
	}
	public void TurnOnKeys(int key)
	{
		if(key == 0)
		{
			Keys[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
			playerInformation.hasUtilityKeyCard = true;
			print ("has utility true");
		}
		else if(key == 1)
		{
			Keys[1].transform.GetChild(0).GetComponent<Image>().enabled = true;
			playerInformation.hasStaffKeyCard = true;
			print ("has staff key true");
		}
		//		else if(key == 2)
		//		{
		//			Keys[2].transform.GetChild(0).GetComponent<Image>().enabled = true;
		//			hasKey3 = true;
		//		}
	}
}

