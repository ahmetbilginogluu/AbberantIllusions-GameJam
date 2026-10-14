using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class DeathScript : MonoBehaviour
{
    public AudioSource hitAudioSource;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(hitAudioSource != null)
            {
                hitAudioSource.pitch = Random.Range(0.85f, 0.95f);
                hitAudioSource.Play();
            }
            other.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 0.95f);
            other.GetComponent<AudioSource>().Play();
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<CapsuleCollider>().enabled = false;
            GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().PlayTransition(0);
        }
    }
}