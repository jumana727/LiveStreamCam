namespace ApplicationCore.Specifications;

public class OneByIdSpec<T> : Specification<T> where T: EntityBase<Guid>, IAggregateRoot
{
    public OneByIdSpec(Guid id)
    {
        Query.Where(element => element.Id == id);
    }
}
