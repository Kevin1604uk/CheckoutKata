namespace checkoutkata
{
    public interface IBasket
    {
        void AddItem(string item);
        void RemoveItem(string item);
        Dictionary<string, int> GetItemCounts();
    }
}
