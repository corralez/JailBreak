using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour 
{
	public bool searched = false; // has it already been searched
	[HideInInspector]
	public DroppedItem insideItem;
	public bool offLimitsContainer = false;
	public  bool locked = false;
	
	public GameObject droppedItem; // prefab of the blank item
	[SerializeField]
	[Range(0, 8)]
	private int minNumber;
	[SerializeField]
	[Range(0, 8)]
	private int maxNumberOfItems;
	[SerializeField]
	private string[] possibleItems; // what can be spawned in container
	
	public bool shouldShuffle = true; // sets the default of shuffling to be on
	private Interact _interact;
	[SerializeField]
	private int totalSlots = 6; // total slots available
	public int usedSlots = 0; // slots actually available
	
	public int getTotalSlots
	{
		get	{ return totalSlots; }
	}
	
	void Start () 
	{
		_interact = GameObject.Find("Player").GetComponent<Interact>();
		Shuffle ();
	}
	public void CheckSlots()
	{
		//int temp = totalSlots;
		
		for (int i = 0; i < transform.childCount; i ++) //delete previous items
		{
			if(transform.GetChild(i).gameObject.tag == "ContainerItem")
			{
				this.usedSlots += 1;
			}
		}
	}
	public void Shuffle ()
	{
		if(this.shouldShuffle)
		{ 
			for (int i = 0; i < transform.childCount; i ++) //delete previous items
			{
				if(transform.GetChild(i).gameObject.tag == "ContainerItem")
				{
					Destroy(transform.GetChild(i).gameObject);
					usedSlots = 0;
				}
			}
			//for (int i = Random.Range(minNumber, maxNumberOfItems); i > 0; i--) //assign and create items in containers if their number of items allow it
			for (int i = 0; i < Random.Range(this.minNumber, (this.maxNumberOfItems + 1)); i++) // add random items
			{
				GameObject newItem = Instantiate(droppedItem, this.transform.position, Quaternion.identity) as GameObject;
				newItem.GetComponent<Collider>().enabled = false; // makes it so that the items collisons is off / won't interfere with parents collision
				newItem.GetComponent<Rigidbody>().isKinematic = true; // makes it so the item will stay in the middle, not fall
				newItem.transform.SetParent(this.gameObject.transform, true); // sets the parent to this container
				newItem.transform.SetSiblingIndex(0); // sets the index of the new ite m to zero so that non items will be pushed to the back
				newItem.GetComponent<MeshRenderer>().enabled = false; // makes the object invisible so not to stick out of anything
				if(droppedItem.transform.childCount > 0)
				{
					newItem.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					
					if(newItem.transform.childCount > 0) // check if the container has children
					{
						for(int k = 0; k < newItem.transform.childCount; ++k) // check the containers children
						{
							newItem.transform.GetChild(k).GetComponent<MeshRenderer>().enabled = false;
						}
					}
				}
				newItem.name = possibleItems[Random.Range(0, possibleItems.Length)];
				insideItem = newItem.GetComponent<DroppedItem>();
				usedSlots += 1;
			}
		}
	}
	// used for loading data
	public void AddItems(int itemID)
	{
		ItemDatabase database = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase> ();
		ItemClass item = database.items[itemID];
		
		GameObject newItem = Instantiate(droppedItem, this.transform.position, Quaternion.identity) as GameObject;
		newItem.GetComponent<DroppedItem>().item = item;
		newItem.GetComponent<Collider>().enabled = false; // makes it so that the items collisons is off / won't interfere with parents collision
		newItem.GetComponent<Rigidbody>().isKinematic = true; // makes it so the item will stay in the middle, not fall
		newItem.transform.SetParent(this.gameObject.transform, true); // sets the parent to this container
		newItem.transform.SetSiblingIndex(0); // sets the index of the new ite m to zero so that non items will be pushed to the back
		newItem.GetComponent<MeshRenderer>().enabled = false; // makes the object invisible so not to stick out of anything
		if(droppedItem.transform.childCount > 0)
		{
			newItem.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			
			if(newItem.transform.childCount > 0) // check if the container has children
			{
				for(int k = 0; k < newItem.transform.childCount; ++k) // check the containers children
				{
					newItem.transform.GetChild(k).GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
		newItem.name = item.itemName + "(clone)";
		usedSlots += 1;
	}
	public void resetBool()
	{
		this.searched = false;
	}
	public int ContrabandCheck()
	{
		int amountOfContraband = 0;
		
		for (int i = 0; i < this.transform.childCount; ++i) 
		{
			amountOfContraband += this.GetComponentsInChildren<DroppedItem>()[i].item.contrabandLevel;
		}
		Debug.Log ("amount of contraband found in cell search: " + amountOfContraband);
		return amountOfContraband;
	}
	public void DeleteContrabandItems() // used to delete contraband items when they are checked
	{
		for (int i = 0; i < transform.childCount; i ++) //delete previous items
		{
			if(transform.GetChild(i).gameObject.tag == "ContainerItem")
			{
				//used if you want to check a specific contraband level
				if(transform.GetChild(i).GetComponent<DroppedItem>().item.contrabandLevel >= 1)
				{
					Destroy(transform.GetChild(i).gameObject);
					this.usedSlots -= 1;
				}
			}
		}
	}
	public void DirtyClothes()
	{
		for(int i = 0; i < transform.childCount; ++i)
		{
			if(transform.GetChild(i).name == "Prisoner Clothing (dirty)(Clone)")
			{
				Destroy(transform.GetChild(i).gameObject);
				GameObject clothes = Instantiate (Resources.Load("Inventory/Prisoner Clothing (clean)"), this.transform.position, Quaternion.identity) as GameObject;
				clothes.name = "Prisoner Clothing (clean)";
				clothes.transform.parent = this.gameObject.transform;
				//clothes.transform.SetSiblingIndex (0); // creates a loop NO!!
				clothes.GetComponent<Collider>().enabled = false; // makes it so the item can be in the container
				clothes.GetComponent<Rigidbody>().isKinematic = true; // makes it so the item will not fall through the world
				clothes.GetComponent<MeshRenderer> ().enabled = false; // turn the renderer off the mesh s oit does'nt poke out
				clothes.tag = "ContainerItem"; // changes the name to be accessed by contianer
				clothes.GetComponent<DroppedItem>().setItem();
			}
		}
	}
	public void MustSpawn(string item)
	{
		if(usedSlots < totalSlots)
		{
			GameObject newItem = Instantiate(droppedItem, this.transform.position, Quaternion.identity) as GameObject; // create a new item in the container
			newItem.GetComponent<Collider>().enabled = false; // makes it so that the items collisons is off / won't interfere with parents collision
			newItem.GetComponent<Rigidbody>().isKinematic = true; // makes it so the item will stay in the middle, not fall
			newItem.transform.SetParent(this.gameObject.transform, true); // sets the parent to this container
			newItem.transform.SetSiblingIndex(0); // sets the index of the new ite m to zero so that non items will be pushed to the back
			newItem.GetComponent<MeshRenderer>().enabled = false; // makes the object invisible so not to stick out of anything
			
			if(droppedItem.transform.childCount > 0)
			{
				newItem.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false; // makes item invisible
				
				if(newItem.transform.childCount > 0) // check if the container has children
				{
					for(int k = 0; k < newItem.transform.childCount; ++k) // check the containers children
					{
						newItem.transform.GetChild(k).GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
			newItem.name = item; // set the name so the items will get properties
			insideItem = newItem.GetComponent<DroppedItem>();
			usedSlots += 1; // add 1 to slots
		}
		else
			print("not enough space");
	}
}