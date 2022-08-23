using CsvHelper.Configuration.Attributes;

namespace kommunerochskolverksamhet
{
    public class Skolverksamhet
    {
        [Index(0)]
        public int Kod { get; set; }
        [Index(1)]
        public string Skolenhetsnamn { get; set; }
        [Index(2)]
        public string GrundSkola { get; set; }
        [Index(3)]
        public string FörskoleKlass { get; set; }
        [Index(4)]
        public string FritidsHem { get; set; }
    }
}
