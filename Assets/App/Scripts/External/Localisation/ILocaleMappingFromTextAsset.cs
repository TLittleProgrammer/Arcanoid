namespace App.Scripts.External.Localisation
{
    public interface ILocaleMappingFromTextAsset
    {
        LocaleData GetLocaleMapping(string localeKey);
    }
}