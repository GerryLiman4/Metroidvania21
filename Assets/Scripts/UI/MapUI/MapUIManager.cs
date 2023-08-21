using System;
using System.Collections.Generic;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject characterIcon;
    [SerializeField] private Dictionary<MapId, MapSectionUI> mapSectionsUI = new Dictionary<MapId, MapSectionUI>();

    private MapSectionUI currentMapSectionUI;
    private bool isOpen = false;
    private void Awake()
    {
        GlobalEventSystem.OnChangeMap += OnChangeMap;
        GlobalEventSystem.OnOpenMap += SetActive;

        Initialize();
    }

    private void SetActive()
    {
        isOpen = !isOpen;
        mapPanel.SetActive(isOpen);
    }

    private void OnChangeMap(MapId mapId)
    {
        if (!mapSectionsUI.ContainsKey(mapId)) return;
        currentMapSectionUI = mapSectionsUI[mapId];

        currentMapSectionUI.MarkMapSection(characterIcon);
    }

    private void Initialize()
    {
        mapSectionsUI.Clear();
        MapSectionUI[] availableMapSectionsUI = GetComponentsInChildren<MapSectionUI>(includeInactive: true);

        foreach (MapSectionUI mapSectionUi in availableMapSectionsUI)
        {
            mapSectionsUI.Add(mapSectionUi.GetMapId(), mapSectionUi);
        }

        OnChangeMap(MapId.GreatHall01);
    }

    private void OnDestroy()
    {
        GlobalEventSystem.OnChangeMap -= OnChangeMap;
        GlobalEventSystem.OnOpenMap -= SetActive;
    }
}
