namespace Code.ProtoVM
{
    public interface ISimpleNotifyPropertyChanged
    {
        /// <summary>
        /// The Action only version of the INotifyPropertyChanged interface, which normally has a PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e)
        /// </summary>
        public delegate void PropertyChanged();

    }
}