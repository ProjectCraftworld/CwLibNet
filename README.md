# Craftworld Toolkit .NET

This is a recreation of Craftworld Toolkit in C# .NET code. It takes after [this repository](https://github.com/ennuo/toolkit/tree/cwlib), and is used for our LittleBIGPlanet 2 PC Port, Craftworld HUB. It can be used in anyone else's projects too. Thank you for your interest and continued support!
##### This project is still in production, any files made are subject to change.

## Installation

### .NET Runtime
```bash
# Install dotnet 
# For Windows
winget install Microsoft.DotNet.Runtime

# For macOS
brew install --cask dotnet

# For Linux (Ubuntu)
sudo apt-get update && sudo apt-get install -y dotnet-runtime-8.0
```
### Repository Prep
```bash
# Clone the repository
git clone https://github.com/yourusername/CwLibNet.git

# Navigate to the project directory
cd CwLibNet/CwLibNet

# Build the project
dotnet clean 
dotnet build
```

## Usage
If adding a new file from Craftworld Toolkit, please clean the build first and name your files relative to the rest.

## Contributing

Community contributions are welcome! Please open an issue or submit a pull request. It may take up to a week for someone to revise.

## How to Test Craftworld Toolkit .NET
[Demo Video](https://github.com/ProjectCraftworld/CwLibNet/blob/main/CwLibNetTests/test.mp4)

## License

This project is licensed under the MIT License besides from the Squish directory which is a translation of "jsqush" by acmi and it's BSD-3 Clause licensed. More information in the directory itself.