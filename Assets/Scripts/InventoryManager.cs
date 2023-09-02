using UnityEngine;

namespace Assets.Scripts
{
	public class InventoryManager : MonoBehaviour
	{
		private DraggableItem currentDraggableItem;
		private Collider2D itemCollider;
		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				itemCollider = RaycastForItem().collider;
				if (itemCollider != null)
				{
					currentDraggableItem = itemCollider.gameObject.GetComponent<DraggableItem>();
					currentDraggableItem.Reset();
					currentDraggableItem = null;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				itemCollider = RaycastForItem().collider;
				if (itemCollider != null)
				{
					currentDraggableItem = itemCollider.gameObject.GetComponent<DraggableItem>();
					currentDraggableItem.PickUp();
				}
			}
			else if (Input.GetMouseButtonUp(0) && currentDraggableItem != null)
			{
				currentDraggableItem.Place();
				currentDraggableItem = null;
			}

			if (currentDraggableItem != null)
			{
				currentDraggableItem.Drag();
			}

			if (currentDraggableItem && Input.GetKeyDown(KeyCode.Q))
			{
				currentDraggableItem.Rotate(false);
			}
			else if (currentDraggableItem && Input.GetKeyDown(KeyCode.E))
			{
				currentDraggableItem.Rotate(true);
			}
		}

		private static RaycastHit2D RaycastForItem()
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Item"));
			return hit;
		}
	}
}