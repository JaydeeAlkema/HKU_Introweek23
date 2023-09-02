using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class DraggableItem : MonoBehaviour
	{
		[SerializeField] private PolygonCollider2D polygonCollider;
		[SerializeField] private Vector2 size = default;
		[SerializeField] private int cellCount = default;
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField, Expandable] private ItemStats itemStats = default;
		[Space]
		[SerializeField] private WeightedRandomList<ItemStats> ItemStatsToRollFrom = new();

		private Vector3 originalPosition;
		private Vector3 previousPosition;
		private Quaternion originalRotation;
		private Quaternion previousRotation;
		private Vector2 originalSize;
		private Vector2 previousSize;
		private readonly List<InventoryCell> occupiedCells = new();

		private void Start()
		{
			itemStats = ItemStatsToRollFrom.GetRandom();
			spriteRenderer.sprite = itemStats.Sprite;

			originalSize = size;
			originalPosition = transform.position;
			originalRotation = transform.rotation;
		}

		public void PickUp()
		{
			previousPosition = transform.position;
			previousRotation = transform.rotation;
			Debug.Log($"Picking Up {transform.name}");
		}
		public void Drag()
		{
			Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePosition.z = previousPosition.z;
			transform.position = new Vector3(Mathf.Round(_mousePosition.x * 2) / 2, Mathf.Round(_mousePosition.y * 2) / 2, 0);
			Debug.Log($"Dragging Up {transform.name}");
		}
		public void Place()
		{
			FindAllOverlappingCellsAndPlaceItemInCenter();
			Debug.Log($"Placing {transform.name}");
		}
		public void Reset()
		{
			transform.SetPositionAndRotation(originalPosition, originalRotation);
			size = originalSize;
			ClearOccupiedCells();
			Debug.Log($"Reset {transform.name}");
		}

		public void Rotate(bool right)
		{
			if (right)
				transform.Rotate(0, 0, transform.rotation.y - 90);
			else
				transform.Rotate(0, 0, transform.rotation.y + 90);

			Vector2 newSize = new(size.y, size.x);
			size = newSize;
		}
		private void FindAllOverlappingCellsAndPlaceItemInCenter()
		{
			Bounds combinedCellsBounds;
			ContactFilter2D contactFilter = new()
			{
				layerMask = LayerMask.GetMask("Cell")
			};

			List<Collider2D> colliders = new();
			Physics2D.OverlapCollider(polygonCollider, contactFilter, colliders);

			if (colliders.Count < cellCount)
			{
				transform.SetPositionAndRotation(previousPosition, previousRotation);
				size = previousSize;
				return;
			}

			foreach (Collider2D collider in colliders)
			{
				collider.TryGetComponent(out InventoryCell inventoryCell);

				if (!inventoryCell) continue;

				if (inventoryCell.Occupied && !occupiedCells.Contains(inventoryCell))
				{
					transform.SetPositionAndRotation(previousPosition, previousRotation);
					size = previousSize;
					return;
				}
			}

			previousSize = size;
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
			Gizmos.color = Color.green; // Set the Gizmos color
			Vector2[] points = polygonCollider.GetPath(0); // Get the points of the polygon

			// Draw lines between consecutive points to visualize the edges of the polygon
			for (int i = 0; i < points.Length; i++)
			{
				Vector2 startPoint = transform.TransformPoint(points[i]);
				Vector2 endPoint = transform.TransformPoint(points[(i + 1) % points.Length]);
				Gizmos.DrawLine(startPoint, endPoint);
			}

			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position, size);
		}
	}
}
