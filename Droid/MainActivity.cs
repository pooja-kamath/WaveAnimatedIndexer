using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Animation;
using Android.Graphics;

namespace WaveIndex.Droid
{
    [Activity(Label = "WaveIndex", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private LinearLayout _layoutIndex;
        private FrameLayout _frameLayout;
        private ImageView _thumbImage;

        private TextView[] textViewArray;

        private int indexToAnimate = 0;
        private int indexToEndAnimate = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            _layoutIndex = FindViewById<LinearLayout>(Resource.Id.Layout_Cust_Index);
            _frameLayout = FindViewById<FrameLayout>(Resource.Id.Frame_layout);
            _thumbImage = FindViewById<ImageView>(Resource.Id.thumb);

            textViewArray = new TextView[26];

            AddIndexToList();
            _layoutIndex.Touch += TouchesInStrip;

        }


        private void TouchesInStrip(object sender, View.TouchEventArgs e)
        {
            float x = e.Event.GetY();
            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    {
                        for (int i = 0; i < 26; i++)
                        {
                            float y = textViewArray[i].GetY();
                            if (Math.Abs(y - x) <= 10)
                            {
                                indexToAnimate = i;
                                EndAnimation();
                                StartAnimation();
                                AnimateThumb(x);
                                indexToEndAnimate = indexToAnimate;
                            }
                        }
                    }
                    break;

                case MotionEventActions.Move:
                    {
                        x = e.Event.GetY();
                        for (int i = 0; i < 26; i++)
                        {
                            float y = textViewArray[i].GetY();
                            if (Math.Abs(y - x) <= 10)
                            {
                                indexToAnimate = i;
                                EndAnimation();
                                StartAnimation();
                                AnimateThumb(x);
                                indexToEndAnimate = indexToAnimate;
                                break;
                            }
                        }
                    }
                    break;

                case MotionEventActions.Up:
                    {
                        InitializeToStart();
                    }
                    break;
                case MotionEventActions.Cancel:
                    {
                        InitializeToStart();
                    }
                    break;
                case MotionEventActions.Outside:
                    {
                        InitializeToStart();
                    }
                    break;
                default:
                    break;
            }
        }

        private void AnimateOutward(int index, float value, int duration)
        {
            ObjectAnimator animation = ObjectAnimator.OfFloat(textViewArray[index], "translationX", value);
            animation.SetDuration(duration);
            animation.Start();
        }

        private void AnimateThumb(float y)
        {
            _thumbImage.Visibility = ViewStates.Visible;
            ObjectAnimator animation = ObjectAnimator.OfFloat(_thumbImage, "translationY", y );
            animation.SetDuration(10);
            animation.Start();
        }
        private void StartAnimation()
        {

            if (indexToAnimate >= 2)
                AnimateOutward(indexToAnimate - 2, -80f, 400);
            if (indexToAnimate >= 1)
                AnimateOutward(indexToAnimate - 1, -120f, 400);

            AnimateOutward(indexToAnimate, -160f, 400);

            if (indexToAnimate + 1 <= 24)
                AnimateOutward(indexToAnimate + 1, -120f, 400);
            if (indexToAnimate + 2 <= 24)
                AnimateOutward(indexToAnimate + 2, -80f, 400);
        }


        private void EndAnimation()
        {
            _thumbImage.Visibility = ViewStates.Gone;

            for (int i = 0; i < 26; i++)
            {
                if (i != indexToAnimate && i != indexToAnimate - 1 && i != indexToAnimate - 2 &&
                    i != indexToAnimate + 1 && i != indexToAnimate + 2)
                {
                    AnimateOutward(i, 0f, 200);
                }
            }
        }

        private void InitializeToStart()
        {

            for (int i = 0; i < 26; i++)
            {
                AnimateOutward(i, 0f, 0);
            }
        }

        private void AddIndexToList()
        {
            LinearLayout.LayoutParams lparams = new LinearLayout.LayoutParams(100, 0, 1);
            int i = 0;
            textViewArray = new TextView[26];
            for (char c = 'A'; c <= 'Z'; c++)
            {
                TextView btnindex = new TextView(this);
                btnindex.LayoutParameters = lparams;
                btnindex.SetPadding(10, 10, 40, 10);
                btnindex.Gravity = GravityFlags.Center;
                btnindex.TextSize = 12;
                btnindex.SetTextColor(Color.Black);
                btnindex.Text = c.ToString();
                _layoutIndex.AddView(btnindex);
                textViewArray[i] = btnindex;
                i++;
            }
            _layoutIndex.BringToFront();
        }


        private void ButtonIndexClick(object sender, EventArgs e)
        {

        }
    }
}

