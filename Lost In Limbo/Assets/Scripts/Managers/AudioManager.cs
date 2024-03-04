using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

enum ClipWrapMode
{
    Default = 0,
    Cycle,
    PlayAll,
    Random,
    Loop,
}

[System.Serializable]
struct AudioTrack
{
    public string m_Name;
    public AudioSource m_Source;
    public ClipWrapMode m_WrapMode;
    public AudioClip[] m_Clip;
    public int m_CurrentClip;

    [Space]
    public SubtitleText[] m_Subtitles;
}

[System.Serializable]
struct SubtitleText
{
    public string m_SubtitleText;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioTrack[] m_AudioTrack;

    [Header("Subtitles")]
    [SerializeField] Text m_SubtitleTextObject;
    [SerializeField] bool m_UseSubtitles;

    int m_CurrentTrack = 0;
    bool m_AudioClipIsPlaying = false;
    DataManager m_DataManager;

    private void Start()
    {
        if (m_SubtitleTextObject)
            m_SubtitleTextObject.gameObject.SetActive(false);

        m_DataManager = FindAnyObjectByType<DataManager>();

        if (m_DataManager)
            m_UseSubtitles = m_DataManager.GetSubtitlesState();
    }

    private void Update()
    {
        if (!m_AudioClipIsPlaying)
            return;

        if (m_AudioTrack[m_CurrentTrack].m_WrapMode == ClipWrapMode.PlayAll)
        {
            if (m_AudioTrack[m_CurrentTrack].m_CurrentClip < m_AudioTrack[m_CurrentTrack].m_Clip.Length - 1)
            {
                if (!m_AudioTrack[m_CurrentTrack].m_Source.isPlaying)
                {
                    m_AudioTrack[m_CurrentTrack].m_CurrentClip++;
                    m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                    m_AudioTrack[m_CurrentTrack].m_Source.Play();

                    if (m_UseSubtitles)
                    {
                        m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_AudioTrack[m_CurrentTrack].m_CurrentClip].m_SubtitleText;
                    }
                }
            }
            else
            {
                m_AudioClipIsPlaying = false;
                if (m_UseSubtitles)
                    m_SubtitleTextObject.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!m_AudioTrack[m_CurrentTrack].m_Source.isPlaying)
            {
                if (m_UseSubtitles)
                {
                    m_AudioClipIsPlaying = false;
                    m_SubtitleTextObject.gameObject.SetActive(false);
                }

            }
        }
    }

    public void PlayTrack(int _track)
    {
        m_CurrentTrack = _track;
        m_AudioClipIsPlaying = true;

        if (m_UseSubtitles)
            m_SubtitleTextObject.gameObject.SetActive(true);

        switch (m_AudioTrack[m_CurrentTrack].m_WrapMode)
        {
            case ClipWrapMode.Default:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_CurrentTrack].m_SubtitleText;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                break;

            case ClipWrapMode.Cycle:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_CurrentTrack].m_SubtitleText;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                m_AudioTrack[m_CurrentTrack].m_CurrentClip++;
                break;

            case ClipWrapMode.PlayAll:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                break;


            case ClipWrapMode.Random:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[Random.Range(0, m_AudioTrack[m_CurrentTrack].m_Clip.Length)];
                m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_CurrentTrack].m_SubtitleText;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                break;

            case ClipWrapMode.Loop:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                m_AudioTrack[m_CurrentTrack].m_Source.loop = true;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                break;
        }
    }
}
