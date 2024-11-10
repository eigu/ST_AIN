public class InventoryItem
{
    public WasteInfoSO wasteInfo;
    public int quantity;

    public InventoryItem(WasteInfoSO data,int qty)
    {
        wasteInfo = data;
        quantity = qty;
    }
}