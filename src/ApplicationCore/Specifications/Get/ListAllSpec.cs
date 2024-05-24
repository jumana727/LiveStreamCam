namespace ApplicationCore.Specifications;

public class ListAllSpec<T> : Specification<T> where T : EntityBase<Guid>, IAggregateRoot
{
    public ListAllSpec()
    {
        Query.Where(x => true);
    }
}
