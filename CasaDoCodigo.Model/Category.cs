namespace CasaDoCodigo.Model
{
    public class Category
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        public virtual Category Parent { get; set; }
    }
}