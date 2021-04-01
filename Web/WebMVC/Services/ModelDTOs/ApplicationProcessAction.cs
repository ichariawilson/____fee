namespace WebMVC.Services.ModelDTOs
{
    public record ApplicationProcessAction
    {
        public string Code { get; }
        public string Name { get; }

        public static ApplicationProcessAction Grant = new ApplicationProcessAction(nameof(Grant).ToLowerInvariant(), "Grant");

        protected ApplicationProcessAction()
        {
        }

        public ApplicationProcessAction(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
