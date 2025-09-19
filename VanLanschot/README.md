# VanLanschot Bond Processing Solution

This project provides tools for parsing, processing, and writing bond data using .NET 8 and C# 12. It includes a robust model for bonds, file handling utilities, and a comprehensive test suite.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (or later) recommended

## Getting Started

### 1. Clone the Repository

### 2. Build the Solution

Open the solution in Visual Studio and build, or use the command line:

### 3. Running the Application

The main entry point is in `VanLanschot/Program.cs`. You can run the application using:
dotnet run --project VanLanschot/VanLanschot.csproj

### 4. Running the Tests

Tests are located in the `VanLanschot.Tests` project and use [xUnit](https://xunit.net/).

### 5. Usage

- **Input File:** The application expects a semicolon-separated CSV file with the following headers:

BondID;Issuer;Rate;FaceValue;PaymentFrequency;Rating;Type;YearsToMaturity;DiscountFactor;DeskNotes

- **Output File:** The processed bonds will be written to a similar CSV file, including a `PresentValue` column.

### 6. Customization

- **Bond Parsing:** The `Bond` model automatically parses payment frequency and rate fields for you.
- **File Handling:** Use the `FileHandler` class to parse and write bond lists.

## Project Structure

- `VanLanschot/Model/Bond.cs` - Bond model and parsing logic
- `VanLanschot/FileHandler/FileHandler.cs` - File parsing and writing
- `VanLanschot/Model/Header.cs` - CSV header constants
- `VanLanschot.Tests/` - Unit tests
