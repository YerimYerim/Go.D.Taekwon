using System;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using UnityEngine;

public class UI_PopUp_BattleMenu : UIBase
{
    // 스펠 소스
    [SerializeField] private UISpellSource spellSource;
    [SerializeField] private GameObject _spellSourceParent;
    private List<UISpellSource> UIspellSources = new();
    
    // 서포트 모듈
    [SerializeField] private UISupportModule _uiSupportModule;
    [SerializeField] private GameObject _uiSupportModuleParent;
    
    private List<UISupportModule> _supportModules = new();

    [SerializeField] private DTButton _btnClose;
    private int curSelectedSource = 0;

    private void Awake()
    {
        _btnClose.onClick.AddListener(Hide);
    }

    private void SetSpellSource(List<GameSpellSource> sources)
    {
        for (int i = 0; i < sources.Count; ++i)
        {
            var index = i;
            
            if(UIspellSources.Count <= i)
            {
                var source =  Instantiate(spellSource.gameObject, _spellSourceParent.transform);
                UIspellSources.Add( source.GetComponent<UISpellSource>());
            }
            
            UIspellSources[index].gameObject.SetActive(true);
            UIspellSources[index].SetImage(sources[index].GetSourceIconImage(), sources[index].GetSourceBgImage());
            UIspellSources[index].SetOnClickButton(() => SetSelectedSource(index));
            UIspellSources[index].SetHoverEvent(() =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var uiTooltip))
                {
                    uiTooltip.CreateInfo(sources[index].GetSourceName(), sources[index].GetSourceDesc(), UIspellSources[index].rect );
                    uiTooltip.Show();
                }
            }, () =>
            {
                if (GameUIManager.Instance.TryGet<UITooltip>(out var uiTooltip))
                {
                    uiTooltip.Hide();
                }
            });
            UIspellSources[index].SetSelected(false);  
        }
        spellSource.gameObject.SetActive(false);
    }
    
    private void SetSupportModule(List<SupportModule> module)
    {
        _supportModules.ForEach(_ => _.gameObject.SetActive(false));
        for (int i = 0; i < module.Count; i++)
        {
            if(_supportModules.Count <= i)
            {
                var supportModule = Instantiate(_uiSupportModule.gameObject, _uiSupportModuleParent.transform);
                UISupportModule ui = supportModule.GetComponent<UISupportModule>();
                ui.gameObject.SetActive(true);
                ui.SetImage(module[i].GetImageName(), "support_module_bg");
                var index = i;
                ui.SetHoverEvent(() =>
                {
                    if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var uiTooltip))
                    {
                        uiTooltip.CreateInfo(module[index].GetName(), module[index].GetDesc(), ui.rect );
                        uiTooltip.Show();
                    }
                }, () =>
                {
                    if (GameUIManager.Instance.TryGet<UITooltip>(out var uiTooltip))
                    {
                        uiTooltip.Hide();
                    }
                });
                _supportModules.Add(ui);
            }
            else
            {
                _supportModules[i].gameObject.SetActive(true);
                _supportModules[i].SetImage(module[i].GetImageName(), "support_module_bg");
            }
        }
        _uiSupportModule.gameObject.SetActive(false);
    }
    private void SetSelectedSource(int index)
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var sourceID = gameBattleMode.BattleHandler._sources[index].GetSourceId();
        
        
        for (int i = 0; i < UIspellSources.Count; i++)
        {
            UIspellSources[i].SetSelected(false);
        }
        UIspellSources[index].SetSelected(true);
        curSelectedSource = sourceID;
        SetSupportModule(GameSupportModuleManager.Instance.GetSupportModules(curSelectedSource));
    }
    public override void Show()
    {
        base.Show();
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        SetSpellSource(gameBattleMode.BattleHandler._sources);
        SetSelectedSource(0);
    }
}