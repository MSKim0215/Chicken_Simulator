using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private AudioSource[] audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject temp = new GameObject { name = soundNames[i] };
                audioSources[i] = temp.AddComponent<AudioSource>();
                temp.transform.parent = root.transform;
            }

            audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach(AudioSource source in audioSources)
        {
            source.clip = null;
            source.Stop();
        }
        audioClips.Clear();
    }

    /// <summary>
    /// 사운드 재생 함수
    /// </summary>
    /// <param name="_path">사운드 파일 경로</param>
    /// <param name="_type">사운드 타입</param>
    /// <param name="_pitch">음의 높낮이</param>
    public void Play(string _path, Define.Sound _type = Define.Sound.Sfx, float _pitch = 1f)
    {
        AudioClip clip = GetOrAddAudioClip(_path, _type);
        Play(clip, _type, _pitch);
    }

    /// <summary>
    /// 사운드 재생 함수
    /// </summary>
    /// <param name="_path">사운드 파일 경로</param>
    /// <param name="_type">사운드 타입</param>
    /// <param name="_pitch">음의 높낮이</param>
    public void Play(AudioClip _clip, Define.Sound _type = Define.Sound.Sfx, float _pitch = 1f)
    {
        if (_clip == null) return;

        if (_type == Define.Sound.Bgm)
        {   // TODO: 배경음 재생
            AudioSource source = audioSources[(int)Define.Sound.Bgm];
            if (source.isPlaying) source.Stop();
            source.pitch = _pitch;
            source.clip = _clip;
            source.Play();
        }
        else
        {   // TODO: 효과음 재생
            AudioSource source = audioSources[(int)Define.Sound.Sfx];
            source.pitch = _pitch;
            source.PlayOneShot(_clip);
        }
    }

    /// <summary>
    /// 사운드 파일을 가져오거나 새로 추가 하는 함수
    /// </summary>
    /// <param name="_path">사운드 파일 경로</param>
    /// <param name="_type">사운드 타입</param>
    /// <returns></returns>
    private AudioClip GetOrAddAudioClip(string _path, Define.Sound _type = Define.Sound.Sfx)
    {
        if (!_path.Contains("Sounds/")) _path = $"Sounds/{_path}";

        AudioClip clip = null;

        if (_type == Define.Sound.Bgm)
        {   // TODO: 배경음 재생
            clip = Managers.Resource.Load<AudioClip>(_path);
            if (clip == null)
            {
                Debug.LogWarning($"AudioClip Missing! {_path}");
            }
        }
        else
        {   // TODO: 효과음 재생
            if (!audioClips.TryGetValue(_path, out clip))
            {
                clip = Managers.Resource.Load<AudioClip>(_path);
                audioClips.Add(_path, clip);
            }

            if (clip == null)
            {
                Debug.LogWarning($"AudioClip Missing! {_path}");
            }
        }
        return clip;
    }
}