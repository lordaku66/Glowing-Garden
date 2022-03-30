using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Sets the rotation and postion of the players directional arrow each frame in a radius around the Player sprite.
/// 
/// | Author: Jacques Visser and Krishna Thiruvengadam
/// </summary>

public class GameCursor : MonoBehaviour
{
    public static GameCursor instance;
    private SpriteRenderer spriteRenderer;
    public GameObject player;
    public float fadedOpacity;
    public Sprite defaultCursor;
    private Vector3 v;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnLeftMouse += ShowCursor;
        GameManager.OnLeftUp += HideCursor;

        GameManager.OnPause += OnPause;
        GameManager.OnPlay += OnPlay;

        GameManager.OnInteract += OnPause;
        GameManager.OnDialogueClose += OnPlay;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.color = new Color(1, 1, 1, fadedOpacity);
        v = (transform.position - player.transform.position);
    }

    private void OnPause()
    {
        spriteRenderer.enabled = false;
    }

    private void OnPlay()
    {
        spriteRenderer.enabled = true;
    }

    void ShowCursor()
    {
        LeanTween.color(gameObject, Color.white, 0.5f);
    }

    void HideCursor()
    {
        LeanTween.color(gameObject, new Color(1, 1, 1, fadedOpacity), 0.5f);
    }

    void Update()
    {
        Vector3 centerScreenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 dir = Input.mousePosition - centerScreenPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.position = player.transform.position + q * v;
        transform.rotation = q;

        float zRotation = transform.localEulerAngles.z;
    }

    public static float GetRotation()
    {
        return instance.transform.localEulerAngles.z;
    }

    private void OnDisable()
    {
        GameManager.OnLeftMouse -= ShowCursor;
        GameManager.OnLeftUp -= HideCursor;
    }
}