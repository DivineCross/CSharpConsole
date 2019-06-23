namespace Application.DataAccessor
{
    public class Owner
    {
        public virtual int[] Select(params int[] ids)
        {
            ShowInfo("Really invoking 'Owner.Select'");

            return ids;
        }

        public virtual int? Update(int id, string value)
        {
            ShowInfo("Really invoking 'Owner.Update'");

            return 1;
        }

        public virtual int[] Delete(params int[] ids)
        {
            ShowInfo("Really invoking 'Owner.Delete'");

            return ids;
        }

        private void ShowInfo(string message) =>
            System.Console.WriteLine($"[RealObj] {message}");
    }
}
