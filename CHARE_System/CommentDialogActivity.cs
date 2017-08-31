
using Android.App;
using Android.OS;

namespace CHARE_System
{
    [Activity(Label = "Rate")]
    public class CommentDialogActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Custom_Dialog_Rating);
        }
    }
}