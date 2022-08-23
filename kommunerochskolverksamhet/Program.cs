using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using static System.Console;

namespace kommunerochskolverksamhet
{
    class Program
    {
        public static void Main()
        {
            //Paths to all the input files and output file.
            string MainDirectory = Environment.CurrentDirectory;
            string ProjectDirectory = Directory.GetParent(MainDirectory).Parent.Parent.FullName;
            string KommunerPath = Path.Combine(ProjectDirectory, "kommuner.csv");
            string SkolverksamhetPath = Path.Combine(ProjectDirectory, "skolverksamhet.csv");
            string KommunOchSkolverksamhetPath = Path.Combine(ProjectDirectory, "KommunOchSkolverksamhet.csv");
            
            //Configure Csv Separation
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            //Integration of Swedish characters
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding srcEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1252);

            //Reading Kommuner.csv
            using (StreamReader reader = new StreamReader(KommunerPath, srcEncoding))
            using (CsvReader KommunCsv = new CsvReader(reader, config))
            {
                KommunCsv.Read();
                KommunCsv.ReadHeader();

                List<Kommuner> KommunerList = KommunCsv.GetRecords<Kommuner>().ToList();

                //Reading Skolverksamhet.csv
                using (StreamReader readSkolverksamhet = new StreamReader(SkolverksamhetPath, srcEncoding))
                using (CsvReader SkolverksamhetCsv = new CsvReader(readSkolverksamhet, config))
                {
                    SkolverksamhetCsv.Read();
                    SkolverksamhetCsv.ReadHeader();

                    List<Skolverksamhet> SkolverksamhetList = SkolverksamhetCsv.GetRecords<Skolverksamhet>().ToList();

                    //Merge Kommuner.csv and Skolversakmhet.csv
                    var result = from S in SkolverksamhetList
                                 join K in KommunerList on S.Kod equals K.Kod into X
                                 select new
                                 {
                                     S.Kod,
                                     Kommun = X.First().Kommun,
                                     S.Skolenhetsnamn,
                                     S.GrundSkola,
                                     S.FörskoleKlass,
                                     S.FritidsHem
                                 };
                    //Writing into a new .csv file with the name of KommunOchSkolverksamhet.csv
                    using (StreamWriter writer = new StreamWriter(KommunOchSkolverksamhetPath, false, srcEncoding))
                    using (CsvWriter WriteCSV = new CsvWriter(writer, config))
                    {
                        WriteCSV.WriteRecords(result);
                    }

                    //Confirmation on the console
                    WriteLine(">> Combination complete <<");
                    WriteLine("[File name]: KommunOchSkolverksamhet.csv");
                    WriteLine($"[Path]: {KommunOchSkolverksamhetPath}");
                    WriteLine("Press any key to exit");
                    ReadKey();
                }
            }
        }
        
    }
}