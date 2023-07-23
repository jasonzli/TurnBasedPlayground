namespace ObservableTest
{
    public class ItemDataSource
    {
        public Observable<float> position;
        
        public ItemDataSource(float initialValue)
        {
            position = new Observable<float>(initialValue);
        }
    }
}