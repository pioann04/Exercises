using System.Text;
using System.Text.RegularExpressions;

internal class Module4Demos
{
    // clip 9
    public void AlbumDurationLINQChallenge()
    {
        var duration = "2:54,3:48,4:51,3:32,6:15,4:08,5:17,3:13,4:16,3:55,4:53,5:35,4:24"
            .Split(',')
            .Select(t => TimeSpan.Parse("0:" + t))
            .Aggregate((t1, t2) => t1 + t2);
        Console.WriteLine($"Duration {duration}");
    }

    // clip 10
    public void RangeExpansionLINQChallengePeriklis()
    {
        // Expand the range
        // e.g. "2,3-5,7" should expand to 2,3,4,5,7
        // "6,1-3,2-4" should expand to 1,2,3,4,6
        //"6,1-3,2-4"
        "2,3-5,7"
            .Split(",")
            .Select(s => s.Split("-"))
            .Select(s => new { First = int.Parse(s[0]), Last = int.Parse(s.Last()) })
            .SelectMany(s => Enumerable.Range(s.First, s.Last - s.First + 1))
            .Distinct()
            .Dump();
    }

    // clip 10
    public void RangeExpansionLINQChallenge()
    {
        // Expand the range
        // e.g. "2,3-5,7" should expand to 2,3,4,5,7
        // "6,1-3,2-4" should expand to 1,2,3,4,6
        "2,3-5,7"
            .Split(',')
            .Select(x => x.Split('-'))
            .Select(p => new { First = int.Parse(p[0]), Last = int.Parse(p.Last()) })
            .SelectMany(r => Enumerable.Range(r.First, r.Last - r.First + 1))
            .OrderBy(r => r)
            .Distinct()
            .Dump();
    }

    public void RangeExpansionLINQChallengeExpanded()
    {
        // Expand the range
        // e.g. "2,3-5,7" should expand to 2,3,4,5,7
        // "6,1-3,2-4" should expand to 1,2,3,4,6
        "6,1-3,2-4"
            .Split(',')
            .Select(x => x.Split('-'))
            .Select(p => new { First = int.Parse(p[0]), Last = int.Parse(p.Last()) })
            .SelectMany(r => Enumerable.Range(r.First, r.Last - r.First + 1))
            .OrderBy(r => r)
            .Distinct()
            .Dump();
    }

    public void CreateAndFillCSV()
    {
        for (int i = 0; i < 10; i++)
        {
            StringBuilder sb = new StringBuilder();

            // Add header
            sb.AppendLine("Id,Name,Age,Email,Search");

            // Add mock data
            for (int j = 1; j <= 100; j++)
            {
                sb.AppendLine($"{j},User{j},{20 + j % 30},user{j}@example.com,User Submitted{j}");
                sb.AppendLine($"User Submitted");
            }

            // Write to file
            var f = $"C:\\Mine\\Exercises\\C#\\Pluralsight\\linq-best-practices\\04\\demos\\4-unleashing-the-power-of-pipelines\\SkypeVoiceChanger\\file{i}.csv";
            File.WriteAllText(f, sb.ToString());
        }
    }

    // clip 11
    public void FindInFilesPeriklis()
    {
        var diagsFolder = Path.Combine("C:\\Mine\\Exercises\\C#\\Pluralsight\\linq-best-practices\\04\\demos\\4-unleashing-the-power-of-pipelines", "SkypeVoiceChanger");
        var fileType = "*.csv";
        var searchTerm = "User Submitted";
        Directory.EnumerateFiles(diagsFolder, fileType)
            .SelectMany(file => File.ReadAllLines(file)
                .Select((line, index) => new
                {
                    File = file,
                    Line = line,
                    Index = index
                }))
            .Where(line => string.Equals(line.Line, searchTerm))
            .Dump();
    }

    // clip 11
    public void FindInFiles()
    {
        var diagsFolder = Path.Combine("C:\\Mine\\Exercises\\C#\\Pluralsight\\linq-best-practices\\04\\demos\\4-unleashing-the-power-of-pipelines", "SkypeVoiceChanger");
        var fileType = "*.csv";
        var searchTerm = "User Submitted";
        Directory.EnumerateFiles(diagsFolder, fileType)
            .SelectMany(file => File.ReadAllLines(file)
                .Select((line, index) => new
                {
                    File = file,
                    Text = line,
                    LineNumber = index + 1
                })
            )
            .Where(line => Regex.IsMatch(line.Text, searchTerm))
            //.Take(10)
            .Dump();
    }

    // clip 12
    public void ParsingLogFiles()
    {
        var diagsFolder = Path.Combine("C:\\Mine\\Exercises\\C#\\Pluralsight\\linq-best-practices\\04\\demos\\4-unleashing-the-power-of-pipelines", "SkypeVoiceChanger");
        var fileType = "*.csv";
        Directory.EnumerateFiles(diagsFolder, fileType)
            .SelectMany(file => File.ReadAllLines(file)
                    .Skip(1)
                    .Select(line => line.Split(',')))
            .Where(parts => parts.Length > 1)
            .Where(parts => int.Parse(parts[2]) > 2)
            .Select(parts => new
            {
                Name = parts[1],
                Age = parts[2],
            })
            .Select(e => e)
            .Distinct()
            .Dump();
    }

    private enum Example
    {
        Apples = 1,
        Bananas = 2,
        Oranges = 3,
        Pears = 3
    }

    private enum Example2
    {
        Apples = 1,
        Bananas = 2,
        Oranges = 3,
    }

    // clip 13
    public void EnumDuplicateChecking()
    {
        Enum.GetNames(typeof(Example))
            .Select(n => new { Name = n, Number = (int)Enum.Parse<Example>(n) })
            .GroupBy(n => n.Number)
            .Where(g => g.Count() > 1)
            .Dump("Duplicates");

        Enum.GetNames(typeof(Example))
            .Except(Enum.GetNames(typeof(Example2)))
            .Dump("missing");
    }
}