
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sử dụng để đổi sprite thay vì dùng animation buil in(nặng)
public class SpriteAnimination : MonoBehaviour
{

    public bool IsRunning
    {
        get
        {
            return running;
        }
    }
    public int CurFrameID
    {
        get
        {
            return curFrameID;
        }
        set
        {
            spriteRenderer.sprite = sprites[value];
            curFrameID = value;
        }
    }

    //phải xme có đổi frame không mới biết chạy thật ko ?
    public bool IsReallyRuning { get; private set; }

    public float startTime { get; private set; }

    public Sprite[] sprites;
    public float timeBetweenFrame = 0.04f;

    //key start random
    public bool startRandomKey;
    public bool oneShoot = true;
    
    int curFrameID;

    bool running;
    SpriteRenderer spriteRenderer;
    public virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnEnable()
    {
        IsReallyRuning = false;
        running = true;

        if (!startRandomKey)
        {
            curFrameID = 0;
        }
        else
        {
            curFrameID = Random.Range(0, sprites.Length);
        }

        spriteRenderer.sprite = sprites[curFrameID];

        startTime = Time.time;
    }
    private void OnDisable()
    {
        running = false;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        if (running)
        {
            OnRunning();
        }
    }
    protected virtual void OnRunning()
    {
        float currentTime = 0;

        currentTime = Time.time;

        if (startTime > currentTime)
        {
            startTime = currentTime;
        }

        if (currentTime - startTime >= timeBetweenFrame)
        {
            IsReallyRuning = true;
            curFrameID++;
            startTime = currentTime;

            if (curFrameID < sprites.Length)
            {
                spriteRenderer.sprite = sprites[curFrameID];
            }
            else
            {
                if (oneShoot)
                {
                    running = false;
                }
                else
                {
                    curFrameID = 0;
                    spriteRenderer.sprite = sprites[curFrameID];
                }
            }
        }
    }
}