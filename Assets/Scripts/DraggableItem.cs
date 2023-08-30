using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts
{
	public class DraggableItem : MonoBehaviour
	{
		[SerializeField] private PolygonCollider2D polygonCollider;
		[SerializeField] private Vector2 size = default;

		private Vector3 originalPosition;
		private Quaternion originalRotation;
		private List<InventoryCell> occupiedCells = new List<InventoryCell>();

		private void Start()
		{
			originalPosition = transform.position;
			originalRotation = transform.rotation;
		}

		public void PickUp()
		{
			originalPosition = transform.position;
			originalRotation = transform.rotation;
			Debug.Log($"Picking Up {transform.name}");
		}

		public void Drag()
		{
			Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePosition.z = originalPosition.z;
			transform.position = new Vector3(Mathf.Round(_mousePosition.x * 2) / 2, Mathf.Round(_mousePosition.y * 2) / 2, 0);
			Debug.Log($"Dragging Up {transform.name}");
		}

		public void Place()
		{
			FindAllOverlappingCellsAndPlaceItemInCenter(transform.position);
			Debug.Log($"Placing {transform.name}");
		}

		public void Rotate(bool right)
		{
			if (right)
				transform.Rotate(0, 0, transform.rotation.y + 90);
			else
				transform.Rotate(0, 0, transform.rotation.y - 90);

			Vector2 newSize = new Vector2(size.y, size.x);
			size = newSize;
		}

		private void FindAllOverlappingCellsAndPlaceItemInCenter(Vector2 position)
		{
			Bounds combinedCellsBounds;
			Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size * 0.95f, 0f, LayerMask.GetMask("Cell"));

			foreach (Collider2D collider in colliders)
			{
				collider.TryGetComponent(out InventoryCell inventoryCell);

				if (inventoryCell.Occupied && !occupiedCells.Contains(inventoryCell))
				{
					transform.position = originalPosition;
					transform.rotation = originalRotation;
					return;
				}
			}

			ClearOccupiedCells();
			foreach (Collider2D collider in colliders)
			{
				collider.TryGetComponent(out InventoryCell inventoryCell);
				inventoryCell.SetOccupied();
				occupiedCells.Add(inventoryCell);
			}

			combinedCellsBounds = occupiedCells[0].GetComponent<BoxCollider2D>().bounds;
			for (int i = 1; i < occupiedCells.Count; i++)
			{
				InventoryCell inventoryCell = occupiedCells[i];
				combinedCellsBounds.Encapsulate(inventoryCell.GetComponent<BoxCollider2D>().bounds);
			}

			transform.position = combinedCellsBounds.center;
		}

		private void ClearOccupiedCells()
		{
			if (occupiedCells.Count == 0) return;
			foreach (InventoryCell cell in occupiedCells)
			{
				cell.ClearCell();
			}
			occupiedCells.Clear();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(transform.position, size);
		}
	}
}
