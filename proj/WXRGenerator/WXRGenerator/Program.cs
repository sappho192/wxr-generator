using CsvHelper;

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

		if (File.Exists(outputFilePath))
		{
		}
	}
}