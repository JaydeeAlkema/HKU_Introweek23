using TMPro;
using UnityEngine;

public class ItemStatsOverlay : MonoBehaviour
{
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
		_itemTitleTextElement.text = itemStats.Title;
		_itemDescriptionTextElement.text = itemStats.Description;
		_itemCostTextElement.text = itemStats.Cost.ToString();
	}
}
