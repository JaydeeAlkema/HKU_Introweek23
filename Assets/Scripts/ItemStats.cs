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
	[SerializeField] private Sprite _sprite;
	[SerializeField] private ItemRarity _rarity = ItemRarity.Junk;
	[SerializeField] private string title = default;
	[SerializeField] private string description = default;
	[SerializeField] private int _cost = 25;

	public Sprite Sprite { get => _sprite; }
	public ItemRarity Rarity { get => _rarity; }
	public string Title { get => title; }
	public string Description { get => description; }
	public int Cost { get => _cost; }
}
