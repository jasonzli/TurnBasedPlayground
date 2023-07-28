

namespace Code.ViewModels
{
    /// <summary>
    /// The ability to set visibility is required for all viewmodels
    /// </summary>
    public interface IViewModel
    {
        public void SetVisibility(bool visibility);
    }
}