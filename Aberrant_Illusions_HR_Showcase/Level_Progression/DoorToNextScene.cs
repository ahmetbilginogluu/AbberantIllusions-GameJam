using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextScene : MonoBehaviour
{

    [SerializeField] GameObject playerCopy_prefab;
    [SerializeField] Animator anim;
    [SerializeField] float wait_Time = 0.5f;
    [SerializeField] float loadScene_Time = 3f;
    public Transform targetPosition; 
    public float moveSpeed = 5.0f;

    [SerializeField] bool no_Transition = false;


    private void Start() {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is tagged as Player or adjust according to your needs
        if (other.CompareTag("Player"))
        {
            if(anim != null)
            {anim.Play("OpenDoor");}
            GameObject replacement = Instantiate(playerCopy_prefab, other.transform.position, other.transform.rotation);
            replacement.transform.localScale = other.transform.localScale; // Copy scale

            // Optionally, if you want the replacement to move smoothly to a new position
            replacement.GetComponent<SmoothMover>().targetPosition = targetPosition.position;
            replacement.GetComponent<SmoothMover>().wait_Time = wait_Time;
            replacement.GetComponent<SmoothMover>().moveSpeed = moveSpeed;

            other.gameObject.SetActive(false); // Destroy the player
            Invoke("LoadNextScene", loadScene_Time);
        }
    }

    void LoadNextScene()
    {
        if(no_Transition)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Check if the next scene index exceeds the number of scenes available
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No next scene found. Please check the Build Settings.");
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().PlayTransition(1);
        }
    }
}
