using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Vector3 LastCheckpointPosition;

    private static GameMaster instance = null;
    public GameObject objectToInstantiate;
    public Vector3 instantiationPosition;
    private bool at_Scene_Five = false;
    [SerializeField] GameObject the_Door;
    [SerializeField] Animator anim;
    [SerializeField] float aim_Wait_Time;
    public AudioSource transitionAudioSource;

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;

            // Makes the object not be destroyed automatically when loading a new scene.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // If instance already exists and it's not this, then destroy this to enforce the singleton pattern.
            Destroy(gameObject);
        }
    }


    private void Start() {
        anim.Play("Out");
    }

    public void SaveCheckpoint(Vector3 position)
    {
        LastCheckpointPosition = position;
        // Optionally, save to PlayerPrefs or another persistent storage system for even more persistence (e.g., between game restarts)
    }

    public IEnumerator PlayTransitionCor(int sceneOffset)
    {
        // Play the first animation
        transitionAudioSource.pitch = 1.7f;
        transitionAudioSource.Play();
        anim.Play("In");

        // Wait for the specified time
        yield return new WaitForSeconds(aim_Wait_Time);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + sceneOffset;

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
        

        // Play the second animation
        transitionAudioSource.pitch = 1.2f;
        transitionAudioSource.Play();
        anim.Play("Out");
    }


    public void PlayTransition(int number)
    {
        StartCoroutine(PlayTransitionCor(number));
    }
}
