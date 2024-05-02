using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FileTypeReport
{
    internal static class Program
    {
        // 1. Enumerate all files in a folder recursively
        // recursively enumerates all files in dir/sub dir
        // yield return to return each file path as it's encountered
        private static IEnumerable<string> EnumerateFilesRecursively(string directoryPath)
        {
            // TODO: Fill in your code here.
            // iterates through all files in the directory and its subdirectories
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                yield return file;
            }

            // recursively enumerate files in subdirectieries
            foreach (string subdirectory in Directory.GetDirectories(directoryPath))
            {
                foreach (string file in EnumerateFilesRecursively(subdirectory))
                {
                    yield return file; // yield each file path
                }
            }
        }

        // Human readable byte size
        // iterates through an array of sizes to determine suffix
        private static string FormatFileSize(long fileSize)
        {
            // converts file size to a human-readable format
            // 1. group files by their extensions
            // 2. calculate the total file size for each extension
            // 3. order the results by total file size in descending order
            string[] sizeSuffixes = { "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB" };
            int suffixIndex = 0; // init
            double size = fileSize;

            // determine appropriate suffix for file size
            while (size >= 1000 && suffixIndex < sizeSuffixes.Length - 1)
            {
                size /= 1000;
                suffixIndex++; // move to next suffix
            }

            return $"{size:N2} {sizeSuffixes[suffixIndex]}"; // returns formatted size with suffix
        }

        // Create an HTML report file
        private static XDocument GenerateReport(IEnumerable<string> fileList)
        {
            // 2. Process data
            var query =
                from file in fileList
                    // TODO: Fill in your code here.
                // let keyword!
                let extension = Path.GetExtension(file).ToLower() // gets the lowercase file extension
                group file by extension into fileGroup // groups files by their extension
                let totalSize = fileGroup.Sum(file => new FileInfo(file).Length) // calculates T size of files for each extension
                orderby totalSize descending // order extensions by total size in descending order
                select new
                {
                    FileType = fileGroup.Key, // file extension
                    FileCount = fileGroup.Count(),
                    TotalFileSize = totalSize
                };

            // 3. Functionally construct XML (HTML)
            var alignment = new XAttribute("align", "right");
            var style = "table, th, td { border: 1px solid black; }";

            // generate table rows for each file type with file count and total file size
            var tableRows = query.Select(item =>
                new XElement("tr",
                    new XElement("td", item.FileType),
                    new XElement("td", item.FileCount),
                    new XElement("td", FormatFileSize(item.TotalFileSize))
                )
            );

            var table = new XElement("table",
                new XElement("thead",
                    new XElement("tr",
                        new XElement("th", "File Type"),
                        new XElement("th", "File Count"),
                        new XElement("th", "Total File Size"))),
                new XElement("tbody", tableRows));

            return new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XElement("head",
                        new XElement("title", "File Report"),
                        new XElement("style", style)),
                    new XElement("body", table)));
        }

        // Console application with two arguments
        public static void Main(string[] args)
        {
            try
            {
                string inputDirectory = args[0]; // our input directory path
                string reportFile = args[1]; // our output report file path

                GenerateReport(EnumerateFilesRecursively(inputDirectory)).Save(reportFile);
                // recursively enumerate in inputDirectory
                // EnumerateFilesRecursively = collection of dirs/sub dirs
                // saves method on result of GenerateReport
            }
            catch
            {
                Console.WriteLine("Usage: FileTypeReport <directory> <reportFile>");
            }
        }
    }
}

/*
Steps to run in terminal:
- navigate to dir -- (cd Desktop)
- dotnet new console -o MyProgram -- (creates a new console application project)
- mv assignment3.cs MyProgram/Program.cs
- navigate to new dir -- (cd MyProgram)
- dotnet build
- dotnet run "directory" "/directory2/report.html"
*/
