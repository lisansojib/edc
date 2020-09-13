namespace Presentation.Admin.Models
{
    public abstract class BaseViewModel
    {
        public virtual int Id { get; set; }

        public bool IsNew() => Id > 0;
    }
}
