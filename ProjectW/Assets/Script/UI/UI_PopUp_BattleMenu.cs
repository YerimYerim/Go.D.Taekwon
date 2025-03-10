using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using UnityEngine;

public class UI_PopUp_BattleMenu : UIBase
{
    // 스펠 소스
    [SerializeField] private UISpellSource spellSource;
    [SerializeField] private GameObject _spellSourceParent;
    private readonly List<UISpellSource> _uIspellSources = new();
    
    // 서포트 모듈
    [SerializeField] private UISupportModule _uiSupportModule;
    [SerializeField] private GameObject _uiSupportModuleParent;
    private readonly List<UISupportModule> _supportModules = new();
    
    [SerializeField] private UIApplication _uiApplication;
    [SerializeField] private GameObject _uiApplicationParent;
    private readonly List<UIApplication> _uiApplications = new();
    

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
            
            if(_uIspellSources.Count <= i)
            {
                var source =  Instantiate(spellSource.gameObject, _spellSourceParent.transform);
                _uIspellSources.Add( source.GetComponent<UISpellSource>());
            }
            
            _uIspellSources[index].gameObject.SetActive(true);
            _uIspellSources[index].SetImage(sources[index].GetSourceIconImage(), sources[index].GetSourceBgImage());
            _uIspellSources[index].SetOnClickButton(() => SetSelectedSource(index));
            _uIspellSources[index].SetHoverEvent(() =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var uiTooltip))
                {
                    uiTooltip.CreateInfo(sources[index].GetSourceName(), sources[index].GetSourceDesc(), _uIspellSources[index].rect );
                    uiTooltip.Show();
                }
            }, () =>
            {
                if (GameUIManager.Instance.TryGet<UITooltip>(out var uiTooltip))
                {
                    uiTooltip.Hide();
                }
            });
            _uIspellSources[index].SetSelected(false);  
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
                ui.SetImage(module[i].GetImageName());
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
                _supportModules[i].SetImage(module[i].GetImageName());
            }
        }
        _uiSupportModule.gameObject.SetActive(false);
    }
    private void SetSelectedSource(int index)
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var sourceID = gameBattleMode.BattleHandler._sources[index].GetSourceId();

        for (int i = 0; i < _uIspellSources.Count; i++)
        {
            _uIspellSources[i].SetSelected(false);
        }
        _uIspellSources[index].SetSelected(true);
        curSelectedSource = sourceID;
        SetSupportModule(GameSupportModuleManager.Instance.GetSupportModules(sourceID));
    }
    
    private void SetApplication(List<Relic> relic)
    {
        for (int i = 0; i < relic.Count; ++i)
        {
            var index = i;
            
            if(_uiApplications.Count <= i)
            {
                var relicObject =  Instantiate(_uiApplication.gameObject, _uiApplicationParent.transform);
                _uiApplications.Add( relicObject.GetComponent<UIApplication>());
            }
            
            _uiApplications[index].gameObject.SetActive(true);
            _uiApplications[index].SetImage(relic[index].GetImage());
            _uiApplications[index].SetOnClickButton(() => SetSelectedSource(index));
            _uiApplications[index].SetHoverEvent(() =>
            {
                if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var uiTooltip))
                {
                    uiTooltip.CreateInfo(relic[index].GetName(), relic[index].GetDesc(), _uiApplications[index].rect );
                    uiTooltip.Show();
                }
            }, () =>
            {
                if (GameUIManager.Instance.TryGet<UITooltip>(out var uiTooltip))
                {
                    uiTooltip.Hide();
                }
            });

        }
        _uiApplication.gameObject.SetActive(false);
    }
    
    public override void Show()
    {
        base.Show();
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        SetSpellSource(gameBattleMode.BattleHandler._sources);
        SetSelectedSource(0);
        SetApplication(gameBattleMode.BattleHandler.relics);
    }
}
