using UnityEngine;

namespace Assets.Scripts
{
	public class InventoryManager : MonoBehaviour
	{
		[SerializeField] private ItemTooltip _itemTooltip;

		private DraggableItem _currentDraggableItem;
		private Vector3 _mousePosition;

		private void Update()
		{
			HandleTooltip();
			HandleInput();
		}

		private void HandleInput()
		{
			Collider2D _itemCollider;
			if (Input.GetMouseButtonDown(1))
			{
				_itemCollider = RaycastForItem().collider;
				if (_itemCollider != null)
				{
					_currentDraggableItem = _itemCollider.gameObject.GetComponent<DraggableItem>();
					_currentDraggableItem.Reset();
					_currentDraggableItem = null;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				_itemCollider = RaycastForItem().collider;
				if (_itemCollider != null)
				{
					_currentDraggableItem = _itemCollider.gameObject.GetComponent<DraggableItem>();
					_currentDraggableItem.PickUp();
				}
			}
			else if (Input.GetMouseButtonUp(0) && _currentDraggableItem != null)
			{
				_currentDraggableItem.Place();
				_currentDraggableItem = null;
			}

			if (_currentDraggableItem != null)
			{
				_currentDraggableItem.Drag();
			}

			if (_currentDraggableItem && Input.GetKeyDown(KeyCode.Q))
			{
				_currentDraggableItem.Rotate(false);
			}
			else if (_currentDraggableItem && Input.GetKeyDown(KeyCode.E))
			{
				_currentDraggableItem.Rotate(true);
			}
		}
		private void HandleTooltip()
		{
			Collider2D _itemCollider = RaycastForItem().collider;
			if (_itemCollider != null)
			{
				_itemTooltip.ToggleOverlay(true);
				_itemTooltip.SetPosition(_mousePosition);
				_itemTooltip.SetItemStats(_itemCollider.GetComponent<DraggableItem>().ItemStats);
			}
			else
			{
				_itemTooltip.ToggleOverlay(false);
			}
		}
		private RaycastHit2D RaycastForItem()
		{
			_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(_mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Item"));
			return hit;
		}
	}
}