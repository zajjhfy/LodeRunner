using UnityEngine;

public enum SoundType{
    Lose,
    Win,
    ButtonSelect,
    Break,
    Land,
    Collect,
    Trapped
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _soundList;
    private AudioSource _audioSource;
    private static SoundManager instance;

    private void Awake(){
        instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType soundType, float volume = 1f){
        instance._audioSource.PlayOneShot(instance._soundList[(int)soundType], volume);
    }

}
