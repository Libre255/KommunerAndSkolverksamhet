using CsvHelper.Configuration.Attributes;

namespace kommunerochskolverksamhet
{
    public class Kommuner
    {
        [Index(0)]
        public int Kod { get; set; }
        [Index(1)]
        public string Kommun { get; set; }
    }
}
