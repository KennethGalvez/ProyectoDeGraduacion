using UnityEngine;

public class PipelineMov : MonoBehaviour
{
    public Vector3 targetPosition; 
    public float speed = 2f; 
    public float respawnDelay = 1f; 

    private Vector3 startPosition; 
    private bool isMoving = true; 

    private void Start()
    {
        
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        
        if (transform.position == targetPosition)
        {
            isMoving = false; 
            DestroyPlatform(); 
        }
    }

    private void DestroyPlatform()
    {
        
        gameObject.SetActive(false); 
        Invoke(nameof(RespawnPlatform), respawnDelay); // Respawn 
    }

    private void RespawnPlatform()
    {
        
        transform.position = startPosition;
        gameObject.SetActive(true);
        isMoving = true; // Start again
    }
}
