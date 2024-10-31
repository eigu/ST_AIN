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
    public GameObject barkHolder;
    public float wordsPerMinute = 120f;
    public TextMeshProUGUI barkTextUI;
    public List<string> barkData = new List<string>();
    public float minBarkWaitTime = 2f;
    public float maxBarkWaitTime = 10f;
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
        if (barkData != null && barkData.Count > 0)
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
        yield return new WaitForSeconds(Random.Range(minBarkWaitTime, maxBarkWaitTime));
        
        while (true)
        {
            int index;

            if (barkData.Count > 1)
            {
                do {
                    index = Random.Range(0, barkData.Count);
                } while (index == _prevIndex);
            }
            else
            {
                index = 0;
            }
        
            barkHolder.SetActive(true);
            
            float secs = CalculateDuration(barkData[index]);
        
            yield return ShowBark(secs, barkData[index]);
        
            barkHolder.SetActive(false);
            
            _prevIndex = index;
        
            yield return new WaitForSeconds(Random.Range(minBarkWaitTime, maxBarkWaitTime));
        }
        
    }
    
    IEnumerator ShowBark(float secs, string text)
    {
        barkTextUI.text = text;
        yield return new WaitForSeconds(secs);
    }
    
    private float CalculateDuration(string dialogue)
    {
        int wordCount = dialogue.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        float durationInSeconds = (wordCount / wordsPerMinute) * 60f;

        return durationInSeconds;
    }

    public void StopBark()
    {
        if (_barkCoroutine != null)
        {
            StopCoroutine(_barkCoroutine);
            _barkCoroutine = null; 
        }
        barkHolder.SetActive(false);
        _isActive = false;
    }

    public void StartShowingBark()
    {
        _isActive = true;
        _barkCoroutine = StartCoroutine(StartBark());
        
    }
    
}