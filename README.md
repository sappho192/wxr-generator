# WXRGenerator

 Basic WXR(WordPress Extended RSS) generator using CSV data

# Requirements

* .NET 6.0 Runtime [[Download](https://dotnet.microsoft.com/ko-kr/download/dotnet/6.0)]
  * Download not the Runtime but the SDK if you want to change the code and compile on your own.

# How to use

Usage: WXRGenerator [options...]

Options:

* -i, --input `<String>`: [Required] Filepath of the input CSV data
* -c, --comment `<String>`: [Required] Directory path of comments CSV data
* -m, --metadata `<String>`: [Required] Filepath of CSV metadata
* -o, --output `<String>`: [Required] Filepath of the output XML file

Example:

`dotnet WXRGenerator.dll  -i "D:\temp\wp\input.csv" -c "D:\temp\wp\comments" -m "D:temp\wp\metadata.csv" -o "D:\temp\wp\output.xml"`

After the program have successfully finished, you can upload the file in Import page in WordPress blog and it will work

# Required data

Please check `example/input.csv`, `example/metadata.csv`, and `example/comments` to prepare data files like them.

Filename of comment corresponds to `uniqueId` of input post data.

For example, if `uniqueId` of a post data is `2`, then corresponding comment CSV data should be prepared in `comments/2.csv`.
