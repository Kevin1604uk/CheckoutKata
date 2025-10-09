namespace checkoutkata
{
    public class Basket : IBasket
    {

        private readonly List<string> _items = new();
        private readonly ILogger _logger;

        public Basket(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddItem(string item)
        {
            if (string.IsNullOrEmpty(item) || item.Length != 1)
            {
                _logger.LogError(String.Format(MessageHelpers.ErrorInvalidItemScanned, item ?? ""));
                throw new ArgumentException(nameof(item));
            }
            _items.Add(item);
        }

        public void RemoveItem(string item)
        {
            if (string.IsNullOrEmpty(item) || item.Length != 1)
            {
                return; // Ignore invalid items silently, consistent with AddItem
            }
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index); // Remove one occurrence
            }
        }

        public Dictionary<string, int> GetItemCounts()
        {
            return _items
                .GroupBy(i => i.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

    }
}
