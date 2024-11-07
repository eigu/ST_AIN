using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Compiler;
using Random = UnityEngine.Random;

public class BarkUI : MonoBehaviour
{
    //private bool _playerNear = false;
    [SerializeField] [Tooltip("GameObject that will be disabled and enabled.")]
    private GameObject _barkHolder;
    [SerializeField] [Tooltip("Duration how long the bark will stay.")]
    private float _wordsPerMinute = 120f;
    [SerializeField] [Tooltip("TMP for displaying text.")]
    private TextMeshProUGUI _barkTextUI;
    [SerializeField] [Tooltip("List of array of dialogues, it will be chosen randomly per bark.")]
    private List<string> _barkData = new List<string>();
    [SerializeField] [Tooltip("Minimum duration for the inactive phase of bark.")]
    private float _minBarkWaitTime = 2f;
    [SerializeField] [Tooltip("Maximum duration for the inactive phase of bark.")]
    private float _maxBarkWaitTime = 10f;
    private Camera _mainCam;
    private int _prevIndex = -1;
    private Coroutine _barkCoroutine;
    private bool _isActive = true;
    
    
    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Start()
    {
        if (_barkData != null && _barkData.Count > 0)
        {
            StartShowingBark();
        }
        else
        {
            _isActive = false;
        }
    }

    void Update()
    {
        if (_isActive)
        {
            transform.rotation = _mainCam.transform.rotation;
        }
        
    }

    IEnumerator StartBark()
    {
        yield return new WaitForSeconds(Random.Range(_minBarkWaitTime, _maxBarkWaitTime));
        
        while (true)
        {
            int index;

            if (_barkData.Count > 1)
            {
                do {
                    index = Random.Range(0, _barkData.Count);
                } while (index == _prevIndex);
            }
            else
            {
                index = 0;
            }
        
            _barkHolder.SetActive(true);
            
            float secs = CalculateDuration(_barkData[index]);
        
            yield return ShowBark(secs, _barkData[index]);
        
            _barkHolder.SetActive(false);
            
            _prevIndex = index;
        
            yield return new WaitForSeconds(Random.Range(_minBarkWaitTime, _maxBarkWaitTime));
        }
        
    }
    
    IEnumerator ShowBark(float secs, string text)
    {
        _barkTextUI.text = text;
        yield return new WaitForSeconds(secs);
    }
    
    private float CalculateDuration(string dialogue)
    {
        int wordCount = dialogue.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        float durationInSeconds = (wordCount / _wordsPerMinute) * 60f;

        return durationInSeconds;
    }

    public void StopBark()
    {
        if (_barkCoroutine != null)
        {
            StopCoroutine(_barkCoroutine);
            _barkCoroutine = null; 
        }
        _barkHolder.SetActive(false);
        _isActive = false;
    }

    public void StartShowingBark()
    {
        _isActive = true;
        _barkCoroutine = StartCoroutine(StartBark());
    }
    
}