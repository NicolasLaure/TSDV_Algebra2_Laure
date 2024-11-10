using System;
using System.Collections;
using System.Collections.Generic;
using SortingAlgorithms;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip iterationClip;
    [SerializeField] private AudioClip panningClip;
    [SerializeField] private AudioClip startPanningClip;

    private void OnEnable()
    {
        Sort<int>.onListUpdated += HandleSound;
    }

    private void OnDisable()
    {
        Sort<int>.onListUpdated -= HandleSound;
    }

    private void HandleSound(List<int> list)
    {
        audioSource.pitch = 1;
        audioSource.clip = iterationClip;
        audioSource.Play();
    }

    public void PlayStartPanning()
    {
        audioSource.pitch = 1;
        audioSource.clip = startPanningClip;
        audioSource.Play();
    }

    public void PlayPanningSoundPitched(float pitch)
    {
        audioSource.clip = panningClip;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}