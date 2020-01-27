public static class Paths
{
    public static FilePath SolutionFile => "src/CodeMania.sln";
    public static string ObjPattern => "src/**/[Oo]bj";
    public static string BinPattern => "src/**/[Bb]in";
    public static string TestResultsPattern => "**/[Tt]est[Rr]esults";
    public static string ArtifactsPattern => "**/[Aa]rtifacts";
    public static DirectoryPath ArtifactsDir => "artifacts";
    public static DirectoryPath CoverageDir => "coverage";
    public static DirectoryPath CoverageReportDir => "coverage/report";
    public static string CoveragePattern => "**/[Cc]overage";
}