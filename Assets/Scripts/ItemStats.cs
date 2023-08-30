using UnityEngine;

public enum ItemRarity
{
	Junk = 0,
	Shoddy = 1,
	Basic = 2,
	Good = 3,
	Great = 4,
	Perfect = 5,
	Epic = 6,
	Legendary = 7,
	Mythical = 8,
	Godly = 9,
	Unique = 10
}

[CreateAssetMenu(fileName = "Item Stats", menuName = "ScriptableObjects/New Item Stats")]
public class ItemStats : ScriptableObject
{
	[SerializeField] private Sprite sprite;
	[SerializeField] private ItemRarity rarity = ItemRarity.Junk;
	[SerializeField] private int cost = 25;

	public Sprite Sprite { get => sprite; }
	public ItemRarity Rarity { get => rarity; }
	public int Cost { get => cost; }
}
