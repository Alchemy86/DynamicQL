namespace DynamicQL.Core
{
    public abstract class DQElement
    {
        public static readonly string ROOT_NAME = "<ROOT>";

        public DQElementType ElementType { get; }
        public string Name { get; }

        protected DQElement(DQElementType type, string name)
        {
            ElementType = type;
            Name = name;
        }
    }
}
