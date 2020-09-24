using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private ManagerVars _vars;
    
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        _vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.PlayClickAudio, PlayAudio);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayClickAudio, PlayAudio);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    private void PlayAudio()
    {
        m_AudioSource.PlayOneShot(_vars.buttonClip);
    }
    
    private void IsMusicOn(bool val)
    {
        m_AudioSource.mute = !val;
    }
}
