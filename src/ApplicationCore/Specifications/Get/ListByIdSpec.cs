namespace ApplicationCore.Specifications;

public class ListByIdSpec<T> : Specification<T> where T : EntityBase<Guid>, IAggregateRoot
{
    public ListByIdSpec(List<Guid> Ids)
    {
        Query.Where(element => Ids.Contains(element.Id));
    }
}
