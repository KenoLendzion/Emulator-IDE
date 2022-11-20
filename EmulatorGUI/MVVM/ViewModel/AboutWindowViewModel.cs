using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace EmulatorGUI.MVVM.ViewModel
{
    public class AboutWindowViewModel
    {

        public RelayCommand HyperlinkRequestNavigateCommand { get; set; }

        public AboutWindowViewModel()
        {

            HyperlinkRequestNavigateCommand = new RelayCommand(OpenGithubRepositoryViaURL);
        }

        private void OpenGithubRepositoryViaURL()
        {
            string githubUri = "https://github.com/KenoLendzion/Emulator-IDE";

            var process = new ProcessStartInfo(githubUri)
            {
                UseShellExecute = true
            };

            Process.Start(process);
        }
    }
}
