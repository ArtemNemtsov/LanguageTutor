namespace MediaStudioService.Core.Classes
{
    public class Responce
    {
        public bool Success { get; set; }
        public string ErrorsMessage { get; set; }
        public object Body { get; set; }
    }
}
