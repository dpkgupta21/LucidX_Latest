using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using LucidX.Droid.Source.Activities;
using LucidX.Droid.Source.Models;
using System;
using Activity = Android.App.Activity;
namespace LucidX.Droid.Source.CustomDialogFragment
{

    public class UserListDialogFrag : DialogFragment
    {
        private View mView;
        private Activity mActivity;

        
        public static UserListDialogFrag NewInstance()
        {
            var fragment = new UserListDialogFrag();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mView = inflater.Inflate(Resource.Layout.dialog_add_order_fragment, container, false);

            mActivity = Activity;

            Dialog.SetTitle(Resource.String.select_calendar_type);
            Dialog.SetCancelable(false); //dismiss window on touch outside

            return mView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            Button btn_save = mView.FindViewById<Button>(Resource.Id.btn_save);
            btn_save.Click += Btn_save_Click;

            Button btn_cancel = mView.FindViewById<Button>(Resource.Id.btn_cancel);
            btn_cancel.Click += Btn_cancel_Click;

            // Set Adapter

        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                Dismiss();
            }
            catch (Exception ex)
            {

            }
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                OrderAddModel model = new OrderAddModel();
                model.ItemDescription = mView.FindViewById<EditText>(Resource.Id.edt_item_desc_val).Text;
             
                model.Amount = mView.FindViewById<EditText>(Resource.Id.edt_amount_val).Text;
                model.Vat = mView.FindViewById<EditText>(Resource.Id.edt_vat_val).Text;

                //((AddOrderSecondActivity)mActivity).Add(model);
                Dismiss();

               
            }
            catch (Exception ex)
            {

            }
        }


    }
}