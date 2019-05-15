
using SeatMaker.Interface;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SeatMaker.Views
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public MainPage(INativeService nativeService)
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            var navigationPage = new NavigationPage(new MembersPage());
            navigationPage.Icon = "people.png";
            navigationPage.Title = "全員";
            Children.Add(navigationPage);

            navigationPage = new NavigationPage(new ParticipantsPage());
            navigationPage.Icon = "list.png";
            navigationPage.Title = "参加者";
            Children.Add(navigationPage);

            navigationPage = new NavigationPage(new CreateSeatPage(nativeService));
            navigationPage.Icon = "table.png";
            navigationPage.Title = "席作成";
            Children.Add(navigationPage);
        }
    }
}