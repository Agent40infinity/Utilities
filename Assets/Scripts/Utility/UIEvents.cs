using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEvents : MonoBehaviour
{
    public GameObject previousSelected;
    public List<RectTransform> selectors = new List<RectTransform>();
    public float offset;

    public void Start()
    {
        Visibility(true);
        Selector(EventSystem.current.firstSelectedGameObject);
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
            case "Button":
                if (selected != previousSelected)
                {
                    RectTransform rect = selected.GetComponentInChildren<RectTransform>();
                    float rectPosition = rect.rect.width / 2 + offset;

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
        for (int i = 0; i < selectors.Count; i++)
        {
            selectors[i].gameObject.SetActive(active);
        }
    }
}
