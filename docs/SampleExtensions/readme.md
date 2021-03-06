# Sample Data Generator Sample Extensions #

This folder contains sample extensions to the SDG to help get you started for building your own extensions.

## Adding a sample extension to the SDG ##

Each sample is stored in such a way that you can copy / paste the entire folder structure into the *Application/EdFi.SampleDataGenerator.Core* folder to "install" the extension code.

For example, copy the contents of *SampleExtensions/Transportation/* to *Application/EdFi.SampleDataGenerator.Core*.  Do **NOT** copy the *Transportation* folder itself - only its contents.

If done properly, expect to be prompted to overwrite existing files -- specifically those with names ending in *Extensions.cs*.  If you don't get warnings about overwriting files, chances are you've copied the *Transportation* folder rather than its contents.

## Building the SDG

After you've copied the code files, you'll still need to take additional action before the solution will compile and produce output from your extension generators.

1. Add any new .cs files into the *EdFi.SampleDataGenerator.Core* project file.

You can do this manually by right-clicking the *EdFi.SampleDataGenerator.Core* project in Visual Studio Solution Explorer, choosing Add => Existing Items, and locating each new .cs file.  
Alternataively, Visual Studio Solution Explorer has a "Show All Files" button.  Once clicked, files in the filesystem that are not yet included in the project file are grayed out.  You can right-click these items and add to the project.

 
2. Re-generate the *EdFiEntities.cs* file

For this step, you **MUST** run from a Visual Studio Command prompt or otherwise have Xsd.exe available in your PATH.

Run the following command, from the *Ed-Fi-SDG\Application\EdFi.SampleDataGenerator.Core\Entities* directory:

> Ed-Fi-Ods-Xsd-Generator.bat EXTENSION-Ed-Fi-Ods-Xsd-Generator-v30.xsd

Note the new changes introduced into *Application\EdFi.SampleDataGenerator.Core\Entities\EdFiEntities.cs*


## Run the SDG ##

With the sample extension now installed, debug the solution.  Be sure to set command line arguments - for example:

> -c Samples\SampleDataGenerator\SampleConfig.xml -d Samples\SampleDataGenerator\DataFiles\ -o C:\Temp\SDG\Output\ -AllowOverwrite

Check the output files and confirm the output contains data generated by the extension generators.


## Troubleshooting ##

1. Solution won't compile

Ensure you've regenerated the *EdFiEntities.cs* file as explained above


2. SDG output doesn't contain data from extension generators

Ensure all new .cs files were added to the *EdFi.SampleDataGenerator.Core* project file

