namespace App.Scripts.External.Localisation.Converters
{
    public interface IConverter
    {
        public string[,] ConvertFileToGrid(string text);
    }
}