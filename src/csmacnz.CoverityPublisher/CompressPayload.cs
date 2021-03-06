namespace csmacnz.CoverityPublisher
{
    public sealed class CompressPayload
    {
        public string Output { get; set; }
        public string Directory { get; set; }
        public bool ProduceZipFile { get; set; }
        public bool AbortOnFailures { get; set; }
        public bool OverwriteExistingFile { get; set; }
    }
}