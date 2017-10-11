using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextToSpeech;
using System;

[RequireComponent(typeof(AudioSource))]
public class PlayTextToSpeech : MonoBehaviour
{
    [SerializeField] TextToSpeechManager manager;
    [SerializeField] Button _button;
    [SerializeField] InputField _inputField;
    [SerializeField] Dropdown _languageDropDownList;
    [SerializeField] Dropdown _audioFormatDropDownList;

    private string text;
    
    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_button != null)
        {
            _button.onClick.AddListener(PlayVoice);
        }

        foreach (string name in Enum.GetNames((typeof(Language))))
        {
            _languageDropDownList.options.Add(new Dropdown.OptionData(name));
        }
        _languageDropDownList.value =3;

        foreach (string name in Enum.GetNames((typeof(AudioCodecs))))
        {
            _audioFormatDropDownList.options.Add(new Dropdown.OptionData(name));
        }
        _audioFormatDropDownList.value = 3;
    }

    public void PlayVoice()
    {
        StartCoroutine("RunPlayVoice");
    }

    IEnumerator RunPlayVoice()
    {
        OnGetVoiceStart();

        text = _inputField.text;
        string url = manager.GetTextToSpeechAudioWithIndex(text, _languageDropDownList.value, _audioFormatDropDownList.value);
        WWW www = new WWW(url);
        yield return www;
        AudioClip clip = www.GetAudioClip(false, false, manager.GetCurrentAudioType());
        if (clip.length > 0)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }      
        else
        {
            Debug.LogError("Failed to get the voice. Please try:\n" +
                "1.Try it in other languages.\n" +
                "2.Fill in something in text field.\n" +
                "3.Choose the correct audio format.");
        }

        OnGetVoiceEnd();
    }

    void OnGetVoiceStart()
    {
        if (_button != null)
        {
            _button.enabled = false;
            _button.targetGraphic.color = _button.colors.disabledColor;
        }
    }

    void OnGetVoiceEnd()
    {
        if (_button != null)
        {
            _button.enabled = true;
            _button.targetGraphic.color = _button.colors.normalColor;
        }
    }
}
