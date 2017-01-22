using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour {

    [Header("Music")]
    public AudioClip MenuLoop;
    public AudioClip WinJingle;
    public AudioClip MainL1;
    public AudioClip MainL2;
    public AudioClip MainL3;

    [Header("SFX")]
    public AudioClip SideMoveFx;
    public AudioClip DropSfx;

    [Header("Audio Sources")]
    public AudioSource MusicAudioSource1;
    public AudioSource MusicAudioSource2;
    public AudioSource MusicAudioSource3;
    public AudioSource SfxAudioSource;

    #region API
    public void PlayClip(Clip _clip, bool _play = true) {
        switch (_clip) {
            case Clip.MenuLoop:
                MusicAudioSource1.Stop();
                MusicAudioSource2.Stop();
                MusicAudioSource3.Stop();
                if (!_play)
                    break;
                MusicAudioSource1.clip = MenuLoop;
                MusicAudioSource1.loop = true;
                MusicAudioSource1.Play();
                break;
            case Clip.WinJingle:
                MusicAudioSource1.Stop();
                MusicAudioSource2.Stop();
                MusicAudioSource3.Stop();
                if (!_play)
                    break;
                MusicAudioSource1.clip = WinJingle;
                MusicAudioSource1.loop = false;
                MusicAudioSource1.Play();
                break;
            case Clip.Main:
                MusicAudioSource1.Stop();
                MusicAudioSource2.Stop();
                MusicAudioSource3.Stop();
                if (!_play)
                    break;
                MusicAudioSource1.clip = MainL1;
                MusicAudioSource1.loop = true;
                MusicAudioSource1.Play();
                MusicAudioSource2.clip = MainL2;
                MusicAudioSource2.loop = true;
                MusicAudioSource2.Play();
                MusicAudioSource3.clip = MainL3;
                MusicAudioSource3.loop = true;
                MusicAudioSource3.Play();
                break;
            case Clip.SideMoveFx:
                break;
            case Clip.DropSfx:
                break;
            default:
                break;
        }

    }
    #endregion


    public void PlayLayeredMusic(int _channel, float _volume, float _time = 1.5f) {
        switch (_channel) {
            case 1:
                DOTween.To(() => MusicAudioSource1.volume, x => MusicAudioSource1.volume = x, _volume, _time);
                break;
            case 2:
                DOTween.To(() => MusicAudioSource2.volume, x => MusicAudioSource1.volume = x, _volume, _time);
                break;
            case 3:
                DOTween.To(() => MusicAudioSource3.volume, x => MusicAudioSource1.volume = x, _volume, _time);
                break;
            default:
                break;
        }


        MusicAudioSource1.Stop();
        MusicAudioSource2.Stop();
        MusicAudioSource3.Stop();
        MusicAudioSource1.clip = MainL1;
        MusicAudioSource1.loop = true;
        MusicAudioSource1.Play();
        MusicAudioSource2.clip = MainL1;
        MusicAudioSource2.loop = true;
        MusicAudioSource2.Play();
        MusicAudioSource3.clip = MainL1;
        MusicAudioSource3.loop = true;
        MusicAudioSource3.Play();
    } 

    public enum Clip {

        MenuLoop = 1,
        WinJingle = 2,
        Main = 3,
        SideMoveFx = 6,
        DropSfx = 7,

    }

}
