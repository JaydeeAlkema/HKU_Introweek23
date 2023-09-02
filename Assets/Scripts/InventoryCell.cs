using UnityEngine;

namespace Assets.Scripts
{
	public class InventoryCell : MonoBehaviour
	{
		private bool _occupied = false;
		private SpriteRenderer _spriteRenderer;

		public bool Occupied { get => _occupied; }

		private void Awake()
		{
			_spriteRenderer = transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
			_spriteRenderer.color = Color.white;
		}

		public bool IsEmpty()
		{
			_spriteRenderer.color = Color.white;
			return !_occupied;
		}

		public void SetOccupied()
		{
			_spriteRenderer.color = Color.red;
			_occupied = true;
		}

		public void ClearCell()
		{
			_spriteRenderer.color = Color.white;
			_occupied = false;
		}
	}
}