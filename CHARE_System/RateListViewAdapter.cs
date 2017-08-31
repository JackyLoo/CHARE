using Android.Content;
using Android.Views;
using Android.Widget;
using CHARE_System.JSON_Object;
using System.Collections.Generic;
using System.Linq;

namespace CHARE_System
{
    class RateListViewAdapter : BaseAdapter<RatingDetails>
    {
        private List<RatingDetails> ratings;
        private Context context;               

        public RateListViewAdapter(Context c, List<RatingDetails> ratings)
        {
            this.ratings = ratings;           
            context = c;        
        }

        public override RatingDetails this[int position]
        {
            get
            {
                return ratings[position];
            }
        }

        public override int Count
        {
            get
            {
                return ratings.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RateDetailsRow, parent, false);
                var name = view.FindViewById<TextView>(Resource.Id.name);
                var rate = view.FindViewById<RatingBar>(Resource.Id.rating);
                var comment = view.FindViewById<TextView>(Resource.Id.comment);                                

                view.Tag = new RatingViewHolder()
                {
                    Name = name,                    
                    Rate = rate,
                    Comment = comment
                };
            }
            var holder = (RatingViewHolder)view.Tag;            
            holder.Name.Text = ratings[position].Member1.username;
            holder.Comment.Text = ratings[position].comment;
            holder.Rate.NumStars = ratings[position].rate;            

            return view;
        }
    }

    public class RatingViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public RatingBar Rate { get; set; }
        public TextView Comment { get; set; }        
    }
}