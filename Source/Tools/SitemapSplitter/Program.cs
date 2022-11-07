using CommandLine;
using SitemapSplitter;

internal class Program
{
    private static int Main(string[] args)
    {
        try
        {
            CommandLine.Parser.Default.ParseArguments<
                Splitter.Options>(args)
              .MapResult(
                (Splitter.Options o) => { Splitter.SplitSitemaps(o); return 0; },
                errors => 0);
        }
        catch (Exception e)
        {
            Console.Write(e.ToString());
            return 1;
        }

        return 0;
    }
}