using UnityEngine;

namespace Assets.Scripts
{
	public class InventoryCell : MonoBehaviour
	{
		private bool occupied = false;
		private SpriteRenderer spriteRenderer;

		public bool Occupied { get => occupied; }

		private void Awake()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.color = Color.white;
		}

		public bool IsEmpty()
		{
			spriteRenderer.color = Color.white;
			return !occupied;
		}

		public void SetOccupied()
		{
			spriteRenderer.color = Color.red;
			occupied = true;
		}

		public void ClearCell()
		{
			spriteRenderer.color = Color.white;
			occupied = false;
		}
	}
}