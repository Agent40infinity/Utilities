﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public GameObject previousSelected;
    public List<RectTransform> selectors = new List<RectTransform>();
    public float offset;
    public RectTransform parent;

    public void Start()
    {
        Visibility(true);
    }

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Selector(EventSystem.current.currentSelectedGameObject);
        }
    }

    public void Selector(GameObject selected)
    {
        switch (selected.tag)
        {
            case "Selectable":
                if (selected != previousSelected)
                {
                    RectTransform rect = selected.GetComponentInChildren<RectTransform>();
                    float scale = parent.localScale.x;

                    float rectPosition = (rect.rect.width / 2 + offset) * scale;

                    Visibility(true);

                    selectors[0].position = new Vector2(rect.position.x + rectPosition, rect.position.y);
                    selectors[1].position = new Vector2(rect.position.x - rectPosition, rect.position.y);

                    previousSelected = selected;
                }
                break;
        }
    }

    public void Visibility(bool active)
    {
        switch (EventSystem.current.currentSelectedGameObject != null)
        {
            case true:
                for (int i = 0; i < selectors.Count; i++)
                {
                    selectors[i].gameObject.SetActive(active);
                }
                break;
        }
    }
}
