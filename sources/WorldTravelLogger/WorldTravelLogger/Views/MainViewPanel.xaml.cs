using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Utility;
using WorldTravelLogger.ViewModels;

namespace WorldTravelLogger.Views
{
    /// <summary>
    /// MainViewPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class MainViewPanel : UserControl
    {
        private OptionWindows optionWin_;
        private DebugWin debugWin_;

        public MainViewPanel()
        {
            InitializeComponent();
            var vm = new MainViewPanelVM();
            SideView.SetVM(vm.GetSideViewModel());
            UpperView.SetVM(vm.GetUpperViewModel());
            RouteViewPanel.SetVM(vm.GetRouteViewModel());
            AccommodationViewPanel.SetVM(vm.GetAccommodationViewModel());
            TransporationViewPanel.SetVM(vm.GetTransporationViewModel());
            SightseeingViewPanel.SetVM(vm.GetSightseeingViewModel());
            OtherViewPanel.SetVM(vm.GetOtherViewModel());
            this.DataContext = vm;
            
            vm.FileLoaded_ += Vm_FileLoaded_;
        }

        private void Vm_FileLoaded_(object? sender, FileLoadedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            { 
                if(e.ErrorTypes != ErrorTypes.None)
                {
                    var vm = (MainViewPanelVM)this.DataContext;
                    var filename = vm.GetFilename(e.Type);
                    string messege = string.Format("this system can't open {0} because {1}", filename, e.ErrorTypes);
                    MessageBox.Show(messege, "Errored Opening file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewPanelVM)this.DataContext;
           
            
            vm.Init();
        }

   
        private void OptionMenu_Click(object sender, RoutedEventArgs e)
        {
            if(optionWin_ == null)
            {
                var vm = (MainViewPanelVM)this.DataContext;
                optionWin_ = new OptionWindows(vm.GetOptionWindowViewModel());
                optionWin_.Closed += OptionWin__Closed;
                optionWin_.Show();
            }
            else
            {
                optionWin_.Activate();
            }
        }

        private void OptionWin__Closed(object? sender, EventArgs e)
        {
            optionWin_.Closed -= OptionWin__Closed;
            optionWin_ = null;
        }

        private void DebugMenu_Click(object sender, RoutedEventArgs e)
        {
            if(debugWin_ == null)
            {
                var vm = (MainViewPanelVM)this.DataContext;
                debugWin_ = new DebugWin(vm.GetDebugWinViewModel());
                debugWin_.Closed += DebugWin__Closed;
                debugWin_.Show();
            }
            else
            {
                debugWin_.Activate();
            }
        }

        private void DebugWin__Closed(object? sender, EventArgs e)
        {
            debugWin_.Closed -= DebugWin__Closed;
            debugWin_.Delete();
            debugWin_ = null;
        }

        public void Exit()
        {
            var vm = (MainViewPanelVM)this.DataContext;
            vm.Exit();
        }
     

    }
}
