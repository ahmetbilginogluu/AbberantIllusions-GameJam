using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] float y_Offset;
    Animator childAnimator;
    private bool checkpoint_Set = false;
    GameMaster gm;
    public AudioSource audio;

    private void Start() {
        audio = GetComponent<AudioSource>();
        gm = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
        childAnimator = GetComponentInChildren<Animator>();
        if(gm.LastCheckpointPosition == transform.position + new Vector3(0, y_Offset, 0))
        {
            checkpoint_Set = true;
            childAnimator.Play("FlagWave");
        }
        else
        {
            checkpoint_Set = false;
            childAnimator.Play("NotTouched");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkpoint_Set) // Make sure the player is tagged correctly
        {
            audio.Play();
            childAnimator.Play("Touched");
            gm.SaveCheckpoint(transform.position + new Vector3(0, y_Offset, 0));
            GetComponent<Renderer>().material = material;
            checkpoint_Set = true;
        }
    }
}
