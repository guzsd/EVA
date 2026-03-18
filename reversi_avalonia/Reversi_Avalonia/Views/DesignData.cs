using Reversi.Model;
using Reversi.Presistence;
using Reversi_Avalonia.ViewModels;

namespace Reversi_Avalonia.Views
{
    public static class DesignData
    {
        public static ReversiViewModel ViewModel
        {
            get
            {
                ReversiGameModel model = new(new ReversiFileDataAccess());
                model.NewGame();
                model.Pause();
                return new ReversiViewModel(model);
            }
        }
    }
}
