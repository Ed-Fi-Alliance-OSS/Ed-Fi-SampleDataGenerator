using log4net.Config;

namespace EdFi.InterchangeXmlToCsv.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            PrintCopyrightMessageToConsole();

            BasicConfigurator.Configure();

            var parser = new CommandLineParser();
            var parseResult = parser.Parse(args);

            if (parseResult.HasErrors)
            {
                System.Console.WriteLine(parseResult.ErrorText);
                return -1;
            }

            var converter = new InterchangeXmlToCsvConverter();
            converter.Convert(parser.Object);

#if DEBUG
            System.Console.Write("Press any key to continue...");
            System.Console.ReadKey();
#endif

            return 0;
        }

        private static void PrintCopyrightMessageToConsole()
        {
            const string copyrightText =
                "\r\n" +
                "Sample Data Generator is Copyright \u00a9 2018 Ed-Fi Alliance, LLC\r\n" +
                "License info available at https://techdocs.ed-fi.org/display/SDG/Licensing \r\n";

            //Set encoding to UTF8 so copyright symbol in the above message prints correctly
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.WriteLine(copyrightText);
        }
    }
}
