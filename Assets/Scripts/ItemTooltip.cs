using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _itemTitleTextElement;
	[SerializeField] private TextMeshProUGUI _itemDescriptionTextElement;
	[SerializeField] private TextMeshProUGUI _itemCostTextElement;

	GameObject itemStatsOverlayCanvasGameObject = default;

	private void Awake()
	{
		itemStatsOverlayCanvasGameObject = gameObject.transform.GetChild(0).gameObject;
		ToggleOverlay(false);
	}

	public void ToggleOverlay(bool toggle)
	{
		itemStatsOverlayCanvasGameObject.SetActive(toggle);
	}

	public void SetItemStats(ItemStats itemStats)
	{
		_itemImage.sprite = itemStats.Sprite;
		_itemTitleTextElement.text = itemStats.Title;
		_itemDescriptionTextElement.text = itemStats.Description;
		_itemCostTextElement.text = $"Sell: {itemStats.Cost}";
	}

	public void SetPosition(Vector2 position)
	{
		transform.position = position;
	}
}
