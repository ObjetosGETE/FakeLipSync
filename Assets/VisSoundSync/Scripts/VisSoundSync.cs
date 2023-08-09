using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SharkJets
{
    public class VisSoundSync : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private List<VisSubtitle> visSubtitles;
        [SerializeField] private bool autoStart;
        
        [SerializeField] private UnityEvent OnStart;
        [SerializeField] private UnityEvent OnNextLine;
        [SerializeField] private UnityEvent OnStop;

        public event Action<int> OnStartEvent;
        public event Action<int, string> OnNextLineEvent;
        public event Action OnStopEvent;

        private bool _isPlaying;
        private List<JsonSubtitle> _dialogue;
        private JsonSubtitle _onDeck;
        private int _textCount;
        private int _currentTextId;
        private string _currentText;
        
        void Start()
        {
            if(autoStart) LoadClip(0);
        }

        public void Restart()
        {
            autoStart = true;
        }

        private void Update()
        {
            if (!_isPlaying && autoStart)
            {
                LoadClip(0);
                Play();
            }
        }

        public void LoadClip(int clipId)
        {
            if (visSubtitles.Count < clipId + 1)
            {
                Debug.Log($"Error: You tried to play VisSubtitle #{clipId}, but that number doesn't exist.");
                return;
            }
            
            var clip = visSubtitles[clipId];
            Load(clip);
        }
        
        public void LoadClip(string clipName)
        {
            if (clipName == "")
            {
                Debug.Log($"Error: You tried to load a VisSubtitle by name, but no name was provided.");
                return;
            }

            VisSubtitle clip = null;

            foreach (var item in visSubtitles)
            {
                if (item.name == clipName)
                {
                    clip = item;
                    break;
                }
            }

            if (clip == null)
            {
                Debug.Log($"Error: You tried to load a VisSubtitle by name {clipName}, but the name was not found.");
                return;
            }
            
            Load(clip);
        }    

        private void Load(VisSubtitle clip)
        {
            _dialogue   = JsonUtility.FromJson<JsonSubtitleList>("{\"result\":" + clip.jsonFile.text + "}").result.ToList();
            _textCount = _dialogue.Count;
            audioSource.clip = clip.audioClip;
            _onDeck = _dialogue[0];
            
            if(autoStart) Play();
        }

        public void Play()
        {
            if (_onDeck == null)
            {
                Debug.Log("No subtitle/track is loaded. Be sure to call LoadClip first.");
                return;
            }
            
            _isPlaying = true;
            audioSource.Play();
            InvokeRepeating("CheckTiming", 0, 0.01f);
            OnStart?.Invoke();
            OnStartEvent?.Invoke(_textCount);
        }

        public void Stop()
        {
            textMeshPro.text = "";
            autoStart = false;
            _isPlaying = false;
            _currentTextId = 0;
            OnStop?.Invoke();
            OnStopEvent?.Invoke();
        }

        public void Pause()
        {
            if (_isPlaying)
            {
                _isPlaying = false;
                audioSource.Pause();
            }
            else
            {
                _isPlaying = true;
                audioSource.Play();
            }
            
        }

        private void CheckTiming()
        {
            if (_isPlaying)
            {
                if (audioSource.time >= _onDeck.start && audioSource.time <= _onDeck.end && _currentText != _onDeck.text)
                {
                    _currentText = _onDeck.text;
                    textMeshPro.text = _currentText;
                    OnNextLine?.Invoke();
                    OnNextLineEvent?.Invoke(_currentTextId, _currentText);
                }
                if (audioSource.time > _onDeck.end)
                {
                    _currentTextId++;
                    if (_currentTextId < _textCount)
                    {
                        _onDeck = _dialogue[_currentTextId];
                    }
                    else
                    {
                        Stop();
                    }
                }
            }        
        }
    }

    [Serializable]
    public class JsonSubtitleList
    {
        public JsonSubtitle[] result;
    }

    [Serializable]
    public class JsonSubtitle
    {
        public float start;
        public float end;
        public string text;
    }
}

