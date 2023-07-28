namespace Code.ProtoVM
{
    
    /*
     * A personal interest point for me in this prototype was to explore making my own VM structure as I usually use
     * dependency injection frameworks that are prebuilt. I wanted to see what it would be like to start building my own.
     *
     * I'm not sure if this is the best way to do it, but this, for me, is the start: a observable value that can report
     * changes down a chain. Next would be the associated services to build and manage the various dependencies that exist
     *
     * To me, there remains a big question what "the model" is really meant to be in Unity's case. 
     * 
     * A lot of what is in here is from a mixture of Rider snippets and MicrosoftLearn and StackOverflow reading
     * but specifically big credit to this stack writeup: https://stackoverflow.com/questions/10324009/mvvm-modified-model-how-to-correctly-update-viewmodel-and-view
     *
     * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-7.0
     * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.propertychangedeventhandler?view=net-7.0
     * https://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface
     * https://softwareengineering.stackexchange.com/questions/135413/should-a-view-and-a-model-communicate-or-not
     * https://medium.com/android-news/mvvm-how-view-and-viewmodel-should-communicate-8a386ce1bb42
     * https://forums.swift.org/t/in-mvvm-is-it-ok-for-your-model-to-be-an-observableobject/60355
     */
    public interface ISimpleNotifyPropertyChanged
    {
        /// <summary>
        /// The Action only version of the INotifyPropertyChanged interface, which normally has a PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e)
        /// </summary>
        public delegate void PropertyChanged();

    }
}