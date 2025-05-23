using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : BaseSingleton<MusicManager>
{

    //背景音乐播放组件
    private AudioSource bkMusic = null;

    //背景音乐大小
    private float bkMusicValue = 0.1f;

    //用于音效组件依附的对象
    private GameObject soundObj = null;
    //管理正在播放的音效
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效音量大小
    private float soundValue = 0.3f;
    //音效是否在播放
    private bool soundIsPlay = true;
    private void Update()
    {
        if (!soundIsPlay)
            return;

        //不停的遍历容器 检测有没有音效播放完毕 播放完了 就移除销毁它
        //为了避免边遍历边移除出问题 我们采用逆向遍历
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }
    public void SayTrackMessage(int trackIndex, string name, bool isLoop = false, bool isSync = false, UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject("messageObj");
        }
        //加载音效资源 进行播放
        AudioClip clip = Resources.Load<AudioClip>("Music/TrackMessage/" + name);
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = isLoop;
        source.volume = soundValue;
        source.panStereo = trackIndex;
        source.Play();
        //存储容器 用于记录 方便之后判断是否停止
        soundList.Add(source);
        //传递给外部使用
        callBack?.Invoke(source);

    }

    //播放背景音乐
    public void PlayBKMusic(string resName)
    {
        //动态创建播放背景音乐的组件 并且 不会过场景移除 
        //保证背景音乐在过场景时也能播放
        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //根据传入的背景音乐名字 来播放背景音乐
        AudioClip clip = Resources.Load<AudioClip>("Music/BGM/" + resName);
        bkMusic.clip = clip;
        bkMusic.loop = true;
        bkMusic.volume = bkMusicValue;
        bkMusic.Play();
        //return bkMusic;
    }

    //停止背景音乐
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    //暂停背景音乐
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    //设置背景音乐大小
    public void ChangeBKMusicValue(float v)
    {
        bkMusicValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkMusicValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="isSync">是否同步加载</param>
    /// <param name="callBack">加载结束后的回调</param>
    public void PlaySound(string name, float pitch = 1, bool isLoop = false, bool isSync = false,UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject("soundObj");
        }
        //加载音效资源 进行播放
        AudioClip clip = Resources.Load<AudioClip>("Music/SFX/" + name);
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = isLoop;
        source.pitch = pitch;
        source.volume = soundValue;
        source.Play();
        //存储容器 用于记录 方便之后判断是否停止
        soundList.Add(source);
        //传递给外部使用
        callBack?.Invoke(source);
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source">音效组件对象</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            //停止播放
            source.Stop();
            //从容器中移除
            soundList.Remove(source);
            //从依附对象上移除
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        soundValue = v;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = v;
        }
    }

    /// <summary>
    /// 继续播放或者暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否是继续播放 true为播放 false为暂停</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if (isPlay)
        {
            soundIsPlay = true;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Play();
        }
        else
        {
            soundIsPlay = false;
            for (int i = 0; i < soundList.Count; i++)
                soundList[i].Pause();
        }
    }
}
