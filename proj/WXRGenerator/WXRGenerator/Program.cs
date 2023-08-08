using CsvHelper;
using System.Globalization;
using WXRGenerator.Model;
using WXRGenerator.Model.WXR;

ConsoleApp.Run<MyCommands>(args);

public class MyCommands : ConsoleAppBase
{
	[RootCommand]
	public void RootCommand(
		[Option("i", "Filepath of the input CSV data")] string input,
		[Option("m", "Filepath of CSV metadata")] string metadata,
		[Option("o", "Filepath of the output XML file")] string output)
	{// -i "C:\BIN\input.csv" -m "C:\BIN\metadata.csv" -o "C:\BIN\output.xml"
		var arguments = Context.Arguments;
		Console.WriteLine($"Executing with arguments: \"{string.Join(" ", arguments)}\"");

		Console.WriteLine("Welcome to WXRGenerator");
		GenerateWXRFile(input, metadata, output);
	}

	private static void GenerateWXRFile(string inputFilePath, string metadataFilePath, string outputFilePath)
	{
		Console.WriteLine($"Reading from {inputFilePath} and {metadataFilePath}, result will be saved to {outputFilePath}");

		if (!File.Exists(metadataFilePath))
		{
			Console.WriteLine($"Metadata file {metadataFilePath} does not exist");
			return;
		}
		// Read metadata from metadataFilePath
		List<Metadata> metadata;
		metadata = readMetadataFile(metadataFilePath);
		Console.WriteLine(string.Join(",", metadata));

		Console.WriteLine();

		// Read data from inputFilePath
		List<BlogPost> blogPosts;
		blogPosts = readBlogPostsFile(inputFilePath);
		Console.WriteLine(string.Join(",", blogPosts));

		Console.WriteLine();

		// Generate WXR file
		var wxr = new WXR(metadata, blogPosts);
		var wxrXml = wxr.GenerateWXR();
		Console.WriteLine(wxrXml);
		Console.WriteLine();

		// Save WXR file to outputFilePath
		File.WriteAllText(outputFilePath, wxrXml);
		Console.WriteLine($"WXR file saved to {outputFilePath}");

		Console.WriteLine("Done");
	}

	private static List<Metadata> readMetadataFile(string metadataFilePath)
	{
		List<Metadata> metadata;
		using (var reader = new StreamReader(metadataFilePath))
		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			metadata = csv.GetRecords<Metadata>().ToList();
		}
		return metadata;
	}

	private static List<BlogPost> readBlogPostsFile(string  inputFilePath)
	{
		List<BlogPost> blogPosts;
		using (var reader = new StreamReader(inputFilePath))
		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			blogPosts = csv.GetRecords<BlogPost>().ToList();
		}
		return blogPosts;
	}
}