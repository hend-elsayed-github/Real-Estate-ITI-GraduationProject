namespace Real_Estatae_Project.Models
{
    public class KnowledgeEntry
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public float[] Embedding { get; set; }
    }
}
