using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftSystem : MonoBehaviour 
{
	List<CraftSlot> slots = new List<CraftSlot>();
	public List<int> itemID = new List<int>();
	BlueprintDatabase dataBase;
	public List<ItemClass> possibleItems = new List<ItemClass>();

	//public List<ItemClass> Items = new List<ItemClass>(); // the new list which the slots will take
	//public List<GameObject> Slots = new List<GameObject>(); // define int of inventory slots

 	private GameObject leftArrow;
	private GameObject rightArrow;
	
	CraftResult result; //access the results script to display the item

	Inventory _inventroy;
	
	void Start () 
	{
		for(int i = 0; i < 4; i++) //goes thru and finds the crafting and results slots
		{
			slots.Add(transform.GetChild(i).GetComponent<CraftSlot>());
		}
		
		dataBase = GameObject.FindGameObjectWithTag("BlueprintDatabase").GetComponent<BlueprintDatabase>();
		result = GameObject.FindGameObjectWithTag("Result").GetComponent<CraftResult>();
		leftArrow = transform.GetChild(5).gameObject;
		rightArrow = transform.GetChild(6).gameObject;
		leftArrow.GetComponent<Text>().enabled = false;
		rightArrow.GetComponent<Text>().enabled = false;

		_inventroy = GameObject.Find("Inventory").GetComponent<Inventory>();
	}

	public void ListWithItems()
	{
		OffArrows();
		itemID.Clear(); //clears the list before adding them again so there arent duplicates
		possibleItems.Clear();
		result.temp = 0; // reset the results each time we add or remove items
		
		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[i].item.itemType != ItemClass.ItemType.None)
			{
				itemID.Add(slots[i].item.itemID); // performance wise = easier to compare integer instead of world object
			}
		}
		for(int k = 0; k < dataBase.blueprints.Count; ++k)
		{
			int amountOfTrue = 0; //amount of items that can be made
			
			for(int z = 0; z < dataBase.blueprints[k].ingredients.Count; ++z) // go thru ingredients of blueprint
			{
				for(int d = 0; d < itemID.Count; ++d) // go thru itemID
				{
					if(dataBase.blueprints[k].ingredients[z] == itemID[d]) //when ingredients ID and item ID are equal
					{
						amountOfTrue++;
						break;
					}
				}
				if(amountOfTrue == dataBase.blueprints[k].ingredients.Count) //create item
				{
					/*if(dataBase.blueprints[k] == dataBase.blueprints[6])		{
						int tempCheck = 0;
						for(int p = 0; p < slots.Count; ++p)			{
							if(slots[p].item.itemID == 30) // 				{
								++tempCheck;		}		}
						if(tempCheck == 2)		{
							possibleItems.Add(dataBase.blueprints[k].finalItem);
							possibleItems[0].durability = Random.Range(60, 91);		}		}*/
					if (dataBase.blueprints[k] == dataBase.blueprints[9])
					{
						int tempCheck = 0;
						for(int p = 0; p < slots.Count; ++p)
						{
							if(slots[p].item.itemID == 31) // 
							{
								++tempCheck;
							}
						}
						if(tempCheck == 2)
						{
							possibleItems.Add(dataBase.blueprints[k].finalItem);
							possibleItems[0].durability = Random.Range(60, 91);
						}
					}
					else if (dataBase.blueprints[k] == dataBase.blueprints[12])
					{
						int tempCheck = 0;
						for(int p = 0; p < slots.Count; ++p)
						{
							if(slots[p].item.itemID == 3) // 
							{
								++tempCheck;
							}
						}
						if(tempCheck == 2)
						{
							possibleItems.Add(dataBase.blueprints[k].finalItem);
							possibleItems[0].durability = Random.Range(60, 91);
						}
					}
					else if (dataBase.blueprints[k] == dataBase.blueprints[3])
					{
						int tempCheck = 0;
						for(int p = 0; p < slots.Count; ++p)
						{
							if(slots[p].item.itemID == 3 || slots[p].item.itemID == 22) // 
							{
								++tempCheck;
							}
						}
						if(tempCheck == 4)
						{
							possibleItems.Add(dataBase.blueprints[k].finalItem);
							possibleItems[0].durability = Random.Range(60, 91);
						}
					}
					/*else if (dataBase.blueprints[k] == dataBase.blueprints[18])	{
						int tempCheck = 0;
						for(int p = 0; p < slots.Count; ++p)	{
							if(slots[p].item.itemID == 30) //		{
								++tempCheck;	}		}
						if(tempCheck == 2)	{
							possibleItems.Add(dataBase.blueprints[k].finalItem);	}	}*/
					else
						possibleItems.Add(dataBase.blueprints[k].finalItem);
				}
			}
		}
		if(possibleItems.Count > 1)
		{
			rightArrow.GetComponent<Text>().enabled = true;
		}
	}
	public void DeleteItem(ItemClass item)
	{
		for(int i = 0; i < dataBase.blueprints.Count; i++)
		{
			if(dataBase.blueprints[i].finalItem.Equals(item))
			{
				for(int k = 0; k < dataBase.blueprints[i].ingredients.Count; k++)
				{
					for(int z = 0; z < slots.Count; z++)
					{
						if(slots[z].item.itemID == dataBase.blueprints[i].ingredients[k])
						{
							//_inventroy.Items[z] = new ItemClass();
							slots[z].item = new ItemClass(); // for old style
							ListWithItems();
							break;
						}
					}
				}
			}
		}
	}
	public void LeftArrow()
	{
		leftArrow.GetComponent<Text>().enabled = true;
		rightArrow.GetComponent<Text>().enabled = false;
	}
	public void RightArrow()
	{
		leftArrow.GetComponent<Text>().enabled = false;
		rightArrow.GetComponent<Text>().enabled = true;
	}
	void OffArrows()
	{
		rightArrow.GetComponent<Text>().enabled = false;
		leftArrow.GetComponent<Text>().enabled = false;
	}
}