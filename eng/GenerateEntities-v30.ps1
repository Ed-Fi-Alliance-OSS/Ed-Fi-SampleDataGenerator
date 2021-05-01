# Must have xsd.exe in the command prompt. Install via MS Build Tools and/or Windows 10 SDK
# This script attempts to find it in either VS Build Tools 2019 or 2017
$env:PATH = "$env:PATH;C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools"
$env:PATH = "$env:PATH;C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools"

# Invoke the XSD to C# code generator
.\Ed-Fi-Ods-Xsd-Generator.ps1 Ed-Fi-Ods-Xsd-Generator-v30.xsd
