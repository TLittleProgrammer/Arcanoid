using App.Scripts.External.Localisation;

namespace App.Scripts.Scenes.MainMenuScene.Command
{
    public class ChangeLocaleCommand : IChangeLocaleCommand
    {
        private readonly ILocaleService _localeService;

        public ChangeLocaleCommand(ILocaleService localeService)
        {
            _localeService = localeService;
        }
        
        public void Execute(string localeKey)
        {
            _localeService.SetLocaleKey(localeKey);
        }
    }
}