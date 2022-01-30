using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _auidoClips;

    public int GetAudioClipIndex(string name)
    {
        for (int i = 0; i < _auidoClips.Count; i++)
        {
            if(_auidoClips[i].name == name)
            {
                return i;
            }
        }

        throw new System.Exception($"Not found {name}");
    }

    public void PlayAudio(int index)
    {
        _audioSource.clip = _auidoClips[index];
        _audioSource.Play();
    }
}
