public class UIAPGaugeIcon : UIAPGaugeIconBase
{
    private GameSpellSource _gameSpellSource;
    
    public override int GetRemainSpellAP()
    {
        return _gameSpellSource.GetRemainAp();
    }

    public override int GetResetAp()
    {
        return _gameSpellSource.GetMaxAp();
    }

    public void SetSpellSource(GameSpellSource gameSpellSource)
    {
        if(_gameSpellSource != null)
        {
            IsShow = true;
        }
        else
        {
            IsShow = false;
        }
        
        _gameSpellSource = gameSpellSource;
        SetImage(gameSpellSource.GetSourceImage());

    }
}
