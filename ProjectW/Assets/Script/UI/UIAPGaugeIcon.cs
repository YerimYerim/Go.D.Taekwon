public class UIAPGaugeIcon : UIAPGaugeIconBase
{
    private GameSpellSource _gameSpellSource;
    
    public override int GetRemainSpellAP()
    {
        return _gameSpellSource.GetRemainAp();
    }
    
    public void SetSpellSource(GameSpellSource gameSpellSource)
    {
        _gameSpellSource = gameSpellSource;
        SetImage(gameSpellSource.GetSourceImage());
    }
}