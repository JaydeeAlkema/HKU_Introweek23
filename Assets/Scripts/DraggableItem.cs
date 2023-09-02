using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class DraggableItem : MonoBehaviour
	{
		[SerializeField] private PolygonCollider2D _polygonCollider;
		[SerializeField] private Vector2 _size = default;
		[SerializeField] private int _cellCount = default;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField, Expandable] private ItemStats _itemStats = default;
		[Space]
		[SerializeField] private WeightedRandomList<ItemStats> _ItemStatsToRollFrom = new();

		private Vector3 _originalPosition;
		private Vector3 _previousPosition;
		private Quaternion _originalRotation;
		private Quaternion _previousRotation;
		private Vector2 _originalSize;
		private Vector2 _previousSize;
		private readonly List<InventoryCell> _occupiedCells = new();

		public ItemStats ItemStats { get => _itemStats; }

		private void Start()
		{
			_itemStats = _ItemStatsToRollFrom.GetRandom();
			_spriteRenderer.sprite = _itemStats.Sprite;

			_originalSize = _size;
			_originalPosition = transform.position;
			_originalRotation = transform.rotation;
		}

		public void PickUp()
		{
			_previousPosition = transform.position;
			_previousRotation = transform.rotation;
			Debug.Log($"Picking Up {transform.name}");
		}
		public void Drag()
		{
			Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePosition.z = _previousPosition.z;
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
			transform.SetPositionAndRotation(_originalPosition, _originalRotation);
			_size = _originalSize;
			ClearOccupiedCells();
			Debug.Log($"Reset {transform.name}");
		}

		public void Rotate(bool right)
		{
			if (right)
				transform.Rotate(0, 0, transform.rotation.y - 90);
			else
				transform.Rotate(0, 0, transform.rotation.y + 90);

			Vector2 newSize = new(_size.y, _size.x);
			_size = newSize;
		}
		private void FindAllOverlappingCellsAndPlaceItemInCenter()
		{
			Bounds combinedCellsBounds;
			ContactFilter2D contactFilter = new()
			{
				layerMask = LayerMask.GetMask("Cell")
			};

			List<Collider2D> colliders = new();
			Physics2D.OverlapCollider(_polygonCollider, contactFilter, colliders);

			if (colliders.Count < _cellCount)
			{
				transform.SetPositionAndRotation(_previousPosition, _previousRotation);
				_size = _previousSize;
				return;
			}

			foreach (Collider2D collider in colliders)
			{
				collider.TryGetComponent(out InventoryCell inventoryCell);

				if (!inventoryCell) continue;

				if (inventoryCell.Occupied && !_occupiedCells.Contains(inventoryCell))
				{
					transform.SetPositionAndRotation(_previousPosition, _previousRotation);
					_size = _previousSize;
					return;
				}
			}

			_previousSize = _size;
			ClearOccupiedCells();
			foreach (Collider2D collider in colliders)
			{
				collider.TryGetComponent(out InventoryCell inventoryCell);
				inventoryCell.SetOccupied();
				_occupiedCells.Add(inventoryCell);
			}

			combinedCellsBounds = _occupiedCells[0].GetComponent<BoxCollider2D>().bounds;
			for (int i = 1; i < _occupiedCells.Count; i++)
			{
				InventoryCell inventoryCell = _occupiedCells[i];
				combinedCellsBounds.Encapsulate(inventoryCell.GetComponent<BoxCollider2D>().bounds);
			}

			transform.position = combinedCellsBounds.center;
		}
		private void ClearOccupiedCells()
		{
			if (_occupiedCells.Count == 0) return;
			foreach (InventoryCell cell in _occupiedCells)
			{
				cell.ClearCell();
			}
			_occupiedCells.Clear();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green; // Set the Gizmos color
			Vector2[] points = _polygonCollider.GetPath(0); // Get the points of the polygon

			// Draw lines between consecutive points to visualize the edges of the polygon
			for (int i = 0; i < points.Length; i++)
			{
				Vector2 startPoint = transform.TransformPoint(points[i]);
				Vector2 endPoint = transform.TransformPoint(points[(i + 1) % points.Length]);
				Gizmos.DrawLine(startPoint, endPoint);
			}

			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position, _size);
		}
	}
}
