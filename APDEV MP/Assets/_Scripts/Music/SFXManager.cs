using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;


    private List<AssetReference> _clipReferences; 
    private List<AudioSource> _audioChannels;
    [SerializeField] private SFXDatabase SO_AudioDatabase;

    public void PlaySFX(EnumSFX sfxID)
    {
        for (int i = 0; i < _audioChannels.Count; i++)
        {
            if (!_audioChannels[i].isPlaying)
            {
                this.PlayClipFromAddressables(sfxID, i); ;
                return;
            }
        }
    }

    private void PlayClipFromAddressables(EnumSFX sfxID, int index)
    {
        if (_clipReferences[index] != null)
        {
            this._clipReferences[index].ReleaseAsset();
            this._clipReferences[index] = null;
        }


        AssetReference toPlay = this.SO_AudioDatabase.GetClipReference(sfxID);

        if (this._clipReferences.Contains(toPlay))
        {
            return;
        }


        this._clipReferences[index] = toPlay;

        this._clipReferences[index].LoadAssetAsync<AudioClip>().Completed += (handle) =>
        {
            if(handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to load sfx {sfxID}");
                return;
            }

            Debug.Log($"SFX Found: {handle.Result.name}");
            if(_audioChannels[index] != null && handle.Result != null)
            {
                _audioChannels[index].PlayOneShot(handle.Result);
            }
        };
    }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        _audioChannels = new();
        _clipReferences = new();

        for (int i = 0; i < 4; i++)
        {
            _audioChannels.Add(this.AddComponent<AudioSource>());
            _audioChannels[i].playOnAwake = false;
            _audioChannels[i].loop = false;

            _clipReferences.Add(new());
        }
    }

    private void OnDestroy()
    {
        foreach(var reference  in _clipReferences)
        {
            if(reference != null)
            {
                reference.ReleaseAsset();
            }
        }
    }
}
