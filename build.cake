#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=ReportGenerator&version=4.3.3"
#tool "nuget:?package=GitVersion.CommandLine&version=5.1.1"

#addin "nuget:?package=Cake.Coverlet&version=2.3.4"

#load "build/paths.cake"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var reportTypes = Argument("reportTypes", "Html");
var solutionPath = GetFiles("./**/*.sln").First().ToString();
var packageVersion = "0.1.0";
var cleanupSettings = new DeleteDirectorySettings {
   Recursive = true,
   Force = true
};

// TASKS

Task("Clean")
   .Does(() => {
      if (DirectoryExists(Paths.CoverageDir))
      {
         DeleteDirectory(Paths.CoverageDir, cleanupSettings);
         Verbose("Removed coverage folder");
      }

      var binDirs = GetDirectories(Paths.BinPattern);
      if (binDirs.Count > 0)
      {
         DeleteDirectories(binDirs, cleanupSettings);
         Verbose("Removed {0} \"bin\" directories", binDirs.Count);
      }

      var objDirs = GetDirectories(Paths.ObjPattern);
      if (objDirs.Count > 0)
      {
         DeleteDirectories(objDirs, cleanupSettings);
         Verbose("Removed {0} \"obj\" directories", objDirs.Count);
      }

      var testResultsDirs = GetDirectories(Paths.TestResultsPattern);
      if (testResultsDirs.Count > 0)
      {
         DeleteDirectories(testResultsDirs, cleanupSettings);
         Verbose("Removed {0} \"TestResults\" directories", testResultsDirs.Count);
      }

      var artifactDir = GetDirectories(Paths.ArtifactsPattern);
      if (artifactDir.Count > 0)
      {
         DeleteDirectories(artifactDir, cleanupSettings);
         Verbose("Removed {0} artifact directories", artifactDir.Count);
      }
   });

Task("Version")
   .Does(() => {
      var version = GitVersion();
      Information($"Calculated semantic version: {version.SemVer}");

      packageVersion = version.NuGetVersionV2;
      Information($"Corresponding package version: {packageVersion}");
   });

Task("Restore")
   .IsDependentOn("Clean")
   .Does(() => {
      Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "true");
      DotNetCoreRestore("src");
   });

Task("Build")
   .IsDependentOn("Restore")
   .IsDependentOn("Version")
   .Does(() => {
      
      DotNetCoreBuild(
         solutionPath,
         new DotNetCoreBuildSettings
         { 
            Configuration = configuration,
            ArgumentCustomization = args => args.Append($"/p:Version={packageVersion}")
         });
   });

Task("Test")
   .Does(() => {
      EnsureDirectoryExists(Paths.CoverageDir);
      var testSettings = new DotNetCoreTestSettings {
         NoBuild = true,
         Configuration = configuration,
         ResultsDirectory = Directory("TestResults"),
         ArgumentCustomization = args => args.Append($"--logger trx")
      };
      var coverletSettings = new CoverletSettings {
         CollectCoverage = true,
         CoverletOutputDirectory = Paths.CoverageDir,
         CoverletOutputFormat = CoverletOutputFormat.opencover,
      };
      var testProjects = GetFiles("./**/*.UnitTests.csproj").ToList();
      foreach (var testProject in testProjects)
      {
         var testProjectName = testProject.GetFilenameWithoutExtension();

         coverletSettings.CoverletOutputName = $"{testProjectName}.opencover.xml";
         DotNetCoreTest(testProject.ToString(), testSettings, coverletSettings);
      }
   });

Task("Test:Coverage:Report")
   .Does(() => {
      var reportSettings = new ReportGeneratorSettings {
         ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
      };

      ReportGenerator("./coverage/*.xml", Paths.CoverageReportDir, reportSettings);
   });

Task("NuGet:Package")
   .IsDependentOn("Version")
   .Does(() => {
      EnsureDirectoryExists(Paths.ArtifactsDir);

      var projects = ParseSolution(solutionPath).Projects
      .Where(p => 
      p.GetType() != typeof(SolutionFolder) 
      && !p.Name.EndsWith("Tests") 
      && !p.Name.EndsWith("Benchmarks")
      && !p.Name.EndsWith("TestData"));

      foreach(var project in projects)
      {
         Information($"Packaging project {project.Name}...");
         DotNetCorePack(project.Path.ToString(), new DotNetCorePackSettings {
            Configuration = configuration,
            OutputDirectory = Directory("./artifacts/"),
            IncludeSymbols = true,
            ArgumentCustomization = args => args.Append($"/p:Version={packageVersion} /p:SymbolPackageFormat=snupkg")
         });
      }
   });

Task("NuGet:Publish")
   .Does(() => {
      var apiKey = EnvironmentVariable("NUGET_API_KEY");

      if(string.IsNullOrWhiteSpace(apiKey))
      {
         Error("Unable to find variable {0} on current environment", "NUGET_API_KEY");
      }

      var settings = new NuGetPushSettings {
         ApiKey = apiKey,
         Source = "https://api.nuget.org/v3/index.json",
         SkipDuplicate = true
      };

      var packagePath = GetFiles("./artifacts/*.nupkg").Single();

      NuGetPush(packagePath, settings);
   });

RunTarget(target);