using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> damageTakenSFX = new();
    [SerializeField] private List<AudioClip> deathSFX = new();
    [SerializeField] private List<AudioClip> playerHitSFX = new();
    [SerializeField] private AudioClip levelStart;
    [SerializeField] private AudioClip playerSwimming;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource.PlayOneShot(levelStart);
    }

   public void PlayDamageTakenSFX()
   {
        audioSource.Stop();
		audioSource.PlayOneShot(damageTakenSFX[Random.Range(0, damageTakenSFX.Count)]);
   }

	public void PlayDeathSFX()
	{
		audioSource.Stop();
		audioSource.PlayOneShot(deathSFX[Random.Range(0, deathSFX.Count)]);
	}

	public void PlayerHitSFX()
	{
		audioSource.Stop();
		audioSource.PlayOneShot(playerHitSFX[Random.Range(0, playerHitSFX.Count)]);
	}

    public void PlayerSwimmingSFX()
    {
		audioSource.Stop();
		audioSource.PlayOneShot(playerSwimming);
    }
}
