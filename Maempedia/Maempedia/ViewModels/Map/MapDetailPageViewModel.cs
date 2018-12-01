namespace Maempedia.ViewModels.Map
{
    public class MapDetailPageViewModel : BaseViewModel
    {
        private Models.Owner selectedOwner;
        public Models.Owner SelectedOwner
        {
            get { return this.selectedOwner; }
            set { SetProperty<Models.Owner>(ref this.selectedOwner, value); }
        }

        public MapDetailPageViewModel(Models.Owner owner)
        {
            this.SelectedOwner = owner;
        }
    }
}
