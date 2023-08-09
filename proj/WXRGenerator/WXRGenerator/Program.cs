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
		[Option("c", "Directory path of comments CSV data")] string comment,
		[Option("m", "Filepath of CSV metadata")] string metadata,
		[Option("o", "Filepath of the output XML file")] string output)
	{// -i "C:\BIN\input.csv" -m "C:\BIN\metadata.csv" -c "C:\BIN\comments" -o "C:\BIN\output.xml"
		var arguments = Context.Arguments;
		Console.WriteLine($"Executing with arguments: \"{string.Join(" ", arguments)}\"");

		Console.WriteLine("Welcome to WXRGenerator");
		GenerateWXRFile(input, comment, metadata, output);
	}

	private static void GenerateWXRFile(string inputFilePath, string commentDirPath, string metadataFilePath, string outputFilePath)
	{
		Console.WriteLine($"Reading from {inputFilePath}, {commentDirPath} and {metadataFilePath}, result will be saved to {outputFilePath}");

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

		if (!File.Exists(inputFilePath))
		{
			Console.WriteLine($"Input file {inputFilePath} does not exist");
			return;
		}
		if (!Directory.Exists(commentDirPath))
		{
			Console.WriteLine($"Comment directory {commentDirPath} does not exist. Comments data will be ignored.");
			commentDirPath = string.Empty;
		}
		// Read data from inputFilePath
		List<BlogPost> blogPosts;
		blogPosts = readBlogPostsFile(inputFilePath);
		Console.WriteLine(string.Join(",", blogPosts));

		Console.WriteLine();

		// Generate WXR file
		var wxr = new WXR(metadata, blogPosts, commentDirPath);
		wxr.GenerateWXR(outputFilePath);
		Console.WriteLine($"WXR file saved to {outputFilePath}");
		Console.WriteLine();

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

	private static List<BlogPost> readBlogPostsFile(string inputFilePath)
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