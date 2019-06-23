namespace Application.DataAccessor
{
    public class Dog
    {
        public virtual int[] Select(params int[] ids)
        {
            return ids;
        }

        public virtual int[] Delete(params int[] ids)
        {
            return ids;
        }
    }
}
