# WXRGenerator

 Basic WXR(WordPress Extended RSS) generator using CSV data

# Requirements

* .NET 6.0 Runtime [[Download](https://dotnet.microsoft.com/ko-kr/download/dotnet/6.0)]
  * Download not the Runtime but the SDK if you want to change the code and compile on your own.

# How to use

Usage: WXRGenerator [options...]

Options:

* -i, --input `<String>`: [Required] Filepath of the input CSV data
* -m, --metadata `<String>`: [Required] Filepath of CSV metadata
* -o, --output `<String>`: [Required] Filepath of the output XML file

Example:

`dotnet WXRGenerator.dll  -i "D:\temp\wp\input.csv" -m "D:temp\wp\metadata.csv" -o "D:\temp\wp\output.xml"`

After the program have successfully finished, you can upload the file in Import page in WordPress blog and it will work

# Required data

Please check `example/input.csv` and `example/metadata.csv` and prepare data files like them.
