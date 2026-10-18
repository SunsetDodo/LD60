using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public bool isFollowing;
    public GameObject followObject;
    [SerializeField] private float followSpeed = 5f;
    
    
    
    void Update()
    {
        if (isFollowing)
        {
            var target = new Vector3(followObject.transform.position.x, followObject.transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
        }    
    }
}
