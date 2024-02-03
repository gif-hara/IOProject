using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField]
    private UIDocument sampleButtonUIPrefab;

    private UIDocument sampleButtonUI;

    void Start()
    {
        sampleButtonUI = Instantiate(sampleButtonUIPrefab);
        sampleButtonUI.rootVisualElement.Q<Button>("Button1").clicked += OnButton1Clicked;
    }

    private void OnButton1Clicked()
    {
        sampleButtonUI.rootVisualElement.transform.position = new Vector3
        (
            UnityEngine.Random.Range(0, 50),
            UnityEngine.Random.Range(0, 50),
            0
        );
        Debug.Log("Button 1 Clicked");
    }
}
