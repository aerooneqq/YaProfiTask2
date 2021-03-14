namespace YaProfiTask2.Controllers
{
    public class Note
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Content { get; set;  }


        public Note(string title, string content)
        {
            Id = GetNextId();
            Title = title;
            Content = content;
        }

        
        private static int currentId = 0;
        private static readonly object @lock = new object();
        private static int GetNextId()
        {
            lock (@lock)
            {
                ++currentId;
                return currentId;
            }
        }
    }
}