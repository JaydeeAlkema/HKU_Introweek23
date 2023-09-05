using UnityEngine;


public class ClampToCameraView : MonoBehaviour
{
	private Camera _mainCamera;
	[SerializeField] private RectTransform _canvasRect;
	[SerializeField] private RectTransform _elementRect;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void LateUpdate()
	{
		Vector3[] corners = new Vector3[4];
		_elementRect.GetWorldCorners(corners);

		Vector2 minViewportPoint = _mainCamera.WorldToViewportPoint(corners[0]);
		Vector2 maxViewportPoint = _mainCamera.WorldToViewportPoint(corners[2]);

		// Clamp the viewport coordinates to keep the element within the camera's view
		minViewportPoint = new Vector2(Mathf.Clamp01(minViewportPoint.x), Mathf.Clamp01(minViewportPoint.y));
		maxViewportPoint = new Vector2(Mathf.Clamp01(maxViewportPoint.x), Mathf.Clamp01(maxViewportPoint.y));

		// Calculate the new position in world space
		Vector3 minWorldPoint = _mainCamera.ViewportToWorldPoint(minViewportPoint);
		Vector3 maxWorldPoint = _mainCamera.ViewportToWorldPoint(maxViewportPoint);

		// Calculate the element's new position
		Vector3 newPosition = transform.position + (minWorldPoint - corners[0]) + (maxWorldPoint - corners[2]);
		newPosition.z = 0;

		// Update the element's position
		transform.position = newPosition;
	}
}

