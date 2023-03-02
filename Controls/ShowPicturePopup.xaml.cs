using Mopups.Pages;

namespace IMP_reseni.Controls;

public partial class ShowPicturePopup
{
    public ShowPicturePopup(string url)
    {
        InitializeComponent();

        Image image = new Image
        {
            Source = ImageSource.FromFile(url),
            HeightRequest = 200,
            WidthRequest = 200          
        };
        MyGrid.Add(image,0,1);
    }
}