using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour 
{

    #region Global variable Declaration

    private List<Item> database = new List<Item>();
	private JsonData itemData;

    #endregion

    void Awake()
	{

		#region creating an Item from json commented out

//		Item item = new Item (0, "Ball", 5);
//		database.Add (item);
//		Debug.Log(database[0].Title);

		#endregion

		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructItemDatabase ();
        
	}

	public Item FetchItemByID(int id)
	{
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
            {
                return database[i];
            }
        }
	
		return null;
	}
	void ConstructItemDatabase()
	{
		for(int i = 0; i < itemData.Count; i++)
		{
			database.Add (new Item((int)itemData[i]["id"], 
                itemData[i]["title"].ToString(), 
                (int)itemData[i]["value"],
				(int)itemData[i]["stats"]["attack_power"], 
                (int)itemData[i]["stats"]["defence_strength"], 
                (int)itemData[i]["stats"]["durability"], 
                itemData[i]["description"].ToString(),
                (bool)itemData[i]["stackable"],
				(bool)itemData[i]["destructable"], 
                (int)itemData[i]["rarity"], 
                itemData[i]["slug"].ToString()));
		}
	}

}

public class Item
{

    #region Global Variable Declaration

    public int ID { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public int Power{ get; set; }
	public int Defence{ get; set; }
	public int Duability { get; set; }
	public string Description{ get; set; }
	public bool Stackable{ get; set; }
    public bool Destructable { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }

    #endregion

    public Item(int id, string title, int value, int power, int defence, int durability, string description, bool stackable, bool destructable, int rarity, string slug)
	{
		this.ID = id;
		this.Title = title;
		this.Value = value;
		this.Power = power;
		this.Defence = defence;
		this.Duability = durability;
		this.Description = description;
		this.Stackable = stackable;
        this.Destructable = destructable;
		this.Rarity = rarity;
		this.Slug = slug;
		this.Sprite = Resources.Load <Sprite>("Sprites/Items/" + slug);
	}
	public Item(int id, string title, int value)
	{
		this.ID = id;
		this.Title = title;
		this.Value = value;
	}
	public Item()
	{
		this.ID = -1;
	}
}
