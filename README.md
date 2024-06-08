# FileTypeReport

FileTypeReport is a console application written in C# that generates an HTML report of file types within a specified directory and its subdirectories. The report includes the count of files and the total size of files for each file type.

## Features

- Recursively enumerates all files in a directory and its subdirectories.
- Groups files by their extensions.
- Calculates the total size of files for each extension.
- Generates an HTML report that lists each file type, the count of files, and the total size in a human-readable format.

## Prerequisites

- .NET SDK installed on your machine.

## Usage

### Steps to Run

1. **Clone the Repository**

   ```sh
   git clone https://github.com/yourusername/FileTypeReport.git
   cd FileTypeReport

2. **Build the Project**

   Navigate to the new directory and build the project:

   ```sh
   cd FileTypeReport
   dotnet build

4. **Run the Application**

   Run the application with the required arguments: the input directory path and the output report file path.

   ```sh
   dotnet run "path/to/input/directory" "path/to/output/report.html"
