using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour 
{
	CursorLockMode inInventory;
	
	public List<ItemClass> items = new List<ItemClass>();
	public Canvas InventoryPanel;
	public GameObject toolTip; //description pop up menu
	static int totalItemCount = 48;
	
	private int[] ItemID = new int[totalItemCount];
	public int[] getItemID
	{
		get
		{
			return ItemID;
		}
	}
	private string[] itemDescription = new string[totalItemCount];
	public string[] getItemDescription
	{
		get
		{
			return itemDescription;
		}
	}
	private int[] itemContraband = new int[totalItemCount];
	public int[] getContrband
	{
		get
		{
			return itemContraband;
		}
	}
	private ItemClass.ItemType[] itemType = new ItemClass.ItemType[totalItemCount];
	public ItemClass.ItemType[] getItemType
	{
		get
		{
			return itemType;
		}
	}
	/// <summary>
	/// Class used to define all possible items in the game
	/// </summary>
	
	void Awake()
	{
		InventoryPanel = GameObject.Find ("JournalCanvas").GetComponent<Canvas>();
		
		toolTip = GameObject.FindGameObjectWithTag("Tooltip");
	}
	void Start ()
	{		
		//sets up possibe items         	// Weapons
		items.Add (new ItemClass("Pistol", ItemID[0] = 0, itemDescription [0] = "Shooty bang bang", itemContraband[0] = 9, itemType[0] = ItemClass.ItemType.Equipable, true, true));
		items.Add (new ItemClass("Screwdriver", ItemID[1] = 1, itemDescription [1] = "A normal screwdriver" , itemContraband[1] = 5, itemType[1] = ItemClass.ItemType.Equipable, true, true));
		items.Add (new ItemClass("Battery", ItemID[2] = 2, itemDescription [2] = "Used to power electronic items", itemContraband[2] = 0, itemType[2] = ItemClass.ItemType.SmallItem, false, true)); // *
		items.Add (new ItemClass("Stick", ItemID[3] = 3, itemDescription [3] = "A stick from a tree", itemContraband[3] = 2, itemType[3] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Kitchen Knife", ItemID[4] = 4, itemDescription [4] = "A kitchen knife", itemContraband[4] = 7, itemType[4] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Makeshift Wire Cutters", ItemID[5] = 5, itemDescription [5] = "Used to cut wire(s)", itemContraband[5] = 8, itemType[5] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Fork", ItemID[6] = 6, itemDescription [6] = "Generic fork", itemContraband[6] = 0, itemType[6] = ItemClass.ItemType.SmallItem, true, true)); // *
		items.Add(new ItemClass("Pen", ItemID[7] = 7, itemDescription [7] = "Generic Pen, *craftable", itemContraband[7] = 1, itemType[7] = ItemClass.ItemType.SmallItem, false, false)); // *
		// Consumables
		items.Add(new ItemClass("Syringe", ItemID[8] = 8, itemDescription [8] = "Knocks People Out", itemContraband[8] = 4, itemType[8] =  ItemClass.ItemType.Equipable, true, false));
		items.Add(new ItemClass("Apple", ItemID[9] = 9, itemDescription [9] = "Restore small energy", itemContraband[9] = 0, itemType[9] = ItemClass.ItemType.Consumable, false, false));
		items.Add(new ItemClass("Banana", ItemID[10] = 10, itemDescription [10] = "Restore small energy", itemContraband[10] = 0, itemType[10] = ItemClass.ItemType.Consumable, false, false));
		items.Add(new ItemClass("Chicken Wing", ItemID[11] = 11, itemDescription [11] = "Restore medium energy", itemContraband[11] = 3, itemType[11] = ItemClass.ItemType.Consumable, false, false));
		items.Add(new ItemClass("Bottle of Alcohol", ItemID[12] = 12, itemDescription [12] = "Quest Item", itemContraband[12] = 4, itemType[12] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Pack of Cigarettes", ItemID[13] = 13, itemDescription [13] = "Quest Item", itemContraband[13] = 3, itemType[13] = ItemClass.ItemType.SmallItem, false, false));
		items.Add(new ItemClass("Makeshift Hammer",  ItemID[14] =  14, itemDescription [14] =  "Hammer things", itemContraband[14] = 8, itemType[14] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Tape",  ItemID[15] =  15, itemDescription [15] =  "sticky tape", itemContraband[15] = 0, itemType[15] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Wirecoil",  ItemID[16] =  16, itemDescription [16] =  "A coil of wire", itemContraband[16] = 0, itemType[16] = ItemClass.ItemType.Equipable, false, true));   	 
		items.Add(new ItemClass("File",  ItemID[17] =  17, itemDescription [17] =  "A file for filing", itemContraband[17] = 5, itemType[17] = ItemClass.ItemType.SmallItem, false, true)); // *
		items.Add(new ItemClass("Prisoner Clothing (dirty)",  ItemID[18] =  18, itemDescription [18] =  "dirty prisoner clothes", itemContraband[18] = 0, itemType[18] = ItemClass.ItemType.Equipable, false, false));   	 
		items.Add(new ItemClass("Prisoner Clothing (clean)",  ItemID[19] =  19, itemDescription [19] =  "clean prisoner clothes", itemContraband[19] = 0, itemType[19] = ItemClass.ItemType.Equipable, false, false));    
		//set up craftable items
		items.Add(new ItemClass("Shiv",  ItemID[20] = 20, itemDescription [14] = "A small shiv made from a small knife", itemContraband[20] = 7, itemType[20] = ItemClass.ItemType.SmallItem, true, true)); // *
		items.Add(new ItemClass("Screwdriver (sharp)",  ItemID[21] = 21, itemDescription [21] =  "A sharpened screwdriver", itemContraband[21] = 7, itemType[21] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Sharpened Metal",  ItemID[22] = 22, itemDescription [22] =  "A sharpened piece of metal", itemContraband[22] = 4, itemType[22] = ItemClass.ItemType.Equipable, false, true));
		items.Add(new ItemClass("Scrap Metal",  ItemID[23] = 23, itemDescription [23] =  "scrap metal", itemContraband[23] = 4, itemType[23] = ItemClass.ItemType.Equipable, false, true));
		items.Add(new ItemClass("Explosive",  ItemID[24] = 24, itemDescription [24] =  "A small explosive charge", itemContraband[24] = 9, itemType[24] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Bandage",  ItemID[25] = 25, itemDescription [25] =  "A crafted bandage from paper", itemContraband[25] = 0, itemType[25] = ItemClass.ItemType.Consumable, false, false));    
		items.Add(new ItemClass("Medical Bandage",  ItemID[26] = 26, itemDescription [26] =  "A medical grade bandage", itemContraband[26] = 0, itemType[26] = ItemClass.ItemType.Consumable, false, false));    
		items.Add(new ItemClass("Gun Powder",  ItemID[27] = 27, itemDescription [25] =  "A crafted bandage from paper", itemContraband[27] = 9, itemType[27] = ItemClass.ItemType.Equipable, false, false));    
		items.Add(new ItemClass("Glue",  ItemID[28] = 28, itemDescription [28] =  "Adheasive", itemContraband[28] = 0, itemType[28] = ItemClass.ItemType.Equipable, false, false));    
		items.Add(new ItemClass("Guest Pass",  ItemID[29] = 29, itemDescription [29] =  "Pass used to enter and exit prison", itemContraband[29] = 10, itemType[29] = ItemClass.ItemType.SmallItem, false, false)); // *   
		items.Add(new ItemClass("Crafted Paper",  ItemID[30] = 30, itemDescription [30] =  "Paper made from toilet paper", itemContraband[30] = 0, itemType[30] = ItemClass.ItemType.SmallItem, false, false)); // *
		items.Add(new ItemClass("Toilet Paper",  ItemID[31] = 31, itemDescription [31] =  "A standard roll of toilet paper", itemContraband[31] = 0, itemType[31] = ItemClass.ItemType.SmallItem, false, false));
		items.Add(new ItemClass("Makeshift Axe",  ItemID[32] = 32, itemDescription [32] =  "A shiv put on a stick", itemContraband[32] = 8, itemType[32] = ItemClass.ItemType.Equipable, true, false));
		items.Add(new ItemClass("Electric Component",  ItemID[33] = 33, itemDescription [33] =  "A small electrical component", itemContraband[33] = 4, itemType[33] = ItemClass.ItemType.Equipable, false, true));
		items.Add(new ItemClass("Handle",  ItemID[34] = 34, itemDescription [34] =  "A reinforced wooden handle", itemContraband[34] = 5, itemType[34] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Signal Jammer",  ItemID[35] = 35, itemDescription [35] =  "Used to jam cameras", itemContraband[35] = 9, itemType[35] = ItemClass.ItemType.Equipable, false, true));
		items.Add(new ItemClass("Wardens Seal",  ItemID[36] = 36, itemDescription [36] =  "Official seal of the warden", itemContraband[36] = 9, itemType[36] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Light Emitter", ItemID [37] = 37, itemDescription [37] = "Used to see in dark areas", itemContraband [37] = 3, itemType[37] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Library Key",  ItemID[38] = 38, itemDescription [38] =  "A key that opens the library", itemContraband[38] = 7, itemType[38] = ItemClass.ItemType.SmallItem, false, true));
		items.Add(new ItemClass("KeyCard Utility",  ItemID[39] = 39, itemDescription [39] =  "Used on red keycard readers", itemContraband[39] = 9, itemType[39] = ItemClass.ItemType.SmallItem, false, false));
		items.Add(new ItemClass("KeyCard Staff",  ItemID[40] = 40, itemDescription [40] =  "Used on blue keycard readers", itemContraband[40] = 9, itemType[40] = ItemClass.ItemType.SmallItem, false, false));
		items.Add(new ItemClass("Key Card (green)",  ItemID[41] = 41, itemDescription [41] =  "Used on green keycard readers", itemContraband[41] = 9, itemType[41] = ItemClass.ItemType.SmallItem, false, false));
		items.Add(new ItemClass("Stone Block",  ItemID[42] = 42, itemDescription [42] =  "Large stone block", itemContraband[42] = 0, itemType[42] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Fortified Wire Cutters",  ItemID[43] = 43, itemDescription [43] =  "Strengthened wire cutters", itemContraband[43] = 8, itemType[43] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Fortified Hammer", ItemID [44] = 44, itemDescription [44] = "Strengthened hammer", itemContraband [44] = 8, itemType [44] = ItemClass.ItemType.Equipable, true, true));
		items.Add(new ItemClass("Poster",  ItemID[45] = 45, itemDescription [45] =  "A folded poster", itemContraband[45] = 0, itemType[45] = ItemClass.ItemType.Equipable, false, false));
		items.Add(new ItemClass("Lock Pick", ItemID [46] = 46, itemDescription [46] = "Used To Open Locks", itemContraband [46] = 7, itemType[46] = ItemClass.ItemType.SmallItem, true, true));
		items.Add(new ItemClass("Book", ItemID [47] = 47, itemDescription [47] = "Take back to cell to read", itemContraband [47] = 7, itemType[47] = ItemClass.ItemType.Equipable, true, false));
	}
	public void InventoryOnOff()
	{
		if(InventoryPanel.enabled == true)
			InventoryPanel.enabled = false;
		else if(InventoryPanel.enabled == false)
			InventoryPanel.enabled = true;
	}
	public void ShowInventory()
	{
		InventoryPanel.enabled = true;
	}
	public void closeInventory()
	{
		print ("closing inv");
		//InventoryPanel.enabled = false;
	}
	public void OnlyInventoryOpen()
	{
		//InventoryPanel.enabled = true;
		//InventoryPanel.transform.GetChild (1).gameObject.SetActive(false);
		//InventoryPanel.transform.GetChild (2).gameObject.SetActive(false);
		//InventoryPanel.transform.GetChild (3).gameObject.SetActive(false);
	}
	public void ResetCanvas()
	{/*
		InventoryPanel.enabled = false;
		//Cursor.lockState = CursorLockMode.None;
		//Cursor.visible = (CursorLockMode.Locked == inInventory);
		InventoryPanel.transform.GetChild (1).gameObject.SetActive(true);
		InventoryPanel.transform.GetChild (2).gameObject.SetActive(true);
		InventoryPanel.transform.GetChild (3).gameObject.SetActive(true);
		*/
	}
	public void DropRandomItem(ItemClass item) // don't think its needed
	{
		GameObject newItem = Instantiate(item.itemModel, GameObject.FindGameObjectWithTag("Dropzone").transform.position, Quaternion.identity) as GameObject;
	}
}