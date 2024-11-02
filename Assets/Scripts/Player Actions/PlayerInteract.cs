using System.Collections;
using System.Collections.Generic;
using Interact;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	[SerializeField] private PlayerMovement playerMovement;
	
	[SerializeField] private bool canInteract = true;
	
	[Header("Interaction")]
	[SerializeField] private float interactionDistance = 3;
	[SerializeField] private LayerMask interactionLayer = default;
	[SerializeField] private Interactable currentInteractable;
	[SerializeField] private Interactable previousInteractable;
	private List<Collider> _colliders = new List<Collider>();
	
    // Start is called before the first frame update
    void Start()
    {
	    playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void OnEnable()
    {
	    GameEventsManager.Instance.InputEvents.OnInteractEvent += OnInteract;
    }
    
    private void OnDisable()
    {
	    GameEventsManager.Instance.InputEvents.OnInteractEvent -= OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
	    Interaction();
    }
    
    // ***** PLAYER INTERACTION *****
		
	private void Interaction()
	{
		if (!canInteract) return;
		
		previousInteractable = currentInteractable;

		// third person
		_colliders = new List<Collider>(); 
		Vector3 targetPosition = transform.position;
		targetPosition.y = playerMovement.StandHeight / 2;
		
		Collider[] overlappingColliders = Physics.OverlapSphere(targetPosition, interactionDistance, interactionLayer);
		
		_colliders.AddRange(overlappingColliders);

		if (_colliders.Count > 0)
		{
			float nearestDistance = float.MaxValue;
			Collider nearestInteractable = null;

			foreach (Collider collider in _colliders)
			{
				if (collider == null) continue;

				if (collider != null)
				{
					Vector3 directionToInteractable = collider.transform.position - new Vector3(transform.position.x, playerMovement.StandHeight/2,transform.position.z );

					float distanceToInteractable = directionToInteractable.magnitude;
					if (distanceToInteractable < nearestDistance)
					{
						nearestDistance = distanceToInteractable;
						nearestInteractable = collider;
					}
				}
			}

			if (nearestInteractable != null)
				currentInteractable = nearestInteractable.GetComponent<Interactable>();
			currentInteractable.OnFocus();

			if (currentInteractable != null && currentInteractable != previousInteractable)
			{
				if (previousInteractable != null)
				{
					previousInteractable.OnLoseFocus();
				}
			}
				
		}
		else
		{
			currentInteractable = null;
		}
		
		if (previousInteractable != currentInteractable)
		{
			if (previousInteractable != null)
			{
				previousInteractable.OnLoseFocus();
			}
		}
	}

	private void OnInteract()
	{
		if (currentInteractable != null)
		{
			currentInteractable.OnInteract();
		}
	}
	
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, interactionDistance);
	}
}