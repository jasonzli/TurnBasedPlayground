using System.Collections;
using System.Collections.Generic;
using Code.ViewScripts;
using UnityEngine;

/// <summary>
/// A class to contain the UI Prefabs and everything we need. This is mostly just a service for the UI
/// </summary>
public class UIContainer : MonoBehaviour
{
    [SerializeField] public BattleOverlayPanelUIView _battleOverlayPanelUIView;
    [SerializeField] public BattleActorUIView _playerPanel;
    [SerializeField] public BattleActorUIView _enemyPanel;
    [SerializeField] public BattleActionSelectionPanelView _playerBattleActionSelectionPanelView;
    
    private Dictionary<string,IView> _views = new Dictionary<string, IView>();

    void OnEnable()
    {
        _views.Add("OverlayPanel",_battleOverlayPanelUIView);
        _views.Add("PlayerPanel",_playerPanel);
        _views.Add("EnemyPanel",_enemyPanel);
        _views.Add("PlayerActionPanel",_playerBattleActionSelectionPanelView);
    }
    
    public IView GetUI(string name)
    {
        _views.TryGetValue(name, out IView view);
        return view;
    }
}
